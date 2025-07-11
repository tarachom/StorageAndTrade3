
/*
        РозміщенняТоварівНаСкладі_ТабличнаЧастина_Товари.cs
        Таблична Частина
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode;
using GeneratedCode.Довідники;
using GeneratedCode.Документи;
using GeneratedCode.Константи;
using GeneratedCode.РегістриВідомостей;

namespace StorageAndTrade
{
    class РозміщенняТоварівНаСкладі_ТабличнаЧастина_Товари : ДокументТабличнаЧастина
    {
        public РозміщенняТоварівНаСкладі_Objest? ЕлементВласник { get; set; }
        public System.Action? ОбновитиЗначенняДокумента { get; set; }

        #region Записи

        enum Columns
        {
            НомерРядка,
            Номенклатура,
            ХарактеристикаНоменклатури,
            Серія,
            КількістьУпаковок,
            Пакування,
            Кількість,
            КількістьФакт,
            Комірка,
        }

        ListStore Store = new ListStore([
            typeof(int), //НомерРядка
            typeof(string), //Номенклатура
            typeof(string), //ХарактеристикаНоменклатури
            typeof(string), //Серія
            typeof(int), //КількістьУпаковок
            typeof(string), //Пакування
            typeof(float), //Кількість
            typeof(float), //КількістьФакт
            typeof(string), //Комірка
        ]);

        List<Запис> Записи = [];

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            public int НомерРядка { get; set; } = 0;
            public Номенклатура_Pointer Номенклатура { get; set; } = new Номенклатура_Pointer();
            public ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; } = new ХарактеристикиНоменклатури_Pointer();
            public СеріїНоменклатури_Pointer Серія { get; set; } = new СеріїНоменклатури_Pointer();
            public int КількістьУпаковок { get; set; } = 1;
            public ПакуванняОдиниціВиміру_Pointer Пакування { get; set; } = new ПакуванняОдиниціВиміру_Pointer();
            public decimal Кількість { get; set; } = 1;
            public decimal КількістьФакт { get; set; } = 1;
            public СкладськіКомірки_Pointer Комірка { get; set; } = new СкладськіКомірки_Pointer();

            public object[] ToArray()
            {
                return
                [
                    НомерРядка,
                    Номенклатура.Назва,
                    ХарактеристикаНоменклатури.Назва,
                    Серія.Назва,
                    КількістьУпаковок,
                    Пакування.Назва,
                    (float)Кількість,
                    (float)КількістьФакт,
                    Комірка.Назва,
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
                    Серія = запис.Серія.Copy(),
                    КількістьУпаковок = запис.КількістьУпаковок,
                    Пакування = запис.Пакування.Copy(),
                    Кількість = запис.Кількість,
                    КількістьФакт = запис.КількістьФакт,
                    Комірка = запис.Комірка.Copy(),
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

            public static async ValueTask ПісляЗміни_Серія(Запис запис)
            {
                await запис.Серія.GetPresentation();
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

                Запис.ПісляЗміни_Кількість(запис);
            }

            public static async ValueTask ПісляЗміни_Комірка(Запис запис)
            {
                await запис.Комірка.GetPresentation();
            }

            public static void ПісляЗміни_Кількість(Запис запис)
            {
                запис.КількістьФакт = запис.Кількість * запис.КількістьУпаковок;
            }
        }

        #endregion

        public РозміщенняТоварівНаСкладі_ТабличнаЧастина_Товари()
        {
            TreeViewGrid.Model = Store;
            AddColumn();

            //Separator
            ToolItem toolItemSeparator = new ToolItem { new Separator(Orientation.Horizontal) };
            ToolbarTop.Add(toolItemSeparator);

            //
            // Func
            //

            ToolButton fillButton = new ToolButton(new Image(Stock.Convert, IconSize.Menu), "Заповнити розміщення") { IsImportant = true };
            fillButton.Clicked += ЗаповнитиРозміщенняНоменклатуриПоКомірках;
            ToolbarTop.Add(fillButton);
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
                TreeViewColumn column = new TreeViewColumn("Характеристика", new CellRendererText(), "text", (int)Columns.ХарактеристикаНоменклатури) { Resizable = true, MinWidth = 200, Visible = Системні.ВестиОблікПоХарактеристикахНоменклатури_Const };

                SetColIndex(column, Columns.ХарактеристикаНоменклатури);
                TreeViewGrid.AppendColumn(column);
            }

            //Серія
            {
                TreeViewColumn column = new TreeViewColumn("Серія", new CellRendererText(), "text", (int)Columns.Серія) { Resizable = true, MinWidth = 100, Visible = Системні.ВестиОблікПоСеріяхНоменклатури_Const };

                SetColIndex(column, Columns.Серія);
                TreeViewGrid.AppendColumn(column);
            }

            //КількістьУпаковок
            {
                CellRendererText cellNumber = new CellRendererText() { Editable = true, Xalign = 1 };
                cellNumber.Edited += EditCell;
                cellNumber.EditingStarted += EditingStarted;
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
                cellNumber.EditingStarted += EditingStarted;
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

            //Комірка
            {
                TreeViewColumn column = new TreeViewColumn("Комірка", new CellRendererText(), "text", (int)Columns.Комірка) { Resizable = true, MinWidth = 100 };

                SetColIndex(column, Columns.Комірка);
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
                ЕлементВласник.Товари_TablePart.FillJoin([РозміщенняТоварівНаСкладі_Товари_TablePart.НомерРядка]);
                await ЕлементВласник.Товари_TablePart.Read();

                Записи.Clear();
                Store.Clear();

                foreach (РозміщенняТоварівНаСкладі_Товари_TablePart.Record record in ЕлементВласник.Товари_TablePart.Records)
                {
                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        НомерРядка = record.НомерРядка,
                        Номенклатура = record.Номенклатура,
                        ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                        Серія = record.Серія,
                        КількістьУпаковок = record.КількістьУпаковок,
                        Пакування = record.Пакування,
                        Кількість = record.Кількість,
                        КількістьФакт = record.Кількість * record.КількістьУпаковок,
                        Комірка = record.Комірка,
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
                foreach (Запис запис in Записи)
                {
                    ЕлементВласник.Товари_TablePart.Records.Add(new РозміщенняТоварівНаСкладі_Товари_TablePart.Record()
                    {
                        UID = запис.ID,
                        Номенклатура = запис.Номенклатура,
                        ХарактеристикаНоменклатури = запис.ХарактеристикаНоменклатури,
                        Серія = запис.Серія,
                        КількістьУпаковок = запис.КількістьУпаковок,
                        Пакування = запис.Пакування,
                        Кількість = запис.Кількість,
                        Комірка = запис.Комірка,
                    });
                }

                await ЕлементВласник.Товари_TablePart.Save(true);
                await LoadRecords();
            }
        }

        public string КлючовіСловаДляПошуку()
        {
            string ключовіСлова = "";

            if (ЕлементВласник != null)
            {
                int sequenceNumber = 0;
                foreach (Запис запис in Записи)
                    ключовіСлова += $"\n{++sequenceNumber}. {запис.Номенклатура.Назва} {запис.ХарактеристикаНоменклатури.Назва} {запис.Серія.Назва}";
            }

            return ключовіСлова;
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
                case Columns.Серія:
                    {
                        СеріїНоменклатури_ШвидкийВибір page = new()
                        {
                            DirectoryPointerItem = запис.Серія.UnigueID,
                            CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                            {
                                запис.Серія = new СеріїНоменклатури_Pointer(selectPointer);
                                await Запис.ПісляЗміни_Серія(запис);
                                Store.SetValues(iter, запис.ToArray());
                            },
                            CallBack_OnMultipleSelectPointer = async (UnigueID[] selectPointers) =>
                            {
                                foreach (var selectPointer in selectPointers)
                                {
                                    (Запис запис, TreeIter iter) = НовийЗапис();

                                    запис.Серія = new СеріїНоменклатури_Pointer(selectPointer);
                                    await Запис.ПісляЗміни_Серія(запис);
                                    Store.SetValues(iter, запис.ToArray());
                                }
                            }
                        };
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
                case Columns.Комірка:
                    {
                        СкладськіКомірки_ШвидкийВибір page = new()
                        {
                            DirectoryPointerItem = запис.Комірка.UnigueID,
                            CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                            {
                                запис.Комірка = new СкладськіКомірки_Pointer(selectPointer);
                                await Запис.ПісляЗміни_Комірка(запис);
                                Store.SetValues(iter, запис.ToArray());
                            },
                            CallBack_OnMultipleSelectPointer = async (UnigueID[] selectPointers) =>
                            {
                                foreach (var selectPointer in selectPointers)
                                {
                                    (Запис запис, TreeIter iter) = НовийЗапис();

                                    запис.Комірка = new СкладськіКомірки_Pointer(selectPointer);
                                    await Запис.ПісляЗміни_Комірка(запис);
                                    Store.SetValues(iter, запис.ToArray());
                                }
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
                case Columns.Серія: { запис.Серія.Clear(); break; }
                case Columns.Пакування: { запис.Пакування.Clear(); break; }
                case Columns.Комірка: { запис.Комірка.Clear(); break; }

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
                            Запис.ПісляЗміни_Кількість(запис);
                        }
                        break;
                    }
                case Columns.Кількість:
                    {
                        var (check, value) = Validate.IsDecimal(newText);
                        if (check)
                        {
                            запис.Кількість = value;
                            Запис.ПісляЗміни_Кількість(запис);
                        }
                        break;
                    }
                default: break;
            }
            Store.SetValues(iter, запис.ToArray());
        }

        void NumericCellDataFunc(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            if (GetColIndex(column, out int colNumber))
            {
                int rowNumber = int.Parse(Store.GetPath(iter).ToString());
                if (rowNumber >= 0 && rowNumber < Записи.Count)
                {
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
                        default: break;
                    }
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

        #region ОбробкаТабЧастини Товари

        async void ЗаповнитиРозміщенняНоменклатуриПоКомірках(object? sender, EventArgs args)
        {
            if (Записи.Count == 0)
                return;

            List<Guid> списокНоменклатуриДляВідбору = [];

            foreach (Запис запис in Записи)
                списокНоменклатуриДляВідбору.Add(запис.Номенклатура.UnigueID.UGuid);

            string query = @$"
WITH register AS
(
    SELECT DISTINCT 
        {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.Номенклатура} AS Номенклатура
    FROM
        {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.TABLE}
    WHERE
        {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.Номенклатура} IN
        (
            '{string.Join("', '", списокНоменклатуриДляВідбору)}'
        )
)
SELECT
    register.Номенклатура,
    (
        SELECT
            {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.Комірка}
        FROM
            {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.TABLE} AS РозміщенняНоменклатуриПоКоміркамНаСкладі
        WHERE
            РозміщенняНоменклатуриПоКоміркамНаСкладі.{РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.Номенклатура} = register.Номенклатура
        ORDER BY РозміщенняНоменклатуриПоКоміркамНаСкладі.period DESC
        LIMIT 1
    ) AS Комірка
FROM
    register
";

            var recordResult = await Config.Kernel.DataBase.SelectRequest(query);

            Dictionary<Guid, Guid> НоменклатураТаКомірки = [];

            foreach (Dictionary<string, object> row in recordResult.ListRow)
                НоменклатураТаКомірки.Add((Guid)row["Номенклатура"], (Guid)row["Комірка"]);

            int sequenceNumber = 0;

            foreach (Запис запис in Записи)
            {
                if (НоменклатураТаКомірки.TryGetValue(запис.Номенклатура.UnigueID.UGuid, out Guid value))
                {
                    запис.Комірка = new СкладськіКомірки_Pointer(value);
                    await Запис.ПісляЗміни_Комірка(запис);

                    Store.GetIterFromString(out TreeIter iter, sequenceNumber.ToString());
                    Store.SetValues(iter, запис.ToArray());
                }

                sequenceNumber++;
            }
        }

        #endregion
    }
}
