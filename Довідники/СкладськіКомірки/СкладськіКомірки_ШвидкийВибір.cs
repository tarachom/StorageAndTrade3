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
    class СкладськіКомірки_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public СкладськіПриміщення_PointerControl СкладПриміщенняВласник = new СкладськіПриміщення_PointerControl();

        public СкладськіКомірки_ШвидкийВибір() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.Store;
            ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            //Сторінка
            {
                LinkButton linkPage = new LinkButton($" {СкладськіКомірки_Const.FULLNAME}") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
                linkPage.Clicked += (object? sender, EventArgs args) =>
                {
                    СкладськіКомірки page = new СкладськіКомірки()
                    {
                        DirectoryPointerItem = DirectoryPointerItem,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer
                    };

                    page.СкладПриміщенняВласник.Pointer = СкладПриміщенняВласник.Pointer;

                    Program.GeneralForm?.CreateNotebookPage($"Вибір - {СкладськіКомірки_Const.FULLNAME}", () => { return page; }, true);

                    page.LoadRecords();
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

                    Program.GeneralForm?.CreateNotebookPage($"{СкладськіКомірки_Const.FULLNAME} *", () => { return page; }, true);

                    page.SetValue();
                };

                HBoxTop.PackStart(linkNew, false, false, 0);
            }

            //Власник
            HBoxTop.PackStart(СкладПриміщенняВласник, false, false, 2);
            СкладПриміщенняВласник.Caption = $"{СкладськіПриміщення_Const.FULLNAME}:";
            СкладПриміщенняВласник.AfterSelectFunc = () =>
            {
                DirectoryPointerItem?.Clear();
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
            ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.Where.Clear();

            if (!СкладПриміщенняВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.Where.Add(
                    new Where(СкладськіКомірки_Const.Приміщення, Comparison.EQ, СкладПриміщенняВласник.Pointer.UnigueID.UGuid));
            }

            ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.LoadRecords();

            if (ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.Where.Clear();

            if (!СкладПриміщенняВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.Where.Add(
                    new Where(СкладськіКомірки_Const.Приміщення, Comparison.EQ, СкладПриміщенняВласник.Pointer.UnigueID.UGuid));
            }

            //Назва
            ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.Where.Add(
                new Where(СкладськіКомірки_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.LoadRecords();
        }
    }
}