using ExpenseTracker.CLI.Abstractions;
using ExpenseTracker.CLI.Commands;
using Serilog;
using System.Globalization;

namespace ExpenseTracker.CLI.Handlers
{
    public class AddCommandHandler(ILogger logger, IDateParser dateParser)
    {
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
        private readonly IDateParser _dateParser = dateParser ?? throw new ArgumentNullException(nameof(dateParser), "Date parser cannot be null.");

        public async Task<int> RunAddAsync(AddCommand command)
        {
            if (command.Amount < 0)
            {
                _logger.Error("Amount must be greater or equal to 0.");
                return 1;
            }

            string format = "yyyy-MM-dd";
            CultureInfo provider = CultureInfo.InvariantCulture;

            if (!_dateParser.TryParseExact(command.Date, format, provider, DateTimeStyles.None, out DateTime parsedDate))
            {
                _logger.Error($"Wrong data format for Expense date: {command.Date}");
                _logger.Error("Please use this format: yyyy-MM-dd (e.g., 2024-01-15)");
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

            _logger.Information("Expense recorded successfully!");
            _logger.Information($"Description: {expense.Description}");
            _logger.Information($"Amount     : {expense.Amount:C}");   // currency format
            _logger.Information($"Category   : {expense.Category}");
            _logger.Information($"Date       : {expense.Date:yyyy-MM-dd}");

            await Task.CompletedTask;

            return 0;
        }
    }
}
