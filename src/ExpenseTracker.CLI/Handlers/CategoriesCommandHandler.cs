using ExpenseTracker.CLI.Commands;
using ExpenseTracker.CLI.Constants;
using ExpenseTracker.CLI.Utils;
using Serilog;

namespace ExpenseTracker.CLI.Handlers
{
    public class CategoriesCommandHandler(ILogger logger)
    {
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger), Messages.LoggerCannotBeNull);

        public async Task<int> RunCategoriesAsync(CategoriesCommand command)
        {
            switch (command.Action)
            {
                case CategoryAction.list:
                    return await HandleListAsync();

                case CategoryAction.add:
                    return await HandleAddAsync(command);

                case CategoryAction.delete:
                    return await HandleDeleteAsync(command);

                default:
                    _logger.Error(Messages.InvalidAction);
                    return 1;
            }
        }

        private async Task<int> HandleListAsync()
        {
            // Simulation
            await Task.CompletedTask;
            _logger.Information(Messages.Categories);
            return 0;
        }

        private async Task<int> HandleAddAsync(CategoriesCommand cmd)
        {
            if (string.IsNullOrWhiteSpace(cmd.Name))
            {
                _logger.Error(Messages.MissingNameForAdd);
                return 1;
            }

            // Simulation
            await Task.CompletedTask;
            _logger.Information(Messages.CategoryAdded, cmd.Name.Trim());
            return 0;
        }

        private async Task<int> HandleDeleteAsync(CategoriesCommand cmd)
        {
            // Allow deletion by name or id
            bool hasId = cmd.Id is > 0;
            bool hasName = !string.IsNullOrWhiteSpace(cmd.Name);

            if (hasId == hasName)
            {
                _logger.Error(Messages.DeleteRequiresIdOrName);
                _logger.Information(Messages.ExamplesHeader);
                _logger.Information(Messages.ExampleDeleteById);
                _logger.Information(Messages.ExampleDeleteByName);
                return 1;
            }

            // Simulation
            await Task.CompletedTask;
            if (hasId)
                _logger.Information(Messages.CategoryDeletedById, cmd.Id);
            else
                _logger.Information(Messages.CategoryDeletedByName, cmd.Name!.Trim());

            return 0;
        }
    }

}
