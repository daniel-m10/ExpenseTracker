using CommandLine;

namespace ExpenseTracker.CLI.Commands
{
    [Verb("delete", HelpText = "Remove expense entries by ID with confirmation")]
    public class DeleteCommand
    {
        //expense-tracker delete --id 2
        [Option("id", Required = true,
        HelpText = "Unique expense id (e.g., 42)",
        MetaValue = "EXPENSE_ID")]
        public int Id { get; set; }

        [Option("force", Required = false,
        HelpText = "Skip confirmation prompt",
        Default = false)]
        public bool Force { get; set; }
    }
}
