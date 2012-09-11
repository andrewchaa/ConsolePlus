using System.Diagnostics;

namespace ConsolePlus.Domain
{
    public class ConsoleProcess : IConsoleProcess
    {
        private Process _process;

        public ConsoleProcess()
        {
            var info = new ProcessStartInfo("cmd.exe", string.Empty)
            {
                UseShellExecute = false,
                ErrorDialog = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };

            _process = new Process { EnableRaisingEvents = true, StartInfo = info, };
        }

        public int Read(char[] buffer, int index, int size)
        {
            return _process.StandardOutput.Read(buffer, 0, 1024);
        }

        public void Start()
        {
            _process.Start();
        }

        public void Write(string command)
        {
            _process.StandardInput.Write(command); 
        }
    }
}
