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
            // Треба протестувати чи варто заміняти крапку на кому на інших мовах операційної системи
            if (text.IndexOf(".") != -1)
                text = text.Replace(".", ",");

            decimal value;
            return (decimal.TryParse(text, out value), value);
        }
    }
}