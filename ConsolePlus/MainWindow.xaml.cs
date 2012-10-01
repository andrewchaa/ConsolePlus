using System;
using System.ComponentModel;
using System.Threading;
using System.Timers;
using System.Windows.Documents;
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
        private readonly DispatcherTimer _timer;
        private readonly KeyHandler _keyHandler;

        public MainWindow()
        {
            InitializeComponent();
            tbxConsole.Focus();

            _console = new EnhancedConsole();
            _keyHandler = new KeyHandler();

            Thread.Sleep(200);

            _timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(500)};
            _timer.Tick += (o, args) =>
                {
                    if (!_console.ContentChanged)
                        return;

                    UpdateConsole();
                };
            _timer.IsEnabled = true;
        }

        public void UpdateConsole()
        {
            new TextRange(tbxConsole.Document.ContentStart, tbxConsole.Document.ContentEnd)
                {
                    Text = _console.ReadAll()
                };

            tbxConsole.CaretPosition = tbxConsole.CaretPosition.DocumentEnd;
        }

        private void TbxConsolePreviewKeyDown(object sender, KeyEventArgs e)
        {
            char key = _keyHandler.GetCharacterFrom(e.Key);
            if (key == char.MinValue)
                return;

            _console.Write(key);
            e.Handled = true; // Stop keys from being typed into textbox
            UpdateConsole();
        }

    }
}
