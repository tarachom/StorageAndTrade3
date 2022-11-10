using Gtk;

namespace StorageAndTrade
{
    class NumericControl : HBox
    {
        Entry entryNumeric = new Entry();

        public NumericControl() : base()
        {
            PackStart(entryNumeric, false, false, 2);
        }

        decimal mValue;
        public decimal Value
        {
            get
            {
                return mValue;
            }
            set
            {
                mValue = value;
                entryNumeric.Text = mValue.ToString();
            }
        }
    }
}