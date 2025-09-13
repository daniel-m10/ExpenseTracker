using CommandLine;

namespace ExpenseTracker.CLI.Commands
{
    [Verb("summary", HelpText = "Calculate totals with optional filters")]
    public class SummaryCommand
    {
        // expense-tracker summary [--month 8] [--year 2024] [--category Food]
        [Option('m', "month", Required = false,
        HelpText = "Filter by month (1-12, e.g., 8)",
        MetaValue = "MONTH")]
        public int? Month { get; set; }

        [Option('y', "year", Required = false,
        HelpText = "Filter by year (e.g., 2024)",
        MetaValue = "YEAR")]
        public int? Year { get; set; }

        [Option('c', "category", Required = false,
        HelpText = "Filter by category (e.g., Food, Transport, Utilities)",
        MetaValue = "CATEGORY")]
        public string? Category { get; set; }
    }
}
