using System.ComponentModel;
using System.Globalization;

namespace ExpenseTracker.CLI.Utils
{
    public class CategoryActionConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) => sourceType == typeof(string);

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string str)
            {
                if (Enum.TryParse<CategoryAction>(str, ignoreCase: true, out var action))
                {
                    return action;
                }
                throw new ArgumentException($"Invalid action '{str}'. Allowed values: list, add, delete.");
            }
            throw new ArgumentException("Action must be a string.");
        }
    }

    public enum CategoryAction
    {
        list,
        add,
        delete
    }
}
