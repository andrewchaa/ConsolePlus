using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ConsolePlus.Domain;
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
            
            var events = new List<EventArgs>();
            events.Add(MakeKeyEvent('H', ConsoleKey.H, true));
            events.Add(MakeKeyEvent('H', ConsoleKey.H, false));
            events.Add(MakeKeyEvent('e', ConsoleKey.E, true));
            events.Add(MakeKeyEvent('e', ConsoleKey.E, false));
            events.Add(MakeKeyEvent('l', ConsoleKey.L, true));
            events.Add(MakeKeyEvent('l', ConsoleKey.L, false));
            events.Add(MakeKeyEvent('l', ConsoleKey.L, true));
            events.Add(MakeKeyEvent('l', ConsoleKey.L, false));
            events.Add(MakeKeyEvent('o', ConsoleKey.O, true));
            events.Add(MakeKeyEvent('o', ConsoleKey.O, false));
            events.Add(MakeKeyEvent(Convert.ToChar(13), ConsoleKey.Enter, true));
            events.Add(MakeKeyEvent(Convert.ToChar(13), ConsoleKey.Enter, false));

            var inputBuffer = JConsole.GetInputBuffer();
            inputBuffer.WindowInput = true;
            inputBuffer.WriteEvents(events, events.Count());


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
