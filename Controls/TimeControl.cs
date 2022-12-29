using Gtk;

namespace StorageAndTrade
{
    class TimeControl : HBox
    {
        Entry entryTime = new Entry();
        HBox hBoxInfoValid = new HBox() { WidthRequest = 16 };

        public TimeControl() : base()
        {
            PackStart(hBoxInfoValid, false, false, 1);

            entryTime.Changed += OnEntryTimeChanged;
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

        void ClearHBoxInfoValid()
        {
            foreach (Widget item in hBoxInfoValid.Children)
                hBoxInfoValid.Remove(item);
        }

        public bool IsValidValue()
        {
            ClearHBoxInfoValid();

            TimeSpan value;
            if (TimeSpan.TryParse(entryTime.Text, out value))
            {
                mValue = value;

                hBoxInfoValid.Add(new Image("images/16/ok.png"));
                hBoxInfoValid.ShowAll();

                return true;
            }
            else
            {
                hBoxInfoValid.Add(new Image("images/16/error.png"));
                hBoxInfoValid.ShowAll();

                return false;
            }
        }

        void OnEntryTimeChanged(object? sender, EventArgs args)
        {
            IsValidValue();
        }
    }
}