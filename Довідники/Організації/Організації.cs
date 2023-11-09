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
    public class Організації : ДовідникЖурнал
    {
        public Організації() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.Організації_Записи.Store;
            ТабличніСписки.Організації_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.Організації_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Організації_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Організації_Записи.Where.Clear();

            ТабличніСписки.Організації_Записи.LoadRecords();

            if (ТабличніСписки.Організації_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Організації_Записи.SelectPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.Організації_Записи.Where.Clear();

            //Назва
            ТабличніСписки.Організації_Записи.Where.Add(
                new Where(Організації_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.Організації_Записи.LoadRecords();

            if (ТабличніСписки.Організації_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Організації_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{Організації_Const.FULLNAME} *", () =>
                {
                    Організації_Елемент page = new Організації_Елемент
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
                Організації_Objest Організації_Objest = new Організації_Objest();
                if (Організації_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{Організації_Objest.Назва}", () =>
                    {
                        Організації_Елемент page = new Організації_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            Організації_Objest = Організації_Objest,
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
            Організації_Objest Організації_Objest = new Організації_Objest();
            if (Організації_Objest.Read(unigueID))
                Організації_Objest.SetDeletionLabel(!Організації_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override UnigueID? Copy(UnigueID unigueID)
        {
            Організації_Objest Організації_Objest = new Організації_Objest();
            if (Організації_Objest.Read(unigueID))
            {
                Організації_Objest Організації_Objest_Новий = Організації_Objest.Copy(true);
                Організації_Objest_Новий.Save();
                Організації_Objest_Новий.Контакти_TablePart.Save(false);

                return Організації_Objest_Новий.UnigueID;
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