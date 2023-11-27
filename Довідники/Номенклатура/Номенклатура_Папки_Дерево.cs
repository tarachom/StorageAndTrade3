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
    class Номенклатура_Папки_Дерево : ДовідникДерево
    {
        public Номенклатура_Папки_Дерево() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.Номенклатура_Папки_Записи.Store;
            ТабличніСписки.Номенклатура_Папки_Записи.AddColumns(TreeViewGrid);
        }

        public override async void LoadTree()
        {
            await ТабличніСписки.Номенклатура_Папки_Записи.LoadTree(OpenFolder, DirectoryPointerItem);

            TreeViewGrid.ExpandToPath(ТабличніСписки.Номенклатура_Папки_Записи.RootPath);
            TreeViewGrid.SetCursor(ТабличніСписки.Номенклатура_Папки_Записи.RootPath, TreeViewGrid.Columns[0], false);

            if (ТабличніСписки.Номенклатура_Папки_Записи.SelectPath != null)
            {
                TreeViewGrid.ExpandToPath(ТабличніСписки.Номенклатура_Папки_Записи.SelectPath);
                TreeViewGrid.SetCursor(ТабличніСписки.Номенклатура_Папки_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            }

            RowActivated();
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{Номенклатура_Папки_Const.FULLNAME} *", () =>
                {
                    Номенклатура_Папки_Елемент page = new Номенклатура_Папки_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadTree,
                        IsNew = true,
                        РодичДляНового = new Номенклатура_Папки_Pointer(DirectoryPointerItem ?? new UnigueID())
                    };

                    page.SetValue();

                    return page;
                }, true);
            }
            else if (unigueID != null)
            {
                Номенклатура_Папки_Objest Номенклатура_Папки_Objest = new Номенклатура_Папки_Objest();
                if (await Номенклатура_Папки_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{Номенклатура_Папки_Objest.Назва}", () =>
                    {
                        Номенклатура_Папки_Елемент page = new Номенклатура_Папки_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadTree,
                            IsNew = false,
                            Номенклатура_Папки_Objest = Номенклатура_Папки_Objest
                        };

                        page.SetValue();

                        return page;
                    }, true);
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }


        #region ToolBar

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            Номенклатура_Папки_Objest Номенклатура_Папки_Objest = new Номенклатура_Папки_Objest();
            if (await Номенклатура_Папки_Objest.Read(unigueID))
                await Номенклатура_Папки_Objest.SetDeletionLabel(!Номенклатура_Папки_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            Номенклатура_Папки_Objest Номенклатура_Папки_Objest = new Номенклатура_Папки_Objest();
            if (await Номенклатура_Папки_Objest.Read(unigueID))
            {
                Номенклатура_Папки_Objest Номенклатура_Папки_Objest_Новий = await Номенклатура_Папки_Objest.Copy(true);
                await Номенклатура_Папки_Objest_Новий.Save();

                return Номенклатура_Папки_Objest_Новий.UnigueID;
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