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

/*

Пошук з виводом результатів у таб частину

*/

using Gtk;

namespace StorageAndTrade
{
    class SearchControl2 : HBox
    {
        SearchEntry entrySearch = new SearchEntry() { WidthRequest = 200 };

        public SearchControl2() : base()
        {
            entrySearch.KeyReleaseEvent += OnKeyReleaseEntrySearch;
            entrySearch.TextDeleted += OnClear;
            PackStart(entrySearch, false, false, 2);
        }

        public System.Action<string>? Select { get; set; }
        public System.Action? Clear { get; set; }

        void OnKeyReleaseEntrySearch(object? sender, KeyReleaseEventArgs args)
        {
            if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter)
                if (Select != null)
                    Select.Invoke(entrySearch.Text);
        }

        void OnClear(object? sender, EventArgs args)
        {
            entrySearch.Text = "";

            if (Clear != null)
                Clear.Invoke();
        }
    }
}