#region Info

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

#endregion

using Gtk;

using AccountingSoftware;
using ТабличніСписки = StorageAndTrade_1_0.РегістриВідомостей.ТабличніСписки;
using StorageAndTrade_1_0.РегістриВідомостей;

namespace StorageAndTrade
{
    class КурсиВалют : VBox
    {
        TreeView TreeViewGrid;
        public Валюти_PointerControl ВалютаВласник = new Валюти_PointerControl();
        SearchControl2 ПошукПовнотекстовий = new SearchControl2();

        public КурсиВалют(bool IsSelectPointer = false) : base()
        {
            new VBox(false, 0);
            BorderWidth = 0;

            //Кнопки
            HBox hBoxBotton = new HBox();

            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

            hBoxBotton.PackStart(bClose, false, false, 10);

            PackStart(hBoxBotton, false, false, 10);

            //Пошук 2
            hBoxBotton.PackStart(ПошукПовнотекстовий, false, false, 2);
            ПошукПовнотекстовий.Select = LoadRecords_OnSearch;
            ПошукПовнотекстовий.Clear = LoadRecords;

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Never, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.КурсиВалют_Записи.Store);
            ТабличніСписки.КурсиВалют_Записи.AddColumns(TreeViewGrid);

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;

            scrollTree.Add(TreeViewGrid);

            PackStart(scrollTree, true, true, 0);

            ShowAll();
        }

        void CreateToolbar()
        {
            Toolbar toolbar = new Toolbar();
            PackStart(toolbar, false, false, 0);

            ToolButton refreshButton = new ToolButton(Stock.Refresh) { TooltipText = "Обновити" };
            refreshButton.Clicked += OnRefreshClick;
            toolbar.Add(refreshButton);
        }

        public void LoadRecords()
        {
            ТабличніСписки.КурсиВалют_Записи.Where.Clear();

            if (!ВалютаВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.КурсиВалют_Записи.Where.Add(
                    new Where(КурсиВалют_Const.Валюта, Comparison.EQ, ВалютаВласник.Pointer.UnigueID.UGuid));
            }

            ТабличніСписки.КурсиВалют_Записи.LoadRecords();

            if (ТабличніСписки.КурсиВалют_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.КурсиВалют_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.КурсиВалют_Записи.Where.Clear();

            if (!ВалютаВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.КурсиВалют_Записи.Where.Add(
                    new Where(КурсиВалют_Const.Валюта, Comparison.EQ, ВалютаВласник.Pointer.UnigueID.UGuid));
            }

            //period
            ТабличніСписки.КурсиВалют_Записи.Where.Add(
                new Where(Comparison.AND, "period", Comparison.LIKE, searchText)
                {
                    FuncToField = "to_char",
                    FuncToField_Param1 = "'DD.MM.YYYY'"
                }
            );

            ТабличніСписки.КурсиВалют_Записи.LoadRecords();
        }

        #region ToolBar

        void OnRefreshClick(object? sender, EventArgs args)
        {
            LoadRecords();
        }

        #endregion

    }
}