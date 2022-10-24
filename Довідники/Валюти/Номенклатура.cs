using Gtk;

using AccountingSoftware;
using Довідники = StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Номенклатура : VBox
    {
        TreeView TreeViewGrid;

        public Номенклатура() : base()
        {
            new VBox(false, 0);
            BorderWidth = 0;

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

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Never, PolicyType.Automatic);

            TreeViewGrid = new TreeView(Довідники.ТабличніСписки.Номенклатура_Записи.Store);
            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            //ViewGrid.RowActivated += OnRowActivated;
            Довідники.ТабличніСписки.Номенклатура_Записи.AddColumns(TreeViewGrid);

            scrollTree.Add(TreeViewGrid);

            PackStart(scrollTree, true, true, 0);

            ShowAll();
        }

        public void LoadRecords()
        {
            Довідники.ТабличніСписки.Номенклатура_Записи.LoadRecords();
        }

        void OnRefreshClick(object? sender, EventArgs args)
        {
            Довідники.ТабличніСписки.Номенклатура_Записи.LoadRecords();
        }
    }
}