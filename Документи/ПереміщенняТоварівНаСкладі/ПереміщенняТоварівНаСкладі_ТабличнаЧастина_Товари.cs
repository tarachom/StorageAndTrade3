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
    class ПереміщенняТоварівНаСкладі_ТабличнаЧастина_Товари : VBox
    {
        ScrolledWindow scrollTree;
        public ПереміщенняТоварівНаСкладі_Objest? ПереміщенняТоварівНаСкладі_Objest { get; set; }
        public System.Action? ОбновитиЗначенняДокумента { get; set; }

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
            КоміркаВідправник,
            КоміркаОтримувач
        }

        ListStore Store = new ListStore(
            typeof(int),      //НомерРядка
            typeof(string),   //Номенклатура
            typeof(string),   //Характеристика
            typeof(string),   //Серія
            typeof(int),      //КількістьУпаковок
            typeof(string),   //Пакування
            typeof(float),    //Кількість
            typeof(string),   //КоміркаВідправник
            typeof(string)    //КоміркаОтримувач
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
            public СкладськіКомірки_Pointer КоміркаВідправник { get; set; } = new СкладськіКомірки_Pointer();
            public СкладськіКомірки_Pointer КоміркаОтримувач { get; set; } = new СкладськіКомірки_Pointer();

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
                    КоміркаВідправник.Назва,
                    КоміркаОтримувач.Назва
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
                    КоміркаВідправник = запис.КоміркаВідправник,
                    КоміркаОтримувач = запис.КоміркаОтримувач
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
            public static void ПісляЗміни_КоміркаВідправник(Запис запис)
            {
                запис.КоміркаВідправник.GetPresentation();
            }
            public static void ПісляЗміни_КоміркаОтримувач(Запис запис)
            {
                запис.КоміркаОтримувач.GetPresentation();
            }
        }

        #endregion

        TreeView TreeViewGrid;

        public ПереміщенняТоварівНаСкладі_ТабличнаЧастина_Товари() : base()
        {
            CreateToolbar();

            scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeViewGrid = new TreeView(Store);
            AddColumn();

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;
            TreeViewGrid.ButtonPressEvent += OnButtonPressEvent;
            TreeViewGrid.KeyReleaseEvent += OnKeyReleaseEvent;

            scrollTree.Add(TreeViewGrid);

            Add(scrollTree);

            ShowAll();
        }

        void OnButtonPressEvent(object sender, ButtonPressEventArgs args)
        {
            if (args.Event.Type == Gdk.EventType.DoubleButtonPress && TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath itemPath;
                TreeViewColumn treeColumn;

                TreeViewGrid.GetCursor(out itemPath, out treeColumn);

                if (treeColumn.Data.ContainsKey("Column"))
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    Gdk.Rectangle rectangleCell = TreeViewGrid.GetCellArea(itemPath, treeColumn);
                    rectangleCell.Offset(-(int)scrollTree.Hadjustment.Value, rectangleCell.Height);

                    Popover PopoverSmallSelect = new Popover(TreeViewGrid) { Position = PositionType.Bottom, BorderWidth = 2 };
                    PopoverSmallSelect.PointingTo = rectangleCell;

                    int rowNumber = int.Parse(itemPath.ToString());
                    Запис запис = Записи[rowNumber];

                    switch ((Columns)treeColumn.Data["Column"]!)
                    {
                        case Columns.Номенклатура:
                            {
                                Номенклатура_ШвидкийВибір page = new Номенклатура_ШвидкийВибір() { PopoverParent = PopoverSmallSelect, DirectoryPointerItem = запис.Номенклатура };
                                page.CallBack_OnSelectPointer = (Номенклатура_Pointer selectPointer) =>
                                {
                                    запис.Номенклатура = selectPointer;
                                    Запис.ПісляЗміни_Номенклатура(запис);

                                    if (ОбновитиЗначенняДокумента != null)
                                        ОбновитиЗначенняДокумента.Invoke();

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                PopoverSmallSelect.Add(page);
                                PopoverSmallSelect.ShowAll();

                                page.LoadRecords();
                                break;
                            }
                        case Columns.Характеристика:
                            {
                                ХарактеристикиНоменклатури_ШвидкийВибір page = new ХарактеристикиНоменклатури_ШвидкийВибір() { PopoverParent = PopoverSmallSelect, DirectoryPointerItem = запис.Характеристика };

                                page.НоменклатураВласник.Pointer = запис.Номенклатура;
                                page.CallBack_OnSelectPointer = (ХарактеристикиНоменклатури_Pointer selectPointer) =>
                                {
                                    запис.Характеристика = selectPointer;
                                    Запис.ПісляЗміни_Характеристика(запис);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                PopoverSmallSelect.Add(page);
                                PopoverSmallSelect.ShowAll();

                                page.LoadRecords();
                                break;
                            }
                        case Columns.Серія:
                            {
                                СеріїНоменклатури_ШвидкийВибір page = new СеріїНоменклатури_ШвидкийВибір() { PopoverParent = PopoverSmallSelect, DirectoryPointerItem = запис.Серія };
                                page.CallBack_OnSelectPointer = (СеріїНоменклатури_Pointer selectPointer) =>
                                {
                                    запис.Серія = selectPointer;
                                    Запис.ПісляЗміни_Серія(запис);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                PopoverSmallSelect.Add(page);
                                PopoverSmallSelect.ShowAll();

                                page.LoadRecords();
                                break;
                            }
                        case Columns.Пакування:
                            {
                                ПакуванняОдиниціВиміру_ШвидкийВибір page = new ПакуванняОдиниціВиміру_ШвидкийВибір() { PopoverParent = PopoverSmallSelect, DirectoryPointerItem = запис.Пакування };
                                page.CallBack_OnSelectPointer = (ПакуванняОдиниціВиміру_Pointer selectPointer) =>
                                {
                                    запис.Пакування = selectPointer;
                                    Запис.ПісляЗміни_Пакування(запис);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                PopoverSmallSelect.Add(page);
                                PopoverSmallSelect.ShowAll();

                                page.LoadRecords();
                                break;
                            }
                        case Columns.КоміркаВідправник:
                            {
                                СкладськіКомірки_ШвидкийВибір page = new СкладськіКомірки_ШвидкийВибір() { PopoverParent = PopoverSmallSelect, DirectoryPointerItem = запис.КоміркаВідправник };
                                page.CallBack_OnSelectPointer = (СкладськіКомірки_Pointer selectPointer) =>
                                {
                                    запис.КоміркаВідправник = selectPointer;
                                    Запис.ПісляЗміни_КоміркаВідправник(запис);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                PopoverSmallSelect.Add(page);
                                PopoverSmallSelect.ShowAll();

                                page.LoadRecords();
                                break;
                            }
                        case Columns.КоміркаОтримувач:
                            {
                                СкладськіКомірки_ШвидкийВибір page = new СкладськіКомірки_ШвидкийВибір() { PopoverParent = PopoverSmallSelect, DirectoryPointerItem = запис.КоміркаОтримувач };
                                page.CallBack_OnSelectPointer = (СкладськіКомірки_Pointer selectPointer) =>
                                {
                                    запис.КоміркаОтримувач = selectPointer;
                                    Запис.ПісляЗміни_КоміркаОтримувач(запис);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                PopoverSmallSelect.Add(page);
                                PopoverSmallSelect.ShowAll();

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

            if (ПереміщенняТоварівНаСкладі_Objest != null)
            {
                Query querySelect = ПереміщенняТоварівНаСкладі_Objest.Товари_TablePart.QuerySelect;
                querySelect.Clear();

                //JOIN Номенклатура
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(Номенклатура_Const.TABLE + "." + Номенклатура_Const.Назва, "Номенклатура"));
                querySelect.Joins.Add(
                    new Join(Номенклатура_Const.TABLE, ПереміщенняТоварівНаСкладі_Товари_TablePart.Номенклатура, querySelect.Table));

                //JOIN Характеристика
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ХарактеристикиНоменклатури_Const.TABLE + "." + ХарактеристикиНоменклатури_Const.Назва, "Характеристика"));
                querySelect.Joins.Add(
                    new Join(ХарактеристикиНоменклатури_Const.TABLE, ПереміщенняТоварівНаСкладі_Товари_TablePart.ХарактеристикаНоменклатури, querySelect.Table));

                //JOIN Серія
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(СеріїНоменклатури_Const.TABLE + "." + СеріїНоменклатури_Const.Номер, "Серія"));
                querySelect.Joins.Add(
                    new Join(СеріїНоменклатури_Const.TABLE, ПереміщенняТоварівНаСкладі_Товари_TablePart.Серія, querySelect.Table));

                //JOIN Пакування
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ПакуванняОдиниціВиміру_Const.TABLE + "." + ПакуванняОдиниціВиміру_Const.Назва, "Пакування"));
                querySelect.Joins.Add(
                    new Join(ПакуванняОдиниціВиміру_Const.TABLE, ПереміщенняТоварівНаСкладі_Товари_TablePart.Пакування, querySelect.Table));

                //JOIN КоміркаВідправник
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(СкладськіКомірки_Const.TABLE + "." + СкладськіКомірки_Const.Назва, "КоміркаВідправник"));
                querySelect.Joins.Add(
                    new Join(СкладськіКомірки_Const.TABLE, ПереміщенняТоварівНаСкладі_Товари_TablePart.КоміркаВідправник, querySelect.Table));

                //JOIN КоміркаОтримувач
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>("tab_otrymuvach." + СкладськіКомірки_Const.Назва, "КоміркаОтримувач"));
                querySelect.Joins.Add(
                    new Join(СкладськіКомірки_Const.TABLE, ПереміщенняТоварівНаСкладі_Товари_TablePart.КоміркаОтримувач, querySelect.Table, "tab_otrymuvach"));

                //ORDER
                querySelect.Order.Add(ПереміщенняТоварівНаСкладі_Товари_TablePart.НомерРядка, SelectOrder.ASC);

                ПереміщенняТоварівНаСкладі_Objest.Товари_TablePart.Read();

                Dictionary<string, Dictionary<string, string>> JoinValue = ПереміщенняТоварівНаСкладі_Objest.Товари_TablePart.JoinValue;

                foreach (ПереміщенняТоварівНаСкладі_Товари_TablePart.Record record in ПереміщенняТоварівНаСкладі_Objest.Товари_TablePart.Records)
                {
                    string uid = record.UID.ToString();

                    record.Номенклатура.Назва = JoinValue[uid]["Номенклатура"];
                    record.ХарактеристикаНоменклатури.Назва = JoinValue[uid]["Характеристика"];
                    record.Серія.Назва = JoinValue[uid]["Серія"];
                    record.Пакування.Назва = JoinValue[uid]["Пакування"];
                    record.КоміркаВідправник.Назва = JoinValue[uid]["КоміркаВідправник"];
                    record.КоміркаОтримувач.Назва = JoinValue[uid]["КоміркаОтримувач"];

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
                        КоміркаВідправник = record.КоміркаВідправник,
                        КоміркаОтримувач = record.КоміркаОтримувач
                    };

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }
        }

        public void SaveRecords()
        {
            if (ПереміщенняТоварівНаСкладі_Objest != null)
            {
                ПереміщенняТоварівНаСкладі_Objest.Товари_TablePart.Records.Clear();

                int sequenceNumber = 0;

                foreach (Запис запис in Записи)
                {
                    ПереміщенняТоварівНаСкладі_Товари_TablePart.Record record = new ПереміщенняТоварівНаСкладі_Товари_TablePart.Record();

                    record.UID = запис.ID;
                    record.НомерРядка = ++sequenceNumber;
                    record.Номенклатура = запис.Номенклатура;
                    record.ХарактеристикаНоменклатури = запис.Характеристика;
                    record.Серія = запис.Серія;
                    record.КількістьУпаковок = запис.КількістьУпаковок;
                    record.Пакування = запис.Пакування;
                    record.Кількість = запис.Кількість;
                    record.КоміркаВідправник = запис.КоміркаВідправник;
                    record.КоміркаОтримувач = запис.КоміркаОтримувач;

                    ПереміщенняТоварівНаСкладі_Objest.Товари_TablePart.Records.Add(record);
                }

                ПереміщенняТоварівНаСкладі_Objest.Товари_TablePart.Save(true);
            }
        }

        public string КлючовіСловаДляПошуку()
        {
            string ключовіСлова = "";

            if (ПереміщенняТоварівНаСкладі_Objest != null)
            {
                int sequenceNumber = 0;
                foreach (Запис запис in Записи)
                    ключовіСлова += $"\n{++sequenceNumber}. {запис.Номенклатура.Назва} {запис.Характеристика.Назва} {запис.Серія.Назва} {запис.КоміркаВідправник.Назва} {запис.КоміркаОтримувач.Назва}";
            }

            return ключовіСлова;
        }

        #region TreeView

        void AddColumn()
        {
            //НомерРядка
            TreeViewGrid.AppendColumn(new TreeViewColumn("№", new CellRendererText(), "text", (int)Columns.НомерРядка) { Resizable = true, MinWidth =  30 });

            //Номенклатура
            {
                TreeViewColumn Номенклатура = new TreeViewColumn("Номенклатура", new CellRendererText(), "text", (int)Columns.Номенклатура) { Resizable = true, MinWidth =  200 };
                Номенклатура.Data.Add("Column", Columns.Номенклатура);

                TreeViewGrid.AppendColumn(Номенклатура);
            }

            //Характеристика
            {
                TreeViewColumn Характеристика = new TreeViewColumn("Характеристика", new CellRendererText(), "text", (int)Columns.Характеристика) { Resizable = true, MinWidth =  200 };
                Характеристика.Visible = Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const;
                Характеристика.Data.Add("Column", Columns.Характеристика);

                TreeViewGrid.AppendColumn(Характеристика);
            }

            //Серія
            {
                TreeViewColumn Серія = new TreeViewColumn("Серія", new CellRendererText(), "text", (int)Columns.Серія) { Resizable = true, MinWidth =  200 };
                Серія.Visible = Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const;
                Серія.Data.Add("Column", Columns.Серія);

                TreeViewGrid.AppendColumn(Серія);
            }

            //КількістьУпаковок
            {
                CellRendererText КількістьУпаковок = new CellRendererText() { Editable = true };
                КількістьУпаковок.Edited += TextChanged;
                КількістьУпаковок.Data.Add("Column", (int)Columns.КількістьУпаковок);

                TreeViewColumn Column = new TreeViewColumn("Пак", КількістьУпаковок, "text", (int)Columns.КількістьУпаковок) { Resizable = true, MinWidth =  50 };
                Column.SetCellDataFunc(КількістьУпаковок, new TreeCellDataFunc(NumericCellDataFunc));
                TreeViewGrid.AppendColumn(Column);
            }

            //Пакування
            {
                TreeViewColumn Пакування = new TreeViewColumn("Пакування", new CellRendererText(), "text", (int)Columns.Пакування) { Resizable = true, MinWidth =  100 };
                Пакування.Data.Add("Column", Columns.Пакування);

                TreeViewGrid.AppendColumn(Пакування);
            }

            //Кількість
            {
                CellRendererText Кількість = new CellRendererText() { Editable = true };
                Кількість.Edited += TextChanged;
                Кількість.Data.Add("Column", (int)Columns.Кількість);

                TreeViewColumn Column = new TreeViewColumn("Кількість", Кількість, "text", (int)Columns.Кількість) { Resizable = true, MinWidth =  100 };
                Column.SetCellDataFunc(Кількість, new TreeCellDataFunc(NumericCellDataFunc));
                TreeViewGrid.AppendColumn(Column);
            }

            //КоміркаВідправник
            {
                TreeViewColumn КоміркаВідправник = new TreeViewColumn("Комірка відправник", new CellRendererText(), "text", (int)Columns.КоміркаВідправник) { Resizable = true, MinWidth =  100 };
                КоміркаВідправник.Data.Add("Column", Columns.КоміркаВідправник);

                TreeViewGrid.AppendColumn(КоміркаВідправник);
            }

            //КоміркаОтримувач
            {
                TreeViewColumn КоміркаОтримувач = new TreeViewColumn("Комірка отримувач", new CellRendererText(), "text", (int)Columns.КоміркаОтримувач) { Resizable = true, MinWidth =  100 };
                КоміркаОтримувач.Data.Add("Column", Columns.КоміркаОтримувач);

                TreeViewGrid.AppendColumn(КоміркаОтримувач);
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
                            }

                            break;
                        }
                }

                Store.SetValues(iter, запис.ToArray());
            }
        }

        void OnKeyReleaseEvent(object? sender, KeyReleaseEventArgs args)
        {
            switch (args.Event.Key)
            {
                case Gdk.Key.Insert:
                    {
                        OnAddClick(null, new EventArgs());
                        break;
                    }
                case Gdk.Key.Delete:
                    {
                        OnDeleteClick(TreeViewGrid, new EventArgs());
                        break;
                    }
            }
        }

        #endregion

        #region ToolBar

        void OnAddClick(object? sender, EventArgs args)
        {
            Запис запис = new Запис();
            Записи.Add(запис);

            TreeIter iter = Store.AppendValues(запис.ToArray());
            TreeViewGrid.SetCursor(Store.GetPath(iter), TreeViewGrid.Columns[0], false);
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