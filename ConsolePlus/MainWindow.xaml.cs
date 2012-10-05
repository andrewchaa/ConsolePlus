using System;
using System.ComponentModel;
using System.Threading;
using System.Timers;
using System.Windows;
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
        private DispatcherTimer _timer;
        private readonly KeyHandler _keyHandler;
        private OffsetColorizer _offsetColorizer;

        public MainWindow()
        {
            InitializeComponent();

            _offsetColorizer = new OffsetColorizer();
            tbxConsole.Focus();
            tbxConsole.TextArea.TextView.LineTransformers.Add(_offsetColorizer);

            _console = new EnhancedConsole();
            _keyHandler = new KeyHandler();

            SetUpdateTimer();
        }

        private void SetUpdateTimer()
        {
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
            tbxConsole.Document.Text = _console.ReadAll();
            tbxConsole.ScrollToEnd();
            _offsetColorizer.StartOffset = 0;
            _offsetColorizer.EndOffset = 200;
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
