namespace ExpenseTracker.CLI.Abstractions
{
    public interface IConsoleOutput
    {
        void WriteLine(string message);
        void WriteError(string message);
    }
}
