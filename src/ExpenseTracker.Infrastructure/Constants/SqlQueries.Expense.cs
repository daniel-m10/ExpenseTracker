namespace ExpenseTracker.Infrastructure.Constants
{
    public static partial class SqlQueries
    {
        public const string GetExpenseById = "SELECT * FROM Expenses WHERE Id = @Id";
        public const string GetAllExpenses = "SELECT * FROM Expenses";
        public const string AddExpense =
            @"INSERT INTO Expenses (Id, Amount, Description, Date, CategoryId, CreatedAt)
                VALUES (@Id, @Amount, @Description, @Date, @CategoryId, @CreatedAt)";
        public const string UpdateExpense =
            @"UPDATE Expenses SET 
                    Amount = @Amount,
                    Description = @Description,
                    Date = @Date,
                    CategoryId = @CategoryId
                  WHERE Id = @Id";
        public const string DeleteExpense = "DELETE FROM Expenses WHERE Id = @Id";
    }
}
