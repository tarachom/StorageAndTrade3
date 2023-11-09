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
    public class ВидиЦін : ДовідникЖурнал
    {
        public ВидиЦін() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.ВидиЦін_Записи.Store;
            ТабличніСписки.ВидиЦін_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.ВидиЦін_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ВидиЦін_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ВидиЦін_Записи.Where.Clear();

            ТабличніСписки.ВидиЦін_Записи.LoadRecords();

            if (ТабличніСписки.ВидиЦін_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВидиЦін_Записи.SelectPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ВидиЦін_Записи.Where.Clear();

            //Назва
            ТабличніСписки.ВидиЦін_Записи.Where.Add(
                new Where(ВидиЦін_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.ВидиЦін_Записи.LoadRecords();

            if (ТабличніСписки.ВидиЦін_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВидиЦін_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{ВидиЦін_Const.FULLNAME} *", () =>
                {
                    ВидиЦін_Елемент page = new ВидиЦін_Елемент
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
                ВидиЦін_Objest ВидиЦін_Objest = new ВидиЦін_Objest();
                if (ВидиЦін_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ВидиЦін_Objest.Назва}", () =>
                    {
                        ВидиЦін_Елемент page = new ВидиЦін_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            ВидиЦін_Objest = ВидиЦін_Objest,
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
            ВидиЦін_Objest ВидиЦін_Objest = new ВидиЦін_Objest();
            if (ВидиЦін_Objest.Read(unigueID))
                ВидиЦін_Objest.SetDeletionLabel(!ВидиЦін_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override UnigueID? Copy(UnigueID unigueID)
        {
            ВидиЦін_Objest ВидиЦін_Objest = new ВидиЦін_Objest();
            if (ВидиЦін_Objest.Read(unigueID))
            {
                ВидиЦін_Objest ВидиЦін_Objest_Новий = ВидиЦін_Objest.Copy(true);
                ВидиЦін_Objest_Новий.Save();

                return ВидиЦін_Objest_Новий.UnigueID;
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