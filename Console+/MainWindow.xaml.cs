using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
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
                tbxConsole.Focus();
                tbxConsole.SelectionStart = tbxConsole.Text.ToCharArray().Length;
                tbxConsole.SelectionLength = 0;
            }
        }

        private void TbxConsoleKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string input = e.Key.ToString();
            if (e.Key == Key.Return)
                input = Environment.NewLine;

            _console.Write(input);
        }

    }
}
