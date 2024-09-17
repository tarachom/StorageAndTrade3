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
    public class ВнутрішнєСпоживанняТоварів : ДокументЖурнал
    {
        public ВнутрішнєСпоживанняТоварів() 
        {
            ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            //Назва
            ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(ВнутрішнєСпоживанняТоварів_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            ВнутрішнєСпоживанняТоварів_Елемент page = new ВнутрішнєСпоживанняТоварів_Елемент
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
            ВнутрішнєСпоживанняТоварів_Objest ВнутрішнєСпоживанняТоварів_Objest = new ВнутрішнєСпоживанняТоварів_Objest();
            if (await ВнутрішнєСпоживанняТоварів_Objest.Read(unigueID))
                await ВнутрішнєСпоживанняТоварів_Objest.SetDeletionLabel(!ВнутрішнєСпоживанняТоварів_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ВнутрішнєСпоживанняТоварів_Objest ВнутрішнєСпоживанняТоварів_Objest = new ВнутрішнєСпоживанняТоварів_Objest();
            if (await ВнутрішнєСпоживанняТоварів_Objest.Read(unigueID))
            {
                ВнутрішнєСпоживанняТоварів_Objest ВнутрішнєСпоживанняТоварів_Objest_Новий = await ВнутрішнєСпоживанняТоварів_Objest.Copy(true);
                await ВнутрішнєСпоживанняТоварів_Objest_Новий.Save();
                await ВнутрішнєСпоживанняТоварів_Objest_Новий.Товари_TablePart.Save(true);

                return ВнутрішнєСпоживанняТоварів_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "Документи.ВнутрішнєСпоживанняТоварів";

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
            ВнутрішнєСпоживанняТоварів_Pointer ВнутрішнєСпоживанняТоварів_Pointer = new ВнутрішнєСпоживанняТоварів_Pointer(unigueID);
            ВнутрішнєСпоживанняТоварів_Objest? ВнутрішнєСпоживанняТоварів_Objest = await ВнутрішнєСпоживанняТоварів_Pointer.GetDocumentObject(true);
            if (ВнутрішнєСпоживанняТоварів_Objest == null) return;

            if (spendDoc)
            {
                if (!await ВнутрішнєСпоживанняТоварів_Objest.SpendTheDocument(ВнутрішнєСпоживанняТоварів_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(ВнутрішнєСпоживанняТоварів_Objest.UnigueID);
            }
            else
                await ВнутрішнєСпоживанняТоварів_Objest.ClearSpendTheDocument();
        }

        protected override void ReportSpendTheDocument(UnigueID unigueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ВнутрішнєСпоживанняТоварів_Pointer(unigueID));
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{ВнутрішнєСпоживанняТоварів_Const.FULLNAME}_{unigueID}.xml");
            await ВнутрішнєСпоживанняТоварів_Export.ToXmlFile(new ВнутрішнєСпоживанняТоварів_Pointer(unigueID), pathToSave);
        }

        #endregion
    }
}