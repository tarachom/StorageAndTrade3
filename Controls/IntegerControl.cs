using Gtk;

namespace StorageAndTrade
{
    class IntegerControl : HBox
    {
        Entry entryInteger = new Entry();

        public IntegerControl() : base()
        {
            PackStart(entryInteger, false, false, 2);
        }

        int mValue;
        public int Value
        {
            get
            {
                return mValue;
            }
            set
            {
                mValue = value;
                entryInteger.Text = mValue.ToString();
            }
        }
    }
}