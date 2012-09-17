using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ConsolePlus.Infrastructure;
using Microsoft.Win32.SafeHandles;
using NUnit.Framework;

namespace ConsolePlus.Test
{
    [TestFixture]
    public class TestConsoleHandle
    {
        private Process _process;

        [SetUp]
        public void Before_Each_Test()
        {
            _process = new Process { StartInfo = { FileName = "cmd.exe" } };
            _process.Start();
            WinCon.AttachConsole(_process.Id);
        }

        [Test]
        public void Attach_Console()
        {
            using (ConsoleScreenBuffer buffer = JConsole.GetActiveScreenBuffer())
            {
                buffer.WriteLine("dir");

                var block = new ConsoleCharInfo[buffer.Height,buffer.Width];
                buffer.ReadBlock(block, 0, 0, 0, 0, buffer.Height, buffer.Width);

            }
        }

        [Test]
        public void Should_Send_Typed_Content_To_Console()
        {
            var buffer = JConsole.GetActiveScreenBuffer();
            
            var ea = new EventArgs[12];
            ea[0] = MakeKeyEvent('H', ConsoleKey.H, 35, true);
            ea[1] = MakeKeyEvent('H', ConsoleKey.H, 35, false);
            ea[2] = MakeKeyEvent('e', ConsoleKey.E, 18, true);
            ea[3] = MakeKeyEvent('e', ConsoleKey.E, 18, false);
            ea[4] = MakeKeyEvent('l', ConsoleKey.L, 38, true);
            ea[5] = MakeKeyEvent('l', ConsoleKey.L, 38, false);
            ea[6] = MakeKeyEvent('l', ConsoleKey.L, 38, true);
            ea[7] = MakeKeyEvent('l', ConsoleKey.L, 38, false);
            ea[8] = MakeKeyEvent('o', ConsoleKey.O, 24, true);
            ea[9] = MakeKeyEvent('o', ConsoleKey.O, 24, false);
            ea[10] = MakeKeyEvent(Convert.ToChar(13), ConsoleKey.Enter, 28, true);
            ea[11] = MakeKeyEvent(Convert.ToChar(13), ConsoleKey.Enter, 28, false);

            var inputBuffer = JConsole.GetInputBuffer();
            inputBuffer.WindowInput = true;
            inputBuffer.WriteEvents(ea);


        }

        static ConsoleKeyEventArgs MakeKeyEvent(char keyChar, ConsoleKey key, int scanCode, bool keyDown)
        {
            ConsoleKeyEventArgs eKey = new ConsoleKeyEventArgs();
            eKey.KeyDown = keyDown;
            eKey.RepeatCount = 1;
            eKey.KeyChar = keyChar;
            eKey.Key = key;
            eKey.VirtualScanCode = scanCode;
            return eKey;
        }


    }
}
