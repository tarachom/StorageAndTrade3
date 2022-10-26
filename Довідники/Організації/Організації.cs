using Gtk;

using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;

using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class Організації : VBox
    {
        public FormStorageAndTrade? GeneralForm { get; set; }

        TreeView TreeViewGrid;

        public Організації() : base()
        {
            new VBox(false, 0);
            BorderWidth = 0;

            //Кнопки
            HBox hBoxBotton = new HBox();

            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { GeneralForm?.CloseCurrentPageNotebook(); };

            hBoxBotton.PackStart(bClose, false, false, 10);

            PackStart(hBoxBotton, false, false, 10);

            //Список
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
            //copyButton.Clicked += OnCopyClick;
            toolbar.Add(copyButton);

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Never, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.Організації_Записи.Store);
            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.RowActivated += OnRowActivated;
            ТабличніСписки.Організації_Записи.AddColumns(TreeViewGrid);

            scrollTree.Add(TreeViewGrid);

            PackStart(scrollTree, true, true, 0);

            ShowAll();
        }

        public void LoadRecords()
        {
            ТабличніСписки.Організації_Записи.LoadRecords();
        }

        void OnRefreshClick(object? sender, EventArgs args)
        {
            LoadRecords();
        }

        void OnAddClick(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage($"Організація: *", () =>
            {
                Організації_Елемент page = new Організації_Елемент
                {
                    GeneralForm = GeneralForm,
                    IsNew = true,
                    CallBack_RefreshList = LoadRecords
                };

                page.SetValue();

                return page;
            });
        }

        void OnRowActivated(object sender, RowActivatedArgs args)
        {
            TreeIter iter;

            if (TreeViewGrid.Model.GetIter(out iter, args.Path))
            {
                string rowUid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                Організації_Objest Організації_Objest = new Організації_Objest();
                if (Організації_Objest.Read(new UnigueID(rowUid)))
                {
                    GeneralForm?.CreateNotebookPage($"Організація: {Організації_Objest.Назва}", () =>
                    {
                        Організації_Елемент page = new Організації_Елемент
                        {
                            GeneralForm = GeneralForm,
                            IsNew = false,
                            Організації_Objest = Організації_Objest
                        };

                        page.SetValue();

                        return page;
                    });
                }
                else
                    Message.Error(GeneralForm, "Не вдалось прочитати!");
            }
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

                        Організації_Objest Організації_Objest = new Організації_Objest();
                        if (Організації_Objest.Read(new UnigueID(uid)))
                            Організації_Objest.Delete();
                        else
                            Message.Error(GeneralForm, "Не вдалось прочитати!");
                    }

                    LoadRecords();
                }
            }
        }

    }
}