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

using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0.Довідники;

using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class ВидиНоменклатури : ДовідникЖурнал
    {
        public ВидиНоменклатури() : base()
        {
            ТабличніСписки.ВидиНоменклатури_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ВидиНоменклатури_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ВидиНоменклатури_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ВидиНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ВидиНоменклатури_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ВидиНоменклатури_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВидиНоменклатури_Записи.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Відбори
            ТабличніСписки.ВидиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid, ВидиНоменклатури_ВідбориДляПошуку.Відбори(searchText), true);

            await ТабличніСписки.ВидиНоменклатури_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ВидиНоменклатури_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВидиНоменклатури_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{ВидиНоменклатури_Const.FULLNAME} *", () =>
                {
                    ВидиНоменклатури_Елемент page = new ВидиНоменклатури_Елемент
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
                ВидиНоменклатури_Objest ВидиНоменклатури_Objest = new ВидиНоменклатури_Objest();
                if (await ВидиНоменклатури_Objest.Read(unigueID))
                {
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{ВидиНоменклатури_Objest.Назва}", () =>
                    {
                        ВидиНоменклатури_Елемент page = new ВидиНоменклатури_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            ВидиНоменклатури_Objest = ВидиНоменклатури_Objest,
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
            ВидиНоменклатури_Objest ВидиНоменклатури_Objest = new ВидиНоменклатури_Objest();
            if (await ВидиНоменклатури_Objest.Read(unigueID))
                await ВидиНоменклатури_Objest.SetDeletionLabel(!ВидиНоменклатури_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ВидиНоменклатури_Objest ВидиНоменклатури_Objest = new ВидиНоменклатури_Objest();
            if (await ВидиНоменклатури_Objest.Read(unigueID))
            {
                ВидиНоменклатури_Objest ВидиНоменклатури_Objest_Новий = await ВидиНоменклатури_Objest.Copy(true);
                await ВидиНоменклатури_Objest_Новий.Save();

                return ВидиНоменклатури_Objest_Новий.UnigueID;
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