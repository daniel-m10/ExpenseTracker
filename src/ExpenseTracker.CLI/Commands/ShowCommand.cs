using CommandLine;

namespace ExpenseTracker.CLI.Commands
{
    [Verb("show", HelpText = "Display single expense details with category information")]
    public class ShowCommand
    {
        // expense-tracker show --id 1
        [Option("id", Required = true,
        HelpText = "Unique expense id (e.g., 42)",
        MetaValue = "EXPENSE_ID")]
        public int Id { get; set; }
    }
}
