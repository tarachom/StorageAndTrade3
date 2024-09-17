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
    public class ДоговориКонтрагентів : ДовідникЖурнал
    {
        public Контрагенти_PointerControl КонтрагентВласник = new Контрагенти_PointerControl();

        public ДоговориКонтрагентів() 
        {
            //Власник
            HBoxTop.PackStart(КонтрагентВласник, false, false, 2);
            КонтрагентВласник.Caption = $"{Контрагенти_Const.FULLNAME}:";
            КонтрагентВласник.AfterSelectFunc = async () => await LoadRecords();

            ТабличніСписки.ДоговориКонтрагентів_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ДоговориКонтрагентів_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ДоговориКонтрагентів_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ДоговориКонтрагентів_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!КонтрагентВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.ДоговориКонтрагентів_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ДоговориКонтрагентів_Const.Контрагент, Comparison.EQ, КонтрагентВласник.Pointer.UnigueID.UGuid));
            }

            await ТабличніСписки.ДоговориКонтрагентів_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ДоговориКонтрагентів_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!КонтрагентВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.ДоговориКонтрагентів_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ДоговориКонтрагентів_Const.Контрагент, Comparison.EQ, КонтрагентВласник.Pointer.UnigueID.UGuid));
            }

            //Відбори
            ТабличніСписки.ДоговориКонтрагентів_Записи.ДодатиВідбір(TreeViewGrid, ДоговориКонтрагентів_Функції.Відбори(searchText));

            await ТабличніСписки.ДоговориКонтрагентів_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.ДоговориКонтрагентів_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ДоговориКонтрагентів_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ДоговориКонтрагентів_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ДоговориКонтрагентів_Функції.Copy(unigueID);
        }

        #endregion
    }
}