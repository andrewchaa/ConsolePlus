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
            ea[0] = MakeKeyEvent('H', ConsoleKey.H, true);
            ea[1] = MakeKeyEvent('H', ConsoleKey.H, false);
            ea[2] = MakeKeyEvent('e', ConsoleKey.E, true);
            ea[3] = MakeKeyEvent('e', ConsoleKey.E, false);
            ea[4] = MakeKeyEvent('l', ConsoleKey.L, true);
            ea[5] = MakeKeyEvent('l', ConsoleKey.L, false);
            ea[6] = MakeKeyEvent('l', ConsoleKey.L, true);
            ea[7] = MakeKeyEvent('l', ConsoleKey.L, false);
            ea[8] = MakeKeyEvent('o', ConsoleKey.O, true);
            ea[9] = MakeKeyEvent('o', ConsoleKey.O, false);
            ea[10] = MakeKeyEvent(Convert.ToChar(13), ConsoleKey.Enter, true);
            ea[11] = MakeKeyEvent(Convert.ToChar(13), ConsoleKey.Enter, false);

            var inputBuffer = JConsole.GetInputBuffer();
            inputBuffer.WindowInput = true;
            inputBuffer.WriteEvents(ea);


        }

        static ConsoleKeyEventArgs MakeKeyEvent(char keyChar, ConsoleKey key, bool keyDown)
        {
            var eKey = new ConsoleKeyEventArgs();
            eKey.KeyDown = keyDown;
            eKey.RepeatCount = 1;
            eKey.KeyChar = keyChar;
            eKey.Key = key;
            return eKey;
        }
    }
}
