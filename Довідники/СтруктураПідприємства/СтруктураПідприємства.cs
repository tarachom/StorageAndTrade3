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
    public class СтруктураПідприємства : ДовідникЖурнал
    {
        public СтруктураПідприємства() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.СтруктураПідприємства_Записи.Store;
            ТабличніСписки.СтруктураПідприємства_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.СтруктураПідприємства_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.СтруктураПідприємства_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СтруктураПідприємства_Записи.Where.Clear();

            ТабличніСписки.СтруктураПідприємства_Записи.LoadRecords();

            if (ТабличніСписки.СтруктураПідприємства_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.СтруктураПідприємства_Записи.SelectPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.СтруктураПідприємства_Записи.Where.Clear();

            //Назва
            ТабличніСписки.СтруктураПідприємства_Записи.Where.Add(
                new Where(СтруктураПідприємства_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.СтруктураПідприємства_Записи.LoadRecords();

            if (ТабличніСписки.СтруктураПідприємства_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.СтруктураПідприємства_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{СтруктураПідприємства_Const.FULLNAME} *", () =>
                {
                    СтруктураПідприємства_Елемент page = new СтруктураПідприємства_Елемент
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
                СтруктураПідприємства_Objest СтруктураПідприємства_Objest = new СтруктураПідприємства_Objest();
                if (await СтруктураПідприємства_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{СтруктураПідприємства_Objest.Назва}", () =>
                    {
                        СтруктураПідприємства_Елемент page = new СтруктураПідприємства_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            СтруктураПідприємства_Objest = СтруктураПідприємства_Objest,
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
            СтруктураПідприємства_Objest СтруктураПідприємства_Objest = new СтруктураПідприємства_Objest();
            if (await СтруктураПідприємства_Objest.Read(unigueID))
                await СтруктураПідприємства_Objest.SetDeletionLabel(!СтруктураПідприємства_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            СтруктураПідприємства_Objest СтруктураПідприємства_Objest = new СтруктураПідприємства_Objest();
            if (await СтруктураПідприємства_Objest.Read(unigueID))
            {
                СтруктураПідприємства_Objest СтруктураПідприємства_Objest_Новий = СтруктураПідприємства_Objest.Copy(true);
                await СтруктураПідприємства_Objest_Новий.Save();

                return СтруктураПідприємства_Objest_Новий.UnigueID;
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