using ExpenseTracker.CLI.Commands;
using ExpenseTracker.CLI.Constants;
using Serilog;

namespace ExpenseTracker.CLI.Handlers
{
    public class SummaryCommandHandler(ILogger logger)
    {
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger), Messages.LoggerCannotBeNull);

        public async Task<int> RunSummaryAsync(SummaryCommand command)
        {
            // Defaults
            var now = DateTime.UtcNow;
            int year = command.Year ?? now.Year;
            int month = command.Month ?? now.Month;

            if (command.Month < 1 || command.Month > 12)
            {
                _logger.Error(Messages.MonthMustBeBetween1And12);
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

            _logger.Information(Messages.SummaryHeader, year, month, category);
            _logger.Information(Messages.TotalExpenses, total);

            return 0;
        }
    }
}
