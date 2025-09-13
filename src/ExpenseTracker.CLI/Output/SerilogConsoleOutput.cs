using ExpenseTracker.CLI.Abstractions;
using Serilog;

namespace ExpenseTracker.CLI.Output
{
    public class SerilogConsoleOutput(ILogger logger) : IConsoleOutput
    {
        private readonly ILogger _logger = logger;

        public void WriteError(string message) => _logger.Error(message);

        public void WriteLine(string message) => _logger.Information(message);
    }
}
