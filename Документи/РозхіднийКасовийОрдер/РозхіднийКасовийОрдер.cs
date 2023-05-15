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
    public class РозхіднийКасовийОрдер : ДокументЖурнал
    {
        public РозхіднийКасовийОрдер() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.РозхіднийКасовийОрдер_Записи.Store;
            ТабличніСписки.РозхіднийКасовийОрдер_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.РозхіднийКасовийОрдер_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.РозхіднийКасовийОрдер_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.РозхіднийКасовийОрдер_Записи.LoadRecords();

            if (ТабличніСписки.РозхіднийКасовийОрдер_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РозхіднийКасовийОрдер_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.РозхіднийКасовийОрдер_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РозхіднийКасовийОрдер_Записи.CurrentPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.РозхіднийКасовийОрдер_Записи.Where.Clear();

            //Назва
            ТабличніСписки.РозхіднийКасовийОрдер_Записи.Where.Add(
                new Where(РозхіднийКасовийОрдер_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.РозхіднийКасовийОрдер_Записи.LoadRecords();

            if (ТабличніСписки.РозхіднийКасовийОрдер_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РозхіднийКасовийОрдер_Записи.FirstPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{РозхіднийКасовийОрдер_Const.FULLNAME} *", () =>
                {
                    РозхіднийКасовийОрдер_Елемент page = new РозхіднийКасовийОрдер_Елемент
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
                РозхіднийКасовийОрдер_Objest РозхіднийКасовийОрдер_Objest = new РозхіднийКасовийОрдер_Objest();
                if (РозхіднийКасовийОрдер_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{РозхіднийКасовийОрдер_Objest.Назва}", () =>
                    {
                        РозхіднийКасовийОрдер_Елемент page = new РозхіднийКасовийОрдер_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            РозхіднийКасовийОрдер_Objest = РозхіднийКасовийОрдер_Objest,
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
            РозхіднийКасовийОрдер_Objest РозхіднийКасовийОрдер_Objest = new РозхіднийКасовийОрдер_Objest();
            if (РозхіднийКасовийОрдер_Objest.Read(unigueID))
                РозхіднийКасовийОрдер_Objest.SetDeletionLabel(!РозхіднийКасовийОрдер_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override UnigueID? Copy(UnigueID unigueID)
        {
            РозхіднийКасовийОрдер_Objest РозхіднийКасовийОрдер_Objest = new РозхіднийКасовийОрдер_Objest();
            if (РозхіднийКасовийОрдер_Objest.Read(unigueID))
            {
                РозхіднийКасовийОрдер_Objest РозхіднийКасовийОрдер_Objest_Новий = РозхіднийКасовийОрдер_Objest.Copy(true);
                РозхіднийКасовийОрдер_Objest_Новий.Save();

                return РозхіднийКасовийОрдер_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        protected override void PeriodWhereChanged()
        {
            ТабличніСписки.РозхіднийКасовийОрдер_Записи.ДодатиВідбірПоПеріоду(Enum.Parse<ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }

        protected override void SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            РозхіднийКасовийОрдер_Pointer РозхіднийКасовийОрдер_Pointer = new РозхіднийКасовийОрдер_Pointer(unigueID);
            РозхіднийКасовийОрдер_Objest? РозхіднийКасовийОрдер_Objest = РозхіднийКасовийОрдер_Pointer.GetDocumentObject(true);
            if (РозхіднийКасовийОрдер_Objest == null) return;

            if (spendDoc)
            {
                if (!РозхіднийКасовийОрдер_Objest.SpendTheDocument(РозхіднийКасовийОрдер_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ВідкритиТермінал();
            }
            else
                РозхіднийКасовийОрдер_Objest.ClearSpendTheDocument();
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new РозхіднийКасовийОрдер_Pointer(unigueID);
        }

        protected override void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{РозхіднийКасовийОрдер_Const.FULLNAME}_{unigueID}.xml");
            РозхіднийКасовийОрдер_Export.ToXmlFile(new РозхіднийКасовийОрдер_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar

        #endregion
    }
}