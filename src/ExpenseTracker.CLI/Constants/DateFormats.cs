using System.Globalization;

namespace ExpenseTracker.CLI.Constants
{
    public static class DateFormats
    {
        public const string StandardDateFormat = "yyyy-MM-dd";
        public static readonly CultureInfo DefaultCultureInfo = CultureInfo.InvariantCulture;
        public const DateTimeStyles DefaultDateTimeStyles = DateTimeStyles.None;
    }
}