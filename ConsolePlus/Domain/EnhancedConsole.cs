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

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AttachConsole(int dwProcessId);

        public EnhancedConsole()
        {
            var info = new ProcessStartInfo("cmd.exe", String.Empty)
            {
                //                UseShellExecute = false,
                //                ErrorDialog = false,
                //                CreateNoWindow = true,
                //                RedirectStandardError = true,
                //                RedirectStandardInput = true,
                //                RedirectStandardOutput = true
            };

            _process = new Process { EnableRaisingEvents = true, StartInfo = info, };
            
            History = new CommandHistory();
        }

        public CommandHistory History { get; private set; }

        public void Start()
        {
            _process.Start();
            AttachConsole(_process.Id);
        }

        public string ReadAll()
        {
            AttachConsole(_process.Id);
            var buffer = JConsole.GetActiveScreenBuffer();

            int height = buffer.CursorTop + 1;
            int width = buffer.Width;

            Console.WriteLine(buffer.CursorTop);
            Console.WriteLine(buffer.OutputMode.ToString());
            Console.WriteLine(buffer.ProcessedOutput);

            var block = new ConsoleCharInfo[height, width];
            buffer.ReadBlock(block, 0, 0, 0, 0, width, height);

            var builder = new StringBuilder(height * width);
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
