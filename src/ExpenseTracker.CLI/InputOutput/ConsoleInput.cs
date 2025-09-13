using ExpenseTracker.CLI.Abstractions;

namespace ExpenseTracker.CLI.InputOutput
{
    public class ConsoleInput : IConsoleInput
    {
        public string? ReadLine()
        {
            string? response = Console.ReadLine();

            if (response == null)
            {
                return string.Empty;
            }
            return response.Trim().ToLowerInvariant();
        }
    }
}
