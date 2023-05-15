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
    public abstract class ДовідникДерево : VBox
    {
        /// <summary>
        /// Поточний елемент
        /// </summary>
        //public UnigueID? SelectPointerItem { get; set; }

        /// <summary>
        /// Для вибору
        /// </summary>
        public UnigueID? DirectoryPointerItem { get; set; }

        /// <summary>
        /// Відкрита папка.
        /// Використовується при загрузці дерева щоб приховати вітку.
        /// Актуальну у випадку вибору родича, щоб не можна було вибрати у якості родича відкриту папку
        /// </summary>
        public UnigueID? OpenFolder { get; set; }

        /// <summary>
        /// Функція яка викликається коли в дереві активується вітка.
        /// Це зазвичай завантаження списку елементів у таблиці
        /// </summary>
        public System.Action? CallBack_RowActivated { get; set; }

        /// <summary>
        /// Функція вибору
        /// </summary>
        public System.Action<UnigueID>? CallBack_OnSelectPointer { get; set; }

        protected Toolbar ToolbarTop = new Toolbar();
        protected TreeView TreeViewGrid = new TreeView();

        public ДовідникДерево() : base()
        {
            BorderWidth = 0;

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeViewGrid.Selection.Mode = SelectionMode.Single;
            TreeViewGrid.ActivateOnSingleClick = true;
            TreeViewGrid.RowActivated += OnRowActivated;
            TreeViewGrid.ButtonPressEvent += OnButtonPressEvent;

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

            MenuItem setDeletionLabel = new MenuItem("Помітка на видалення");
            setDeletionLabel.Activated += OnDeleteClick;
            Menu.Append(setDeletionLabel);

            Menu.ShowAll();

            return Menu;
        }

        #endregion

        protected void CallBack_LoadTree(UnigueID? selectPointer)
        {
            DirectoryPointerItem = selectPointer;
            LoadTree();
        }

        #region Virtual Function

        public virtual void LoadTree() { }

        protected virtual void OpenPageElement(bool IsNew, UnigueID? unigueID = null) { }

        protected virtual void SetDeletionLabel(UnigueID unigueID) { }

        protected virtual UnigueID? Copy(UnigueID unigueID) { return null; }

        #endregion

        #region  TreeView

        protected void RowActivated()
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]);

                DirectoryPointerItem = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                if (CallBack_RowActivated != null)
                    CallBack_RowActivated.Invoke();
            }
        }

        void OnRowActivated(object sender, RowActivatedArgs args)
        {
            RowActivated();
        }

        void OnButtonReleaseEvent(object? sender, ButtonReleaseEventArgs args)
        {
            if (args.Event.Button == 3 && TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    DirectoryPointerItem = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));
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

                    if (DirectoryPointerItem == null)
                    {
                        if (unigueID.IsEmpty())
                            return;

                        OpenPageElement(false, unigueID);
                    }
                    else
                    {
                        if (CallBack_OnSelectPointer != null)
                            CallBack_OnSelectPointer.Invoke(unigueID);

                        Program.GeneralForm?.CloseNotebookPageToCode(this.Name);
                    }
                }
            }
        }

        /*
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
                        LoadTree();
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
        */

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
                    UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                    if (unigueID.IsEmpty())
                        return;

                    OpenPageElement(false, unigueID);
                }
            }
        }

        void OnRefreshClick(object? sender, EventArgs args)
        {
            LoadTree();
        }

        void OnDeleteClick(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                if (Message.Request(Program.GeneralForm, "Встановити або зняти помітку на видалення?") == ResponseType.Yes)
                {
                    TreePath selectionRow = TreeViewGrid.Selection.GetSelectedRows()[0];

                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, selectionRow);

                    UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                    if (unigueID.IsEmpty())
                        return;

                    SetDeletionLabel(unigueID);

                    DirectoryPointerItem = unigueID;

                    LoadTree();
                }
            }
        }

        void OnCopyClick(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                if (Message.Request(Program.GeneralForm, "Копіювати?") == ResponseType.Yes)
                {
                    TreePath selectionRow = TreeViewGrid.Selection.GetSelectedRows()[0];

                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, selectionRow);

                    UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                    if (unigueID.IsEmpty())
                        return;

                    UnigueID? newUnigueID = Copy(unigueID);

                    if (newUnigueID != null)
                        DirectoryPointerItem = newUnigueID;

                    LoadTree();
                }
            }
        }

        #endregion
    }
}