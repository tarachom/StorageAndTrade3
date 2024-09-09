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
    class ХарактеристикиНоменклатури_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Номенклатура_PointerControl НоменклатураВласник = new Номенклатура_PointerControl() { WidthPresentation = 100 };

        public ХарактеристикиНоменклатури_ШвидкийВибір(bool IsSelectPointer = false)
        {
            ТабличніСписки.ХарактеристикиНоменклатури_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            //Власник
            HBoxTop.PackStart(НоменклатураВласник, false, false, 2);
            НоменклатураВласник.Caption = $"{Номенклатура_Const.FULLNAME}:";
            НоменклатураВласник.AfterSelectFunc = async () => { await LoadRecords(); };
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ХарактеристикиНоменклатури_ЗаписиШвидкийВибір.SelectPointerItem = null;
            ТабличніСписки.ХарактеристикиНоменклатури_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ХарактеристикиНоменклатури_ЗаписиШвидкийВибір.ОчиститиВідбір(TreeViewGrid);

            if (!НоменклатураВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.ХарактеристикиНоменклатури_ЗаписиШвидкийВибір.ДодатиВідбір(TreeViewGrid,
                    new Where(ХарактеристикиНоменклатури_Const.Номенклатура, Comparison.EQ, НоменклатураВласник.Pointer.UnigueID.UGuid));
            }

            await ТабличніСписки.ХарактеристикиНоменклатури_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!НоменклатураВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.ХарактеристикиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ХарактеристикиНоменклатури_Const.Номенклатура, Comparison.EQ, НоменклатураВласник.Pointer.UnigueID.UGuid));
            }

            //Відбори
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid, ХарактеристикиНоменклатури_ВідбориДляПошуку.Відбори(searchText));

            await ТабличніСписки.ХарактеристикиНоменклатури_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            ХарактеристикиНоменклатури page = new ХарактеристикиНоменклатури()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                OpenFolder = OpenFolder
            };

            page.НоменклатураВласник.Pointer = НоменклатураВласник.Pointer;

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ХарактеристикиНоменклатури_Const.FULLNAME}", () => page);

            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            ХарактеристикиНоменклатури_Елемент page = new ХарактеристикиНоменклатури_Елемент
            {
                IsNew = IsNew,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer
            };

            if (IsNew)
            {
                await page.Елемент.New();
                page.НоменклатураДляНового = НоменклатураВласник.Pointer;
            }
            else if (unigueID == null || !await page.Елемент.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return;
            }

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);

            page.SetValue();
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            ХарактеристикиНоменклатури_Objest Обєкт = new ХарактеристикиНоменклатури_Objest();
            if (await Обєкт.Read(unigueID))
                await Обєкт.SetDeletionLabel(!Обєкт.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }
    }
}