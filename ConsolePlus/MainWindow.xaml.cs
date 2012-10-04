using System;
using System.ComponentModel;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ConsolePlus.Domain;
using ConsolePlus.Infrastructure;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

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

    public class OffsetColorizer : DocumentColorizingTransformer
    {
        public int StartOffset { get; set; }
        public int EndOffset { get; set; }

        protected override void ColorizeLine(DocumentLine line)
        {
            if (line.Length == 0)
                return;

            if (line.Offset < StartOffset || line.Offset > EndOffset)
                return;

            int start = line.Offset > StartOffset ? line.Offset : StartOffset;
            int end = EndOffset > line.EndOffset ? line.EndOffset : EndOffset;

            ChangeLinePart(start, end, element => element.TextRunProperties.SetForegroundBrush(Brushes.Red));
        }
    }
}
