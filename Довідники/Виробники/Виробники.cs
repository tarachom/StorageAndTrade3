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

using StorageAndTrade_1_0.Довідники;

using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class Виробники : ДовідникЖурнал
    {
        public Виробники() : base()
        {
            ТабличніСписки.Виробники_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.Виробники_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Виробники_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Виробники_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Виробники_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.Виробники_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Виробники_Записи.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Відбори
            ТабличніСписки.Виробники_Записи.ДодатиВідбір(TreeViewGrid, Виробники_ВідбориДляПошуку.Відбори(searchText), true);

            await ТабличніСписки.Виробники_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.Виробники_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Виробники_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{Виробники_Const.FULLNAME} *", () =>
                {
                    Виробники_Елемент page = new Виробники_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadRecords,
                        IsNew = true
                    };

                    page.SetValue();

                    return page;
                });
            }
            else if (unigueID != null)
            {
                Виробники_Objest Виробники_Objest = new Виробники_Objest();
                if (await Виробники_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{Виробники_Objest.Назва}", () =>
                    {
                        Виробники_Елемент page = new Виробники_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            Виробники_Objest = Виробники_Objest
                        };

                        page.SetValue();

                        return page;
                    });
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            Виробники_Objest Виробники_Objest = new Виробники_Objest();
            if (await Виробники_Objest.Read(unigueID))
                await Виробники_Objest.SetDeletionLabel(!Виробники_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            Виробники_Objest Виробники_Objest = new Виробники_Objest();
            if (await Виробники_Objest.Read(unigueID))
            {
                Виробники_Objest Виробники_Objest_Новий = await Виробники_Objest.Copy(true);
                await Виробники_Objest_Новий.Save();

                return Виробники_Objest_Новий.UnigueID;
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