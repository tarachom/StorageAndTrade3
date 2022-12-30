using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ВведенняЗалишків_ТабличнаЧастина_БанківськіРахунки : VBox
    {
        public ВведенняЗалишків_Objest? ВведенняЗалишків_Objest { get; set; }

        #region Записи

        enum Columns
        {
            НомерРядка,
            БанківськийРахунокНазва,
            Сума
        }

        ListStore Store = new ListStore(
            typeof(int),      //НомерРядка
            typeof(string),   //БанківськийРахунокНазва
            typeof(float)     //Сума
        );

        List<Запис> Записи = new List<Запис>();

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            public int НомерРядка { get; set; }
            public БанківськіРахункиОрганізацій_Pointer БанківськийРахунок { get; set; } = new БанківськіРахункиОрганізацій_Pointer();
            public string БанківськийРахунокНазва { get; set; } = "";
            public decimal Сума { get; set; }

            public object[] ToArray()
            {
                return new object[]
                {
                    НомерРядка,
                    БанківськийРахунокНазва,
                    (float)Сума
                };
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    БанківськийРахунок = запис.БанківськийРахунок,
                    БанківськийРахунокНазва = запис.БанківськийРахунокНазва,
                    Сума = запис.Сума
                };
            }

            public static void ПісляЗміни_БанківськийРахунок(Запис запис)
            {
                запис.БанківськийРахунокНазва = запис.БанківськийРахунок.GetPresentation();
            }
        }

        #endregion

        TreeView TreeViewGrid;

        public ВведенняЗалишків_ТабличнаЧастина_БанківськіРахунки() : base()
        {
            new VBox();

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeViewGrid = new TreeView(Store);
            AddColumn();

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;
            TreeViewGrid.ButtonPressEvent += OnButtonPressEvent;

            scrollTree.Add(TreeViewGrid);

            Add(scrollTree);

            ShowAll();
        }

        void OnButtonPressEvent(object sender, ButtonPressEventArgs args)
        {
            if (args.Event.Type == Gdk.EventType.DoubleButtonPress)
            {
                TreePath itemPath;
                TreeViewColumn treeColumn;

                TreeViewGrid.GetCursor(out itemPath, out treeColumn);

                if (treeColumn.Data.ContainsKey("Column"))
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    int rowNumber = int.Parse(itemPath.ToString());
                    Запис запис = Записи[rowNumber];

                    switch ((Columns)treeColumn.Data["Column"]!)
                    {
                        case Columns.БанківськийРахунокНазва:
                            {
                                БанківськіРахункиОрганізацій page = new БанківськіРахункиОрганізацій(true);

                                page.DirectoryPointerItem = запис.БанківськийРахунок;
                                page.CallBack_OnSelectPointer = (БанківськіРахункиОрганізацій_Pointer selectPointer) =>
                                {
                                    запис.БанківськийРахунок = selectPointer;
                                    Запис.ПісляЗміни_БанківськийРахунок(запис);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Банківські рахунки", () => { return page; });

                                page.LoadRecords();

                                break;
                            }
                    }
                }
            }
        }

        void CreateToolbar()
        {
            Toolbar toolbar = new Toolbar();
            PackStart(toolbar, false, false, 0);

            ToolButton upButton = new ToolButton(Stock.Add) { Label = "Додати", IsImportant = true };
            upButton.Clicked += OnAddClick;
            toolbar.Add(upButton);

            ToolButton copyButton = new ToolButton(Stock.Copy) { Label = "Копіювати", IsImportant = true };
            copyButton.Clicked += OnCopyClick;
            toolbar.Add(copyButton);

            ToolButton deleteButton = new ToolButton(Stock.Delete) { Label = "Видалити", IsImportant = true };
            deleteButton.Clicked += OnDeleteClick;
            toolbar.Add(deleteButton);
        }

        public void LoadRecords()
        {
            Store.Clear();
            Записи.Clear();

            if (ВведенняЗалишків_Objest != null)
            {
                Query querySelect = ВведенняЗалишків_Objest.БанківськіРахунки_TablePart.QuerySelect;
                querySelect.Clear();

                //JOIN 1
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(БанківськіРахункиОрганізацій_Const.TABLE + "." + БанківськіРахункиОрганізацій_Const.Назва, "bank_rah_name"));
                querySelect.Joins.Add(
                    new Join(БанківськіРахункиОрганізацій_Const.TABLE, ВведенняЗалишків_БанківськіРахунки_TablePart.БанківськийРахунок, querySelect.Table));

                //ORDER
                querySelect.Order.Add(ВведенняЗалишків_БанківськіРахунки_TablePart.НомерРядка, SelectOrder.ASC);

                ВведенняЗалишків_Objest.БанківськіРахунки_TablePart.Read();

                Dictionary<string, Dictionary<string, string>> join = ВведенняЗалишків_Objest.БанківськіРахунки_TablePart.JoinValue;

                foreach (ВведенняЗалишків_БанківськіРахунки_TablePart.Record record in ВведенняЗалишків_Objest.БанківськіРахунки_TablePart.Records)
                {
                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        НомерРядка = record.НомерРядка,
                        БанківськийРахунок = record.БанківськийРахунок,
                        БанківськийРахунокНазва = join[record.UID.ToString()]["bank_rah_name"],
                        Сума = record.Сума
                    };

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }
        }

        public void SaveRecords()
        {
            if (ВведенняЗалишків_Objest != null)
            {
                ВведенняЗалишків_Objest.БанківськіРахунки_TablePart.Records.Clear();

                int sequenceNumber = 0;

                foreach (Запис запис in Записи)
                {
                    ВведенняЗалишків_БанківськіРахунки_TablePart.Record record = new ВведенняЗалишків_БанківськіРахунки_TablePart.Record();

                    record.UID = запис.ID;
                    record.НомерРядка = ++sequenceNumber;
                    record.БанківськийРахунок = запис.БанківськийРахунок;
                    record.Сума = запис.Сума;

                    ВведенняЗалишків_Objest.БанківськіРахунки_TablePart.Records.Add(record);
                }

                ВведенняЗалишків_Objest.БанківськіРахунки_TablePart.Save(true);
            }
        }

        #region TreeView

        void AddColumn()
        {
            //НомерРядка
            TreeViewGrid.AppendColumn(new TreeViewColumn("№", new CellRendererText(), "text", (int)Columns.НомерРядка) { MinWidth = 30 });

            //БанківськийРахунокНазва
            {
                TreeViewColumn БанківськийРахунокНазва = new TreeViewColumn("Банківський рахунок", new CellRendererText(), "text", (int)Columns.БанківськийРахунокНазва) { MinWidth = 300 };
                БанківськийРахунокНазва.Data.Add("Column", Columns.БанківськийРахунокНазва);

                TreeViewGrid.AppendColumn(БанківськийРахунокНазва);
            }

            //Сума
            {
                CellRendererText Сума = new CellRendererText() { Editable = true };
                Сума.Edited += TextChanged;
                Сума.Data.Add("Column", (int)Columns.Сума);

                TreeViewColumn Column = new TreeViewColumn("Сума", Сума, "text", (int)Columns.Сума) { MinWidth = 100 };
                Column.SetCellDataFunc(Сума, new TreeCellDataFunc(NumericCellDataFunc));
                TreeViewGrid.AppendColumn(Column);
            }
        }

        void NumericCellDataFunc(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            CellRendererText cellText = (CellRendererText)cell;
            if (cellText.Data.Contains("Column"))
            {
                int rowNumber = int.Parse(Store.GetPath(iter).ToString());
                Запис запис = Записи[rowNumber];

                cellText.Foreground = "green";

                switch ((Columns)cellText.Data["Column"]!)
                {
                    case Columns.Сума:
                        {
                            cellText.Text = запис.Сума.ToString();
                            break;
                        }
                }
            }
        }

        void TextChanged(object sender, EditedArgs args)
        {
            CellRenderer cellRender = (CellRenderer)sender;

            if (cellRender.Data.Contains("Column"))
            {
                int ColumnNum = (int)cellRender.Data["Column"]!;

                TreeIter iter;
                Store.GetIterFromString(out iter, args.Path);

                int rowNumber = int.Parse(args.Path);
                Запис запис = Записи[rowNumber];

                switch ((Columns)cellRender.Data["Column"]!)
                {
                    case Columns.Сума:
                        {
                            var (check, value) = Validate.IsDecimal(args.NewText);
                            if (check)
                                запис.Сума = value;

                            break;
                        }
                }

                Store.SetValues(iter, запис.ToArray());
            }
        }

        #endregion

        #region ToolBar

        void OnAddClick(object? sender, EventArgs args)
        {
            Запис запис = new Запис();
            Записи.Add(запис);

            Store.AppendValues(запис.ToArray());
        }

        void OnCopyClick(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    int rowNumber = int.Parse(itemPath.ToString());
                    Запис запис = Записи[rowNumber];

                    Запис записНовий = Запис.Clone(запис);

                    Записи.Add(записНовий);
                    Store.AppendValues(записНовий.ToArray());
                }
            }
        }

        void OnDeleteClick(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();
                for (int i = selectionRows.Length - 1; i >= 0; i--)
                {
                    TreePath itemPath = selectionRows[i];

                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    int rowNumber = int.Parse(itemPath.ToString());
                    Запис запис = Записи[rowNumber];

                    Записи.Remove(запис);
                    Store.Remove(ref iter);
                }
            }
        }

        #endregion

    }
}