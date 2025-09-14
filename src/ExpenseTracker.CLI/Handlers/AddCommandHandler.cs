using ExpenseTracker.CLI.Abstractions;
using ExpenseTracker.CLI.Commands;
using ExpenseTracker.CLI.Constants;
using Serilog;

namespace ExpenseTracker.CLI.Handlers
{
    public class AddCommandHandler(ILogger logger, IDateParser dateParser)
    {
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger), Messages.LoggerCannotBeNull);
        private readonly IDateParser _dateParser = dateParser ?? throw new ArgumentNullException(nameof(dateParser), Messages.DateParserCannotBeNull);

        public async Task<int> RunAddAsync(AddCommand command)
        {
            if (command.Amount < 0)
            {
                _logger.Error(Messages.AmountMustBeNonNegative);
                return 1;
            }

            if (!_dateParser.TryParseExact(command.Date, DateFormats.StandardDateFormat, DateFormats.DefaultCultureInfo, DateFormats.DefaultDateTimeStyles, out DateTime parsedDate))
            {
                _logger.Error(Messages.WrongDateFormat, command.Date);
                _logger.Error(Messages.DateFormatHint);
                return 1;
            }

            // Simulate expense was correctly saved
            var expense = new
            {
                command.Description,
                command.Amount,
                command.Category,
                Date = parsedDate
            };

            _logger.Information(Messages.ExpenseRecordedSuccessfully);
            _logger.Information(Messages.DescriptionLabel, expense.Description);
            _logger.Information(Messages.AmountLabel, expense.Amount);
            _logger.Information(Messages.CategoryLabel, expense.Category);
            _logger.Information(Messages.DateLabel, expense.Date);

            await Task.CompletedTask;

            return 0;
        }
    }
}
