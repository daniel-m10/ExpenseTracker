using ExpenseTracker.CLI.Commands;
using ExpenseTracker.CLI.Utils;
using Serilog;

namespace ExpenseTracker.CLI.Handlers
{
    public class CategoriesCommandHandler(ILogger logger)
    {
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");

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
                    _logger.Error("Invalid ACTION. Use: list | add | delete");
                    return 1;
            }
        }

        private async Task<int> HandleListAsync()
        {
            // Simulation
            await Task.CompletedTask;
            _logger.Information("Categories");
            return 0;
        }

        private async Task<int> HandleAddAsync(CategoriesCommand cmd)
        {
            if (string.IsNullOrWhiteSpace(cmd.Name))
            {
                _logger.Error("Missing --name for 'add'. Example: categories add --name Food");
                return 1;
            }

            // Simulation
            await Task.CompletedTask;
            _logger.Information($"Category added: {cmd.Name.Trim()} (id: 7)");
            return 0;
        }

        private async Task<int> HandleDeleteAsync(CategoriesCommand cmd)
        {
            // Allow deletion by name or id
            bool hasId = cmd.Id is > 0;
            bool hasName = !string.IsNullOrWhiteSpace(cmd.Name);

            if (hasId == hasName)
            {
                _logger.Error("For 'delete', provide exactly one: --id ID  OR  --name NAME");
                _logger.Information("Examples:");
                _logger.Information("  categories delete --id 3");
                _logger.Information("  categories delete --name Food");
                return 1;
            }

            // Simulation
            await Task.CompletedTask;
            if (hasId)
                _logger.Information($"Category deleted (id: {cmd.Id})");
            else
                _logger.Information($"Category deleted (name: {cmd.Name!.Trim()})");

            return 0;
        }
    }

}
