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
    public class ФізичніОсоби : ДовідникЖурнал
    {
        public ФізичніОсоби() : base()
        {
            ТабличніСписки.ФізичніОсоби_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ФізичніОсоби_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ФізичніОсоби_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ФізичніОсоби_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ФізичніОсоби_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ФізичніОсоби_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ФізичніОсоби_Записи.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Відбори
            ТабличніСписки.ФізичніОсоби_Записи.ДодатиВідбір(TreeViewGrid, ФізичніОсоби_ВідбориДляПошуку.Відбори(searchText), true);

            await ТабличніСписки.ФізичніОсоби_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ФізичніОсоби_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ФізичніОсоби_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{ФізичніОсоби_Const.FULLNAME} *", () =>
                {
                    ФізичніОсоби_Елемент page = new ФізичніОсоби_Елемент
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
                ФізичніОсоби_Objest ФізичніОсоби_Objest = new ФізичніОсоби_Objest();
                if (await ФізичніОсоби_Objest.Read(unigueID))
                {
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{ФізичніОсоби_Objest.Назва}", () =>
                    {
                        ФізичніОсоби_Елемент page = new ФізичніОсоби_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            ФізичніОсоби_Objest = ФізичніОсоби_Objest,
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
            ФізичніОсоби_Objest ФізичніОсоби_Objest = new ФізичніОсоби_Objest();
            if (await ФізичніОсоби_Objest.Read(unigueID))
                await ФізичніОсоби_Objest.SetDeletionLabel(!ФізичніОсоби_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ФізичніОсоби_Objest ФізичніОсоби_Objest = new ФізичніОсоби_Objest();
            if (await ФізичніОсоби_Objest.Read(unigueID))
            {
                ФізичніОсоби_Objest ФізичніОсоби_Objest_Новий = await ФізичніОсоби_Objest.Copy(true);
                await ФізичніОсоби_Objest_Новий.Save();

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