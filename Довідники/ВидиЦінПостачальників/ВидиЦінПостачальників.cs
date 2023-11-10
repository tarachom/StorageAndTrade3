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
    public class ВидиЦінПостачальників : ДовідникЖурнал
    {
        public ВидиЦінПостачальників() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.ВидиЦінПостачальників_Записи.Store;
            ТабличніСписки.ВидиЦінПостачальників_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.ВидиЦінПостачальників_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ВидиЦінПостачальників_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ВидиЦінПостачальників_Записи.Where.Clear();

            ТабличніСписки.ВидиЦінПостачальників_Записи.LoadRecords();

            if (ТабличніСписки.ВидиЦінПостачальників_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВидиЦінПостачальників_Записи.SelectPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ВидиЦінПостачальників_Записи.Where.Clear();

            //Назва
            ТабличніСписки.ВидиЦінПостачальників_Записи.Where.Add(
                new Where(ВидиЦінПостачальників_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.ВидиЦінПостачальників_Записи.LoadRecords();

            if (ТабличніСписки.ВидиЦінПостачальників_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВидиЦінПостачальників_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{ВидиЦінПостачальників_Const.FULLNAME} *", () =>
                {
                    ВидиЦінПостачальників_Елемент page = new ВидиЦінПостачальників_Елемент
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
                ВидиЦінПостачальників_Objest ВидиЦінПостачальників_Objest = new ВидиЦінПостачальників_Objest();
                if (ВидиЦінПостачальників_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ВидиЦінПостачальників_Objest.Назва}", () =>
                    {
                        ВидиЦінПостачальників_Елемент page = new ВидиЦінПостачальників_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            ВидиЦінПостачальників_Objest = ВидиЦінПостачальників_Objest,
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
            ВидиЦінПостачальників_Objest ВидиЦінПостачальників_Objest = new ВидиЦінПостачальників_Objest();
            if (ВидиЦінПостачальників_Objest.Read(unigueID))
                await ВидиЦінПостачальників_Objest.SetDeletionLabel(!ВидиЦінПостачальників_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ВидиЦінПостачальників_Objest ВидиЦінПостачальників_Objest = new ВидиЦінПостачальників_Objest();
            if (ВидиЦінПостачальників_Objest.Read(unigueID))
            {
                ВидиЦінПостачальників_Objest ВидиЦінПостачальників_Objest_Новий = ВидиЦінПостачальників_Objest.Copy(true);
                await ВидиЦінПостачальників_Objest_Новий.Save();

                return ВидиЦінПостачальників_Objest_Новий.UnigueID;
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