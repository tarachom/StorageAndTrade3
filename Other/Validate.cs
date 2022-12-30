using Gtk;

namespace StorageAndTrade
{
    class Validate
    {
        public static (bool, int) IsInt(string text)
        {
            int value;
            return (int.TryParse(text, out value), value);
        }

        public static (bool, decimal) IsDecimal(string text)
        {
            decimal value;
            return (decimal.TryParse(text, out value), value);
        }
    }
}