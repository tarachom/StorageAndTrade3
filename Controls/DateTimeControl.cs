using Gtk;

namespace StorageAndTrade
{
    class DateTimeControl : HBox
    {
        Entry entryDateTimeValue = new Entry();

        public DateTimeControl() : base()
        {
            //Entry
            entryDateTimeValue.Changed += OnEntryDateTimeChanged;

            PackStart(entryDateTimeValue, false, false, 2);

            //Button
            Button openCalendarButton = new Button(new Image("images/find.png"));
            openCalendarButton.Clicked += OnOpenCalendar;

            PackStart(openCalendarButton, false, false, 2);
        }

        bool disableChanged = false;

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

                disableChanged = true;
                entryDateTimeValue.Text = mValue.ToString();
                disableChanged = false;
            }
        }

        void OnOpenCalendar(object? sender, EventArgs args)
        {
            CalendarWindow cw = new CalendarWindow();
            cw.Select = (DateTime x) => { Value = x; };
            cw.Value = Value;
            cw.Show();
        }

        void OnEntryDateTimeChanged(object? sender, EventArgs args)
        {
            if (!disableChanged)
            {
                DateTime value;
                if (DateTime.TryParse(entryDateTimeValue.Text, out value))
                {
                    mValue = value;
                    Console.WriteLine(mValue);
                }
                else
                {
                    //
                }
            }
        }
    }
}