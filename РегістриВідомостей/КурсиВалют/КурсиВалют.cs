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
using ТабличніСписки = StorageAndTrade_1_0.РегістриВідомостей.ТабличніСписки;
using StorageAndTrade_1_0.РегістриВідомостей;

namespace StorageAndTrade
{
    class КурсиВалют : РегістриВідомостейЖурнал
    {
        public Валюти_PointerControl ВалютаВласник = new Валюти_PointerControl();

        public КурсиВалют() 
        {
            ТабличніСписки.КурсиВалют_Записи.AddColumns(TreeViewGrid);

            HBoxTop.PackStart(ВалютаВласник, false, false, 2);
            ВалютаВласник.AfterSelectFunc = async () =>
            {
                SelectPointerItem?.Clear();
                await LoadRecords();
            };
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.КурсиВалют_Записи.SelectPointerItem = SelectPointerItem;

            ТабличніСписки.КурсиВалют_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            if (!ВалютаВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.КурсиВалют_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(КурсиВалют_Const.Валюта, Comparison.EQ, ВалютаВласник.Pointer.UnigueID.UGuid));
            }

            await ТабличніСписки.КурсиВалют_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.КурсиВалют_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.КурсиВалют_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.КурсиВалют_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.КурсиВалют_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            if (ВалютаВласник.Pointer.UnigueID.IsEmpty())
                return;

            ТабличніСписки.КурсиВалют_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!ВалютаВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.КурсиВалют_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(КурсиВалют_Const.Валюта, Comparison.EQ, ВалютаВласник.Pointer.UnigueID.UGuid));
            }

            //period
            ТабличніСписки.КурсиВалют_Записи.ДодатиВідбір(TreeViewGrid,
                new Where("period", Comparison.LIKE, searchText)
                {
                    FuncToField = "to_char",
                    FuncToField_Param1 = "'DD.MM.YYYY'"
                }
            );

            await ТабличніСписки.КурсиВалют_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            КурсиВалют_Елемент page = new КурсиВалют_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                IsNew = IsNew
            };

            if (IsNew)
                page.КурсиВалют_Objest.New();
            else if (unigueID == null || !await page.КурсиВалют_Objest.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return;
            }

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);

            page.SetValue();
        }

        protected override async ValueTask Delete(UnigueID unigueID)
        {
            КурсиВалют_Objest КурсиВалют_Objest = new КурсиВалют_Objest();
            if (await КурсиВалют_Objest.Read(unigueID))
                await КурсиВалют_Objest.Delete();
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            КурсиВалют_Objest КурсиВалют_Objest = new КурсиВалют_Objest();
            if (await КурсиВалют_Objest.Read(unigueID))
            {
                КурсиВалют_Objest КурсиВалют_Objest_Новий = КурсиВалют_Objest.Copy();
                await КурсиВалют_Objest_Новий.Save();

                return КурсиВалют_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "РегістриВідомостей.КурсиВалют";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await LoadRecords();
        }
    }
}