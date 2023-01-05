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