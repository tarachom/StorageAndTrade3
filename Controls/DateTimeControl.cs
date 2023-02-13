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
    class DateTimeControl : HBox
    {
        Entry entryDateTimeValue = new Entry();
        HBox hBoxInfoValid = new HBox() { WidthRequest = 16 };
        Button bOpenCalendar;

        public DateTimeControl() : base()
        {
            PackStart(hBoxInfoValid, false, false, 1);

            //Entry
            entryDateTimeValue.Changed += OnEntryDateTimeChanged;
            PackStart(entryDateTimeValue, false, false, 1);

            //Button
            bOpenCalendar = new Button(new Image("images/find.png"));
            bOpenCalendar.Clicked += OnOpenCalendar;

            PackStart(bOpenCalendar, false, false, 1);
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

                if (OnlyDate)
                    mValue = new DateTime(mValue.Year, mValue.Month, mValue.Day);

                entryDateTimeValue.Text = OnlyDate ? mValue.ToString("dd.MM.yyyy") : mValue.ToString("dd.MM.yyyy HH:mm:ss");
            }
        }

        public bool OnlyDate { get; set; } = false;

        public DateTime ПочатокДня()
        {
            return new DateTime(Value.Year, Value.Month, Value.Day, 0, 0, 0);
        }

        public DateTime КінецьДня()
        {
            return new DateTime(Value.Year, Value.Month, Value.Day, 23, 59, 59);
        }

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
            Popover popoverCalendar = new Popover(bOpenCalendar);
            popoverCalendar.BorderWidth = 5;

            VBox vBox = new VBox();

            //Calendar
            Calendar calendar = new Calendar();
            calendar.Date = Value;

            calendar.DaySelected += (object? sender, EventArgs args) =>
            {
                Value = new DateTime(
                    calendar.Date.Year, calendar.Date.Month, calendar.Date.Day,
                    Value.Hour, Value.Minute, Value.Second);
            };

            calendar.DaySelectedDoubleClick += (object? sender, EventArgs args) =>
            {
                popoverCalendar.Hide();
            };

            vBox.PackStart(calendar, false, false, 0);

            SpinButton hourSpin = new SpinButton(0, 23, 1) { Orientation = Orientation.Vertical };
            SpinButton minuteSpin = new SpinButton(0, 59, 1) { Orientation = Orientation.Vertical };
            SpinButton secondSpin = new SpinButton(0, 59, 1) { Orientation = Orientation.Vertical };

            if (!OnlyDate)
            {
                HBox hBoxTime = new HBox() { Halign = Align.Center };
                vBox.PackStart(hBoxTime, false, false, 5);

                //Hour
                {
                    hourSpin.Value = TimeOnly.FromDateTime(Value).Hour;
                    hourSpin.ValueChanged += (object? sender, EventArgs args) =>
                    {
                        Value = new DateTime(
                            calendar.Date.Year, calendar.Date.Month, calendar.Date.Day,
                            (int)hourSpin.Value, Value.Minute, Value.Second);
                    };

                    hBoxTime.PackStart(hourSpin, false, false, 0);
                }

                hBoxTime.PackStart(new Label(":"), false, false, 5);

                //Minute
                {
                    minuteSpin.Value = TimeOnly.FromDateTime(Value).Minute;
                    minuteSpin.ValueChanged += (object? sender, EventArgs args) =>
                    {
                        Value = new DateTime(
                            calendar.Date.Year, calendar.Date.Month, calendar.Date.Day,
                            Value.Hour, (int)minuteSpin.Value, Value.Second);
                    };

                    hBoxTime.PackStart(minuteSpin, false, false, 0);
                }

                hBoxTime.PackStart(new Label(":"), false, false, 5);

                //Second
                {
                    secondSpin.Value = TimeOnly.FromDateTime(Value).Second;
                    secondSpin.ValueChanged += (object? sender, EventArgs args) =>
                    {
                        Value = new DateTime(
                            calendar.Date.Year, calendar.Date.Month, calendar.Date.Day,
                            Value.Hour, Value.Minute, (int)secondSpin.Value);
                    };

                    hBoxTime.PackStart(secondSpin, false, false, 0);
                }
            }

            //Поточна дата
            {
                LinkButton lbCurrentDate = new LinkButton("", "Поточна дата");
                lbCurrentDate.Clicked += (object? sender, EventArgs args) =>
                {
                    calendar.Date = Value = DateTime.Now;

                    hourSpin.Value = TimeOnly.FromDateTime(Value).Hour;
                    minuteSpin.Value = TimeOnly.FromDateTime(Value).Minute;
                    secondSpin.Value = TimeOnly.FromDateTime(Value).Second;

                    popoverCalendar.Hide();
                };

                vBox.PackStart(lbCurrentDate, false, false, 0);
            }

            popoverCalendar.Add(vBox);
            popoverCalendar.ShowAll();
        }

        void OnEntryDateTimeChanged(object? sender, EventArgs args)
        {
            IsValidValue();
        }
    }
}