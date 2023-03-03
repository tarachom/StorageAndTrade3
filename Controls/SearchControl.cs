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

Пошук з виводом результатів у список

*/

using Gtk;

using AccountingSoftware;
using StorageAndTrade_1_0;

namespace StorageAndTrade
{
    class SearchControl : HBox
    {
        Label labelCaption = new Label("Пошук:");
        ListStore Store = new ListStore(typeof(string), typeof(string));
        ComboBoxText entrySearch = ComboBoxText.NewWithEntry();

        public SearchControl() : base()
        {
            PackStart(labelCaption, false, false, 2);

            entrySearch.Model = Store;
            entrySearch.EntryTextColumn = 1;
            entrySearch.Entry.KeyReleaseEvent += OnEntrySearchKeyRelease;
            entrySearch.Changed += OnEntrySearchChanged;
            PackStart(entrySearch, false, false, 2);

            Button bClear = new Button(new Image(AppContext.BaseDirectory + "images/clean.png"));
            bClear.Clicked += OnClear;
            PackStart(bClear, false, false, 2);
        }

        public string Caption
        {
            get
            {
                return labelCaption.Text;
            }
            set
            {
                labelCaption.Text = value;
            }
        }

        public string QueryFind { get; set; } = "";

        public System.Action<UnigueID>? Select { get; set; }

        void OnEntrySearchKeyRelease(object? sender, KeyReleaseEventArgs args)
        {
            if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter)
            {
                Fill(QueryFind, entrySearch.Entry.Text);

                TreeIter iter;
                if (entrySearch.Model.GetIterFirst(out iter))
                    entrySearch.Popup();
            }
        }

        void OnEntrySearchChanged(object? sender, EventArgs args)
        {
            TreeIter iter;
            if (entrySearch.GetActiveIter(out iter))
            {
                string value = (string)entrySearch.Model.GetValue(iter, 0);

                if (Select != null)
                    Select.Invoke(new UnigueID(value));
            }
        }

        void OnClear(object? sender, EventArgs args)
        {
            entrySearch.Entry.Text = "";
            Store.Clear();
        }

        void Fill(string queryFind, string findText)
        {
            Store.Clear();

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("like_param", "%" + findText.ToLower().Trim() + "%");

            string[] columnsName;
            List<Dictionary<string, object>>? listRow = null;

            Config.Kernel?.DataBase.SelectRequest(queryFind, paramQuery, out columnsName, out listRow);

            if (listRow != null)
                if (listRow.Count > 0)
                {
                    foreach (Dictionary<string, object> row in listRow)
                        Store.InsertWithValues(0, row["uid"].ToString(), row["Назва"].ToString());
                }
        }
    }
}