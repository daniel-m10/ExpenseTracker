using ExpenseTracker.CLI.Commands;
using Serilog;

namespace ExpenseTracker.CLI.Handlers
{
    public class SummaryCommandHandler(ILogger logger)
    {
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");

        public async Task<int> RunSummaryAsync(SummaryCommand command)
        {
            // Defaults
            var now = DateTime.UtcNow;
            int year = command.Year ?? now.Year;
            int month = command.Month ?? now.Month;

            if (command.Month < 1 || command.Month > 12)
            {
                _logger.Error("Month must be between 1 and 12.");
                return 1;
            }

            if (year < 2025)
            {
                _logger.Error("Year must be greater or equal than 2025.");
                return 1;
            }

            var category = string.IsNullOrWhiteSpace(command.Category) ? "(any)" : command.Category;

            // Total expenses
            decimal total = 123.45m;

            await Task.CompletedTask;

            _logger.Information($"Summary | Year: {year} | Month: {month} | Category: {category}");
            _logger.Information($"Total expenses: {total:C}");

            return 0;
        }
    }
}
