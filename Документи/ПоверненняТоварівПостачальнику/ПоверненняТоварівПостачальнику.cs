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

using Gtk;

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
            TreeViewGrid.Model = ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.Store;
            ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.LoadRecords();

            if (ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.CurrentPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.Where.Clear();

            //Назва
            ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.Where.Add(
                new Where(ПоверненняТоварівПостачальнику_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.LoadRecords();

            if (ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.FirstPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{ПоверненняТоварівПостачальнику_Const.FULLNAME} *", () =>
                {
                    ПоверненняТоварівПостачальнику_Елемент page = new ПоверненняТоварівПостачальнику_Елемент
                    {
                        PageList = this,
                        IsNew = true
                    };

                    page.SetValue();

                    return page;
                }, true);
            }
            else if (unigueID != null)
            {
                ПоверненняТоварівПостачальнику_Objest ПоверненняТоварівПостачальнику_Objest = new ПоверненняТоварівПостачальнику_Objest();
                if (ПоверненняТоварівПостачальнику_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ПоверненняТоварівПостачальнику_Objest.Назва}", () =>
                    {
                        ПоверненняТоварівПостачальнику_Елемент page = new ПоверненняТоварівПостачальнику_Елемент
                        {
                            PageList = this,
                            IsNew = false,
                            ПоверненняТоварівПостачальнику_Objest = ПоверненняТоварівПостачальнику_Objest,
                        };

                        page.SetValue();

                        return page;
                    }, true);
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }

        protected override void SetDeletionLabel(UnigueID unigueID)
        {
            ПоверненняТоварівПостачальнику_Objest ПоверненняТоварівПостачальнику_Objest = new ПоверненняТоварівПостачальнику_Objest();
            if (ПоверненняТоварівПостачальнику_Objest.Read(unigueID))
                ПоверненняТоварівПостачальнику_Objest.SetDeletionLabel(!ПоверненняТоварівПостачальнику_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override UnigueID? Copy(UnigueID unigueID)
        {
            ПоверненняТоварівПостачальнику_Objest ПоверненняТоварівПостачальнику_Objest = new ПоверненняТоварівПостачальнику_Objest();
            if (ПоверненняТоварівПостачальнику_Objest.Read(unigueID))
            {
                ПоверненняТоварівПостачальнику_Objest ПоверненняТоварівПостачальнику_Objest_Новий = ПоверненняТоварівПостачальнику_Objest.Copy(true);
                ПоверненняТоварівПостачальнику_Objest_Новий.Save();
                ПоверненняТоварівПостачальнику_Objest_Новий.Товари_TablePart.Save(true);

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
            ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.ДодатиВідбірПоПеріоду(Enum.Parse<ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }

        protected override void SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ПоверненняТоварівПостачальнику_Pointer ПоверненняТоварівПостачальнику_Pointer = new ПоверненняТоварівПостачальнику_Pointer(unigueID);
            ПоверненняТоварівПостачальнику_Objest? ПоверненняТоварівПостачальнику_Objest = ПоверненняТоварівПостачальнику_Pointer.GetDocumentObject(true);
            if (ПоверненняТоварівПостачальнику_Objest == null) return;

            if (spendDoc)
            {
                if (!ПоверненняТоварівПостачальнику_Objest.SpendTheDocument(ПоверненняТоварівПостачальнику_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ВідкритиТермінал();
            }
            else
                ПоверненняТоварівПостачальнику_Objest.ClearSpendTheDocument();
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new ПоверненняТоварівПостачальнику_Pointer(unigueID);
        }

        protected override void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{ПоверненняТоварівПостачальнику_Const.FULLNAME}_{unigueID}.xml");
            ПоверненняТоварівПостачальнику_Export.ToXmlFile(new ПоверненняТоварівПостачальнику_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar


        #endregion
    }
}