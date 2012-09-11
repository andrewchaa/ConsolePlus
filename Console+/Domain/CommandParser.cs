using System.Text.RegularExpressions;

namespace ConsolePlus.Domain
{
    public class CommandParser : IParse
    {
        public Command Parse(string theLastLine)
        {
            var pattern = new Regex(@"(?<Text>.+)?>(?<Command>.+?)$");
            var match = pattern.Match(theLastLine);

            var command = new Command { ScreenText = match.Groups["Text"].Value + ">", UserCommand = match.Groups["Command"].Value };

            return command;
        }
    }
}
