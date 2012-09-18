using System;
using ConsolePlus.Domain;
using Moq;
using NUnit.Framework;

namespace ConsolePlus.Test
{
    [TestFixture]
    public class TestCommandHistory
    {
        private EnhancedConsole _console;

        [SetUp]
        public void Before_Each_Test()
        {
            _console = new EnhancedConsole();
        }

        [Test]
        public void Pressing_Enter_Stores_The_Command()
        {
            _console.WriteLine("A");
            _console.WriteLine("B");

            Assert.That(_console.History.Count, Is.EqualTo(2));
        }

        [Test]
        public void Last_Command_Comes_Up_First()
        {
            _console.WriteLine("A");
            _console.WriteLine("B");

            Assert.That(_console.History.Get(), Is.EqualTo("B"));
            Assert.That(_console.History.Get(), Is.EqualTo("A"));
        }

        [Test]
        public void Command_Should_Not_Have_New_Line_Character()
        {
            _console.WriteLine("A" + Environment.NewLine);
            _console.WriteLine("B" + Environment.NewLine);

            Assert.That(_console.History.Get(), Is.EqualTo("B"));
        }

    }
}
