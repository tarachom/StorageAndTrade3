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
    public class ВидиЗапасів : ДовідникЖурнал
    {
        public ВидиЗапасів() : base()
        {
            ТабличніСписки.ВидиЗапасів_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ВидиЗапасів_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ВидиЗапасів_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ВидиЗапасів_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ВидиЗапасів_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ВидиЗапасів_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВидиЗапасів_Записи.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Відбори
            ТабличніСписки.ВидиЗапасів_Записи.ДодатиВідбір(TreeViewGrid, ВидиЗапасів_ВідбориДляПошуку.Відбори(searchText), true);

            await ТабличніСписки.ВидиЗапасів_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ВидиЗапасів_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВидиЗапасів_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.ВидиЗапасів_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask<(string Name, Func<Widget>? FuncWidget, System.Action? SetValue)> OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            ВидиЗапасів_Елемент page = new ВидиЗапасів_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                IsNew = IsNew
            };

            if (IsNew)
                await page.Елемент.New();
            else if (unigueID == null || !await page.Елемент.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return ("", null, null);
            }

            return (page.Caption, () => page, page.SetValue);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            ВидиЗапасів_Objest ВидиЗапасів_Objest = new ВидиЗапасів_Objest();
            if (await ВидиЗапасів_Objest.Read(unigueID))
                await ВидиЗапасів_Objest.SetDeletionLabel(!ВидиЗапасів_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ВидиЗапасів_Objest ВидиЗапасів_Objest = new ВидиЗапасів_Objest();
            if (await ВидиЗапасів_Objest.Read(unigueID))
            {
                ВидиЗапасів_Objest ВидиЗапасів_Objest_Новий = await ВидиЗапасів_Objest.Copy(true);
                await ВидиЗапасів_Objest_Новий.Save();

                return ВидиЗапасів_Objest_Новий.UnigueID;
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