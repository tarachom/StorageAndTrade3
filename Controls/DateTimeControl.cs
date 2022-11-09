using Gtk;

namespace StorageAndTrade
{
    class DateTimeControl : HBox
    {
        Entry entryDateTimeValue = new Entry();

        public DateTimeControl() : base()
        {
            PackStart(entryDateTimeValue, false, false, 2);
        }

        DateTime mValue;
        public DateTime Value
        {
            get
            {
                return mValue;
            }
            set
            {
                mValue = value;
                entryDateTimeValue.Text = mValue.ToString();
            }
        }
    }
}