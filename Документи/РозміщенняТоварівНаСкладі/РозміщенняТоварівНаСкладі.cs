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
    public class РозміщенняТоварівНаСкладі : ДокументЖурнал
    {
        public РозміщенняТоварівНаСкладі() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.Store;
            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.LoadRecords();

            if (ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.CurrentPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.Where.Clear();

            //Назва
            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.Where.Add(
                new Where(РозміщенняТоварівНаСкладі_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.LoadRecords();

            if (ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.FirstPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{РозміщенняТоварівНаСкладі_Const.FULLNAME} *", () =>
                {
                    РозміщенняТоварівНаСкладі_Елемент page = new РозміщенняТоварівНаСкладі_Елемент
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
                РозміщенняТоварівНаСкладі_Objest РозміщенняТоварівНаСкладі_Objest = new РозміщенняТоварівНаСкладі_Objest();
                if (РозміщенняТоварівНаСкладі_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{РозміщенняТоварівНаСкладі_Objest.Назва}", () =>
                    {
                        РозміщенняТоварівНаСкладі_Елемент page = new РозміщенняТоварівНаСкладі_Елемент
                        {
                            PageList = this,
                            IsNew = false,
                            РозміщенняТоварівНаСкладі_Objest = РозміщенняТоварівНаСкладі_Objest,
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
            РозміщенняТоварівНаСкладі_Objest РозміщенняТоварівНаСкладі_Objest = new РозміщенняТоварівНаСкладі_Objest();
            if (РозміщенняТоварівНаСкладі_Objest.Read(unigueID))
                РозміщенняТоварівНаСкладі_Objest.SetDeletionLabel(!РозміщенняТоварівНаСкладі_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override UnigueID? Copy(UnigueID unigueID)
        {
            РозміщенняТоварівНаСкладі_Objest РозміщенняТоварівНаСкладі_Objest = new РозміщенняТоварівНаСкладі_Objest();
            if (РозміщенняТоварівНаСкладі_Objest.Read(unigueID))
            {
                РозміщенняТоварівНаСкладі_Objest РозміщенняТоварівНаСкладі_Objest_Новий = РозміщенняТоварівНаСкладі_Objest.Copy(true);
                РозміщенняТоварівНаСкладі_Objest_Новий.Save();
                РозміщенняТоварівНаСкладі_Objest_Новий.Товари_TablePart.Save(true);

                return РозміщенняТоварівНаСкладі_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        protected override void PeriodWhereChanged()
        {
            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.ДодатиВідбірПоПеріоду(Enum.Parse<ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }

        protected override void SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            РозміщенняТоварівНаСкладі_Pointer РозміщенняТоварівНаСкладі_Pointer = new РозміщенняТоварівНаСкладі_Pointer(unigueID);
            РозміщенняТоварівНаСкладі_Objest? РозміщенняТоварівНаСкладі_Objest = РозміщенняТоварівНаСкладі_Pointer.GetDocumentObject(true);
            if (РозміщенняТоварівНаСкладі_Objest == null) return;

            if (spendDoc)
            {
                if (!РозміщенняТоварівНаСкладі_Objest.SpendTheDocument(РозміщенняТоварівНаСкладі_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ВідкритиТермінал();
            }
            else
                РозміщенняТоварівНаСкладі_Objest.ClearSpendTheDocument();
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new РозміщенняТоварівНаСкладі_Pointer(unigueID);
        }

        protected override void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{РозміщенняТоварівНаСкладі_Const.FULLNAME}_{unigueID}.xml");
            РозміщенняТоварівНаСкладі_Export.ToXmlFile(new РозміщенняТоварівНаСкладі_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar

        #endregion
    }
}