using System;
using System.Threading;
using ConsolePlus.Domain;
using Moq;
using NUnit.Framework;

namespace ConsolePlus.Test
{
    [TestFixture]
    public class TestEnahncedConsole
    {
        private EnhancedConsole _console;

        [Test]
        public void Should_Read_Output()
        {
            _console = new EnhancedConsole();
            _console.Start();

            Assert.That(_console.ReadAll(), Is.Not.Null, "Output is null");
            Assert.That(_console.ReadAll(), Contains.Substring(Environment.NewLine), "Newline character is not present");

            _console.WriteLine("dir");
            Assert.That(_console.ReadAll(), Contains.Substring("dir"), "user command doesn't exist");

            _console.WriteLine("exit");
        }

    }
}
