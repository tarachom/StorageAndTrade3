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
    public class Валюти : ДовідникЖурнал
    {
        public Валюти() : base()
        {
            //Курси валют
            {
                LinkButton linkButtonCurs = new LinkButton(" Курси валют") { Halign = Align.Start, Image = new Image(InterfaceGtk.Іконки.ДляКнопок.Doc), AlwaysShowImage = true };
                linkButtonCurs.Clicked += (object? sender, EventArgs args) =>
                {
                    if (SelectPointerItem != null || DirectoryPointerItem != null)
                    {
                        КурсиВалют page = new КурсиВалют();
                        page.ВалютаВласник.Pointer = new Валюти_Pointer(SelectPointerItem != null ? SelectPointerItem : DirectoryPointerItem!);

                        NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Курси валют", () => { return page; });
                        page.SetValue();
                    }
                };

                HBoxTop.PackStart(linkButtonCurs, false, false, 10);
            }

            //Завантаження курсів валют НБУ
            {
                LinkButton linkButtonDownloadCurs = new LinkButton(" Завантаження курсів валют НБУ") { Halign = Align.Start, Image = new Image(InterfaceGtk.Іконки.ДляКнопок.Doc), AlwaysShowImage = true };
                linkButtonDownloadCurs.Clicked += (object? sender, EventArgs args) =>
                {
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Завантаження курсів валют НБУ", () =>
                    {
                        return new Обробка_ЗавантаженняКурсівВалют();
                    });
                };

                HBoxTop.PackStart(linkButtonDownloadCurs, false, false, 10);
            }

            ТабличніСписки.Валюти_Записи.AddColumns(TreeViewGrid);

            MessageRequestText = "Встановити або зняти помітку на видалення?\n\nУВАГА!\nПри встановленні помітку на видалення, буде очищений регіст Курси Валют!";
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.Валюти_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Валюти_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Валюти_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Валюти_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.Валюти_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Валюти_Записи.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Відбори
            ТабличніСписки.Валюти_Записи.ДодатиВідбір(TreeViewGrid, Валюти_ВідбориДляПошуку.Відбори(searchText), true);

            await ТабличніСписки.Валюти_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.Валюти_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Валюти_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask<(string Name, Func<Widget>? FuncWidget, System.Action? SetValue)> OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            Валюти_Елемент page = new Валюти_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                IsNew = IsNew
            };

            if (!IsNew && unigueID != null)
                if (!await page.Валюти_Objest.Read(unigueID))
                {
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                    return ("", null, null);
                }

            return (IsNew ? Валюти_Const.FULLNAME : page.Валюти_Objest.Назва, () => page, page.SetValue);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            Валюти_Objest Валюти_Objest = new Валюти_Objest();
            if (await Валюти_Objest.Read(unigueID))
                await Валюти_Objest.SetDeletionLabel(!Валюти_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            Валюти_Objest Валюти_Objest = new Валюти_Objest();
            if (await Валюти_Objest.Read(unigueID))
            {
                Валюти_Objest Валюти_Objest_Новий = await Валюти_Objest.Copy(true);
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