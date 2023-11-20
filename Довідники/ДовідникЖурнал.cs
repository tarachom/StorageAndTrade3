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
    public abstract class ДовідникЖурнал : VBox
    {
        public UnigueID? SelectPointerItem { get; set; }
        public UnigueID? DirectoryPointerItem { get; set; }
        public System.Action<UnigueID>? CallBack_OnSelectPointer { get; set; }

        protected Toolbar ToolbarTop = new Toolbar();
        protected HBox HBoxTop = new HBox();
        protected HPaned HPanedTable = new HPaned();
        protected TreeView TreeViewGrid = new TreeView();
        SearchControl2 ПошукПовнотекстовий = new SearchControl2();

        protected string MessageRequestText { get; set; } = "Встановити або зняти помітку на видалення?";

        public ДовідникЖурнал() : base()
        {
            BorderWidth = 0;

            //Кнопки
            PackStart(HBoxTop, false, false, 10);

            //Пошук
            ПошукПовнотекстовий.Select = async (string x) => { await LoadRecords_OnSearch(x); };
            ПошукПовнотекстовий.Clear = async () => { await LoadRecords(); };
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
            //TreeViewGrid.KeyPressEvent += OnKeyPressEvent;
            scrollTree.Add(TreeViewGrid);

            HPanedTable.Pack1(scrollTree, true, true);

            PackStart(HPanedTable, true, true, 0);

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

        #region Virtual Function

        public virtual ValueTask LoadRecords() { return new ValueTask(); }

        protected virtual ValueTask LoadRecords_OnSearch(string searchText) { return new ValueTask(); }

        protected virtual void OpenPageElement(bool IsNew, UnigueID? unigueID = null) { }

        protected virtual ValueTask SetDeletionLabel(UnigueID unigueID) { return new ValueTask(); }

        protected virtual ValueTask<UnigueID?> Copy(UnigueID unigueID) { return new ValueTask<UnigueID?>(); }

        public virtual async void CallBack_LoadRecords(UnigueID? selectPointer)
        {
            SelectPointerItem = selectPointer;
            await LoadRecords();
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

                    if (DirectoryPointerItem == null)
                        OpenPageElement(false, unigueID);
                    else
                    {
                        if (CallBack_OnSelectPointer != null)
                            CallBack_OnSelectPointer.Invoke(unigueID);

                        Program.GeneralForm?.CloseNotebookPageToCode(this.Name);
                    }
                }
            }
        }

        async void OnKeyReleaseEvent(object? sender, KeyReleaseEventArgs args)
        {
            /*
            if (args.Event.State == (Gdk.ModifierType.ControlMask | Gdk.ModifierType.Mod2Mask))
            {
                switch (args.Event.Key)
                {
                    case Gdk.Key.Return:
                        {
                            Console.WriteLine(1);
                            break;
                        }
                }
            }
            else
            */
            switch (args.Event.Key)
            {
                case Gdk.Key.Insert:
                    {
                        OpenPageElement(true);
                        break;
                    }
                case Gdk.Key.F5:
                    {
                        await LoadRecords();
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

        // void OnKeyPressEvent(object? sender, KeyPressEventArgs args)
        // {
        // Console.WriteLine(args.Event.State);
        // Console.WriteLine(Gdk.ModifierType.ControlMask);
        // Console.WriteLine(Gdk.ModifierType.Mod2Mask);
        // Console.WriteLine(Gdk.ModifierType.ControlMask | Gdk.ModifierType.Mod2Mask);
        // Console.WriteLine(args.Event.Key);

        // if (args.Event.State == (Gdk.ModifierType.ControlMask | Gdk.ModifierType.Mod2Mask) && args.Event.Key == Gdk.Key.Return)
        // {
        //     Console.WriteLine(1);
        // }
        //}

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

        async void OnRefreshClick(object? sender, EventArgs args)
        {
            await LoadRecords();
        }

        async void OnDeleteClick(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                if (Message.Request(Program.GeneralForm, MessageRequestText) == ResponseType.Yes)
                {
                    TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                    foreach (TreePath itemPath in selectionRows)
                    {
                        TreeIter iter;
                        TreeViewGrid.Model.GetIter(out iter, itemPath);

                        UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                        await SetDeletionLabel(unigueID);

                        SelectPointerItem = unigueID;
                    }

                    await LoadRecords();
                }
            }
        }

        async void OnCopyClick(object? sender, EventArgs args)
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

                        UnigueID? newUnigueID = await Copy(unigueID);

                        if (newUnigueID != null)
                            SelectPointerItem = newUnigueID;
                    }

                    await LoadRecords();
                }
            }
        }

        #endregion
    }
}