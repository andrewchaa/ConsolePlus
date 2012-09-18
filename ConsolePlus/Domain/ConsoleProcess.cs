using System;
using System.Diagnostics;
using System.Text;
using ConsolePlus.Infrastructure;

namespace ConsolePlus.Domain
{
    public class ConsoleProcess : IConsoleProcess
    {
        private readonly Process _process;
        private ConsoleScreenBuffer _screenBufferbuffer;

        public ConsoleProcess()
        {
            var info = new ProcessStartInfo("cmd.exe", string.Empty)
            {
//                UseShellExecute = false,
//                ErrorDialog = false,
//                CreateNoWindow = true,
//                RedirectStandardError = true,
//                RedirectStandardInput = true,
//                RedirectStandardOutput = true
            };

            _process = new Process { EnableRaisingEvents = true, StartInfo = info, };
        }

        public int Read(char[] buffer, int index, int size)
        {
            return _process.StandardOutput.Read(buffer, 0, 1024);
        }

        public string Read1()
        {

            WinCon.AttachConsole(_process.Id);
            _screenBufferbuffer = JConsole.GetActiveScreenBuffer();

            var block = new ConsoleCharInfo[_screenBufferbuffer.Height, _screenBufferbuffer.Width];
            _screenBufferbuffer.ReadBlock(block, 0, 0, 0, 0, _screenBufferbuffer.Height, _screenBufferbuffer.Width);

            var builder = new StringBuilder(_screenBufferbuffer.Width);
            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < _screenBufferbuffer.Width; i++)
                {
                    builder.Append(block[j, i].UnicodeChar);
                }
            }

            return builder.ToString();
        }

        public void Start()
        {
            _process.Start();
        }

        public void Write(string command)
        {
            var buffer = JConsole.GetActiveScreenBuffer();

            var ea = new EventArgs[8];
            ea[0] = MakeKeyEvent('d', ConsoleKey.D, 35, true);
            ea[1] = MakeKeyEvent('d', ConsoleKey.D, 35, false);
            ea[2] = MakeKeyEvent('i', ConsoleKey.I, 18, true);
            ea[3] = MakeKeyEvent('i', ConsoleKey.I, 18, false);
            ea[4] = MakeKeyEvent('r', ConsoleKey.R, 38, true);
            ea[5] = MakeKeyEvent('r', ConsoleKey.R, 38, false);
            ea[6] = MakeKeyEvent(Convert.ToChar(13), ConsoleKey.Enter, 28, true);
            ea[7] = MakeKeyEvent(Convert.ToChar(13), ConsoleKey.Enter, 28, false);

            var inputBuffer = JConsole.GetInputBuffer();
            inputBuffer.WindowInput = true;
            inputBuffer.WriteEvents(ea);
        }

        private ConsoleKeyEventArgs MakeKeyEvent(char keyChar, ConsoleKey key, int scanCode, bool keyDown)
        {
            var eKey = new ConsoleKeyEventArgs
                           {
                               KeyDown = keyDown,
                               RepeatCount = 1,
                               KeyChar = keyChar,
                               Key = key,
                               VirtualScanCode = scanCode
                           };
            return eKey;
        }

    }
}
