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
        const int serviceNum = 1; //服务数量
        const bool isPublishToLinux = true;
        const string docker = isPublishToLinux ? "docker" : "docker";
        const string deployFolder = @"/users/liuyu/code/practice-release";
        const string deployScriptFileName = isPublishToLinux ? "deploy.sh" : "deploy.bat";

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
            Console.Read();
        }

        private static void Start(ref string[] args)
        {
            try
            {
#if DEBUG
                args = new string[] { "practice", "gateway,daemon,services" };
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


                //var publish = Path.Combine(workspaceDir, "PublishToProduct");
                //Directory.Delete(publish,true);
                //Directory.CreateDirectory(publish);

                var options = args[1].Split(",").Distinct();
                Deploy(options).Wait();

                string result = sbDockerCmd.ToString();

                if (!isPublishToLinux)
                {
                    ExcuteBat(() => { return sbDockerCmd; });
                }
                else
                {
                    result = result.Replace("\r\n", "\n");
                }
                using (FileStream fs = File.Create(Path.Combine(deployFolder, deployScriptFileName)))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(result);
                    sw.Flush();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static async Task Deploy(IEnumerable<string> options)
        {
            if (!options.Intersect(new string[] { "base" }).IsNullOrEmpty())//默认不发布基础设施
            {
                DeployBase();
            }
            if (!options.Intersect(new string[] { "gateway", "all" }).IsNullOrEmpty())
            {
                await DeployGateway();
            }
            if (!options.Intersect(new string[] { "daemon", "all" }).IsNullOrEmpty())
            {
                await DeployDaemon();
            }
            if (!options.Intersect(new string[] { "services", "all" }).IsNullOrEmpty())
            {
                await DeployServices();
            }
            if (!options.Intersect(new string[] { "vue", "all" }).IsNullOrEmpty())
            {
                await DeployVue();
            }
        }

        #region common
        private static void ExcuteBat(Func<StringBuilder> func)
        {
            var sb = func();
            using (Process process = new Process())
            {
                process.StartInfo.FileName = isPublishToLinux?"/bin/bash":"cmd.exe";
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
                    //ExcuteBat(() =>
                    //{
                    //    StringBuilder sb = new StringBuilder();
                    //    sb.AppendLine($"cd {Path.Combine(workspaceDir, name)}");
                    //    sb.AppendLine($"npm install");
                    //    return sb;
                    //});
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
                    sb.AppendLine($"{docker} stop {lowName}-server{index}");
                    sb.AppendLine($"{docker} rm {lowName}-server{index}");
                }
            }
            else
            {
                sb.AppendLine($"{docker} stop {lowName}-server");
                sb.AppendLine($"{docker} rm {lowName}-server");
            }

            sb.AppendLine($"{docker} rmi {lowName}{imageVersion}");
            string sourceDir = null;
            //var targetDir = Path.Combine(workspaceDir, "PublishToProduct", name + "\\");
            var targetDir = Path.Combine(deployFolder, name.ToLower());
            if(!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);//这里在mac里面如果用cp来创建文件夹，会出现系统cd无法识别的情况
            }
            if (type == ImageType.DockerHubImage)
            {
                sb.AppendLine($"{docker} pull {lowName}{imageVersion}");
            }
            else
            {
                switch (type)
                {
                    case ImageType.Dotnet:
                        sourceDir = Path.Combine(workspaceDir, name, "bin", "Release", netcoreappVersion, "publish/");
                        break;
                    case ImageType.Vue:
                        sourceDir = Path.Combine(workspaceDir, name, "dist/");
                        break;
                    default:
                        break;
                }
            }

            
            if (!sourceDir.IsNullOrEmpty())
            {
                string copyStr = isPublishToLinux?$"cp -r -f {sourceDir}  {targetDir} \n":$"xcopy /Y /S  {sourceDir}  {targetDir} \n";

                ExcuteBat(() => new StringBuilder(copyStr));
                if (isPublishToLinux)
                {
                    sb.AppendLine($"{docker} build -t {lowName} /var/jenkins_home/workspace/test/{name.ToLower()}/");
                }
                else
                {
                    sb.AppendLine($"cd {targetDir}");
                    sb.AppendLine($"{docker} build -t {lowName} .");
                }
            } 

            string result = sb.ToString().Trim();
            sbDockerCmd.AppendLine(result);
        }

        private static void CreateContainer(string name, string ip = null, string port = null, int? index = null)
        {
            var lowName = name.ToLower();

            var strIndex = ((index.HasValue && index > 0) ? index.ToString() : string.Empty);
            sbDockerCmd.AppendLine($"{docker} run -d --net=lynet {ip} {port} --name={lowName}-server{strIndex} {lowName}");
        }
        #endregion

        private async static Task DeployGateway()
        {
            await Task.Run(() => {
                DirectoryInfo target = workspace.GetDirectories().FirstOrDefault(x => x.Name.EndsWith("Gateway"));
                if (target != null)
                {
                    var name = target.Name;
                    BuildApp(name, ImageType.Dotnet);
                    CreateDockerfile(name, ImageType.Dotnet);
                    BuildImage(name, ImageType.Dotnet);
                    CreateContainer(name, $"--ip={Const.IP._gateway}", $"-p {Const.Port._gateway}:{Const.Port._gateway}");
                }
            });
        }

        private async static Task DeployDaemon()
        {
            await Task.Run(() => {
                DirectoryInfo target = workspace.GetDirectories().FirstOrDefault(x => x.Name.EndsWith("Daemon"));
                if (target != null)
                {
                    var name = target.Name;
                    BuildApp(name, ImageType.Dotnet);
                    CreateDockerfile(name, ImageType.Dotnet);
                    BuildImage(name, ImageType.Dotnet);
                    CreateContainer(name, $"--ip={Const.IP._daemon}", $"-p {Const.Port._daemon}:{Const.Port._daemon}");
                }
            });

        }

        private async static Task DeployServices()
        {
            await Task.Run(()=>{
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
            });

        }

        private async static Task DeployVue()
        {
            await Task.Run(() =>
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
            });

        }

        /// <summary>
        /// 第一次部署执行
        /// </summary>
        private static void DeployBase()
        {
            sbDockerCmd.AppendLine($"{docker} network create --subnet={Const.IP._network} lynet");

            #region jenkins(dood)
            //sudo chown -R 1000:1000 /data/docker/jenkins/
            sbDockerCmd.AppendLine($"{docker} run -d --name=jenkins-server --network=lynet  -p 10000:{Const.Port._jenkins} -p 50000:50000 -v /var/run/docker.sock:/var/run/docker.sock -v /path/to/your/jenkins/home:/var/jenkins_home pitkley/jenkins-dood:latest");
            #endregion

            #region mysql master and slave
            sbDockerCmd.AppendLine($"{docker} run -d --name=mysql-master --network=lynet --ip={Const.IP._mysqlMaster} -p 3306:3306 -e MYSQL_ROOT_PASSWORD=123456 -d mysql:latest");
            //sbDockerCmd.AppendLine($"{docker} run --network=lynet --ip={Const.IP._mysqlSlave} -itd --name=mysql-slave -p 3307:3306 -e MYSQL_ROOT_PASSWORD=123456 -d mysql");

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
            //docker cp  C:\Users\liudocker yu\Desktop\mysql\my.cnf mysql-master:/etc/mysql/my.cnf
            //docker cp  C:\Users\liuyu\Desktop\mysql\my.cnf mysql-slave:/etc/mysql/my.cnf

            //开启从库的slavelog
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

            #region redis
            sbDockerCmd.AppendLine($"{docker} run -d  --net=lynet --ip={Const.IP._redis} -p {Const.Port._redis}:{Const.Port._redis} --name=redis-server redis:latest");
            //mkdir redis-cluster
            //cd redis-cluster
            //touch redis-cluster.tmpl

            /*
port ${PORT}
protected-mode no
cluster-enabled yes
cluster-config-file nodes.conf
cluster-node-timeout 5000
cluster-announce-ip 172.16.30.37
cluster-announce-port ${PORT}
cluster-announce-bus-port 1${PORT}
appendonly yes
             */

            /*
for port in `seq 7000 7005`; do \
  mkdir -p ./${port}/conf \
  && PORT=${port} envsubst < ./redis-cluster.tmpl > ./${port}/conf/redis.conf \
  && mkdir -p ./${port}/data; \
done
            */

            /*
for port in `seq 7000 7005`; do \
  sudo docker run -d -ti -p ${port}:${port} -p 1${port}:1${port} \
  -v /home/liuyu/redis-cluster/${port}/conf/redis.conf:/usr/local/etc/redis/redis.conf \
  -v /home/liuyu/redis-cluster/${port}/data:/data \
  --restart always --name redis-${port} --net lynet \
  --sysctl net.core.somaxconn=1024 redis redis-server /usr/local/etc/redis/redis.conf; \
done
            */

            //sodu docker exec -it redis-7000 bash
            //redis-cli -p 7001 -h 172.16.30.37

            /*
redis-cli --cluster create 172.16.30.37:7000 172.16.30.37:7001 \
172.16.30.37:7002 172.16.30.37:7003 172.16.30.37:7004 172.16.30.37:7005 \
--cluster-replicas 1
             */

            #endregion

            #region consul
            sbDockerCmd.AppendLine($"{docker} run -d  --net=lynet --ip=172.19.202.4 -p {Const.Port._consul}:{Const.Port._consul} --name=consul-server consul:latest");

            //sbDockerCmd.AppendLine($"{docker} run -d  --net=lynet --ip=172.19.202.1 --name=consul-server consul:latest agent -server -bootstrap-expect=2");
            //sbDockerCmd.AppendLine($"{docker} run -d  --net=lynet --ip=172.19.202.2 --name=consul-server1 consul:latest agent -server -join 172.19.202.1");
            //sbDockerCmd.AppendLine($"{docker} run -d  --net=lynet --ip=172.19.202.3 --name=consul-server2 consul:latest agent -server -join 172.19.202.1");
            //sbDockerCmd.AppendLine($"{docker} run -d  --net=lynet --ip={Const.IP._consul} -p {Const.Port._consul}:{Const.Port._consul}  --name=consul-server3 consul:latest agent -join 172.19.202.1 -client=\"0.0.0.0\" -ui");
            #endregion

            #region rabbitmq
            sbDockerCmd.AppendLine($"{docker} run -d -p {Const.Port._rabbitmq}:{Const.Port._rabbitmq} -p 15672:15672 --net=lynet --ip={Const.IP._rabbitmq} --name=rabbitmq-server rabbitmq:latest");
            //docker run -d -p 5672:5672 -p 15672:15672 --net=lynet --ip=172.19.203.1 --name=rabbitmq-server1 --hostname=rabbit1 -e RABBITMQ_ERLANG_COOKIE='rabbitcookie'  rabbitmq:management
            //docker run -d --net=lynet --ip=172.19.203.2 --name=rabbitmq-server2 --hostname=rabbit2 --link rabbitmq-server1:rabbit1 -e RABBITMQ_ERLANG_COOKIE='rabbitcookie'  rabbitmq:management
            //docker run -d --net=lynet --ip=172.19.203.3 --name=rabbitmq-server3 --hostname=rabbit3 --link rabbitmq-server1:rabbit1 --link rabbitmq-server2:rabbit2 -e RABBITMQ_ERLANG_COOKIE='rabbitcookie'  rabbitmq:management

            //docker exec -it rabbitmq-server1 bash
            //rabbitmqctl stop_app
            //rabbitmqctl reset
            //rabbitmqctl start_app
            //exit

            //docker exec -it rabbitmq-server2 bash
            //rabbitmqctl stop_app
            //rabbitmqctl reset
            //rabbitmqctl join_cluster --ram rabbit@rabbit1
            //rabbitmqctl start_app
            //exit

            //docker exec -it rabbitmq-server3 bash
            //rabbitmqctl stop_app
            //rabbitmqctl reset
            //rabbitmqctl join_cluster --ram rabbit@rabbit1
            //rabbitmqctl start_app
            //exit 
            #endregion

            #region elasticsearch
            //sysctl -w vm.max_map_count=262144
            //docker run -d -p 9200:9200 -p 9300:9300 --net=lynet --ip=172.19.204.1 --name=elasticsearch-server elasticsearch:6.6.1
            //集群部署：
            //https://www.elastic.co/guide/en/elasticsearch/reference/6.6/docker.html#docker-prod-cluster-composefile
            /*
version: '2.2'
services:
  elasticsearch-server:
    image: docker.elastic.co/elasticsearch/elasticsearch:6.6.1
    container_name: elasticsearch-server
    environment:
      - cluster.name=docker-cluster
      - bootstrap.memory_lock=true
      - "ES_JAVA_OPTS=-Xms512m -Xmx512m"
    ulimits:
      memlock:
        soft: -1
        hard: -1
    volumes:
      - esdata1:/usr/share/elasticsearch/data
    ports:
      - 9200:9200
    networks:
        lynet:
          ipv4_address: 172.19.204.1
  elasticsearch-server2:
    image: docker.elastic.co/elasticsearch/elasticsearch:6.6.1
    container_name: elasticsearch-server2
    environment:
      - cluster.name=docker-cluster
      - bootstrap.memory_lock=true
      - "ES_JAVA_OPTS=-Xms512m -Xmx512m"
      - "discovery.zen.ping.unicast.hosts=elasticsearch-server"
    ulimits:
      memlock:
        soft: -1
        hard: -1
    volumes:
      - esdata2:/usr/share/elasticsearch/data
    networks:
        lynet:
          ipv4_address: 172.19.204.2

  elasticsearch-server3:
    image: docker.elastic.co/elasticsearch/elasticsearch:6.6.1
    container_name: elasticsearch-server3
    environment:
      - cluster.name=docker-cluster
      - bootstrap.memory_lock=true
      - "ES_JAVA_OPTS=-Xms512m -Xmx512m"
      - "discovery.zen.ping.unicast.hosts=elasticsearch-server"
    ulimits:
      memlock:
        soft: -1
        hard: -1
    volumes:
      - esdata3:/usr/share/elasticsearch/data
    networks:
        lynet:
          ipv4_address: 172.19.204.3

volumes:
  esdata1:
    driver: local
  esdata2:
    driver: local
  esdata3:
    driver: local

networks:
  lynet:
    external:
      name: lynet
             */
            sbDockerCmd.AppendLine($"{docker} run -d -p {Const.Port._elasticsearch}:{Const.Port._elasticsearch} -p 9300:9300 -e \"discovery.type=single-node\"  -e ES_JAVA_OPTS=\" - Xms256m - Xmx256m\" --net=lynet --ip={Const.IP._elasticsearch} --name=elasticsearch-server elasticsearch:6.6.1"); 
            #endregion

            sbDockerCmd.AppendLine($"{docker} run -d -p {Const.Port._kibana}:{Const.Port._kibana} -e \"elasticsearch.hosts=http://{Const.IP._elasticsearch}:{Const.Port._elasticsearch}\" --net=lynet --ip={Const.IP._kibana} --name=kibana-server kibana:6.6.1");
            //docker cp kibana-server:/usr/share/kibana/config/kibana.yml C:\Users\liuyu\Desktop\mysql
            //docker cp C:\Users\liuyu\Desktop\mysql\kibana.yml kibana-server:/usr/share/kibana/config/kibana.yml

            //sbDockerCmd.AppendLine($"{docker} start mysql-master");
            //sbDockerCmd.AppendLine($"{docker} start mysql-slave");
            //sbDockerCmd.AppendLine($"{docker} start redis-server");
            //sbDockerCmd.AppendLine($"{docker} start elasticsearch-server");
            //sbDockerCmd.AppendLine($"{docker} start kibana-server");
            //sbDockerCmd.AppendLine($"{docker} start rabbitmq-server");
            //sbDockerCmd.AppendLine($"{docker} start consul-server");
            //sbDockerCmd.AppendLine($"{docker} start consul-server1");
            //sbDockerCmd.AppendLine($"{docker} start consul-server2");
            //sbDockerCmd.AppendLine($"{docker} start consul-server3");
        }
    }
}
