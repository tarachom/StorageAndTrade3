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
    class СкладськіПриміщення_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Склади_PointerControl СкладВласник = new Склади_PointerControl();

        public СкладськіПриміщення_ШвидкийВибір() : base()
        {
            ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            //Сторінка
            {
                LinkButton linkPage = new LinkButton($" {СкладськіПриміщення_Const.FULLNAME}") { Halign = Align.Start, Image = new Image(InterfaceGtk.Іконки.ДляКнопок.Doc), AlwaysShowImage = true };
                linkPage.Clicked += async (object? sender, EventArgs args) =>
                {
                    СкладськіПриміщення page = new СкладськіПриміщення()
                    {
                        DirectoryPointerItem = DirectoryPointerItem,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer
                    };

                    page.СкладВласник.Pointer = СкладВласник.Pointer;

                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {СкладськіПриміщення_Const.FULLNAME}", () => { return page; });

                    await page.SetValue();
                };

                HBoxTop.PackStart(linkPage, false, false, 10);
            }

            //Новий
            {
                LinkButton linkNew = new LinkButton("Новий");
                linkNew.Clicked += async (object? sender, EventArgs args) =>
                {
                    СкладськіПриміщення_Елемент page = new СкладськіПриміщення_Елемент
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

            //Власник
            HBoxTop.PackStart(СкладВласник, false, false, 2);
            СкладВласник.Caption = "Склад:";
            СкладВласник.AfterSelectFunc = async () => { await LoadRecords(); };
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.SelectPointerItem = null;
            ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.ОчиститиВідбір(TreeViewGrid);

            if (!СкладВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіПриміщення_Const.Склад, Comparison.EQ, СкладВласник.Pointer.UnigueID.UGuid));
            }

            await ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.СкладськіПриміщення_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!СкладВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.СкладськіПриміщення_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіПриміщення_Const.Склад, Comparison.EQ, СкладВласник.Pointer.UnigueID.UGuid));
            }

            //Відбори
            ТабличніСписки.СкладськіПриміщення_Записи.ДодатиВідбір(TreeViewGrid, СкладськіПриміщення_ВідбориДляПошуку.Відбори(searchText));

            await ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid);
        }
    }
}