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
            if (args == null || args.Length == 0)
            {
                args = new string[] { "practice" };
            }


            if (args == null || args.Length <= 0)
            {
                Console.WriteLine("请输入workspace名称");
                return;
            }
            var workspace = $"C:\\Program Files (x86)\\Jenkins\\workspace\\{args[0]}";
            if (!Directory.Exists(workspace))
            {
                Console.WriteLine($"无法找到workspace{workspace}");
                return;
            }
            var dirWorkspace = new DirectoryInfo(workspace);
            var targets = new List<DirectoryInfo>() { };
            var gateway = dirWorkspace.GetDirectories().FirstOrDefault(x => x.Name.EndsWith("Gateway"));
            if (gateway != null)
            {
                targets.Add(gateway);
            }
            targets.AddRange(dirWorkspace.GetDirectories().Where(x => x.Name.EndsWith("Service")));

            //kill
            foreach (var existProcess in Process.GetProcessesByName("dotnet"))
            {
                if (existProcess.Id != Process.GetCurrentProcess().Id)
                {
                    existProcess.Kill();
                }
            }

            foreach (var dic in targets)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"cd {dic.FullName}");
                sb.AppendLine($"dotnet restore");
                sb.AppendLine($"dotnet build");
                sb.AppendLine($"dotnet publish -c Release -o D:\\MyFiles\\Code\\practicePublish\\{dic.Name}");
                sb.AppendLine($"cd D:\\MyFiles\\Code\\practicePublish\\{dic.Name}");
                sb.AppendLine($"dotnet D:\\MyFiles\\Code\\practicePublish\\{dic.Name}\\{dic.Name}.dll");
                using (FileStream fs = File.Create($"{dic.Name}.bat"))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(sb.ToString());
                    sw.Flush();
                }
                Process.Start(Path.Combine(Directory.GetCurrentDirectory(), $"{dic.Name}.bat"));
            }    
        }
    }
}
