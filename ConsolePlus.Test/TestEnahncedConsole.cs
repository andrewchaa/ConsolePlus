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
        

        [Test]
        public void Should_Read_Output()
        {
            var console = new EnhancedConsole();
            console.Start();

            Assert.That(console.ReadAll(), Is.Not.Null, "Output is null");
            Assert.That(console.ReadAll(), Contains.Substring(Environment.NewLine), "Newline character is not present");

            console.Write("dir");
            Assert.That(console.ReadAll(), Contains.Substring("dir"), "user command doesn't exist");

            console.Write("exit");
        }

        [Test]
        public void Should_Write_Login_Credentials_Correctly()
        {
            var console = new EnhancedConsole();
            console.Start();

            console.Write("dir" + Convert.ToChar(13).ToString());

            Thread.Sleep(1000);
//            console.Write("exit" + Convert.ToChar(13).ToString());
        }

    }
}
