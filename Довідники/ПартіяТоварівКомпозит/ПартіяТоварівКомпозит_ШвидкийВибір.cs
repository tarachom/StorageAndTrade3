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
using StorageAndTrade_1_0.РегістриНакопичення;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class ПартіяТоварівКомпозит_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Номенклатура_PointerControl НоменклатураВідбір = new Номенклатура_PointerControl() { WidthPresentation = 100 };

        public ПартіяТоварівКомпозит_ШвидкийВибір() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.ПартіяТоварівКомпозит_ЗаписиШвидкийВибір.Store;
            ТабличніСписки.ПартіяТоварівКомпозит_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            //Сторінка
            {
                LinkButton linkPage = new LinkButton($" {ПартіяТоварівКомпозит_Const.FULLNAME}") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
                linkPage.Clicked += async (object? sender, EventArgs args) =>
                {
                    ПартіяТоварівКомпозит page = new ПартіяТоварівКомпозит()
                    {
                        DirectoryPointerItem = DirectoryPointerItem,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer
                    };

                    Program.GeneralForm?.CreateNotebookPage($"Вибір - {ПартіяТоварівКомпозит_Const.FULLNAME}", () => { return page; }, true);

                    await page.LoadRecords();
                };

                HBoxTop.PackStart(linkPage, false, false, 10);
            }

            //Відбір
            HBoxTop.PackStart(НоменклатураВідбір, false, false, 2);
            НоменклатураВідбір.Caption = $"{Номенклатура_Const.FULLNAME}:";
            НоменклатураВідбір.AfterSelectFunc = async () => { await LoadRecords(); };
        }

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ПартіяТоварівКомпозит_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ПартіяТоварівКомпозит_ЗаписиШвидкийВибір.Where.Clear();

            if (!НоменклатураВідбір.Pointer.IsEmpty())
            {
                /*
                Якщо вибрана Номенклатура тоді показуємо тільки партії, які є в наявності.
                Використовується таблиця Підсумки з регістру ПартіїТоварів
                */

                //Відбір партій які є в наявності по номенклатурі
                string query = @$"
SELECT DISTINCT
    ПартіїТоварів_Підсумки.{ПартіїТоварів_Підсумки_TablePart.ПартіяТоварівКомпозит}
FROM 
    {ПартіїТоварів_Підсумки_TablePart.TABLE} AS ПартіїТоварів_Підсумки
WHERE
    ПартіїТоварів_Підсумки.{ПартіїТоварів_Підсумки_TablePart.Номенклатура} = '{НоменклатураВідбір.Pointer.UnigueID}' AND
    ПартіїТоварів_Підсумки.{ПартіїТоварів_Підсумки_TablePart.Кількість} > 0
";

                //Додатково показується партія яка вже вибрана, навіть якщо її немає в залишку через об'єднання (UNION)
                if (DirectoryPointerItem != null && !DirectoryPointerItem.IsEmpty())
                    query = @$"( {query} ) UNION ( SELECT '{DirectoryPointerItem}' )";

                ТабличніСписки.ПартіяТоварівКомпозит_ЗаписиШвидкийВибір.Where.Add(new Where("uid", Comparison.IN, query, true));
            }

            await ТабличніСписки.ПартіяТоварівКомпозит_ЗаписиШвидкийВибір.LoadRecords();

            if (ТабличніСписки.ПартіяТоварівКомпозит_ЗаписиШвидкийВибір.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПартіяТоварівКомпозит_ЗаписиШвидкийВибір.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ПартіяТоварівКомпозит_ЗаписиШвидкийВибір.Where.Clear();

            //Назва
            ТабличніСписки.ПартіяТоварівКомпозит_ЗаписиШвидкийВибір.Where.Add(
                new Where(Comparison.OR, ПартіяТоварівКомпозит_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            await ТабличніСписки.ПартіяТоварівКомпозит_ЗаписиШвидкийВибір.LoadRecords();
        }
    }
}