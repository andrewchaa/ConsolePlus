using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using ConsolePlus.Infrastructure;

namespace ConsolePlus.Domain
{
    public class EnhancedConsole
    {
        private readonly Process _process;
        private ConsoleScreenBuffer _outputBuffer;

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AttachConsole(int dwProcessId);

        public EnhancedConsole()
        {
            _process = new Process
                           {
                               EnableRaisingEvents = true,
                               StartInfo = new ProcessStartInfo("cmd.exe", String.Empty) 
                                               {
                                                   //                                UseShellExecute = false,
                                                   //                                ErrorDialog = false,
                                                   //                                CreateNoWindow = true
                                               }
                           };

            _process.Start();
            History = new CommandHistory();
        }

        public CommandHistory History { get; private set; }

        private string _oldLastLine = string.Empty;
        public bool ContentChanged
        {
            get
            {
                AttachConsole(_process.Id);
                _outputBuffer = JConsole.GetActiveScreenBuffer();

                string lastLine = Read(_outputBuffer.CursorTop, _outputBuffer.Width);
                if (String.Compare(_oldLastLine, lastLine, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    _oldLastLine = lastLine;
                    return true;
                }
                
                return false;
            }
        }

        public string ReadAll()
        {
            AttachConsole(_process.Id);
            _outputBuffer = JConsole.GetActiveScreenBuffer();

            var builder = new StringBuilder(_outputBuffer.Width * _outputBuffer.Height);
            for (int i = 0; i < _outputBuffer.CursorTop + 1; i++)
            {
                bool isLastLine = i == _outputBuffer.CursorTop;
                if (isLastLine)
                {
                    builder.Append(Read(i, _outputBuffer.CursorLeft));
                    return builder.ToString();
                }

                builder.Append(Read(i, _outputBuffer.Width));
            }

            return builder.ToString();

        }

        private string Read(int lineNumber, int width)
        {
            var buffer = new ConsoleCharInfo[1, width];
            _outputBuffer.ReadBlock(buffer, 0, 0, 0, lineNumber, width-1, lineNumber);

            return GetContent(buffer, width, 1);
        }

        private static string GetContent(ConsoleCharInfo[,] block, int width, int height)
        {
            var builder = new StringBuilder(width + 2);
            builder.Append(Environment.NewLine);
            for (int i = 0; i < width; i++)
            {
                builder.Append(block[0, i].UnicodeChar);
            }

            return builder.ToString();
        }

        private ConsoleCharInfo[,] GetBlock(ConsoleScreenBuffer buffer, int height, int width)
        {
            var block = new ConsoleCharInfo[height,width];
            buffer.ReadBlock(block, 0, 0, 0, 0, width, height);
            return block;
        }

        public void Write(char key)
        {
            WriteEventsToConsoleProcess(key);
//            History.Add(key);
        }


        private void WriteEventsToConsoleProcess(char key)
        {
            var events = new List<EventArgs>
                             {
                                 new ConsoleKeyEventArgs {KeyChar = key, KeyDown = true, RepeatCount = 1},
                                 new ConsoleKeyEventArgs {KeyChar = key, KeyDown = false, RepeatCount = 1}
                             };

            var inputBuffer = JConsole.GetInputBuffer();
            inputBuffer.WindowInput = true;
            inputBuffer.WriteEvents(events.ToArray(), events.Count);
        }
    }
}
