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
    class СкладськіПриміщення_ШвидкийВибір : VBox
    {
        public Popover? PopoverParent { get; set; }
        public СкладськіПриміщення_Pointer? DirectoryPointerItem { get; set; }
        public System.Action<СкладськіПриміщення_Pointer>? CallBack_OnSelectPointer { get; set; }

        TreeView TreeViewGrid;
        public Склади_PointerControl СкладВласник = new Склади_PointerControl();
        SearchControl2 ПошукПовнотекстовий = new SearchControl2();

        public СкладськіПриміщення_ШвидкийВибір(bool IsSelectPointer = false) : base()
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
                LinkButton linkPage = new LinkButton(" Складські приміщення") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
                linkPage.Clicked += (object? sender, EventArgs args) =>
                {
                    СкладськіПриміщення page = new СкладськіПриміщення();
                    page.DirectoryPointerItem = DirectoryPointerItem;
                    page.СкладВласник.Pointer = СкладВласник.Pointer;
                    page.CallBack_OnSelectPointer = CallBack_OnSelectPointer;

                    Program.GeneralForm?.CreateNotebookPage("Вибір - Складські приміщення", () => { return page; }, true);

                    page.LoadRecords();
                };

                hBoxTop.PackStart(linkPage, false, false, 10);
            }

            //Новий
            {
                LinkButton linkNew = new LinkButton("Новий");
                linkNew.Clicked += (object? sender, EventArgs args) =>
                {
                    СкладськіПриміщення_Елемент page = new СкладськіПриміщення_Елемент { IsNew = true };

                    Program.GeneralForm?.CreateNotebookPage($"Складські приміщення: *", () => { return page; }, true);

                    page.SetValue();
                };

                hBoxTop.PackStart(linkNew, false, false, 0);
            }

            //Власник
            hBoxTop.PackStart(СкладВласник, false, false, 2);
            СкладВласник.Caption = "Склад:";
            СкладВласник.AfterSelectFunc = () =>
            {
                LoadRecords();
            };

            //Очистка
            {
                LinkButton linkClear = new LinkButton(" Очистити") { Image = new Image(AppContext.BaseDirectory + "images/clean.png"), AlwaysShowImage = true };
                linkClear.Clicked += (object? sender, EventArgs args) =>
                {
                    if (CallBack_OnSelectPointer != null)
                        CallBack_OnSelectPointer.Invoke(new СкладськіПриміщення_Pointer());

                    if (PopoverParent != null)
                        PopoverParent.Hide();
                };

                hBoxTop.PackEnd(linkClear, false, false, 10);
            }

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = 600, HeightRequest = 300 };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.Store);
            ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;
            TreeViewGrid.ButtonPressEvent += OnButtonPressEvent;

            scrollTree.Add(TreeViewGrid);

            PackStart(scrollTree, true, true, 0);

            ShowAll();
        }

        public void LoadRecords()
        {
            ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.Where.Clear();

            if (!СкладВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.Where.Add(
                    new Where(СкладськіПриміщення_Const.Склад, Comparison.EQ, СкладВласник.Pointer.UnigueID.UGuid));
            }

            ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.LoadRecords();

            if (ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.SelectPath, TreeViewGrid.Columns[0], false);
        }

        void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.Where.Clear();

            if (!СкладВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.Where.Add(
                    new Where(СкладськіПриміщення_Const.Склад, Comparison.EQ, СкладВласник.Pointer.UnigueID.UGuid));
            }

            //Назва
            ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.Where.Add(
                new Where(Comparison.AND, СкладськіПриміщення_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.LoadRecords();
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
                        CallBack_OnSelectPointer.Invoke(new СкладськіПриміщення_Pointer(new UnigueID(uid)));

                    if (PopoverParent != null)
                        PopoverParent.Hide();
                }
            }
        }

        #endregion
    }
}