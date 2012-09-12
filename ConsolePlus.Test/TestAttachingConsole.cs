using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ConsolePlus.Infrastructure;
using NUnit.Framework;

namespace ConsolePlus.Test
{
    [TestFixture]
    public class TestAttachingConsole
    {
        [Test]
        public void Attach_Console()
        {
            var process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.Start();

            WinCon.AttachConsole(process.Id);

            IntPtr hConsoleOutput = WinCon.GetStdHandle(WinCon.STD_OUTPUT_HANDLE);
            
            using (ConsoleScreenBuffer buffer = JConsole.GetActiveScreenBuffer())
            {
//                buffer.SetBufferSize(100, 100);
//                buffer.SetWindowSize(buffer.MaximumWindowWidth, buffer.MaximumWindowHeight);

                buffer.WriteLine("dir");

                var block = new ConsoleCharInfo[buffer.Height,buffer.Width];
                buffer.ReadBlock(block, 0, 0, 0, 0, buffer.Height, buffer.Width);
            }

//            var bufferInfo = new ConsoleScreenBufferInfo();
//            WinCon.GetConsoleScreenBufferInfo(hConsoleOutput, bufferInfo);
//            WinCon.SetConsoleScreenBufferSize(hConsoleOutput, new Coord(200, 200));

            
        }

    }
}
