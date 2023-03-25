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
    class ХарактеристикиНоменклатури_ШвидкийВибір : VBox
    {
        public Popover? PopoverParent { get; set; }
        public ХарактеристикиНоменклатури_Pointer? DirectoryPointerItem { get; set; }
        public System.Action<ХарактеристикиНоменклатури_Pointer>? CallBack_OnSelectPointer { get; set; }

        TreeView TreeViewGrid;
        public Номенклатура_PointerControl НоменклатураВласник = new Номенклатура_PointerControl() { WidthPresentation = 100 };
        SearchControl2 ПошукПовнотекстовий = new SearchControl2();

        public ХарактеристикиНоменклатури_ШвидкийВибір(bool IsSelectPointer = false) : base()
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
                LinkButton linkPage = new LinkButton(" Характеристики") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
                linkPage.Clicked += (object? sender, EventArgs args) =>
                {
                    ХарактеристикиНоменклатури page = new ХарактеристикиНоменклатури();
                    page.DirectoryPointerItem = DirectoryPointerItem;
                    page.НоменклатураВласник.Pointer = НоменклатураВласник.Pointer;
                    page.CallBack_OnSelectPointer = CallBack_OnSelectPointer;

                    Program.GeneralForm?.CreateNotebookPage("Вибір - Характеристики", () => { return page; }, true);

                    page.LoadRecords();
                };

                hBoxTop.PackStart(linkPage, false, false, 10);
            }

            //Новий
            {
                LinkButton linkNew = new LinkButton("Новий");
                linkNew.Clicked += (object? sender, EventArgs args) =>
                {
                    ХарактеристикиНоменклатури_Елемент page = new ХарактеристикиНоменклатури_Елемент
                    {
                        IsNew = true,
                        НоменклатураДляНового = НоменклатураВласник.Pointer,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer
                    };

                    Program.GeneralForm?.CreateNotebookPage($"Характеристики: *", () => { return page; }, true);

                    page.SetValue();
                };

                hBoxTop.PackStart(linkNew, false, false, 0);
            }

            //Власник
            hBoxTop.PackStart(НоменклатураВласник, false, false, 2);
            НоменклатураВласник.Caption = "Номенклатура:";
            НоменклатураВласник.AfterSelectFunc = () =>
            {
                LoadRecords();
            };

            //Очистка
            {
                LinkButton linkClear = new LinkButton(" Очистити") { Image = new Image(AppContext.BaseDirectory + "images/clean.png"), AlwaysShowImage = true };
                linkClear.Clicked += (object? sender, EventArgs args) =>
                {
                    if (CallBack_OnSelectPointer != null)
                        CallBack_OnSelectPointer.Invoke(new ХарактеристикиНоменклатури_Pointer());

                    if (PopoverParent != null)
                        PopoverParent.Hide();
                };

                hBoxTop.PackEnd(linkClear, false, false, 0);
            }

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = 600, HeightRequest = 300 };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.ХарактеристикиНоменклатури_ЗаписиШвидкийВибір.Store);
            ТабличніСписки.ХарактеристикиНоменклатури_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;
            TreeViewGrid.ButtonPressEvent += OnButtonPressEvent;

            scrollTree.Add(TreeViewGrid);

            PackStart(scrollTree, true, true, 0);

            ShowAll();
        }

        public void LoadRecords()
        {
            ТабличніСписки.ХарактеристикиНоменклатури_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ХарактеристикиНоменклатури_ЗаписиШвидкийВибір.Where.Clear();

            if (!НоменклатураВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.ХарактеристикиНоменклатури_ЗаписиШвидкийВибір.Where.Add(
                    new Where(ХарактеристикиНоменклатури_Const.Номенклатура, Comparison.EQ, НоменклатураВласник.Pointer.UnigueID.UGuid));
            }

            ТабличніСписки.ХарактеристикиНоменклатури_ЗаписиШвидкийВибір.LoadRecords();

            if (ТабличніСписки.ХарактеристикиНоменклатури_ЗаписиШвидкийВибір.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ХарактеристикиНоменклатури_ЗаписиШвидкийВибір.SelectPath, TreeViewGrid.Columns[0], false);
        }

        void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ХарактеристикиНоменклатури_ЗаписиШвидкийВибір.Where.Clear();

            if (!НоменклатураВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.ХарактеристикиНоменклатури_ЗаписиШвидкийВибір.Where.Add(
                    new Where(ХарактеристикиНоменклатури_Const.Номенклатура, Comparison.EQ, НоменклатураВласник.Pointer.UnigueID.UGuid));
            }

            //Назва
            ТабличніСписки.ХарактеристикиНоменклатури_ЗаписиШвидкийВибір.Where.Add(
                new Where(Comparison.AND, ХарактеристикиНоменклатури_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.ХарактеристикиНоменклатури_ЗаписиШвидкийВибір.LoadRecords();
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
                        CallBack_OnSelectPointer.Invoke(new ХарактеристикиНоменклатури_Pointer(new UnigueID(uid)));

                    if (PopoverParent != null)
                        PopoverParent.Hide();
                }
            }
        }

        #endregion
    }
}