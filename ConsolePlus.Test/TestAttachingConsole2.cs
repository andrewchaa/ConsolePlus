using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;
using NUnit.Framework;

namespace ConsolePlus.Test
{
    [TestFixture]
    public class TestAttachingConsole2
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("kernel32.dll",
            EntryPoint = "GetStdHandle",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32", SetLastError = true)]
        static extern bool AttachConsole(uint dwProcessId);

        [DllImport("kernel32.dll",
            EntryPoint = "AllocConsole",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool FreeConsole();

        private const int STD_OUTPUT_HANDLE = -11;
        private const int STD_ERROR_HANDLE = -12;
        private static bool _consoleAttached = false;
        private static IntPtr consoleWindow;

        [Test]
        public void Test_Attach_Console_Handle()
        {

            var process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.Start();

            if (AttachConsole((uint)process.Id))
            {
                _consoleAttached = true;
                var stdHandle = GetStdHandle(STD_ERROR_HANDLE); // must be error dunno why
                var safeFileHandle = new SafeFileHandle(stdHandle, true);
                var fileStream = new FileStream(safeFileHandle, FileAccess.Write);
                var encoding = Encoding.ASCII;
                var standardOutput = new StreamWriter(fileStream, encoding) {AutoFlush = true};
                var standardInput = new StreamReader(fileStream, encoding);
                
                Console.SetOut(standardOutput);
                Console.WriteLine(MediaTypeNames.Application.Rtf + " was launched from a console window and will redirect output to it.");

                string text = standardInput.ReadLine();
            }
            // ... do whatever, use console.writeline or debug.writeline
            // if you started the app with /debug from a console

            

            Cleanup();

        }

        private static void Cleanup()
        {
            try
            {
                if (_consoleAttached)
                {
                    SetForegroundWindow(consoleWindow);
//                    SendKeys.SendWait("{ENTER}");
                    FreeConsole();
                }
            } catch {}
        }
    }
}
