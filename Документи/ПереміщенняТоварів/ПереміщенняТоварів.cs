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
    public class ПереміщенняТоварів : ДокументЖурнал
    {
        public ПереміщенняТоварів() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.ПереміщенняТоварів_Записи.Store;
            ТабличніСписки.ПереміщенняТоварів_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.ПереміщенняТоварів_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПереміщенняТоварів_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ПереміщенняТоварів_Записи.LoadRecords();

            if (ТабличніСписки.ПереміщенняТоварів_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПереміщенняТоварів_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ПереміщенняТоварів_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПереміщенняТоварів_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ПереміщенняТоварів_Записи.Where.Clear();

            //Назва
            ТабличніСписки.ПереміщенняТоварів_Записи.Where.Add(
                new Where(ПереміщенняТоварів_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.ПереміщенняТоварів_Записи.LoadRecords();

            if (ТабличніСписки.ПереміщенняТоварів_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПереміщенняТоварів_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{ПереміщенняТоварів_Const.FULLNAME} *", () =>
                {
                    ПереміщенняТоварів_Елемент page = new ПереміщенняТоварів_Елемент
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
                ПереміщенняТоварів_Objest ПереміщенняТоварів_Objest = new ПереміщенняТоварів_Objest();
                if (await ПереміщенняТоварів_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ПереміщенняТоварів_Objest.Назва}", () =>
                    {
                        ПереміщенняТоварів_Елемент page = new ПереміщенняТоварів_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            ПереміщенняТоварів_Objest = ПереміщенняТоварів_Objest,
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
            ПереміщенняТоварів_Objest ПереміщенняТоварів_Objest = new ПереміщенняТоварів_Objest();
            if (await ПереміщенняТоварів_Objest.Read(unigueID))
                await ПереміщенняТоварів_Objest.SetDeletionLabel(!ПереміщенняТоварів_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ПереміщенняТоварів_Objest ПереміщенняТоварів_Objest = new ПереміщенняТоварів_Objest();
            if (await ПереміщенняТоварів_Objest.Read(unigueID))
            {
                ПереміщенняТоварів_Objest ПереміщенняТоварів_Objest_Новий = ПереміщенняТоварів_Objest.Copy(true);
                await ПереміщенняТоварів_Objest_Новий.Save();
                await ПереміщенняТоварів_Objest_Новий.Товари_TablePart.Save(true);

                return ПереміщенняТоварів_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        protected override void PeriodWhereChanged()
        {
            ТабличніСписки.ПереміщенняТоварів_Записи.ДодатиВідбірПоПеріоду(Enum.Parse<ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ПереміщенняТоварів_Pointer ПереміщенняТоварів_Pointer = new ПереміщенняТоварів_Pointer(unigueID);
            ПереміщенняТоварів_Objest? ПереміщенняТоварів_Objest = await ПереміщенняТоварів_Pointer.GetDocumentObject(true);
            if (ПереміщенняТоварів_Objest == null) return;

            if (spendDoc)
            {
                if (!await ПереміщенняТоварів_Objest.SpendTheDocument(ПереміщенняТоварів_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ВідкритиТермінал();
            }
            else
                await ПереміщенняТоварів_Objest.ClearSpendTheDocument();
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new ПереміщенняТоварів_Pointer(unigueID);
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{ПереміщенняТоварів_Const.FULLNAME}_{unigueID}.xml");
            await ПереміщенняТоварів_Export.ToXmlFile(new ПереміщенняТоварів_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar


        #endregion
    }
}