using ExpenseTracker.CLI.Abstractions;
using ExpenseTracker.CLI.Handlers;
using ExpenseTracker.CLI.Output;
using ExpenseTracker.CLI.Parsers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ExpenseTracker.CLI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);
            ArgumentNullException.ThrowIfNull(services);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()
                .CreateLogger();

            services.AddSingleton(configuration);
            services.AddSingleton(Log.Logger);
            services.AddSingleton<IConsoleOutput, SerilogConsoleOutput>();
            services.AddSingleton<IDateParser, DateParser>();

            services.AddTransient<AddCommandHandler>();
            services.AddTransient<ListCommandHandler>();
            services.AddTransient<DeleteCommandHandler>();
            services.AddTransient<ShowCommandHandler>();
            services.AddTransient<SummaryCommandHandler>();
            services.AddTransient<CategoriesCommandHandler>();

            return services;
        }
    }
}
