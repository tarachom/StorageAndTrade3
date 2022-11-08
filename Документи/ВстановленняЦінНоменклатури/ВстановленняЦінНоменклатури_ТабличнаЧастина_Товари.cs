using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари : VBox
    {
        public ВстановленняЦінНоменклатури_Objest? ВстановленняЦінНоменклатури_Objest { get; set; }

        #region Записи

        enum Columns
        {
            НомерРядка,
            НоменклатураНазва,
            ХарактеристикаНазва,
            ПакуванняНазва,
            ВидЦіниНазва,
            Ціна
        }

        ListStore Store = new ListStore(
            typeof(int),      //НомерРядка
            typeof(string),   //НоменклатураНазва
            typeof(string),   //ХарактеристикаНазва
            typeof(string),   //ПакуванняНазва
            typeof(string),   //ВидЦіни
            typeof(float)     //Ціна
        );

        List<Запис> Записи = new List<Запис>();

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            public int НомерРядка { get; set; }
            public Номенклатура_Pointer Номенклатура { get; set; } = new Номенклатура_Pointer();
            public string НоменклатураНазва { get; set; } = "";
            public ХарактеристикиНоменклатури_Pointer Характеристика { get; set; } = new ХарактеристикиНоменклатури_Pointer();
            public string ХарактеристикаНазва { get; set; } = "";
            public ПакуванняОдиниціВиміру_Pointer Пакування { get; set; } = new ПакуванняОдиниціВиміру_Pointer();
            public string ПакуванняНазва { get; set; } = "";
            public ВидиЦін_Pointer ВидЦіни { get; set; } = new ВидиЦін_Pointer();
            public string ВидЦіниНазва { get; set; } = "";
            public decimal Ціна { get; set; }

            public object[] ToArray()
            {
                return new object[]
                {
                    НомерРядка,
                    НоменклатураНазва,
                    ХарактеристикаНазва,
                    ПакуванняНазва,
                    ВидЦіниНазва,
                    (float)Ціна
                };
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    Номенклатура = запис.Номенклатура,
                    НоменклатураНазва = запис.НоменклатураНазва,
                    Характеристика = запис.Характеристика,
                    ХарактеристикаНазва = запис.ХарактеристикаНазва,
                    Пакування = запис.Пакування,
                    ПакуванняНазва = запис.ПакуванняНазва,
                    ВидЦіни = запис.ВидЦіни,
                    ВидЦіниНазва = запис.ВидЦіниНазва,
                    Ціна = запис.Ціна
                };
            }

            public static void ПісляЗміни_Номенклатура(Запис запис)
            {
                if (запис.Номенклатура.IsEmpty())
                {
                    запис.НоменклатураНазва = "";
                    return;
                }

                Номенклатура_Objest? номенклатура_Objest = запис.Номенклатура.GetDirectoryObject();
                if (номенклатура_Objest != null)
                {
                    запис.НоменклатураНазва = номенклатура_Objest.Назва;

                    if (!номенклатура_Objest.ОдиницяВиміру.IsEmpty())
                        запис.Пакування = номенклатура_Objest.ОдиницяВиміру;
                }
                else
                {
                    запис.НоменклатураНазва = "";
                    запис.Пакування = new ПакуванняОдиниціВиміру_Pointer();
                }

                if (!запис.Пакування.IsEmpty())
                {
                    ПакуванняОдиниціВиміру_Objest? пакуванняОдиниціВиміру_Objest = запис.Пакування.GetDirectoryObject();
                    if (пакуванняОдиниціВиміру_Objest != null)
                    {
                        запис.ПакуванняНазва = пакуванняОдиниціВиміру_Objest.Назва;
                    }
                    else
                    {
                        запис.ПакуванняНазва = "";
                    }
                }
            }
            public static void ПісляЗміни_Характеристика(Запис запис)
            {
                запис.ХарактеристикаНазва = запис.Характеристика.GetPresentation();
            }
            public static void ПісляЗміни_Пакування(Запис запис)
            {
                запис.ПакуванняНазва = запис.Пакування.GetPresentation();
            }
            public static void ПісляЗміни_ВидЦіни(Запис запис)
            {
                запис.ВидЦіниНазва = запис.ВидЦіни.GetPresentation();
            }
        }

        #endregion

        TreeView TreeViewGrid;

        public ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари() : base()
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
                        case Columns.НоменклатураНазва:
                            {
                                Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Номенклатура", () =>
                                {
                                    Номенклатура page = new Номенклатура(true);

                                    page.DirectoryPointerItem = запис.Номенклатура;
                                    page.CallBack_OnSelectPointer = (Номенклатура_Pointer selectPointer) =>
                                    {
                                        запис.Номенклатура = selectPointer;
                                        Запис.ПісляЗміни_Номенклатура(запис);

                                        Store.SetValues(iter, запис.ToArray());
                                    };

                                    page.LoadTree();

                                    return page;
                                });

                                break;
                            }
                        case Columns.ХарактеристикаНазва:
                            {
                                Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Характеристика", () =>
                                {
                                    ХарактеристикиНоменклатури page = new ХарактеристикиНоменклатури(true);

                                    page.НоменклатураВласник.Pointer = запис.Номенклатура;
                                    page.DirectoryPointerItem = запис.Характеристика;
                                    page.CallBack_OnSelectPointer = (ХарактеристикиНоменклатури_Pointer selectPointer) =>
                                    {
                                        запис.Характеристика = selectPointer;
                                        Запис.ПісляЗміни_Характеристика(запис);

                                        Store.SetValues(iter, запис.ToArray());
                                    };

                                    page.LoadRecords();

                                    return page;
                                });

                                break;
                            }
                        case Columns.ПакуванняНазва:
                            {
                                Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Пакування", () =>
                                {
                                    ПакуванняОдиниціВиміру page = new ПакуванняОдиниціВиміру(true);

                                    page.DirectoryPointerItem = запис.Пакування;
                                    page.CallBack_OnSelectPointer = (ПакуванняОдиниціВиміру_Pointer selectPointer) =>
                                    {
                                        запис.Пакування = selectPointer;
                                        Запис.ПісляЗміни_Пакування(запис);

                                        Store.SetValues(iter, запис.ToArray());
                                    };

                                    page.LoadRecords();

                                    return page;
                                });

                                break;
                            }
                        case Columns.ВидЦіниНазва:
                            {
                                Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Види цін", () =>
                                {
                                    ВидиЦін page = new ВидиЦін(true);

                                    page.DirectoryPointerItem = запис.ВидЦіни;
                                    page.CallBack_OnSelectPointer = (ВидиЦін_Pointer selectPointer) =>
                                    {
                                        запис.ВидЦіни = selectPointer;
                                        Запис.ПісляЗміни_ВидЦіни(запис);

                                        Store.SetValues(iter, запис.ToArray());
                                    };

                                    page.LoadRecords();

                                    return page;
                                });

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

            if (ВстановленняЦінНоменклатури_Objest != null)
            {
                Query querySelect = ВстановленняЦінНоменклатури_Objest.Товари_TablePart.QuerySelect;
                querySelect.Clear();

                //JOIN 1
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(Номенклатура_Const.TABLE + "." + Номенклатура_Const.Назва, "tovar_name"));
                querySelect.Joins.Add(
                    new Join(Номенклатура_Const.TABLE, ВстановленняЦінНоменклатури_Товари_TablePart.Номенклатура, querySelect.Table));

                //JOIN 2
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ПакуванняОдиниціВиміру_Const.TABLE + "." + ПакуванняОдиниціВиміру_Const.Назва, "pak_name"));
                querySelect.Joins.Add(
                    new Join(ПакуванняОдиниціВиміру_Const.TABLE, ВстановленняЦінНоменклатури_Товари_TablePart.Пакування, querySelect.Table));

                //JOIN 3
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ХарактеристикиНоменклатури_Const.TABLE + "." + ХарактеристикиНоменклатури_Const.Назва, "xar_name"));
                querySelect.Joins.Add(
                    new Join(ХарактеристикиНоменклатури_Const.TABLE, ВстановленняЦінНоменклатури_Товари_TablePart.ХарактеристикаНоменклатури, querySelect.Table));

                //JOIN 4
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ВидиЦін_Const.TABLE + "." + ВидиЦін_Const.Назва, "vidcen_name"));
                querySelect.Joins.Add(
                    new Join(ВидиЦін_Const.TABLE, ВстановленняЦінНоменклатури_Товари_TablePart.ВидЦіни, querySelect.Table));

                //ORDER
                querySelect.Order.Add(ВстановленняЦінНоменклатури_Товари_TablePart.НомерРядка, SelectOrder.ASC);

                ВстановленняЦінНоменклатури_Objest.Товари_TablePart.Read();

                Dictionary<string, Dictionary<string, string>> join = ВстановленняЦінНоменклатури_Objest.Товари_TablePart.JoinValue;

                foreach (ВстановленняЦінНоменклатури_Товари_TablePart.Record record in ВстановленняЦінНоменклатури_Objest.Товари_TablePart.Records)
                {
                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        НомерРядка = record.НомерРядка,
                        Номенклатура = record.Номенклатура,
                        НоменклатураНазва = join[record.UID.ToString()]["tovar_name"],
                        Характеристика = record.ХарактеристикаНоменклатури,
                        ХарактеристикаНазва = join[record.UID.ToString()]["xar_name"],
                        Пакування = record.Пакування,
                        ПакуванняНазва = join[record.UID.ToString()]["pak_name"],
                        ВидЦіни = record.ВидЦіни,
                        ВидЦіниНазва = join[record.UID.ToString()]["vidcen_name"],
                        Ціна = Math.Round(record.Ціна, 2)
                    };

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }
        }

        public void SaveRecords()
        {
            if (ВстановленняЦінНоменклатури_Objest != null)
            {
                ВстановленняЦінНоменклатури_Objest.Товари_TablePart.Records.Clear();

                int sequenceNumber = 0;

                foreach (Запис запис in Записи)
                {
                    ВстановленняЦінНоменклатури_Товари_TablePart.Record record = new ВстановленняЦінНоменклатури_Товари_TablePart.Record();

                    record.UID = запис.ID;
                    record.НомерРядка = ++sequenceNumber;
                    record.Номенклатура = запис.Номенклатура;
                    record.ХарактеристикаНоменклатури = запис.Характеристика;
                    record.Пакування = запис.Пакування;
                    record.ВидЦіни = запис.ВидЦіни;
                    record.Ціна = запис.Ціна;

                    ВстановленняЦінНоменклатури_Objest.Товари_TablePart.Records.Add(record);
                }

                ВстановленняЦінНоменклатури_Objest.Товари_TablePart.Save(true);
            }
        }

        #region TreeView

        void AddColumn()
        {
            //НомерРядка
            TreeViewGrid.AppendColumn(new TreeViewColumn("№", new CellRendererText(), "text", (int)Columns.НомерРядка) { MinWidth = 30 });

            //НоменклатураНазва
            TreeViewColumn НоменклатураНазва = new TreeViewColumn("Номенклатура", new CellRendererText(), "text", (int)Columns.НоменклатураНазва) { MinWidth = 300 };
            НоменклатураНазва.Data.Add("Column", Columns.НоменклатураНазва);

            TreeViewGrid.AppendColumn(НоменклатураНазва);

            //ХарактеристикаНазва
            TreeViewColumn ХарактеристикаНазва = new TreeViewColumn("Характеристика", new CellRendererText(), "text", (int)Columns.ХарактеристикаНазва) { MinWidth = 300 };
            ХарактеристикаНазва.Data.Add("Column", Columns.ХарактеристикаНазва);

            TreeViewGrid.AppendColumn(ХарактеристикаНазва);

            //ПакуванняНазва
            TreeViewColumn ПакуванняНазва = new TreeViewColumn("Пакування", new CellRendererText(), "text", (int)Columns.ПакуванняНазва) { MinWidth = 100 };
            ПакуванняНазва.Data.Add("Column", Columns.ПакуванняНазва);

            TreeViewGrid.AppendColumn(ПакуванняНазва);

            //ВидЦіниНазва
            TreeViewColumn ВидЦіниНазва = new TreeViewColumn("Вид ціни", new CellRendererText(), "text", (int)Columns.ВидЦіниНазва) { MinWidth = 100 };
            ВидЦіниНазва.Data.Add("Column", Columns.ВидЦіниНазва);

            TreeViewGrid.AppendColumn(ВидЦіниНазва);

            //Ціна
            CellRendererText Ціна = new CellRendererText() { Editable = true };
            Ціна.Edited += TextChanged;
            Ціна.Data.Add("Column", (int)Columns.Ціна);

            TreeViewColumn Column_Ціна = new TreeViewColumn("Ціна", Ціна, "text", (int)Columns.Ціна) { MinWidth = 100 };
            //Column_Ціна.SetCellDataFunc(Ціна, new TreeCellDataFunc(RenderArtistName));
            //Column_Ціна.SetCellDataFunc(Ціна, new CellLayoutDataFunc(RenderArtistName2));

            TreeViewGrid.AppendColumn(Column_Ціна);
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
                    case Columns.Ціна:
                        {
                            запис.Ціна = decimal.Parse(args.NewText);
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