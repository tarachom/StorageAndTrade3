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
    public class Валюти : ДовідникЖурнал
    {
        public Валюти() : base()
        {
            //Курси валют
            {
                LinkButton linkButtonCurs = new LinkButton(" Курси валют") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
                linkButtonCurs.Clicked += (object? sender, EventArgs args) =>
                {
                    if (SelectPointerItem != null || DirectoryPointerItem != null)
                    {
                        КурсиВалют page = new КурсиВалют();
                        page.ВалютаВласник.Pointer = new Валюти_Pointer(SelectPointerItem != null ? SelectPointerItem : DirectoryPointerItem!);

                        Program.GeneralForm?.CreateNotebookPage("Курси валют", () => { return page; }, true);
                        page.LoadRecords();
                    }
                };

                HBoxTop.PackStart(linkButtonCurs, false, false, 10);
            }

            //Завантаження курсів валют НБУ
            {
                LinkButton linkButtonDownloadCurs = new LinkButton(" Завантаження курсів валют НБУ") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
                linkButtonDownloadCurs.Clicked += (object? sender, EventArgs args) =>
                {
                    Program.GeneralForm?.CreateNotebookPage("Завантаження курсів валют НБУ", () =>
                    {
                        return new Обробка_ЗавантаженняКурсівВалют();
                    }, true);
                };

                HBoxTop.PackStart(linkButtonDownloadCurs, false, false, 10);
            }

            TreeViewGrid.Model = ТабличніСписки.Валюти_Записи.Store;
            ТабличніСписки.Валюти_Записи.AddColumns(TreeViewGrid);

            MessageRequestText = "Встановити або зняти помітку на видалення?\n\nУВАГА!\nПри встановленні помітку на видалення, буде очищений регіст Курси Валют!";
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.Валюти_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Валюти_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Валюти_Записи.Where.Clear();

            ТабличніСписки.Валюти_Записи.LoadRecords();

            if (ТабличніСписки.Валюти_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Валюти_Записи.SelectPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.Валюти_Записи.Where.Clear();

            //Назва
            ТабличніСписки.Валюти_Записи.Where.Add(
                new Where(Валюти_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.Валюти_Записи.LoadRecords();

            if (ТабличніСписки.Валюти_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Валюти_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{Валюти_Const.FULLNAME} *", () =>
                {
                    Валюти_Елемент page = new Валюти_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadRecords,
                        IsNew = true
                    };

                    page.SetValue();

                    return page;
                }, true);
            }
            else if (unigueID != null)
            {
                Валюти_Objest Валюти_Objest = new Валюти_Objest();
                if (Валюти_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{Валюти_Objest.Назва}", () =>
                    {
                        Валюти_Елемент page = new Валюти_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            Валюти_Objest = Валюти_Objest,
                        };

                        page.SetValue();

                        return page;
                    }, true);
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            Валюти_Objest Валюти_Objest = new Валюти_Objest();
            if (Валюти_Objest.Read(unigueID))
                await Валюти_Objest.SetDeletionLabel(!Валюти_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            Валюти_Objest Валюти_Objest = new Валюти_Objest();
            if (Валюти_Objest.Read(unigueID))
            {
                Валюти_Objest Валюти_Objest_Новий = Валюти_Objest.Copy(true);
                await Валюти_Objest_Новий.Save();

                return Валюти_Objest_Новий.UnigueID;
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