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
    class Блокнот_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Блокнот_ШвидкийВибір() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.Блокнот_ЗаписиШвидкийВибір.Store;
            ТабличніСписки.Блокнот_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            //Сторінка
            {
                LinkButton linkPage = new LinkButton($" {Блокнот_Const.FULLNAME}") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
                linkPage.Clicked += async (object? sender, EventArgs args) =>
                {
                    Блокнот page = new Блокнот()
                    {
                        DirectoryPointerItem = DirectoryPointerItem,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer
                    };

                    Program.GeneralForm?.CreateNotebookPage($"Вибір - {Блокнот_Const.FULLNAME}", () => { return page; }, true);

                    await page.LoadRecords();
                };

                HBoxTop.PackStart(linkPage, false, false, 10);
            }

            //Новий
            {
                LinkButton linkNew = new LinkButton("Новий");
                linkNew.Clicked += (object? sender, EventArgs args) =>
                {
                    Блокнот_Елемент page = new Блокнот_Елемент
                    {
                        IsNew = true,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer
                    };

                    Program.GeneralForm?.CreateNotebookPage($"{Блокнот_Const.FULLNAME} *", () => { return page; }, true);

                    page.SetValue();
                };

                HBoxTop.PackStart(linkNew, false, false, 0);
            }
        }

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.Блокнот_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Блокнот_ЗаписиШвидкийВибір.Where.Clear();

            await ТабличніСписки.Блокнот_ЗаписиШвидкийВибір.LoadRecords();

            if (ТабличніСписки.Блокнот_ЗаписиШвидкийВибір.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Блокнот_ЗаписиШвидкийВибір.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
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

            await ТабличніСписки.Блокнот_ЗаписиШвидкийВибір.LoadRecords();
        }
    }
}