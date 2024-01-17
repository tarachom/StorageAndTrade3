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

using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0.Довідники;

using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class Банки : ДовідникЖурнал
    {
        public Банки() : base()
        {
            //Завантаження списку Банків
            {
                LinkButton linkButtonDownloadCurs = new LinkButton(" Завантаження списку Банків") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
                linkButtonDownloadCurs.Clicked += (object? sender, EventArgs args) =>
                {
                    Program.GeneralForm?.CreateNotebookPage("Завантаження списку Банків", () =>
                    {
                        return new Обробка_ЗавантаженняБанків();
                    });
                };

                HBoxTop.PackStart(linkButtonDownloadCurs, false, false, 10);
            }

            ТабличніСписки.Банки_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.Банки_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Банки_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Банки_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Банки_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.Банки_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Банки_Записи.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Відбори
            ТабличніСписки.Банки_Записи.ДодатиВідбір(TreeViewGrid, Банки_ВідбориДляПошуку.Відбори(searchText), true);

            await ТабличніСписки.Банки_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.Банки_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Банки_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{Банки_Const.FULLNAME} *", () =>
                {
                    Банки_Елемент page = new Банки_Елемент
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
                Банки_Objest Банки_Objest = new Банки_Objest();
                if (await Банки_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{Банки_Objest.Назва}", () =>
                    {
                        Банки_Елемент page = new Банки_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            Банки_Objest = Банки_Objest,
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
            Банки_Objest Банки_Objest = new Банки_Objest();
            if (await Банки_Objest.Read(unigueID))
                await Банки_Objest.SetDeletionLabel(!Банки_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            Банки_Objest Банки_Objest = new Банки_Objest();
            if (await Банки_Objest.Read(unigueID))
            {
                Банки_Objest Банки_Objest_Новий = await Банки_Objest.Copy(true);
                await Банки_Objest_Новий.Save();

                return Банки_Objest_Новий.UnigueID;
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