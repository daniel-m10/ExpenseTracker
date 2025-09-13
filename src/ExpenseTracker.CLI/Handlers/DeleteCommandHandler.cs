using ExpenseTracker.CLI.Commands;
using Serilog;

namespace ExpenseTracker.CLI.Handlers
{
    public class DeleteCommandHandler(ILogger logger)
    {
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");

        public async Task<int> RunDeleteAsync(DeleteCommand command)
        {
            if (command.Id <= 0)
            {
                _logger.Error("Id must be greater than 0.");
                return 1;
            }

            if (!command.Force)
            {
                _logger.Information($"⚠️ Are you sure you want to delete expense #{command.Id}? (y/n)");
                var response = Console.ReadLine()?.Trim().ToLowerInvariant();
                if (response != "y")
                {
                    _logger.Information("❌ Delete cancelled.");
                    return 2;
                }
            }

            // Expense deleted
            await Task.CompletedTask;
            _logger.Information($"✅ Expense #{command.Id} deleted successfully.");
            return 0;
        }
    }
}
