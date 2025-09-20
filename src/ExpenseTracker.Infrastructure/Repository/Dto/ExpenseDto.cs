namespace ExpenseTracker.Infrastructure.Repository.Dto
{
    public class ExpenseDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}