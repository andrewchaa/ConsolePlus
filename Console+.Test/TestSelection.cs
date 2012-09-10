using Console_.Domain;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace Console_.Test
{
    [TestFixture]
    public class TestSelection
    {
        [Test]
        public void Should_Return_Everything_after_First_GreaterThan()
        {  
            string theLastLine = @"C:\User\sandrwe.chaa\Documents\Projects\Console+>dir";

            var command = new Command();
            string userTyping = command.Parse(theLastLine);

            Assert.That("dir", Is.EqualTo(userTyping));
        }
    }
}