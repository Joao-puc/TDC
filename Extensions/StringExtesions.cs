using System.Globalization;

namespace TDC.Extensions
{
    public static class StringExtensions
    {
        public static double? ToDouble(this string value, CultureInfo culture = null)
        {
            if (string.IsNullOrEmpty(value.Trim()))
                return null;

            var result = double.Parse(value, culture ?? CultureInfo.InvariantCulture);
            return result;
        }
    }
}