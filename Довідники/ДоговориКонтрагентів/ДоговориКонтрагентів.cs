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
    public class ДоговориКонтрагентів : ДовідникЖурнал
    {
        public Контрагенти_PointerControl КонтрагентВласник = new Контрагенти_PointerControl();

        public ДоговориКонтрагентів() : base()
        {
            //Власник
            HBoxTop.PackStart(КонтрагентВласник, false, false, 2);
            КонтрагентВласник.Caption = $"{Контрагенти_Const.FULLNAME}:";
            КонтрагентВласник.AfterSelectFunc = async () => await LoadRecords();

            ТабличніСписки.ДоговориКонтрагентів_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ДоговориКонтрагентів_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ДоговориКонтрагентів_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ДоговориКонтрагентів_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!КонтрагентВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.ДоговориКонтрагентів_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ДоговориКонтрагентів_Const.Контрагент, Comparison.EQ, КонтрагентВласник.Pointer.UnigueID.UGuid));
            }

            await ТабличніСписки.ДоговориКонтрагентів_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ДоговориКонтрагентів_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ДоговориКонтрагентів_Записи.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ДоговориКонтрагентів_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!КонтрагентВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.ДоговориКонтрагентів_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ДоговориКонтрагентів_Const.Контрагент, Comparison.EQ, КонтрагентВласник.Pointer.UnigueID.UGuid));
            }

            //Відбори
            ТабличніСписки.ДоговориКонтрагентів_Записи.ДодатиВідбір(TreeViewGrid, ДоговориКонтрагентів_ВідбориДляПошуку.Відбори(searchText));

            await ТабличніСписки.ДоговориКонтрагентів_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ДоговориКонтрагентів_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ДоговориКонтрагентів_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask<(string Name, Func<Widget>? FuncWidget, System.Action? SetValue)> OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            ДоговориКонтрагентів_Елемент page = new ДоговориКонтрагентів_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                IsNew = IsNew
            };

            if (IsNew)
                await page.ДоговориКонтрагентів_Objest.New();
            else if (unigueID == null || !await page.ДоговориКонтрагентів_Objest.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return ("", null, null);
            }

            return (page.Caption, () => page, page.SetValue);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            ДоговориКонтрагентів_Objest ДоговориКонтрагентів_Objest = new ДоговориКонтрагентів_Objest();
            if (await ДоговориКонтрагентів_Objest.Read(unigueID))
                await ДоговориКонтрагентів_Objest.SetDeletionLabel(!ДоговориКонтрагентів_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ДоговориКонтрагентів_Objest ДоговориКонтрагентів_Objest = new ДоговориКонтрагентів_Objest();
            if (await ДоговориКонтрагентів_Objest.Read(unigueID))
            {
                ДоговориКонтрагентів_Objest ДоговориКонтрагентів_Objest_Новий = await ДоговориКонтрагентів_Objest.Copy(true);
                await ДоговориКонтрагентів_Objest_Новий.Save();

                return ДоговориКонтрагентів_Objest_Новий.UnigueID;
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