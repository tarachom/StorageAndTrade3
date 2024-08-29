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
    class Склади_Папки_Дерево_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public UnigueID? OpenFolder { get; set; }

        public Склади_Папки_Дерево_ШвидкийВибір() : base(false)
        {
            ТабличніСписки.Склади_Папки_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            //Сторінка
            {
                LinkButton linkPage = new LinkButton($" {Склади_Папки_Const.FULLNAME}") { Halign = Align.Start, Image = new Image(InterfaceGtk.Іконки.ДляКнопок.Doc), AlwaysShowImage = true };
                linkPage.Clicked += (object? sender, EventArgs args) =>
                {
                    Склади_Папки_Дерево page = new Склади_Папки_Дерево()
                    {
                        DirectoryPointerItem = DirectoryPointerItem,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                        OpenFolder = OpenFolder
                    };

                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {Склади_Папки_Const.FULLNAME}", () => { return page; });

                    page.LoadTree();
                };

                HBoxTop.PackStart(linkPage, false, false, 10);
            }

            //Новий
            {
                LinkButton linkNew = new LinkButton("Новий");
                linkNew.Clicked += async (object? sender, EventArgs args) =>
                {
                    Склади_Папки_Елемент page = new Склади_Папки_Елемент
                    {
                        IsNew = true,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer
                    };

                    await page.Елемент.New();

                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => { return page; });

                    page.SetValue();
                };

                HBoxTop.PackStart(linkNew, false, false, 0);
            }
        }

        public async void LoadTree()
        {
            await ТабличніСписки.Склади_Папки_ЗаписиШвидкийВибір.LoadTree(TreeViewGrid, OpenFolder, DirectoryPointerItem);

            TreeViewGrid.ExpandToPath(ТабличніСписки.Склади_Папки_ЗаписиШвидкийВибір.RootPath);
            TreeViewGrid.SetCursor(ТабличніСписки.Склади_Папки_ЗаписиШвидкийВибір.RootPath, TreeViewGrid.Columns[0], false);

            if (ТабличніСписки.Склади_Папки_ЗаписиШвидкийВибір.SelectPath != null)
            {
                TreeViewGrid.ExpandToPath(ТабличніСписки.Склади_Папки_ЗаписиШвидкийВибір.SelectPath);
                TreeViewGrid.SetCursor(ТабличніСписки.Склади_Папки_ЗаписиШвидкийВибір.SelectPath, TreeViewGrid.Columns[0], false);
            }
        }
    }
}