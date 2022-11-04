using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;

using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class СеріїНоменклатури : VBox
    {
        public СеріїНоменклатури_Pointer? SelectPointerItem { get; set; }
        public СеріїНоменклатури_Pointer? DirectoryPointerItem { get; set; }
        public System.Action<СеріїНоменклатури_Pointer>? CallBack_OnSelectPointer { get; set; }

        TreeView TreeViewGrid;

        public СеріїНоменклатури() : base()
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

            TreeViewGrid = new TreeView(ТабличніСписки.СеріїНоменклатури_Записи.Store);
            ТабличніСписки.СеріїНоменклатури_Записи.AddColumns(TreeViewGrid);

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
            ТабличніСписки.СеріїНоменклатури_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.СеріїНоменклатури_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СеріїНоменклатури_Записи.LoadRecords();

            if (ТабличніСписки.СеріїНоменклатури_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.СеріїНоменклатури_Записи.SelectPath, TreeViewGrid.Columns[0], false);
        }

        #region TreeView

        void OnRowActivated(object sender, RowActivatedArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]);

                UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                SelectPointerItem = new StorageAndTrade_1_0.Довідники.СеріїНоменклатури_Pointer(unigueID);
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
                        СеріїНоменклатури_Objest СеріїНоменклатури_Objest = new СеріїНоменклатури_Objest();
                        if (СеріїНоменклатури_Objest.Read(new UnigueID(uid)))
                        {
                            Program.GeneralForm?.CreateNotebookPage($"Cерійний номер: {СеріїНоменклатури_Objest.Номер}", () =>
                            {
                                СеріїНоменклатури_Елемент page = new СеріїНоменклатури_Елемент
                                {
                                    PageList = this,
                                    IsNew = false,
                                    СеріїНоменклатури_Objest = СеріїНоменклатури_Objest,
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
                            CallBack_OnSelectPointer.Invoke(new СеріїНоменклатури_Pointer(new UnigueID(uid)));

                        Program.GeneralForm?.CloseCurrentPageNotebook();
                    }
                }
            }
        }

        #endregion

        #region ToolBar

        void OnAddClick(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage($"Cерійний номер: *", () =>
            {
                СеріїНоменклатури_Елемент page = new СеріїНоменклатури_Елемент
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

                        СеріїНоменклатури_Objest СеріїНоменклатури_Objest = new СеріїНоменклатури_Objest();
                        if (СеріїНоменклатури_Objest.Read(new UnigueID(uid)))
                            СеріїНоменклатури_Objest.Delete();
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

                        СеріїНоменклатури_Objest СеріїНоменклатури_Objest = new СеріїНоменклатури_Objest();
                        if (СеріїНоменклатури_Objest.Read(new UnigueID(uid)))
                        {
                            СеріїНоменклатури_Objest СеріїНоменклатури_Objest_Новий = СеріїНоменклатури_Objest.Copy();
                            СеріїНоменклатури_Objest_Новий.Номер = Guid.NewGuid().ToString();
                            СеріїНоменклатури_Objest_Новий.Коментар = "Копія - " + СеріїНоменклатури_Objest_Новий.Номер;
                            СеріїНоменклатури_Objest_Новий.Save();

                            SelectPointerItem = СеріїНоменклатури_Objest_Новий.GetDirectoryPointer();
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