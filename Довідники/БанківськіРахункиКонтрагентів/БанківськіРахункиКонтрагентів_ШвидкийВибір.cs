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
    class БанківськіРахункиКонтрагентів_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public БанківськіРахункиКонтрагентів_ШвидкийВибір() 
        {
            ТабличніСписки.БанківськіРахункиКонтрагентів_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.БанківськіРахункиКонтрагентів_ЗаписиШвидкийВибір.SelectPointerItem = null;
            ТабличніСписки.БанківськіРахункиКонтрагентів_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.БанківськіРахункиКонтрагентів_ЗаписиШвидкийВибір.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.БанківськіРахункиКонтрагентів_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            //Відбори
            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.ДодатиВідбір(TreeViewGrid, БанківськіРахункиКонтрагентів_ВідбориДляПошуку.Відбори(searchText), true);

            await ТабличніСписки.БанківськіРахункиКонтрагентів_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            БанківськіРахункиКонтрагентів page = new БанківськіРахункиКонтрагентів()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {БанківськіРахункиКонтрагентів_Const.FULLNAME}", () => page);

            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            БанківськіРахункиКонтрагентів_Елемент page = new БанківськіРахункиКонтрагентів_Елемент
            {
                IsNew = IsNew,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer
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
            БанківськіРахункиКонтрагентів_Objest Обєкт = new БанківськіРахункиКонтрагентів_Objest();
            if (await Обєкт.Read(unigueID))
                await Обєкт.SetDeletionLabel(!Обєкт.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }
    }
}