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
    public class СтруктураПідприємства : ДовідникЖурнал
    {
        public СтруктураПідприємства() : base()
        {
            ТабличніСписки.СтруктураПідприємства_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.СтруктураПідприємства_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.СтруктураПідприємства_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СтруктураПідприємства_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.СтруктураПідприємства_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            //Відбори
            ТабличніСписки.СтруктураПідприємства_Записи.ДодатиВідбір(TreeViewGrid, СтруктураПідприємства_ВідбориДляПошуку.Відбори(searchText), true);

            await ТабличніСписки.СтруктураПідприємства_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.СтруктураПідприємства_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask<(string Name, Func<Widget>? FuncWidget, System.Action? SetValue)> OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            СтруктураПідприємства_Елемент page = new СтруктураПідприємства_Елемент
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
            СтруктураПідприємства_Objest СтруктураПідприємства_Objest = new СтруктураПідприємства_Objest();
            if (await СтруктураПідприємства_Objest.Read(unigueID))
                await СтруктураПідприємства_Objest.SetDeletionLabel(!СтруктураПідприємства_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            СтруктураПідприємства_Objest СтруктураПідприємства_Objest = new СтруктураПідприємства_Objest();
            if (await СтруктураПідприємства_Objest.Read(unigueID))
            {
                СтруктураПідприємства_Objest СтруктураПідприємства_Objest_Новий = await СтруктураПідприємства_Objest.Copy(true);
                await СтруктураПідприємства_Objest_Новий.Save();

                return СтруктураПідприємства_Objest_Новий.UnigueID;
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