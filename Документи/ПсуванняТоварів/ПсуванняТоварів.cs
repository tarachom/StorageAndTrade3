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
    public class ПсуванняТоварів : ДокументЖурнал
    {
        public ПсуванняТоварів() : base()
        {
            ТабличніСписки.ПсуванняТоварів_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async void LoadRecords()
        {
            ТабличніСписки.ПсуванняТоварів_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПсуванняТоварів_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ПсуванняТоварів_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ПсуванняТоварів_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ПсуванняТоварів_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПсуванняТоварів_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ПсуванняТоварів_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПсуванняТоварів_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Назва
            ТабличніСписки.ПсуванняТоварів_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(ПсуванняТоварів_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.ПсуванняТоварів_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ПсуванняТоварів_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПсуванняТоварів_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask<(string Name, Func<Widget>? FuncWidget, System.Action? SetValue)> OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            ПсуванняТоварів_Елемент page = new ПсуванняТоварів_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                IsNew = IsNew
            };

            if (!IsNew && unigueID != null)
                if (!await page.ПсуванняТоварів_Objest.Read(unigueID))
                {
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                    return ("", null, null);
                }

            return (IsNew ? ПсуванняТоварів_Const.FULLNAME : page.ПсуванняТоварів_Objest.Назва, () => page, page.SetValue);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            ПсуванняТоварів_Objest ПсуванняТоварів_Objest = new ПсуванняТоварів_Objest();
            if (await ПсуванняТоварів_Objest.Read(unigueID))
                await ПсуванняТоварів_Objest.SetDeletionLabel(!ПсуванняТоварів_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ПсуванняТоварів_Objest ПсуванняТоварів_Objest = new ПсуванняТоварів_Objest();
            if (await ПсуванняТоварів_Objest.Read(unigueID))
            {
                ПсуванняТоварів_Objest ПсуванняТоварів_Objest_Новий = await ПсуванняТоварів_Objest.Copy(true);
                await ПсуванняТоварів_Objest_Новий.Save();
                await ПсуванняТоварів_Objest_Новий.Товари_TablePart.Save(true);

                return ПсуванняТоварів_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "Документи.ПсуванняТоварів";

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
            ПсуванняТоварів_Pointer ПсуванняТоварів_Pointer = new ПсуванняТоварів_Pointer(unigueID);
            ПсуванняТоварів_Objest? ПсуванняТоварів_Objest = await ПсуванняТоварів_Pointer.GetDocumentObject(true);
            if (ПсуванняТоварів_Objest == null) return;

            if (spendDoc)
            {
                if (!await ПсуванняТоварів_Objest.SpendTheDocument(ПсуванняТоварів_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(ПсуванняТоварів_Objest.UnigueID);
            }
            else
                await ПсуванняТоварів_Objest.ClearSpendTheDocument();
        }

        protected override void ReportSpendTheDocument(UnigueID unigueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ПсуванняТоварів_Pointer(unigueID));
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{ПсуванняТоварів_Const.FULLNAME}_{unigueID}.xml");
            await ПсуванняТоварів_Export.ToXmlFile(new ПсуванняТоварів_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar


        #endregion
    }
}