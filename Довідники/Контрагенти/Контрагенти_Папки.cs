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
    class Контрагенти_Папки : ДовідникЖурнал
    {
        public Контрагенти_Папки() 
        {
            ТабличніСписки.Контрагенти_Папки_Записи.AddColumns(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.Контрагенти_Папки_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Контрагенти_Папки_Записи.DirectoryPointerItem = DirectoryPointerItem;
            ТабличніСписки.Контрагенти_Папки_Записи.OpenFolder = OpenFolder;

            ТабличніСписки.Контрагенти_Папки_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Контрагенти_Папки_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            //Назва
            ТабличніСписки.Контрагенти_Папки_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(Comparison.OR, Контрагенти_Папки_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.Контрагенти_Папки_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.Контрагенти_Папки_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            Контрагенти_Папки_Елемент page = new Контрагенти_Папки_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                IsNew = IsNew,
                РодичДляНового = new Контрагенти_Папки_Pointer(DirectoryPointerItem ?? new UnigueID())
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

        #region ToolBar

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            Контрагенти_Папки_Objest Контрагенти_Папки_Objest = new Контрагенти_Папки_Objest();
            if (await Контрагенти_Папки_Objest.Read(unigueID))
                await Контрагенти_Папки_Objest.SetDeletionLabel(!Контрагенти_Папки_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            Контрагенти_Папки_Objest Контрагенти_Папки_Objest = new Контрагенти_Папки_Objest();
            if (await Контрагенти_Папки_Objest.Read(unigueID))
            {
                Контрагенти_Папки_Objest Контрагенти_Папки_Objest_Новий = await Контрагенти_Папки_Objest.Copy(true);
                await Контрагенти_Папки_Objest_Новий.Save();

                return Контрагенти_Папки_Objest_Новий.UnigueID;
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