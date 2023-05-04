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

namespace StorageAndTrade
{
    public abstract class ДокументТабличнаЧастина : VBox
    {
        ScrolledWindow scrollTree;
        protected Toolbar ToolbarTop = new Toolbar();
        protected TreeView TreeViewGrid = new TreeView();

        public ДокументТабличнаЧастина() : base()
        {
            CreateToolbar();

            scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;
            TreeViewGrid.ButtonPressEvent += OnButtonPressEvent;
            TreeViewGrid.ButtonReleaseEvent += OnButtonReleaseEvent;
            TreeViewGrid.KeyReleaseEvent += OnKeyReleaseEvent;

            scrollTree.Add(TreeViewGrid);
            PackStart(scrollTree, true, true, 0);

            ShowAll();
        }

        void CreateToolbar()
        {
            PackStart(ToolbarTop, false, false, 0);

            ToolButton upButton = new ToolButton(Stock.Add) { TooltipText = "Додати" };
            upButton.Clicked += OnAddClick;
            ToolbarTop.Add(upButton);

            ToolButton copyButton = new ToolButton(Stock.Copy) { TooltipText = "Копіювати" };
            copyButton.Clicked += OnCopyClick;
            ToolbarTop.Add(copyButton);

            ToolButton deleteButton = new ToolButton(Stock.Delete) { TooltipText = "Видалити" };
            deleteButton.Clicked += OnDeleteClick;
            ToolbarTop.Add(deleteButton);
        }

        public virtual void LoadRecords() { }

        public virtual void SaveRecords() { }

        protected virtual void ButtonSelect(TreeIter iter, int rowNumber, int colNumber, Popover popoverSmallSelect) { }

        protected virtual void ButtonPopupClear(TreeIter iter, int rowNumber, int colNumber) { }

        protected virtual void AddRecord() { }

        protected virtual void CopyRecord(int rowNumber) { }

        protected virtual void DeleteRecord(TreeIter iter, int rowNumber) { }

        #region TreeView

        void OnButtonPressEvent(object sender, ButtonPressEventArgs args)
        {
            if (args.Event.Button == 1 && args.Event.Type == Gdk.EventType.DoubleButtonPress && TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath itemPath;
                TreeViewColumn treeColumn;

                TreeViewGrid.GetCursor(out itemPath, out treeColumn);

                if (treeColumn.Data.ContainsKey("Column"))
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    //Швидкий вибір
                    Gdk.Rectangle rectangleCell = TreeViewGrid.GetCellArea(itemPath, treeColumn);
                    rectangleCell.Offset(-(int)scrollTree.Hadjustment.Value, rectangleCell.Height);
                    Popover popoverSmallSelect = new Popover(TreeViewGrid)
                    {
                        PointingTo = rectangleCell,
                        Position = PositionType.Bottom,
                        BorderWidth = 2
                    };

                    int rowNumber = int.Parse(itemPath.ToString());

                    ButtonSelect(iter, rowNumber, (int)treeColumn.Data["Column"]!, popoverSmallSelect);
                }
            }
        }

        void OnButtonReleaseEvent(object? sender, ButtonReleaseEventArgs args)
        {
            if (args.Event.Button == 3 && TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath itemPath;
                TreeViewColumn treeColumn;

                TreeViewGrid.GetCursor(out itemPath, out treeColumn);

                if (treeColumn.Data.ContainsKey("Column"))
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    int rowNumber = int.Parse(itemPath.ToString());

                    //Меню
                    {
                        Menu menuPopup = new Menu();

                        MenuItem caption = new MenuItem("[ " + treeColumn.Title + " ]");
                        menuPopup.Append(caption);


                        MenuItem select = new MenuItem("Вибрати");
                        menuPopup.Append(select);
                        select.Activated += (object? sender, EventArgs args) =>
                        {
                            Gdk.Rectangle rectangleCell = TreeViewGrid.GetCellArea(itemPath, treeColumn);
                            rectangleCell.Offset(-(int)scrollTree.Hadjustment.Value, rectangleCell.Height);

                            Popover PopoverSmallSelect = new Popover(TreeViewGrid) { Position = PositionType.Bottom, BorderWidth = 2 };
                            PopoverSmallSelect.PointingTo = rectangleCell;

                            ButtonSelect(iter, rowNumber, (int)treeColumn.Data["Column"]!, PopoverSmallSelect);
                        };

                        MenuItem copy = new MenuItem("Копіювати");
                        menuPopup.Append(copy);
                        copy.Activated += (object? sender, EventArgs args) =>
                        {
                            CopyRecord(rowNumber);
                        };

                        MenuItem clear = new MenuItem("Очистити");
                        menuPopup.Append(clear);
                        clear.Activated += (object? sender, EventArgs args) =>
                        {
                            ButtonPopupClear(iter, rowNumber, (int)treeColumn.Data["Column"]!);
                        };

                        MenuItem delete = new MenuItem("Видалити");
                        menuPopup.Append(delete);
                        delete.Activated += (object? sender, EventArgs args) =>
                        {
                            DeleteRecord(iter, rowNumber);
                        };

                        menuPopup.ShowAll();
                        menuPopup.Popup();
                    }

                }
            }
        }

        void OnKeyReleaseEvent(object? sender, KeyReleaseEventArgs args)
        {
            switch (args.Event.Key)
            {
                case Gdk.Key.Insert:
                    {
                        OnAddClick(null, new EventArgs());
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
            AddRecord();
        }

        void OnCopyClick(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    int rowNumber = int.Parse(itemPath.ToString());

                    CopyRecord(rowNumber);
                }
            }
        }

        void OnDeleteClick(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();
                for (int i = selectionRows.Length - 1; i >= 0; i--)
                {
                    TreePath itemPath = selectionRows[i];

                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    int rowNumber = int.Parse(itemPath.ToString());

                    DeleteRecord(iter, rowNumber);
                }
            }
        }

        #endregion

    }
}