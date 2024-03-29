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
    public class БанківськіРахункиКонтрагентів : ДовідникЖурнал
    {
        public БанківськіРахункиКонтрагентів() : base()
        {
            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Відбори
            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.ДодатиВідбір(TreeViewGrid, БанківськіРахункиКонтрагентів_ВідбориДляПошуку.Відбори(searchText), true);

            await ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{БанківськіРахункиКонтрагентів_Const.FULLNAME} *", () =>
                {
                    БанківськіРахункиКонтрагентів_Елемент page = new БанківськіРахункиКонтрагентів_Елемент
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
                БанківськіРахункиКонтрагентів_Objest БанківськіРахункиКонтрагентів_Objest = new БанківськіРахункиКонтрагентів_Objest();
                if (await БанківськіРахункиКонтрагентів_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{БанківськіРахункиКонтрагентів_Objest.Назва}", () =>
                    {
                        БанківськіРахункиКонтрагентів_Елемент page = new БанківськіРахункиКонтрагентів_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            БанківськіРахункиКонтрагентів_Objest = БанківськіРахункиКонтрагентів_Objest,
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
            БанківськіРахункиКонтрагентів_Objest БанківськіРахункиКонтрагентів_Objest = new БанківськіРахункиКонтрагентів_Objest();
            if (await БанківськіРахункиКонтрагентів_Objest.Read(unigueID))
                await БанківськіРахункиКонтрагентів_Objest.SetDeletionLabel(!БанківськіРахункиКонтрагентів_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            БанківськіРахункиКонтрагентів_Objest БанківськіРахункиКонтрагентів_Objest = new БанківськіРахункиКонтрагентів_Objest();
            if (await БанківськіРахункиКонтрагентів_Objest.Read(unigueID))
            {
                БанківськіРахункиКонтрагентів_Objest БанківськіРахункиКонтрагентів_Objest_Новий = await БанківськіРахункиКонтрагентів_Objest.Copy(true);
                await БанківськіРахункиКонтрагентів_Objest_Новий.Save();

                return БанківськіРахункиКонтрагентів_Objest_Новий.UnigueID;
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