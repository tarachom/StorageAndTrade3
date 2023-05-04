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

using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Номенклатура_Папки_Дерево : VBox
    {
        TreeView TreeViewGrid;

        public System.Action? CallBack_RowActivated { get; set; }
        public UnigueID? DirectoryPointerItem { get; set; }
        public System.Action<UnigueID>? CallBack_OnSelectPointer { get; set; }
        public Номенклатура_Папки_Pointer Parent_Pointer { get; set; } = new Номенклатура_Папки_Pointer();

        public string UidOpenFolder { get; set; } = "";

        public Номенклатура_Папки_Дерево() : base()
        {
            BorderWidth = 0;

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeViewGrid = new TreeView();
            Номенклатура_Папки_Дерево_СпільніФункції.AddColumns(TreeViewGrid);

            TreeViewGrid.Selection.Mode = SelectionMode.Single;
            TreeViewGrid.ActivateOnSingleClick = true;
            TreeViewGrid.RowActivated += OnRowActivated;
            TreeViewGrid.ButtonPressEvent += OnButtonPressEvent;
            TreeViewGrid.Model = Номенклатура_Папки_Дерево_СпільніФункції.Store; ;

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

        public void LoadTree()
        {
            if (DirectoryPointerItem != null)
                Parent_Pointer = new Номенклатура_Папки_Pointer(DirectoryPointerItem);

            Номенклатура_Папки_Дерево_СпільніФункції.FillTree(TreeViewGrid, UidOpenFolder, Parent_Pointer);

            OnRowActivated(TreeViewGrid, new RowActivatedArgs());
        }

        void CallBack_LoadRecords(UnigueID? selectPointer)
        {
            if (selectPointer != null)
                Parent_Pointer = new Номенклатура_Папки_Pointer(selectPointer);

            LoadTree();
        }

        void OpenPageElement(bool IsNew, string uid = "")
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{Номенклатура_Папки_Const.FULLNAME} *", () =>
                {
                    Номенклатура_Папки_Елемент page = new Номенклатура_Папки_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadRecords,
                        IsNew = true,
                        РодичДляНового = Parent_Pointer
                    };

                    page.SetValue();

                    return page;
                }, true);
            }
            else
            {
                Номенклатура_Папки_Objest Номенклатура_Папки_Objest = new Номенклатура_Папки_Objest();
                if (Номенклатура_Папки_Objest.Read(new UnigueID(uid)))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{Номенклатура_Папки_Objest.Назва}", () =>
                    {
                        Номенклатура_Папки_Елемент page = new Номенклатура_Папки_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
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

        #region TreeView

        void OnRowActivated(object sender, RowActivatedArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]);

                UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, (int)Номенклатура_Папки_Дерево_СпільніФункції.Columns.ID));

                if (!unigueID.IsEmpty())
                    Parent_Pointer = new Номенклатура_Папки_Pointer(unigueID);
                else
                    Parent_Pointer = new Номенклатура_Папки_Pointer();

                if (CallBack_RowActivated != null)
                    CallBack_RowActivated.Invoke();
            }
        }

        void OnButtonPressEvent(object? sender, ButtonPressEventArgs args)
        {
            if (args.Event.Type == Gdk.EventType.DoubleButtonPress && TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;

                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, (int)Номенклатура_Папки_Дерево_СпільніФункції.Columns.ID);

                    if (DirectoryPointerItem == null)
                    {
                        if (new UnigueID(uid).IsEmpty())
                            return;

                        OpenPageElement(false, uid);
                    }
                    else
                    {
                        if (CallBack_OnSelectPointer != null)
                            CallBack_OnSelectPointer.Invoke(new UnigueID(uid));

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
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, (int)Номенклатура_Папки_Дерево_СпільніФункції.Columns.ID);

                    if (new UnigueID(uid).IsEmpty())
                        return;

                    OpenPageElement(false, uid);
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
                TreePath selectionRow = TreeViewGrid.Selection.GetSelectedRows()[0];

                TreeIter iter;
                TreeViewGrid.Model.GetIter(out iter, selectionRow);

                UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, (int)Номенклатура_Папки_Дерево_СпільніФункції.Columns.ID));

                if (unigueID.IsEmpty())
                    return;

                if (Message.Request(Program.GeneralForm, "Встановити або зняти помітку на видалення?") == ResponseType.Yes)
                {
                    Номенклатура_Папки_Objest Номенклатура_Папки_Objest = new Номенклатура_Папки_Objest();
                    if (Номенклатура_Папки_Objest.Read(unigueID))
                        Номенклатура_Папки_Objest.SetDeletionLabel(!Номенклатура_Папки_Objest.DeletionLabel);
                    else
                        Message.Error(Program.GeneralForm, "Не вдалось прочитати!");

                    LoadTree();
                }
            }
        }

        void OnCopyClick(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath selectionRow = TreeViewGrid.Selection.GetSelectedRows()[0];

                TreeIter iter;
                TreeViewGrid.Model.GetIter(out iter, selectionRow);

                UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, (int)Номенклатура_Папки_Дерево_СпільніФункції.Columns.ID));

                if (unigueID.IsEmpty())
                    return;

                if (Message.Request(Program.GeneralForm, "Копіювати?") == ResponseType.Yes)
                {
                    Номенклатура_Папки_Objest Номенклатура_Папки_Objest = new Номенклатура_Папки_Objest();
                    if (Номенклатура_Папки_Objest.Read(unigueID))
                    {
                        Номенклатура_Папки_Objest Номенклатура_Папки_Objest_Новий = Номенклатура_Папки_Objest.Copy(true);
                        Номенклатура_Папки_Objest_Новий.Save();

                        Parent_Pointer = Номенклатура_Папки_Objest_Новий.GetDirectoryPointer();
                    }
                    else
                        Message.Error(Program.GeneralForm, "Не вдалось прочитати!");

                    LoadTree();
                }
            }
        }

        #endregion
    }
}