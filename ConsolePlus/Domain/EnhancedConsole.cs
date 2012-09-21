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

            History = new CommandHistory();
        }

        public CommandHistory History { get; private set; }

        public int CurrentLine
        {
            get
            {
                AttachConsole(_process.Id);
                _outputBuffer = JConsole.GetActiveScreenBuffer();
                return _outputBuffer.CursorTop;
            }
        }

        public void Start()
        {
            _process.Start();
        }

        public string ReadAll()
        {
            AttachConsole(_process.Id);

            var buffer = JConsole.GetActiveScreenBuffer();
            int height = buffer.CursorTop + 1;
            int width = buffer.Width;

            var block = GetBlock(buffer, height, width);

            return GetContent(block, width, height);
        }

        public string Read(int start, int end)
        {
            var buffer = new ConsoleCharInfo[end - start + 1, _outputBuffer.Width];
            _outputBuffer.ReadBlock(buffer, 0, 0, 0, start, _outputBuffer.Width - 1, end);
            
            return GetContent(buffer, _outputBuffer.Width - 1, end-start + 1);
            
        }

        private static string GetContent(ConsoleCharInfo[,] block, int width, int height)
        {
            var builder = new StringBuilder(height*width);
            for (int line = 0; line < height; line++)
            {
                builder.Append(Environment.NewLine);
                for (int i = 0; i < width; i++)
                {
                    builder.Append(block[line, i].UnicodeChar);
                }
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
