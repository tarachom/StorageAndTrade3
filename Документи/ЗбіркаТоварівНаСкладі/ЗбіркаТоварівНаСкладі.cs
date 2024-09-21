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

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    public class ЗбіркаТоварівНаСкладі : ДокументЖурнал
    {
        public ЗбіркаТоварівНаСкладі()
        {
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.ДодатиВідбір(TreeViewGrid, ЗбіркаТоварівНаСкладі_Функції.Відбори(searchText));

            await ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ЗбіркаТоварівНаСкладі_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ЗбіркаТоварівНаСкладі_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ЗбіркаТоварівНаСкладі_Функції.Copy(unigueID);
        }

        const string КлючНалаштуванняКористувача = "Документи.ЗбіркаТоварівНаСкладі";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await LoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ЗбіркаТоварівНаСкладі_Objest? Обєкт = await new ЗбіркаТоварівНаСкладі_Pointer(unigueID).GetDocumentObject(true);
            if (Обєкт == null) return;

            if (spendDoc)
            {
                if (!await Обєкт.SpendTheDocument(Обєкт.ДатаДок))
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(Обєкт.UnigueID);
            }
            else
                await Обєкт.ClearSpendTheDocument();
        }

        protected override void ReportSpendTheDocument(UnigueID unigueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ЗбіркаТоварівНаСкладі_Pointer(unigueID));
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{ЗбіркаТоварівНаСкладі_Const.FULLNAME}_{unigueID}.xml");
            await ЗбіркаТоварівНаСкладі_Export.ToXmlFile(new ЗбіркаТоварівНаСкладі_Pointer(unigueID), pathToSave);
        }

        #endregion
    }
}