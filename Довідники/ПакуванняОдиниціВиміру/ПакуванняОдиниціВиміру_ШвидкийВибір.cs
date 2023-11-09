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
    class ПакуванняОдиниціВиміру_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public ПакуванняОдиниціВиміру_ШвидкийВибір() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.ПакуванняОдиниціВиміру_ЗаписиШвидкийВибір.Store;
            ТабличніСписки.ПакуванняОдиниціВиміру_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            //Сторінка
            {
                LinkButton linkPage = new LinkButton($" {ПакуванняОдиниціВиміру_Const.FULLNAME}") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
                linkPage.Clicked += (object? sender, EventArgs args) =>
                {
                    ПакуванняОдиниціВиміру page = new ПакуванняОдиниціВиміру()
                    {
                        DirectoryPointerItem = DirectoryPointerItem,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer
                    };

                    Program.GeneralForm?.CreateNotebookPage($"Вибір - {ПакуванняОдиниціВиміру_Const.FULLNAME}", () => { return page; }, true);

                    page.LoadRecords();
                };

                HBoxTop.PackStart(linkPage, false, false, 10);
            }

            //Новий
            {
                LinkButton linkNew = new LinkButton("Новий");
                linkNew.Clicked += (object? sender, EventArgs args) =>
                {
                    ПакуванняОдиниціВиміру_Елемент page = new ПакуванняОдиниціВиміру_Елемент
                    {
                        IsNew = true,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer
                    };

                    Program.GeneralForm?.CreateNotebookPage($"{ПакуванняОдиниціВиміру_Const.FULLNAME} *", () => { return page; }, true);

                    page.SetValue();
                };

                HBoxTop.PackStart(linkNew, false, false, 0);
            }
        }

        public override void LoadRecords()
        {
            ТабличніСписки.ПакуванняОдиниціВиміру_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ПакуванняОдиниціВиміру_ЗаписиШвидкийВибір.Where.Clear();

            ТабличніСписки.ПакуванняОдиниціВиміру_ЗаписиШвидкийВибір.LoadRecords();

            if (ТабличніСписки.ПакуванняОдиниціВиміру_ЗаписиШвидкийВибір.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПакуванняОдиниціВиміру_ЗаписиШвидкийВибір.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ПакуванняОдиниціВиміру_ЗаписиШвидкийВибір.Where.Clear();

            //Код
            ТабличніСписки.ПакуванняОдиниціВиміру_ЗаписиШвидкийВибір.Where.Add(
                new Where(ПакуванняОдиниціВиміру_Const.Код, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            //Назва
            ТабличніСписки.ПакуванняОдиниціВиміру_ЗаписиШвидкийВибір.Where.Add(
                new Where(Comparison.OR, ПакуванняОдиниціВиміру_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.ПакуванняОдиниціВиміру_ЗаписиШвидкийВибір.LoadRecords();
        }
    }
}