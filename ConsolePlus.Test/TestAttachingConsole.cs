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
    public class TestAttachingConsole
    {
        [Test]
        public void Attach_Console()
        {
            var process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.Start();

            WinCon.AttachConsole(process.Id);
                   
            

//            using (ConsoleScreenBuffer buffer = JConsole.GetActiveScreenBuffer())
            using (var buffer = new ConsoleScreenBuffer())
            {
                JConsole.SetActiveScreenBuffer(buffer);
                buffer.WriteLine("dir");

                var block = new ConsoleCharInfo[buffer.Height,buffer.Width];
                buffer.ReadBlock(block, 0, 0, 0, 0, buffer.Height, buffer.Width);


            }
        }

    }
}
