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

namespace StorageAndTrade
{
    class Номенклатура_Папки_Дерево : VBox
    {
        TreeView TreeViewGrid;
        TreeStore TreeStore = new TreeStore(typeof(string), typeof(string));

        public System.Action? CallBack_RowActivated { get; set; }
        public Номенклатура_Папки_Pointer? DirectoryPointerItem { get; set; }
        public System.Action<Номенклатура_Папки_Pointer>? CallBack_OnSelectPointer { get; set; }
        public Номенклатура_Папки_Pointer Parent_Pointer { get; set; } = new Номенклатура_Папки_Pointer();

        public string UidOpenFolder { get; set; } = "";

        public Номенклатура_Папки_Дерево() : base()
        {
            new VBox(false, 0);
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
            TreeViewGrid.Model = TreeStore;

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
                Parent_Pointer = DirectoryPointerItem;

            Номенклатура_Папки_Дерево_СпільніФункції.FillTree(TreeViewGrid, TreeStore, UidOpenFolder, Parent_Pointer);

            OnRowActivated(TreeViewGrid, new RowActivatedArgs());
        }

        void OpenPageElement(bool IsNew, string uid = "")
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"Номенклатура папка: *", () =>
                {
                    Номенклатура_Папки_Елемент page = new Номенклатура_Папки_Елемент
                    {
                        PageList = this,
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
                    Program.GeneralForm?.CreateNotebookPage($"Номенклатура папка: {Номенклатура_Папки_Objest.Назва}", () =>
                    {
                        Номенклатура_Папки_Елемент page = new Номенклатура_Папки_Елемент
                        {
                            PageList = this,
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

                UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 0));

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
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 0);

                    if (DirectoryPointerItem == null)
                    {
                        if (new UnigueID(uid).IsEmpty())
                            return;

                        OpenPageElement(false, uid);
                    }
                    else
                    {
                        if (CallBack_OnSelectPointer != null)
                            CallBack_OnSelectPointer.Invoke(new Номенклатура_Папки_Pointer(new UnigueID(uid)));

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
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 0);

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

                UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 0));

                if (unigueID.IsEmpty())
                    return;

                if (Message.Request(Program.GeneralForm, "Видалити?") == ResponseType.Yes)
                {
                    Номенклатура_Папки_Objest Номенклатура_Папки_Objest = new Номенклатура_Папки_Objest();
                    if (Номенклатура_Папки_Objest.Read(unigueID))
                    {
                        Номенклатура_Папки_Objest.Delete();
                        Parent_Pointer = new Номенклатура_Папки_Pointer();
                    }
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

                UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 0));

                if (unigueID.IsEmpty())
                    return;

                if (Message.Request(Program.GeneralForm, "Копіювати?") == ResponseType.Yes)
                {
                    Номенклатура_Папки_Objest Номенклатура_Папки_Objest = new Номенклатура_Папки_Objest();
                    if (Номенклатура_Папки_Objest.Read(unigueID))
                    {
                        Номенклатура_Папки_Objest Номенклатура_Папки_Objest_Новий = Номенклатура_Папки_Objest.Copy();
                        Номенклатура_Папки_Objest_Новий.Назва += " - Копія";
                        Номенклатура_Папки_Objest_Новий.Код = (++НумераціяДовідників.Номенклатура_Папки_Const).ToString("D6");
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