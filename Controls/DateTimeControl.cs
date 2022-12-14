#region Info

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

#endregion

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