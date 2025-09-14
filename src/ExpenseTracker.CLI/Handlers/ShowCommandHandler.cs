using ExpenseTracker.CLI.Commands;
using ExpenseTracker.CLI.Constants;
using Serilog;

namespace ExpenseTracker.CLI.Handlers
{
    public class ShowCommandHandler(ILogger logger)
    {
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger), Messages.LoggerCannotBeNull);

        public async Task<int> RunShowAsync(ShowCommand command)
        {
            if (command.Id <= 0)
            {
                _logger.Error(Messages.IdMustBeGreaterThanZero);
                return 1;
            }

            await Task.CompletedTask;

            await Task.CompletedTask;
            _logger.Information(Messages.ShowingDetailsForExpense, command.Id);
            _logger.Information(Messages.DescriptionLabel, "Lunch");
            _logger.Information(Messages.AmountLabel, 20.00m);
            _logger.Information(Messages.CategoryLabel, "Food");
            _logger.Information(Messages.DateLabel, new DateTime(2024, 1, 15));

            return 0;
        }
    }
}
