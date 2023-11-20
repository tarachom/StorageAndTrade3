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
    public class ВнутрішнєСпоживанняТоварів : ДокументЖурнал
    {
        public ВнутрішнєСпоживанняТоварів() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.Store;
            ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.LoadRecords();

            if (ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.CurrentPath, TreeViewGrid.Columns[0], false);


        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.Where.Clear();

            //Назва
            ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.Where.Add(
                new Where(ВнутрішнєСпоживанняТоварів_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.LoadRecords();

            if (ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{ВнутрішнєСпоживанняТоварів_Const.FULLNAME} *", () =>
                {
                    ВнутрішнєСпоживанняТоварів_Елемент page = new ВнутрішнєСпоживанняТоварів_Елемент
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
                ВнутрішнєСпоживанняТоварів_Objest ВнутрішнєСпоживанняТоварів_Objest = new ВнутрішнєСпоживанняТоварів_Objest();
                if (await ВнутрішнєСпоживанняТоварів_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ВнутрішнєСпоживанняТоварів_Objest.Назва}", () =>
                    {
                        ВнутрішнєСпоживанняТоварів_Елемент page = new ВнутрішнєСпоживанняТоварів_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            ВнутрішнєСпоживанняТоварів_Objest = ВнутрішнєСпоживанняТоварів_Objest,
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
            ВнутрішнєСпоживанняТоварів_Objest ВнутрішнєСпоживанняТоварів_Objest = new ВнутрішнєСпоживанняТоварів_Objest();
            if (await ВнутрішнєСпоживанняТоварів_Objest.Read(unigueID))
                await ВнутрішнєСпоживанняТоварів_Objest.SetDeletionLabel(!ВнутрішнєСпоживанняТоварів_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ВнутрішнєСпоживанняТоварів_Objest ВнутрішнєСпоживанняТоварів_Objest = new ВнутрішнєСпоживанняТоварів_Objest();
            if (await ВнутрішнєСпоживанняТоварів_Objest.Read(unigueID))
            {
                ВнутрішнєСпоживанняТоварів_Objest ВнутрішнєСпоживанняТоварів_Objest_Новий = ВнутрішнєСпоживанняТоварів_Objest.Copy(true);
                await ВнутрішнєСпоживанняТоварів_Objest_Новий.Save();
                await ВнутрішнєСпоживанняТоварів_Objest_Новий.Товари_TablePart.Save(true);

                return ВнутрішнєСпоживанняТоварів_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        protected override void PeriodWhereChanged()
        {
            ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.ДодатиВідбірПоПеріоду(Enum.Parse<ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ВнутрішнєСпоживанняТоварів_Pointer ВнутрішнєСпоживанняТоварів_Pointer = new ВнутрішнєСпоживанняТоварів_Pointer(unigueID);
            ВнутрішнєСпоживанняТоварів_Objest? ВнутрішнєСпоживанняТоварів_Objest = await ВнутрішнєСпоживанняТоварів_Pointer.GetDocumentObject(true);
            if (ВнутрішнєСпоживанняТоварів_Objest == null) return;

            if (spendDoc)
            {
                if (!await ВнутрішнєСпоживанняТоварів_Objest.SpendTheDocument(ВнутрішнєСпоживанняТоварів_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ВідкритиТермінал();
            }
            else
                await ВнутрішнєСпоживанняТоварів_Objest.ClearSpendTheDocument();
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new ВнутрішнєСпоживанняТоварів_Pointer(unigueID);
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{ВнутрішнєСпоживанняТоварів_Const.FULLNAME}_{unigueID}.xml");
            await ВнутрішнєСпоживанняТоварів_Export.ToXmlFile(new ВнутрішнєСпоживанняТоварів_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar

        #endregion
    }
}