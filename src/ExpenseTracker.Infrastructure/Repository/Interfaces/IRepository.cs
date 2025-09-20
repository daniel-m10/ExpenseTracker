namespace ExpenseTracker.Infrastructure.Repository.Interfaces
{
    public interface IRepository<TDto, TKey>
    {
        Task<TDto?> GetByIdAsync(TKey id, CancellationToken ct);
        Task<IEnumerable<TDto>> GetAllAsync(CancellationToken ct);
        Task AddAsync(TDto dto, CancellationToken ct);
        Task UpdateAsync(TDto dto, CancellationToken ct);
        Task DeleteAsync(TKey id, CancellationToken ct);
    }
}
