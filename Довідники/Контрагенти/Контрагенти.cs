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
    public class Контрагенти : ДовідникЖурнал
    {
        CheckButton checkButtonIsHierarchy = new CheckButton("Ієрархія папок") { Active = true };
        Контрагенти_Папки_Дерево ДеревоПапок;

        public Контрагенти() : base()
        {
            //Враховувати ієрархію папок
            checkButtonIsHierarchy.Clicked += OnCheckButtonIsHierarchyClicked;
            HBoxTop.PackStart(checkButtonIsHierarchy, false, false, 10);

            //Договори
            {
                LinkButton linkButton = new LinkButton($" {ДоговориКонтрагентів_Const.FULLNAME}") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
                linkButton.Clicked += async (object? sender, EventArgs args) =>
                {
                    ДоговориКонтрагентів page = new ДоговориКонтрагентів();

                    if (SelectPointerItem != null)
                        page.КонтрагентВласник.Pointer = new Контрагенти_Pointer(SelectPointerItem);

                    Program.GeneralForm?.CreateNotebookPage($"{ДоговориКонтрагентів_Const.FULLNAME}", () => { return page; });

                    await page.LoadRecords();
                };

                HBoxTop.PackStart(linkButton, false, false, 10);
            }

            //Дерево папок зправа
            ДеревоПапок = new Контрагенти_Папки_Дерево
            {
                WidthRequest = 500,
                CallBack_RowActivated = LoadRecords_TreeCallBack
            };
            HPanedTable.Pack2(ДеревоПапок, false, true);

            TreeViewGrid.Model = ТабличніСписки.Контрагенти_Записи.Store;
            ТабличніСписки.Контрагенти_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            if (DirectoryPointerItem != null || SelectPointerItem != null)
            {
                UnigueID? unigueID = SelectPointerItem != null ? SelectPointerItem : DirectoryPointerItem;

                Контрагенти_Objest? контрагенти_Objest = await new Контрагенти_Pointer(unigueID ?? new UnigueID()).GetDirectoryObject();
                if (контрагенти_Objest != null)
                    ДеревоПапок.DirectoryPointerItem = контрагенти_Objest.Папка.UnigueID;
            }

            ДеревоПапок.LoadTree();
        }

        async void LoadRecords_TreeCallBack()
        {
            ТабличніСписки.Контрагенти_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Контрагенти_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Контрагенти_Записи.Where.Clear();

            if (checkButtonIsHierarchy.Active)
                ТабличніСписки.Контрагенти_Записи.Where.Add(new Where(Контрагенти_Const.Папка, Comparison.EQ,
                    ДеревоПапок.DirectoryPointerItem?.UGuid ?? new UnigueID().UGuid));

            await ТабличніСписки.Контрагенти_Записи.LoadRecords();

            if (ТабличніСписки.Контрагенти_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Контрагенти_Записи.SelectPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.Контрагенти_Записи.Where.Clear();

            //Код
            ТабличніСписки.Контрагенти_Записи.Where.Add(
                new Where(Контрагенти_Const.Код, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            //Назва
            ТабличніСписки.Контрагенти_Записи.Where.Add(
                new Where(Comparison.OR, Контрагенти_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            await ТабличніСписки.Контрагенти_Записи.LoadRecords();

            if (ТабличніСписки.Контрагенти_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Контрагенти_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{Контрагенти_Const.FULLNAME} *", () =>
                {
                    Контрагенти_Елемент page = new Контрагенти_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadRecords,
                        IsNew = true,
                        РодичДляНового = new Контрагенти_Папки_Pointer(ДеревоПапок.DirectoryPointerItem ?? new UnigueID())
                    };

                    page.SetValue();

                    return page;
                }, true);
            }
            else if (unigueID != null)
            {
                Контрагенти_Objest Контрагенти_Objest = new Контрагенти_Objest();
                if (await Контрагенти_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{Контрагенти_Objest.Назва}", () =>
                    {
                        Контрагенти_Елемент page = new Контрагенти_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            Контрагенти_Objest = Контрагенти_Objest,
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
            Контрагенти_Objest Контрагенти_Objest = new Контрагенти_Objest();
            if (await Контрагенти_Objest.Read(unigueID))
                await Контрагенти_Objest.SetDeletionLabel(!Контрагенти_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            Контрагенти_Objest Контрагенти_Objest = new Контрагенти_Objest();
            if (await Контрагенти_Objest.Read(unigueID))
            {
                Контрагенти_Objest Контрагенти_Objest_Новий = Контрагенти_Objest.Copy(true);
                await Контрагенти_Objest_Новий.Save();
                await Контрагенти_Objest_Новий.Контакти_TablePart.Save(false);

                return Контрагенти_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        #endregion

        async void OnCheckButtonIsHierarchyClicked(object? sender, EventArgs args)
        {
            await LoadRecords();
        }
    }
}