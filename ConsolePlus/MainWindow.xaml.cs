using System;
using System.ComponentModel;
using System.Threading;
using System.Timers;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
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

        public MainWindow()
        {
            InitializeComponent();
            
            tbxConsole.Focus();
            tbxConsole.TextArea.TextEntering += TextEntering;

            _console = new EnhancedConsole();

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
        }

        private void TextEntering(object sender, TextCompositionEventArgs e)
        {
            char key = e.Text[0];
            if (key == '\n')
                key = (char) 13;

            _console.Write(key);
            e.Handled = true; // Stop keys from being typed into textbox
            UpdateConsole();
        }

    }
}
