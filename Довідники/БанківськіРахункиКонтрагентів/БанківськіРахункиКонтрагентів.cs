using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;

using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class БанківськіРахункиКонтрагентів : VBox
    {
        public FormStorageAndTrade? GeneralForm { get; set; }

        TreeView TreeViewGrid;

        public БанківськіРахункиКонтрагентів() : base()
        {
            new VBox(false, 0);
            BorderWidth = 0;

            //Кнопки
            HBox hBoxBotton = new HBox();

            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { GeneralForm?.CloseCurrentPageNotebook(); };

            hBoxBotton.PackStart(bClose, false, false, 10);

            PackStart(hBoxBotton, false, false, 10);

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
            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.LoadRecords();

            if (ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.SelectPath, TreeViewGrid.Columns[0], false);
        }

        #region TreeView

        void OnRowActivated(object sender, RowActivatedArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]);

                UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.SelectPointerItem =
                    new StorageAndTrade_1_0.Довідники.БанківськіРахункиКонтрагентів_Pointer(unigueID);
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

                    БанківськіРахункиКонтрагентів_Objest БанківськіРахункиКонтрагентів_Objest = new БанківськіРахункиКонтрагентів_Objest();
                    if (БанківськіРахункиКонтрагентів_Objest.Read(new UnigueID(uid)))
                    {
                        GeneralForm?.CreateNotebookPage($"Банківський рахунок контрагента: {БанківськіРахункиКонтрагентів_Objest.Назва}", () =>
                        {
                            БанківськіРахункиКонтрагентів_Елемент page = new БанківськіРахункиКонтрагентів_Елемент
                            {
                                GeneralForm = GeneralForm,
                                CallBack_RefreshList = LoadRecords,
                                IsNew = false,
                                БанківськіРахункиКонтрагентів_Objest = БанківськіРахункиКонтрагентів_Objest
                            };

                            page.SetValue();

                            return page;
                        });
                    }
                    else
                        Message.Error(GeneralForm, "Не вдалось прочитати!");
                }
            }
        }

        #endregion

        #region ToolBar

        void OnAddClick(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage($"Банківський рахунок контрагента: *", () =>
            {
                БанківськіРахункиКонтрагентів_Елемент page = new БанківськіРахункиКонтрагентів_Елемент
                {
                    GeneralForm = GeneralForm,
                    CallBack_RefreshList = LoadRecords,
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
                if (Message.Request(GeneralForm, "Видалити?") == ResponseType.Yes)
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
                            Message.Error(GeneralForm, "Не вдалось прочитати!");
                    }

                    LoadRecords();
                }
            }
        }

        void OnCopyClick(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                if (Message.Request(GeneralForm, "Копіювати?") == ResponseType.Yes)
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

                            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.SelectPointerItem = БанківськіРахункиКонтрагентів_Objest_Новий.GetDirectoryPointer();
                        }
                        else
                            Message.Error(GeneralForm, "Не вдалось прочитати!");
                    }

                    LoadRecords();
                }
            }
        }

        #endregion

    }
}