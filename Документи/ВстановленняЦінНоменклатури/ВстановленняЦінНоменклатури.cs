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
    public class ВстановленняЦінНоменклатури : ДокументЖурнал
    {
        public ВстановленняЦінНоменклатури() : base()
        {
            ТабличніСписки.ВстановленняЦінНоменклатури_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async void LoadRecords()
        {
            ТабличніСписки.ВстановленняЦінНоменклатури_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ВстановленняЦінНоменклатури_Записи.DocumentPointerItem = DocumentPointerItem;

            await ТабличніСписки.ВстановленняЦінНоменклатури_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ВстановленняЦінНоменклатури_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВстановленняЦінНоменклатури_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ВстановленняЦінНоменклатури_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВстановленняЦінНоменклатури_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Назва
            ТабличніСписки.ВстановленняЦінНоменклатури_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(ВстановленняЦінНоменклатури_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.ВстановленняЦінНоменклатури_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ВстановленняЦінНоменклатури_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВстановленняЦінНоменклатури_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{ВстановленняЦінНоменклатури_Const.FULLNAME} *", () =>
                {
                    ВстановленняЦінНоменклатури_Елемент page = new ВстановленняЦінНоменклатури_Елемент
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
                ВстановленняЦінНоменклатури_Objest ВстановленняЦінНоменклатури_Objest = new ВстановленняЦінНоменклатури_Objest();
                if (await ВстановленняЦінНоменклатури_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ВстановленняЦінНоменклатури_Objest.Назва}", () =>
                    {
                        ВстановленняЦінНоменклатури_Елемент page = new ВстановленняЦінНоменклатури_Елемент
                        {
                            UnigueID = unigueID,
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            ВстановленняЦінНоменклатури_Objest = ВстановленняЦінНоменклатури_Objest,
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
            ВстановленняЦінНоменклатури_Objest ВстановленняЦінНоменклатури_Objest = new ВстановленняЦінНоменклатури_Objest();
            if (await ВстановленняЦінНоменклатури_Objest.Read(unigueID))
                await ВстановленняЦінНоменклатури_Objest.SetDeletionLabel(!ВстановленняЦінНоменклатури_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ВстановленняЦінНоменклатури_Objest ВстановленняЦінНоменклатури_Objest = new ВстановленняЦінНоменклатури_Objest();
            if (await ВстановленняЦінНоменклатури_Objest.Read(unigueID))
            {
                ВстановленняЦінНоменклатури_Objest ВстановленняЦінНоменклатури_Objest_Новий = await ВстановленняЦінНоменклатури_Objest.Copy(true);
                await ВстановленняЦінНоменклатури_Objest_Новий.Save();
                await ВстановленняЦінНоменклатури_Objest_Новий.Товари_TablePart.Save(true);

                return ВстановленняЦінНоменклатури_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        protected override void PeriodWhereChanged()
        {
            ТабличніСписки.ВстановленняЦінНоменклатури_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Enum.Parse<ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ВстановленняЦінНоменклатури_Pointer ВстановленняЦінНоменклатури_Pointer = new ВстановленняЦінНоменклатури_Pointer(unigueID);
            ВстановленняЦінНоменклатури_Objest? ВстановленняЦінНоменклатури_Objest = await ВстановленняЦінНоменклатури_Pointer.GetDocumentObject(true);
            if (ВстановленняЦінНоменклатури_Objest == null) return;

            if (spendDoc)
            {
                if (!await ВстановленняЦінНоменклатури_Objest.SpendTheDocument(ВстановленняЦінНоменклатури_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(ВстановленняЦінНоменклатури_Objest.UnigueID);
            }
            else
                await ВстановленняЦінНоменклатури_Objest.ClearSpendTheDocument();
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new ВстановленняЦінНоменклатури_Pointer(unigueID);
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{ВстановленняЦінНоменклатури_Const.FULLNAME}_{unigueID}.xml");
            await ВстановленняЦінНоменклатури_Export.ToXmlFile(new ВстановленняЦінНоменклатури_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar

        #endregion
    }
}