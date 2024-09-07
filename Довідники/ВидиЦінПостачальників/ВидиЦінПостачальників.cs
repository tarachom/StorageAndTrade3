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
    public class ВидиЦінПостачальників : ДовідникЖурнал
    {
        public ВидиЦінПостачальників() : base()
        {
            ТабличніСписки.ВидиЦінПостачальників_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ВидиЦінПостачальників_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ВидиЦінПостачальників_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ВидиЦінПостачальників_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ВидиЦінПостачальників_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            //Відбори
            ТабличніСписки.ВидиЦінПостачальників_Записи.ДодатиВідбір(TreeViewGrid, ВидиЦінПостачальників_ВідбориДляПошуку.Відбори(searchText), true);

            await ТабличніСписки.ВидиЦінПостачальників_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.ВидиЦінПостачальників_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask<(string Name, Func<Widget>? FuncWidget, System.Action? SetValue)> OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            ВидиЦінПостачальників_Елемент page = new ВидиЦінПостачальників_Елемент
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
            ВидиЦінПостачальників_Objest ВидиЦінПостачальників_Objest = new ВидиЦінПостачальників_Objest();
            if (await ВидиЦінПостачальників_Objest.Read(unigueID))
                await ВидиЦінПостачальників_Objest.SetDeletionLabel(!ВидиЦінПостачальників_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ВидиЦінПостачальників_Objest ВидиЦінПостачальників_Objest = new ВидиЦінПостачальників_Objest();
            if (await ВидиЦінПостачальників_Objest.Read(unigueID))
            {
                ВидиЦінПостачальників_Objest ВидиЦінПостачальників_Objest_Новий = await ВидиЦінПостачальників_Objest.Copy(true);
                await ВидиЦінПостачальників_Objest_Новий.Save();

                return ВидиЦінПостачальників_Objest_Новий.UnigueID;
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