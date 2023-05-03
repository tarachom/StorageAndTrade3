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
    public class Файли : ДовідникЖурнал
    {
        public Файли() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.Файли_Записи.Store;
            ТабличніСписки.Файли_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.Файли_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Файли_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Файли_Записи.Where.Clear();

            ТабличніСписки.Файли_Записи.LoadRecords();

            if (ТабличніСписки.Файли_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Файли_Записи.SelectPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.Файли_Записи.Where.Clear();

            //Назва
            ТабличніСписки.Файли_Записи.Where.Add(
                new Where(Файли_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.Файли_Записи.LoadRecords();

            if (ТабличніСписки.Файли_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Файли_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{Файли_Const.FULLNAME} *", () =>
                {
                    Файли_Елемент page = new Файли_Елемент
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
                Файли_Objest Файли_Objest = new Файли_Objest();
                if (Файли_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{Файли_Objest.Назва}", () =>
                    {
                        Файли_Елемент page = new Файли_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            Файли_Objest = Файли_Objest,
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
            Файли_Objest Файли_Objest = new Файли_Objest();
            if (Файли_Objest.Read(unigueID))
                Файли_Objest.SetDeletionLabel(!Файли_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override UnigueID? Copy(UnigueID unigueID)
        {
            Файли_Objest Файли_Objest = new Файли_Objest();
            if (Файли_Objest.Read(unigueID))
            {
                Файли_Objest Файли_Objest_Новий = Файли_Objest.Copy(true);
                Файли_Objest_Новий.Save();

                return Файли_Objest_Новий.UnigueID;
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