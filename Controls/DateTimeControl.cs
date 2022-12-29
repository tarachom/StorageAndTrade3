using Gtk;

namespace StorageAndTrade
{
    class DateTimeControl : HBox
    {
        Entry entryDateTimeValue = new Entry();
        HBox hBoxInfoValid = new HBox() { WidthRequest = 16 };

        public DateTimeControl() : base()
        {
            PackStart(hBoxInfoValid, false, false, 1);

            //Entry
            entryDateTimeValue.Changed += OnEntryDateTimeChanged;
            PackStart(entryDateTimeValue, false, false, 1);

            //Button
            Button openCalendarButton = new Button(new Image("images/find.png"));
            openCalendarButton.Clicked += OnOpenCalendar;

            PackStart(openCalendarButton, false, false, 1);
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
                entryDateTimeValue.Text = OnlyDate ? mValue.ToShortDateString() : mValue.ToString();
            }
        }

        public bool OnlyDate { get; set; } = false;

        void ClearHBoxInfoValid()
        {
            foreach (Widget item in hBoxInfoValid.Children)
                hBoxInfoValid.Remove(item);
        }

        public bool IsValidValue()
        {
            ClearHBoxInfoValid();

            DateTime value;
            if (DateTime.TryParse(entryDateTimeValue.Text, out value))
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

        void OnOpenCalendar(object? sender, EventArgs args)
        {
            CalendarWindow cw = new CalendarWindow();
            cw.Select = (DateTime x) => { Value = x; };
            cw.Value = Value;
            cw.Show();
        }

        void OnEntryDateTimeChanged(object? sender, EventArgs args)
        {
            IsValidValue();
        }
    }
}