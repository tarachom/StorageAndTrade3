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
    public class ПрихіднийКасовийОрдер : ДокументЖурнал
    {
        public ПрихіднийКасовийОрдер() : base()
        {
            ТабличніСписки.ПрихіднийКасовийОрдер_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async void LoadRecords()
        {
            ТабличніСписки.ПрихіднийКасовийОрдер_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПрихіднийКасовийОрдер_Записи.DocumentPointerItem = DocumentPointerItem;

            await ТабличніСписки.ПрихіднийКасовийОрдер_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ПрихіднийКасовийОрдер_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПрихіднийКасовийОрдер_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ПрихіднийКасовийОрдер_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПрихіднийКасовийОрдер_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Назва
            ТабличніСписки.ПрихіднийКасовийОрдер_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(ПрихіднийКасовийОрдер_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.ПрихіднийКасовийОрдер_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ПрихіднийКасовийОрдер_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПрихіднийКасовийОрдер_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{ПрихіднийКасовийОрдер_Const.FULLNAME} *", () =>
                {
                    ПрихіднийКасовийОрдер_Елемент page = new ПрихіднийКасовийОрдер_Елемент
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
                ПрихіднийКасовийОрдер_Objest ПрихіднийКасовийОрдер_Objest = new ПрихіднийКасовийОрдер_Objest();
                if (await ПрихіднийКасовийОрдер_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ПрихіднийКасовийОрдер_Objest.Назва}", () =>
                    {
                        ПрихіднийКасовийОрдер_Елемент page = new ПрихіднийКасовийОрдер_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            ПрихіднийКасовийОрдер_Objest = ПрихіднийКасовийОрдер_Objest,
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
            ПрихіднийКасовийОрдер_Objest ПрихіднийКасовийОрдер_Objest = new ПрихіднийКасовийОрдер_Objest();
            if (await ПрихіднийКасовийОрдер_Objest.Read(unigueID))
                await ПрихіднийКасовийОрдер_Objest.SetDeletionLabel(!ПрихіднийКасовийОрдер_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ПрихіднийКасовийОрдер_Objest ПрихіднийКасовийОрдер_Objest = new ПрихіднийКасовийОрдер_Objest();
            if (await ПрихіднийКасовийОрдер_Objest.Read(unigueID))
            {
                ПрихіднийКасовийОрдер_Objest ПрихіднийКасовийОрдер_Objest_Новий = await ПрихіднийКасовийОрдер_Objest.Copy(true);
                await ПрихіднийКасовийОрдер_Objest_Новий.Save();

                return ПрихіднийКасовийОрдер_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        protected override void PeriodWhereChanged()
        {
            ТабличніСписки.ПрихіднийКасовийОрдер_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Enum.Parse<ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ПрихіднийКасовийОрдер_Pointer ПрихіднийКасовийОрдер_Pointer = new ПрихіднийКасовийОрдер_Pointer(unigueID);
            ПрихіднийКасовийОрдер_Objest? ПрихіднийКасовийОрдер_Objest = await ПрихіднийКасовийОрдер_Pointer.GetDocumentObject(true);
            if (ПрихіднийКасовийОрдер_Objest == null) return;

            if (spendDoc)
            {
                if (!await ПрихіднийКасовийОрдер_Objest.SpendTheDocument(ПрихіднийКасовийОрдер_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ВідкритиТермінал();
            }
            else
                await ПрихіднийКасовийОрдер_Objest.ClearSpendTheDocument();
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new ПрихіднийКасовийОрдер_Pointer(unigueID);
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{ПрихіднийКасовийОрдер_Const.FULLNAME}_{unigueID}.xml");
            await ПрихіднийКасовийОрдер_Export.ToXmlFile(new ПрихіднийКасовийОрдер_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar


        #endregion
    }
}