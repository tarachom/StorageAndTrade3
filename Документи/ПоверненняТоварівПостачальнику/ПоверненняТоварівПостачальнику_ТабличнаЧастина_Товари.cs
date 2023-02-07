/*
Copyright (C) 2019-2023 TARAKHOMYN YURIY IVANOVYCH
All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

/*
Автор:    Тарахомин Юрій Іванович
Адреса:   Україна, м. Львів
Сайт:     accounting.org.ua
*/

using Gtk;

using AccountingSoftware;

using Константи = StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ПоверненняТоварівПостачальнику_ТабличнаЧастина_Товари : VBox
    {
        public ПоверненняТоварівПостачальнику_Objest? ПоверненняТоварівПостачальнику_Objest { get; set; }

        #region Записи

        enum Columns
        {
            НомерРядка,
            Номенклатура,
            Характеристика,
            Серія,
            КількістьУпаковок,
            Пакування,
            Кількість,
            Ціна,
            Сума,
            ПоступленняТоварівТаПослуг
        }

        ListStore Store = new ListStore(
            typeof(int),      //НомерРядка
            typeof(string),   //Номенклатура
            typeof(string),   //Характеристика
            typeof(string),   //Серія
            typeof(int),      //КількістьУпаковок
            typeof(string),   //Пакування
            typeof(float),    //Кількість
            typeof(float),    //Ціна
            typeof(float),    //Сума
            typeof(string)    //ПоступленняТоварівТаПослуг
        );

        List<Запис> Записи = new List<Запис>();

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            public int НомерРядка { get; set; }
            public Номенклатура_Pointer Номенклатура { get; set; } = new Номенклатура_Pointer();
            public ХарактеристикиНоменклатури_Pointer Характеристика { get; set; } = new ХарактеристикиНоменклатури_Pointer();
            public СеріїНоменклатури_Pointer Серія { get; set; } = new СеріїНоменклатури_Pointer();
            public int КількістьУпаковок { get; set; } = 1;
            public ПакуванняОдиниціВиміру_Pointer Пакування { get; set; } = new ПакуванняОдиниціВиміру_Pointer();
            public decimal Кількість { get; set; } = 1;
            public decimal Ціна { get; set; }
            public decimal Сума { get; set; }
            public ПоступленняТоварівТаПослуг_Pointer ПоступленняТоварівТаПослуг { get; set; } = new ПоступленняТоварівТаПослуг_Pointer();

            public object[] ToArray()
            {
                return new object[]
                {
                    НомерРядка,
                    Номенклатура.Назва,
                    Характеристика.Назва,
                    Серія.Назва,
                    КількістьУпаковок,
                    Пакування.Назва,
                    (float)Кількість,
                    (float)Ціна,
                    (float)Сума,
                    ПоступленняТоварівТаПослуг.Назва
                };
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    Номенклатура = запис.Номенклатура,
                    Характеристика = запис.Характеристика,
                    Серія = запис.Серія,
                    КількістьУпаковок = запис.КількістьУпаковок,
                    Пакування = запис.Пакування,
                    Кількість = запис.Кількість,
                    Ціна = запис.Ціна,
                    Сума = запис.Сума,
                    ПоступленняТоварівТаПослуг = запис.ПоступленняТоварівТаПослуг
                };
            }

            public static void ПісляЗміни_Номенклатура(Запис запис)
            {
                запис.Номенклатура.GetPresentation();

                Номенклатура_Objest? номенклатура_Objest = запис.Номенклатура.GetDirectoryObject();
                if (номенклатура_Objest != null && !номенклатура_Objest.ОдиницяВиміру.IsEmpty())
                {
                    запис.Пакування = номенклатура_Objest.ОдиницяВиміру;
                    Запис.ПісляЗміни_Пакування(запис);
                }

                if (!запис.Пакування.IsEmpty())
                {
                    ПакуванняОдиниціВиміру_Objest? пакуванняОдиниціВиміру_Objest = запис.Пакування.GetDirectoryObject();
                    if (пакуванняОдиниціВиміру_Objest != null)
                        запис.КількістьУпаковок = пакуванняОдиниціВиміру_Objest.КількістьУпаковок;
                    else
                        запис.КількістьУпаковок = 1;
                }
            }
            public static void ПісляЗміни_Характеристика(Запис запис)
            {
                запис.Характеристика.GetPresentation();
            }
            public static void ПісляЗміни_Серія(Запис запис)
            {
                запис.Серія.GetPresentation();
            }
            public static void ПісляЗміни_Пакування(Запис запис)
            {
                запис.Пакування.GetPresentation();
            }
            public static void ПісляЗміни_КількістьАбоЦіна(Запис запис)
            {
                запис.Сума = запис.Кількість * запис.Ціна;
            }
            public static void ПісляЗміни_ПоступленняТоварівТаПослуг(Запис запис)
            {
                запис.ПоступленняТоварівТаПослуг.GetPresentation();
            }
        }

        #endregion

        TreeView TreeViewGrid;

        public ПоверненняТоварівПостачальнику_ТабличнаЧастина_Товари() : base()
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
                        case Columns.Номенклатура:
                            {
                                Номенклатура page = new Номенклатура(true);

                                page.DirectoryPointerItem = запис.Номенклатура;
                                page.CallBack_OnSelectPointer = (Номенклатура_Pointer selectPointer) =>
                                {
                                    запис.Номенклатура = selectPointer;
                                    Запис.ПісляЗміни_Номенклатура(запис);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                Program.GeneralForm?.CreateNotebookPage("Вибір - Номенклатура", () => { return page; }, true);

                                page.LoadTree();

                                break;
                            }
                        case Columns.Характеристика:
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

                                Program.GeneralForm?.CreateNotebookPage("Вибір - Характеристика", () => { return page; }, true);

                                page.LoadRecords();

                                break;
                            }
                        case Columns.Серія:
                            {
                                СеріїНоменклатури page = new СеріїНоменклатури(true);

                                page.DirectoryPointerItem = запис.Серія;
                                page.CallBack_OnSelectPointer = (СеріїНоменклатури_Pointer selectPointer) =>
                                {
                                    запис.Серія = selectPointer;
                                    Запис.ПісляЗміни_Серія(запис);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                Program.GeneralForm?.CreateNotebookPage("Вибір - Серія", () => { return page; }, true);

                                page.LoadRecords();

                                break;
                            }
                        case Columns.Пакування:
                            {
                                ПакуванняОдиниціВиміру page = new ПакуванняОдиниціВиміру(true);

                                page.DirectoryPointerItem = запис.Пакування;
                                page.CallBack_OnSelectPointer = (ПакуванняОдиниціВиміру_Pointer selectPointer) =>
                                {
                                    запис.Пакування = selectPointer;
                                    Запис.ПісляЗміни_Пакування(запис);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                Program.GeneralForm?.CreateNotebookPage("Вибір - Пакування", () => { return page; }, true);

                                page.LoadRecords();

                                break;
                            }
                        case Columns.ПоступленняТоварівТаПослуг:
                            {
                                ПоступленняТоварівТаПослуг page = new ПоступленняТоварівТаПослуг(true);

                                page.DocumentPointerItem = запис.ПоступленняТоварівТаПослуг;
                                page.CallBack_OnSelectPointer = (ПоступленняТоварівТаПослуг_Pointer selectPointer) =>
                                {
                                    запис.ПоступленняТоварівТаПослуг = selectPointer;
                                    Запис.ПісляЗміни_ПоступленняТоварівТаПослуг(запис);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                Program.GeneralForm?.CreateNotebookPage("Вибір - Поступлення товарів та послуг", () => { return page; }, true);

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

            ToolButton upButton = new ToolButton(Stock.Add) { TooltipText = "Додати" };
            upButton.Clicked += OnAddClick;
            toolbar.Add(upButton);

            ToolButton copyButton = new ToolButton(Stock.Copy) { TooltipText = "Копіювати" };
            copyButton.Clicked += OnCopyClick;
            toolbar.Add(copyButton);

            ToolButton deleteButton = new ToolButton(Stock.Delete) { TooltipText = "Видалити" };
            deleteButton.Clicked += OnDeleteClick;
            toolbar.Add(deleteButton);
        }

        public void LoadRecords()
        {
            Store.Clear();
            Записи.Clear();

            if (ПоверненняТоварівПостачальнику_Objest != null)
            {
                Query querySelect = ПоверненняТоварівПостачальнику_Objest.Товари_TablePart.QuerySelect;
                querySelect.Clear();

                //JOIN Номенклатура
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(Номенклатура_Const.TABLE + "." + Номенклатура_Const.Назва, "Номенклатура"));
                querySelect.Joins.Add(
                    new Join(Номенклатура_Const.TABLE, ПоверненняТоварівПостачальнику_Товари_TablePart.Номенклатура, querySelect.Table));

                //JOIN Характеристика
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ХарактеристикиНоменклатури_Const.TABLE + "." + ХарактеристикиНоменклатури_Const.Назва, "Характеристика"));
                querySelect.Joins.Add(
                    new Join(ХарактеристикиНоменклатури_Const.TABLE, ПоверненняТоварівПостачальнику_Товари_TablePart.ХарактеристикаНоменклатури, querySelect.Table));

                //JOIN Серія
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(СеріїНоменклатури_Const.TABLE + "." + СеріїНоменклатури_Const.Номер, "Серія"));
                querySelect.Joins.Add(
                    new Join(СеріїНоменклатури_Const.TABLE, ПоверненняТоварівПостачальнику_Товари_TablePart.Серія, querySelect.Table));

                //JOIN Пакування
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ПакуванняОдиниціВиміру_Const.TABLE + "." + ПакуванняОдиниціВиміру_Const.Назва, "Пакування"));
                querySelect.Joins.Add(
                    new Join(ПакуванняОдиниціВиміру_Const.TABLE, ПоверненняТоварівПостачальнику_Товари_TablePart.Пакування, querySelect.Table));

                //JOIN ДокументПоступлення
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ПоступленняТоварівТаПослуг_Const.TABLE + "." + ПоступленняТоварівТаПослуг_Const.Назва, "ДокументПоступлення"));
                querySelect.Joins.Add(
                    new Join(ПоступленняТоварівТаПослуг_Const.TABLE, ПоверненняТоварівПостачальнику_Товари_TablePart.ДокументПоступлення, querySelect.Table));

                //ORDER
                querySelect.Order.Add(ПоверненняТоварівПостачальнику_Товари_TablePart.НомерРядка, SelectOrder.ASC);

                ПоверненняТоварівПостачальнику_Objest.Товари_TablePart.Read();

                Dictionary<string, Dictionary<string, string>> JoinValue = ПоверненняТоварівПостачальнику_Objest.Товари_TablePart.JoinValue;

                foreach (ПоверненняТоварівПостачальнику_Товари_TablePart.Record record in ПоверненняТоварівПостачальнику_Objest.Товари_TablePart.Records)
                {
                    string uid = record.UID.ToString();

                    record.Номенклатура.Назва = JoinValue[uid]["Номенклатура"];
                    record.ХарактеристикаНоменклатури.Назва = JoinValue[uid]["Характеристика"];
                    record.Серія.Назва = JoinValue[uid]["Серія"];
                    record.Пакування.Назва = JoinValue[uid]["Пакування"];
                    record.ДокументПоступлення.Назва = JoinValue[uid]["ДокументПоступлення"];

                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        НомерРядка = record.НомерРядка,
                        Номенклатура = record.Номенклатура,
                        Характеристика = record.ХарактеристикаНоменклатури,
                        Серія = record.Серія,
                        КількістьУпаковок = record.КількістьУпаковок,
                        Пакування = record.Пакування,
                        Кількість = record.Кількість,
                        Ціна = record.Ціна,
                        Сума = record.Сума,
                        ПоступленняТоварівТаПослуг = record.ДокументПоступлення
                    };

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }
        }

        public void SaveRecords()
        {
            if (ПоверненняТоварівПостачальнику_Objest != null)
            {
                ПоверненняТоварівПостачальнику_Objest.Товари_TablePart.Records.Clear();

                int sequenceNumber = 0;

                foreach (Запис запис in Записи)
                {
                    ПоверненняТоварівПостачальнику_Товари_TablePart.Record record = new ПоверненняТоварівПостачальнику_Товари_TablePart.Record();

                    record.UID = запис.ID;
                    record.НомерРядка = ++sequenceNumber;
                    record.Номенклатура = запис.Номенклатура;
                    record.ХарактеристикаНоменклатури = запис.Характеристика;
                    record.Серія = запис.Серія;
                    record.КількістьУпаковок = запис.КількістьУпаковок;
                    record.Пакування = запис.Пакування;
                    record.Кількість = запис.Кількість;
                    record.Ціна = запис.Ціна;
                    record.Сума = запис.Сума;
                    record.ДокументПоступлення = запис.ПоступленняТоварівТаПослуг;

                    ПоверненняТоварівПостачальнику_Objest.Товари_TablePart.Records.Add(record);
                }

                ПоверненняТоварівПостачальнику_Objest.Товари_TablePart.Save(true);
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

            //Номенклатура
            {
                TreeViewColumn Номенклатура = new TreeViewColumn("Номенклатура", new CellRendererText(), "text", (int)Columns.Номенклатура) { MinWidth = 300 };
                Номенклатура.Data.Add("Column", Columns.Номенклатура);

                TreeViewGrid.AppendColumn(Номенклатура);
            }

            //Характеристика
            {
                TreeViewColumn Характеристика = new TreeViewColumn("Характеристика", new CellRendererText(), "text", (int)Columns.Характеристика) { MinWidth = 300 };
                Характеристика.Visible = Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const;
                Характеристика.Data.Add("Column", Columns.Характеристика);

                TreeViewGrid.AppendColumn(Характеристика);
            }

            //Серія
            {
                TreeViewColumn Серія = new TreeViewColumn("Серія", new CellRendererText(), "text", (int)Columns.Серія) { MinWidth = 300 };
                Серія.Visible = Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const;
                Серія.Data.Add("Column", Columns.Серія);

                TreeViewGrid.AppendColumn(Серія);
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

            //Пакування
            {
                TreeViewColumn Пакування = new TreeViewColumn("Пакування", new CellRendererText(), "text", (int)Columns.Пакування) { MinWidth = 100 };
                Пакування.Data.Add("Column", Columns.Пакування);

                TreeViewGrid.AppendColumn(Пакування);
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

            //Пакування
            {
                TreeViewColumn ПоступленняТоварівТаПослуг = new TreeViewColumn("Документ поступлення", new CellRendererText(), "text", (int)Columns.ПоступленняТоварівТаПослуг) { MinWidth = 100 };
                ПоступленняТоварівТаПослуг.Data.Add("Column", Columns.ПоступленняТоварівТаПослуг);

                TreeViewGrid.AppendColumn(ПоступленняТоварівТаПослуг);
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