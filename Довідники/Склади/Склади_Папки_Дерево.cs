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
    class Склади_Папки_Дерево : ДовідникДерево
    {
        public Склади_Папки_Дерево() : base()
        {
            ТабличніСписки.Склади_Папки_Записи.AddColumns(TreeViewGrid);
        }

        public override async void LoadTree()
        {
            await ТабличніСписки.Склади_Папки_Записи.LoadTree(TreeViewGrid, OpenFolder, DirectoryPointerItem);

            TreeViewGrid.ExpandToPath(ТабличніСписки.Склади_Папки_Записи.RootPath);
            TreeViewGrid.SetCursor(ТабличніСписки.Склади_Папки_Записи.RootPath, TreeViewGrid.Columns[0], false);

            if (ТабличніСписки.Склади_Папки_Записи.SelectPath != null)
            {
                TreeViewGrid.ExpandToPath(ТабличніСписки.Склади_Папки_Записи.SelectPath);
                TreeViewGrid.SetCursor(ТабличніСписки.Склади_Папки_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            }

            RowActivated();
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{Склади_Папки_Const.FULLNAME} *", () =>
                {
                    Склади_Папки_Елемент page = new Склади_Папки_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadTree,
                        IsNew = true,
                        РодичДляНового = new Склади_Папки_Pointer(DirectoryPointerItem ?? new UnigueID())
                    };

                    page.SetValue();

                    return page;
                });
            }
            else if (unigueID != null)
            {
                Склади_Папки_Objest Склади_Папки_Objest = new Склади_Папки_Objest();
                if (await Склади_Папки_Objest.Read(unigueID))
                {
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{Склади_Папки_Objest.Назва}", () =>
                    {
                        Склади_Папки_Елемент page = new Склади_Папки_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadTree,
                            IsNew = false,
                            Склади_Папки_Objest = Склади_Папки_Objest
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
            Склади_Папки_Objest Склади_Папки_Objest = new Склади_Папки_Objest();
            if (await Склади_Папки_Objest.Read(unigueID))
                await Склади_Папки_Objest.SetDeletionLabel(!Склади_Папки_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            Склади_Папки_Objest Склади_Папки_Objest = new Склади_Папки_Objest();
            if (await Склади_Папки_Objest.Read(unigueID))
            {
                Склади_Папки_Objest Склади_Папки_Objest_Новий = await Склади_Папки_Objest.Copy(true);
                await Склади_Папки_Objest_Новий.Save();

                return Склади_Папки_Objest_Новий.UnigueID;
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