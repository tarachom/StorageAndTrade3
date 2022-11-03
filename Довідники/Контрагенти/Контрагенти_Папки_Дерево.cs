using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Контрагенти_Папки_Дерево : VBox
    {
        TreeView TreeViewGrid;
        TreeStore TreeStore = new TreeStore(typeof(string), typeof(string));

        public System.Action? CallBack_RowActivated { get; set; }
        public Контрагенти_Папки_Pointer? DirectoryPointerItem { get; set; }
        public System.Action<Контрагенти_Папки_Pointer>? CallBack_OnSelectPointer { get; set; }
        public Контрагенти_Папки_Pointer Parent_Pointer { get; set; } = new Контрагенти_Папки_Pointer();

        public string UidOpenFolder { get; set; } = "";

        public Контрагенти_Папки_Дерево(bool isVisibleClose = false) : base()
        {
            new VBox(false, 0);
            BorderWidth = 0;

            if (isVisibleClose)
            {
                //Кнопки
                HBox hBoxBotton = new HBox();

                Button bClose = new Button("Закрити");
                bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

                hBoxBotton.PackStart(bClose, false, false, 10);

                PackStart(hBoxBotton, false, false, 10);
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

            ToolButton upButton = new ToolButton(Stock.Add) { Label = "Додати", IsImportant = true };
            upButton.Clicked += OnAddClick;
            toolbar.Add(upButton);

            ToolButton refreshButton = new ToolButton(Stock.Refresh) { Label = "Обновити", IsImportant = true };
            refreshButton.Clicked += OnRefreshClick;
            toolbar.Add(refreshButton);

            ToolButton deleteButton = new ToolButton(Stock.Delete) { Label = "Видалити", IsImportant = true };
            deleteButton.Clicked += OnDeleteClick;
            toolbar.Add(deleteButton);

            ToolButton copyButton = new ToolButton(Stock.Copy) { Label = "Копіювати", IsImportant = true };
            copyButton.Clicked += OnCopyClick;
            toolbar.Add(copyButton);
        }

        public void LoadTree()
        {
            TreeStore.Clear();

            TreeIter rootIter = TreeStore.AppendValues(Guid.Empty.ToString(), " Контрагенти ");

            #region SQL

            string query = $@"
WITH RECURSIVE r AS (
    SELECT 
        uid, 
        {Контрагенти_Папки_Const.Назва}, 
        {Контрагенти_Папки_Const.Родич}, 
        1 AS level 
    FROM {Контрагенти_Папки_Const.TABLE}
    WHERE {Контрагенти_Папки_Const.Родич} = '{Guid.Empty}'";

            if (!String.IsNullOrEmpty(UidOpenFolder))
            {
                query += $@"
AND uid != '{UidOpenFolder}'
";
            }

            query += $@"
    UNION ALL
    SELECT 
        {Контрагенти_Папки_Const.TABLE}.uid, 
        {Контрагенти_Папки_Const.TABLE}.{Контрагенти_Папки_Const.Назва}, 
        {Контрагенти_Папки_Const.TABLE}.{Контрагенти_Папки_Const.Родич}, 
        r.level + 1 AS level
    FROM {Контрагенти_Папки_Const.TABLE}
        JOIN r ON {Контрагенти_Папки_Const.TABLE}.{Контрагенти_Папки_Const.Родич} = r.uid";

            if (!String.IsNullOrEmpty(UidOpenFolder))
            {
                query += $@"
WHERE {Контрагенти_Папки_Const.TABLE}.uid != '{UidOpenFolder}'
";
            }

            query += $@"
)
SELECT 
    uid, 
    {Контрагенти_Папки_Const.Назва}, 
    {Контрагенти_Папки_Const.Родич}, 
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
                TreeIter parentIter = NodeDictionary[Parent_Pointer.UnigueID.ToString()];
                TreePath parentPath = TreeViewGrid.Model.GetPath(parentIter);
                TreeViewGrid.ExpandToPath(parentPath);
                TreeViewGrid.SetCursor(parentPath, TreeViewGrid.Columns[0], false);
            }

            OnRowActivated(TreeViewGrid, new RowActivatedArgs());
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
                    Parent_Pointer = new Контрагенти_Папки_Pointer(unigueID);
                else
                    Parent_Pointer = new Контрагенти_Папки_Pointer();

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
                    UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 0));

                    if (DirectoryPointerItem == null)
                    {
                        if (unigueID.IsEmpty())
                            return;

                        Контрагенти_Папки_Objest Контрагенти_Папки_Objest = new Контрагенти_Папки_Objest();
                        if (Контрагенти_Папки_Objest.Read(unigueID))
                        {
                            Program.GeneralForm?.CreateNotebookPage($"Контрагент Папка: {Контрагенти_Папки_Objest.Назва}", () =>
                            {
                                Контрагенти_Папки_Елемент page = new Контрагенти_Папки_Елемент
                                {
                                    PageList = this,
                                    IsNew = false,
                                    Контрагенти_Папки_Objest = Контрагенти_Папки_Objest
                                };

                                page.SetValue();

                                return page;
                            });
                        }
                        else
                            Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                    }
                    else
                    {
                        if (CallBack_OnSelectPointer != null)
                            CallBack_OnSelectPointer.Invoke(new Контрагенти_Папки_Pointer(unigueID));

                        Program.GeneralForm?.CloseCurrentPageNotebook();
                    }
                }
            }
        }

        #endregion

        #region ToolBar

        void OnAddClick(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage($"Контрагент Папка: *", () =>
            {
                Контрагенти_Папки_Елемент page = new Контрагенти_Папки_Елемент
                {
                    PageList = this,
                    IsNew = true,
                    РодичДляНового = Parent_Pointer
                };

                page.SetValue();

                return page;
            });
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
                    Контрагенти_Папки_Objest Контрагенти_Папки_Objest = new Контрагенти_Папки_Objest();
                    if (Контрагенти_Папки_Objest.Read(unigueID))
                    {
                        Контрагенти_Папки_Objest.Delete();
                        Parent_Pointer = new Контрагенти_Папки_Pointer();
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
                    Контрагенти_Папки_Objest Контрагенти_Папки_Objest = new Контрагенти_Папки_Objest();
                    if (Контрагенти_Папки_Objest.Read(unigueID))
                    {
                        Контрагенти_Папки_Objest Контрагенти_Папки_Objest_Новий = Контрагенти_Папки_Objest.Copy();
                        Контрагенти_Папки_Objest_Новий.Назва += " - Копія";
                        Контрагенти_Папки_Objest_Новий.Код = (++НумераціяДовідників.Контрагенти_Папки_Const).ToString("D6");
                        Контрагенти_Папки_Objest_Новий.Save();

                        Parent_Pointer = Контрагенти_Папки_Objest_Новий.GetDirectoryPointer();
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