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

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;

using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class БанківськіРахункиКонтрагентів : VBox
    {
        public БанківськіРахункиКонтрагентів_Pointer? SelectPointerItem { get; set; }
        public БанківськіРахункиКонтрагентів_Pointer? DirectoryPointerItem { get; set; }
        public System.Action<БанківськіРахункиКонтрагентів_Pointer>? CallBack_OnSelectPointer { get; set; }

        TreeView TreeViewGrid;

        public БанківськіРахункиКонтрагентів() : base()
        {
            new VBox(false, 0);
            BorderWidth = 0;

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Never, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.Store);
            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.AddColumns(TreeViewGrid);

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;
            TreeViewGrid.RowActivated += OnRowActivated;
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

            ToolButton copyButton = new ToolButton(Stock.Copy) { TooltipText = "Копіювати" };
            copyButton.Clicked += OnCopyClick;
            toolbar.Add(copyButton);

            ToolButton deleteButton = new ToolButton(Stock.Delete) { TooltipText = "Видалити" };
            deleteButton.Clicked += OnDeleteClick;
            toolbar.Add(deleteButton);

            ToolButton refreshButton = new ToolButton(Stock.Refresh) { TooltipText = "Обновити" };
            refreshButton.Clicked += OnRefreshClick;
            toolbar.Add(refreshButton);
        }

        public void LoadRecords()
        {
            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.LoadRecords();

            if (ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.SelectPath, TreeViewGrid.Columns[0], false);
        }

        void OpenPageElement(bool IsNew, string uid = "")
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"Банківський рахунок контрагента: *", () =>
                {
                    БанківськіРахункиКонтрагентів_Елемент page = new БанківськіРахункиКонтрагентів_Елемент
                    {
                        PageList = this,
                        IsNew = true
                    };

                    page.SetValue();

                    return page;
                }, true);
            }
            else
            {
                БанківськіРахункиКонтрагентів_Objest БанківськіРахункиКонтрагентів_Objest = new БанківськіРахункиКонтрагентів_Objest();
                if (БанківськіРахункиКонтрагентів_Objest.Read(new UnigueID(uid)))
                {
                    Program.GeneralForm?.CreateNotebookPage($"Банківський рахунок контрагента: {БанківськіРахункиКонтрагентів_Objest.Назва}", () =>
                    {
                        БанківськіРахункиКонтрагентів_Елемент page = new БанківськіРахункиКонтрагентів_Елемент
                        {
                            PageList = this,
                            IsNew = false,
                            БанківськіРахункиКонтрагентів_Objest = БанківськіРахункиКонтрагентів_Objest
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

        void OnRowActivated(object sender, RowActivatedArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]);

                UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                SelectPointerItem = new StorageAndTrade_1_0.Довідники.БанківськіРахункиКонтрагентів_Pointer(unigueID);
            }
        }

        void OnButtonPressEvent(object? sender, ButtonPressEventArgs args)
        {
            if (args.Event.Type == Gdk.EventType.DoubleButtonPress && TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;

                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    if (DirectoryPointerItem == null)
                        OpenPageElement(false, uid);
                    else
                    {
                        if (CallBack_OnSelectPointer != null)
                            CallBack_OnSelectPointer.Invoke(new БанківськіРахункиКонтрагентів_Pointer(new UnigueID(uid)));

                        Program.GeneralForm?.CloseCurrentPageNotebook();
                    }
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

                        БанківськіРахункиКонтрагентів_Objest БанківськіРахункиКонтрагентів_Objest = new БанківськіРахункиКонтрагентів_Objest();
                        if (БанківськіРахункиКонтрагентів_Objest.Read(new UnigueID(uid)))
                            БанківськіРахункиКонтрагентів_Objest.Delete();
                        else
                            Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
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

                        string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                        БанківськіРахункиКонтрагентів_Objest БанківськіРахункиКонтрагентів_Objest = new БанківськіРахункиКонтрагентів_Objest();
                        if (БанківськіРахункиКонтрагентів_Objest.Read(new UnigueID(uid)))
                        {
                            БанківськіРахункиКонтрагентів_Objest БанківськіРахункиКонтрагентів_Objest_Новий = БанківськіРахункиКонтрагентів_Objest.Copy();
                            БанківськіРахункиКонтрагентів_Objest_Новий.Назва += " - Копія";
                            БанківськіРахункиКонтрагентів_Objest_Новий.Код = (++НумераціяДовідників.БанківськіРахункиКонтрагентів_Const).ToString("D6");
                            БанківськіРахункиКонтрагентів_Objest_Новий.Save();

                            SelectPointerItem = БанківськіРахункиКонтрагентів_Objest_Новий.GetDirectoryPointer();
                        }
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