/*
Copyright (C) 2019-2023 TARAKHOMYN YURIY IVANOVYCH
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

using AccountingSoftware;

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    public class ПоверненняТоварівПостачальнику : ДокументЖурнал
    {
        public ПоверненняТоварівПостачальнику() : base()
        {
            ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async void LoadRecords()
        {
            ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.DocumentPointerItem = DocumentPointerItem;

            await ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Назва
            ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(ПоверненняТоварівПостачальнику_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{ПоверненняТоварівПостачальнику_Const.FULLNAME} *", () =>
                {
                    ПоверненняТоварівПостачальнику_Елемент page = new ПоверненняТоварівПостачальнику_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadRecords,
                        IsNew = true
                    };

                    page.SetValue();

                    return page;
                });
            }
            else if (unigueID != null)
            {
                ПоверненняТоварівПостачальнику_Objest ПоверненняТоварівПостачальнику_Objest = new ПоверненняТоварівПостачальнику_Objest();
                if (await ПоверненняТоварівПостачальнику_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ПоверненняТоварівПостачальнику_Objest.Назва}", () =>
                    {
                        ПоверненняТоварівПостачальнику_Елемент page = new ПоверненняТоварівПостачальнику_Елемент
                        {
                            UnigueID = unigueID,
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            ПоверненняТоварівПостачальнику_Objest = ПоверненняТоварівПостачальнику_Objest,
                        };

                        page.SetValue();

                        return page;
                    });
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            ПоверненняТоварівПостачальнику_Objest ПоверненняТоварівПостачальнику_Objest = new ПоверненняТоварівПостачальнику_Objest();
            if (await ПоверненняТоварівПостачальнику_Objest.Read(unigueID))
                await ПоверненняТоварівПостачальнику_Objest.SetDeletionLabel(!ПоверненняТоварівПостачальнику_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ПоверненняТоварівПостачальнику_Objest ПоверненняТоварівПостачальнику_Objest = new ПоверненняТоварівПостачальнику_Objest();
            if (await ПоверненняТоварівПостачальнику_Objest.Read(unigueID))
            {
                ПоверненняТоварівПостачальнику_Objest ПоверненняТоварівПостачальнику_Objest_Новий = await ПоверненняТоварівПостачальнику_Objest.Copy(true);
                await ПоверненняТоварівПостачальнику_Objest_Новий.Save();
                await ПоверненняТоварівПостачальнику_Objest_Новий.Товари_TablePart.Save(true);

                return ПоверненняТоварівПостачальнику_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        protected override void PeriodWhereChanged()
        {
            ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Enum.Parse<ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ПоверненняТоварівПостачальнику_Pointer ПоверненняТоварівПостачальнику_Pointer = new ПоверненняТоварівПостачальнику_Pointer(unigueID);
            ПоверненняТоварівПостачальнику_Objest? ПоверненняТоварівПостачальнику_Objest = await ПоверненняТоварівПостачальнику_Pointer.GetDocumentObject(true);
            if (ПоверненняТоварівПостачальнику_Objest == null) return;

            if (spendDoc)
            {
                if (!await ПоверненняТоварівПостачальнику_Objest.SpendTheDocument(ПоверненняТоварівПостачальнику_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(ПоверненняТоварівПостачальнику_Objest.UnigueID);
            }
            else
                await ПоверненняТоварівПостачальнику_Objest.ClearSpendTheDocument();
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new ПоверненняТоварівПостачальнику_Pointer(unigueID);
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{ПоверненняТоварівПостачальнику_Const.FULLNAME}_{unigueID}.xml");
            await ПоверненняТоварівПостачальнику_Export.ToXmlFile(new ПоверненняТоварівПостачальнику_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar


        #endregion
    }
}