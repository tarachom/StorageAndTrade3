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
    public class СеріїНоменклатури : ДовідникЖурнал
    {
        public СеріїНоменклатури() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.СеріїНоменклатури_Записи.Store;
            ТабличніСписки.СеріїНоменклатури_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.СеріїНоменклатури_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.СеріїНоменклатури_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СеріїНоменклатури_Записи.Where.Clear();

            ТабличніСписки.СеріїНоменклатури_Записи.LoadRecords();

            if (ТабличніСписки.СеріїНоменклатури_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.СеріїНоменклатури_Записи.SelectPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.СеріїНоменклатури_Записи.Where.Clear();

            //Номер
            ТабличніСписки.СеріїНоменклатури_Записи.Where.Add(
                new Where(СеріїНоменклатури_Const.Номер, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.СеріїНоменклатури_Записи.LoadRecords();

            if (ТабличніСписки.СеріїНоменклатури_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.СеріїНоменклатури_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{СеріїНоменклатури_Const.FULLNAME} *", () =>
                {
                    СеріїНоменклатури_Елемент page = new СеріїНоменклатури_Елемент
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
                СеріїНоменклатури_Objest СеріїНоменклатури_Objest = new СеріїНоменклатури_Objest();
                if (СеріїНоменклатури_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{СеріїНоменклатури_Objest.Номер}", () =>
                    {
                        СеріїНоменклатури_Елемент page = new СеріїНоменклатури_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            СеріїНоменклатури_Objest = СеріїНоменклатури_Objest,
                        };

                        page.SetValue();

                        return page;
                    }, true);
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }

        protected override void SetDeletionLabel(UnigueID unigueID)
        {
            СеріїНоменклатури_Objest СеріїНоменклатури_Objest = new СеріїНоменклатури_Objest();
            if (СеріїНоменклатури_Objest.Read(unigueID))
                СеріїНоменклатури_Objest.SetDeletionLabel(!СеріїНоменклатури_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override UnigueID? Copy(UnigueID unigueID)
        {
            СеріїНоменклатури_Objest СеріїНоменклатури_Objest = new СеріїНоменклатури_Objest();
            if (СеріїНоменклатури_Objest.Read(unigueID))
            {
                СеріїНоменклатури_Objest СеріїНоменклатури_Objest_Новий = СеріїНоменклатури_Objest.Copy(true);
                СеріїНоменклатури_Objest_Новий.Save();

                return СеріїНоменклатури_Objest_Новий.UnigueID;
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