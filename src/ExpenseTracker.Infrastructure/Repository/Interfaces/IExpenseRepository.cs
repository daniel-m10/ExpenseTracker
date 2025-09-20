using ExpenseTracker.Infrastructure.Repository.Dto;

namespace ExpenseTracker.Infrastructure.Repository.Interfaces
{
    public interface IExpenseRepository : IRepository<ExpenseDto, Guid> { }
}
