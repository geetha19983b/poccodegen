using System.Diagnostics;
using System.Threading.Tasks;

namespace IMP4CMACGM.Core.Common
{
    public static class ProcessAsyncHelper
    {
        public static async Task<ProcessResult> RunProcessAsync(string command, string arguments, int timeout)
        {
            var result = new ProcessResult();

            using (var process = new Process())
            {
                process.StartInfo.FileName = command;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = false;

                var isStarted = process.Start();
                if (!isStarted)
                {
                    result.ExitCode = process.ExitCode;
                    return result;
                }

                var waitForExit = WaitForExitAsync(process, timeout);
                var processTask = Task.WhenAll(waitForExit);
                if (await Task.WhenAny(Task.Delay(timeout), processTask) == processTask && waitForExit.Result)
                {
                    result.ExitCode = process.ExitCode;
                }
                else
                {
                    try
                    {
                        process.Kill();
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            return result;
        }


        private static Task<bool> WaitForExitAsync(Process process, int timeout)
        {
            return Task.Run(() => process.WaitForExit(timeout));
        }


        public struct ProcessResult
        {
            public int? ExitCode;
            public string Output;
            public string Error;
        }
    }
}
