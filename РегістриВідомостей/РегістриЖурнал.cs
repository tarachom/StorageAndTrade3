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

namespace StorageAndTrade
{
    public abstract class РегістриЖурнал : VBox
    {
        public UnigueID? SelectPointerItem { get; set; }//?

        protected Toolbar ToolbarTop = new Toolbar();
        protected HBox HBoxTop = new HBox();
        protected TreeView TreeViewGrid = new TreeView();
        SearchControl2 ПошукПовнотекстовий = new SearchControl2();

        public РегістриЖурнал() : base()
        {
            BorderWidth = 0;

            //Кнопки
            HBoxTop = new HBox();
            PackStart(HBoxTop, false, false, 10);

            //Пошук
            ПошукПовнотекстовий.Select = LoadRecords_OnSearch;
            ПошукПовнотекстовий.Clear = LoadRecords;
            HBoxTop.PackStart(ПошукПовнотекстовий, false, false, 2);

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;
            TreeViewGrid.RowActivated += OnRowActivated;
            TreeViewGrid.ButtonPressEvent += OnButtonPressEvent;
            TreeViewGrid.ButtonReleaseEvent += OnButtonReleaseEvent;
            TreeViewGrid.KeyReleaseEvent += OnKeyReleaseEvent;
            scrollTree.Add(TreeViewGrid);

            PackStart(scrollTree, true, true, 0);

            ShowAll();
        }

        #region Toolbar & Menu

        void CreateToolbar()
        {
            PackStart(ToolbarTop, false, false, 0);

            ToolButton addButton = new ToolButton(Stock.Add) { TooltipText = "Додати" };
            addButton.Clicked += OnAddClick;
            ToolbarTop.Add(addButton);

            ToolButton upButton = new ToolButton(Stock.Edit) { TooltipText = "Редагувати" };
            upButton.Clicked += OnEditClick;
            ToolbarTop.Add(upButton);

            ToolButton copyButton = new ToolButton(Stock.Copy) { TooltipText = "Копіювати" };
            copyButton.Clicked += OnCopyClick;
            ToolbarTop.Add(copyButton);

            ToolButton deleteButton = new ToolButton(Stock.Delete) { TooltipText = "Видалити" };
            deleteButton.Clicked += OnDeleteClick;
            ToolbarTop.Add(deleteButton);

            ToolButton refreshButton = new ToolButton(Stock.Refresh) { TooltipText = "Обновити" };
            refreshButton.Clicked += OnRefreshClick;
            ToolbarTop.Add(refreshButton);
        }

        Menu PopUpContextMenu()
        {
            Menu Menu = new Menu();

            MenuItem delete = new MenuItem("Видалити");
            delete.Activated += OnDeleteClick;
            Menu.Append(delete);

            Menu.ShowAll();

            return Menu;
        }

        #endregion

        #region Virtual Function

        public virtual void LoadRecords() { }

        protected virtual void LoadRecords_OnSearch(string searchText) { }

        protected virtual void OpenPageElement(bool IsNew, UnigueID? unigueID = null) { }

        protected virtual void Delete(UnigueID unigueID) { }

        protected virtual UnigueID? Copy(UnigueID unigueID) { return null; }

        public virtual void CallBack_LoadRecords(UnigueID? selectPointer)
        {
            SelectPointerItem = selectPointer;
            LoadRecords();
        }

        #endregion

        #region  TreeView

        void OnRowActivated(object sender, RowActivatedArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]);

                SelectPointerItem = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));
            }
        }

        void OnButtonReleaseEvent(object? sender, ButtonReleaseEventArgs args)
        {
            if (args.Event.Button == 3 && TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    SelectPointerItem = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));
                    PopUpContextMenu().Popup();
                }
            }
        }

        void OnButtonPressEvent(object? sender, ButtonPressEventArgs args)
        {
            if (args.Event.Type == Gdk.EventType.DoubleButtonPress && TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;

                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                    OpenPageElement(false, unigueID);
                }
            }
        }

        void OnKeyReleaseEvent(object? sender, KeyReleaseEventArgs args)
        {
            switch (args.Event.Key)
            {
                case Gdk.Key.Insert:
                    {
                        OpenPageElement(true);
                        break;
                    }
                case Gdk.Key.F5:
                    {
                        LoadRecords();
                        break;
                    }
                case Gdk.Key.KP_Enter:
                case Gdk.Key.Return:
                    {
                        OnEditClick(null, new EventArgs());
                        break;
                    }
                case Gdk.Key.End:
                case Gdk.Key.Home:
                case Gdk.Key.Up:
                case Gdk.Key.Down:
                case Gdk.Key.Prior:
                case Gdk.Key.Next:
                    {
                        OnRowActivated(TreeViewGrid, new RowActivatedArgs());
                        break;
                    }
                case Gdk.Key.Delete:
                    {
                        OnDeleteClick(TreeViewGrid, new EventArgs());
                        break;
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
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    if (TreeViewGrid.Model.GetIter(out iter, itemPath))
                    {
                        UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));
                        OpenPageElement(false, unigueID);
                    }
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
                if (Message.Request(Program.GeneralForm, "Встановити або зняти помітку на видалення?") == ResponseType.Yes)
                {
                    TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                    foreach (TreePath itemPath in selectionRows)
                    {
                        TreeIter iter;
                        TreeViewGrid.Model.GetIter(out iter, itemPath);

                        UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                        Delete(unigueID);

                        SelectPointerItem = unigueID;
                    }

                    LoadRecords();
                }
            }
        }

        void OnCopyClick(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                if (Message.Request(Program.GeneralForm, "Копіювати?") == ResponseType.Yes)
                {
                    TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                    foreach (TreePath itemPath in selectionRows)
                    {
                        TreeIter iter;
                        TreeViewGrid.Model.GetIter(out iter, itemPath);

                        UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                        UnigueID? newUnigueID = Copy(unigueID);

                        if (newUnigueID != null)
                            SelectPointerItem = newUnigueID;
                    }

                    LoadRecords();
                }
            }
        }

        #endregion
    }
}