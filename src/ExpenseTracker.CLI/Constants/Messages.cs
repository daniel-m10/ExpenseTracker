namespace ExpenseTracker.CLI.Constants
{
    public static class Messages
    {
        // Validation Error Messages
        public const string AmountMustBeNonNegative = "Amount must be greater or equal to 0.";
        public const string MonthMustBeBetween1And12 = "Month must be between 1 and 12.";
        public const string LimitMustBeGreaterThanZero = "Limit must be greater than 0.";
        public const string IdMustBeGreaterThanZero = "Id must be greater than 0.";
        
        // Date Error Messages
        public const string WrongDateFormat = "Wrong data format for Expense date: {0}";
        public const string DateFormatHint = "Please use this format: yyyy-MM-dd (e.g., 2024-01-15)";
        
        // Success Messages
        public const string ExpenseRecordedSuccessfully = "Expense recorded successfully!";
        public const string ExpenseDeletedSuccessfully = "Expense #{0} deleted successfully.";
        public const string DeleteCancelled = "Delete cancelled.";
        
        // Info Messages
        public const string ConfirmDeletePrompt = "Are you sure you want to delete expense #{0}? (y/n)";
        public const string ListingExpenses = "Listing expenses | Year: {0} | Month: {1} | Category: {2} | Limit: {3}";
        
        // Field Labels
        public const string DescriptionLabel = "Description: {0}";
        public const string AmountLabel = "Amount     : {0:C}";
        public const string CategoryLabel = "Category   : {0}";
        public const string DateLabel = "Date       : {0:yyyy-MM-dd}";
        
        // Show Command Messages
        public const string ShowingDetailsForExpense = "Showing details for expense #{0}";
        
        // Summary Command Messages  
        public const string SummaryHeader = "Summary | Year: {0} | Month: {1} | Category: {2}";
        public const string TotalExpenses = "Total expenses: {0:C}";
        
        // Categories Command Messages
        public const string InvalidAction = "Invalid ACTION. Use: list | add | delete";
        public const string Categories = "Categories";
        public const string MissingNameForAdd = "Missing --name for 'add'. Example: categories add --name Food";
        public const string CategoryAdded = "Category added: {0} (id: 7)";
        public const string CategoryDeletedById = "Category deleted (id: {0})";
        public const string CategoryDeletedByName = "Category deleted (name: {0})";
        public const string DeleteRequiresIdOrName = "For 'delete', provide exactly one: --id ID  OR  --name NAME";
        public const string ExamplesHeader = "Examples:";
        public const string ExampleDeleteById = "  categories delete --id 3";
        public const string ExampleDeleteByName = "  categories delete --name Food";
        
        // General Error Messages
        public const string InvalidCommand = "Invalid command. Use --help for usage.";
        public const string UnhandledException = "Unhandled exception occurred.";
        public const string FailedToParseCommand = "Failed to parse command: {@Errors}";
        
        // Null Check Messages
        public const string LoggerCannotBeNull = "Logger cannot be null.";
        public const string DateParserCannotBeNull = "Date parser cannot be null.";
        public const string ConsoleInputCannotBeNull = "Console Input cannot be null.";
    }
}