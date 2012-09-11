using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using Console_.Domain;

namespace Console_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private TextReader _outputReader;
        private TextReader _errorReader;
        private readonly EnhancedConsole _console;
        private int _lastPosition;
        private IParse _command;

        public MainWindow()
        {
            InitializeComponent();

            _command = new CommandParser();
            _console = new EnhancedConsole(this);
            _console.Start();
        }

        public void UpdateConsole(ProgressChangedEventArgs args)
        {
            if (!(args.UserState is string)) return;
            
            tbxConsole.Text += args.UserState;
            _lastPosition = tbxConsole.Text.ToCharArray().Length;

            tbxConsole.Focus();
            tbxConsole.SelectionStart = _lastPosition;
            tbxConsole.SelectionLength = 0;
        }

        private void TbxConsoleKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var command = _command.Parse(tbxConsole.GetLineText(tbxConsole.LineCount - 1));
                
                _console.Write(command.UserCommand + Environment.NewLine);
                tbxConsole.Text = tbxConsole.Text.Substring(0, tbxConsole.Text.Length - command.UserCommand.Length);

            }
                

        }

    }
}
