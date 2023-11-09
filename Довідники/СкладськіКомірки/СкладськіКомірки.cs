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
    public class СкладськіКомірки : ДовідникЖурнал
    {
        CheckButton checkButtonIsHierarchy = new CheckButton("Ієрархія папок") { Active = true };
        СкладськіКомірки_Папки_Дерево ДеревоПапок;
        public СкладськіПриміщення_PointerControl СкладПриміщенняВласник = new СкладськіПриміщення_PointerControl();

        public СкладськіКомірки() : base()
        {
            //Враховувати ієрархію папок
            checkButtonIsHierarchy.Clicked += OnCheckButtonIsHierarchyClicked;
            HBoxTop.PackStart(checkButtonIsHierarchy, false, false, 10);

            //Власник
            HBoxTop.PackStart(СкладПриміщенняВласник, false, false, 2);
            СкладПриміщенняВласник.Caption = "Приміщення:";
            СкладПриміщенняВласник.AfterSelectFunc = () =>
            {
                LoadRecords();
            };

            //Дерево папок зправа
            ДеревоПапок = new СкладськіКомірки_Папки_Дерево
            {
                WidthRequest = 500,
                CallBack_RowActivated = LoadRecords_TreeCallBack
            };
            HPanedTable.Pack2(ДеревоПапок, false, true);

            TreeViewGrid.Model = ТабличніСписки.СкладськіКомірки_Записи.Store;
            ТабличніСписки.СкладськіКомірки_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            if (DirectoryPointerItem != null || SelectPointerItem != null)
            {
                string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                UnigueID unigueID = new UnigueID(UidSelect);

                СкладськіКомірки_Objest? контрагенти_Objest = new СкладськіКомірки_Pointer(unigueID).GetDirectoryObject();
                if (контрагенти_Objest != null)
                    ДеревоПапок.DirectoryPointerItem = контрагенти_Objest.Папка.UnigueID;
            }

            ДеревоПапок.СкладПриміщенняВласник = СкладПриміщенняВласник.Pointer;
            ДеревоПапок.LoadTree();
        }

        void LoadRecords_TreeCallBack()
        {
            ТабличніСписки.СкладськіКомірки_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.СкладськіКомірки_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СкладськіКомірки_Записи.Where.Clear();

            if (checkButtonIsHierarchy.Active)
                ТабличніСписки.СкладськіКомірки_Записи.Where.Add(new Where(СкладськіКомірки_Const.Папка, Comparison.EQ,
                    ДеревоПапок.DirectoryPointerItem?.UGuid ?? new UnigueID().UGuid));

            if (!СкладПриміщенняВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.СкладськіКомірки_Записи.Where.Add(
                    new Where(Comparison.AND, СкладськіКомірки_Const.Приміщення, Comparison.EQ, СкладПриміщенняВласник.Pointer.UnigueID.UGuid));
            }

            ТабличніСписки.СкладськіКомірки_Записи.LoadRecords();

            if (ТабличніСписки.СкладськіКомірки_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.СкладськіКомірки_Записи.SelectPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.СкладськіКомірки_Записи.Where.Clear();

            //Назва
            ТабличніСписки.СкладськіКомірки_Записи.Where.Add(
                new Where(Comparison.OR, СкладськіКомірки_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.СкладськіКомірки_Записи.LoadRecords();

            if (ТабличніСписки.СкладськіКомірки_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.СкладськіКомірки_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{СкладськіКомірки_Const.FULLNAME} *", () =>
                {
                    СкладськіКомірки_Елемент page = new СкладськіКомірки_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadRecords,
                        IsNew = true,
                        РодичДляНового = new СкладськіКомірки_Папки_Pointer(ДеревоПапок.DirectoryPointerItem ?? new UnigueID()),
                        СкладськеПриміщенняДляНового = СкладПриміщенняВласник.Pointer
                    };

                    page.SetValue();

                    return page;
                }, true);
            }
            else if (unigueID != null)
            {
                СкладськіКомірки_Objest СкладськіКомірки_Objest = new СкладськіКомірки_Objest();
                if (СкладськіКомірки_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{СкладськіКомірки_Objest.Назва}", () =>
                    {
                        СкладськіКомірки_Елемент page = new СкладськіКомірки_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            СкладськіКомірки_Objest = СкладськіКомірки_Objest,
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
            СкладськіКомірки_Objest СкладськіКомірки_Objest = new СкладськіКомірки_Objest();
            if (СкладськіКомірки_Objest.Read(unigueID))
                СкладськіКомірки_Objest.SetDeletionLabel(!СкладськіКомірки_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override UnigueID? Copy(UnigueID unigueID)
        {
            СкладськіКомірки_Objest СкладськіКомірки_Objest = new СкладськіКомірки_Objest();
            if (СкладськіКомірки_Objest.Read(unigueID))
            {
                СкладськіКомірки_Objest СкладськіКомірки_Objest_Новий = СкладськіКомірки_Objest.Copy(true);
                СкладськіКомірки_Objest_Новий.Save();

                return СкладськіКомірки_Objest_Новий.UnigueID;
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