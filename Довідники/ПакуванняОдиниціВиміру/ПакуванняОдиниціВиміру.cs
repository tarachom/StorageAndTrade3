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

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0.Довідники;

using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class ПакуванняОдиниціВиміру : ДовідникЖурнал
    {
        public ПакуванняОдиниціВиміру() : base()
        {
            ТабличніСписки.ПакуванняОдиниціВиміру_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ПакуванняОдиниціВиміру_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПакуванняОдиниціВиміру_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ПакуванняОдиниціВиміру_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ПакуванняОдиниціВиміру_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ПакуванняОдиниціВиміру_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПакуванняОдиниціВиміру_Записи.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Відбори
            ТабличніСписки.ПакуванняОдиниціВиміру_Записи.ДодатиВідбір(TreeViewGrid, ПакуванняОдиниціВиміру_ВідбориДляПошуку.Відбори(searchText), true);
            
            await ТабличніСписки.ПакуванняОдиниціВиміру_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ПакуванняОдиниціВиміру_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПакуванняОдиниціВиміру_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask<(string Name, Func<Widget>? FuncWidget, System.Action? SetValue)> OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            ПакуванняОдиниціВиміру_Елемент page = new ПакуванняОдиниціВиміру_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                IsNew = IsNew
            };

            if (IsNew)
                await page.ПакуванняОдиниціВиміру_Objest.New();
            else if (unigueID == null || !await page.ПакуванняОдиниціВиміру_Objest.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return ("", null, null);
            }

            return (page.Caption, () => page, page.SetValue);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            ПакуванняОдиниціВиміру_Objest ПакуванняОдиниціВиміру_Objest = new ПакуванняОдиниціВиміру_Objest();
            if (await ПакуванняОдиниціВиміру_Objest.Read(unigueID))
                await ПакуванняОдиниціВиміру_Objest.SetDeletionLabel(!ПакуванняОдиниціВиміру_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ПакуванняОдиниціВиміру_Objest ПакуванняОдиниціВиміру_Objest = new ПакуванняОдиниціВиміру_Objest();
            if (await ПакуванняОдиниціВиміру_Objest.Read(unigueID))
            {
                ПакуванняОдиниціВиміру_Objest ПакуванняОдиниціВиміру_Objest_Новий = await ПакуванняОдиниціВиміру_Objest.Copy(true);
                await ПакуванняОдиниціВиміру_Objest_Новий.Save();

                return ПакуванняОдиниціВиміру_Objest_Новий.UnigueID;
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