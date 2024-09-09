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
    public class Контрагенти : ДовідникЖурнал
    {
        CheckButton checkButtonIsHierarchy = new CheckButton("Ієрархія папок") { Active = true };
        Контрагенти_Папки ДеревоПапок;

        public Контрагенти()
        {
            //Враховувати ієрархію папок
            checkButtonIsHierarchy.Clicked += async (object? sender, EventArgs args) => await LoadRecords();
            HBoxTop.PackStart(checkButtonIsHierarchy, false, false, 10);

            CreateLink(HBoxTop, ДоговориКонтрагентів_Const.FULLNAME, async () =>
            {
                ДоговориКонтрагентів page = new ДоговориКонтрагентів();

                if (SelectPointerItem != null)
                    page.КонтрагентВласник.Pointer = new Контрагенти_Pointer(SelectPointerItem);

                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ДоговориКонтрагентів_Const.FULLNAME}", () => page);

                await page.SetValue();
            });

            //Дерево папок зправа
            ДеревоПапок = new Контрагенти_Папки
            {
                WidthRequest = 500,
                CallBack_RowActivated = LoadRecords_TreeCallBack
            };
            HPanedTable.Pack2(ДеревоПапок, false, true);

            ТабличніСписки.Контрагенти_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            if (DirectoryPointerItem != null || SelectPointerItem != null)
            {
                UnigueID? unigueID = SelectPointerItem != null ? SelectPointerItem : DirectoryPointerItem;

                Контрагенти_Objest? контрагенти_Objest = await new Контрагенти_Pointer(unigueID ?? new UnigueID()).GetDirectoryObject();
                if (контрагенти_Objest != null)
                    ДеревоПапок.SelectPointerItem = контрагенти_Objest.Папка.UnigueID;
            }

            await ДеревоПапок.SetValue();
        }

        async void LoadRecords_TreeCallBack()
        {
            ТабличніСписки.Контрагенти_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Контрагенти_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Контрагенти_Записи.ОчиститиВідбір(TreeViewGrid);

            if (checkButtonIsHierarchy.Active)
                ТабличніСписки.Контрагенти_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(Контрагенти_Const.Папка, Comparison.EQ, ДеревоПапок.SelectPointerItem?.UGuid ?? new UnigueID().UGuid));

            await ТабличніСписки.Контрагенти_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            //Відбори
            ТабличніСписки.Контрагенти_Записи.ДодатиВідбір(TreeViewGrid, Контрагенти_ВідбориДляПошуку.Відбори(searchText), true);

            await ТабличніСписки.Контрагенти_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.Контрагенти_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            Контрагенти_Елемент page = new Контрагенти_Елемент
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
            Контрагенти_Objest Контрагенти_Objest = new Контрагенти_Objest();
            if (await Контрагенти_Objest.Read(unigueID))
                await Контрагенти_Objest.SetDeletionLabel(!Контрагенти_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            Контрагенти_Objest Контрагенти_Objest = new Контрагенти_Objest();
            if (await Контрагенти_Objest.Read(unigueID))
            {
                Контрагенти_Objest Контрагенти_Objest_Новий = await Контрагенти_Objest.Copy(true);
                await Контрагенти_Objest_Новий.Save();
                await Контрагенти_Objest_Новий.Контакти_TablePart.Save(false);
                await Контрагенти_Objest_Новий.Файли_TablePart.Save(false);

                return Контрагенти_Objest_Новий.UnigueID;
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