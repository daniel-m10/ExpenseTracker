using CommandLine;
using ExpenseTracker.CLI.Abstractions;
using ExpenseTracker.CLI.Commands;
using ExpenseTracker.CLI.Extensions;
using ExpenseTracker.CLI.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var services = new ServiceCollection();

services.AddCore(configuration);

var serviceProvider = services.BuildServiceProvider();

var normalizedArgs = NormalizeArgs(args);

try
{
    var parserResult = Parser.Default.ParseArguments<
        AddCommand, ListCommand, DeleteCommand, ShowCommand, SummaryCommand, CategoriesCommand>(normalizedArgs);

    await parserResult.MapResult(
        async (AddCommand cmd) =>
        {
            var handler = serviceProvider.GetRequiredService<AddCommandHandler>();
            return await handler.RunAddAsync(cmd);
        },
        async (ListCommand cmd) =>
        {
            var handler = serviceProvider.GetRequiredService<ListCommandHandler>();
            return await handler.RunListAsync(cmd);
        },
        async (DeleteCommand cmd) =>
        {
            var handler = serviceProvider.GetRequiredService<DeleteCommandHandler>();
            return await handler.RunDeleteAsync(cmd);
        },
        async (ShowCommand cmd) =>
        {
            var handler = serviceProvider.GetRequiredService<ShowCommandHandler>();
            return await handler.RunShowAsync(cmd);
        },
        async (SummaryCommand cmd) =>
        {
            var handler = serviceProvider.GetRequiredService<SummaryCommandHandler>();
            return await handler.RunSummaryAsync(cmd);
        },
        async (CategoriesCommand cmd) =>
        {
            var handler = serviceProvider.GetRequiredService<CategoriesCommandHandler>();
            return await handler.RunCategoriesAsync(cmd);
        },
        errs =>
        {
            if (errs.IsHelp() || errs.IsVersion())
            {
                return Task.FromResult(0);
            }

            Log.Error("Failed to parse command: {@Errors}", errs);
            var output = serviceProvider.GetRequiredService<IConsoleOutput>();
            output.WriteError("Invalid command. Use --help for usage.");
            return Task.FromResult(1);
        });
}
catch (Exception ex)
{
    Log.Error(ex, "Unhandled exception ocurred.");
    var output = serviceProvider.GetRequiredService<IConsoleOutput>();
    output.WriteError("Invalid command. Use --help for usage.");
}
finally
{
    Log.CloseAndFlush();
}

static string[] NormalizeArgs(string[] args)
{
    return [.. args
        .Where(a => !string.IsNullOrWhiteSpace(a))
        .Select(a => a.Trim())];
}
