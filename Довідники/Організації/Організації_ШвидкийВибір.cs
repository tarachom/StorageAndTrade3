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
        public Організації_ШвидкийВибір() : base()
        {
            ТабличніСписки.Організації_Записи.AddColumns(TreeViewGrid);

            //Сторінка
            {
                LinkButton linkPage = new LinkButton($" {Організації_Const.FULLNAME}") { Halign = Align.Start, Image = new Image(InterfaceGtk.Іконки.ДляКнопок.Doc), AlwaysShowImage = true };
                linkPage.Clicked += async (object? sender, EventArgs args) =>
                {
                    Організації page = new Організації()
                    {
                        DirectoryPointerItem = DirectoryPointerItem,
                        OpenFolder = OpenFolder,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer
                    };

                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {Організації_Const.FULLNAME}", () => page);

                    await page.SetValue();
                };

                HBoxTop.PackStart(linkPage, false, false, 10);
            }

            //Новий
            {
                LinkButton linkNew = new LinkButton("Новий");
                linkNew.Clicked += async (object? sender, EventArgs args) =>
                {
                    Організації_Елемент page = new Організації_Елемент
                    {
                        IsNew = true,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer
                    };

                    await page.Елемент.New();

                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);

                    page.SetValue();
                };

                HBoxTop.PackStart(linkNew, false, false, 0);
            }
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
    }
}