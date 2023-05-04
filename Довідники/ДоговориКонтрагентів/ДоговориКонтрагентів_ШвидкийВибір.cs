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
    class ДоговориКонтрагентів_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Контрагенти_PointerControl КонтрагентВласник = new Контрагенти_PointerControl() { WidthPresentation = 100 };

        public ДоговориКонтрагентів_ШвидкийВибір() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.Store;
            ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            //Сторінка
            {
                LinkButton linkPage = new LinkButton($" {ДоговориКонтрагентів_Const.FULLNAME}") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
                linkPage.Clicked += (object? sender, EventArgs args) =>
                {
                    ДоговориКонтрагентів page = new ДоговориКонтрагентів();
                    page.DirectoryPointerItem = DirectoryPointerItem;
                    page.КонтрагентВласник.Pointer = КонтрагентВласник.Pointer;
                    page.CallBack_OnSelectPointer = CallBack_OnSelectPointer;

                    Program.GeneralForm?.CreateNotebookPage($"Вибір - {ДоговориКонтрагентів_Const.FULLNAME}", () => { return page; }, true);

                    page.LoadRecords();
                };

                HBoxTop.PackStart(linkPage, false, false, 10);
            }

            //Новий
            {
                LinkButton linkNew = new LinkButton("Новий");
                linkNew.Clicked += (object? sender, EventArgs args) =>
                {
                    ДоговориКонтрагентів_Елемент page = new ДоговориКонтрагентів_Елемент { IsNew = true, CallBack_OnSelectPointer = CallBack_OnSelectPointer };

                    Program.GeneralForm?.CreateNotebookPage($"{ДоговориКонтрагентів_Const.FULLNAME} *", () => { return page; }, true);

                    page.SetValue();
                };

                HBoxTop.PackStart(linkNew, false, false, 0);
            }

            //Власник
            HBoxTop.PackStart(КонтрагентВласник, false, false, 2);
            КонтрагентВласник.Caption = "Контрагент:";
            КонтрагентВласник.AfterSelectFunc = () =>
            {
                DirectoryPointerItem?.Clear();
                LoadRecords();
            };

            //Очистка
            {
                LinkButton linkClear = new LinkButton(" Очистити") { Image = new Image(AppContext.BaseDirectory + "images/clean.png"), AlwaysShowImage = true };
                linkClear.Clicked += (object? sender, EventArgs args) =>
                {
                    if (CallBack_OnSelectPointer != null)
                        CallBack_OnSelectPointer.Invoke(new UnigueID());

                    if (PopoverParent != null)
                        PopoverParent.Hide();
                };

                HBoxTop.PackEnd(linkClear, false, false, 10);
            }
        }

        public override void LoadRecords()
        {
            ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.Where.Clear();

            if (!КонтрагентВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.Where.Add(
                    new Where(ДоговориКонтрагентів_Const.Контрагент, Comparison.EQ, КонтрагентВласник.Pointer.UnigueID.UGuid));
            }

            ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.LoadRecords();

            if (ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.Where.Clear();

            if (!КонтрагентВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.Where.Add(
                    new Where(ДоговориКонтрагентів_Const.Контрагент, Comparison.EQ, КонтрагентВласник.Pointer.UnigueID.UGuid));
            }

            //Назва
            ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.Where.Add(
                new Where(Comparison.AND, ДоговориКонтрагентів_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.LoadRecords();
        }
    }
}