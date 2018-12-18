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
    class Program
    {
        public static string workspaceRoot = string.Empty;
        public static string workspaceDir = string.Empty;
        public static string publishDir = @"D:\MyFiles\Code\practicePublish";
        public static string publishRoot = new DirectoryInfo(publishDir).Root.Name.TrimEnd('\\', '\\');
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
                var workspace = new DirectoryInfo(Directory.GetCurrentDirectory());
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
                IEnumerable<DirectoryInfo> targets = GetTargets(workspace);
                KillDotnet();

                //build
                Build(targets);

                //start
                Start(targets);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static IEnumerable<DirectoryInfo> GetTargets(DirectoryInfo workspace)
        {
            return workspace.GetDirectories().Where(x => x.Name.EndsWith("Gateway") || x.Name.EndsWith("Daemon") || x.Name.EndsWith("Service"));
        }
        
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

        private static void Build(IEnumerable<DirectoryInfo> targets)
        {
            foreach (var dic in targets)
            {

                ExcuteBat($"{dic.Name}_build", () =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"{workspaceRoot}");
                    sb.AppendLine($"cd {dic.FullName}");
                    sb.AppendLine($"dotnet restore");
                    sb.AppendLine($"dotnet build");
                    sb.AppendLine($"dotnet publish -c Release -o {Path.Combine(publishDir, dic.Name)}");
                    sb.AppendLine($"exit");
                    return sb;
                }, true);
            }
            Console.WriteLine("build sucess");
        }

        private static void Start(IEnumerable<DirectoryInfo> targets)
        {
            ExcuteBat("Createlynet", () =>
            {
                var sb = new StringBuilder();
                sb.AppendLine("docker network create --subnet=172.18.0.0/16 lynet");
                return sb;
            }, true);
            ExcuteBat("RunMysql", () =>
            {
                var sb = new StringBuilder();
                sb.AppendLine("docker pull mysql");
                //sb.AppendLine("docker stop mysql-server");
                //sb.AppendLine("docker rm mysql-server");
                sb.AppendLine("docker run --network=lynet --ip=172.18.0.2 -itd --name=mysql-server -p 3306:3306 -e MYSQL_ROOT_PASSWORD=123456 -d mysql");
                sb.AppendLine("docker exec -it mysql-server bash");
                sb.AppendLine($"docker cp {Path.Combine(workspaceDir, "update.sql")} mysql-server:/");
                sb.AppendLine("mysql -u root -p123456");
                sb.AppendLine("source ly.sql");
                return sb;
            }, true);
            ExcuteBat("RunRedis", () =>
            {
                var sb = new StringBuilder();
                sb.AppendLine("docker pull redis");
                sb.AppendLine("docker stop redis-server");
                sb.AppendLine("docker rm redis-server");
                sb.AppendLine("docker run --network=lynet --ip=172.18.0.3 -itd --name=redis-server -p 6379:6379 -d redis");
                return sb;
            }, true);
            ExcuteBat("RunConsul", () =>
            {
                var sb = new StringBuilder();
                sb.AppendLine("docker pull consul");
                sb.AppendLine("docker stop consul-server");
                sb.AppendLine("docker rm consul-server");
                sb.AppendLine("docker run --network=lynet --ip=172.18.0.4 -itd --name=consul-server -p 8500:8500 -d consul");
                return sb;
            }, true);

            void GenDockerfile(string name)
            {
                using (FileStream fs = File.Create(Path.Combine(publishDir, name,"Dockerfile")))
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
            }

            foreach (var item in targets)
            {
                GenDockerfile(item.Name);
                ExcuteBat($"{item.Name}_docker_build", () =>
                {
                    var sb = new StringBuilder();
                    sb.AppendLine($"docker stop {item.Name.ToLower()}");
                    sb.AppendLine($"docker rm {item.Name.ToLower()}");
                    sb.AppendLine($"docker rmi {item.Name.ToLower()}");
                    sb.AppendLine($"{publishRoot}");
                    sb.AppendLine($"cd {Path.Combine(publishDir, item.Name)}");
                    sb.AppendLine($"docker build -t {item.Name.ToLower()} .");
                    return sb;
                }, true);
            }

            var gateway = targets.FirstOrDefault(x => x.Name.EndsWith("Gateway"));

            if (gateway != null)
            {
                ExcuteBat($"{gateway.Name}_docker_run", () =>
                {
                    return GetCMDForServieDockerRun(gateway);
                }, true);
            }

            var services = targets.Except(new DirectoryInfo[] { gateway });

            foreach (var service in services)
            {
                ExcuteBat($"{service.Name}_docker_run", () =>
                {
                    return GetCMDForServieDockerRun(service);
                }, true);
            }
        }

        private static StringBuilder GetCMDForServieDockerRun(DirectoryInfo dir)
        {
            var sb = new StringBuilder();
            string port = string.Empty;
            string ip = string.Empty;
            var name = dir.Name;
            if (name.EndsWith("Gateway"))
            {
                port = " -p 9000:80 -p 5555:5555 -p 5556:5556";
                ip = " --ip=172.18.0.5";
            }
            sb.AppendLine($"docker stop {dir.Name.ToLower()}-server");
            sb.AppendLine($"docker rm {dir.Name.ToLower()}-server");
            sb.AppendLine($"docker run --network=lynet{ip} -itd --name={dir.Name.ToLower()}-server{port} -d {dir.Name.ToLower()}");
            return sb;
        }

        private static void KillDotnet()
        {  
            //kill
            foreach (var existProcess in Process.GetProcessesByName("dotnet"))
            {
                if (existProcess.Id != Process.GetCurrentProcess().Id)
                {
                    existProcess.Kill();
                }
            }
        }

    }
}
