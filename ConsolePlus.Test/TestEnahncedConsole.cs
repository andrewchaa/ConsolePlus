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


    }
}
