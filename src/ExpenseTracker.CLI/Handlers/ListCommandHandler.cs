using ExpenseTracker.CLI.Commands;
using Serilog;

namespace ExpenseTracker.CLI.Handlers
{
    public class ListCommandHandler(ILogger logger)
    {
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");

        public async Task<int> RunListAsync(ListCommand command)
        {
            // Defaults
            var now = DateTime.UtcNow;
            int year = command.Year ?? now.Year;
            int month = command.Month ?? now.Month;
            int limit = command.Limit ?? 1;

            if (month < 1 || month > 12)
            {
                _logger.Error("Month must be between 1 and 12.");
                return 1;
            }

            if (year < 2025)
            {
                _logger.Error("Year must be greater or equal than 2025.");
                return 1;
            }

            if (limit <= 0)
            {
                _logger.Error("Limit must be greater than 0.");
                return 1;
            }

            // Simulate that task was completed
            await Task.CompletedTask;

            _logger.Information($"Listing expenses | Year: {year} | Month: {month} | Category: {command.Category ?? "(any)"} | Limit: {limit}");

            return 0;
        }
    }
}
