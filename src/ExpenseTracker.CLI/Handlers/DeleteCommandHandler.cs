using ExpenseTracker.CLI.Abstractions;
using ExpenseTracker.CLI.Commands;
using ExpenseTracker.CLI.Constants;
using Serilog;

namespace ExpenseTracker.CLI.Handlers
{
    public class DeleteCommandHandler(ILogger logger, IConsoleInput input)
    {
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger), Messages.LoggerCannotBeNull);

        private readonly IConsoleInput _input = input ?? throw new ArgumentNullException(nameof(input), Messages.ConsoleInputCannotBeNull);

        public async Task<int> RunDeleteAsync(DeleteCommand command)
        {
            if (command.Id <= 0)
            {
                _logger.Error(Messages.IdMustBeGreaterThanZero);
                return 1;
            }

            if (!command.Force)
            {
                _logger.Information(Messages.ConfirmDeletePrompt, command.Id);
                var response = _input.ReadLine();
                if (response != "y")
                {
                    _logger.Information(Messages.DeleteCancelled);
                    return 1;
                }
            }

            // Expense deleted
            await Task.CompletedTask;
            _logger.Information(Messages.ExpenseDeletedSuccessfully, command.Id);
            return 0;
        }
    }
}
