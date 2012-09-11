using Console_.Domain;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace Console_.Test
{
    [TestFixture]
    public class TestCommandParsing
    {
        private const string TheLastLine = @"C:\User\sandrwe.chaa\Documents\Projects\Console+>dir";

        [Test]
        public void Should_Return_User_Command()
        {
            IParse commandParser = new CommandParser();
            var command = commandParser.Parse(TheLastLine);

            Assert.That("dir", Is.EqualTo(command.UserCommand));
        }

        [Test]
        public void Should_Return_Screen_Text()
        {
            IParse commandParser = new CommandParser();
            var command = commandParser.Parse(TheLastLine);

            Assert.That(@"C:\User\sandrwe.chaa\Documents\Projects\Console+>", Is.EqualTo(command.ScreenText));
        }
    }
}