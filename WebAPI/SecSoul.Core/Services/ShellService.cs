using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace SecSoul.Core.Services
{

    public class ShellService
    {
        private bool IsWindows() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        private bool IsLinux() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        private ILogger<ShellService> _logger;
        public ShellService(ILogger<ShellService> logger)
        {
            _logger = logger;
        }

        public async void ShellExecute(string cmd)
        {
            if (IsLinux())
                LinuxBash(cmd);
            else if (IsWindows())
                WindowsBash(cmd);
            
        }

        private async void LinuxBash(string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            _logger.LogDebug($"Linux Bash finished process with result: {result}");

        }
        private async void WindowsBash(string cmd)
        {

            int exitCode;
            ProcessStartInfo processInfo;
            Process process;

            processInfo = new ProcessStartInfo("cmd.exe", "/c " + cmd);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            // *** Redirect the output ***
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            process = Process.Start(processInfo);
            process.WaitForExit();

            // *** Read the streams ***
            // Warning: This approach can lead to deadlocks, see Edit #2
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            exitCode = process.ExitCode;
            process.Close();
            _logger.LogDebug($"Windows Bash finished process with result \nOUTPUT: {output} \nERROR:{error} \nEXIT CODE: {exitCode}");

        }
    }

}
