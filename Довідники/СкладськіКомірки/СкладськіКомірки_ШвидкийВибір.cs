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
    class СкладськіКомірки_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public СкладськіПриміщення_PointerControl СкладПриміщенняВласник = new СкладськіПриміщення_PointerControl();

        public СкладськіКомірки_ШвидкийВибір() : base()
        {
            ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            //Сторінка
            {
                LinkButton linkPage = new LinkButton($" {СкладськіКомірки_Const.FULLNAME}") { Halign = Align.Start, Image = new Image(InterfaceGtk.Іконки.ДляКнопок.Doc), AlwaysShowImage = true };
                linkPage.Clicked += async (object? sender, EventArgs args) =>
                {
                    СкладськіКомірки page = new СкладськіКомірки()
                    {
                        DirectoryPointerItem = DirectoryPointerItem,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer
                    };

                    page.СкладПриміщенняВласник.Pointer = СкладПриміщенняВласник.Pointer;

                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"Вибір - {СкладськіКомірки_Const.FULLNAME}", () => { return page; });

                    await page.LoadRecords();
                };

                HBoxTop.PackStart(linkPage, false, false, 10);
            }

            //Новий
            {
                LinkButton linkNew = new LinkButton("Новий");
                linkNew.Clicked += (object? sender, EventArgs args) =>
                {
                    СкладськіКомірки_Елемент page = new СкладськіКомірки_Елемент
                    {
                        IsNew = true,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer
                    };

                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{СкладськіКомірки_Const.FULLNAME} *", () => { return page; });

                    page.SetValue();
                };

                HBoxTop.PackStart(linkNew, false, false, 0);
            }

            //Власник
            HBoxTop.PackStart(СкладПриміщенняВласник, false, false, 2);
            СкладПриміщенняВласник.Caption = $"{СкладськіПриміщення_Const.FULLNAME}:";
            СкладПриміщенняВласник.AfterSelectFunc = async () =>
            {
                DirectoryPointerItem?.Clear();
                await LoadRecords();
            };
        }

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.ОчиститиВідбір(TreeViewGrid);

            if (!СкладПриміщенняВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіКомірки_Const.Приміщення, Comparison.EQ, СкладПриміщенняВласник.Pointer.UnigueID.UGuid));
            }

            await ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.ОчиститиВідбір(TreeViewGrid);

            if (!СкладПриміщенняВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіКомірки_Const.Приміщення, Comparison.EQ, СкладПриміщенняВласник.Pointer.UnigueID.UGuid));
            }

            //Відбори
            ТабличніСписки.СкладськіКомірки_Записи.ДодатиВідбір(TreeViewGrid, СкладськіКомірки_ВідбориДляПошуку.Відбори(searchText));

            await ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid);
        }
    }
}