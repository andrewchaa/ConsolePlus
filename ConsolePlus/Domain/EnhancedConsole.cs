using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            _process = CreateProcess();
        }

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
            _outputBuffer = GetActiveScreenBuffer();

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
            if (width <= 0)
                return string.Empty;

            var buffer = new ConsoleCharInfo[1, width];
            _outputBuffer.ReadBlock(buffer, 0, 0, 0, lineNumber, width-1, lineNumber);

            return GetContent(buffer, width, 1);
        }

        private Process CreateProcess()
        {
            var process = new Process
                              {
                                  EnableRaisingEvents = true,
                                  StartInfo = new ProcessStartInfo("cmd.exe", String.Empty)
                                                  {
                                                      UseShellExecute = false,
                                                      ErrorDialog = false,
                                                      CreateNoWindow = true
                                                  }
                              };

            process.Start();

            return process;
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

        public void Write(char key)
        {
            AttachConsole(_process.Id);

            WriteEventsToConsoleProcess(key);
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

        /// <summary>
        /// Opens the currently active screen buffer.
        /// </summary>
        /// <returns>A new <see cref="ConsoleScreenBuffer" /> instance that references the currently active
        /// console screen buffer.</returns>
        /// <remarks>This method allocates a new ConsoleScreenBuffer instance.  Callers should
        /// call Dispose on the returned instance when they're done with it.</remarks>
        private ConsoleScreenBuffer GetActiveScreenBuffer()
        {
            // CONOUT$ always references the current active screen buffer.
            // NOTE:  *MUST* specify GENERIC_READ | GENERIC_WRITE.  Otherwise
            // the console API calls will fail with Win32 error INVALID_HANDLE_VALUE.
            // Also must include the file sharing flags or CreateFile will fail.
            IntPtr outHandle = WinApi.CreateFile("CONOUT$",
                WinApi.GENERIC_READ | WinApi.GENERIC_WRITE,
                WinApi.FILE_SHARE_READ | WinApi.FILE_SHARE_WRITE,
                null,
                WinApi.OPEN_EXISTING,
                0,
                IntPtr.Zero);
            if (outHandle.ToInt32() == WinApi.INVALID_HANDLE_VALUE)
            {
                throw new IOException("Unable to open CONOUT$", Marshal.GetLastWin32Error());
            }

            return new ConsoleScreenBuffer(outHandle) { ownsHandle = true };
        }

    }
}
