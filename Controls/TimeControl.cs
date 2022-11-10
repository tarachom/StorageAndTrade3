using Gtk;

namespace StorageAndTrade
{
    class TimeControl : HBox
    {
        Entry entryTime = new Entry();

        public TimeControl() : base()
        {
            PackStart(entryTime, false, false, 2);
        }

        TimeSpan mValue;
        public TimeSpan Value
        {
            get
            {
                return mValue;
            }
            set
            {
                mValue = value;
                entryTime.Text = mValue.ToString();
            }
        }
    }
}