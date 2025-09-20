using Dapper;
using ExpenseTracker.Infrastructure.Config;
using ExpenseTracker.Infrastructure.Data.Interfaces;
using ExpenseTracker.Infrastructure.Repository.Dto;
using ExpenseTracker.Infrastructure.Repository.Interfaces;
using static ExpenseTracker.Infrastructure.Constants.SqlQueries;

namespace ExpenseTracker.Infrastructure.Repository
{
    public class ExpenseRepository(IDbConnectionFactory connectionFactory, DatabaseConfiguration dbConfig) : IExpenseRepository
    {
        private readonly IDbConnectionFactory _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        private readonly DatabaseConfiguration _dbConfig = dbConfig ?? throw new ArgumentNullException(nameof(dbConfig));

        public async Task AddAsync(ExpenseDto dto, CancellationToken ct)
        {
            using var connection = await _connectionFactory.CreateAndOpenConnectionAsync(dbConfig, ct);
            await connection.ExecuteAsync(AddExpense, dto);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            using var connection = await _connectionFactory.CreateAndOpenConnectionAsync(_dbConfig, ct);
            await connection.ExecuteAsync(DeleteExpense, new { Id = id });
        }

        public async Task<IEnumerable<ExpenseDto>> GetAllAsync(CancellationToken ct)
        {
            using var connection = await _connectionFactory.CreateAndOpenConnectionAsync(_dbConfig, ct);
            return await connection.QueryAsync<ExpenseDto>(GetAllExpenses);
        }

        public async Task<ExpenseDto?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            using var connection = await _connectionFactory.CreateAndOpenConnectionAsync(_dbConfig, ct);
            return await connection.QuerySingleOrDefaultAsync<ExpenseDto>(GetExpenseById, new { Id = id });
        }

        public async Task UpdateAsync(ExpenseDto dto, CancellationToken ct)
        {
            using var connection = await _connectionFactory.CreateAndOpenConnectionAsync(_dbConfig, ct);
            await connection.ExecuteAsync(UpdateExpense, dto);
        }
    }
}
