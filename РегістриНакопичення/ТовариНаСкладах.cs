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

using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = StorageAndTrade_1_0.РегістриНакопичення.ТабличніСписки;

namespace StorageAndTrade
{
    class ТовариНаСкладах : РегістриЖурнал
    {
        public ТовариНаСкладах() : base()
        {
            ТабличніСписки.ТовариНаСкладах_Записи.AddColumns(TreeViewGrid);
        }

        protected override async void LoadRecords()
        {
            ТабличніСписки.ТовариНаСкладах_Записи.SelectPointerItem = SelectPointerItem;

            ТабличніСписки.ТовариНаСкладах_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ТовариНаСкладах_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ТовариНаСкладах_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ТовариНаСкладах_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ТовариНаСкладах_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ТовариНаСкладах_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ТовариНаСкладах_Записи.ОчиститиВідбір(TreeViewGrid);

            //period
            ТабличніСписки.ТовариНаСкладах_Записи.ДодатиВідбір(TreeViewGrid,
                new Where("period", Comparison.LIKE, searchText)
                {
                    FuncToField = "to_char",
                    FuncToField_Param1 = "'DD.MM.YYYY'"
                }
            );

            await ТабличніСписки.ТовариНаСкладах_Записи.LoadRecords(TreeViewGrid);
        }

        const string КлючНалаштуванняКористувача = "РегістриНакопичення.ТовариНаСкладах";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період);
        }

        protected override void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            LoadRecords();           
        }
    }
}