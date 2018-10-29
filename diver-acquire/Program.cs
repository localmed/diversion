using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Diversion.Aquisition
{
    class Program
    {
        static int Main(string[] args)
        {
            var fetcher = new NuGetReleaseFetcher();
            var task = fetcher.Fetch("Microsoft.Extensions.Logging", "net47", "*", Path.GetFullPath("latest"));
            Console.WriteLine("Fetching Release...");
            task.Wait();
            Console.Write("Press any key to exit");
            Console.ReadKey();
            return task.IsCompletedSuccessfully ? 0 : 1;
        }
    }

    public class NuGetReleaseFetcher
    {
        public async Task<bool> Fetch(string packageId, string targetFramework, string targetVersion, string targetPath)
        {
            return await Task.Run(() =>
            {
                var diversionPath = Path.Combine(Path.GetTempPath(), ".diversion");
                var projectPath = Path.Combine(diversionPath, $"{packageId}.csproj");
                Directory.CreateDirectory(diversionPath);
                using (var output = new FileStream(projectPath, FileMode.Create))
                using (var source = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(NuGetReleaseFetcher), "Resources.acquire.csproj"))
                    source.CopyTo(output);
                var config = new ProcessStartInfo
                {
                    FileName = "msbuild",
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                };
                config.ArgumentList.Add("/restore");
                config.ArgumentList.Add(projectPath);
                config.ArgumentList.Add($"/p:TargetFramework={targetFramework}");
                if (!string.IsNullOrEmpty(targetPath))
                    config.ArgumentList.Add($"/p:OutputPath={targetPath}");
                if (!string.IsNullOrEmpty(targetVersion))
                    config.ArgumentList.Add($"/p:TargetVersion={targetVersion}");
                var process = Process.Start(config);
                process.WaitForExit();
                return process.ExitCode == 0;
            });
        }
    }
}
