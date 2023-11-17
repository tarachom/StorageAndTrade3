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
    public class ПсуванняТоварів : ДокументЖурнал
    {
        public ПсуванняТоварів() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.ПсуванняТоварів_Записи.Store;
            ТабличніСписки.ПсуванняТоварів_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.ПсуванняТоварів_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПсуванняТоварів_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ПсуванняТоварів_Записи.LoadRecords();

            if (ТабличніСписки.ПсуванняТоварів_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПсуванняТоварів_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ПсуванняТоварів_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПсуванняТоварів_Записи.CurrentPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ПсуванняТоварів_Записи.Where.Clear();

            //Назва
            ТабличніСписки.ПсуванняТоварів_Записи.Where.Add(
                new Where(ПсуванняТоварів_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.ПсуванняТоварів_Записи.LoadRecords();

            if (ТабличніСписки.ПсуванняТоварів_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПсуванняТоварів_Записи.FirstPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{ПсуванняТоварів_Const.FULLNAME} *", () =>
                {
                    ПсуванняТоварів_Елемент page = new ПсуванняТоварів_Елемент
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
                ПсуванняТоварів_Objest ПсуванняТоварів_Objest = new ПсуванняТоварів_Objest();
                if (await ПсуванняТоварів_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ПсуванняТоварів_Objest.Назва}", () =>
                    {
                        ПсуванняТоварів_Елемент page = new ПсуванняТоварів_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            ПсуванняТоварів_Objest = ПсуванняТоварів_Objest,
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
            ПсуванняТоварів_Objest ПсуванняТоварів_Objest = new ПсуванняТоварів_Objest();
            if (await ПсуванняТоварів_Objest.Read(unigueID))
                await ПсуванняТоварів_Objest.SetDeletionLabel(!ПсуванняТоварів_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ПсуванняТоварів_Objest ПсуванняТоварів_Objest = new ПсуванняТоварів_Objest();
            if (await ПсуванняТоварів_Objest.Read(unigueID))
            {
                ПсуванняТоварів_Objest ПсуванняТоварів_Objest_Новий = ПсуванняТоварів_Objest.Copy(true);
                await ПсуванняТоварів_Objest_Новий.Save();
                await ПсуванняТоварів_Objest_Новий.Товари_TablePart.Save(true);

                return ПсуванняТоварів_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        protected override void PeriodWhereChanged()
        {
            ТабличніСписки.ПсуванняТоварів_Записи.ДодатиВідбірПоПеріоду(Enum.Parse<ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ПсуванняТоварів_Pointer ПсуванняТоварів_Pointer = new ПсуванняТоварів_Pointer(unigueID);
            ПсуванняТоварів_Objest? ПсуванняТоварів_Objest = await ПсуванняТоварів_Pointer.GetDocumentObject(true);
            if (ПсуванняТоварів_Objest == null) return;

            if (spendDoc)
            {
                if (!await ПсуванняТоварів_Objest.SpendTheDocument(ПсуванняТоварів_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ВідкритиТермінал();
            }
            else
                await ПсуванняТоварів_Objest.ClearSpendTheDocument();
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new ПсуванняТоварів_Pointer(unigueID);
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{ПсуванняТоварів_Const.FULLNAME}_{unigueID}.xml");
            await ПсуванняТоварів_Export.ToXmlFile(new ПсуванняТоварів_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar


        #endregion
    }
}