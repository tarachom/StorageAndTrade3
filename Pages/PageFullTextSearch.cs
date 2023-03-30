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

Стартова сторінка.

*/

using Gtk;
using AccountingSoftware;
using StorageAndTrade_1_0;

namespace StorageAndTrade
{
    class PageFullTextSearch : VBox
    {
        VBox vBoxMessage = new VBox();
        SearchEntry entryFullTextSearch = new SearchEntry() { WidthRequest = 500 };
        const int maxRowsToPage = 10;
        uint offset = 0;
        int count = 0;

        public PageFullTextSearch() : base()
        {
            HBox hBoxTop = new HBox() { Halign = Align.Center };
            PackStart(hBoxTop, false, false, 10);

            hBoxTop.PackStart(entryFullTextSearch, false, false, 10);
            entryFullTextSearch.KeyReleaseEvent += (object? sender, KeyReleaseEventArgs args) =>
            {
                if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter)
                    Find(entryFullTextSearch.Text, offset = 0);
            };

            ScrolledWindow scroll = new ScrolledWindow() { ShadowType = ShadowType.In };
            scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scroll.Add(vBoxMessage);

            PackStart(scroll, true, true, 0);

            ShowAll();
        }

        public void Find(string findtext, uint offset = 0)
        {
            entryFullTextSearch.Text = findtext;

            foreach (Widget Child in vBoxMessage.Children)
                vBoxMessage.Remove(Child);

            List<Dictionary<string, object>>? listRow = Config.Kernel!.DataBase.SpetialTableFullTextSearchSelect(findtext, offset);

            if (listRow != null)
            {
                count = listRow.Count;

                CreatePagination();

                foreach (Dictionary<string, object> row in listRow)
                    CreateMessage(row);

                CreatePagination();
            }

            vBoxMessage.ShowAll();
        }

        void CreateMessage(Dictionary<string, object> row)
        {
            Basis_PointerControl Обєкт = new Basis_PointerControl() { Caption = "" };
            Обєкт.Pointer = (UuidAndText)row["obj"];

            HBox hBoxRowInfo = new HBox();
            vBoxMessage.PackStart(hBoxRowInfo, false, false, 3);
            hBoxRowInfo.PackStart(new Label(row["value"].ToString()) { UseMarkup = true, Wrap = true, Selectable = true }, false, false, 12);

            HBox hBoxRowType = new HBox();
            vBoxMessage.PackStart(hBoxRowType, false, false, 3);
            hBoxRowType.PackStart(new Label("<small>" + Обєкт.PointerName + ": " + Обєкт.Type + "</small>") { UseMarkup = true, Selectable = true }, false, false, 12);
            hBoxRowType.PackStart(new Label("<small>Додано: " + row["dateadd"].ToString() + "</small>") { UseMarkup = true, Selectable = true }, false, false, 12);

            HBox hBoxRowControl = new HBox();
            vBoxMessage.PackStart(hBoxRowControl, false, false, 3);
            hBoxRowControl.PackStart(Обєкт, false, false, 0);

            vBoxMessage.PackStart(new Separator(Orientation.Horizontal), false, false, 0);
        }

        void CreatePagination()
        {
            HBox hBoxPagination = new HBox() { Halign = Align.Center };

            if (offset >= maxRowsToPage)
            {
                LinkButton linkButtonLast = new LinkButton("", " Попередня сторінка") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
                linkButtonLast.Clicked += (object? sender, EventArgs args) =>
                {
                    if (offset >= maxRowsToPage) offset -= maxRowsToPage;
                    Find(entryFullTextSearch.Text, offset);
                };

                hBoxPagination.PackStart(linkButtonLast, false, false, 0);
            }

            if (count == maxRowsToPage)
            {
                LinkButton linkButtonNext = new LinkButton("", " Наступна сторінка") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
                linkButtonNext.Clicked += (object? sender, EventArgs args) =>
                {
                    Find(entryFullTextSearch.Text, offset += maxRowsToPage);
                };

                hBoxPagination.PackStart(linkButtonNext, false, false, 0);
            }

            vBoxMessage.PackStart(hBoxPagination, false, false, 10);
        }
    }
}