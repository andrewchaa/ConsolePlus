using System;
using System.ComponentModel;
using System.Threading;
using System.Timers;
using System.Windows.Input;
using System.Windows.Threading;
using ConsolePlus.Domain;
using ConsolePlus.Infrastructure;

namespace ConsolePlus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly EnhancedConsole _console;
        private readonly IParse _command;
        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();

            _command = new CommandParser();
            _console = new EnhancedConsole();
            _console.Start();

            Thread.Sleep(200);
            tbxConsole.Text = _console.ReadAll();
            MoveCursorToTheEnd();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(500);
            _timer.Tick += (o, args) =>
                                  {
                                      tbxConsole.Text = _console.ReadAll();
                                      MoveCursorToTheEnd();
                                  };
            _timer.IsEnabled = true;
        }

        private void MoveCursorToTheEnd()
        {
            int lastPosition = tbxConsole.Text.Length;
            tbxConsole.Focus();
            tbxConsole.SelectionStart = lastPosition;
            tbxConsole.SelectionLength = 0;
        }

        private void tbxConsole_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            char key = KeyHelper.GetCharFromKey(e.Key);
            if (e.Key == Key.Return)
            {
                key = Convert.ToChar(13);
            } 
            else if (e.Key == Key.LeftShift)
            {
                return;
            }

            _console.Write(key);
        }

    }
}
