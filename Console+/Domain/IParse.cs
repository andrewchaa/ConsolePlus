namespace ConsolePlus.Domain
{
    public interface IParse
    {
        Command Parse(string input);
    }
}