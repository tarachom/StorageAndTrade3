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
    public class РозміщенняТоварівНаСкладі : ДокументЖурнал
    {
        public РозміщенняТоварівНаСкладі() 
        {
            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            //Назва
            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(РозміщенняТоварівНаСкладі_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            РозміщенняТоварівНаСкладі_Елемент page = new РозміщенняТоварівНаСкладі_Елемент
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
            РозміщенняТоварівНаСкладі_Objest РозміщенняТоварівНаСкладі_Objest = new РозміщенняТоварівНаСкладі_Objest();
            if (await РозміщенняТоварівНаСкладі_Objest.Read(unigueID))
                await РозміщенняТоварівНаСкладі_Objest.SetDeletionLabel(!РозміщенняТоварівНаСкладі_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            РозміщенняТоварівНаСкладі_Objest РозміщенняТоварівНаСкладі_Objest = new РозміщенняТоварівНаСкладі_Objest();
            if (await РозміщенняТоварівНаСкладі_Objest.Read(unigueID))
            {
                РозміщенняТоварівНаСкладі_Objest РозміщенняТоварівНаСкладі_Objest_Новий = await РозміщенняТоварівНаСкладі_Objest.Copy(true);
                await РозміщенняТоварівНаСкладі_Objest_Новий.Save();
                await РозміщенняТоварівНаСкладі_Objest_Новий.Товари_TablePart.Save(true);

                return РозміщенняТоварівНаСкладі_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "Документи.РозміщенняТоварівНаСкладі";

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
            РозміщенняТоварівНаСкладі_Pointer РозміщенняТоварівНаСкладі_Pointer = new РозміщенняТоварівНаСкладі_Pointer(unigueID);
            РозміщенняТоварівНаСкладі_Objest? РозміщенняТоварівНаСкладі_Objest = await РозміщенняТоварівНаСкладі_Pointer.GetDocumentObject(true);
            if (РозміщенняТоварівНаСкладі_Objest == null) return;

            if (spendDoc)
            {
                if (!await РозміщенняТоварівНаСкладі_Objest.SpendTheDocument(РозміщенняТоварівНаСкладі_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(РозміщенняТоварівНаСкладі_Objest.UnigueID);
            }
            else
                await РозміщенняТоварівНаСкладі_Objest.ClearSpendTheDocument();
        }

        protected override void ReportSpendTheDocument(UnigueID unigueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new РозміщенняТоварівНаСкладі_Pointer(unigueID));
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{РозміщенняТоварівНаСкладі_Const.FULLNAME}_{unigueID}.xml");
            await РозміщенняТоварівНаСкладі_Export.ToXmlFile(new РозміщенняТоварівНаСкладі_Pointer(unigueID), pathToSave);
        }

        #endregion
    }
}