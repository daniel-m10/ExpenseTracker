namespace ExpenseTracker.Domain.Results
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public IReadOnlyList<string> Errors { get; } = [];

        private Result(bool isSuccess, T? value, IReadOnlyList<string> errors)
        {
            IsSuccess = isSuccess;
            Value = value;
            Errors = errors;
        }

        public static Result<T> Success(T value) => new(true, value, []);
        public static Result<T> Failure(string error) => new(false, default, [error]);
        public static Result<T> Failure(params string[] errors) => new(false, default, errors);
        public static Result<T> Failure(IEnumerable<string> errors) => new(false, default, [.. errors]);
        public static Result<T> Try(Func<T> operation)
        {
            try { return Success(operation()); }
            catch (Exception ex) { return Failure(ex.Message); }
        }
    }

    public class Result
    {
        public bool IsSuccess { get; }
        public IReadOnlyList<string> Errors { get; } = [];

        private Result(bool isSuccess, IReadOnlyList<string> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public static Result Success() => new(true, []);
        public static Result Failure(string error) => new(false, [error]);
        public static Result Failure(params string[] errors) => new(false, errors);
        public static Result Failure(IEnumerable<string> errors) => new(false, [.. errors]);
    }
}
