using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;

using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class ПартіяТоварівКомпозит : VBox
    {
        public ПартіяТоварівКомпозит_Pointer? SelectPointerItem { get; set; }
        public ПартіяТоварівКомпозит_Pointer? DirectoryPointerItem { get; set; }
        public System.Action<ПартіяТоварівКомпозит_Pointer>? CallBack_OnSelectPointer { get; set; }

        TreeView TreeViewGrid;

        public ПартіяТоварівКомпозит(bool IsSelectPointer = false) : base()
        {
            new VBox(false, 0);
            BorderWidth = 0;

            //Кнопки
            HBox hBoxBotton = new HBox();

            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

            hBoxBotton.PackStart(bClose, false, false, 10);

            PackStart(hBoxBotton, false, false, 10);

            //Як форма відкрита для вибору
            if (IsSelectPointer)
            {
                Button bEmptyPointer = new Button("Вибрати пустий елемент");
                bEmptyPointer.Clicked += (object? sender, EventArgs args) =>
                {
                    if (CallBack_OnSelectPointer != null)
                        CallBack_OnSelectPointer.Invoke(new ПартіяТоварівКомпозит_Pointer());

                    Program.GeneralForm?.CloseCurrentPageNotebook();
                };

                hBoxBotton.PackStart(bEmptyPointer, false, false, 10);
            }

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Never, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.ПартіяТоварівКомпозит_Записи.Store);
            ТабличніСписки.ПартіяТоварівКомпозит_Записи.AddColumns(TreeViewGrid);

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

            ToolButton refreshButton = new ToolButton(Stock.Refresh) { Label = "Обновити", IsImportant = true };
            refreshButton.Clicked += OnRefreshClick;
            toolbar.Add(refreshButton);

            ToolButton deleteButton = new ToolButton(Stock.Delete) { Label = "Видалити", IsImportant = true };
            deleteButton.Clicked += OnDeleteClick;
            toolbar.Add(deleteButton);
        }

        public void LoadRecords()
        {
            ТабличніСписки.ПартіяТоварівКомпозит_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПартіяТоварівКомпозит_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ПартіяТоварівКомпозит_Записи.LoadRecords();

            if (ТабличніСписки.ПартіяТоварівКомпозит_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПартіяТоварівКомпозит_Записи.SelectPath, TreeViewGrid.Columns[0], false);
        }

        #region TreeView

        void OnRowActivated(object sender, RowActivatedArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]);

                UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                SelectPointerItem = new StorageAndTrade_1_0.Довідники.ПартіяТоварівКомпозит_Pointer(unigueID);
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
                        ПартіяТоварівКомпозит_Objest ПартіяТоварівКомпозит_Objest = new ПартіяТоварівКомпозит_Objest();
                        if (ПартіяТоварівКомпозит_Objest.Read(new UnigueID(uid)))
                        {
                            Program.GeneralForm?.CreateNotebookPage($"Партія: {ПартіяТоварівКомпозит_Objest.Назва}", () =>
                            {
                                ПартіяТоварівКомпозит_Елемент page = new ПартіяТоварівКомпозит_Елемент
                                {
                                    PageList = this,
                                    IsNew = false,
                                    ПартіяТоварівКомпозит_Objest = ПартіяТоварівКомпозит_Objest,
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
                            CallBack_OnSelectPointer.Invoke(new ПартіяТоварівКомпозит_Pointer(new UnigueID(uid)));

                        Program.GeneralForm?.CloseCurrentPageNotebook();
                    }
                }
            }
        }

        #endregion

        #region ToolBar

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

                        ПартіяТоварівКомпозит_Objest ПартіяТоварівКомпозит_Objest = new ПартіяТоварівКомпозит_Objest();
                        if (ПартіяТоварівКомпозит_Objest.Read(new UnigueID(uid)))
                            ПартіяТоварівКомпозит_Objest.Delete();
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