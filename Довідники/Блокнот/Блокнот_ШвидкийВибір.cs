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

using AccountingSoftware;

using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class Блокнот_ШвидкийВибір : VBox
    {
        public Popover? PopoverParent { get; set; }
        public Блокнот_Pointer? DirectoryPointerItem { get; set; }
        public System.Action<Блокнот_Pointer>? CallBack_OnSelectPointer { get; set; }

        TreeView TreeViewGrid;
        SearchControl2 ПошукПовнотекстовий = new SearchControl2();

        public Блокнот_ШвидкийВибір(bool IsSelectPointer = false) : base()
        {
            BorderWidth = 0;

            //Зверху
            HBox hBoxTop = new HBox();
            PackStart(hBoxTop, false, false, 5);

            //Пошук 2
            hBoxTop.PackStart(ПошукПовнотекстовий, false, false, 0);
            ПошукПовнотекстовий.Select = LoadRecords_OnSearch;
            ПошукПовнотекстовий.Clear = LoadRecords;

            //Сторінка
            {
                LinkButton linkPage = new LinkButton($" {Блокнот_Const.FULLNAME}") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
                linkPage.Clicked += (object? sender, EventArgs args) =>
                {
                    Блокнот page = new Блокнот();
                    page.DirectoryPointerItem = DirectoryPointerItem;
                    page.CallBack_OnSelectPointer = CallBack_OnSelectPointer;

                    Program.GeneralForm?.CreateNotebookPage($"Вибір - {Блокнот_Const.FULLNAME}", () => { return page; }, true);

                    page.LoadRecords();
                };

                hBoxTop.PackStart(linkPage, false, false, 10);
            }

            //Новий
            {
                LinkButton linkNew = new LinkButton("Новий");
                linkNew.Clicked += (object? sender, EventArgs args) =>
                {
                    Блокнот_Елемент page = new Блокнот_Елемент { IsNew = true, CallBack_OnSelectPointer = CallBack_OnSelectPointer };

                    Program.GeneralForm?.CreateNotebookPage($"{Блокнот_Const.FULLNAME} *", () => { return page; }, true);

                    page.SetValue();
                };

                hBoxTop.PackStart(linkNew, false, false, 0);
            }

            //Очистка
            {
                LinkButton linkClear = new LinkButton(" Очистити") { Image = new Image(AppContext.BaseDirectory + "images/clean.png"), AlwaysShowImage = true };
                linkClear.Clicked += (object? sender, EventArgs args) =>
                {
                    if (CallBack_OnSelectPointer != null)
                        CallBack_OnSelectPointer.Invoke(new Блокнот_Pointer());

                    if (PopoverParent != null)
                        PopoverParent.Hide();
                };

                hBoxTop.PackEnd(linkClear, false, false, 10);
            }

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = 600, HeightRequest = 300 };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.Блокнот_ЗаписиШвидкийВибір.Store);
            ТабличніСписки.Блокнот_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;
            TreeViewGrid.RowActivated += OnRowActivated;
            TreeViewGrid.ButtonPressEvent += OnButtonPressEvent;

            scrollTree.Add(TreeViewGrid);

            PackStart(scrollTree, true, true, 0);

            ShowAll();
        }

        public void LoadRecords()
        {
            ТабличніСписки.Блокнот_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Блокнот_ЗаписиШвидкийВибір.Where.Clear();

            ТабличніСписки.Блокнот_ЗаписиШвидкийВибір.LoadRecords();

            if (ТабличніСписки.Блокнот_ЗаписиШвидкийВибір.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Блокнот_ЗаписиШвидкийВибір.SelectPath, TreeViewGrid.Columns[0], false);
        }

        void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.Блокнот_ЗаписиШвидкийВибір.Where.Clear();

            //Код
            ТабличніСписки.Блокнот_ЗаписиШвидкийВибір.Where.Add(
                new Where(Блокнот_Const.Код, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            //Назва
            ТабличніСписки.Блокнот_ЗаписиШвидкийВибір.Where.Add(
                new Where(Comparison.OR, Блокнот_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            //Опис
            ТабличніСписки.Блокнот_ЗаписиШвидкийВибір.Where.Add(
                new Where(Comparison.OR, Блокнот_Const.Опис, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.Блокнот_ЗаписиШвидкийВибір.LoadRecords();
        }

        #region TreeView

        void OnRowActivated(object sender, RowActivatedArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]);

                UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                DirectoryPointerItem = new Блокнот_Pointer(unigueID);
            }
        }

        void OnButtonPressEvent(object? sender, ButtonPressEventArgs args)
        {
            if (args.Event.Type == Gdk.EventType.DoubleButtonPress && TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;

                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    if (CallBack_OnSelectPointer != null)
                        CallBack_OnSelectPointer.Invoke(new Блокнот_Pointer(new UnigueID(uid)));

                    if (PopoverParent != null)
                        PopoverParent.Hide();
                }
            }
        }

        #endregion
    }
}