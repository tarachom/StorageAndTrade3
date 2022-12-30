using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0;
using Константи = StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.РегістриВідомостей;

namespace StorageAndTrade
{
    class РеалізаціяТоварівТаПослуг_ТабличнаЧастина_Товари : VBox
    {
        public РеалізаціяТоварівТаПослуг_Objest? РеалізаціяТоварівТаПослуг_Objest { get; set; }
        public System.Action? ОбновитиЗначенняДокумента { get; set; }

        #region Записи

        enum Columns
        {
            НомерРядка,
            НоменклатураНазва,
            ХарактеристикаНазва,
            СеріяНазва,
            КількістьУпаковок,
            ПакуванняНазва,
            Кількість,
            ВидЦіниНазва,
            Ціна,
            Сума,
            Скидка,
            ЗамовленняКлієнтаНазва,
            РахунокФактураНазва,
            СкладНазва
        }

        ListStore Store = new ListStore(
            typeof(int),      //НомерРядка
            typeof(string),   //НоменклатураНазва
            typeof(string),   //ХарактеристикаНазва
            typeof(string),   //СеріяНазва
            typeof(int),      //КількістьУпаковок
            typeof(string),   //ПакуванняНазва
            typeof(float),    //Кількість
            typeof(string),   //ВидЦіниНазва
            typeof(float),    //Ціна
            typeof(float),    //Сума
            typeof(float),    //Скидка
            typeof(string),   //ЗамовленняКлієнтаНазва
            typeof(string),   //РахунокФактураНазва
            typeof(string)    //СкладНазва
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
            public СеріїНоменклатури_Pointer Серія { get; set; } = new СеріїНоменклатури_Pointer();
            public string СеріяНазва { get; set; } = "";
            public int КількістьУпаковок { get; set; } = 1;
            public ПакуванняОдиниціВиміру_Pointer Пакування { get; set; } = new ПакуванняОдиниціВиміру_Pointer();
            public string ПакуванняНазва { get; set; } = "";
            public decimal Кількість { get; set; } = 1;
            public ВидиЦін_Pointer ВидЦіни { get; set; } = new ВидиЦін_Pointer();
            public string ВидЦіниНазва { get; set; } = "";
            public decimal Ціна { get; set; }
            public decimal Сума { get; set; }
            public decimal Скидка { get; set; }
            public ЗамовленняКлієнта_Pointer ЗамовленняКлієнта { get; set; } = new ЗамовленняКлієнта_Pointer();
            public string ЗамовленняКлієнтаНазва { get; set; } = "";
            public РахунокФактура_Pointer РахунокФактура { get; set; } = new РахунокФактура_Pointer();
            public string РахунокФактураНазва { get; set; } = "";
            public Склади_Pointer Склад { get; set; } = new Склади_Pointer();
            public string СкладНазва { get; set; } = "";

            public object[] ToArray()
            {
                return new object[]
                {
                    НомерРядка,
                    НоменклатураНазва,
                    ХарактеристикаНазва,
                    СеріяНазва,
                    КількістьУпаковок,
                    ПакуванняНазва,
                    (float)Кількість,
                    ВидЦіниНазва,
                    (float)Ціна,
                    (float)Сума,
                    (float)Скидка,
                    ЗамовленняКлієнтаНазва,
                    РахунокФактураНазва,
                    СкладНазва
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
                    Серія = запис.Серія,
                    СеріяНазва = запис.СеріяНазва,
                    КількістьУпаковок = запис.КількістьУпаковок,
                    Пакування = запис.Пакування,
                    ПакуванняНазва = запис.ПакуванняНазва,
                    Кількість = запис.Кількість,
                    ВидЦіни = запис.ВидЦіни,
                    ВидЦіниНазва = запис.ВидЦіниНазва,
                    Ціна = запис.Ціна,
                    Сума = запис.Сума,
                    Скидка = запис.Скидка,
                    ЗамовленняКлієнта = запис.ЗамовленняКлієнта,
                    ЗамовленняКлієнтаНазва = запис.ЗамовленняКлієнтаНазва,
                    РахунокФактура = запис.РахунокФактура,
                    РахунокФактураНазва = запис.РахунокФактураНазва,
                    Склад = запис.Склад,
                    СкладНазва = запис.СкладНазва
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
            public static void ПісляЗміни_ВидЦіни(Запис запис)
            {
                запис.ВидЦіниНазва = запис.ВидЦіни.GetPresentation();
            }
            public static void ПісляЗміни_ЗамовленняКлієнта(Запис запис)
            {
                запис.ЗамовленняКлієнтаНазва = запис.ЗамовленняКлієнта.GetPresentation();
            }
            public static void ПісляЗміни_РахункуФактури(Запис запис)
            {
                запис.РахунокФактураНазва = запис.РахунокФактура.GetPresentation();
            }
            public static void ПісляЗміни_Склад(Запис запис)
            {
                запис.СкладНазва = запис.Склад.GetPresentation();
            }
            public static void ПісляЗміни_КількістьАбоЦіна(Запис запис)
            {
                запис.Сума = запис.Кількість * запис.Ціна;
            }
            public static void ОтриматиЦіну(Запис запис, РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
            {
                if (запис.Номенклатура.IsEmpty())
                    return;

                if (запис.ВидЦіни.IsEmpty())
                {
                    if (ДокументОбєкт == null)
                        return;
                    else
                    {
                        Склади_Objest? cклади_Objest = ДокументОбєкт.Склад.GetDirectoryObject();
                        if (cклади_Objest != null)
                        {
                            запис.ВидЦіни = cклади_Objest.ВидЦін;
                            Запис.ПісляЗміни_ВидЦіни(запис);
                        }
                        else
                            return;
                    }
                }

                if (запис.Ціна == 0)
                {
                    string query = $@"
SELECT
    ЦіниНоменклатури.{ЦіниНоменклатури_Const.Ціна} AS Ціна
FROM 
    {ЦіниНоменклатури_Const.TABLE} AS ЦіниНоменклатури
WHERE
    ЦіниНоменклатури.{ЦіниНоменклатури_Const.ВидЦіни} = '{запис.ВидЦіни.UnigueID}' AND
    ЦіниНоменклатури.{ЦіниНоменклатури_Const.Номенклатура} = '{запис.Номенклатура.UnigueID}'
ORDER BY 
    ЦіниНоменклатури.period DESC 
LIMIT 1
";
                    Dictionary<string, object> paramQuery = new Dictionary<string, object>();

                    string[] columnsName;
                    List<Dictionary<string, object>> listRow;

                    Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

                    if (listRow.Count > 0)
                        foreach (Dictionary<string, object> row in listRow)
                        {
                            запис.Ціна = (decimal)row["Ціна"];
                            запис.Сума = запис.Кількість * запис.Ціна;
                        }
                }
            }
        }

        #endregion

        TreeView TreeViewGrid;

        public РеалізаціяТоварівТаПослуг_ТабличнаЧастина_Товари() : base()
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
                                Номенклатура page = new Номенклатура(true);

                                page.DirectoryPointerItem = запис.Номенклатура;
                                page.CallBack_OnSelectPointer = (Номенклатура_Pointer selectPointer) =>
                                {
                                    запис.Номенклатура = selectPointer;
                                    Запис.ПісляЗміни_Номенклатура(запис);

                                    if (ОбновитиЗначенняДокумента != null)
                                        ОбновитиЗначенняДокумента.Invoke();

                                    Запис.ОтриматиЦіну(запис, РеалізаціяТоварівТаПослуг_Objest!);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                Program.GeneralForm?.CreateNotebookPage("Вибір - Номенклатура", () => { return page; });

                                page.LoadTree();

                                break;
                            }
                        case Columns.ХарактеристикаНазва:
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

                                Program.GeneralForm?.CreateNotebookPage("Вибір - Характеристика", () => { return page; });

                                page.LoadRecords();

                                break;
                            }
                        case Columns.СеріяНазва:
                            {
                                СеріїНоменклатури page = new СеріїНоменклатури(true);

                                page.DirectoryPointerItem = запис.Серія;
                                page.CallBack_OnSelectPointer = (СеріїНоменклатури_Pointer selectPointer) =>
                                {
                                    запис.Серія = selectPointer;
                                    Запис.ПісляЗміни_Серія(запис);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                Program.GeneralForm?.CreateNotebookPage("Вибір - Серія", () => { return page; });

                                page.LoadRecords();

                                break;
                            }
                        case Columns.ПакуванняНазва:
                            {
                                ПакуванняОдиниціВиміру page = new ПакуванняОдиниціВиміру(true);

                                page.DirectoryPointerItem = запис.Пакування;
                                page.CallBack_OnSelectPointer = (ПакуванняОдиниціВиміру_Pointer selectPointer) =>
                                {
                                    запис.Пакування = selectPointer;
                                    Запис.ПісляЗміни_Пакування(запис);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                Program.GeneralForm?.CreateNotebookPage("Вибір - Пакування", () => { return page; });

                                page.LoadRecords();

                                break;
                            }
                        case Columns.ВидЦіниНазва:
                            {
                                ВидиЦін page = new ВидиЦін(true);

                                page.DirectoryPointerItem = запис.ВидЦіни;
                                page.CallBack_OnSelectPointer = (ВидиЦін_Pointer selectPointer) =>
                                {
                                    запис.ВидЦіни = selectPointer;
                                    Запис.ПісляЗміни_ВидЦіни(запис);

                                    if (ОбновитиЗначенняДокумента != null)
                                        ОбновитиЗначенняДокумента.Invoke();

                                    Запис.ОтриматиЦіну(запис, РеалізаціяТоварівТаПослуг_Objest!);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                Program.GeneralForm?.CreateNotebookPage("Вибір - Вид ціни", () => { return page; });

                                page.LoadRecords();

                                break;
                            }
                        case Columns.ЗамовленняКлієнтаНазва:
                            {
                                ЗамовленняКлієнта page = new ЗамовленняКлієнта(true);

                                page.DocumentPointerItem = запис.ЗамовленняКлієнта;
                                page.CallBack_OnSelectPointer = (ЗамовленняКлієнта_Pointer selectPointer) =>
                                {
                                    запис.ЗамовленняКлієнта = selectPointer;
                                    Запис.ПісляЗміни_ЗамовленняКлієнта(запис);
                                    Store.SetValues(iter, запис.ToArray());
                                };

                                Program.GeneralForm?.CreateNotebookPage("Вибір - Замовлення клієнта", () => { return page; });

                                page.LoadRecords();

                                break;
                            }
                        case Columns.РахунокФактураНазва:
                            {
                                РахунокФактура page = new РахунокФактура(true);

                                page.DocumentPointerItem = запис.РахунокФактура;
                                page.CallBack_OnSelectPointer = (РахунокФактура_Pointer selectPointer) =>
                                {
                                    запис.РахунокФактура = selectPointer;
                                    Запис.ПісляЗміни_РахункуФактури(запис);
                                    Store.SetValues(iter, запис.ToArray());
                                };

                                Program.GeneralForm?.CreateNotebookPage("Вибір - Рахунок фактура", () => { return page; });

                                page.LoadRecords();

                                break;
                            }
                        case Columns.СкладНазва:
                            {
                                Склади page = new Склади(true);

                                page.DirectoryPointerItem = запис.Склад;
                                page.CallBack_OnSelectPointer = (Склади_Pointer selectPointer) =>
                                {
                                    запис.Склад = selectPointer;
                                    Запис.ПісляЗміни_Склад(запис);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                Program.GeneralForm?.CreateNotebookPage("Вибір - Склад", () => { return page; });

                                page.LoadTree();

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

            if (РеалізаціяТоварівТаПослуг_Objest != null)
            {
                Query querySelect = РеалізаціяТоварівТаПослуг_Objest.Товари_TablePart.QuerySelect;
                querySelect.Clear();

                //JOIN 1
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(Номенклатура_Const.TABLE + "." + Номенклатура_Const.Назва, "tovar_name"));
                querySelect.Joins.Add(
                    new Join(Номенклатура_Const.TABLE, РеалізаціяТоварівТаПослуг_Товари_TablePart.Номенклатура, querySelect.Table));

                //JOIN 2
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ПакуванняОдиниціВиміру_Const.TABLE + "." + ПакуванняОдиниціВиміру_Const.Назва, "pak_name"));
                querySelect.Joins.Add(
                    new Join(ПакуванняОдиниціВиміру_Const.TABLE, РеалізаціяТоварівТаПослуг_Товари_TablePart.Пакування, querySelect.Table));

                //JOIN 3
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ХарактеристикиНоменклатури_Const.TABLE + "." + ХарактеристикиНоменклатури_Const.Назва, "xar_name"));
                querySelect.Joins.Add(
                    new Join(ХарактеристикиНоменклатури_Const.TABLE, РеалізаціяТоварівТаПослуг_Товари_TablePart.ХарактеристикаНоменклатури, querySelect.Table));

                //JOIN 4
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(СеріїНоменклатури_Const.TABLE + "." + СеріїНоменклатури_Const.Номер, "seria_number"));
                querySelect.Joins.Add(
                    new Join(СеріїНоменклатури_Const.TABLE, РеалізаціяТоварівТаПослуг_Товари_TablePart.Серія, querySelect.Table));

                //JOIN 5
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ВидиЦін_Const.TABLE + "." + ВидиЦін_Const.Назва, "vidy_cen"));
                querySelect.Joins.Add(
                    new Join(ВидиЦін_Const.TABLE, РеалізаціяТоварівТаПослуг_Товари_TablePart.ВидЦіни, querySelect.Table));

                //JOIN 6
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ЗамовленняКлієнта_Const.TABLE + "." + ЗамовленняКлієнта_Const.Назва, "doc_name"));
                querySelect.Joins.Add(
                    new Join(ЗамовленняКлієнта_Const.TABLE, РеалізаціяТоварівТаПослуг_Товари_TablePart.ЗамовленняКлієнта, querySelect.Table));

                //JOIN 7
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(РахунокФактура_Const.TABLE + "." + РахунокФактура_Const.Назва, "rahunok_name"));
                querySelect.Joins.Add(
                    new Join(РахунокФактура_Const.TABLE, РеалізаціяТоварівТаПослуг_Товари_TablePart.РахунокФактура, querySelect.Table));

                //JOIN 8
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(Склади_Const.TABLE + "." + Склади_Const.Назва, "sklad_name"));
                querySelect.Joins.Add(
                    new Join(Склади_Const.TABLE, РеалізаціяТоварівТаПослуг_Товари_TablePart.Склад, querySelect.Table));

                //ORDER
                querySelect.Order.Add(РеалізаціяТоварівТаПослуг_Товари_TablePart.НомерРядка, SelectOrder.ASC);

                РеалізаціяТоварівТаПослуг_Objest.Товари_TablePart.Read();

                Dictionary<string, Dictionary<string, string>> join = РеалізаціяТоварівТаПослуг_Objest.Товари_TablePart.JoinValue;

                foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record record in РеалізаціяТоварівТаПослуг_Objest.Товари_TablePart.Records)
                {
                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        НомерРядка = record.НомерРядка,
                        Номенклатура = record.Номенклатура,
                        НоменклатураНазва = join[record.UID.ToString()]["tovar_name"],
                        Характеристика = record.ХарактеристикаНоменклатури,
                        ХарактеристикаНазва = join[record.UID.ToString()]["xar_name"],
                        Серія = record.Серія,
                        СеріяНазва = join[record.UID.ToString()]["seria_number"],
                        КількістьУпаковок = record.КількістьУпаковок,
                        Пакування = record.Пакування,
                        ПакуванняНазва = join[record.UID.ToString()]["pak_name"],
                        Кількість = record.Кількість,
                        ВидЦіни = record.ВидЦіни,
                        ВидЦіниНазва = join[record.UID.ToString()]["vidy_cen"],
                        Ціна = record.Ціна,
                        Сума = record.Сума,
                        Скидка = record.Скидка,
                        ЗамовленняКлієнта = record.ЗамовленняКлієнта,
                        ЗамовленняКлієнтаНазва = join[record.UID.ToString()]["doc_name"],
                        РахунокФактура = record.РахунокФактура,
                        РахунокФактураНазва = join[record.UID.ToString()]["rahunok_name"],
                        Склад = record.Склад,
                        СкладНазва = join[record.UID.ToString()]["sklad_name"]
                    };

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }
        }

        public void SaveRecords()
        {
            if (РеалізаціяТоварівТаПослуг_Objest != null)
            {
                РеалізаціяТоварівТаПослуг_Objest.Товари_TablePart.Records.Clear();

                int sequenceNumber = 0;

                foreach (Запис запис in Записи)
                {
                    РеалізаціяТоварівТаПослуг_Товари_TablePart.Record record = new РеалізаціяТоварівТаПослуг_Товари_TablePart.Record();

                    record.UID = запис.ID;
                    record.НомерРядка = ++sequenceNumber;
                    record.Номенклатура = запис.Номенклатура;
                    record.ХарактеристикаНоменклатури = запис.Характеристика;
                    record.Серія = запис.Серія;
                    record.КількістьУпаковок = запис.КількістьУпаковок;
                    record.Пакування = запис.Пакування;
                    record.Кількість = запис.Кількість;
                    record.ВидЦіни = запис.ВидЦіни;
                    record.Ціна = запис.Ціна;
                    record.Сума = запис.Сума;
                    record.Скидка = запис.Скидка;
                    record.ЗамовленняКлієнта = запис.ЗамовленняКлієнта;
                    record.РахунокФактура = запис.РахунокФактура;
                    record.Склад = запис.Склад;

                    РеалізаціяТоварівТаПослуг_Objest.Товари_TablePart.Records.Add(record);
                }

                РеалізаціяТоварівТаПослуг_Objest.Товари_TablePart.Save(true);
            }
        }

        public decimal СумаДокументу()
        {
            decimal Сума = 0;

            foreach (Запис запис in Записи)
                Сума += запис.Сума;

            return Math.Round(Сума, 2);
        }

        #region TreeView

        void AddColumn()
        {
            //НомерРядка
            TreeViewGrid.AppendColumn(new TreeViewColumn("№", new CellRendererText(), "text", (int)Columns.НомерРядка) { MinWidth = 30 });

            //НоменклатураНазва
            {
                TreeViewColumn НоменклатураНазва = new TreeViewColumn("Номенклатура", new CellRendererText(), "text", (int)Columns.НоменклатураНазва) { MinWidth = 300 };
                НоменклатураНазва.Data.Add("Column", Columns.НоменклатураНазва);

                TreeViewGrid.AppendColumn(НоменклатураНазва);
            }

            //ХарактеристикаНазва
            {
                TreeViewColumn ХарактеристикаНазва = new TreeViewColumn("Характеристика", new CellRendererText(), "text", (int)Columns.ХарактеристикаНазва) { MinWidth = 300 };
                ХарактеристикаНазва.Visible = Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const;
                ХарактеристикаНазва.Data.Add("Column", Columns.ХарактеристикаНазва);

                TreeViewGrid.AppendColumn(ХарактеристикаНазва);
            }

            //СеріяНазва
            {
                TreeViewColumn СеріяНазва = new TreeViewColumn("Серія", new CellRendererText(), "text", (int)Columns.СеріяНазва) { MinWidth = 300 };
                СеріяНазва.Visible = Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const;
                СеріяНазва.Data.Add("Column", Columns.СеріяНазва);

                TreeViewGrid.AppendColumn(СеріяНазва);
            }

            //КількістьУпаковок
            {
                CellRendererText КількістьУпаковок = new CellRendererText() { Editable = true };
                КількістьУпаковок.Edited += TextChanged;
                КількістьУпаковок.Data.Add("Column", (int)Columns.КількістьУпаковок);

                TreeViewColumn Column = new TreeViewColumn("Пак", КількістьУпаковок, "text", (int)Columns.КількістьУпаковок) { MinWidth = 50 };
                Column.SetCellDataFunc(КількістьУпаковок, new TreeCellDataFunc(NumericCellDataFunc));
                TreeViewGrid.AppendColumn(Column);
            }

            //ПакуванняНазва
            {
                TreeViewColumn ПакуванняНазва = new TreeViewColumn("Пакування", new CellRendererText(), "text", (int)Columns.ПакуванняНазва) { MinWidth = 100 };
                ПакуванняНазва.Data.Add("Column", Columns.ПакуванняНазва);

                TreeViewGrid.AppendColumn(ПакуванняНазва);
            }

            //Кількість
            {
                CellRendererText Кількість = new CellRendererText() { Editable = true };
                Кількість.Edited += TextChanged;
                Кількість.Data.Add("Column", (int)Columns.Кількість);

                TreeViewColumn Column = new TreeViewColumn("Кількість", Кількість, "text", (int)Columns.Кількість) { MinWidth = 100 };
                Column.SetCellDataFunc(Кількість, new TreeCellDataFunc(NumericCellDataFunc));
                TreeViewGrid.AppendColumn(Column);
            }

            //ВидЦіниНазва
            {
                TreeViewColumn ВидЦіниНазва = new TreeViewColumn("Вид ціни", new CellRendererText(), "text", (int)Columns.ВидЦіниНазва) { MinWidth = 100 };
                ВидЦіниНазва.Data.Add("Column", Columns.ВидЦіниНазва);

                TreeViewGrid.AppendColumn(ВидЦіниНазва);
            }

            //Ціна
            {
                CellRendererText Ціна = new CellRendererText() { Editable = true };
                Ціна.Edited += TextChanged;
                Ціна.Data.Add("Column", (int)Columns.Ціна);

                TreeViewColumn Column = new TreeViewColumn("Ціна", Ціна, "text", (int)Columns.Ціна) { MinWidth = 100 };
                Column.SetCellDataFunc(Ціна, new TreeCellDataFunc(NumericCellDataFunc));
                TreeViewGrid.AppendColumn(Column);
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

            //Скидка
            {
                CellRendererText Скидка = new CellRendererText() { Editable = true };
                Скидка.Edited += TextChanged;
                Скидка.Data.Add("Column", (int)Columns.Скидка);

                TreeViewColumn Column = new TreeViewColumn("Скидка", Скидка, "text", (int)Columns.Скидка) { MinWidth = 100 };
                Column.SetCellDataFunc(Скидка, new TreeCellDataFunc(NumericCellDataFunc));
                TreeViewGrid.AppendColumn(Column);
            }

            //ЗамовленняКлієнтаНазва
            {
                TreeViewColumn ЗамовленняКлієнтаНазва = new TreeViewColumn("Замовлення клієнта", new CellRendererText(), "text", (int)Columns.ЗамовленняКлієнтаНазва) { MinWidth = 300 };
                ЗамовленняКлієнтаНазва.Data.Add("Column", Columns.ЗамовленняКлієнтаНазва);

                TreeViewGrid.AppendColumn(ЗамовленняКлієнтаНазва);
            }

            //РахунокФактураНазва
            {
                TreeViewColumn РахунокФактураНазва = new TreeViewColumn("Рахунок фактура", new CellRendererText(), "text", (int)Columns.РахунокФактураНазва) { MinWidth = 300 };
                РахунокФактураНазва.Data.Add("Column", Columns.РахунокФактураНазва);

                TreeViewGrid.AppendColumn(РахунокФактураНазва);
            }

            //СкладНазва
            {
                TreeViewColumn СкладНазва = new TreeViewColumn("Склад", new CellRendererText(), "text", (int)Columns.СкладНазва) { MinWidth = 300 };
                СкладНазва.Data.Add("Column", Columns.СкладНазва);

                TreeViewGrid.AppendColumn(СкладНазва);
            }

            //Колонка пустишка для заповнення вільного простору
            TreeViewGrid.AppendColumn(new TreeViewColumn());
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
                    case Columns.КількістьУпаковок:
                        {
                            cellText.Text = запис.КількістьУпаковок.ToString();
                            break;
                        }
                    case Columns.Кількість:
                        {
                            cellText.Text = запис.Кількість.ToString();
                            break;
                        }
                    case Columns.Ціна:
                        {
                            cellText.Text = запис.Ціна.ToString();
                            break;
                        }
                    case Columns.Сума:
                        {
                            cellText.Text = запис.Сума.ToString();
                            break;
                        }
                    case Columns.Скидка:
                        {
                            cellText.Text = запис.Скидка.ToString();
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
                    case Columns.КількістьУпаковок:
                        {
                            var (check, value) = Validate.IsInt(args.NewText);
                            if (check)
                                запис.КількістьУпаковок = value;

                            break;
                        }
                    case Columns.Кількість:
                        {
                            var (check, value) = Validate.IsDecimal(args.NewText);
                            if (check)
                            {
                                запис.Кількість = value;
                                Запис.ПісляЗміни_КількістьАбоЦіна(запис);
                            }

                            break;
                        }
                    case Columns.Ціна:
                        {
                            var (check, value) = Validate.IsDecimal(args.NewText);
                            if (check)
                            {
                                запис.Ціна = value;
                                Запис.ПісляЗміни_КількістьАбоЦіна(запис);
                            }

                            break;
                        }
                    case Columns.Сума:
                        {
                            var (check, value) = Validate.IsDecimal(args.NewText);
                            if (check)
                                запис.Сума = value;

                            break;
                        }
                    case Columns.Скидка:
                        {
                            var (check, value) = Validate.IsDecimal(args.NewText);
                            if (check)
                                запис.Скидка = value;

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