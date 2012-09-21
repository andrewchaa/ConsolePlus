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
    public class TestConsoleApi
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AttachConsole(int dwProcessId);

        [Test]
        public void Write_To_Buffer()
        {
            var process = new Process { StartInfo = { FileName = "cmd.exe" } };
            process.Start();

            AttachConsole(process.Id);
            var buffer = JConsole.GetActiveScreenBuffer();
            buffer.WriteLine("dir");

            var block = new ConsoleCharInfo[buffer.Height,buffer.Width];
            buffer.ReadBlock(block, 0, 0, 0, 0, buffer.Height, buffer.Width);

        }

        [Test]
        public void Buffer_States()
        {
            var process = new Process { StartInfo = { FileName = "cmd.exe" } };
            process.Start();

            AttachConsole(process.Id);
            var buffer = JConsole.GetActiveScreenBuffer();

            Assert.That(buffer.CursorTop, Is.EqualTo(3));
            Assert.That(buffer.CursorLeft, Is.GreaterThan(0));

            var events = new List<EventArgs>();
            events.Add(new ConsoleKeyEventArgs { KeyDown = true, RepeatCount = 1, KeyChar = 'd' });
            events.Add(new ConsoleKeyEventArgs { KeyDown = false, RepeatCount = 1, KeyChar = 'd' });
            events.Add(new ConsoleKeyEventArgs { KeyDown = true, RepeatCount = 1, KeyChar = 'i' });
            events.Add(new ConsoleKeyEventArgs { KeyDown = false, RepeatCount = 1, KeyChar = 'i' });
            events.Add(new ConsoleKeyEventArgs { KeyDown = true, RepeatCount = 1, KeyChar = 'r' });
            events.Add(new ConsoleKeyEventArgs { KeyDown = false, RepeatCount = 1, KeyChar = 'r' });
            events.Add(new ConsoleKeyEventArgs { KeyDown = true, RepeatCount = 1, KeyChar = (char)13 });
            events.Add(new ConsoleKeyEventArgs { KeyDown = false, RepeatCount = 1, KeyChar = (char)13 });

            var inputBuffer = JConsole.GetInputBuffer();
            inputBuffer.WindowInput = true;
            inputBuffer.WriteEvents(events, events.Count());

            buffer.WriteLine(buffer.OutputMode.ToString());

        }

        [Test]
        public void Should_Send_Typed_Content_To_Console()
        {
            var process = new Process { StartInfo = { FileName = "cmd.exe" } };
            process.Start();

            AttachConsole(process.Id);
            var buffer = JConsole.GetActiveScreenBuffer();
            
            var events = new List<EventArgs>();
            events.Add(new ConsoleKeyEventArgs {KeyDown = true, RepeatCount = 1, KeyChar = 'd'});
            events.Add(new ConsoleKeyEventArgs {KeyDown = false, RepeatCount = 1, KeyChar = 'd'});
            events.Add(new ConsoleKeyEventArgs {KeyDown = true, RepeatCount = 1, KeyChar = 'i'});
            events.Add(new ConsoleKeyEventArgs {KeyDown = false, RepeatCount = 1, KeyChar = 'i'});
            events.Add(new ConsoleKeyEventArgs {KeyDown = true, RepeatCount = 1, KeyChar = 'r'});
            events.Add(new ConsoleKeyEventArgs {KeyDown = false, RepeatCount = 1, KeyChar = 'r'});
            events.Add(new ConsoleKeyEventArgs {KeyDown = true, RepeatCount = 1, KeyChar = (char)13});
            events.Add(new ConsoleKeyEventArgs {KeyDown = false, RepeatCount = 1, KeyChar = (char)13});

            var inputBuffer = JConsole.GetInputBuffer();
            inputBuffer.WindowInput = true;
            inputBuffer.WriteEvents(events, events.Count());

            

        }
    }
}
