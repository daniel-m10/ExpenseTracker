using ExpenseTracker.CLI.Abstractions;
using System.Globalization;

namespace ExpenseTracker.CLI.Parsers
{
    public class DateParser : IDateParser
    {
        public bool TryParseExact(
            string? dateString,
            string? format,
            IFormatProvider? provider,
            DateTimeStyles style,
            out DateTime result)
            => DateTime.TryParseExact(dateString, format, provider, style, out result);
    }
}
