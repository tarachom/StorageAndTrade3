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
    public class ПереміщенняТоварівНаСкладі : ДокументЖурнал
    {
        public ПереміщенняТоварівНаСкладі() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.Store;
            ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.LoadRecords();

            if (ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.CurrentPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.Where.Clear();

            //Назва
            ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.Where.Add(
                new Where(ПереміщенняТоварівНаСкладі_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.LoadRecords();

            if (ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.FirstPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{ПереміщенняТоварівНаСкладі_Const.FULLNAME} *", () =>
                {
                    ПереміщенняТоварівНаСкладі_Елемент page = new ПереміщенняТоварівНаСкладі_Елемент
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
                ПереміщенняТоварівНаСкладі_Objest ПереміщенняТоварівНаСкладі_Objest = new ПереміщенняТоварівНаСкладі_Objest();
                if (ПереміщенняТоварівНаСкладі_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ПереміщенняТоварівНаСкладі_Objest.Назва}", () =>
                    {
                        ПереміщенняТоварівНаСкладі_Елемент page = new ПереміщенняТоварівНаСкладі_Елемент
                        {
                            PageList = this,
                            IsNew = false,
                            ПереміщенняТоварівНаСкладі_Objest = ПереміщенняТоварівНаСкладі_Objest,
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
            ПереміщенняТоварівНаСкладі_Objest ПереміщенняТоварівНаСкладі_Objest = new ПереміщенняТоварівНаСкладі_Objest();
            if (ПереміщенняТоварівНаСкладі_Objest.Read(unigueID))
                ПереміщенняТоварівНаСкладі_Objest.SetDeletionLabel(!ПереміщенняТоварівНаСкладі_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override UnigueID? Copy(UnigueID unigueID)
        {
            ПереміщенняТоварівНаСкладі_Objest ПереміщенняТоварівНаСкладі_Objest = new ПереміщенняТоварівНаСкладі_Objest();
            if (ПереміщенняТоварівНаСкладі_Objest.Read(unigueID))
            {
                ПереміщенняТоварівНаСкладі_Objest ПереміщенняТоварівНаСкладі_Objest_Новий = ПереміщенняТоварівНаСкладі_Objest.Copy(true);
                ПереміщенняТоварівНаСкладі_Objest_Новий.Save();
                ПереміщенняТоварівНаСкладі_Objest_Новий.Товари_TablePart.Save(true);

                return ПереміщенняТоварівНаСкладі_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        protected override void PeriodWhereChanged()
        {
            ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.ДодатиВідбірПоПеріоду(Enum.Parse<ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }

        protected override void SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ПереміщенняТоварівНаСкладі_Pointer ПереміщенняТоварівНаСкладі_Pointer = new ПереміщенняТоварівНаСкладі_Pointer(unigueID);
            ПереміщенняТоварівНаСкладі_Objest? ПереміщенняТоварівНаСкладі_Objest = ПереміщенняТоварівНаСкладі_Pointer.GetDocumentObject(true);
            if (ПереміщенняТоварівНаСкладі_Objest == null) return;

            if (spendDoc)
            {
                if (!ПереміщенняТоварівНаСкладі_Objest.SpendTheDocument(ПереміщенняТоварівНаСкладі_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ВідкритиТермінал();
            }
            else
                ПереміщенняТоварівНаСкладі_Objest.ClearSpendTheDocument();
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new ПереміщенняТоварівНаСкладі_Pointer(unigueID);
        }

        protected override void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{ПереміщенняТоварівНаСкладі_Const.FULLNAME}_{unigueID}.xml");
            ПереміщенняТоварівНаСкладі_Export.ToXmlFile(new ПереміщенняТоварівНаСкладі_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar


        #endregion
    }
}