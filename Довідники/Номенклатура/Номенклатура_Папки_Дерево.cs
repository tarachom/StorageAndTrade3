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
    class Номенклатура_Папки_Дерево : ДовідникДерево
    {
        public Номенклатура_Папки_Дерево() : base()
        {
            ТабличніСписки.Номенклатура_Папки_Записи.AddColumns(TreeViewGrid);
        }

        public override async void LoadTree()
        {
            await ТабличніСписки.Номенклатура_Папки_Записи.LoadTree(TreeViewGrid, OpenFolder, DirectoryPointerItem);

            TreeViewGrid.ExpandToPath(ТабличніСписки.Номенклатура_Папки_Записи.RootPath);
            TreeViewGrid.SetCursor(ТабличніСписки.Номенклатура_Папки_Записи.RootPath, TreeViewGrid.Columns[0], false);

            if (ТабличніСписки.Номенклатура_Папки_Записи.SelectPath != null)
            {
                TreeViewGrid.ExpandToPath(ТабличніСписки.Номенклатура_Папки_Записи.SelectPath);
                TreeViewGrid.SetCursor(ТабличніСписки.Номенклатура_Папки_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            }

            RowActivated();
        }

        protected override async ValueTask<(string Name, Func<Widget>? FuncWidget, System.Action? SetValue)> OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            Номенклатура_Папки_Елемент page = new Номенклатура_Папки_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadTree,
                IsNew = IsNew,
                РодичДляНового = new Номенклатура_Папки_Pointer(DirectoryPointerItem ?? new UnigueID())
            };

            if (!IsNew && unigueID != null)
                if (!await page.Номенклатура_Папки_Objest.Read(unigueID))
                {
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                    return ("", null, null);
                }

            return (IsNew ? Номенклатура_Папки_Const.FULLNAME : page.Номенклатура_Папки_Objest.Назва, () => page, page.SetValue);
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