namespace Console_.Domain
{
    public interface IConsoleProcess
    {
        int Read(char[] buffer, int index, int size);
        void Start();
        void Write(string command);
    }
}