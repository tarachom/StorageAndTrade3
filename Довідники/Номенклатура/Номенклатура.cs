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
using StorageAndTrade_1_0.РегістриВідомостей;

using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class Номенклатура : ДовідникЖурнал
    {
        CheckButton checkButtonIsHierarchy = new CheckButton("Ієрархія папок") { Active = true };
        Номенклатура_Папки ДеревоПапок;

        public Номенклатура()
        {
            //Враховувати ієрархію папок
            checkButtonIsHierarchy.Clicked += async (object? sender, EventArgs args) => await LoadRecords();
            HBoxTop.PackStart(checkButtonIsHierarchy, false, false, 10);

            CreateLink(HBoxTop, ХарактеристикиНоменклатури_Const.FULLNAME, async () =>
            {
                ХарактеристикиНоменклатури page = new ХарактеристикиНоменклатури();

                if (SelectPointerItem != null)
                    page.НоменклатураВласник.Pointer = new Номенклатура_Pointer(SelectPointerItem);

                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ХарактеристикиНоменклатури_Const.FULLNAME}", () => page);

                await page.SetValue();
            });

            CreateLink(HBoxTop, ШтрихкодиНоменклатури_Const.FULLNAME, async () =>
            {
                ШтрихкодиНоменклатури page = new ШтрихкодиНоменклатури();

                if (SelectPointerItem != null)
                    page.НоменклатураВласник.Pointer = new Номенклатура_Pointer(SelectPointerItem);

                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ШтрихкодиНоменклатури_Const.FULLNAME}", () => page);

                await page.SetValue();
            });

            //Дерево папок cправа
            ДеревоПапок = new Номенклатура_Папки
            {
                WidthRequest = 500,
                CallBack_RowActivated = LoadRecords_TreeCallBack
            };

            HPanedTable.Pack2(ДеревоПапок, false, true);

            ТабличніСписки.Номенклатура_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            if (DirectoryPointerItem != null || SelectPointerItem != null)
            {
                Номенклатура_Objest? Обєкт = await new Номенклатура_Pointer(SelectPointerItem ?? DirectoryPointerItem ?? new UnigueID()).GetDirectoryObject();
                if (Обєкт != null) ДеревоПапок.SelectPointerItem = Обєкт.Папка.UnigueID;
            }

            await ДеревоПапок.SetValue();
        }

        async void LoadRecords_TreeCallBack()
        {
            ТабличніСписки.Номенклатура_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Номенклатура_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Номенклатура_Записи.ОчиститиВідбір(TreeViewGrid);

            if (checkButtonIsHierarchy.Active)
                ТабличніСписки.Номенклатура_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(Номенклатура_Const.Папка, Comparison.EQ, ДеревоПапок.SelectPointerItem?.UGuid ?? new UnigueID().UGuid));

            await ТабличніСписки.Номенклатура_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            //Відбори
            ТабличніСписки.Номенклатура_Записи.ДодатиВідбір(TreeViewGrid, Номенклатура_ВідбориДляПошуку.Відбори(searchText), true);

            await ТабличніСписки.Номенклатура_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.Номенклатура_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            Номенклатура_Елемент page = new Номенклатура_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                IsNew = IsNew
            };

            if (IsNew)
                await page.Елемент.New();
            else if (unigueID == null || !await page.Елемент.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return;
            }

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);

            page.SetValue();
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            Номенклатура_Objest Номенклатура_Objest = new Номенклатура_Objest();
            if (await Номенклатура_Objest.Read(unigueID))
                await Номенклатура_Objest.SetDeletionLabel(!Номенклатура_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            Номенклатура_Objest Номенклатура_Objest = new Номенклатура_Objest();
            if (await Номенклатура_Objest.Read(unigueID))
            {
                Номенклатура_Objest Номенклатура_Objest_Новий = await Номенклатура_Objest.Copy(true);
                await Номенклатура_Objest_Новий.Save();
                await Номенклатура_Objest_Новий.Файли_TablePart.Save(false);

                return Номенклатура_Objest_Новий.UnigueID;
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