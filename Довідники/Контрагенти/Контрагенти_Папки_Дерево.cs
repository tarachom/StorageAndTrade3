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
    class Контрагенти_Папки_Дерево : ДовідникДерево
    {
        public Контрагенти_Папки_Дерево() : base()
        {
            ТабличніСписки.Контрагенти_Папки_Записи.AddColumns(TreeViewGrid);
        }

        public override async void LoadTree()
        {
            await ТабличніСписки.Контрагенти_Папки_Записи.LoadTree(TreeViewGrid, OpenFolder, DirectoryPointerItem);

            TreeViewGrid.ExpandToPath(ТабличніСписки.Контрагенти_Папки_Записи.RootPath);
            TreeViewGrid.SetCursor(ТабличніСписки.Контрагенти_Папки_Записи.RootPath, TreeViewGrid.Columns[0], false);

            if (ТабличніСписки.Контрагенти_Папки_Записи.SelectPath != null)
            {
                TreeViewGrid.ExpandToPath(ТабличніСписки.Контрагенти_Папки_Записи.SelectPath);
                TreeViewGrid.SetCursor(ТабличніСписки.Контрагенти_Папки_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            }

            RowActivated();
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{Контрагенти_Папки_Const.FULLNAME} *", () =>
                {
                    Контрагенти_Папки_Елемент page = new Контрагенти_Папки_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadTree,
                        IsNew = true,
                        РодичДляНового = new Контрагенти_Папки_Pointer(DirectoryPointerItem ?? new UnigueID())
                    };

                    page.SetValue();

                    return page;
                });
            }
            else if (unigueID != null)
            {
                Контрагенти_Папки_Objest Контрагенти_Папки_Objest = new Контрагенти_Папки_Objest();
                if (await Контрагенти_Папки_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{Контрагенти_Папки_Objest.Назва}", () =>
                    {
                        Контрагенти_Папки_Елемент page = new Контрагенти_Папки_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadTree,
                            IsNew = false,
                            Контрагенти_Папки_Objest = Контрагенти_Папки_Objest
                        };

                        page.SetValue();

                        return page;
                    });
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }


        #region ToolBar

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            Контрагенти_Папки_Objest Контрагенти_Папки_Objest = new Контрагенти_Папки_Objest();
            if (await Контрагенти_Папки_Objest.Read(unigueID))
                await Контрагенти_Папки_Objest.SetDeletionLabel(!Контрагенти_Папки_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            Контрагенти_Папки_Objest Контрагенти_Папки_Objest = new Контрагенти_Папки_Objest();
            if (await Контрагенти_Папки_Objest.Read(unigueID))
            {
                Контрагенти_Папки_Objest Контрагенти_Папки_Objest_Новий = await Контрагенти_Папки_Objest.Copy(true);
                await Контрагенти_Папки_Objest_Новий.Save();

                return Контрагенти_Папки_Objest_Новий.UnigueID;
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