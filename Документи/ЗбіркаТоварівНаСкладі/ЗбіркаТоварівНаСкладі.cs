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
        public ЗбіркаТоварівНаСкладі() : base()
        {
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async void LoadRecords()
        {
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            //Назва
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(ЗбіркаТоварівНаСкладі_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask<(string Name, Func<Widget>? FuncWidget, System.Action? SetValue)> OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            ЗбіркаТоварівНаСкладі_Елемент page = new ЗбіркаТоварівНаСкладі_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                IsNew = IsNew
            };

            if (IsNew)
                await page.Елемент.New();
            else if (unigueID == null || !await page.Елемент.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return ("", null, null);
            }

            return (page.Caption, () => page, page.SetValue);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            ЗбіркаТоварівНаСкладі_Objest ЗбіркаТоварівНаСкладі_Objest = new ЗбіркаТоварівНаСкладі_Objest();
            if (await ЗбіркаТоварівНаСкладі_Objest.Read(unigueID))
                await ЗбіркаТоварівНаСкладі_Objest.SetDeletionLabel(!ЗбіркаТоварівНаСкладі_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ЗбіркаТоварівНаСкладі_Objest ЗбіркаТоварівНаСкладі_Objest = new ЗбіркаТоварівНаСкладі_Objest();
            if (await ЗбіркаТоварівНаСкладі_Objest.Read(unigueID))
            {
                ЗбіркаТоварівНаСкладі_Objest ЗбіркаТоварівНаСкладі_Objest_Новий = await ЗбіркаТоварівНаСкладі_Objest.Copy(true);
                await ЗбіркаТоварівНаСкладі_Objest_Новий.Save();
                await ЗбіркаТоварівНаСкладі_Objest_Новий.Товари_TablePart.Save(true);

                return ЗбіркаТоварівНаСкладі_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "Документи.ЗбіркаТоварівНаСкладі";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
        }

        protected override void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            LoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ЗбіркаТоварівНаСкладі_Pointer ЗбіркаТоварівНаСкладі_Pointer = new ЗбіркаТоварівНаСкладі_Pointer(unigueID);
            ЗбіркаТоварівНаСкладі_Objest? ЗбіркаТоварівНаСкладі_Objest = await ЗбіркаТоварівНаСкладі_Pointer.GetDocumentObject(true);
            if (ЗбіркаТоварівНаСкладі_Objest == null) return;

            if (spendDoc)
            {
                if (!await ЗбіркаТоварівНаСкладі_Objest.SpendTheDocument(ЗбіркаТоварівНаСкладі_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(ЗбіркаТоварівНаСкладі_Objest.UnigueID);
            }
            else
                await ЗбіркаТоварівНаСкладі_Objest.ClearSpendTheDocument();
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

        #region ToolBar

        #endregion
    }
}