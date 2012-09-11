using System.ComponentModel;

namespace Console_.Domain
{
    public interface IConsoleWindow
    {
        void UpdateConsole(ProgressChangedEventArgs args);
    }
}