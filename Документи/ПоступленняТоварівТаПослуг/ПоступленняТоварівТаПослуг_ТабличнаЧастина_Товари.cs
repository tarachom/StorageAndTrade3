using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class ПоступленняТоварівТаПослуг_ТабличнаЧастина_Товари : VBox
    {
        public Організації_Objest? Організації_Objest { get; set; }

        enum Columns
        {
            UID,
            НомерРядка,
            НоменклатураНазва
            // ХарактеристикаНоменклатури,
            // Серія,
            // Пакування,
            // КількістьУпаковок,
            // Кількість,
            // Ціна,
            // Сума,
            // Склад,
            // ЗамовленняПостачальнику
        }

        ListStore Store = new ListStore(
            typeof(string),                //UID
            typeof(int),                   //НомерРядка
            typeof(string)                 //НоменклатураНазва
        );

        #region Записи

        List<Запис> Записи = new List<Запис>();

        private class Запис
        {
            public string ID { get; set; } = Guid.Empty.ToString();
            public int НомерРядка { get; set; }
            public Номенклатура_Pointer Номенклатура { get; set; } = new Номенклатура_Pointer();
            public string НоменклатураНазва { get; set; } = "";
            public ХарактеристикиНоменклатури_Pointer Характеристика { get; set; } = new ХарактеристикиНоменклатури_Pointer();
            public string ХарактеристикаНазва { get; set; } = "";
            public СеріїНоменклатури_Pointer Серія { get; set; } = new СеріїНоменклатури_Pointer();
            public string СеріяНазва { get; set; } = "";
            public int КількістьУпаковок { get; set; } = 1;
            public ПакуванняОдиниціВиміру_Pointer Пакування { get; set; } = new ПакуванняОдиниціВиміру_Pointer();
            public string ПакуванняНазва { get; set; } = "";
            public decimal Кількість { get; set; }
            public decimal Ціна { get; set; }
            public decimal Сума { get; set; }
            public decimal Скидка { get; set; }
            public ЗамовленняПостачальнику_Pointer ЗамовленняПостачальнику { get; set; } = new ЗамовленняПостачальнику_Pointer();
            public string ЗамовленняПостачальникуНазва { get; set; } = "";
            public Склади_Pointer Склад { get; set; } = new Склади_Pointer();
            public string СкладНазва { get; set; } = "";

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty.ToString(),
                    Номенклатура = запис.Номенклатура,
                    НоменклатураНазва = запис.НоменклатураНазва,
                    Характеристика = запис.Характеристика,
                    ХарактеристикаНазва = запис.ХарактеристикаНазва,
                    Серія = запис.Серія,
                    СеріяНазва = запис.СеріяНазва,
                    КількістьУпаковок = запис.КількістьУпаковок,
                    Пакування = запис.Пакування,
                    ПакуванняНазва = запис.ПакуванняНазва,
                    Кількість = запис.Кількість,
                    Ціна = запис.Ціна,
                    Сума = запис.Сума,
                    Скидка = запис.Скидка,
                    ЗамовленняПостачальнику = запис.ЗамовленняПостачальнику,
                    ЗамовленняПостачальникуНазва = запис.ЗамовленняПостачальникуНазва,
                    Склад = запис.Склад,
                    СкладНазва = запис.СкладНазва
                };
            }

            public object[] ToArray()
            {
                return new object[]
                {
                    ID,
                    НомерРядка,
                    НоменклатураНазва
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
                        запис.КількістьУпаковок = пакуванняОдиниціВиміру_Objest.КількістьУпаковок;
                    }
                    else
                    {
                        запис.ПакуванняНазва = "";
                        запис.КількістьУпаковок = 1;
                    }
                }
            }
            public static void ПісляЗміни_Характеристика(Запис запис)
            {
                запис.ХарактеристикаНазва = запис.Характеристика.GetPresentation();
            }
            public static void ПісляЗміни_Серія(Запис запис)
            {
                запис.СеріяНазва = запис.Серія.GetPresentation();
            }
            public static void ПісляЗміни_Пакування(Запис запис)
            {
                запис.ПакуванняНазва = запис.Пакування.GetPresentation();
            }
            public static void ПісляЗміни_ЗамовленняПостачальнику(Запис запис)
            {
                запис.ЗамовленняПостачальникуНазва = запис.ЗамовленняПостачальнику.GetPresentation();
            }
            public static void ПісляЗміни_Склад(Запис запис)
            {
                запис.СкладНазва = запис.Склад.GetPresentation();
            }
        }

        #endregion

        TreeView TreeViewGrid;

        public ПоступленняТоварівТаПослуг_ТабличнаЧастина_Товари() : base()
        {
            new VBox();

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In, HeightRequest = 300 };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeViewGrid = new TreeView(Store);
            AddColumn();

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;
            TreeViewGrid.ButtonPressEvent += OnButtonPressEvent;

            scrollTree.Add(TreeViewGrid);

            PackStart(scrollTree, true, false, 0);

            ShowAll();
        }

        void OnButtonPressEvent(object sender, ButtonPressEventArgs args)
        {
            Console.WriteLine("OnButtonPressEvent");

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
                                    Номенклатура page = new Номенклатура();

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

        // public void LoadRecords()
        // {
        //     Store.Clear();

        //     if (Організації_Objest != null)
        //     {
        //         Організації_Objest.Контакти_TablePart.Read();

        //         foreach (Організації_Контакти_TablePart.Record record in Організації_Objest.Контакти_TablePart.Records)
        //         {
        //             Store.AppendValues(
        //                 record.UID.ToString(),
        //                 record.Тип.ToString(),
        //                 record.Значення,
        //                 record.Телефон,
        //                 record.ЕлектроннаПошта,
        //                 record.Країна,
        //                 record.Область,
        //                 record.Район,
        //                 record.Місто
        //             );
        //         }
        //     }
        // }

        // public void SaveRecords()
        // {
        //     if (Організації_Objest != null)
        //     {
        //         Організації_Objest.Контакти_TablePart.Records.Clear();

        //         TreeIter iter;
        //         if (Store.GetIterFirst(out iter))
        //             do
        //             {
        //                 Організації_Контакти_TablePart.Record record = new Організації_Контакти_TablePart.Record();
        //                 Організації_Objest.Контакти_TablePart.Records.Add(record);

        //                 string uid = (string)Store.GetValue(iter, (int)Columns.UID);

        //                 if (!String.IsNullOrEmpty(uid))
        //                     record.UID = Guid.Parse(uid);

        //                 string type = (string)Store.GetValue(iter, (int)Columns.Тип);
        //                 record.Тип = Enum.Parse<ТипиКонтактноїІнформації>(type);

        //                 record.Значення = Store.GetValue(iter, (int)Columns.Значення)?.ToString() ?? "";
        //                 record.Телефон = Store.GetValue(iter, (int)Columns.Телефон)?.ToString() ?? "";
        //                 record.ЕлектроннаПошта = Store.GetValue(iter, (int)Columns.ЕлектроннаПошта)?.ToString() ?? "";
        //                 record.Країна = Store.GetValue(iter, (int)Columns.Країна)?.ToString() ?? "";
        //                 record.Область = Store.GetValue(iter, (int)Columns.Область)?.ToString() ?? "";
        //                 record.Район = Store.GetValue(iter, (int)Columns.Район)?.ToString() ?? "";
        //                 record.Місто = Store.GetValue(iter, (int)Columns.Місто)?.ToString() ?? "";
        //             }
        //             while (Store.IterNext(ref iter));

        //         Організації_Objest.Контакти_TablePart.Save(true);
        //     }
        // }

        #region TreeView

        void AddColumn()
        {
            TreeViewGrid.AppendColumn(new TreeViewColumn("UID", new CellRendererText(), "text", (int)Columns.UID) { Visible = false });

            //НомерРядка
            TreeViewGrid.AppendColumn(new TreeViewColumn("№", new CellRendererText(), "text", (int)Columns.НомерРядка) { MinWidth = 30 });

            //НоменклатураНазва
            TreeViewColumn НоменклатураНазва = new TreeViewColumn("Номенклатура", new CellRendererText(), "text", (int)Columns.НоменклатураНазва) { MinWidth = 300 };
            НоменклатураНазва.Data.Add("Column", Columns.НоменклатураНазва);

            TreeViewGrid.AppendColumn(НоменклатураНазва);
        }

        void TextChanged(object sender, EditedArgs args)
        {
            CellRenderer cellRender = (CellRenderer)sender;

            if (cellRender.Data.Contains("Column"))
            {
                int ColumnNum = (int)cellRender.Data["Column"]!;

                TreeIter iter;
                if (Store.GetIterFromString(out iter, args.Path))
                    Store.SetValue(iter, ColumnNum, args.NewText);
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