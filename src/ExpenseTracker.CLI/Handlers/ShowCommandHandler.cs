using ExpenseTracker.CLI.Commands;
using Serilog;

namespace ExpenseTracker.CLI.Handlers
{
    public class ShowCommandHandler(ILogger logger)
    {
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");

        public async Task<int> RunShowAsync(ShowCommand command)
        {
            if (command.Id <= 0)
            {
                _logger.Information("Id must be greater than 0.");
                return 1;
            }

            await Task.CompletedTask;

            await Task.CompletedTask;
            _logger.Information($"Showing details for expense #{command.Id}");
            _logger.Information("Description: Lunch");
            _logger.Information("Amount     : $20.00");
            _logger.Information("Category   : Food");
            _logger.Information("Date       : 2024-01-15");

            return 0;
        }
    }
}
