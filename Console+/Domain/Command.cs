using System.Text.RegularExpressions;

namespace Console_.Domain
{
    public class Command
    {
        public string Parse(string theLastLine)
        {
            var pattern = new Regex(@">(.+)$");

            return pattern.Match(theLastLine).Groups[1].Value;
        }
    }
}
