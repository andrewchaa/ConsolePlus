using System.ComponentModel;

namespace ConsolePlus.Domain
{
    public interface IConsoleWindow
    {
        void UpdateConsole(ProgressChangedEventArgs args);
    }
}