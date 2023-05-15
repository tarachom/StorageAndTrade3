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
    public class ВведенняЗалишків : ДокументЖурнал
    {
        public ВведенняЗалишків() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.ВведенняЗалишків_Записи.Store;
            ТабличніСписки.ВведенняЗалишків_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.ВведенняЗалишків_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ВведенняЗалишків_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ВведенняЗалишків_Записи.LoadRecords();

            if (ТабличніСписки.ВведенняЗалишків_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВведенняЗалишків_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ВведенняЗалишків_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВведенняЗалишків_Записи.CurrentPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ВведенняЗалишків_Записи.Where.Clear();

            //Назва
            ТабличніСписки.ВведенняЗалишків_Записи.Where.Add(
                new Where(ВведенняЗалишків_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.ВведенняЗалишків_Записи.LoadRecords();

            if (ТабличніСписки.ВведенняЗалишків_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВведенняЗалишків_Записи.FirstPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{ВведенняЗалишків_Const.FULLNAME} *", () =>
                {
                    ВведенняЗалишків_Елемент page = new ВведенняЗалишків_Елемент
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
                ВведенняЗалишків_Objest ВведенняЗалишків_Objest = new ВведенняЗалишків_Objest();
                if (ВведенняЗалишків_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ВведенняЗалишків_Objest.Назва}", () =>
                    {
                        ВведенняЗалишків_Елемент page = new ВведенняЗалишків_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            ВведенняЗалишків_Objest = ВведенняЗалишків_Objest,
                        };

                        page.SetValue();

                        return page;
                    });
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }

        protected override void SetDeletionLabel(UnigueID unigueID)
        {
            ВведенняЗалишків_Objest ВведенняЗалишків_Objest = new ВведенняЗалишків_Objest();
            if (ВведенняЗалишків_Objest.Read(unigueID))
                ВведенняЗалишків_Objest.SetDeletionLabel(!ВведенняЗалишків_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override UnigueID? Copy(UnigueID unigueID)
        {
            ВведенняЗалишків_Objest ВведенняЗалишків_Objest = new ВведенняЗалишків_Objest();
            if (ВведенняЗалишків_Objest.Read(unigueID))
            {
                ВведенняЗалишків_Objest ВведенняЗалишків_Objest_Новий = ВведенняЗалишків_Objest.Copy(true);
                ВведенняЗалишків_Objest_Новий.Save();
                ВведенняЗалишків_Objest_Новий.Товари_TablePart.Save(true);
                ВведенняЗалишків_Objest_Новий.Каси_TablePart.Save(true);
                ВведенняЗалишків_Objest_Новий.БанківськіРахунки_TablePart.Save(true);
                ВведенняЗалишків_Objest_Новий.РозрахункиЗКонтрагентами_TablePart.Save(true);

                return ВведенняЗалишків_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        protected override void PeriodWhereChanged()
        {
            ТабличніСписки.ВведенняЗалишків_Записи.ДодатиВідбірПоПеріоду(Enum.Parse<ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }

        protected override void SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ВведенняЗалишків_Pointer ВведенняЗалишків_Pointer = new ВведенняЗалишків_Pointer(unigueID);
            ВведенняЗалишків_Objest? ВведенняЗалишків_Objest = ВведенняЗалишків_Pointer.GetDocumentObject(true);
            if (ВведенняЗалишків_Objest == null) return;

            if (spendDoc)
            {
                if (!ВведенняЗалишків_Objest.SpendTheDocument(ВведенняЗалишків_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ВідкритиТермінал();
            }
            else
                ВведенняЗалишків_Objest.ClearSpendTheDocument();
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new ВведенняЗалишків_Pointer(unigueID);
        }

        protected override void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{ВведенняЗалишків_Const.FULLNAME}_{unigueID}.xml");
            ВведенняЗалишків_Export.ToXmlFile(new ВведенняЗалишків_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar

        #endregion
    }
}