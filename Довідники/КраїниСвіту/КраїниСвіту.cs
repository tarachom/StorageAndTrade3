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
    public class КраїниСвіту : ДовідникЖурнал
    {
        public КраїниСвіту() 
        {
            ТабличніСписки.КраїниСвіту_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.КраїниСвіту_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.КраїниСвіту_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.КраїниСвіту_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.КраїниСвіту_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            //Відбори
            ТабличніСписки.КраїниСвіту_Записи.ДодатиВідбір(TreeViewGrid, КраїниСвіту_ВідбориДляПошуку.Відбори(searchText), true);

            await ТабличніСписки.КраїниСвіту_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.КраїниСвіту_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            КраїниСвіту_Елемент page = new КраїниСвіту_Елемент
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
            КраїниСвіту_Objest КраїниСвіту_Objest = new КраїниСвіту_Objest();
            if (await КраїниСвіту_Objest.Read(unigueID))
                await КраїниСвіту_Objest.SetDeletionLabel(!КраїниСвіту_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            КраїниСвіту_Objest КраїниСвіту_Objest = new КраїниСвіту_Objest();
            if (await КраїниСвіту_Objest.Read(unigueID))
            {
                КраїниСвіту_Objest КраїниСвіту_Objest_Новий = await КраїниСвіту_Objest.Copy(true);
                await КраїниСвіту_Objest_Новий.Save();

                return КраїниСвіту_Objest_Новий.UnigueID;
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