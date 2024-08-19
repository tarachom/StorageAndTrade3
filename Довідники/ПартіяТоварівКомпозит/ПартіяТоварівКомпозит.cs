/*
Copyright (C) 2019-2024 TARAKHOMYN YURIY IVANOVYCH
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

using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0.Довідники;

using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class ПартіяТоварівКомпозит : ДовідникЖурнал
    {
        public ПартіяТоварівКомпозит() : base()
        {
            ТабличніСписки.ПартіяТоварівКомпозит_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ПартіяТоварівКомпозит_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПартіяТоварівКомпозит_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ПартіяТоварівКомпозит_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ПартіяТоварівКомпозит_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ПартіяТоварівКомпозит_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПартіяТоварівКомпозит_Записи.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Відбори
            ТабличніСписки.ПартіяТоварівКомпозит_Записи.ДодатиВідбір(TreeViewGrid, ПартіяТоварівКомпозит_ВідбориДляПошуку.Відбори(searchText), true);

            await ТабличніСписки.ПартіяТоварівКомпозит_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ПартіяТоварівКомпозит_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПартіяТоварівКомпозит_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{ПартіяТоварівКомпозит_Const.FULLNAME} *", () =>
                {
                    ПартіяТоварівКомпозит_Елемент page = new ПартіяТоварівКомпозит_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadRecords,
                        IsNew = true
                    };

                    page.SetValue();

                    return page;
                });
            }
            else if (unigueID != null)
            {
                ПартіяТоварівКомпозит_Objest ПартіяТоварівКомпозит_Objest = new ПартіяТоварівКомпозит_Objest();
                if (await ПартіяТоварівКомпозит_Objest.Read(unigueID))
                {
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{ПартіяТоварівКомпозит_Objest.Назва}", () =>
                    {
                        ПартіяТоварівКомпозит_Елемент page = new ПартіяТоварівКомпозит_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            ПартіяТоварівКомпозит_Objest = ПартіяТоварівКомпозит_Objest,
                        };

                        page.SetValue();

                        return page;
                    });
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