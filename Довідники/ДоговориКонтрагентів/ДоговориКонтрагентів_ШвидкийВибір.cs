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
    class ДоговориКонтрагентів_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Контрагенти_PointerControl КонтрагентВласник = new Контрагенти_PointerControl() { Caption = "Контрагент:", WidthPresentation = 100 };

        public ДоговориКонтрагентів_ШвидкийВибір() 
        {
            ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            //Власник
            HBoxTop.PackStart(КонтрагентВласник, false, false, 2);
            КонтрагентВласник.AfterSelectFunc = async () => await LoadRecords();
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.SelectPointerItem = null;
            ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.ОчиститиВідбір(TreeViewGrid);

            if (!КонтрагентВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.ДодатиВідбір(TreeViewGrid,
                    new Where(ДоговориКонтрагентів_Const.Контрагент, Comparison.EQ, КонтрагентВласник.Pointer.UnigueID.UGuid));
            }

            await ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.ОчиститиВідбір(TreeViewGrid);

            if (!КонтрагентВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.ДодатиВідбір(TreeViewGrid,
                    new Where(ДоговориКонтрагентів_Const.Контрагент, Comparison.EQ, КонтрагентВласник.Pointer.UnigueID.UGuid));
            }

            //Відбори
            ТабличніСписки.ДоговориКонтрагентів_Записи.ДодатиВідбір(TreeViewGrid, ДоговориКонтрагентів_Функції.Відбори(searchText));

            await ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            ДоговориКонтрагентів page = new ДоговориКонтрагентів()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                OpenFolder = OpenFolder
            };

            page.КонтрагентВласник.Pointer = КонтрагентВласник.Pointer;

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ДоговориКонтрагентів_Const.FULLNAME}", () => page);

            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            ДоговориКонтрагентів_Елемент page = new ДоговориКонтрагентів_Елемент
            {
                IsNew = IsNew,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer
            };

            if (IsNew)
            {
                await page.Елемент.New();
                page.КонтрагентиДляНового = КонтрагентВласник.Pointer;
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
            await ДоговориКонтрагентів_Функції.SetDeletionLabel(unigueID);

        }
    }
}