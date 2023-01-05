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

using StorageAndTrade_1_0;
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

        public Номенклатура_Папки_Дерево(bool IsSelectPointer = false) : base()
        {
            new VBox(false, 0);
            BorderWidth = 0;

            //Як форма відкрита для вибору
            if (IsSelectPointer)
            {
                //Кнопки
                HBox hBoxBotton = new HBox();

                Button bClose = new Button("Закрити");
                bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

                hBoxBotton.PackStart(bClose, false, false, 10);

                PackStart(hBoxBotton, false, false, 10);

                Button bEmptyPointer = new Button("Вибрати пустий елемент");
                bEmptyPointer.Clicked += (object? sender, EventArgs args) =>
                {
                    if (CallBack_OnSelectPointer != null)
                        CallBack_OnSelectPointer.Invoke(new Номенклатура_Папки_Pointer());

                    Program.GeneralForm?.CloseCurrentPageNotebook();
                };

                hBoxBotton.PackStart(bEmptyPointer, false, false, 10);
            }

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeViewGrid = new TreeView();
            AddColumns();

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

            ToolButton addButton = new ToolButton(Stock.Add) { Label = "Додати", IsImportant = true };
            addButton.Clicked += OnAddClick;
            toolbar.Add(addButton);

            ToolButton upButton = new ToolButton(Stock.Edit) { Label = "Редагувати", IsImportant = true };
            upButton.Clicked += OnEditClick;
            toolbar.Add(upButton);

            ToolButton copyButton = new ToolButton(Stock.Copy) { Label = "Копіювати", IsImportant = true };
            copyButton.Clicked += OnCopyClick;
            toolbar.Add(copyButton);

            ToolButton deleteButton = new ToolButton(Stock.Delete) { Label = "Видалити", IsImportant = true };
            deleteButton.Clicked += OnDeleteClick;
            toolbar.Add(deleteButton);

            ToolButton refreshButton = new ToolButton(Stock.Refresh) { Label = "Обновити", IsImportant = true };
            refreshButton.Clicked += OnRefreshClick;
            toolbar.Add(refreshButton);
        }

        public void LoadTree()
        {
            TreeStore.Clear();

            TreeIter rootIter = TreeStore.AppendValues(Guid.Empty.ToString(), " Номенклатура ");

            #region SQL

            string query = $@"
WITH RECURSIVE r AS (
    SELECT 
        uid, 
        {Номенклатура_Папки_Const.Назва}, 
        {Номенклатура_Папки_Const.Родич}, 
        1 AS level 
    FROM {Номенклатура_Папки_Const.TABLE}
    WHERE {Номенклатура_Папки_Const.Родич} = '{Guid.Empty}'";

            if (!String.IsNullOrEmpty(UidOpenFolder))
            {
                query += $@"
AND uid != '{UidOpenFolder}'
";
            }

            query += $@"
    UNION ALL
    SELECT 
        {Номенклатура_Папки_Const.TABLE}.uid, 
        {Номенклатура_Папки_Const.TABLE}.{Номенклатура_Папки_Const.Назва}, 
        {Номенклатура_Папки_Const.TABLE}.{Номенклатура_Папки_Const.Родич}, 
        r.level + 1 AS level
    FROM {Номенклатура_Папки_Const.TABLE}
        JOIN r ON {Номенклатура_Папки_Const.TABLE}.{Номенклатура_Папки_Const.Родич} = r.uid";

            if (!String.IsNullOrEmpty(UidOpenFolder))
            {
                query += $@"
WHERE {Номенклатура_Папки_Const.TABLE}.uid != '{UidOpenFolder}'
";
            }

            query += $@"
)
SELECT 
    uid, 
    {Номенклатура_Папки_Const.Назва}, 
    {Номенклатура_Папки_Const.Родич}, 
    level FROM r
ORDER BY level ASC
";

            #endregion

            string[] columnsName;
            List<object[]>? listRow = null;

            Config.Kernel?.DataBase.SelectRequest(query, null, out columnsName, out listRow);

            Dictionary<string, TreeIter> NodeDictionary = new Dictionary<string, TreeIter>();

            if (listRow != null)
                foreach (object[] o in listRow)
                {
                    string uid = o[0]?.ToString() ?? Guid.Empty.ToString();
                    string fieldName = o[1]?.ToString() ?? "";
                    string fieldParent = o[2]?.ToString() ?? Guid.Empty.ToString();
                    int level = (int)o[3];

                    if (level == 1)
                    {
                        TreeIter Iter = TreeStore.AppendValues(rootIter, uid, fieldName);
                        NodeDictionary.Add(uid, Iter);
                    }
                    else
                    {
                        TreeIter parentIter = NodeDictionary[fieldParent];

                        TreeIter Iter = TreeStore.AppendValues(parentIter, uid, fieldName);
                        NodeDictionary.Add(uid, Iter);
                    }
                }

            TreePath rootPath = TreeViewGrid.Model.GetPath(rootIter);
            TreeViewGrid.ExpandToPath(rootPath);

            if (DirectoryPointerItem != null)
                Parent_Pointer = DirectoryPointerItem;

            if (Parent_Pointer.IsEmpty())
            {
                TreeViewGrid.SetCursor(rootPath, TreeViewGrid.Columns[0], false);
            }
            else
            {
                if (NodeDictionary.ContainsKey(Parent_Pointer.UnigueID.ToString()))
                {
                    TreeIter parentIter = NodeDictionary[Parent_Pointer.UnigueID.ToString()];
                    TreePath parentPath = TreeViewGrid.Model.GetPath(parentIter);
                    TreeViewGrid.ExpandToPath(parentPath);
                    TreeViewGrid.SetCursor(parentPath, TreeViewGrid.Columns[0], false);
                }
                else
                {
                    Parent_Pointer = new Номенклатура_Папки_Pointer();
                    TreeViewGrid.SetCursor(rootPath, TreeViewGrid.Columns[0], false);
                }
            }

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
                });
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
                    });
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }

        #region TreeView

        void AddColumns()
        {
            TreeViewGrid.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 0) { Visible = false });
            TreeViewGrid.AppendColumn(new TreeViewColumn("Папки", new CellRendererText(), "text", 1));
        }

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