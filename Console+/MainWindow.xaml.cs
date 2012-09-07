using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

            _worker = new BackgroundWorker()
                          {
                              WorkerReportsProgress = true,
                              WorkerSupportsCancellation = true
                          };
            _worker.DoWork += (sender, args) =>
                                  {
                                      while (!_process.StandardOutput.EndOfStream)
                                      {
                                          string output = _process.StandardOutput.ReadLine();
                                          _worker.ReportProgress(0, output);
                                      }
                                  };

            _worker.ProgressChanged += (sender, args) =>
                                           {
                                               if (args.UserState is string)
                                               {
                                                   tbxConsole.Text += args.UserState + Environment.NewLine;
                                               }
                                           };

            StartProcess("cmd.exe", string.Empty);
        }

        private void StartProcess(string filename, string arguments)
        {
            var info = new ProcessStartInfo(filename, arguments)
                           {
                               UseShellExecute = false, 
                               ErrorDialog = false,
//                               CreateNoWindow = true,
                               RedirectStandardError = true,
                               RedirectStandardInput = true,
                               RedirectStandardOutput = true
                           };

            _process = new Process()
                           {
                               EnableRaisingEvents = true,
                               StartInfo = info,
                           };
            _process.Start();

            _worker.RunWorkerAsync();
//            tbxConsole.Text += _process.StandardOutput.ReadToEnd();
        }
    }
}
