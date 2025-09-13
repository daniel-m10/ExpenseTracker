using CommandLine;

namespace ExpenseTracker.CLI.Commands
{
    [Verb("list", HelpText = "Display expenses with optional filtering")]
    public class ListCommand
    {
        // expense-tracker list[--month 8] [--year 2024][--category Food][--limit 10]
        [Option('m', "month", Required = false,
        HelpText = "Expense month as int (1-12), e.g., 8",
        MetaValue = "MONTH")]
        public int? Month { get; set; }

        [Option('y', "year", Required = false,
        HelpText = "Expense year as int, e.g., 2024",
        MetaValue = "YEAR")]
        public int? Year { get; set; }

        [Option('c', "category", Required = false,
        HelpText = "Expense category (e.g., Food, Transport, Utilities)",
        MetaValue = "CATEGORY")]
        public string? Category { get; set; }

        [Option('l', "limit", Required = false,
        HelpText = "Max number of expenses to display (e.g., 10)",
        MetaValue = "LIMIT")]
        public int? Limit { get; set; }
    }
}
