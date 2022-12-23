using Gtk;

namespace StorageAndTrade
{
    class CalendarWindow : Window
    {
        Calendar calendar;

        public CalendarWindow() : base("Calendar")
        {
            SetDefaultSize(300, 0);
            SetDefaultIconFromFile("images/form.ico");
            SetPosition(WindowPosition.Mouse);
            
            Modal = true;

            VBox vbox = new VBox();
            Add(vbox);

            calendar = new Calendar();
            calendar.DaySelectedDoubleClick += OnCalendarDaySelected;

            vbox.PackStart(calendar, false, false, 0);

            ShowAll();
        }

        public Action<DateTime>? Select { get; set; }

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
                calendar.Date = value;
            }
        }

        void OnCalendarDaySelected(object? sender, EventArgs args)
        {
            if (Select != null)
                Select.Invoke(
                    new DateTime(
                        calendar.Date.Year, calendar.Date.Month, calendar.Date.Day,
                        Value.Hour, Value.Minute, Value.Second));

            Close();
        }
    }
}