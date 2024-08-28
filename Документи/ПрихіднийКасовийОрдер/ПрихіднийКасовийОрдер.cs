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
    public class ПрихіднийКасовийОрдер : ДокументЖурнал
    {
        public ПрихіднийКасовийОрдер() : base()
        {
            ТабличніСписки.ПрихіднийКасовийОрдер_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async void LoadRecords()
        {
            ТабличніСписки.ПрихіднийКасовийОрдер_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПрихіднийКасовийОрдер_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ПрихіднийКасовийОрдер_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ПрихіднийКасовийОрдер_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ПрихіднийКасовийОрдер_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПрихіднийКасовийОрдер_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ПрихіднийКасовийОрдер_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПрихіднийКасовийОрдер_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Назва
            ТабличніСписки.ПрихіднийКасовийОрдер_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(ПрихіднийКасовийОрдер_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.ПрихіднийКасовийОрдер_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ПрихіднийКасовийОрдер_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПрихіднийКасовийОрдер_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask<(string Name, Func<Widget>? FuncWidget, System.Action? SetValue)> OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            ПрихіднийКасовийОрдер_Елемент page = new ПрихіднийКасовийОрдер_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                IsNew = IsNew
            };

            if (IsNew)
                await page.ПрихіднийКасовийОрдер_Objest.New();
            else if (unigueID == null || !await page.ПрихіднийКасовийОрдер_Objest.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return ("", null, null);
            }

            return (page.Caption, () => page, page.SetValue);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            ПрихіднийКасовийОрдер_Objest ПрихіднийКасовийОрдер_Objest = new ПрихіднийКасовийОрдер_Objest();
            if (await ПрихіднийКасовийОрдер_Objest.Read(unigueID))
                await ПрихіднийКасовийОрдер_Objest.SetDeletionLabel(!ПрихіднийКасовийОрдер_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ПрихіднийКасовийОрдер_Objest ПрихіднийКасовийОрдер_Objest = new ПрихіднийКасовийОрдер_Objest();
            if (await ПрихіднийКасовийОрдер_Objest.Read(unigueID))
            {
                ПрихіднийКасовийОрдер_Objest ПрихіднийКасовийОрдер_Objest_Новий = await ПрихіднийКасовийОрдер_Objest.Copy(true);
                await ПрихіднийКасовийОрдер_Objest_Новий.Save();

                return ПрихіднийКасовийОрдер_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "Документи.ПрихіднийКасовийОрдер";

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
            ПрихіднийКасовийОрдер_Pointer ПрихіднийКасовийОрдер_Pointer = new ПрихіднийКасовийОрдер_Pointer(unigueID);
            ПрихіднийКасовийОрдер_Objest? ПрихіднийКасовийОрдер_Objest = await ПрихіднийКасовийОрдер_Pointer.GetDocumentObject(true);
            if (ПрихіднийКасовийОрдер_Objest == null) return;

            if (spendDoc)
            {
                if (!await ПрихіднийКасовийОрдер_Objest.SpendTheDocument(ПрихіднийКасовийОрдер_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(ПрихіднийКасовийОрдер_Objest.UnigueID);
            }
            else
                await ПрихіднийКасовийОрдер_Objest.ClearSpendTheDocument();
        }

        protected override void ReportSpendTheDocument(UnigueID unigueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ПрихіднийКасовийОрдер_Pointer(unigueID));
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{ПрихіднийКасовийОрдер_Const.FULLNAME}_{unigueID}.xml");
            await ПрихіднийКасовийОрдер_Export.ToXmlFile(new ПрихіднийКасовийОрдер_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar


        #endregion
    }
}