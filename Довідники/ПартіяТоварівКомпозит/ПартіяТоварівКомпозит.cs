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
using GLib;
using StorageAndTrade_1_0.Довідники;

using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class ПартіяТоварівКомпозит : ДовідникЖурнал
    {
        public ПартіяТоварівКомпозит() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.ПартіяТоварівКомпозит_Записи.Store;
            ТабличніСписки.ПартіяТоварівКомпозит_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ПартіяТоварівКомпозит_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПартіяТоварівКомпозит_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ПартіяТоварівКомпозит_Записи.Where.Clear();

            await ТабличніСписки.ПартіяТоварівКомпозит_Записи.LoadRecords();

            if (ТабличніСписки.ПартіяТоварівКомпозит_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПартіяТоварівКомпозит_Записи.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ПартіяТоварівКомпозит_Записи.Where.Clear();

            //Назва
            ТабличніСписки.ПартіяТоварівКомпозит_Записи.Where.Add(
                new Where(ПартіяТоварівКомпозит_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            await ТабличніСписки.ПартіяТоварівКомпозит_Записи.LoadRecords();

            if (ТабличніСписки.ПартіяТоварівКомпозит_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПартіяТоварівКомпозит_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{ПартіяТоварівКомпозит_Const.FULLNAME} *", () =>
                {
                    ПартіяТоварівКомпозит_Елемент page = new ПартіяТоварівКомпозит_Елемент
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
                ПартіяТоварівКомпозит_Objest ПартіяТоварівКомпозит_Objest = new ПартіяТоварівКомпозит_Objest();
                if (await ПартіяТоварівКомпозит_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ПартіяТоварівКомпозит_Objest.Назва}", () =>
                    {
                        ПартіяТоварівКомпозит_Елемент page = new ПартіяТоварівКомпозит_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            ПартіяТоварівКомпозит_Objest = ПартіяТоварівКомпозит_Objest,
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
            ПартіяТоварівКомпозит_Objest ПартіяТоварівКомпозит_Objest = new ПартіяТоварівКомпозит_Objest();
            if (await ПартіяТоварівКомпозит_Objest.Read(unigueID))
                await ПартіяТоварівКомпозит_Objest.SetDeletionLabel(!ПартіяТоварівКомпозит_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ПартіяТоварівКомпозит_Objest ПартіяТоварівКомпозит_Objest = new ПартіяТоварівКомпозит_Objest();
            if (await ПартіяТоварівКомпозит_Objest.Read(unigueID))
            {
                ПартіяТоварівКомпозит_Objest ПартіяТоварівКомпозит_Objest_Новий = await ПартіяТоварівКомпозит_Objest.Copy(true);
                await ПартіяТоварівКомпозит_Objest_Новий.Save();

                return ПартіяТоварівКомпозит_Objest_Новий.UnigueID;
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