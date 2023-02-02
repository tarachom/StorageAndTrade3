/*
Copyright (C) 2019-2023 TARAKHOMYN YURIY IVANOVYCH
All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

/*
Автор:    Тарахомин Юрій Іванович
Адреса:   Україна, м. Львів
Сайт:     accounting.org.ua
*/

using Gtk;

namespace StorageAndTrade
{
    class CalendarWindow : Window
    {
        Calendar calendar;

        public CalendarWindow() : base("Календар")
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
            {
                Select.Invoke(new DateTime(
                    calendar.Date.Year, calendar.Date.Month, calendar.Date.Day,
                        Value.Hour, Value.Minute, Value.Second));
            }

            Close();
        }
    }
}