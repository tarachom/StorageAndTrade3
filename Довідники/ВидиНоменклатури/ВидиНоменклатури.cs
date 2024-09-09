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
    public class ВидиНоменклатури : ДовідникЖурнал
    {
        public ВидиНоменклатури() 
        {
            ТабличніСписки.ВидиНоменклатури_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ВидиНоменклатури_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ВидиНоменклатури_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ВидиНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ВидиНоменклатури_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            //Відбори
            ТабличніСписки.ВидиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid, ВидиНоменклатури_ВідбориДляПошуку.Відбори(searchText), true);

            await ТабличніСписки.ВидиНоменклатури_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.ВидиНоменклатури_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            ВидиНоменклатури_Елемент page = new ВидиНоменклатури_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                IsNew = IsNew
            };

            if (IsNew)
                await page.Елемент.New();
            else if (unigueID == null || !await page.Елемент.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return;
            }

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);

            page.SetValue();
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            ВидиНоменклатури_Objest ВидиНоменклатури_Objest = new ВидиНоменклатури_Objest();
            if (await ВидиНоменклатури_Objest.Read(unigueID))
                await ВидиНоменклатури_Objest.SetDeletionLabel(!ВидиНоменклатури_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ВидиНоменклатури_Objest ВидиНоменклатури_Objest = new ВидиНоменклатури_Objest();
            if (await ВидиНоменклатури_Objest.Read(unigueID))
            {
                ВидиНоменклатури_Objest ВидиНоменклатури_Objest_Новий = await ВидиНоменклатури_Objest.Copy(true);
                await ВидиНоменклатури_Objest_Новий.Save();

                return ВидиНоменклатури_Objest_Новий.UnigueID;
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