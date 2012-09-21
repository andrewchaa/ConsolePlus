using System;
using System.Runtime.InteropServices;
using System.Threading;
using ConsolePlus.Domain;
using Moq;
using NUnit.Framework;

namespace ConsolePlus.Test
{
    [TestFixture]
    public class TestEnahncedConsole
    {

        [Test]
        public void Should_Read_Output()
        {
            var console = new EnhancedConsole();
            console.Start();

            Assert.That(console.ReadAll(), Is.Not.Null, "Output is null");
            Assert.That(console.ReadAll(), Contains.Substring(Environment.NewLine), "Newline character is not present");

            console.Write('d');
            console.Write('i');
            console.Write('r');
            console.Write((char)13);

            Assert.That(console.ReadAll(), Contains.Substring("dir"), "user command doesn't exist");

            console.Write('e');
            console.Write('x');
            console.Write('i');
            console.Write('t');
            console.Write((char)13);
        }

        [Test]
        public void Should_Get_The_Current_Line_Of_The_Output_Buffer()
        {
            var console = new EnhancedConsole();
            console.Start();

            Assert.That(console.CurrentLine, Is.EqualTo(3));

            Close(console);
        }

        [Test]
        public void Should_Read_Output_Buffer_Incrementally()
        {
            var console = new EnhancedConsole();
            console.Start();

            int currentLine = 0;
            string result = string.Empty;

            if (console.CurrentLine > currentLine)
            {
                result = console.Read(currentLine, console.CurrentLine);
            }
            
            Assert.That(result.Length, Is.GreaterThan(0));

            Close(console);
        }

        private static void Close(EnhancedConsole console)
        {
            console.Write('e');
            console.Write('x');
            console.Write('i');
            console.Write('t');
            console.Write((char) 13);
        }
    }
}
