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
    public class ТипорозміриКомірок : ДовідникЖурнал
    {
        public ТипорозміриКомірок() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.ТипорозміриКомірок_Записи.Store;
            ТабличніСписки.ТипорозміриКомірок_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ТипорозміриКомірок_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ТипорозміриКомірок_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ТипорозміриКомірок_Записи.Where.Clear();

            await ТабличніСписки.ТипорозміриКомірок_Записи.LoadRecords();

            if (ТабличніСписки.ТипорозміриКомірок_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ТипорозміриКомірок_Записи.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ТипорозміриКомірок_Записи.Where.Clear();

            //Назва
            ТабличніСписки.ТипорозміриКомірок_Записи.Where.Add(
                new Where(ТипорозміриКомірок_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            await ТабличніСписки.ТипорозміриКомірок_Записи.LoadRecords();

            if (ТабличніСписки.ТипорозміриКомірок_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ТипорозміриКомірок_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{ТипорозміриКомірок_Const.FULLNAME} *", () =>
                {
                    ТипорозміриКомірок_Елемент page = new ТипорозміриКомірок_Елемент
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
                ТипорозміриКомірок_Objest ТипорозміриКомірок_Objest = new ТипорозміриКомірок_Objest();
                if (await ТипорозміриКомірок_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ТипорозміриКомірок_Objest.Назва}", () =>
                    {
                        ТипорозміриКомірок_Елемент page = new ТипорозміриКомірок_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            ТипорозміриКомірок_Objest = ТипорозміриКомірок_Objest,
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
            ТипорозміриКомірок_Objest ТипорозміриКомірок_Objest = new ТипорозміриКомірок_Objest();
            if (await ТипорозміриКомірок_Objest.Read(unigueID))
                await ТипорозміриКомірок_Objest.SetDeletionLabel(!ТипорозміриКомірок_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ТипорозміриКомірок_Objest ТипорозміриКомірок_Objest = new ТипорозміриКомірок_Objest();
            if (await ТипорозміриКомірок_Objest.Read(unigueID))
            {
                ТипорозміриКомірок_Objest ТипорозміриКомірок_Objest_Новий = await ТипорозміриКомірок_Objest.Copy(true);
                await ТипорозміриКомірок_Objest_Новий.Save();

                return ТипорозміриКомірок_Objest_Новий.UnigueID;
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