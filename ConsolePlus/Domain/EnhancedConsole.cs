using System.ComponentModel;
using System.Text;

namespace ConsolePlus.Domain
{
    public class EnhancedConsole
    {
        private readonly IConsoleWindow _window;
        private BackgroundWorker _worker;
        private readonly IConsoleProcess _process;

        public EnhancedConsole(IConsoleWindow window, IConsoleProcess process)
        {
            _window = window;
            _process = process;
            SetBackgroundWorker();
            History = new CommandHistory();
        }

        public CommandHistory History { get; private set; }

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
                count = _process.Read(buffer, 0, 1024);
                builder.Append(buffer, 0, count);
                worker.ReportProgress(0, builder.ToString());

            } while (count > 0);
        }

        public void Start()
        {
            _process.Start();

            _worker.RunWorkerAsync();
        }

        public void Write(string command)
        {
            _process.Write(command); 
            History.Add(command);
        }
    }
}
