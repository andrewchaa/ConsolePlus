using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;

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

        public MainWindow()
        {
            InitializeComponent();

            _console = new EnhancedConsole(this);
            _console.Start();
        }

        public void UpdateConsole(ProgressChangedEventArgs args)
        {
            if (args.UserState is string)
            {
                tbxConsole.Text += args.UserState;
                _lastPosition = tbxConsole.Text.ToCharArray().Length;

                tbxConsole.Focus();
                tbxConsole.SelectionStart = _lastPosition;
                tbxConsole.SelectionLength = 0;
            }
        }

        private void TbxConsoleKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                _console.Write(tbxConsole.GetLineText(tbxConsole.LineCount-1) + Environment.NewLine);

            }
                

        }

    }
}
