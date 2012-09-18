using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsolePlus.Domain;
using Moq;
using NUnit.Framework;

namespace ConsolePlus.Test
{
    [TestFixture]
    public class TestEnahncedConsole
    {
        [Test]
        public void Should_Write_Typed_Commands_To_Console_And_Execute_It()
        {
            var mockWindow = new Mock<IConsoleWindow>();
            var console = new EnhancedConsole(mockWindow.Object, new ConsoleProcess());
            console.Start();

            console.Write("dir");

            
        }
    }
}
