using LY.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace LY.AutoStart
{
    public enum ImageType
    {
        DockerHubImage,
        Dotnet,
        Vue
    }

    class Program
    {
        static DirectoryInfo workspace = null;
        static string workspaceDir = string.Empty;
        static StringBuilder sbDockerCmd = new StringBuilder();

        const string netcoreappVersion = "netcoreapp2.2";
        const int serviceNum = 2; //服务数量
        const int consulAgentNum = 4; //consul集群数量
        const bool isPublishToLinux = false;

        static void Main(string[] args)
        {
            //Console.WriteLine((DateTime.Parse("2019-03-04") == DateTime.Today));
            //Console.Read();
            //return;
            Stopwatch sp = new Stopwatch();
            sp.Start();
            Start(ref args);
            sp.Stop();
            Console.WriteLine(sp.Elapsed.TotalSeconds);
            Console.Read();
        }

        private static void Start(ref string[] args)
        {
            try
            {
#if DEBUG
                //args = new string[] { "practice", "base" };
                //args = new string[] { "practice", "services,gateway,daemon,vue" };
                args = new string[] { "practice", "base" };
#endif
                if (args == null || args.Length == 0)
                {
                    Console.WriteLine("请输入workspace名称");
                    return;
                }

                if (args.Length == 1 || string.IsNullOrEmpty(args[1]))
                {
                    Console.WriteLine("请输入发布选项【services,vue,gateway,daemon,base】，多选用逗号隔开，全选使用all");
                    return;
                }
                workspace = new DirectoryInfo(Directory.GetCurrentDirectory());
                var root = workspace.Root;
                while (true)
                {
                    if (workspace.FullName == root.FullName)
                    {
                        Console.WriteLine($"无法找到workspace{args[0]}");
                        return;
                    }
                    if (workspace.Name == args[0])
                    {
                        break;
                    }
                    workspace = workspace.Parent;
                }
                workspaceDir = workspace.FullName;

                var options = args[1].Split(",").Distinct();
                if (!options.Intersect(new string[] { "base" }).IsNullOrEmpty())//默认不发布基础设施
                {
                    DeployBase();
                }
                if (!options.Intersect(new string[] { "gateway", "all" }).IsNullOrEmpty())
                {
                    DeployGateway();
                }
                if (!options.Intersect(new string[] { "daemon", "all" }).IsNullOrEmpty())
                {
                    DeployDaemon();
                }
                if (!options.Intersect(new string[] { "services", "all" }).IsNullOrEmpty())
                {
                    DeployServices();
                }
                if (!options.Intersect(new string[] { "vue", "all" }).IsNullOrEmpty())
                {
                    DeployVue();
                }

                string fileName = isPublishToLinux ? "deploy.sh" : "deploy.bat";
                using (FileStream fs = File.Create(Path.Combine(workspaceDir, "PublishToProduct", fileName)))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(sbDockerCmd.ToString());
                    sw.Flush();
                }
                if (!isPublishToLinux)
                {
                    ExcuteBat(() => { return sbDockerCmd; });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #region common
        private static void ExcuteBat(Func<StringBuilder> func)
        {
            var sb = func();
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.RedirectStandardInput = true;
                process.Start();
                sb.AppendLine("exit");
                process.StandardInput.WriteLine(sb.ToString());
                process.WaitForExit();
            }
        }

        private static void BuildApp(string name, ImageType type)
        {
            var lowName = name;
            switch (type)
            {
                case ImageType.Dotnet:
                    ExcuteBat(() =>
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"cd {Path.Combine(workspaceDir, name)}");
                        //sb.AppendLine($"dotnet restore");
                        sb.AppendLine($"dotnet build");
                        sb.AppendLine($"dotnet publish -c Release");
                        return sb;
                    });
                    break;
                case ImageType.Vue:
                    ExcuteBat(() =>
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"cd {Path.Combine(workspaceDir, name)}");
                        sb.AppendLine($"npm install");
                        return sb;
                    });
                    ExcuteBat(() =>
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"cd {Path.Combine(workspaceDir, name)}");
                        sb.AppendLine($"npm run build");
                        return sb;
                    });
                    break;
                default:
                    break;
            }

        }

        private static void CreateDockerfile(string name, ImageType type)
        {
            switch (type)
            {
                case ImageType.Dotnet:
                    using (FileStream fs = File.Create(Path.Combine(workspaceDir, name, "bin", "Release", netcoreappVersion, "publish", "Dockerfile")))
                    {
                        StreamWriter sw = new StreamWriter(fs);
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("FROM microsoft/dotnet:2.2-aspnetcore-runtime");
                        sb.AppendLine("WORKDIR /app");
                        //sb.AppendLine($"ENV ASPNETCORE_URLS http://+:{port}");
                        sb.AppendLine("COPY . .");
                        sb.AppendLine($"ENTRYPOINT [\"dotnet\", \"{name}.dll\"]");
                        sw.WriteLine(sb.ToString());
                        sw.Flush();
                    }
                    break;
                case ImageType.Vue:
                    using (FileStream fs = File.Create(Path.Combine(workspaceDir, name, "dist", "Dockerfile")))
                    {
                        StreamWriter sw = new StreamWriter(fs);
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("FROM nginx");
                        sb.AppendLine("COPY . /usr/share/nginx/html/");
                        sw.WriteLine(sb.ToString());
                        sw.Flush();
                    }
                    break;
                default:
                    break;
            }
        }

        private static void BuildImage(string name, ImageType type, string version = null)
        {
            var lowName = name.ToLower();

            var imageVersion = string.IsNullOrEmpty(version) ? string.Empty : ":" + version;

            var sb = new StringBuilder();
            if (type == ImageType.Dotnet && !name.EndsWith("Gateway") && !name.EndsWith("Daemon"))
            {
                for (int i = 0; i < serviceNum; i++)
                {
                    var index = (i == 0 ? string.Empty : i.ToString());
                    sb.AppendLine($"docker stop {lowName}-server{index}");
                    sb.AppendLine($"docker rm {lowName}-server{index}");
                }
            }
            else if (lowName == "consul")
            {
                for (int i = 0; i < consulAgentNum; i++)
                {
                    var index = (i == 0 ? string.Empty : i.ToString());
                    sb.AppendLine($"docker stop {lowName}-server{index}");
                    sb.AppendLine($"docker rm {lowName}-server{index}");
                }
            }
            else
            {
                sb.AppendLine($"docker stop {lowName}-server");
                sb.AppendLine($"docker rm {lowName}-server");
            }

            sb.AppendLine($"docker rmi {lowName}{imageVersion}");
            string sourceDir = null;
            var targetDir = Path.Combine(workspaceDir, "PublishToProduct", name + "\\");
            if (type == ImageType.DockerHubImage)
            {
                sb.AppendLine($"docker pull {lowName}{imageVersion}");
            }
            else
            {
                switch (type)
                {
                    case ImageType.Dotnet:
                        sourceDir = Path.Combine(workspaceDir, name, "bin", "Release", netcoreappVersion, "publish");
                        break;
                    case ImageType.Vue:
                        sourceDir = Path.Combine(workspaceDir, name, "dist");
                        break;
                    default:
                        break;
                }
            }

            
            if (!sourceDir.IsNullOrEmpty())
            {
                sb.AppendLine($"xcopy /Y /S  {sourceDir}  {targetDir}");
                sb.AppendLine($"cd {targetDir}");
                sb.AppendLine($"docker build -t {lowName} .");
            }

            string result = sb.ToString();
            if (isPublishToLinux)
            {
                result = result.Replace(sourceDir, "/root/PublishToProduct/" + name);
            }
            sbDockerCmd.AppendLine(result);
        }

        private static void CreateContainer(string name, string ip = null, string port = null, int? index = null, string version = null, string postfix = null)
        {
            var lowName = name.ToLower();
            var imageVersion = string.IsNullOrEmpty(version) ? string.Empty : ":" + version;

            var strIndex = ((index.HasValue && index > 0) ? index.ToString() : string.Empty);
            var sb = new StringBuilder();
            sb.AppendLine($"docker run -d {port} --network=lynet {ip} --name={lowName}-server{strIndex} {lowName}{imageVersion} {postfix}");
            sbDockerCmd.AppendLine(sb.ToString());
        }
        #endregion

        private static void DeployGateway()
        {
            DirectoryInfo target = workspace.GetDirectories().FirstOrDefault(x => x.Name.EndsWith("Gateway"));
            if (target != null)
            {
                var name = target.Name;
                BuildApp(name, ImageType.Dotnet);
                CreateDockerfile(name, ImageType.Dotnet);
                BuildImage(name, ImageType.Dotnet);
                CreateContainer(name, $"--ip={Const.IP._gateway}", "-p 9000:9000");
            }
        }

        private static void DeployDaemon()
        {
            DirectoryInfo target = workspace.GetDirectories().FirstOrDefault(x => x.Name.EndsWith("Daemon"));
            if (target != null)
            {
                var name = target.Name;
                BuildApp(name, ImageType.Dotnet);
                CreateDockerfile(name, ImageType.Dotnet);
                BuildImage(name, ImageType.Dotnet);
                CreateContainer(name, $"--ip={Const.IP._daemon}", "-p 9009:9009");
            }
        }

        private static void DeployServices()
        {
            IEnumerable<DirectoryInfo> targets = workspace.GetDirectories().Where(x =>
            x.Name.EndsWith("Service")
            );

            //dotnet build
            foreach (var dic in targets)
            {
                BuildApp(dic.Name, ImageType.Dotnet);
            }

            foreach (var item in targets)
            {
                CreateDockerfile(item.Name, ImageType.Dotnet);
                BuildImage(item.Name, ImageType.Dotnet);
            }

            foreach (var service in targets)
            {
                for (int i = 0; i < serviceNum; i++)
                {
                    CreateContainer(service.Name, null, null, i);
                }
            }
        }

        private static void DeployVue()
        {
            IEnumerable<DirectoryInfo> targets = workspace.GetDirectories().Where(x => x.Name.StartsWith("ly.vue"));

            foreach (var dic in targets)
            {
                BuildApp(dic.Name, ImageType.Vue);
            }

            foreach (var item in targets)
            {
                CreateDockerfile(item.Name, ImageType.Vue);
                BuildImage(item.Name, ImageType.Vue);
            }

            var t1 = targets.FirstOrDefault(x => x.Name == "ly.vue.pc");
            if (t1 != null)
            {
                CreateContainer(t1.Name, null, "-p 8081:80");
            }
            var t2 = targets.FirstOrDefault(x => x.Name == "ly.vue.mobile");
            if (t2 != null)
            {
                CreateContainer(t2.Name, null, "-p 8082:80");
            }
        }

        /// <summary>
        /// 第一次部署执行
        /// </summary>
        private static void DeployBase()
        {
            sbDockerCmd.AppendLine($"docker network create --subnet=172.19.0.0/16 lynet");

            #region mysql master and slave

            //docker run --network=lynet --ip=172.19.200.1 -itd --name=mysql-master -p 3306:3306 -e MYSQL_ROOT_PASSWORD=123456 -d mysql
            //docker run --network=lynet --ip=172.19.200.2 -itd --name=mysql-slave -p 3307:3306 -e MYSQL_ROOT_PASSWORD=123456 -d mysql

            //CREATE USER 'slave'@'%' IDENTIFIED BY '123456';
            //GRANT REPLICATION SLAVE ON *.* TO 'slave'@'%';

            //修改user的plugin 'caching_sha2_password'-->'mysql_native_password'
            //docker exec -it  mysql-master /bin/bash
            //mysql -u root -p123456
            //alter user 'root'@'%' identified by '123456' password expire never;
            //alter user 'root'@'%' identified with mysql_native_password by '123456';
            //flush privileges;

            //alter user 'slave'@'%' identified by '123456' password expire never;
            //alter user 'slave'@'%' identified with mysql_native_password by '123456';
            //flush privileges;

            //设置docker的配置文件
            //log_bin=mysql-bin
            //server_id = 128 //从:129
            //docker cp mysql-master:/etc/mysql/my.cnf C:\Users\liuyu\Desktop\mysql
            //docker cp  C:\Users\liuyu\Desktop\mysql\my.cnf mysql-master:/etc/mysql/my.cnf
            //docker cp  C:\Users\liuyu\Desktop\mysql\my.cnf mysql-slave:/etc/mysql/my.cnf

            //开启从库的slave
            //mysql -u root -p123456
            /*
            STOP SLAVE;
            CHANGE MASTER TO
            MASTER_HOST = '172.19.200.1',
            MASTER_USER = 'root',
            MASTER_PASSWORD = '123456',
            MASTER_LOG_FILE = 'mysql-bin.000001',
            MASTER_LOG_POS = 155;
            START SLAVE;
            */
            //START SLAVE;
            //SHOW SLAVE STATUS \G  


            //Slave_SQL_Running: No
            //1.程序可能在slave上进行了写操作

            //2.也可能是slave机器重起后，事务回滚造成的.

            //一般是事务回滚造成的：
            //解决办法：
            /*
            stop slave;
            set GLOBAL SQL_SLAVE_SKIP_COUNTER = 1;
            start slave; 
             */

            #endregion
            /*

            BuildImage("redis", ImageType.DockerHubImage);
            CreateContainer("redis", $"--ip={Const.IP._redis}", "-p 6379:6379");

            BuildImage("consul", ImageType.DockerHubImage);
            //consul cluster
            CreateContainer("consul", $"--ip=172.19.202.1", null, null, null, "agent -server -bootstrap-expect=2");
            CreateContainer("consul", $"--ip=172.19.202.2", null, 1, null, "agent -server -join 172.19.202.1");
            CreateContainer("consul", $"--ip=172.19.202.3", null, 2, null, "agent -server -join 172.19.202.1");
            CreateContainer("consul", $"--ip={Const.IP._consul}", "-p 8500:8500", 3, null, "agent -join 172.19.202.1 -client=\"0.0.0.0\" -ui");

            BuildImage("rabbitmq", ImageType.DockerHubImage);
            CreateContainer("rabbitmq", $"--ip={Const.IP._rabbitmq}", "-p 15672:15672 -p 5672:5672");

            BuildImage("elasticsearch", ImageType.DockerHubImage, "6.6.1");
            CreateContainer("elasticsearch", $"--ip={Const.IP._elasticsearch}", "-p 9200:9200 -p 9300:9300 -e \"discovery.type = single - node\"", null, "6.6.1");

            BuildImage("kibana", ImageType.DockerHubImage, "6.6.1");
            //docker cp kibana-server:/usr/share/kibana/config/kibana.yml C:\Users\liuyu\Desktop\mysql
            //docker cp C:\Users\liuyu\Desktop\mysql\kibana.yml kibana-server:/usr/share/kibana/config/kibana.yml
            CreateContainer("kibana", $"--ip={Const.IP._kibana}", $"-p 5601:5601 -e \"elasticsearch.hosts = http://{Const.IP._elasticsearch}:9200\"", null, "6.6.1");

            */

            sbDockerCmd.AppendLine("docker start mysql-master");
            sbDockerCmd.AppendLine("docker start mysql-slave");
            sbDockerCmd.AppendLine("docker start redis-server");
            sbDockerCmd.AppendLine("docker start elasticsearch-server");
            sbDockerCmd.AppendLine("docker start kibana-server");
            sbDockerCmd.AppendLine("docker start rabbitmq-server");
            sbDockerCmd.AppendLine("docker start consul-server");
            sbDockerCmd.AppendLine("docker start consul-server1");
            sbDockerCmd.AppendLine("docker start consul-server2");
            sbDockerCmd.AppendLine("docker start consul-server3");
        }
    }
}
