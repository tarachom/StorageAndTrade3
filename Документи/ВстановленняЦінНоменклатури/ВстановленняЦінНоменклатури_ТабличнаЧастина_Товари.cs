
/*
        ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари.cs
        Таблична Частина
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.Перелічення;
using StorageAndTrade_1_0.РегістриВідомостей;
using StorageAndTrade_1_0.Константи;

namespace StorageAndTrade
{
    class ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари : ДокументТабличнаЧастина
    {
        public ВстановленняЦінНоменклатури_Objest? ЕлементВласник { get; set; }
        public System.Action? ОбновитиЗначенняДокумента { get; set; }

        #region Записи

        enum Columns
        {
            НомерРядка,
            Номенклатура,
            ХарактеристикаНоменклатури,
            Пакування,
            ВидЦіни,
            Ціна,
        }

        ListStore Store = new ListStore([

            typeof(int), //НомерРядка
            typeof(string), //Номенклатура
            typeof(string), //ХарактеристикаНоменклатури
            typeof(string), //Пакування
            typeof(string), //ВидЦіни
            typeof(float), //Ціна
        ]);

        List<Запис> Записи = [];

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            public int НомерРядка { get; set; } = 0;
            public Номенклатура_Pointer Номенклатура { get; set; } = new Номенклатура_Pointer();
            public ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; } = new ХарактеристикиНоменклатури_Pointer();
            public ПакуванняОдиниціВиміру_Pointer Пакування { get; set; } = new ПакуванняОдиниціВиміру_Pointer();
            public ВидиЦін_Pointer ВидЦіни { get; set; } = new ВидиЦін_Pointer();
            public decimal Ціна { get; set; } = 0;

            public object[] ToArray()
            {
                return
                [
                    НомерРядка,
                    Номенклатура.Назва,
                    ХарактеристикаНоменклатури.Назва,
                    Пакування.Назва,
                    ВидЦіни.Назва,
                    (float)Ціна,
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
                    Пакування = запис.Пакування.Copy(),
                    ВидЦіни = запис.ВидЦіни.Copy(),
                    Ціна = запис.Ціна,
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
            }

            public static async ValueTask ПісляЗміни_ВидЦіни(Запис запис)
            {
                await запис.ВидЦіни.GetPresentation();
            }
        }

        #endregion

        public ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари()
        {
            TreeViewGrid.Model = Store;
            AddColumn();

            //Доповнення ToolbarTop новими функціями
            ToolItem toolItemSeparator = new ToolItem
            {
                new Separator(Orientation.Horizontal)
            };

            ToolbarTop.Add(toolItemSeparator);

            ToolButton fillDirectoryButton = new ToolButton(new Image(Stock.Add, IconSize.Menu), "Заповнити товарами") { IsImportant = true };
            fillDirectoryButton.Clicked += OnFillDirectory;
            ToolbarTop.Add(fillDirectoryButton);

            ToolButton fillRegisterButton = new ToolButton(new Image(Stock.Add, IconSize.Menu), "Заповнити товарами з цінами") { IsImportant = true };
            fillRegisterButton.Clicked += OnFillRegister;
            ToolbarTop.Add(fillRegisterButton);
        }

        void AddColumn()
        {
            //НомерРядка
            {
                CellRendererText cellNumber = new CellRendererText();
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

            //Пакування
            {
                TreeViewColumn column = new TreeViewColumn("Пакування", new CellRendererText(), "text", (int)Columns.Пакування) { Resizable = true, MinWidth = 100 };

                SetColIndex(column, Columns.Пакування);
                TreeViewGrid.AppendColumn(column);
            }

            //ВидЦіни
            {
                TreeViewColumn column = new TreeViewColumn("Вид ціни", new CellRendererText(), "text", (int)Columns.ВидЦіни) { Resizable = true, MinWidth = 100 };

                SetColIndex(column, Columns.ВидЦіни);
                TreeViewGrid.AppendColumn(column);
            }

            //Ціна
            {
                CellRendererText cellNumber = new CellRendererText() { Editable = true };
                cellNumber.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("Ціна", cellNumber, "text", (int)Columns.Ціна) { Resizable = true, MinWidth = 100 };
                column.SetCellDataFunc(cellNumber, new TreeCellDataFunc(NumericCellDataFunc));

                SetColIndex(column, Columns.Ціна);
                TreeViewGrid.AppendColumn(column);
            }

            //Колонка пустишка для заповнення вільного простору
            TreeViewGrid.AppendColumn(new TreeViewColumn());
        }

        #region Load and Save

        public override async ValueTask LoadRecords()
        {
            Store.Clear();
            Записи.Clear();

            if (ЕлементВласник != null)
            {
                ЕлементВласник.Товари_TablePart.FillJoin([ВстановленняЦінНоменклатури_Товари_TablePart.НомерРядка]);
                await ЕлементВласник.Товари_TablePart.Read();

                foreach (ВстановленняЦінНоменклатури_Товари_TablePart.Record record in ЕлементВласник.Товари_TablePart.Records)
                {
                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        НомерРядка = record.НомерРядка,
                        Номенклатура = record.Номенклатура,
                        ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                        Пакування = record.Пакування,
                        ВидЦіни = record.ВидЦіни,
                        Ціна = record.Ціна,
                    };

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
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
                    ЕлементВласник.Товари_TablePart.Records.Add(new ВстановленняЦінНоменклатури_Товари_TablePart.Record()
                    {
                        UID = запис.ID,
                        НомерРядка = ++sequenceNumber,
                        Номенклатура = запис.Номенклатура,
                        ХарактеристикаНоменклатури = запис.ХарактеристикаНоменклатури,
                        Пакування = запис.Пакування,
                        ВидЦіни = запис.ВидЦіни,
                        Ціна = запис.Ціна,

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
                    ключовіСлова += $"\n{++sequenceNumber}. {запис.Номенклатура.Назва} {запис.ХарактеристикаНоменклатури.Назва} ";
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
                            }
                        };
                        page.НоменклатураВласник.Pointer = запис.Номенклатура;
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
                case Columns.ВидЦіни:
                    {
                        ВидиЦін_ШвидкийВибір page = new()
                        {
                            DirectoryPointerItem = запис.ВидЦіни.UnigueID,
                            CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                            {
                                запис.ВидЦіни = new ВидиЦін_Pointer(selectPointer);
                                await Запис.ПісляЗміни_ВидЦіни(запис);
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
                case Columns.ВидЦіни: { запис.ВидЦіни.Clear(); break; }

                default: break;
            }
            Store.SetValues(iter, запис.ToArray());
        }

        protected override void ChangeCell(TreeIter iter, int rowNumber, int colNumber, string newText)
        {
            Запис запис = Записи[rowNumber];
            switch ((Columns)colNumber)
            {
                case Columns.Ціна: { var (check, value) = Validate.IsDecimal(newText); if (check) запис.Ціна = value; break; }
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
                    case Columns.Ціна:
                        {
                            cellText.Text = запис.Ціна.ToString();
                            break;
                        }
                    default: break;
                }
            }
        }

        #endregion

        #region ToolBar

        protected override void AddRecord()
        {
            Запис запис = new Запис();
            Записи.Add(запис);

            TreeIter iter = Store.AppendValues(запис.ToArray());
            TreeViewGrid.SetCursor(Store.GetPath(iter), TreeViewGrid.Columns[0], false);
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

        async void OnFillDirectory(object? sender, EventArgs args)
        {
            ОбновитиЗначенняДокумента?.Invoke();

            string query = $@"
SELECT
    Номенклатура.uid AS Номенклатура,
    Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    Номенклатура.{Номенклатура_Const.ОдиницяВиміру} AS Пакування,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS Пакування_Назва,
    (
        SELECT
            {ЦіниНоменклатури_Const.Ціна}
        FROM
            {ЦіниНоменклатури_Const.TABLE} AS ЦіниНоменклатури
        WHERE
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.Номенклатура} = Номенклатура.uid AND
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.ХарактеристикаНоменклатури} = '{Guid.Empty}' AND
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.Пакування} = Номенклатура.{Номенклатура_Const.ОдиницяВиміру} AND
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.ВидЦіни} = @vid_cen AND
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.Валюта} = @valuta
        ORDER BY ЦіниНоменклатури.period DESC
        LIMIT 1
    ) AS Ціна
FROM
    {Номенклатура_Const.TABLE} AS Номенклатура
    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON 
        Довідник_ПакуванняОдиниціВиміру.uid = Номенклатура.{Номенклатура_Const.ОдиницяВиміру}
WHERE
    Номенклатура.{Номенклатура_Const.ТипНоменклатури} = {(int)ТипиНоменклатури.Товар} OR
    Номенклатура.{Номенклатура_Const.ТипНоменклатури} = {(int)ТипиНоменклатури.Послуга}
ORDER BY Номенклатура_Назва, Пакування_Назва
";
            Store.Clear();
            Записи.Clear();

            if (ЕлементВласник != null)
            {
                Dictionary<string, object> paramQuery = new Dictionary<string, object>
                {
                    { "valuta", ЕлементВласник.Валюта.UnigueID.UGuid },
                    { "vid_cen", ЕлементВласник.ВидЦіни.UnigueID.UGuid }
                };

                var recordResult = await Config.Kernel.DataBase.SelectRequest(query, paramQuery);

                string ВидЦіниНазва = await ЕлементВласник.ВидЦіни.GetPresentation();

                foreach (Dictionary<string, object> row in recordResult.ListRow)
                {
                    Запис запис = new Запис
                    {
                        ID = Guid.Empty,
                        Номенклатура = new Номенклатура_Pointer(row["Номенклатура"]),
                        ХарактеристикаНоменклатури = new ХарактеристикиНоменклатури_Pointer(),
                        Пакування = new ПакуванняОдиниціВиміру_Pointer(row["Пакування"]),
                        ВидЦіни = ЕлементВласник.ВидЦіни,
                        Ціна = row["Ціна"] != DBNull.Value ? (decimal)row["Ціна"] : 0
                    };

                    запис.Номенклатура.Назва = row["Номенклатура_Назва"].ToString() ?? "";
                    запис.Пакування.Назва = row["Пакування_Назва"].ToString() ?? "";
                    запис.ВидЦіни.Назва = ВидЦіниНазва;

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }
        }

        async void OnFillRegister(object? sender, EventArgs args)
        {
            ОбновитиЗначенняДокумента?.Invoke();

            string query = $@"
WITH register AS
(
    SELECT DISTINCT {ЦіниНоменклатури_Const.Номенклатура} AS Номенклатура,
        {ЦіниНоменклатури_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        {ЦіниНоменклатури_Const.Пакування} AS Пакування,
        {ЦіниНоменклатури_Const.ВидЦіни} AS ВидЦіни
    FROM
        {ЦіниНоменклатури_Const.TABLE}
    WHERE
        {ЦіниНоменклатури_Const.Валюта} = @valuta";

            #region WHERE

            if (ЕлементВласник != null && !ЕлементВласник.ВидЦіни.IsEmpty())
            {
                query += $@"
AND {ЦіниНоменклатури_Const.ВидЦіни} = @vid_cen
";
            }

            #endregion

            query += $@"
)
SELECT
    register.Номенклатура,
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    register.ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    register.Пакування,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS Пакування_Назва,
    register.ВидЦіни,
    Довідник_ВидиЦін.{ВидиЦін_Const.Назва} AS ВидЦіни_Назва,
    (
        SELECT 
            {ЦіниНоменклатури_Const.Ціна}
        FROM 
            {ЦіниНоменклатури_Const.TABLE} AS ЦіниНоменклатури
        WHERE
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.Номенклатура} = register.Номенклатура AND
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.ХарактеристикаНоменклатури} = register.ХарактеристикаНоменклатури AND
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.Пакування} = register.Пакування AND
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.ВидЦіни} = register.ВидЦіни AND
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.Валюта} = @valuta
        ORDER BY ЦіниНоменклатури.period DESC
        LIMIT 1
    ) AS Ціна
FROM
    register
    
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON 
        Довідник_Номенклатура.uid = register.Номенклатура
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON 
        Довідник_ХарактеристикиНоменклатури.uid = register.ХарактеристикаНоменклатури
    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON 
        Довідник_ПакуванняОдиниціВиміру.uid = register.Пакування
    LEFT JOIN {ВидиЦін_Const.TABLE} AS Довідник_ВидиЦін ON 
        Довідник_ВидиЦін.uid = register.ВидЦіни
ORDER BY
    Номенклатура_Назва, ХарактеристикаНоменклатури_Назва, Пакування_Назва, ВидЦіни_Назва
";

            Store.Clear();
            Записи.Clear();

            if (ЕлементВласник != null)
            {
                Dictionary<string, object> paramQuery = new Dictionary<string, object>
                {
                    { "valuta", ЕлементВласник.Валюта.UnigueID.UGuid },
                    { "vid_cen", ЕлементВласник.ВидЦіни.UnigueID.UGuid }
                };

                var recordResult = await Config.Kernel.DataBase.SelectRequest(query, paramQuery);

                foreach (Dictionary<string, object> row in recordResult.ListRow)
                {
                    Запис запис = new Запис
                    {
                        ID = Guid.Empty,
                        Номенклатура = new Номенклатура_Pointer(row["Номенклатура"]),
                        ХарактеристикаНоменклатури = new ХарактеристикиНоменклатури_Pointer(row["ХарактеристикаНоменклатури"]),
                        Пакування = new ПакуванняОдиниціВиміру_Pointer(row["Пакування"]),
                        ВидЦіни = new ВидиЦін_Pointer(row["ВидЦіни"]),
                        Ціна = (decimal)row["Ціна"]
                    };

                    запис.Номенклатура.Назва = row["Номенклатура_Назва"].ToString() ?? "";
                    запис.Пакування.Назва = row["Пакування_Назва"].ToString() ?? "";
                    запис.ВидЦіни.Назва = row["ВидЦіни_Назва"].ToString() ?? "";

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }
        }

        #endregion
    }
}
