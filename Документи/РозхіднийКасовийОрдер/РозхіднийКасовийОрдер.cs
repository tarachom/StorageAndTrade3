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
    public class РозхіднийКасовийОрдер : ДокументЖурнал
    {
        public РозхіднийКасовийОрдер() 
        {
            ТабличніСписки.РозхіднийКасовийОрдер_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.РозхіднийКасовийОрдер_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.РозхіднийКасовийОрдер_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.РозхіднийКасовийОрдер_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.РозхіднийКасовийОрдер_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            //Назва
            ТабличніСписки.РозхіднийКасовийОрдер_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(РозхіднийКасовийОрдер_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.РозхіднийКасовийОрдер_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.РозхіднийКасовийОрдер_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            РозхіднийКасовийОрдер_Елемент page = new РозхіднийКасовийОрдер_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                IsNew = IsNew
            };

            if (IsNew)
                await page.Елемент.New();
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
            РозхіднийКасовийОрдер_Objest РозхіднийКасовийОрдер_Objest = new РозхіднийКасовийОрдер_Objest();
            if (await РозхіднийКасовийОрдер_Objest.Read(unigueID))
                await РозхіднийКасовийОрдер_Objest.SetDeletionLabel(!РозхіднийКасовийОрдер_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            РозхіднийКасовийОрдер_Objest РозхіднийКасовийОрдер_Objest = new РозхіднийКасовийОрдер_Objest();
            if (await РозхіднийКасовийОрдер_Objest.Read(unigueID))
            {
                РозхіднийКасовийОрдер_Objest РозхіднийКасовийОрдер_Objest_Новий = await РозхіднийКасовийОрдер_Objest.Copy(true);
                await РозхіднийКасовийОрдер_Objest_Новий.Save();

                return РозхіднийКасовийОрдер_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "Документи.РозхіднийКасовийОрдер";

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
            РозхіднийКасовийОрдер_Pointer РозхіднийКасовийОрдер_Pointer = new РозхіднийКасовийОрдер_Pointer(unigueID);
            РозхіднийКасовийОрдер_Objest? РозхіднийКасовийОрдер_Objest = await РозхіднийКасовийОрдер_Pointer.GetDocumentObject(true);
            if (РозхіднийКасовийОрдер_Objest == null) return;

            if (spendDoc)
            {
                if (!await РозхіднийКасовийОрдер_Objest.SpendTheDocument(РозхіднийКасовийОрдер_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(РозхіднийКасовийОрдер_Objest.UnigueID);
            }
            else
                await РозхіднийКасовийОрдер_Objest.ClearSpendTheDocument();
        }

        protected override void ReportSpendTheDocument(UnigueID unigueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new РозхіднийКасовийОрдер_Pointer(unigueID));
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{РозхіднийКасовийОрдер_Const.FULLNAME}_{unigueID}.xml");
            await РозхіднийКасовийОрдер_Export.ToXmlFile(new РозхіднийКасовийОрдер_Pointer(unigueID), pathToSave);
        }

        #endregion
    }
}