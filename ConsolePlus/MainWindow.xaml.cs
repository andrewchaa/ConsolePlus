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
            tbxConsole.Document.Text = _console.ReadAll();
            tbxConsole.ScrollToEnd();
            tbxConsole.TextArea.TextView.LineTransformers.Add(new OffsetColorizer());
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
        protected override void ColorizeLine(DocumentLine line)
        {
            if (line.Length == 0)
                return;

            if (line.Offset < 2 || line.Offset > 20)
                return;

            ChangeLinePart(
                2,
                20,
                element =>
                {
                    Typeface tf = element.TextRunProperties.Typeface;
                    element.TextRunProperties.SetForegroundBrush(Brushes.Red);
                    element.TextRunProperties.SetTypeface(new Typeface(
                        tf.FontFamily,
                        FontStyles.Italic,
                        FontWeights.Bold,
                        tf.Stretch
                    ));
                });
        }
    }
}
