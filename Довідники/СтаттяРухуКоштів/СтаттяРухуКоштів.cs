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

using StorageAndTrade_1_0.Довідники;

using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class СтаттяРухуКоштів : ДовідникЖурнал
    {
        public СтаттяРухуКоштів() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.СтаттяРухуКоштів_Записи.Store;
            ТабличніСписки.СтаттяРухуКоштів_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.СтаттяРухуКоштів_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.СтаттяРухуКоштів_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СтаттяРухуКоштів_Записи.Where.Clear();

            await ТабличніСписки.СтаттяРухуКоштів_Записи.LoadRecords();

            if (ТабличніСписки.СтаттяРухуКоштів_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.СтаттяРухуКоштів_Записи.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.СтаттяРухуКоштів_Записи.Where.Clear();

            //Назва
            ТабличніСписки.СтаттяРухуКоштів_Записи.Where.Add(
                new Where(СтаттяРухуКоштів_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            await ТабличніСписки.СтаттяРухуКоштів_Записи.LoadRecords();

            if (ТабличніСписки.СтаттяРухуКоштів_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.СтаттяРухуКоштів_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{СтаттяРухуКоштів_Const.FULLNAME} *", () =>
                {
                    СтаттяРухуКоштів_Елемент page = new СтаттяРухуКоштів_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadRecords,
                        IsNew = true
                    };

                    page.SetValue();

                    return page;
                }, true);
            }
            else if (unigueID != null)
            {
                СтаттяРухуКоштів_Objest СтаттяРухуКоштів_Objest = new СтаттяРухуКоштів_Objest();
                if (await СтаттяРухуКоштів_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{СтаттяРухуКоштів_Objest.Назва}", () =>
                    {
                        СтаттяРухуКоштів_Елемент page = new СтаттяРухуКоштів_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            СтаттяРухуКоштів_Objest = СтаттяРухуКоштів_Objest,
                        };

                        page.SetValue();

                        return page;
                    }, true);
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            СтаттяРухуКоштів_Objest СтаттяРухуКоштів_Objest = new СтаттяРухуКоштів_Objest();
            if (await СтаттяРухуКоштів_Objest.Read(unigueID))
                await СтаттяРухуКоштів_Objest.SetDeletionLabel(!СтаттяРухуКоштів_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            СтаттяРухуКоштів_Objest СтаттяРухуКоштів_Objest = new СтаттяРухуКоштів_Objest();
            if (await СтаттяРухуКоштів_Objest.Read(unigueID))
            {
                СтаттяРухуКоштів_Objest СтаттяРухуКоштів_Objest_Новий = СтаттяРухуКоштів_Objest.Copy(true);
                await СтаттяРухуКоштів_Objest_Новий.Save();

                return СтаттяРухуКоштів_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        #endregion
    }
}