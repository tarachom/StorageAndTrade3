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
        Номенклатура_Папки_Дерево ДеревоПапок;

        public Номенклатура() : base()
        {
            //Враховувати ієрархію папок
            checkButtonIsHierarchy.Clicked += OnCheckButtonIsHierarchyClicked;
            HBoxTop.PackStart(checkButtonIsHierarchy, false, false, 10);

            //Характеристики
            {
                LinkButton linkButtonHar = new LinkButton($" {ХарактеристикиНоменклатури_Const.FULLNAME}") { Halign = Align.Start, Image = new Image(InterfaceGtk.Іконки.ДляКнопок.Doc), AlwaysShowImage = true };
                linkButtonHar.Clicked += async (object? sender, EventArgs args) =>
                {
                    ХарактеристикиНоменклатури page = new ХарактеристикиНоменклатури();

                    if (SelectPointerItem != null)
                        page.НоменклатураВласник.Pointer = new Номенклатура_Pointer(SelectPointerItem);

                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{ХарактеристикиНоменклатури_Const.FULLNAME}", () => { return page; });

                    await page.LoadRecords();
                };

                HBoxTop.PackStart(linkButtonHar, false, false, 10);
            }

            //ШтрихКоди
            {
                LinkButton linkButtonShKody = new LinkButton($" {ШтрихкодиНоменклатури_Const.FULLNAME}") { Halign = Align.Start, Image = new Image(InterfaceGtk.Іконки.ДляКнопок.Doc), AlwaysShowImage = true };
                linkButtonShKody.Clicked += (object? sender, EventArgs args) =>
                {
                    ШтрихкодиНоменклатури page = new ШтрихкодиНоменклатури();

                    if (SelectPointerItem != null)
                        page.НоменклатураВласник.Pointer = new Номенклатура_Pointer(SelectPointerItem);

                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{ШтрихкодиНоменклатури_Const.FULLNAME}", () => { return page; });

                    page.LoadRecords();
                };

                HBoxTop.PackStart(linkButtonShKody, false, false, 10);
            }

            //Дерево папок cправа
            ДеревоПапок = new Номенклатура_Папки_Дерево
            {
                WidthRequest = 500,
                CallBack_RowActivated = LoadRecords_TreeCallBack
            };

            HPanedTable.Pack2(ДеревоПапок, false, true);

            ТабличніСписки.Номенклатура_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            if (DirectoryPointerItem != null || SelectPointerItem != null)
            {
                UnigueID? unigueID = SelectPointerItem != null ? SelectPointerItem : DirectoryPointerItem;

                Номенклатура_Objest? номенклатура_Objest = await new Номенклатура_Pointer(unigueID ?? new UnigueID()).GetDirectoryObject();
                if (номенклатура_Objest != null)
                    ДеревоПапок.DirectoryPointerItem = номенклатура_Objest.Папка.UnigueID;
            }

            ДеревоПапок.LoadTree();
        }

        async void LoadRecords_TreeCallBack()
        {
            ТабличніСписки.Номенклатура_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Номенклатура_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Номенклатура_Записи.ОчиститиВідбір(TreeViewGrid);

            if (checkButtonIsHierarchy.Active)
                ТабличніСписки.Номенклатура_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(Номенклатура_Const.Папка, Comparison.EQ, ДеревоПапок.DirectoryPointerItem?.UGuid ?? new UnigueID().UGuid));

            await ТабличніСписки.Номенклатура_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.Номенклатура_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Номенклатура_Записи.SelectPath, TreeViewGrid.Columns[0], false);

        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Відбори
            ТабличніСписки.Номенклатура_Записи.ДодатиВідбір(TreeViewGrid, Номенклатура_ВідбориДляПошуку.Відбори(searchText), true);

            await ТабличніСписки.Номенклатура_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.Номенклатура_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Номенклатура_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{Номенклатура_Const.FULLNAME} *", () =>
                {
                    Номенклатура_Елемент page = new Номенклатура_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadRecords,
                        IsNew = true,
                        РодичДляНового = new Номенклатура_Папки_Pointer(ДеревоПапок.DirectoryPointerItem ?? new UnigueID())
                    };

                    page.SetValue();

                    return page;
                });
            }
            else if (unigueID != null)
            {
                Номенклатура_Objest Номенклатура_Objest = new Номенклатура_Objest();
                if (await Номенклатура_Objest.Read(unigueID))
                {
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{Номенклатура_Objest.Назва}", () =>
                    {
                        Номенклатура_Елемент page = new Номенклатура_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            Номенклатура_Objest = Номенклатура_Objest,
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

        protected override void FilterRecords(Box vBox)
        {

        }

        #endregion

        async void OnCheckButtonIsHierarchyClicked(object? sender, EventArgs args)
        {
            await LoadRecords();
        }
    }
}