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
    public class Склади : ДовідникЖурнал
    {
        CheckButton checkButtonIsHierarchy = new CheckButton("Ієрархія папок") { Active = true };
        Склади_Папки_Дерево ДеревоПапок;

        public Склади() : base()
        {
            //Враховувати ієрархію папок
            checkButtonIsHierarchy.Clicked += OnCheckButtonIsHierarchyClicked;
            HBoxTop.PackStart(checkButtonIsHierarchy, false, false, 10);

            //Склади приміщення
            LinkButton linkButtonHar = new LinkButton($" {СкладськіПриміщення_Const.FULLNAME}") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
            linkButtonHar.Clicked += (object? sender, EventArgs args) =>
            {
                СкладськіПриміщення page = new СкладськіПриміщення();

                if (SelectPointerItem != null)
                    page.СкладВласник.Pointer = new Склади_Pointer(SelectPointerItem);

                Program.GeneralForm?.CreateNotebookPage($"{СкладськіПриміщення_Const.FULLNAME}", () => { return page; });

                page.LoadRecords();
            };

            HBoxTop.PackStart(linkButtonHar, false, false, 10);

            //Дерево папок зправа
            ДеревоПапок = new Склади_Папки_Дерево
            {
                WidthRequest = 500,
                CallBack_RowActivated = LoadRecords_TreeCallBack
            };
            HPanedTable.Pack2(ДеревоПапок, false, true);

            TreeViewGrid.Model = ТабличніСписки.Склади_Записи.Store;
            ТабличніСписки.Склади_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            if (DirectoryPointerItem != null || SelectPointerItem != null)
            {
                UnigueID? unigueID = SelectPointerItem != null ? SelectPointerItem : DirectoryPointerItem;

                Склади_Objest? контрагенти_Objest = new Склади_Pointer(unigueID ?? new UnigueID()).GetDirectoryObject();
                if (контрагенти_Objest != null)
                    ДеревоПапок.DirectoryPointerItem = контрагенти_Objest.Папка.UnigueID;
            }

            ДеревоПапок.LoadTree();
        }

        void LoadRecords_TreeCallBack()
        {
            ТабличніСписки.Склади_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Склади_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Склади_Записи.Where.Clear();

            if (checkButtonIsHierarchy.Active)
                ТабличніСписки.Склади_Записи.Where.Add(new Where(Склади_Const.Папка, Comparison.EQ,
                    ДеревоПапок.DirectoryPointerItem?.UGuid ?? new UnigueID().UGuid));

            ТабличніСписки.Склади_Записи.LoadRecords();

            if (ТабличніСписки.Склади_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Склади_Записи.SelectPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.Склади_Записи.Where.Clear();

            //Код
            ТабличніСписки.Склади_Записи.Where.Add(
                new Where(Склади_Const.Код, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            //Назва
            ТабличніСписки.Склади_Записи.Where.Add(
                new Where(Comparison.OR, Склади_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.Склади_Записи.LoadRecords();

            if (ТабличніСписки.Склади_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Склади_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{Склади_Const.FULLNAME} *", () =>
                {
                    Склади_Елемент page = new Склади_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadRecords,
                        IsNew = true,
                        РодичДляНового = new Склади_Папки_Pointer(ДеревоПапок.DirectoryPointerItem ?? new UnigueID())
                    };

                    page.SetValue();

                    return page;
                }, true);
            }
            else if (unigueID != null)
            {
                Склади_Objest Склади_Objest = new Склади_Objest();
                if (Склади_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{Склади_Objest.Назва}", () =>
                    {
                        Склади_Елемент page = new Склади_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            Склади_Objest = Склади_Objest,
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
            Склади_Objest Склади_Objest = new Склади_Objest();
            if (Склади_Objest.Read(unigueID))
                Склади_Objest.SetDeletionLabel(!Склади_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override UnigueID? Copy(UnigueID unigueID)
        {
            Склади_Objest Склади_Objest = new Склади_Objest();
            if (Склади_Objest.Read(unigueID))
            {
                Склади_Objest Склади_Objest_Новий = Склади_Objest.Copy(true);
                Склади_Objest_Новий.Save();
                Склади_Objest_Новий.Контакти_TablePart.Save(false);

                return Склади_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        #endregion

        void OnCheckButtonIsHierarchyClicked(object? sender, EventArgs args)
        {
            LoadRecords();
        }
    }
}