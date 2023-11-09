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
    public class СкладськіПриміщення : ДовідникЖурнал
    {
        public Склади_PointerControl СкладВласник = new Склади_PointerControl();

        public СкладськіПриміщення() : base()
        {
            //Власник
            HBoxTop.PackStart(СкладВласник, false, false, 2);
            СкладВласник.Caption = "Склад:";
            СкладВласник.AfterSelectFunc = LoadRecords;

            //Складські комірки
            LinkButton linkButtonHar = new LinkButton($" {СкладськіКомірки_Const.FULLNAME}") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
            linkButtonHar.Clicked += (object? sender, EventArgs args) =>
            {
                СкладськіКомірки page = new СкладськіКомірки();

                if (SelectPointerItem != null)
                    page.СкладПриміщенняВласник.Pointer = new СкладськіПриміщення_Pointer(SelectPointerItem);

                Program.GeneralForm?.CreateNotebookPage($"{СкладськіКомірки_Const.FULLNAME}", () => { return page; });

                page.LoadRecords();
            };

            HBoxTop.PackStart(linkButtonHar, false, false, 10);

            TreeViewGrid.Model = ТабличніСписки.СкладськіПриміщення_Записи.Store;
            ТабличніСписки.СкладськіПриміщення_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.СкладськіПриміщення_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.СкладськіПриміщення_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СкладськіПриміщення_Записи.Where.Clear();

            if (!СкладВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.СкладськіПриміщення_Записи.Where.Add(
                    new Where(СкладськіПриміщення_Const.Склад, Comparison.EQ, СкладВласник.Pointer.UnigueID.UGuid));
            }

            ТабличніСписки.СкладськіПриміщення_Записи.LoadRecords();

            if (ТабличніСписки.СкладськіПриміщення_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.СкладськіПриміщення_Записи.SelectPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.СкладськіПриміщення_Записи.Where.Clear();

            //Назва
            ТабличніСписки.СкладськіПриміщення_Записи.Where.Add(
                new Where(СкладськіПриміщення_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.СкладськіПриміщення_Записи.LoadRecords();

            if (ТабличніСписки.СкладськіПриміщення_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.СкладськіПриміщення_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{СкладськіПриміщення_Const.FULLNAME} *", () =>
                {
                    СкладськіПриміщення_Елемент page = new СкладськіПриміщення_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadRecords,
                        IsNew = true
                    };

                    page.SetValue();

                    return page;
                }, true);
            }
            else if (unigueID != null)
            {
                СкладськіПриміщення_Objest СкладськіПриміщення_Objest = new СкладськіПриміщення_Objest();
                if (СкладськіПриміщення_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{СкладськіПриміщення_Objest.Назва}", () =>
                    {
                        СкладськіПриміщення_Елемент page = new СкладськіПриміщення_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            СкладськіПриміщення_Objest = СкладськіПриміщення_Objest,
                        };

                        page.SetValue();

                        return page;
                    }, true);
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }

        protected override void SetDeletionLabel(UnigueID unigueID)
        {
            СкладськіПриміщення_Objest СкладськіПриміщення_Objest = new СкладськіПриміщення_Objest();
            if (СкладськіПриміщення_Objest.Read(unigueID))
                СкладськіПриміщення_Objest.SetDeletionLabel(!СкладськіПриміщення_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override UnigueID? Copy(UnigueID unigueID)
        {
            СкладськіПриміщення_Objest СкладськіПриміщення_Objest = new СкладськіПриміщення_Objest();
            if (СкладськіПриміщення_Objest.Read(unigueID))
            {
                СкладськіПриміщення_Objest СкладськіПриміщення_Objest_Новий = СкладськіПриміщення_Objest.Copy(true);
                СкладськіПриміщення_Objest_Новий.Save();

                return СкладськіПриміщення_Objest_Новий.UnigueID;
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