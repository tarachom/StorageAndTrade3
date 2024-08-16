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
    public class СеріїНоменклатури : ДовідникЖурнал
    {
        public СеріїНоменклатури() : base()
        {
            ТабличніСписки.СеріїНоменклатури_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.СеріїНоменклатури_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.СеріїНоменклатури_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СеріїНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.СеріїНоменклатури_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.СеріїНоменклатури_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.СеріїНоменклатури_Записи.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Відбори
            ТабличніСписки.СеріїНоменклатури_Записи.ДодатиВідбір(TreeViewGrid, СеріїНоменклатури_ВідбориДляПошуку.Відбори(searchText), true);

            await ТабличніСписки.СеріїНоменклатури_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.СеріїНоменклатури_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.СеріїНоменклатури_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{СеріїНоменклатури_Const.FULLNAME} *", () =>
                {
                    СеріїНоменклатури_Елемент page = new СеріїНоменклатури_Елемент
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
                СеріїНоменклатури_Objest СеріїНоменклатури_Objest = new СеріїНоменклатури_Objest();
                if (await СеріїНоменклатури_Objest.Read(unigueID))
                {
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{СеріїНоменклатури_Objest.Номер}", () =>
                    {
                        СеріїНоменклатури_Елемент page = new СеріїНоменклатури_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            СеріїНоменклатури_Objest = СеріїНоменклатури_Objest,
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
            СеріїНоменклатури_Objest СеріїНоменклатури_Objest = new СеріїНоменклатури_Objest();
            if (await СеріїНоменклатури_Objest.Read(unigueID))
                await СеріїНоменклатури_Objest.SetDeletionLabel(!СеріїНоменклатури_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            СеріїНоменклатури_Objest СеріїНоменклатури_Objest = new СеріїНоменклатури_Objest();
            if (await СеріїНоменклатури_Objest.Read(unigueID))
            {
                СеріїНоменклатури_Objest СеріїНоменклатури_Objest_Новий = await СеріїНоменклатури_Objest.Copy(true);
                await СеріїНоменклатури_Objest_Новий.Save();

                return СеріїНоменклатури_Objest_Новий.UnigueID;
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