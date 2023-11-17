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
    public class КраїниСвіту : ДовідникЖурнал
    {
        public КраїниСвіту() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.КраїниСвіту_Записи.Store;
            ТабличніСписки.КраїниСвіту_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.КраїниСвіту_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.КраїниСвіту_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.КраїниСвіту_Записи.Where.Clear();

            ТабличніСписки.КраїниСвіту_Записи.LoadRecords();

            if (ТабличніСписки.КраїниСвіту_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.КраїниСвіту_Записи.SelectPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.КраїниСвіту_Записи.Where.Clear();

            //Назва
            ТабличніСписки.КраїниСвіту_Записи.Where.Add(
                new Where(КраїниСвіту_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.КраїниСвіту_Записи.LoadRecords();

            if (ТабличніСписки.КраїниСвіту_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.КраїниСвіту_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{КраїниСвіту_Const.FULLNAME} *", () =>
                {
                    КраїниСвіту_Елемент page = new КраїниСвіту_Елемент
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
                КраїниСвіту_Objest КраїниСвіту_Objest = new КраїниСвіту_Objest();
                if (await КраїниСвіту_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{КраїниСвіту_Objest.Назва}", () =>
                    {
                        КраїниСвіту_Елемент page = new КраїниСвіту_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            КраїниСвіту_Objest = КраїниСвіту_Objest,
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
            КраїниСвіту_Objest КраїниСвіту_Objest = new КраїниСвіту_Objest();
            if (await КраїниСвіту_Objest.Read(unigueID))
                await КраїниСвіту_Objest.SetDeletionLabel(!КраїниСвіту_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            КраїниСвіту_Objest КраїниСвіту_Objest = new КраїниСвіту_Objest();
            if (await КраїниСвіту_Objest.Read(unigueID))
            {
                КраїниСвіту_Objest КраїниСвіту_Objest_Новий = КраїниСвіту_Objest.Copy(true);
                await КраїниСвіту_Objest_Новий.Save();

                return КраїниСвіту_Objest_Новий.UnigueID;
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