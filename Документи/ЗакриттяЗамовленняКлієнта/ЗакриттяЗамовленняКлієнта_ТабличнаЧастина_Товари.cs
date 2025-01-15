
/*
        ЗакриттяЗамовленняКлієнта_ТабличнаЧастина_Товари.cs
        Таблична Частина
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.Перелічення;


namespace StorageAndTrade
{
    class ЗакриттяЗамовленняКлієнта_ТабличнаЧастина_Товари : ДокументТабличнаЧастина
    {
        public ЗакриттяЗамовленняКлієнта_Objest? ЕлементВласник { get; set; }
        public System.Action? ОбновитиЗначенняДокумента { get; set; }

        #region Записи

        enum Columns
        {
            НомерРядка,
            Номенклатура,
            ХарактеристикаНоменклатури,
            КількістьУпаковок,
            Пакування,
            Кількість,
            КількістьФакт,
            Ціна,
            Сума,
            Склад,
        }

        ListStore Store = new ListStore([

            typeof(int), //НомерРядка
            typeof(string), //Номенклатура
            typeof(string), //ХарактеристикаНоменклатури
            typeof(int), //КількістьУпаковок
            typeof(string), //Пакування
            typeof(float), //Кількість
            typeof(float),    //КількістьФакт
            typeof(float), //Ціна
            typeof(float), //Сума
            typeof(string), //Склад
        ]);

        List<Запис> Записи = [];

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            public int НомерРядка { get; set; } = 0;
            public Номенклатура_Pointer Номенклатура { get; set; } = new Номенклатура_Pointer();
            public ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; } = new ХарактеристикиНоменклатури_Pointer();
            public int КількістьУпаковок { get; set; } = 1;
            public ПакуванняОдиниціВиміру_Pointer Пакування { get; set; } = new ПакуванняОдиниціВиміру_Pointer();
            public decimal Кількість { get; set; } = 1;
            public decimal КількістьФакт { get; set; } = 1;
            public decimal Ціна { get; set; } = 0;
            public decimal Сума { get; set; } = 0;
            public Склади_Pointer Склад { get; set; } = new Склади_Pointer();


            public object[] ToArray()
            {
                return
                [
                    НомерРядка,
                    Номенклатура.Назва,
                    ХарактеристикаНоменклатури.Назва,
                    КількістьУпаковок,
                    Пакування.Назва,
                    (float)Кількість,
                    (float)КількістьФакт,
                    (float)Ціна,
                    (float)Сума,
                    Склад.Назва,
                ];
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    НомерРядка = запис.НомерРядка,
                    Номенклатура = запис.Номенклатура.Copy(),
                    ХарактеристикаНоменклатури = запис.ХарактеристикаНоменклатури.Copy(),
                    КількістьУпаковок = запис.КількістьУпаковок,
                    Пакування = запис.Пакування.Copy(),
                    Кількість = запис.Кількість,
                    КількістьФакт = запис.КількістьФакт,
                    Ціна = запис.Ціна,
                    Сума = запис.Сума,
                    Склад = запис.Склад.Copy(),
                };
            }

            public static async ValueTask ПісляЗміни_Номенклатура(Запис запис)
            {
                await запис.Номенклатура.GetPresentation();

                Номенклатура_Objest? номенклатура_Objest = await запис.Номенклатура.GetDirectoryObject();
                if (номенклатура_Objest != null && !номенклатура_Objest.ОдиницяВиміру.IsEmpty())
                {
                    запис.Пакування = номенклатура_Objest.ОдиницяВиміру;
                    await Запис.ПісляЗміни_Пакування(запис);
                }
            }

            public static async ValueTask ПісляЗміни_ХарактеристикаНоменклатури(Запис запис)
            {
                await запис.ХарактеристикаНоменклатури.GetPresentation();
            }

            public static async ValueTask ПісляЗміни_Пакування(Запис запис)
            {
                await запис.Пакування.GetPresentation();

                if (!запис.Пакування.IsEmpty())
                {
                    ПакуванняОдиниціВиміру_Objest? пакуванняОдиниціВиміру_Objest = await запис.Пакування.GetDirectoryObject();
                    if (пакуванняОдиниціВиміру_Objest != null)
                        запис.КількістьУпаковок = (пакуванняОдиниціВиміру_Objest.КількістьУпаковок > 0) ? пакуванняОдиниціВиміру_Objest.КількістьУпаковок : 1;
                    else
                        запис.КількістьУпаковок = 1;
                }

                Запис.ПісляЗміни_КількістьАбоЦіна(запис);
            }

            public static async ValueTask ПісляЗміни_Склад(Запис запис)
            {
                await запис.Склад.GetPresentation();
            }

            public static void ПісляЗміни_КількістьАбоЦіна(Запис запис)
            {
                запис.КількістьФакт = запис.Кількість * запис.КількістьУпаковок;
                запис.Сума = запис.Кількість * запис.Ціна;
            }
        }

        #endregion

        Label ПідсумокСума = new Label() { Selectable = true };

        public ЗакриттяЗамовленняКлієнта_ТабличнаЧастина_Товари()
        {
            TreeViewGrid.Model = Store;
            AddColumn();

            CreateBottomBlock();

            Store.RowChanged += (object? sender, RowChangedArgs args) => ОбчислитиПідсумки();
            Store.RowDeleted += (object? sender, RowDeletedArgs args) => ОбчислитиПідсумки();

            //
            //
            //

            ToolButton fill = new ToolButton(new Image(Stock.Convert, IconSize.Menu), "Заповнити із замовлення клієнта") { IsImportant = true };
            fill.Clicked += OnFill;
            ToolbarTop.Add(fill);
        }

        #region Підсумки

        void CreateBottomBlock()
        {
            Box hBox = new Box(Orientation.Horizontal, 0) { Halign = Align.Start };
            hBox.PackStart(new Label("<b>Підсумки</b> ") { UseMarkup = true }, false, false, 2);
            hBox.PackStart(ПідсумокСума, false, false, 2);

            PackStart(hBox, false, false, 2);
        }

        void ОбчислитиПідсумки()
        {
            decimal Сума = 0;

            foreach (Запис запис in Записи)
                Сума += запис.Сума;

            ПідсумокСума.Text = $"Сума: <b>{Сума}</b>";
            ПідсумокСума.UseMarkup = true;
        }

        #endregion

        async void OnFill(object? sender, EventArgs args)
        {
            ОбновитиЗначенняДокумента?.Invoke();

            if (ЕлементВласник != null)
                if (!ЕлементВласник.ЗамовленняКлієнта.IsEmpty())
                {
                    ЗамовленняКлієнта_Objest? ЗамовленняКлієнта = await ЕлементВласник.ЗамовленняКлієнта.GetDocumentObject(true);
                    if (ЗамовленняКлієнта != null)
                        foreach (ЗамовленняКлієнта_Товари_TablePart.Record record in ЗамовленняКлієнта.Товари_TablePart.Records)
                        {
                            Запис запис = new Запис
                            {
                                Номенклатура = record.Номенклатура,
                                ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                                КількістьУпаковок = record.КількістьУпаковок,
                                Пакування = record.Пакування,
                                Кількість = record.Кількість,
                                Ціна = record.Ціна,
                                Сума = record.Сума,
                                Склад = record.Склад,
                            };

                            await Запис.ПісляЗміни_Номенклатура(запис);
                            await Запис.ПісляЗміни_ХарактеристикаНоменклатури(запис);
                            await Запис.ПісляЗміни_Пакування(запис);
                            await Запис.ПісляЗміни_Склад(запис);

                            Записи.Add(запис);
                            Store.AppendValues(запис.ToArray());
                        }
                }
        }

        void AddColumn()
        {
            //НомерРядка
            {

                CellRendererText cellNumber = new CellRendererText() { Xalign = 1 };
                TreeViewColumn column = new TreeViewColumn("№", cellNumber, "text", (int)Columns.НомерРядка) { Resizable = true, MinWidth = 30 };
                TreeViewGrid.AppendColumn(column);
            }

            //Номенклатура
            {

                TreeViewColumn column = new TreeViewColumn("Номенклатура", new CellRendererText(), "text", (int)Columns.Номенклатура) { Resizable = true, MinWidth = 200 };

                SetColIndex(column, Columns.Номенклатура);
                TreeViewGrid.AppendColumn(column);
            }

            //ХарактеристикаНоменклатури
            {

                TreeViewColumn column = new TreeViewColumn("Характеристика", new CellRendererText(), "text", (int)Columns.ХарактеристикаНоменклатури) { Resizable = true, MinWidth = 200 };

                SetColIndex(column, Columns.ХарактеристикаНоменклатури);
                TreeViewGrid.AppendColumn(column);
            }

            //КількістьУпаковок
            {

                CellRendererText cellNumber = new CellRendererText() { Editable = true, Xalign = 1 };
                cellNumber.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("Коєфіціент", cellNumber, "text", (int)Columns.КількістьУпаковок) { Resizable = true, Alignment = 1, MinWidth = 50 };
                column.SetCellDataFunc(cellNumber, new TreeCellDataFunc(NumericCellDataFunc));

                SetColIndex(column, Columns.КількістьУпаковок);
                TreeViewGrid.AppendColumn(column);
            }

            //Пакування
            {

                TreeViewColumn column = new TreeViewColumn("Пакування", new CellRendererText(), "text", (int)Columns.Пакування) { Resizable = true, MinWidth = 100 };

                SetColIndex(column, Columns.Пакування);
                TreeViewGrid.AppendColumn(column);
            }

            //Кількість
            {

                CellRendererText cellNumber = new CellRendererText() { Editable = true, Xalign = 1 };
                cellNumber.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("Кількість", cellNumber, "text", (int)Columns.Кількість) { Resizable = true, Alignment = 1, MinWidth = 100 };
                column.SetCellDataFunc(cellNumber, new TreeCellDataFunc(NumericCellDataFunc));

                SetColIndex(column, Columns.Кількість);
                TreeViewGrid.AppendColumn(column);
            }

            //КількістьФакт
            {
                CellRendererText cellNumber = new CellRendererText() { Xalign = 1 };

                TreeViewColumn column = new TreeViewColumn("Кільк.факт", cellNumber, "text", (int)Columns.КількістьФакт) { Resizable = true, Alignment = 1, MinWidth = 100 };
                column.SetCellDataFunc(cellNumber, new TreeCellDataFunc(NumericCellDataFunc));

                SetColIndex(column, Columns.КількістьФакт);
                TreeViewGrid.AppendColumn(column);
            }

            //Ціна
            {

                CellRendererText cellNumber = new CellRendererText() { Editable = true, Xalign = 1 };
                cellNumber.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("Ціна", cellNumber, "text", (int)Columns.Ціна) { Resizable = true, Alignment = 1, MinWidth = 100 };
                column.SetCellDataFunc(cellNumber, new TreeCellDataFunc(NumericCellDataFunc));

                SetColIndex(column, Columns.Ціна);
                TreeViewGrid.AppendColumn(column);
            }

            //Сума
            {

                CellRendererText cellNumber = new CellRendererText() { Editable = true, Xalign = 1 };
                cellNumber.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("Сума", cellNumber, "text", (int)Columns.Сума) { Resizable = true, Alignment = 1, MinWidth = 100 };
                column.SetCellDataFunc(cellNumber, new TreeCellDataFunc(NumericCellDataFunc));

                SetColIndex(column, Columns.Сума);
                TreeViewGrid.AppendColumn(column);
            }

            //Склад
            {

                TreeViewColumn column = new TreeViewColumn("Склад", new CellRendererText(), "text", (int)Columns.Склад) { Resizable = true, MinWidth = 100 };

                SetColIndex(column, Columns.Склад);
                TreeViewGrid.AppendColumn(column);
            }

            //Колонка пустишка для заповнення вільного простору
            TreeViewGrid.AppendColumn(new TreeViewColumn());
        }

        #region Load and Save

        public override async ValueTask LoadRecords()
        {
            if (ЕлементВласник != null)
            {
                ЕлементВласник.Товари_TablePart.FillJoin([ЗакриттяЗамовленняКлієнта_Товари_TablePart.НомерРядка]);
                await ЕлементВласник.Товари_TablePart.Read();

                Записи.Clear();
                Store.Clear();

                foreach (ЗакриттяЗамовленняКлієнта_Товари_TablePart.Record record in ЕлементВласник.Товари_TablePart.Records)
                {
                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        НомерРядка = record.НомерРядка,
                        Номенклатура = record.Номенклатура,
                        ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                        КількістьУпаковок = record.КількістьУпаковок,
                        Пакування = record.Пакування,
                        Кількість = record.Кількість,
                        Ціна = record.Ціна,
                        Сума = record.Сума,
                        Склад = record.Склад,
                    };

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }

                SelectRowActivated();
            }
        }

        public override async ValueTask SaveRecords()
        {
            if (ЕлементВласник != null)
            {
                ЕлементВласник.Товари_TablePart.Records.Clear();
                int sequenceNumber = 0;
                foreach (Запис запис in Записи)
                {
                    ЕлементВласник.Товари_TablePart.Records.Add(new ЗакриттяЗамовленняКлієнта_Товари_TablePart.Record()
                    {
                        UID = запис.ID,
                        НомерРядка = ++sequenceNumber,
                        Номенклатура = запис.Номенклатура,
                        ХарактеристикаНоменклатури = запис.ХарактеристикаНоменклатури,
                        КількістьУпаковок = запис.КількістьУпаковок,
                        Пакування = запис.Пакування,
                        Кількість = запис.Кількість,
                        Ціна = запис.Ціна,
                        Сума = запис.Сума,
                        Склад = запис.Склад,
                    });
                }
                await ЕлементВласник.Товари_TablePart.Save(true);
                await LoadRecords();
            }
        }

        public decimal СумаДокументу()
        {
            decimal Сума = 0;

            foreach (Запис запис in Записи)
                Сума += запис.Сума;

            return Math.Round(Сума, 2);
        }

        public string КлючовіСловаДляПошуку()
        {

            string keyWords = "";
            foreach (Запис запис in Записи)
                keyWords += $"\n {запис.Номенклатура.Назва} {запис.ХарактеристикаНоменклатури.Назва} {запис.Пакування.Назва} {запис.Склад.Назва}";

            return keyWords;
        }

        #endregion

        #region Func

        protected override ФормаЖурнал? OpenSelect(TreeIter iter, int rowNumber, int colNumber)
        {
            Запис запис = Записи[rowNumber];
            switch ((Columns)colNumber)
            {
                case Columns.Номенклатура:
                    {
                        Номенклатура_ШвидкийВибір page = new()
                        {
                            DirectoryPointerItem = запис.Номенклатура.UnigueID,
                            CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                            {
                                запис.Номенклатура = new Номенклатура_Pointer(selectPointer);
                                await Запис.ПісляЗміни_Номенклатура(запис);
                                Store.SetValues(iter, запис.ToArray());
                            },
                            CallBack_OnMultipleSelectPointer = async (UnigueID[] selectPointers) =>
                            {
                                foreach (var selectPointer in selectPointers)
                                {
                                    (Запис запис, TreeIter iter) = НовийЗапис();

                                    запис.Номенклатура = new Номенклатура_Pointer(selectPointer);
                                    await Запис.ПісляЗміни_Номенклатура(запис);
                                    Store.SetValues(iter, запис.ToArray());
                                }
                            }
                        };
                        return page;
                    }
                case Columns.ХарактеристикаНоменклатури:
                    {
                        ХарактеристикиНоменклатури_ШвидкийВибір page = new()
                        {
                            DirectoryPointerItem = запис.ХарактеристикаНоменклатури.UnigueID,
                            CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                            {
                                запис.ХарактеристикаНоменклатури = new ХарактеристикиНоменклатури_Pointer(selectPointer);
                                await Запис.ПісляЗміни_ХарактеристикаНоменклатури(запис);
                                Store.SetValues(iter, запис.ToArray());
                            },
                            CallBack_OnMultipleSelectPointer = async (UnigueID[] selectPointers) =>
                            {
                                foreach (var selectPointer in selectPointers)
                                {
                                    (Запис запис, TreeIter iter) = НовийЗапис();

                                    запис.ХарактеристикаНоменклатури = new ХарактеристикиНоменклатури_Pointer(selectPointer);
                                    await Запис.ПісляЗміни_ХарактеристикаНоменклатури(запис);

                                    //Витягую Номенклатуру із Характеристики
                                    ХарактеристикиНоменклатури_Objest? ХарактеристикаОбєкт = await запис.ХарактеристикаНоменклатури.GetDirectoryObject();
                                    if (ХарактеристикаОбєкт != null)
                                    {
                                        запис.Номенклатура = ХарактеристикаОбєкт.Номенклатура;
                                        await Запис.ПісляЗміни_Номенклатура(запис);
                                    }

                                    Store.SetValues(iter, запис.ToArray());
                                }
                            }
                        };
                        page.Власник.Pointer = запис.Номенклатура;
                        return page;
                    }
                case Columns.Пакування:
                    {
                        ПакуванняОдиниціВиміру_ШвидкийВибір page = new()
                        {
                            DirectoryPointerItem = запис.Пакування.UnigueID,
                            CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                            {
                                запис.Пакування = new ПакуванняОдиниціВиміру_Pointer(selectPointer);
                                await Запис.ПісляЗміни_Пакування(запис);
                                Store.SetValues(iter, запис.ToArray());
                            }
                        };
                        return page;
                    }
                case Columns.Склад:
                    {
                        Склади_ШвидкийВибір page = new()
                        {
                            DirectoryPointerItem = запис.Склад.UnigueID,
                            CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                            {
                                запис.Склад = new Склади_Pointer(selectPointer);
                                await Запис.ПісляЗміни_Склад(запис);
                                Store.SetValues(iter, запис.ToArray());
                            }
                        };
                        return page;
                    }

                default: return null;
            }
        }

        protected override void ClearCell(TreeIter iter, int rowNumber, int colNumber)
        {
            Запис запис = Записи[rowNumber];
            switch ((Columns)colNumber)
            {
                case Columns.Номенклатура: { запис.Номенклатура.Clear(); break; }
                case Columns.ХарактеристикаНоменклатури: { запис.ХарактеристикаНоменклатури.Clear(); break; }
                case Columns.Пакування: { запис.Пакування.Clear(); break; }
                case Columns.Склад: { запис.Склад.Clear(); break; }

                default: break;
            }
            Store.SetValues(iter, запис.ToArray());
        }

        protected override void ChangeCell(TreeIter iter, int rowNumber, int colNumber, string newText)
        {
            Запис запис = Записи[rowNumber];
            switch ((Columns)colNumber)
            {
                case Columns.КількістьУпаковок:
                    {
                        var (check, value) = Validate.IsInt(newText);
                        if (check)
                        {
                            if (value <= 0) value = 1;

                            запис.КількістьУпаковок = value;
                            Запис.ПісляЗміни_КількістьАбоЦіна(запис);
                        }
                        break;
                    }
                case Columns.Кількість:
                    {
                        var (check, value) = Validate.IsDecimal(newText);
                        if (check)
                        {
                            запис.Кількість = value;
                            Запис.ПісляЗміни_КількістьАбоЦіна(запис);
                        }
                        break;
                    }
                case Columns.Ціна:
                    {
                        var (check, value) = Validate.IsDecimal(newText);
                        if (check)
                        {
                            запис.Ціна = value;
                            Запис.ПісляЗміни_КількістьАбоЦіна(запис);
                        }
                        break;
                    }
                case Columns.Сума: { var (check, value) = Validate.IsDecimal(newText); if (check) запис.Сума = value; break; }

                default: break;
            }
            Store.SetValues(iter, запис.ToArray());
        }

        void NumericCellDataFunc(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            if (GetColIndex(column, out int colNumber))
            {
                int rowNumber = int.Parse(Store.GetPath(iter).ToString());
                Запис запис = Записи[rowNumber];

                CellRendererText cellText = (CellRendererText)cell;
                cellText.Foreground = "green";

                switch ((Columns)colNumber)
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

                    case Columns.КількістьФакт:
                        {
                            cellText.Text = запис.КількістьФакт.ToString();
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

                    default: break;
                }
            }
        }


        #endregion

        #region ToolBar

        (Запис запис, TreeIter iter) НовийЗапис()
        {
            Запис запис = new Запис();
            Записи.Add(запис);

            TreeIter iter = Store.AppendValues(запис.ToArray());
            TreeViewGrid.SetCursor(Store.GetPath(iter), TreeViewGrid.Columns[0], false);

            return (запис, iter);
        }

        protected override void AddRecord()
        {
            НовийЗапис();
        }

        protected override void CopyRecord(int rowNumber)
        {
            Запис запис = Записи[rowNumber];
            Запис записНовий = Запис.Clone(запис);
            Записи.Add(записНовий);

            TreeIter iter = Store.AppendValues(записНовий.ToArray());
            TreeViewGrid.SetCursor(Store.GetPath(iter), TreeViewGrid.Columns[0], false);
        }

        protected override void DeleteRecord(TreeIter iter, int rowNumber)
        {
            Запис запис = Записи[rowNumber];
            Записи.Remove(запис);
            Store.Remove(ref iter);
        }

        #endregion
    }
}
