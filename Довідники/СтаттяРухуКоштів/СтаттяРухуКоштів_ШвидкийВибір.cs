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
    class СтаттяРухуКоштів_ШвидкийВибір : VBox
    {
        public Popover? PopoverParent { get; set; }
        public СтаттяРухуКоштів_Pointer? DirectoryPointerItem { get; set; }
        public System.Action<СтаттяРухуКоштів_Pointer>? CallBack_OnSelectPointer { get; set; }

        TreeView TreeViewGrid;
        SearchControl2 ПошукПовнотекстовий = new SearchControl2();

        public СтаттяРухуКоштів_ШвидкийВибір(bool IsSelectPointer = false) : base()
        {
            new VBox(false, 0);
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
                LinkButton linkPage = new LinkButton(" Статті руху коштів") { Halign = Align.Start, Image = new Image("images/doc.png"), AlwaysShowImage = true };
                linkPage.Clicked += (object? sender, EventArgs args) =>
                {
                    СтаттяРухуКоштів page = new СтаттяРухуКоштів();
                    page.DirectoryPointerItem = DirectoryPointerItem;
                    page.CallBack_OnSelectPointer = CallBack_OnSelectPointer;

                    Program.GeneralForm?.CreateNotebookPage("Вибір - Статті руху коштів", () => { return page; }, true);

                    page.LoadRecords();
                };

                hBoxTop.PackStart(linkPage, false, false, 10);
            }

            //Очистка
            {
                Button bClear = new Button(new Image("images/clean.png"));
                bClear.Clicked += (object? sender, EventArgs args) =>
                {
                    if (CallBack_OnSelectPointer != null)
                        CallBack_OnSelectPointer.Invoke(new СтаттяРухуКоштів_Pointer());

                    if (PopoverParent != null)
                        PopoverParent.Hide();
                };

                hBoxTop.PackEnd(bClear, false, false, 10);
            }

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = 600, HeightRequest = 300 };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.СтаттяРухуКоштів_ЗаписиШвидкийВибір.Store);
            ТабличніСписки.СтаттяРухуКоштів_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;
            TreeViewGrid.ButtonPressEvent += OnButtonPressEvent;

            scrollTree.Add(TreeViewGrid);

            PackStart(scrollTree, true, true, 0);

            ShowAll();
        }

        public void LoadRecords()
        {
            ТабличніСписки.СтаттяРухуКоштів_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СтаттяРухуКоштів_ЗаписиШвидкийВибір.Where.Clear();

            ТабличніСписки.СтаттяРухуКоштів_ЗаписиШвидкийВибір.LoadRecords();

            if (ТабличніСписки.СтаттяРухуКоштів_ЗаписиШвидкийВибір.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.СтаттяРухуКоштів_ЗаписиШвидкийВибір.SelectPath, TreeViewGrid.Columns[0], false);
        }

        void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.СтаттяРухуКоштів_ЗаписиШвидкийВибір.Where.Clear();

            //Назва
            ТабличніСписки.СтаттяРухуКоштів_ЗаписиШвидкийВибір.Where.Add(
                new Where(СтаттяРухуКоштів_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.СтаттяРухуКоштів_ЗаписиШвидкийВибір.LoadRecords();
        }

        #region TreeView

        void OnButtonPressEvent(object? sender, ButtonPressEventArgs args)
        {
            if (args.Event.Type == Gdk.EventType.DoubleButtonPress && TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;

                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    if (CallBack_OnSelectPointer != null)
                        CallBack_OnSelectPointer.Invoke(new СтаттяРухуКоштів_Pointer(new UnigueID(uid)));

                    if (PopoverParent != null)
                        PopoverParent.Hide();
                }
            }
        }

        #endregion
    }
}