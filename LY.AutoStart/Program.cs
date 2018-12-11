using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LY.AutoStart
{
    class Program
    {
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
                var targets = new List<DirectoryInfo>() { };
                var daemon = workspace.GetDirectories().FirstOrDefault(x => x.Name.EndsWith("Daemon"));
                if (daemon != null)
                {
                    targets.Add(daemon);
                }
                var gateway = workspace.GetDirectories().FirstOrDefault(x => x.Name.EndsWith("Gateway"));
                if (gateway != null)
                {
                    targets.Add(gateway);
                }
                targets.AddRange(workspace.GetDirectories().Where(x => x.Name.EndsWith("Service")));
                KillDotnet();

                //redis
                var redisProcess = Process.GetProcessesByName("redis-server");
                if (redisProcess != null && redisProcess.Length>0)
                {
                    foreach (var item in redisProcess)
                    {
                        item.Kill();
                    }
                }
                Process.Start(Path.Combine(workspace.FullName, "tools", "redis", "redis-server.exe"));

                //consul
                var consulProcess = Process.GetProcessesByName("consul");
                if (consulProcess != null && consulProcess.Length > 0)
                {
                    foreach (var item in redisProcess)
                    {
                        item.Kill();
                    }
                }

                using (FileStream fs = File.Create("consul.bat"))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"cd {Path.Combine(workspace.FullName, "tools", "consul")}");
                    sb.AppendLine($"consul.exe agent -dev");
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(sb.ToString());
                    sw.Flush();
                }
                Process.Start(Path.Combine(Directory.GetCurrentDirectory(), "consul.bat"));
                

                //build
                foreach (var dic in targets)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"cd {dic.FullName}");
                    sb.AppendLine($"dotnet restore");
                    sb.AppendLine($"dotnet build");
                    sb.AppendLine($"dotnet publish -c Release -o D:\\MyFiles\\Code\\practicePublish\\{dic.Name}");
                    sb.AppendLine($"exit");
                    using (FileStream fs = File.Create($"{dic.Name}_build.bat"))
                    {
                        StreamWriter sw = new StreamWriter(fs);
                        sw.WriteLine(sb.ToString());
                        sw.Flush();
                    }
                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName = Path.Combine(Directory.GetCurrentDirectory(), $"{dic.Name}_build.bat");
                        process.Start();
                        process.WaitForExit();
                    }
                }
                Console.WriteLine("build sucess");

                //kill bulid process
                KillDotnet();

                //start
                foreach (var dic in targets)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"cd D:\\MyFiles\\Code\\practicePublish\\{dic.Name}");
                    sb.AppendLine($"dotnet {dic.Name}.dll");
                    using (FileStream fs = File.Create($"{dic.Name}_run.bat"))
                    {
                        StreamWriter sw = new StreamWriter(fs);
                        sw.WriteLine(sb.ToString());
                        sw.Flush();
                    }
                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName = Path.Combine(Directory.GetCurrentDirectory(), $"{dic.Name}_run.bat");
                        if (process.Start())
                        {
                            Console.WriteLine($"{dic.Name} start sucess");
                        }
                        else
                        {
                            Console.WriteLine($"{dic.Name} start fail");
                        }
                    }
                    if (dic.Name.EndsWith("Gateway"))
                    {
                        Thread.Sleep(5000);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
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
