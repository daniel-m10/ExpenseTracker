using ExpenseTracker.Domain.Models;

namespace ExpenseTracker.Domain.Abstractions
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
    }
}
