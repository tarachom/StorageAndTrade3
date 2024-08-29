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
    public class РозміщенняНоменклатуриПоКоміркам : ДокументЖурнал
    {
        public РозміщенняНоменклатуриПоКоміркам() : base()
        {
            ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async void LoadRecords()
        {
            ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Назва
            ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(РозміщенняНоменклатуриПоКоміркам_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask<(string Name, Func<Widget>? FuncWidget, System.Action? SetValue)> OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            РозміщенняНоменклатуриПоКоміркам_Елемент page = new РозміщенняНоменклатуриПоКоміркам_Елемент
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
            РозміщенняНоменклатуриПоКоміркам_Objest РозміщенняНоменклатуриПоКоміркам_Objest = new РозміщенняНоменклатуриПоКоміркам_Objest();
            if (await РозміщенняНоменклатуриПоКоміркам_Objest.Read(unigueID))
                await РозміщенняНоменклатуриПоКоміркам_Objest.SetDeletionLabel(!РозміщенняНоменклатуриПоКоміркам_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            РозміщенняНоменклатуриПоКоміркам_Objest РозміщенняНоменклатуриПоКоміркам_Objest = new РозміщенняНоменклатуриПоКоміркам_Objest();
            if (await РозміщенняНоменклатуриПоКоміркам_Objest.Read(unigueID))
            {
                РозміщенняНоменклатуриПоКоміркам_Objest РозміщенняНоменклатуриПоКоміркам_Objest_Новий = await РозміщенняНоменклатуриПоКоміркам_Objest.Copy(true);
                await РозміщенняНоменклатуриПоКоміркам_Objest_Новий.Save();
                await РозміщенняНоменклатуриПоКоміркам_Objest_Новий.Товари_TablePart.Save(true);

                return РозміщенняНоменклатуриПоКоміркам_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "Документи.РозміщенняНоменклатуриПоКоміркам";

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
            РозміщенняНоменклатуриПоКоміркам_Pointer РозміщенняНоменклатуриПоКоміркам_Pointer = new РозміщенняНоменклатуриПоКоміркам_Pointer(unigueID);
            РозміщенняНоменклатуриПоКоміркам_Objest? РозміщенняНоменклатуриПоКоміркам_Objest = await РозміщенняНоменклатуриПоКоміркам_Pointer.GetDocumentObject(true);
            if (РозміщенняНоменклатуриПоКоміркам_Objest == null) return;

            if (spendDoc)
            {
                if (!await РозміщенняНоменклатуриПоКоміркам_Objest.SpendTheDocument(РозміщенняНоменклатуриПоКоміркам_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(РозміщенняНоменклатуриПоКоміркам_Objest.UnigueID);
            }
            else
                await РозміщенняНоменклатуриПоКоміркам_Objest.ClearSpendTheDocument();
        }

        protected override void ReportSpendTheDocument(UnigueID unigueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new РозміщенняНоменклатуриПоКоміркам_Pointer(unigueID));
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{РозміщенняНоменклатуриПоКоміркам_Const.FULLNAME}_{unigueID}.xml");
            await РозміщенняНоменклатуриПоКоміркам_Export.ToXmlFile(new РозміщенняНоменклатуриПоКоміркам_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar

        #endregion
    }
}