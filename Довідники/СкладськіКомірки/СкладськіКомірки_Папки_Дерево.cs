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
    class СкладськіКомірки_Папки_Дерево : ДовідникДерево
    {
        public СкладськіПриміщення_Pointer СкладПриміщенняВласник = new СкладськіПриміщення_Pointer();

        public СкладськіКомірки_Папки_Дерево() : base()
        {
            ТабличніСписки.СкладськіКомірки_Папки_Записи.AddColumns(TreeViewGrid);
        }

        public override async void LoadTree()
        {
            await ТабличніСписки.СкладськіКомірки_Папки_Записи.LoadTree(TreeViewGrid, OpenFolder, DirectoryPointerItem, СкладПриміщенняВласник.UnigueID);

            TreeViewGrid.ExpandToPath(ТабличніСписки.СкладськіКомірки_Папки_Записи.RootPath);
            TreeViewGrid.SetCursor(ТабличніСписки.СкладськіКомірки_Папки_Записи.RootPath, TreeViewGrid.Columns[0], false);

            if (ТабличніСписки.СкладськіКомірки_Папки_Записи.SelectPath != null)
            {
                TreeViewGrid.ExpandToPath(ТабличніСписки.СкладськіКомірки_Папки_Записи.SelectPath);
                TreeViewGrid.SetCursor(ТабличніСписки.СкладськіКомірки_Папки_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            }

            RowActivated();
        }

        protected override async ValueTask<(string Name, Func<Widget>? FuncWidget, System.Action? SetValue)> OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            СкладськіКомірки_Папки_Елемент page = new СкладськіКомірки_Папки_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadTree,
                IsNew = IsNew,
                РодичДляНового = new СкладськіКомірки_Папки_Pointer(DirectoryPointerItem ?? new UnigueID())
            };

            if (IsNew)
                await page.Елемент.New();
            else if (unigueID == null || !await page.Елемент.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return ("", null, null);
            }

            return (page.Caption, () => page, page.SetValue);
        }

        #region ToolBar

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            СкладськіКомірки_Папки_Objest СкладськіКомірки_Папки_Objest = new СкладськіКомірки_Папки_Objest();
            if (await СкладськіКомірки_Папки_Objest.Read(unigueID))
                await СкладськіКомірки_Папки_Objest.SetDeletionLabel(!СкладськіКомірки_Папки_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            СкладськіКомірки_Папки_Objest СкладськіКомірки_Папки_Objest = new СкладськіКомірки_Папки_Objest();
            if (await СкладськіКомірки_Папки_Objest.Read(unigueID))
            {
                СкладськіКомірки_Папки_Objest СкладськіКомірки_Папки_Objest_Новий = await СкладськіКомірки_Папки_Objest.Copy(true);
                await СкладськіКомірки_Папки_Objest_Новий.Save();

                return СкладськіКомірки_Папки_Objest_Новий.UnigueID;
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