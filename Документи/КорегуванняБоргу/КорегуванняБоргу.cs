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
    public class КорегуванняБоргу : ДокументЖурнал
    {
        public КорегуванняБоргу() : base()
        {
            ТабличніСписки.КорегуванняБоргу_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async void LoadRecords()
        {
            ТабличніСписки.КорегуванняБоргу_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.КорегуванняБоргу_Записи.DocumentPointerItem = DocumentPointerItem;

            await ТабличніСписки.КорегуванняБоргу_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.КорегуванняБоргу_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.КорегуванняБоргу_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.КорегуванняБоргу_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.КорегуванняБоргу_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Назва
            ТабличніСписки.КорегуванняБоргу_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(КорегуванняБоргу_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.КорегуванняБоргу_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.КорегуванняБоргу_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.КорегуванняБоргу_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{КорегуванняБоргу_Const.FULLNAME} *", () =>
                {
                    КорегуванняБоргу_Елемент page = new КорегуванняБоргу_Елемент
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
                КорегуванняБоргу_Objest КорегуванняБоргу_Objest = new КорегуванняБоргу_Objest();
                if (await КорегуванняБоргу_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{КорегуванняБоргу_Objest.Назва}", () =>
                    {
                        КорегуванняБоргу_Елемент page = new КорегуванняБоргу_Елемент
                        {
                            UnigueID = unigueID,
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            КорегуванняБоргу_Objest = КорегуванняБоргу_Objest,
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
            КорегуванняБоргу_Objest КорегуванняБоргу_Objest = new КорегуванняБоргу_Objest();
            if (await КорегуванняБоргу_Objest.Read(unigueID))
                await КорегуванняБоргу_Objest.SetDeletionLabel(!КорегуванняБоргу_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            КорегуванняБоргу_Objest КорегуванняБоргу_Objest = new КорегуванняБоргу_Objest();
            if (await КорегуванняБоргу_Objest.Read(unigueID))
            {
                КорегуванняБоргу_Objest КорегуванняБоргу_Objest_Новий = await КорегуванняБоргу_Objest.Copy(true);
                await КорегуванняБоргу_Objest_Новий.Save();
                await КорегуванняБоргу_Objest_Новий.РозрахункиЗКонтрагентами_TablePart.Save(true);

                return КорегуванняБоргу_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        protected override void PeriodWhereChanged()
        {
            ТабличніСписки.КорегуванняБоргу_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Enum.Parse<ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            КорегуванняБоргу_Pointer КорегуванняБоргу_Pointer = new КорегуванняБоргу_Pointer(unigueID);
            КорегуванняБоргу_Objest? КорегуванняБоргу_Objest = await КорегуванняБоргу_Pointer.GetDocumentObject(true);
            if (КорегуванняБоргу_Objest == null) return;

            if (spendDoc)
            {
                if (!await КорегуванняБоргу_Objest.SpendTheDocument(КорегуванняБоргу_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(КорегуванняБоргу_Objest.UnigueID);
            }
            else
                await КорегуванняБоргу_Objest.ClearSpendTheDocument();
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new КорегуванняБоргу_Pointer(unigueID);
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{КорегуванняБоргу_Const.FULLNAME}_{unigueID}.xml");
            await КорегуванняБоргу_Export.ToXmlFile(new КорегуванняБоргу_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar

        #endregion
    }
}