namespace TDC.Extensions
{
    public static class StringExtensions
    {
        public static double? ToDouble(this string value)
        {
            if(string.IsNullOrEmpty(value.Trim()))
                return null;

            return double.Parse(value);
        }
    }
}