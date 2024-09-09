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
    public class ВстановленняЦінНоменклатури : ДокументЖурнал
    {
        public ВстановленняЦінНоменклатури() 
        {
            ТабличніСписки.ВстановленняЦінНоменклатури_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ВстановленняЦінНоменклатури_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ВстановленняЦінНоменклатури_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ВстановленняЦінНоменклатури_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ВстановленняЦінНоменклатури_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            //Назва
            ТабличніСписки.ВстановленняЦінНоменклатури_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(ВстановленняЦінНоменклатури_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.ВстановленняЦінНоменклатури_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.ВстановленняЦінНоменклатури_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            ВстановленняЦінНоменклатури_Елемент page = new ВстановленняЦінНоменклатури_Елемент
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
            ВстановленняЦінНоменклатури_Objest ВстановленняЦінНоменклатури_Objest = new ВстановленняЦінНоменклатури_Objest();
            if (await ВстановленняЦінНоменклатури_Objest.Read(unigueID))
                await ВстановленняЦінНоменклатури_Objest.SetDeletionLabel(!ВстановленняЦінНоменклатури_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ВстановленняЦінНоменклатури_Objest ВстановленняЦінНоменклатури_Objest = new ВстановленняЦінНоменклатури_Objest();
            if (await ВстановленняЦінНоменклатури_Objest.Read(unigueID))
            {
                ВстановленняЦінНоменклатури_Objest ВстановленняЦінНоменклатури_Objest_Новий = await ВстановленняЦінНоменклатури_Objest.Copy(true);
                await ВстановленняЦінНоменклатури_Objest_Новий.Save();
                await ВстановленняЦінНоменклатури_Objest_Новий.Товари_TablePart.Save(true);

                return ВстановленняЦінНоменклатури_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "Документи.ВстановленняЦінНоменклатури";

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
            ВстановленняЦінНоменклатури_Pointer ВстановленняЦінНоменклатури_Pointer = new ВстановленняЦінНоменклатури_Pointer(unigueID);
            ВстановленняЦінНоменклатури_Objest? ВстановленняЦінНоменклатури_Objest = await ВстановленняЦінНоменклатури_Pointer.GetDocumentObject(true);
            if (ВстановленняЦінНоменклатури_Objest == null) return;

            if (spendDoc)
            {
                if (!await ВстановленняЦінНоменклатури_Objest.SpendTheDocument(ВстановленняЦінНоменклатури_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(ВстановленняЦінНоменклатури_Objest.UnigueID);
            }
            else
                await ВстановленняЦінНоменклатури_Objest.ClearSpendTheDocument();
        }

        protected override void ReportSpendTheDocument(UnigueID unigueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ВстановленняЦінНоменклатури_Pointer(unigueID));
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{ВстановленняЦінНоменклатури_Const.FULLNAME}_{unigueID}.xml");
            await ВстановленняЦінНоменклатури_Export.ToXmlFile(new ВстановленняЦінНоменклатури_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar

        #endregion
    }
}