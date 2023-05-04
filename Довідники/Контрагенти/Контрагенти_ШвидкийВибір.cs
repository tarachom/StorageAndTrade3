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
    class Контрагенти_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Контрагенти_ШвидкийВибір() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.Контрагенти_ЗаписиШвидкийВибір.Store;
            ТабличніСписки.Контрагенти_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            //Сторінка
            {
                LinkButton linkPage = new LinkButton($" {Контрагенти_Const.FULLNAME}") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
                linkPage.Clicked += (object? sender, EventArgs args) =>
                {
                    Контрагенти page = new Контрагенти();
                    page.DirectoryPointerItem = DirectoryPointerItem;
                    page.CallBack_OnSelectPointer = CallBack_OnSelectPointer;

                    Program.GeneralForm?.CreateNotebookPage($"Вибір - {Контрагенти_Const.FULLNAME}", () => { return page; }, true);

                    page.LoadRecords();
                };

                HBoxTop.PackStart(linkPage, false, false, 10);
            }

            //Новий
            {
                LinkButton linkNew = new LinkButton("Новий");
                linkNew.Clicked += (object? sender, EventArgs args) =>
                {
                    Контрагенти_Елемент page = new Контрагенти_Елемент { IsNew = true, CallBack_OnSelectPointer = CallBack_OnSelectPointer };

                    Program.GeneralForm?.CreateNotebookPage($"{Контрагенти_Const.FULLNAME} *", () => { return page; }, true);

                    page.SetValue();
                };

                HBoxTop.PackStart(linkNew, false, false, 0);
            }

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
            ТабличніСписки.Контрагенти_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Контрагенти_ЗаписиШвидкийВибір.Where.Clear();

            ТабличніСписки.Контрагенти_ЗаписиШвидкийВибір.LoadRecords();

            if (ТабличніСписки.Контрагенти_ЗаписиШвидкийВибір.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Контрагенти_ЗаписиШвидкийВибір.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.Контрагенти_ЗаписиШвидкийВибір.Where.Clear();

            //Код
            ТабличніСписки.Контрагенти_ЗаписиШвидкийВибір.Where.Add(
                new Where(Контрагенти_Const.Код, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            //Назва
            ТабличніСписки.Контрагенти_ЗаписиШвидкийВибір.Where.Add(
                new Where(Comparison.OR, Контрагенти_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.Контрагенти_ЗаписиШвидкийВибір.LoadRecords();
        }
    }
}