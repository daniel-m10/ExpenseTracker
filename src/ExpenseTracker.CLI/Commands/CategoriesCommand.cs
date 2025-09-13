using CommandLine;
using ExpenseTracker.CLI.Utils;
using System.ComponentModel;

namespace ExpenseTracker.CLI.Commands
{
    [Verb("categories", HelpText = "Manage categories (list, add, delete)")]
    public class CategoriesCommand
    {
        // expense-tracker categories [list|add|delete] [--name NAME] [--id ID]
        [Option('a', "action", Required = true,
        HelpText = "Action to perform: list | add | delete")]
        [TypeConverter(typeof(CategoryActionConverter))]
        public CategoryAction Action { get; set; }

        // Used for: add, delete (by name)
        [Option('n', "name", Required = false,
        HelpText = "Category name (e.g., Food, Transport)",
        MetaValue = "NAME")]
        public string? Name { get; set; }

        // Used for: delete (by id)
        [Option('i', "id", Required = false,
        HelpText = "Category id (e.g., 3)",
        MetaValue = "ID")]
        public int? Id { get; set; }
    }
}
