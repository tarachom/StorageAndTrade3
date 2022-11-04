using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;

using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class Номенклатура : VBox
    {
        public Номенклатура_Pointer? SelectPointerItem { get; set; }
        public Номенклатура_Pointer? DirectoryPointerItem { get; set; }
        public System.Action<Номенклатура_Pointer>? CallBack_OnSelectPointer { get; set; }

        TreeView TreeViewGrid;
        Номенклатура_Папки_Дерево ДеревоПапок;
        CheckButton checkButtonIsHierarchy = new CheckButton("Враховувати ієрархію папок") { Active = true };
        Entry entrySearch = new Entry() { PlaceholderText = "Пошук", WidthRequest = 300 };

        public Номенклатура() : base()
        {
            new VBox(false, 0);
            BorderWidth = 0;

            //Кнопки
            HBox hBoxBotton = new HBox();

            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

            hBoxBotton.PackStart(bClose, false, false, 10);

            //Враховувати ієрархію папок
            checkButtonIsHierarchy.Clicked += OnCheckButtonIsHierarchyClicked;
            hBoxBotton.PackStart(checkButtonIsHierarchy, false, false, 10);

            PackStart(hBoxBotton, false, false, 10);

            //Пошук
            entrySearch.KeyReleaseEvent += OnEntrySearchKeyRelease;
            hBoxBotton.PackStart(entrySearch, false, false, 10);

            CreateToolbar();

            HPaned hPaned = new HPaned();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.Номенклатура_Записи.Store);
            ТабличніСписки.Номенклатура_Записи.AddColumns(TreeViewGrid);

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;
            TreeViewGrid.RowActivated += OnRowActivated;
            TreeViewGrid.ButtonPressEvent += OnButtonPressEvent;

            scrollTree.Add(TreeViewGrid);

            hPaned.Pack1(scrollTree, true, true);

            ДеревоПапок = new Номенклатура_Папки_Дерево() { WidthRequest = 500 };
            ДеревоПапок.CallBack_RowActivated = LoadRecords;
            hPaned.Pack2(ДеревоПапок, false, true);

            PackStart(hPaned, true, true, 0);

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
            if (DirectoryPointerItem != null)
            {
                UnigueID unigueID = new UnigueID(DirectoryPointerItem.UnigueID.UGuid);
                Номенклатура_Objest? контрагенти_Objest = new Номенклатура_Pointer(unigueID).GetDirectoryObject();
                if (контрагенти_Objest != null)
                    ДеревоПапок.Parent_Pointer = контрагенти_Objest.Папка;
            }

            ДеревоПапок.LoadTree();
        }

        public void LoadRecords()
        {
            ТабличніСписки.Номенклатура_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Номенклатура_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Номенклатура_Записи.Where.Clear();
            if (checkButtonIsHierarchy.Active)
            {
                ТабличніСписки.Номенклатура_Записи.Where.Add(new Where(Номенклатура_Const.Папка, Comparison.EQ, ДеревоПапок.Parent_Pointer.UnigueID.UGuid));
            }

            ТабличніСписки.Номенклатура_Записи.LoadRecords();

            if (ТабличніСписки.Номенклатура_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Номенклатура_Записи.SelectPath, TreeViewGrid.Columns[0], false);
        }

        #region TreeView

        void OnRowActivated(object sender, RowActivatedArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]);

                UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                SelectPointerItem = new StorageAndTrade_1_0.Довідники.Номенклатура_Pointer(unigueID);
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
                        Номенклатура_Objest Номенклатура_Objest = new Номенклатура_Objest();
                        if (Номенклатура_Objest.Read(new UnigueID(uid)))
                        {
                            Program.GeneralForm?.CreateNotebookPage($"Контрагент: {Номенклатура_Objest.Назва}", () =>
                            {
                                Номенклатура_Елемент page = new Номенклатура_Елемент
                                {
                                    PageList = this,
                                    IsNew = false,
                                    Номенклатура_Objest = Номенклатура_Objest,
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
                            CallBack_OnSelectPointer.Invoke(new Номенклатура_Pointer(new UnigueID(uid)));

                        Program.GeneralForm?.CloseCurrentPageNotebook();
                    }
                }
            }
        }

        #endregion

        #region ToolBar

        void OnAddClick(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage($"Контрагент: *", () =>
            {
                Номенклатура_Елемент page = new Номенклатура_Елемент
                {
                    PageList = this,
                    IsNew = true,
                    РодичДляНового = ДеревоПапок.Parent_Pointer
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

                        Номенклатура_Objest Номенклатура_Objest = new Номенклатура_Objest();
                        if (Номенклатура_Objest.Read(new UnigueID(uid)))
                            Номенклатура_Objest.Delete();
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

                        Номенклатура_Objest Номенклатура_Objest = new Номенклатура_Objest();
                        if (Номенклатура_Objest.Read(new UnigueID(uid)))
                        {
                            Номенклатура_Objest Номенклатура_Objest_Новий = Номенклатура_Objest.Copy();
                            Номенклатура_Objest_Новий.Назва += " - Копія";
                            Номенклатура_Objest_Новий.Код = (++НумераціяДовідників.Номенклатура_Const).ToString("D6");
                            Номенклатура_Objest_Новий.Save();

                            SelectPointerItem = Номенклатура_Objest_Новий.GetDirectoryPointer();
                        }
                        else
                            Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                    }

                    LoadRecords();
                }
            }
        }

        #endregion

        #region Controls

        void OnCheckButtonIsHierarchyClicked(object? sender, EventArgs args)
        {
            LoadRecords();
        }

        void OnEntrySearchKeyRelease(object? sender, KeyReleaseEventArgs args)
        {
            if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter)
            {

            }
        }

        #endregion
    }
}