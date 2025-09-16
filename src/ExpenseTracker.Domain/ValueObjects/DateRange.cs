namespace ExpenseTracker.Domain.ValueObjects
{
    public sealed record DateRange
    {
        public DateTime Start { get; }
        public DateTime End { get; }

        public DateRange(DateTime start, DateTime end)
        {
            if (start > end)
                throw new ArgumentOutOfRangeException(nameof(start), "Start should not be after End.");

            Start = start;
            End = end;
        }

        public bool Contains(DateTime date) => date >= Start && date <= End;

        public bool Overlaps(DateRange other) => Start <= other.End && End >= other.Start;

        public TimeSpan Duration() => End - Start;

        public override string ToString() => $"{Start:yyyy-MM-dd} to {End:yyyy-MM-dd}";
    }
}
