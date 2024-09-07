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
    public class КорегуванняБоргу : ДокументЖурнал
    {
        public КорегуванняБоргу() : base()
        {
            ТабличніСписки.КорегуванняБоргу_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async void LoadRecords()
        {
            ТабличніСписки.КорегуванняБоргу_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.КорегуванняБоргу_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.КорегуванняБоргу_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.КорегуванняБоргу_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            //Назва
            ТабличніСписки.КорегуванняБоргу_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(КорегуванняБоргу_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.КорегуванняБоргу_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.КорегуванняБоргу_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask<(string Name, Func<Widget>? FuncWidget, System.Action? SetValue)> OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            КорегуванняБоргу_Елемент page = new КорегуванняБоргу_Елемент
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
            КорегуванняБоргу_Objest КорегуванняБоргу_Objest = new КорегуванняБоргу_Objest();
            if (await КорегуванняБоргу_Objest.Read(unigueID))
                await КорегуванняБоргу_Objest.SetDeletionLabel(!КорегуванняБоргу_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            КорегуванняБоргу_Objest КорегуванняБоргу_Objest = new КорегуванняБоргу_Objest();
            if (await КорегуванняБоргу_Objest.Read(unigueID))
            {
                КорегуванняБоргу_Objest КорегуванняБоргу_Objest_Новий = await КорегуванняБоргу_Objest.Copy(true);
                await КорегуванняБоргу_Objest_Новий.Save();
                await КорегуванняБоргу_Objest_Новий.РозрахункиЗКонтрагентами_TablePart.Save(true);

                return КорегуванняБоргу_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "Документи.КорегуванняБоргу";

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
            КорегуванняБоргу_Pointer КорегуванняБоргу_Pointer = new КорегуванняБоргу_Pointer(unigueID);
            КорегуванняБоргу_Objest? КорегуванняБоргу_Objest = await КорегуванняБоргу_Pointer.GetDocumentObject(true);
            if (КорегуванняБоргу_Objest == null) return;

            if (spendDoc)
            {
                if (!await КорегуванняБоргу_Objest.SpendTheDocument(КорегуванняБоргу_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(КорегуванняБоргу_Objest.UnigueID);
            }
            else
                await КорегуванняБоргу_Objest.ClearSpendTheDocument();
        }

        protected override void ReportSpendTheDocument(UnigueID unigueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new КорегуванняБоргу_Pointer(unigueID));
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{КорегуванняБоргу_Const.FULLNAME}_{unigueID}.xml");
            await КорегуванняБоргу_Export.ToXmlFile(new КорегуванняБоргу_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar

        #endregion
    }
}