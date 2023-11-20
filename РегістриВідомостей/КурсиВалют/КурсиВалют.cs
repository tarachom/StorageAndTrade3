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

using AccountingSoftware;
using ТабличніСписки = StorageAndTrade_1_0.РегістриВідомостей.ТабличніСписки;
using StorageAndTrade_1_0.РегістриВідомостей;

namespace StorageAndTrade
{
    class КурсиВалют : РегістриЖурнал
    {
        public Валюти_PointerControl ВалютаВласник = new Валюти_PointerControl();

        public КурсиВалют() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.КурсиВалют_Записи.Store;
            ТабличніСписки.КурсиВалют_Записи.AddColumns(TreeViewGrid);

            HBoxTop.PackStart(ВалютаВласник, false, false, 2);
            ВалютаВласник.AfterSelectFunc = () =>
            {
                SelectPointerItem?.Clear();
                LoadRecords();
            };
        }

        public override async void LoadRecords()
        {
            if (ВалютаВласник.Pointer.UnigueID.IsEmpty())
                return;

            ТабличніСписки.КурсиВалют_Записи.SelectPointerItem = SelectPointerItem;

            ТабличніСписки.КурсиВалют_Записи.Where.Clear();

            if (!ВалютаВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.КурсиВалют_Записи.Where.Add(
                    new Where(КурсиВалют_Const.Валюта, Comparison.EQ, ВалютаВласник.Pointer.UnigueID.UGuid));
            }

            await ТабличніСписки.КурсиВалют_Записи.LoadRecords();

            if (ТабличніСписки.КурсиВалют_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.КурсиВалют_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.КурсиВалют_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.КурсиВалют_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            if (ВалютаВласник.Pointer.UnigueID.IsEmpty())
                return;

            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.КурсиВалют_Записи.Where.Clear();

            if (!ВалютаВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.КурсиВалют_Записи.Where.Add(
                    new Where(КурсиВалют_Const.Валюта, Comparison.EQ, ВалютаВласник.Pointer.UnigueID.UGuid));
            }

            //period
            ТабличніСписки.КурсиВалют_Записи.Where.Add(
                new Where(Comparison.AND, "period", Comparison.LIKE, searchText)
                {
                    FuncToField = "to_char",
                    FuncToField_Param1 = "'DD.MM.YYYY'"
                }
            );

            await ТабличніСписки.КурсиВалют_Записи.LoadRecords();
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{КурсиВалют_Const.FULLNAME} *", () =>
                {
                    КурсиВалют_Елемент page = new КурсиВалют_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadRecords,
                        IsNew = true,
                        ВалютаДляНового = ВалютаВласник.Pointer
                    };

                    page.SetValue();

                    return page;
                }, true);
            }
            else if (unigueID != null)
            {
                КурсиВалют_Objest КурсиВалют_Objest = new КурсиВалют_Objest();
                if (await КурсиВалют_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{КурсиВалют_Objest.Курс}", () =>
                    {
                        КурсиВалют_Елемент page = new КурсиВалют_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            КурсиВалют_Objest = КурсиВалют_Objest
                        };

                        page.SetValue();

                        return page;
                    }, true);
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }

        protected override async void Delete(UnigueID unigueID)
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
    }
}