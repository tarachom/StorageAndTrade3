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
    public class ПоверненняТоварівВідКлієнта : ДокументЖурнал
    {
        public ПоверненняТоварівВідКлієнта() 
        {
            ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            //Назва
            ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(ПоверненняТоварівВідКлієнта_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            ПоверненняТоварівВідКлієнта_Елемент page = new ПоверненняТоварівВідКлієнта_Елемент
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
            ПоверненняТоварівВідКлієнта_Objest ПоверненняТоварівВідКлієнта_Objest = new ПоверненняТоварівВідКлієнта_Objest();
            if (await ПоверненняТоварівВідКлієнта_Objest.Read(unigueID))
                await ПоверненняТоварівВідКлієнта_Objest.SetDeletionLabel(!ПоверненняТоварівВідКлієнта_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ПоверненняТоварівВідКлієнта_Objest ПоверненняТоварівВідКлієнта_Objest = new ПоверненняТоварівВідКлієнта_Objest();
            if (await ПоверненняТоварівВідКлієнта_Objest.Read(unigueID))
            {
                ПоверненняТоварівВідКлієнта_Objest ПоверненняТоварівВідКлієнта_Objest_Новий = await ПоверненняТоварівВідКлієнта_Objest.Copy(true);
                await ПоверненняТоварівВідКлієнта_Objest_Новий.Save();
                await ПоверненняТоварівВідКлієнта_Objest_Новий.Товари_TablePart.Save(true);

                return ПоверненняТоварівВідКлієнта_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "Документи.ПоверненняТоварівВідКлієнта";

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
            ПоверненняТоварівВідКлієнта_Pointer ПоверненняТоварівВідКлієнта_Pointer = new ПоверненняТоварівВідКлієнта_Pointer(unigueID);
            ПоверненняТоварівВідКлієнта_Objest? ПоверненняТоварівВідКлієнта_Objest = await ПоверненняТоварівВідКлієнта_Pointer.GetDocumentObject(true);
            if (ПоверненняТоварівВідКлієнта_Objest == null) return;

            if (spendDoc)
            {
                if (!await ПоверненняТоварівВідКлієнта_Objest.SpendTheDocument(ПоверненняТоварівВідКлієнта_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(ПоверненняТоварівВідКлієнта_Objest.UnigueID);
            }
            else
                await ПоверненняТоварівВідКлієнта_Objest.ClearSpendTheDocument();
        }

        protected override void ReportSpendTheDocument(UnigueID unigueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ПоверненняТоварівВідКлієнта_Pointer(unigueID));
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{ПоверненняТоварівВідКлієнта_Const.FULLNAME}_{unigueID}.xml");
            await ПоверненняТоварівВідКлієнта_Export.ToXmlFile(new ПоверненняТоварівВідКлієнта_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar


        #endregion
    }
}