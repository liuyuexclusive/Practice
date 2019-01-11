using LY.Common;
using Microsoft.Extensions.Configuration;
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
        static string workspaceDir = string.Empty;
        static DirectoryInfo workspace = null;
        static string netcoreappVersion = "netcoreapp2.2";
        static int serviceNum = 2;
        static bool isPublishToProduct = false;
        static StringBuilder sbSH = new StringBuilder();        

        static void Main(string[] args)
        {
            Start(ref args);
        }
        private static void Start(ref string[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                {
                    Console.WriteLine("请输入workspace名称");
                    return;
                }

                if (args == null || args.Length == 1 || string.IsNullOrEmpty(args[1]))
                {
                    Console.WriteLine("请输入发布选项services/vue/gateway/daemon/base，多选用逗号隔开，全选使用all");
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

                if (!options.Intersect(new string[] { "services", "all" }).IsNullOrEmpty())
                {
                    DeployServices();
                }
                if (!options.Intersect(new string[] { "vue", "all" }).IsNullOrEmpty())
                {
                    DeployVue();
                }
                if (!options.Intersect(new string[] { "base", "all" }).IsNullOrEmpty())
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

                if (isPublishToProduct)
                {
                    using (FileStream fs = File.Create(Path.Combine(workspaceDir, "PublishToProduct", "deploy.sh")))
                    {
                        StreamWriter sw = new StreamWriter(fs);
                        sw.WriteLine(sbSH.ToString());
                        sw.Flush();
                    }
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

        private static void BuildImage(string name, ImageType type)
        {
            var lowName = name.ToLower();
            ExcuteBat(() =>
            {
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
                else
                {
                    sb.AppendLine($"docker stop {lowName}-server");
                    sb.AppendLine($"docker rm {lowName}-server");
                }

                sb.AppendLine($"docker rmi {lowName}");
                string sourceDir = null;
                if (type == ImageType.DockerHubImage)
                {
                    sb.AppendLine($"docker pull {lowName}");
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
                    sb.AppendLine($"cd {sourceDir}");
                    sb.AppendLine($"docker build -t {lowName} .");
                }
                if (isPublishToProduct)
                {
                    var targetDir = Path.Combine(workspaceDir, "PublishToProduct", name + "\\");
                    sbSH.AppendLine(sb.ToString().Replace(sourceDir, "/root/PublishToProduct/"+name));
                    if (!sourceDir.IsNullOrEmpty())
                    {
                        sb.AppendLine($"xcopy /Y /S  {sourceDir}  {targetDir}");
                    }
                }
                return sb;
            });
        }

        private static void CreateContainer(string name, string ip = null, string port = null, int? index = null)
        {
            var lowName = name.ToLower();
            ExcuteBat(() =>
            {
                var strIndex = ((index.HasValue && index > 0) ? index.ToString() : string.Empty);
                var sb = new StringBuilder();
                sb.AppendLine($"docker run --network=lynet {ip} -itd --name={lowName}-server{strIndex} {port} -d {lowName}");
                if (isPublishToProduct)
                {
                    sbSH.AppendLine(sb.ToString());
                }
                return sb;
            });
        }
        #endregion

        private static void DeployGateway()
        {
            DirectoryInfo target = workspace.GetDirectories().FirstOrDefault(x =>x.Name.EndsWith("Gateway"));
            if (target != null)
            {
                var name = target.Name;
                BuildApp(name, ImageType.Dotnet);
                CreateDockerfile(name, ImageType.Dotnet);
                BuildImage(name, ImageType.Dotnet);
                CreateContainer(name, "-p 9000:80 -p 5555:5555 -p 5556:5556", "--ip=172.18.211.1");
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
                CreateContainer(name, "-p 9009:80", "--ip=172.18.212.1");
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
                    CreateContainer($"{service.Name}", null, null, i);
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
                CreateContainer(t1.Name, "-p 8081:80");
            }
            var t2 = targets.FirstOrDefault(x => x.Name == "ly.vue.wx");
            if (t2 != null)
            {
                CreateContainer(t2.Name, "-p 8082:80");
            }
        }

        /// <summary>
        /// 第一次部署执行
        /// </summary>
        private static void DeployBase()
        {
            ExcuteBat(() =>
            {
                var sb = new StringBuilder();
                sb.AppendLine("docker network create --subnet=172.18.0.0/16 lynet");
                return sb;
            });

            //修改user的plugin 'caching_sha2_password'-->'mysql_native_password'
            //docker exec -it  mysql-master /bin/bash
            //mysql -u root -p123456
            //alter user 'root'@'%' identified by '123456' password expire never;
            //alter user 'root'@'%' identified with mysql_native_password by '123456';
            //flush privileges;

            //设置docker的配置文件
            //log_bin=mysql-bin
            //server_id = 128 //从:129
            //docker cp mysql-server:/etc/mysql/my.cnf C:\Users\liuyu\Desktop\mysql
            //docker cp mysql-slave:/etc/mysql/my.cnf C:\Users\liuyu\Desktop\mysql
            //docker cp  C:\Users\liuyu\Desktop\mysql\my.cnf mysql-server:/etc/mysql/my.cnf
            //docker cp  C:\Users\liuyu\Desktop\mysql\my.cnf mysql-slave:/etc/mysql/my.cnf
            //docker run --network=lynet -itd --name=mysql-slave -p 3307:3306 -e MYSQL_ROOT_PASSWORD=123456 -d mysql

            //CREATE USER 'slave'@'%' IDENTIFIED BY '123456';
            //GRANT REPLICATION SLAVE ON *.* TO 'slave'@'%';

            //开启从库的slave
            //CHANGE MASTER TO
            //MASTER_HOST = '172.18.200.1',
            //MASTER_USER = 'slave',
            //MASTER_PASSWORD = '123456',
            //MASTER_LOG_FILE = 'mysql-bin.000001',
            //MASTER_LOG_POS = 1668;//只会从当前position同步
            //START SLAVE;
            //SHOW SLAVE STATUS \G

            //ExcuteBat("RunMysql", () =>
            //{
            //    var sb = new StringBuilder();
            //    sb.AppendLine("docker pull mysql");
            //    //sb.AppendLine("docker stop mysql-server");
            //    //sb.AppendLine("docker rm mysql-server");
            //    sb.AppendLine("docker run --network=lynet --ip=172.18.200.1 -itd --name=mysql-master -p 3306:3306 -e MYSQL_ROOT_PASSWORD=123456 -d mysql");
            //    sb.AppendLine("docker run --network=lynet --ip=172.18.200.2 -itd --name=mysql-slave -p 3307:3306 -e MYSQL_ROOT_PASSWORD=123456 -d mysql");
            //    sb.AppendLine($"docker cp {Path.Combine(workspaceDir, "update.sql")} mysql-master:/");
            //    sb.AppendLine($"docker cp {Path.Combine(workspaceDir, "update.sql")} mysql-slave:/");
            //    //sb.AppendLine("docker exec -it mysql-server /bin/bash -c 'mysql -u root -p123456 < update.sql'");
            //    return sb;
            //}, true);

            BuildImage("redis", ImageType.DockerHubImage);
            CreateContainer("redis", "--ip=172.18.201.1", "-p 6379:6379");

            BuildImage("consul", ImageType.DockerHubImage);
            CreateContainer("consul", "--ip=172.18.202.1", "-p 8500:8500");

            BuildImage("rabbitmq", ImageType.DockerHubImage);
            CreateContainer("rabbitmq", "--ip=172.18.203.1", "-p 15672:15672 -p 5672:5672");
        }

    }
}
