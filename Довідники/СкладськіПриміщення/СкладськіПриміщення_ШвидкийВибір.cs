/*
Copyright (C) 2019-2023 TARAKHOMYN YURIY IVANOVYCH
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
            TreeViewGrid.Model = ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.Store;
            ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            //Сторінка
            {
                LinkButton linkPage = new LinkButton($" {СкладськіПриміщення_Const.FULLNAME}") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
                linkPage.Clicked += (object? sender, EventArgs args) =>
                {
                    СкладськіПриміщення page = new СкладськіПриміщення()
                    {
                        DirectoryPointerItem = DirectoryPointerItem,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer
                    };

                    page.СкладВласник.Pointer = СкладВласник.Pointer;

                    Program.GeneralForm?.CreateNotebookPage($"Вибір - {СкладськіПриміщення_Const.FULLNAME}", () => { return page; }, true);

                    page.LoadRecords();
                };

                HBoxTop.PackStart(linkPage, false, false, 10);
            }

            //Новий
            {
                LinkButton linkNew = new LinkButton("Новий");
                linkNew.Clicked += (object? sender, EventArgs args) =>
                {
                    СкладськіПриміщення_Елемент page = new СкладськіПриміщення_Елемент
                    {
                        IsNew = true,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer
                    };

                    Program.GeneralForm?.CreateNotebookPage($"{СкладськіПриміщення_Const.FULLNAME} *", () => { return page; }, true);

                    page.SetValue();
                };

                HBoxTop.PackStart(linkNew, false, false, 0);
            }

            //Власник
            HBoxTop.PackStart(СкладВласник, false, false, 2);
            СкладВласник.Caption = "Склад:";
            СкладВласник.AfterSelectFunc = () =>
            {
                //DirectoryPointerItem?.Clear();
                LoadRecords();
            };

            //Очистка
            /*
            {
                LinkButton linkClear = new LinkButton(" Очистити") { Image = new Image(AppContext.BaseDirectory + "images/clean.png"), AlwaysShowImage = true };
                linkClear.Clicked += (object? sender, EventArgs args) =>
                {
                    if (CallBack_OnSelectPointer != null)
                        CallBack_OnSelectPointer.Invoke(new UnigueID());

                    if (PopoverParent != null)
                        PopoverParent.Hide();
                };

                HBoxTop.PackEnd(linkClear, false, false, 10);
            }
            */
        }

        public override void LoadRecords()
        {
            ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.Where.Clear();

            if (!СкладВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.Where.Add(
                    new Where(СкладськіПриміщення_Const.Склад, Comparison.EQ, СкладВласник.Pointer.UnigueID.UGuid));
            }

            ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.LoadRecords();

            if (ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.Where.Clear();

            if (!СкладВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.Where.Add(
                    new Where(СкладськіПриміщення_Const.Склад, Comparison.EQ, СкладВласник.Pointer.UnigueID.UGuid));
            }

            //Назва
            ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.Where.Add(
                new Where(Comparison.AND, СкладськіПриміщення_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.LoadRecords();
        }
    }
}