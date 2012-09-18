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
            
//            return _process.ReadAll(_process.Id);
            AttachConsole(_process.Id);
            var outputBuffer = JConsole.GetActiveScreenBuffer();

            var block = new ConsoleCharInfo[outputBuffer.Height, outputBuffer.Width];
            outputBuffer.ReadBlock(block, 0, 0, 0, 0, outputBuffer.Height, outputBuffer.Width);

            int emptyLineCount = 0;
            var builder = new StringBuilder(outputBuffer.Width);
            for (int line = 0; line < outputBuffer.Height; line++)
            {
                string text = ReadLine(line, block, outputBuffer);
                if (text.Length == 0)
                {
                    emptyLineCount++;
                    if (emptyLineCount >= 2)
                        break;
                    
                    continue;
                }

                builder.AppendLine(text);
                emptyLineCount = 0;
            }

            return builder.ToString().TrimEnd('\r', '\n');

        }

        private string ReadLine(int line, ConsoleCharInfo[,] block, ConsoleScreenBuffer buffer)
        {
            var stringBuilder = new StringBuilder(buffer.Width);

            for (int i = 0; i < buffer.Width; i++)
            {
                stringBuilder.Append(block[line, i].UnicodeChar);
            }

            return stringBuilder.ToString().TrimEnd();
        }

        public void WriteLine(string command)
        {
            WriteEventsToConsoleProcess(command);
            History.Add(command);
        }


        private void WriteEventsToConsoleProcess(string command)
        {
            var events = new List<EventArgs>();
            foreach (var character in command)
            {
                events.Add(new ConsoleKeyEventArgs {KeyChar = character, KeyDown = true, RepeatCount = 1});
                events.Add(new ConsoleKeyEventArgs {KeyChar = character, KeyDown = false, RepeatCount = 1});
            }

            var inputBuffer = JConsole.GetInputBuffer();
            inputBuffer.WindowInput = true;
            inputBuffer.WriteEvents(events.ToArray(), events.Count);
        }
    }
}
