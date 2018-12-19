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
        Docker,
        Dotnet,
        Vue
    }

    class Program
    {
        static string workspaceRoot = string.Empty;
        static string workspaceDir = string.Empty;
        static DirectoryInfo workspace = null;
        static void Main(string[] args)
        {
            Start(ref args);
            Console.Read();
        }

        private static void Start(ref string[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                {
                    args = new string[] { "practice" };
                }

                if (args == null || args.Length <= 0)
                {
                    Console.WriteLine("请输入workspace名称");
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
                workspaceRoot = workspace.Root.Name.TrimEnd('\\', '\\');
                workspaceDir = workspace.FullName;

                //DeployFirst();
                DeployServices();
                DeployVue();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #region common
        private static void ExcuteBat(string name, Func<StringBuilder> func, bool exit = false)
        {
            using (FileStream fs = File.Create($"{name}.bat"))
            {
                StreamWriter sw = new StreamWriter(fs);
                var sb = func();
                if (exit)
                {
                    sb.AppendLine("exit");
                }
                sw.WriteLine(sb.ToString());
                sw.Flush();
            }
            using (Process process = new Process())
            {
                process.StartInfo.FileName = Path.Combine(Directory.GetCurrentDirectory(), $"{name}.bat");
                process.Start();
                if (exit)
                {
                    process.WaitForExit();
                }
            }
        }

        private static void CreateContainer(string name, string ip = null, string port = null)
        {
            var lowName = name.ToLower();
            ExcuteBat($"{name}_docker_run", () =>
            {
                var sb = new StringBuilder();
                sb.AppendLine($"docker run --network=lynet {ip} -itd --name={lowName}-server {port} -d {lowName}");
                return sb;
            }, true);
        }

        private static void BuildImage(string name, ImageType type)
        {
            var lowName = name.ToLower();
            ExcuteBat($"{name}_docker_build", () =>
            {
                var sb = new StringBuilder();
                sb.AppendLine($"docker stop {lowName}-server");
                sb.AppendLine($"docker rm {lowName}-server");
                sb.AppendLine($"docker rmi {lowName}");
                switch (type)
                {
                    case ImageType.Dotnet:
                        sb.AppendLine($"{workspaceRoot}");
                        sb.AppendLine($"cd {Path.Combine(workspaceDir, name, "bin", "Release", "netcoreapp2.2", "publish")}");
                        sb.AppendLine($"docker build -t {lowName} .");
                        break;
                    case ImageType.Docker:
                        sb.AppendLine($"docker pull {lowName}");
                        break;
                    case ImageType.Vue:
                        sb.AppendLine($"{workspaceRoot}");
                        sb.AppendLine($"cd {Path.Combine(workspaceDir, name, "dist")}");
                        sb.AppendLine($"docker build -t {lowName} .");
                        break;
                    default:
                        break;
                }
                return sb;
            }, true);
        }

        private static void BuildApp(string name, ImageType type)
        {
            var lowName = name;
            switch (type)
            {
                case ImageType.Dotnet:
                    ExcuteBat($"{name}_build", () =>
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"{workspaceRoot}");
                        sb.AppendLine($"cd {Path.Combine(workspaceDir, name)}");
                        sb.AppendLine($"dotnet restore");
                        sb.AppendLine($"dotnet build");
                        sb.AppendLine($"dotnet publish -c Release");
                        return sb;
                    }, true);
                    break;
                case ImageType.Vue:
                    ExcuteBat($"{name}_install", () =>
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"{workspaceRoot}");
                        sb.AppendLine($"cd {Path.Combine(workspaceDir, name)}");
                        sb.AppendLine($"npm install");
                        return sb;
                    }, true);
                    ExcuteBat($"{name}_build", () =>
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"{workspaceRoot}");
                        sb.AppendLine($"cd {Path.Combine(workspaceDir, name)}");
                        sb.AppendLine($"npm run build");
                        return sb;
                    }, true);
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
                    using (FileStream fs = File.Create(Path.Combine(workspaceDir, name, "bin", "Release", "netcoreapp2.2", "publish", "Dockerfile")))
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
        #endregion

        private static void DeployServices()
        {
            IEnumerable<DirectoryInfo> targets = workspace.GetDirectories().Where(x => x.Name.EndsWith("Gateway") || x.Name.EndsWith("Daemon") || x.Name.EndsWith("Service"));

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

            //docker run

            var gateway = targets.FirstOrDefault(x => x.Name.EndsWith("Gateway"));
            if (gateway != null)
            {
                CreateContainer(gateway.Name, "-p 9000:80 -p 5555:5555 -p 5556:5556", "--ip=172.18.203.1");
            }

            var daemon = targets.FirstOrDefault(x => x.Name.EndsWith("Daemon"));
            if (daemon != null)
            {
                CreateContainer(daemon.Name, "-p 9009:80", "--ip=172.18.204.1");
            }

            var services = targets.Except(new DirectoryInfo[] { gateway, daemon });

            foreach (var service in services)
            {
                CreateContainer(service.Name);
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
        private static void DeployFirst()
        {
            ExcuteBat("lynet", () =>
            {
                var sb = new StringBuilder();
                sb.AppendLine("docker network create --subnet=172.18.0.0/16 lynet");
                return sb;
            }, true);

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

            BuildImage("redis", ImageType.Docker);
            CreateContainer("redis", "--ip=172.18.201.1", "-p 6379:6379");
            BuildImage("consul", ImageType.Docker);
            CreateContainer("consul", "--ip=172.18.202.1", "-p 8500:8500");
        }

    }
}
