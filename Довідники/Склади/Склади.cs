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
    public class Склади : ДовідникЖурнал
    {
        CheckButton checkButtonIsHierarchy = new CheckButton("Ієрархія папок") { Active = true };
        Склади_Папки ДеревоПапок;

        public Склади()
        {
            //Враховувати ієрархію папок
            checkButtonIsHierarchy.Clicked += async (object? sender, EventArgs args) => await LoadRecords();
            HBoxTop.PackStart(checkButtonIsHierarchy, false, false, 10);

            CreateLink(HBoxTop, СкладськіПриміщення_Const.FULLNAME, async () =>
            {
                СкладськіПриміщення page = new СкладськіПриміщення();

                if (SelectPointerItem != null)
                    page.СкладВласник.Pointer = new Склади_Pointer(SelectPointerItem);

                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{СкладськіПриміщення_Const.FULLNAME}", () => page);

                await page.SetValue();
            });

            //Дерево папок зправа
            ДеревоПапок = new Склади_Папки
            {
                WidthRequest = 500,
                CallBack_RowActivated = LoadRecords_TreeCallBack
            };
            HPanedTable.Pack2(ДеревоПапок, false, true);

            ТабличніСписки.Склади_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            if (DirectoryPointerItem != null || SelectPointerItem != null)
            {
                UnigueID? unigueID = SelectPointerItem != null ? SelectPointerItem : DirectoryPointerItem;

                Склади_Objest? контрагенти_Objest = await new Склади_Pointer(unigueID ?? new UnigueID()).GetDirectoryObject();
                if (контрагенти_Objest != null)
                    ДеревоПапок.SelectPointerItem = контрагенти_Objest.Папка.UnigueID;
            }

            await ДеревоПапок.SetValue();
        }

        async void LoadRecords_TreeCallBack()
        {
            ТабличніСписки.Склади_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Склади_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Склади_Записи.ОчиститиВідбір(TreeViewGrid);

            if (checkButtonIsHierarchy.Active)
                ТабличніСписки.Склади_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(Склади_Const.Папка, Comparison.EQ, ДеревоПапок.SelectPointerItem?.UGuid ?? new UnigueID().UGuid));

            await ТабличніСписки.Склади_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            //Відбори
            ТабличніСписки.Склади_Записи.ДодатиВідбір(TreeViewGrid, Склади_ВідбориДляПошуку.Відбори(searchText), true);

            await ТабличніСписки.Склади_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.Склади_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            Склади_Елемент page = new Склади_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                IsNew = IsNew
            };

            if (IsNew)
                await page.Елемент.New();
            else if (unigueID == null || !await page.Елемент.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return;
            }

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);

            page.SetValue();
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            Склади_Objest Склади_Objest = new Склади_Objest();
            if (await Склади_Objest.Read(unigueID))
                await Склади_Objest.SetDeletionLabel(!Склади_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            Склади_Objest Склади_Objest = new Склади_Objest();
            if (await Склади_Objest.Read(unigueID))
            {
                Склади_Objest Склади_Objest_Новий = await Склади_Objest.Copy(true);
                await Склади_Objest_Новий.Save();
                await Склади_Objest_Новий.Контакти_TablePart.Save(false);

                return Склади_Objest_Новий.UnigueID;
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