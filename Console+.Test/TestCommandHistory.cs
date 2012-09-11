using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Console_.Domain;
using Moq;
using NUnit.Framework;

namespace Console_.Test
{
    [TestFixture]
    public class TestCommandHistory
    {
        private Mock<IConsoleWindow> _window;
        private Mock<IConsoleProcess> _process;
        private EnhancedConsole _console;

        [SetUp]
        public void Before_Each_Test()
        {
            _window = new Mock<IConsoleWindow>();
            _process = new Mock<IConsoleProcess>();
            _console = new EnhancedConsole(_window.Object, _process.Object);
        }

        [Test]
        public void Pressing_Enter_Stores_The_Command()
        {
            _console.Write("A");
            _console.Write("B");

            Assert.That(_console.History.Count, Is.EqualTo(2));
        }

        [Test]
        public void Last_Command_Comes_Up_First()
        {
            _console.Write("A");
            _console.Write("B");

            Assert.That(_console.History.Get(), Is.EqualTo("B"));
            Assert.That(_console.History.Get(), Is.EqualTo("A"));
        }

        [Test]
        public void Command_Should_Not_Have_New_Line_Character()
        {
            _console.Write("A" + Environment.NewLine);
            _console.Write("B" + Environment.NewLine);

            Assert.That(_console.History.Get(), Is.EqualTo("B"));
        }

    }
}
