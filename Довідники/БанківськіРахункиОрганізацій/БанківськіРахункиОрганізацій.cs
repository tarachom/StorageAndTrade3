using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;

using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class БанківськіРахункиОрганізацій : VBox
    {
        public БанківськіРахункиОрганізацій_Pointer? SelectPointerItem { get; set; }
        public БанківськіРахункиОрганізацій_Pointer? DirectoryPointerItem { get; set; }
        public System.Action<БанківськіРахункиОрганізацій_Pointer>? CallBack_OnSelectPointer { get; set; }

        TreeView TreeViewGrid;

        public БанківськіРахункиОрганізацій() : base()
        {
            new VBox(false, 0);
            BorderWidth = 0;

            //Кнопки
            HBox hBoxBotton = new HBox();

            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

            hBoxBotton.PackStart(bClose, false, false, 10);

            PackStart(hBoxBotton, false, false, 10);

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Never, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.БанківськіРахункиОрганізацій_Записи.Store);
            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.AddColumns(TreeViewGrid);

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

        public void LoadRecords()
        {
            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.LoadRecords();

            if (ТабличніСписки.БанківськіРахункиОрганізацій_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.БанківськіРахункиОрганізацій_Записи.SelectPath, TreeViewGrid.Columns[0], false);
        }

        #region TreeView

        void OnRowActivated(object sender, RowActivatedArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]);

                UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                SelectPointerItem = new StorageAndTrade_1_0.Довідники.БанківськіРахункиОрганізацій_Pointer(unigueID);
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
                    {
                        БанківськіРахункиОрганізацій_Objest БанківськіРахункиОрганізацій_Objest = new БанківськіРахункиОрганізацій_Objest();
                        if (БанківськіРахункиОрганізацій_Objest.Read(new UnigueID(uid)))
                        {
                            Program.GeneralForm?.CreateNotebookPage($"Банківський рахунок контрагента: {БанківськіРахункиОрганізацій_Objest.Назва}", () =>
                            {
                                БанківськіРахункиОрганізацій_Елемент page = new БанківськіРахункиОрганізацій_Елемент
                                {
                                    PageList = this,
                                    IsNew = false,
                                    БанківськіРахункиОрганізацій_Objest = БанківськіРахункиОрганізацій_Objest
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
                            CallBack_OnSelectPointer.Invoke(new БанківськіРахункиОрганізацій_Pointer(new UnigueID(uid)));

                        Program.GeneralForm?.CloseCurrentPageNotebook();
                    }
                }
            }
        }

        #endregion

        #region ToolBar

        void OnAddClick(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage($"Банківський рахунок контрагента: *", () =>
            {
                БанківськіРахункиОрганізацій_Елемент page = new БанківськіРахункиОрганізацій_Елемент
                {
                    PageList = this,
                    IsNew = true
                };

                page.SetValue();

                return page;
            });
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

                        БанківськіРахункиОрганізацій_Objest БанківськіРахункиОрганізацій_Objest = new БанківськіРахункиОрганізацій_Objest();
                        if (БанківськіРахункиОрганізацій_Objest.Read(new UnigueID(uid)))
                            БанківськіРахункиОрганізацій_Objest.Delete();
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

                        БанківськіРахункиОрганізацій_Objest БанківськіРахункиОрганізацій_Objest = new БанківськіРахункиОрганізацій_Objest();
                        if (БанківськіРахункиОрганізацій_Objest.Read(new UnigueID(uid)))
                        {
                            БанківськіРахункиОрганізацій_Objest БанківськіРахункиОрганізацій_Objest_Новий = БанківськіРахункиОрганізацій_Objest.Copy();
                            БанківськіРахункиОрганізацій_Objest_Новий.Назва += " - Копія";
                            БанківськіРахункиОрганізацій_Objest_Новий.Код = (++НумераціяДовідників.БанківськіРахункиОрганізацій_Const).ToString("D6");
                            БанківськіРахункиОрганізацій_Objest_Новий.Save();

                            SelectPointerItem = БанківськіРахункиОрганізацій_Objest_Новий.GetDirectoryPointer();
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