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
        public void Should_Read_Output_To_The_End()
        {
            var console = new EnhancedConsole();

            Assert.That(console.ReadAll(), Is.Not.Null, "Output is null");

            console.Write('d');
            console.Write('i');
            console.Write('r');
            console.Write((char)13);

            Assert.That(console.ReadAll(), Contains.Substring("dir"), "user command doesn't exist");

            Close(console);
        }

        [Test]
        public void ReadAll_Should_Truncate_Empty_Lines()
        {
            var console = new EnhancedConsole();

            string content = console.ReadAll();
            Assert.That(content.Split(new string[] {"\r\n"}, StringSplitOptions.None).Length, Is.EqualTo(5));

            Close(console);
        }

        [Test]
        public void ReadAll_Should_Read_The_Last_Line_Without_Empty_Spaces()
        {
            var console = new EnhancedConsole();

            string content = console.ReadAll();
            Assert.That(content.Substring(content.Length-1, 1), Is.EqualTo(">"));

            Close(console);
        }


        [Test]
        public void Content_Changed_Becomes_True_When_Buffer_Has_New_Content()
        {
            var console = new EnhancedConsole();

            Assert.That(console.ContentChanged, Is.True);
            Assert.That(console.ContentChanged, Is.False);

            console.Write('d');
            console.Write('i');
            console.Write('r');
            console.Write((char)13);

            Assert.That(console.ContentChanged, Is.True);

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
