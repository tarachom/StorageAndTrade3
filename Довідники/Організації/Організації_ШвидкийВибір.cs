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
    class Організації_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Організації_ШвидкийВибір() 
        {
            ТабличніСписки.Організації_Записи.AddColumns(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.Організації_Записи.SelectPointerItem = null;
            ТабличніСписки.Організації_Записи.DirectoryPointerItem = DirectoryPointerItem;
            ТабличніСписки.Організації_Записи.OpenFolder = OpenFolder;

            ТабличніСписки.Організації_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Організації_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            //Відбори
            ТабличніСписки.Організації_Записи.ДодатиВідбір(TreeViewGrid, Організації_ВідбориДляПошуку.Відбори(searchText), true);

            await ТабличніСписки.Організації_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            Організації page = new Організації()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {Організації_Const.FULLNAME}", () => page);

            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            Організації_Елемент page = new Організації_Елемент
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
            Організації_Objest Обєкт = new Організації_Objest();
            if (await Обєкт.Read(unigueID))
                await Обєкт.SetDeletionLabel(!Обєкт.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }
    }
}