using ExpenseTracker.CLI.Commands;
using ExpenseTracker.CLI.Constants;
using Serilog;

namespace ExpenseTracker.CLI.Handlers
{
    public class ListCommandHandler(ILogger logger)
    {
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger), Messages.LoggerCannotBeNull);

        public async Task<int> RunListAsync(ListCommand command)
        {
            // Defaults
            var now = DateTime.UtcNow;
            int year = command.Year ?? now.Year;
            int month = command.Month ?? now.Month;
            int limit = command.Limit ?? 1;

            if (month < 1 || month > 12)
            {
                _logger.Error(Messages.MonthMustBeBetween1And12);
                return 1;
            }

            if (limit <= 0)
            {
                _logger.Error(Messages.LimitMustBeGreaterThanZero);
                return 1;
            }

            // Simulate that task was completed
            await Task.CompletedTask;

            _logger.Information(Messages.ListingExpenses, year, month, command.Category ?? "(any)", limit);

            return 0;
        }
    }
}
