using Gtk;

using AccountingSoftware;

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;
using Константи = StorageAndTrade_1_0.Константи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class ПоступленняТоварівТаПослуг : VBox
    {
        public FormStorageAndTrade? GeneralForm { get; set; }

        TreeView TreeViewGrid;
        ComboBoxText ComboBoxPeriodWhere = new ComboBoxText();

        public ПоступленняТоварівТаПослуг() : base()
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

            TreeViewGrid = new TreeView(ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.Store);
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.AddColumns(TreeViewGrid);

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.RowActivated += OnRowActivated;
            TreeViewGrid.ButtonReleaseEvent += OnButtonReleaseEvent;
            scrollTree.Add(TreeViewGrid);

            PackStart(scrollTree, true, true, 0);

            ShowAll();
        }

        void CreateToolbar()
        {
            Toolbar toolbar = new Toolbar();
            PackStart(toolbar, false, false, 0);

            ToolButton upButton = new ToolButton(Stock.Add) { Label = "Додати", IsImportant = true };
            //upButton.Clicked += OnAddClick;
            toolbar.Add(upButton);

            ToolButton refreshButton = new ToolButton(Stock.Refresh) { Label = "Обновити", IsImportant = true };
            refreshButton.Clicked += OnRefreshClick;
            toolbar.Add(refreshButton);

            ToolButton deleteButton = new ToolButton(Stock.Delete) { Label = "Видалити", IsImportant = true };
            //deleteButton.Clicked += OnDeleteClick;
            toolbar.Add(deleteButton);

            ToolButton copyButton = new ToolButton(Stock.Copy) { Label = "Копіювати", IsImportant = true };
            //copyButton.Clicked += OnCopyClick;
            toolbar.Add(copyButton);

            //Відбір
            ToolItem seperatorOne = new ToolItem();
            seperatorOne.Add(new Separator(Orientation.Vertical));
            toolbar.Add(seperatorOne);

            HBox hBoxPeriodWhere = new HBox();
            hBoxPeriodWhere.PackStart(new Label("Період:"), false, false, 5);

            ComboBoxPeriodWhere = ТабличніСписки.Інтерфейс.СписокВідбірПоПеріоду();
            ComboBoxPeriodWhere.Changed += OnComboBoxPeriodWhereChanged;

            hBoxPeriodWhere.PackStart(ComboBoxPeriodWhere, false, false, 0);

            ToolItem periodWhere = new ToolItem();
            periodWhere.Add(hBoxPeriodWhere);
            toolbar.Add(periodWhere);
        }

        public void SetValue()
        {
            if ((int)Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const != 0)
                ComboBoxPeriodWhere.ActiveId = Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const.ToString();
            else
                ComboBoxPeriodWhere.Active = 0;
        }

        public void LoadRecords()
        {
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.LoadRecords();

            if (ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else
                TreeViewGrid.SetCursor(ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        #region  TreeView

        void OnRowActivated(object sender, RowActivatedArgs args)
        {
            Console.WriteLine(args.Path);
        }

        void OnButtonReleaseEvent(object? sender, ButtonReleaseEventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]);

                UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.SelectPointerItem =
                    new StorageAndTrade_1_0.Документи.ПоступленняТоварівТаПослуг_Pointer(unigueID);
            }
        }

        #endregion

        #region ToolBar

        void OnRefreshClick(object? sender, EventArgs args)
        {
            LoadRecords();
        }

        void OnComboBoxPeriodWhereChanged(object? sender, EventArgs args)
        {
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.ДодатиВідбірПоПеріоду(Enum.Parse<Перелічення.ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }

        #endregion

    }
}