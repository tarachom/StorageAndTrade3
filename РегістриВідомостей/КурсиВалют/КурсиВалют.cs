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
            HBox hBoxTop = new HBox();
            PackStart(hBoxTop, false, false, 10);

            //Пошук 2
            hBoxTop.PackStart(ПошукПовнотекстовий, false, false, 2);
            ПошукПовнотекстовий.Select = LoadRecords_OnSearch;
            ПошукПовнотекстовий.Clear = LoadRecords;

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Never, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.КурсиВалют_Записи.Store);
            ТабличніСписки.КурсиВалют_Записи.AddColumns(TreeViewGrid);

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;
            TreeViewGrid.ButtonPressEvent += OnButtonPressEvent;

            scrollTree.Add(TreeViewGrid);

            PackStart(scrollTree, true, true, 0);

            ShowAll();
        }

        void CreateToolbar()
        {
            Toolbar toolbar = new Toolbar();
            PackStart(toolbar, false, false, 0);

            ToolButton addButton = new ToolButton(Stock.Add) { TooltipText = "Додати" };
            addButton.Clicked += OnAddClick;
            toolbar.Add(addButton);

            ToolButton upButton = new ToolButton(Stock.Edit) { TooltipText = "Редагувати" };
            upButton.Clicked += OnEditClick;
            toolbar.Add(upButton);

            ToolButton deleteButton = new ToolButton(Stock.Delete) { TooltipText = "Видалити" };
            deleteButton.Clicked += OnDeleteClick;
            toolbar.Add(deleteButton);

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

        void OpenPageElement(bool IsNew, string uid = "")
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"Курс: *", () =>
                {
                    КурсиВалют_Елемент page = new КурсиВалют_Елемент
                    {
                        PageList = this,
                        IsNew = true,
                        ВалютаДляНового = ВалютаВласник.Pointer
                    };

                    page.SetValue();

                    return page;
                }, true);
            }
            else
            {
                КурсиВалют_Objest КурсиВалют_Objest = new КурсиВалют_Objest();
                if (КурсиВалют_Objest.Read(new UnigueID(uid)))
                {
                    Program.GeneralForm?.CreateNotebookPage($"Курс: {КурсиВалют_Objest.Курс}", () =>
                    {
                        КурсиВалют_Елемент page = new КурсиВалют_Елемент
                        {
                            PageList = this,
                            IsNew = false,
                            КурсиВалют_Objest = КурсиВалют_Objest
                        };

                        page.SetValue();

                        return page;
                    }, true);
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }

        #region TreeView

        void OnButtonPressEvent(object? sender, ButtonPressEventArgs args)
        {
            if (args.Event.Type == Gdk.EventType.DoubleButtonPress && TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;

                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);
                    OpenPageElement(false, uid);
                }
            }
        }

        #endregion

        #region ToolBar

        void OnAddClick(object? sender, EventArgs args)
        {
            OpenPageElement(true);
        }

        void OnEditClick(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);
                    OpenPageElement(false, uid);
                }
            }
        }

        void OnRefreshClick(object? sender, EventArgs args)
        {
            LoadRecords();
        }

        void OnDeleteClick(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                if (Message.Request(Program.GeneralForm, "Видалити?") == ResponseType.Yes)
                {
                    TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                    foreach (TreePath itemPath in selectionRows)
                    {
                        TreeIter iter;
                        TreeViewGrid.Model.GetIter(out iter, itemPath);

                        string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                        КурсиВалют_Objest КурсиВалют_Objest = new КурсиВалют_Objest();
                        if (КурсиВалют_Objest.Read(new UnigueID(uid)))
                            КурсиВалют_Objest.Delete();
                        else
                            Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                    }

                    LoadRecords();
                }
            }
        }

        #endregion

    }
}