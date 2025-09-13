using System.Globalization;

namespace ExpenseTracker.CLI.Abstractions
{
    public interface IDateParser
    {
        bool TryParseExact(string? dateString, string? format, IFormatProvider? provider, DateTimeStyles style, out DateTime result);
    }
}
