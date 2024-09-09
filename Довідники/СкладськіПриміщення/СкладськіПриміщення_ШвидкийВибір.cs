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
    class СкладськіПриміщення_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Склади_PointerControl СкладВласник = new Склади_PointerControl();

        public СкладськіПриміщення_ШвидкийВибір()
        {
            ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            //Власник
            HBoxTop.PackStart(СкладВласник, false, false, 2);
            СкладВласник.Caption = "Склад:";
            СкладВласник.AfterSelectFunc = async () => { await LoadRecords(); };
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.SelectPointerItem = null;
            ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.ОчиститиВідбір(TreeViewGrid);

            if (!СкладВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіПриміщення_Const.Склад, Comparison.EQ, СкладВласник.Pointer.UnigueID.UGuid));
            }

            await ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.СкладськіПриміщення_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!СкладВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.СкладськіПриміщення_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіПриміщення_Const.Склад, Comparison.EQ, СкладВласник.Pointer.UnigueID.UGuid));
            }

            //Відбори
            ТабличніСписки.СкладськіПриміщення_Записи.ДодатиВідбір(TreeViewGrid, СкладськіПриміщення_ВідбориДляПошуку.Відбори(searchText));

            await ТабличніСписки.СкладськіПриміщення_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            СкладськіПриміщення page = new СкладськіПриміщення()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                OpenFolder = OpenFolder
            };

            page.СкладВласник.Pointer = СкладВласник.Pointer;

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {СкладськіПриміщення_Const.FULLNAME}", () => page);

            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            СкладськіПриміщення_Елемент page = new СкладськіПриміщення_Елемент
            {
                IsNew = IsNew,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer
            };

            if (IsNew)
            {
                await page.Елемент.New();
                page.СкладДляНового = СкладВласник.Pointer;
            }
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
            СкладськіПриміщення_Objest Обєкт = new СкладськіПриміщення_Objest();
            if (await Обєкт.Read(unigueID))
                await Обєкт.SetDeletionLabel(!Обєкт.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }
    }
}