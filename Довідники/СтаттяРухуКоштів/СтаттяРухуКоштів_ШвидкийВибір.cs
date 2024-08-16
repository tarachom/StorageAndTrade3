/*
Copyright (C) 2019-2024 TARAKHOMYN YURIY IVANOVYCH
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
using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class СтаттяРухуКоштів_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public СтаттяРухуКоштів_ШвидкийВибір() : base()
        {
            ТабличніСписки.СтаттяРухуКоштів_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            //Сторінка
            {
                LinkButton linkPage = new LinkButton($" {СтаттяРухуКоштів_Const.FULLNAME}") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
                linkPage.Clicked += async (object? sender, EventArgs args) =>
                {
                    СтаттяРухуКоштів page = new СтаттяРухуКоштів()
                    {
                        DirectoryPointerItem = DirectoryPointerItem,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer
                    };

                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"Вибір - {СтаттяРухуКоштів_Const.FULLNAME}", () => { return page; });

                    await page.LoadRecords();
                };

                HBoxTop.PackStart(linkPage, false, false, 10);
            }

            //Новий
            {
                LinkButton linkNew = new LinkButton("Новий");
                linkNew.Clicked += (object? sender, EventArgs args) =>
                {
                    СтаттяРухуКоштів_Елемент page = new СтаттяРухуКоштів_Елемент
                    {
                        IsNew = true,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer
                    };

                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{СтаттяРухуКоштів_Const.FULLNAME} *", () => { return page; });

                    page.SetValue();
                };

                HBoxTop.PackStart(linkNew, false, false, 0);
            }
        }

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.СтаттяРухуКоштів_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СтаттяРухуКоштів_ЗаписиШвидкийВибір.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.СтаттяРухуКоштів_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.СтаттяРухуКоштів_ЗаписиШвидкийВибір.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.СтаттяРухуКоштів_ЗаписиШвидкийВибір.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Відбори
            ТабличніСписки.СтаттяРухуКоштів_Записи.ДодатиВідбір(TreeViewGrid, СтаттяРухуКоштів_ВідбориДляПошуку.Відбори(searchText), true);

            await ТабличніСписки.СтаттяРухуКоштів_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid);
        }
    }
}