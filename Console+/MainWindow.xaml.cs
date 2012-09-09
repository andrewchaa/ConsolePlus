using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Console_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private TextReader _outputReader;
        private TextReader _errorReader;

        private BackgroundWorker _worker;
        private Process _process;

        public MainWindow()
        {
            InitializeComponent();

            SetBackgroundWorker();
            StartProcess("cmd.exe", string.Empty);
        }

        private void SetBackgroundWorker()
        {
            _worker = new BackgroundWorker {WorkerReportsProgress = true, WorkerSupportsCancellation = true};
            _worker.DoWork += (sender, args) => ReadOutput(_worker);
            _worker.ProgressChanged += (sender, args) => UpdateConsole(args);
        }

        private void UpdateConsole(ProgressChangedEventArgs args)
        {
            if (args.UserState is string)
            {
                tbxConsole.Text += args.UserState; // +Environment.NewLine;
                tbxConsole.Focus();
                tbxConsole.SelectionStart = tbxConsole.Text.ToCharArray().Length;
                tbxConsole.SelectionLength = 0;
            }
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

        private void StartProcess(string filename, string arguments)
        {
            var info = new ProcessStartInfo(filename, arguments)
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
    }
}
