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
    public class Каси : ДовідникЖурнал
    {
        public Каси() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.Каси_Записи.Store;
            ТабличніСписки.Каси_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.Каси_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Каси_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Каси_Записи.Where.Clear();

            await ТабличніСписки.Каси_Записи.LoadRecords();

            if (ТабличніСписки.Каси_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Каси_Записи.SelectPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.Каси_Записи.Where.Clear();

            //Назва
            ТабличніСписки.Каси_Записи.Where.Add(
                new Where(Каси_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            await ТабличніСписки.Каси_Записи.LoadRecords();

            if (ТабличніСписки.Каси_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Каси_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{Каси_Const.FULLNAME} *", () =>
                {
                    Каси_Елемент page = new Каси_Елемент
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
                Каси_Objest Каси_Objest = new Каси_Objest();
                if (await Каси_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{Каси_Objest.Назва}", () =>
                    {
                        Каси_Елемент page = new Каси_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            Каси_Objest = Каси_Objest,
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
            Каси_Objest Каси_Objest = new Каси_Objest();
            if (await Каси_Objest.Read(unigueID))
                await Каси_Objest.SetDeletionLabel(!Каси_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            Каси_Objest Каси_Objest = new Каси_Objest();
            if (await Каси_Objest.Read(unigueID))
            {
                Каси_Objest Каси_Objest_Новий = Каси_Objest.Copy(true);
                await Каси_Objest_Новий.Save();

                return Каси_Objest_Новий.UnigueID;
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