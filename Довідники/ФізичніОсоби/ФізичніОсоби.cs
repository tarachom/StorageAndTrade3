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

using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0.Довідники;

using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class ФізичніОсоби : ДовідникЖурнал
    {
        public ФізичніОсоби() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.ФізичніОсоби_Записи.Store;
            ТабличніСписки.ФізичніОсоби_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.ФізичніОсоби_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ФізичніОсоби_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ФізичніОсоби_Записи.Where.Clear();

            ТабличніСписки.ФізичніОсоби_Записи.LoadRecords();

            if (ТабличніСписки.ФізичніОсоби_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ФізичніОсоби_Записи.SelectPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ФізичніОсоби_Записи.Where.Clear();

            //Назва
            ТабличніСписки.ФізичніОсоби_Записи.Where.Add(
                new Where(ФізичніОсоби_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.ФізичніОсоби_Записи.LoadRecords();

            if (ТабличніСписки.ФізичніОсоби_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ФізичніОсоби_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{ФізичніОсоби_Const.FULLNAME} *", () =>
                {
                    ФізичніОсоби_Елемент page = new ФізичніОсоби_Елемент
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
                ФізичніОсоби_Objest ФізичніОсоби_Objest = new ФізичніОсоби_Objest();
                if (ФізичніОсоби_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ФізичніОсоби_Objest.Назва}", () =>
                    {
                        ФізичніОсоби_Елемент page = new ФізичніОсоби_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            ФізичніОсоби_Objest = ФізичніОсоби_Objest,
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
            ФізичніОсоби_Objest ФізичніОсоби_Objest = new ФізичніОсоби_Objest();
            if (ФізичніОсоби_Objest.Read(unigueID))
                ФізичніОсоби_Objest.SetDeletionLabel(!ФізичніОсоби_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override UnigueID? Copy(UnigueID unigueID)
        {
            ФізичніОсоби_Objest ФізичніОсоби_Objest = new ФізичніОсоби_Objest();
            if (ФізичніОсоби_Objest.Read(unigueID))
            {
                ФізичніОсоби_Objest ФізичніОсоби_Objest_Новий = ФізичніОсоби_Objest.Copy(true);
                ФізичніОсоби_Objest_Новий.Save();

                return ФізичніОсоби_Objest_Новий.UnigueID;
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