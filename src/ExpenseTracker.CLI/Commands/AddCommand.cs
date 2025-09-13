using CommandLine;

namespace ExpenseTracker.CLI.Commands
{
    [Verb("add", HelpText = "Create new expense entries")]

    public class AddCommand
    {
        //expense-tracker add --description "Lunch" --amount 20 [--category Food] [--date 2024-01-15]
        [Option('d', "description", Required = true,
        HelpText = "Expense description (e.g., \"Lunch\")",
        MetaValue = "DESCRIPTION")]
        public string? Description { get; set; }

        [Option('a', "amount", Required = true,
        HelpText = "Expense amount as decimal (e.g., 20 or 20.50)",
        MetaValue = "AMOUNT")]
        public decimal Amount { get; set; }

        [Option('c', "category", Required = false,
        HelpText = "Expense category (e.g., Food, Transport, Utilities)",
        MetaValue = "CATEGORY")]
        public string? Category { get; set; }

        [Option('t', "date", Required = false,
        HelpText = "Expense date in ISO format yyyy-MM-dd (e.g., 2024-01-15)",
        MetaValue = "DATE")]
        public string? Date { get; set; }
    }
}
