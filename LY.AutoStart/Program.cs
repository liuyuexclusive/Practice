using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LY.AutoStart
{
    class Program
    {
        static void Main(string[] args)
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
                var gateway = workspace.GetDirectories().FirstOrDefault(x => x.Name.EndsWith("Gateway"));
                if (gateway != null)
                {
                    targets.Add(gateway);
                }
                targets.AddRange(workspace.GetDirectories().Where(x => x.Name.EndsWith("Service")));
                KillDotnet();

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
                //kill
                KillDotnet();
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
                        process.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                KillDotnet();
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
