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
    public class СкладськіКомірки : ДовідникЖурнал
    {
        CheckButton checkButtonIsHierarchy = new CheckButton("Ієрархія папок") { Active = true };
        СкладськіКомірки_Папки ДеревоПапок;
        public СкладськіПриміщення_PointerControl СкладПриміщенняВласник = new СкладськіПриміщення_PointerControl() { Caption = "Приміщення:" };

        public СкладськіКомірки() : base()
        {
            //Враховувати ієрархію папок
            checkButtonIsHierarchy.Clicked += OnCheckButtonIsHierarchyClicked;
            HBoxTop.PackStart(checkButtonIsHierarchy, false, false, 10);

            //Власник
            HBoxTop.PackStart(СкладПриміщенняВласник, false, false, 2);
            СкладПриміщенняВласник.AfterSelectFunc = async () => await LoadRecords();

            //Дерево папок зправа
            ДеревоПапок = new СкладськіКомірки_Папки
            {
                WidthRequest = 500,
                CallBack_RowActivated = LoadRecords_TreeCallBack,
                ParentWidget = this
            };
            HPanedTable.Pack2(ДеревоПапок, false, true);

            ТабличніСписки.СкладськіКомірки_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            if (DirectoryPointerItem != null || SelectPointerItem != null)
            {
                string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                UnigueID unigueID = new UnigueID(UidSelect);

                СкладськіКомірки_Objest? контрагенти_Objest = await new СкладськіКомірки_Pointer(unigueID).GetDirectoryObject();
                if (контрагенти_Objest != null)
                    ДеревоПапок.DirectoryPointerItem = контрагенти_Objest.Папка.UnigueID;
            }

            ДеревоПапок.СкладПриміщенняВласник = СкладПриміщенняВласник.Pointer;
            await ДеревоПапок.SetValue();
        }

        async void LoadRecords_TreeCallBack()
        {
            ТабличніСписки.СкладськіКомірки_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.СкладськіКомірки_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СкладськіКомірки_Записи.ОчиститиВідбір(TreeViewGrid);

            if (checkButtonIsHierarchy.Active)
                ТабличніСписки.СкладськіКомірки_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіКомірки_Const.Папка, Comparison.EQ, ДеревоПапок.DirectoryPointerItem?.UGuid ?? new UnigueID().UGuid));

            if (!СкладПриміщенняВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.СкладськіКомірки_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіКомірки_Const.Приміщення, Comparison.EQ, СкладПриміщенняВласник.Pointer.UnigueID.UGuid));
            }

            await ТабличніСписки.СкладськіКомірки_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.СкладськіКомірки_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.СкладськіКомірки_Записи.SelectPath, TreeViewGrid.Columns[0], false);
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

            await ТабличніСписки.СкладськіКомірки_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.СкладськіКомірки_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.СкладськіКомірки_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.СкладськіКомірки_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask<(string Name, Func<Widget>? FuncWidget, System.Action? SetValue)> OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            СкладськіКомірки_Елемент page = new СкладськіКомірки_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                IsNew = IsNew
            };

            if (IsNew)
                await page.Елемент.New();
            else if (unigueID == null || !await page.Елемент.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return ("", null, null);
            }

            return (page.Caption, () => page, page.SetValue);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            СкладськіКомірки_Objest СкладськіКомірки_Objest = new СкладськіКомірки_Objest();
            if (await СкладськіКомірки_Objest.Read(unigueID))
                await СкладськіКомірки_Objest.SetDeletionLabel(!СкладськіКомірки_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            СкладськіКомірки_Objest СкладськіКомірки_Objest = new СкладськіКомірки_Objest();
            if (await СкладськіКомірки_Objest.Read(unigueID))
            {
                СкладськіКомірки_Objest СкладськіКомірки_Objest_Новий = await СкладськіКомірки_Objest.Copy(true);
                await СкладськіКомірки_Objest_Новий.Save();

                return СкладськіКомірки_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        #endregion

        async void OnCheckButtonIsHierarchyClicked(object? sender, EventArgs args)
        {
            await LoadRecords();
        }
    }
}