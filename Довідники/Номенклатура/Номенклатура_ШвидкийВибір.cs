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
    class Номенклатура_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Номенклатура_ШвидкийВибір() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.Номенклатура_ЗаписиШвидкийВибір.Store;
            ТабличніСписки.Номенклатура_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            //Сторінка
            {
                LinkButton linkPage = new LinkButton($" {Номенклатура_Const.FULLNAME}") { Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
                linkPage.Clicked += async (object? sender, EventArgs args) =>
                {
                    Номенклатура page = new Номенклатура()
                    {
                        DirectoryPointerItem = DirectoryPointerItem,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer
                    };

                    Program.GeneralForm?.CreateNotebookPage($"Вибір - {Номенклатура_Const.FULLNAME}", () => { return page; }, true);

                    await page.LoadRecords();
                };

                HBoxTop.PackStart(linkPage, false, false, 0);
            }

            //Новий
            {
                LinkButton linkNew = new LinkButton("Новий");
                linkNew.Clicked += (object? sender, EventArgs args) =>
                {
                    Номенклатура_Елемент page = new Номенклатура_Елемент
                    {
                        IsNew = true,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer
                    };

                    Program.GeneralForm?.CreateNotebookPage($"{Номенклатура_Const.FULLNAME} *", () => { return page; }, true);

                    page.SetValue();

                    page.Назва.Text = Пошук.Text;
                };

                HBoxTop.PackStart(linkNew, false, false, 0);
            }
        }

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.Номенклатура_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Номенклатура_ЗаписиШвидкийВибір.Where.Clear();

            await ТабличніСписки.Номенклатура_ЗаписиШвидкийВибір.LoadRecords();

            if (ТабличніСписки.Номенклатура_ЗаписиШвидкийВибір.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Номенклатура_ЗаписиШвидкийВибір.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.Номенклатура_ЗаписиШвидкийВибір.Where.Clear();

            //Код
            ТабличніСписки.Номенклатура_ЗаписиШвидкийВибір.Where.Add(
                new Where(Номенклатура_Const.Код, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            //Назва
            ТабличніСписки.Номенклатура_ЗаписиШвидкийВибір.Where.Add(
                new Where(Comparison.OR, Номенклатура_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            await ТабличніСписки.Номенклатура_ЗаписиШвидкийВибір.LoadRecords();
        }
    }
}