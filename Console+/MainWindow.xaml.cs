using System;
using System.ComponentModel;
using System.Windows.Input;
using ConsolePlus.Domain;

namespace ConsolePlus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IConsoleWindow
    {
        private readonly EnhancedConsole _console;
        private readonly IParse _command;

        public MainWindow()
        {
            InitializeComponent();

            _command = new CommandParser();
            _console = new EnhancedConsole(this, new ConsoleProcess());
            _console.Start();
        }

        public void UpdateConsole(ProgressChangedEventArgs args)
        {
            if (!(args.UserState is string)) return;
            
            tbxConsole.Text += args.UserState;

            MoveCursorToTheEnd();
        }

        private void MoveCursorToTheEnd()
        {
            int lastPosition = tbxConsole.Text.Length;
            tbxConsole.Focus();
            tbxConsole.SelectionStart = lastPosition;
            tbxConsole.SelectionLength = 0;
        }

        private void TbxConsoleKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var commandText = _command.Parse(tbxConsole.GetLineText(tbxConsole.LineCount - 1));
                
                _console.Write(commandText.UserCommand + Environment.NewLine);
                tbxConsole.Text = tbxConsole.Text.Substring(0, tbxConsole.Text.Length - commandText.UserCommand.Length);
            }
        }

        private void tbxConsole_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                var commandText = _command.Parse(tbxConsole.GetLineText(tbxConsole.LineCount - 1));
                tbxConsole.Text = tbxConsole.Text.Substring(0, tbxConsole.Text.Length - commandText.UserCommand.Length) +
                    _console.History.Get();

                MoveCursorToTheEnd();
                e.Handled = true;
            } 
        }

    }
}
