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

using StorageAndTrade_1_0.Довідники;

using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class БанківськіРахункиОрганізацій_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public БанківськіРахункиОрганізацій_ШвидкийВибір() : base()
        {
            ТабличніСписки.БанківськіРахункиОрганізацій_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            //Сторінка
            {
                LinkButton linkPage = new LinkButton($" {БанківськіРахункиОрганізацій_Const.FULLNAME}") { Halign = Align.Start, Image = new Image(InterfaceGtk.Іконки.ДляКнопок.Doc), AlwaysShowImage = true };
                linkPage.Clicked += async (object? sender, EventArgs args) =>
                {
                    БанківськіРахункиОрганізацій page = new БанківськіРахункиОрганізацій()
                    {
                        DirectoryPointerItem = DirectoryPointerItem,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer
                    };

                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {БанківськіРахункиОрганізацій_Const.FULLNAME}", () => { return page; });

                    await page.SetValue();
                };

                HBoxTop.PackStart(linkPage, false, false, 10);
            }

            //Новий
            {
                LinkButton linkNew = new LinkButton("Новий");
                linkNew.Clicked += async (object? sender, EventArgs args) =>
                {
                    БанківськіРахункиОрганізацій_Елемент page = new БанківськіРахункиОрганізацій_Елемент
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

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.БанківськіРахункиОрганізацій_ЗаписиШвидкийВибір.SelectPointerItem = null;
            ТабличніСписки.БанківськіРахункиОрганізацій_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.БанківськіРахункиОрганізацій_ЗаписиШвидкийВибір.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.БанківськіРахункиОрганізацій_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            //Відбори
            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.ДодатиВідбір(TreeViewGrid, БанківськіРахункиОрганізацій_ВідбориДляПошуку.Відбори(searchText), true);

            await ТабличніСписки.БанківськіРахункиОрганізацій_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid);
        }
    }
}