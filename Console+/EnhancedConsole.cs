using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Console_
{
    public class EnhancedConsole
    {
        private readonly MainWindow _window;
        private BackgroundWorker _worker;
        private Process _process;

        public EnhancedConsole(MainWindow window)
        {
            _window = window;
            SetBackgroundWorker();
        }

        private void SetBackgroundWorker()
        {
            _worker = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            _worker.DoWork += (sender, args) => ReadOutput(_worker);
            _worker.ProgressChanged += (sender, args) => _window.UpdateConsole(args);
        }

        private void ReadOutput(BackgroundWorker worker)
        {
            int count;
            var buffer = new char[1024];
            do
            {
                var builder = new StringBuilder();
                count = _process.StandardOutput.Read(buffer, 0, 1024);
                builder.Append(buffer, 0, count);
                worker.ReportProgress(0, builder.ToString());
            } while (count > 0);
        }

        public void Start()
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
            _process.Start();

            _worker.RunWorkerAsync();
        }

        public void Write(string s)
        {
            _process.StandardInput.Write(s);
        }
    }
}
