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
    public class Файли : ДовідникЖурнал
    {
        public Файли() : base()
        {
            ТабличніСписки.Файли_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.Файли_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Файли_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Файли_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Файли_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.Файли_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Файли_Записи.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Відбори
            ТабличніСписки.Файли_Записи.ДодатиВідбір(TreeViewGrid, Файли_ВідбориДляПошуку.Відбори(searchText), true);

            await ТабличніСписки.Файли_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.Файли_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Файли_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{Файли_Const.FULLNAME} *", () =>
                {
                    Файли_Елемент page = new Файли_Елемент
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
                Файли_Objest Файли_Objest = new Файли_Objest();
                if (await Файли_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{Файли_Objest.Назва}", () =>
                    {
                        Файли_Елемент page = new Файли_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            Файли_Objest = Файли_Objest,
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
            Файли_Objest Файли_Objest = new Файли_Objest();
            if (await Файли_Objest.Read(unigueID))
                await Файли_Objest.SetDeletionLabel(!Файли_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            Файли_Objest Файли_Objest = new Файли_Objest();
            if (await Файли_Objest.Read(unigueID))
            {
                Файли_Objest Файли_Objest_Новий = await Файли_Objest.Copy(true);
                await Файли_Objest_Новий.Save();

                return Файли_Objest_Новий.UnigueID;
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