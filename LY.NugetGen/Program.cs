using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LY.NugetGen
{
    class Program
    {
        static string workspaceDir = string.Empty;
        static DirectoryInfo workspace = null;
        static string key = "oy2agnj5aerbhx5nlbzq5zblubagmvuejkgqcbrchm7v6q";
        static string version = "1.0.5";

        static void Main(string[] args)
        {
#if DEBUG
            args = new string[] { "practice", "services,gateway,daemon" };
            //args = new string[] { "practice", "services" };
            //args = new string[] { "practice" };
#endif
            if (args == null || args.Length == 0)
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
            workspaceDir = workspace.FullName;

            var names = new string[] {
                "LY.Common",
                //"LY.EFRepository",
                //"LY.Initializer"
            };

            foreach (var name in names)
            {
                NugetPush(name);
                //NugetDelete(item);
            }

            Console.Read();
        }

        #region Common
        private static void NugetPush(string name)
        {
            ExcuteBat(() =>
            {
                var sb = new StringBuilder();
                sb.AppendLine($"cd {Path.Combine(workspaceDir, name)}");
                sb.AppendLine($"dotnet pack -c release -p:PackageVersion={version}");
                return sb;
            });

            ExcuteBat(() =>
            {
                var sb = new StringBuilder();
                sb.AppendLine($"cd {Path.Combine(workspaceDir, name,"bin","release")}");
                sb.AppendLine($"dotnet nuget push {name}.{version}.nupkg -s https://nuget.org -k {key}");
                return sb;
            });
        }

        private static void NugetDelete(string name)
        {
            ExcuteBat(() =>
            {
                var sb = new StringBuilder();
                sb.AppendLine($"dotnet nuget delete {name} {version} -s https://nuget.org -k {key} --non-interactive");
                return sb;
            });
        }

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
        #endregion
    }
}
