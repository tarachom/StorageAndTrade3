
/*
        РозміщенняНоменклатуриПоКоміркам_ТабличнаЧастина_Товари.cs
        Таблична Частина
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using GeneratedCode;
using GeneratedCode.Довідники;
using GeneratedCode.Документи;
using GeneratedCode.Перелічення;
using GeneratedCode.РегістриВідомостей;
using GeneratedCode.Константи;

namespace StorageAndTrade
{
    class РозміщенняНоменклатуриПоКоміркам_ТабличнаЧастина_Товари : ДокументТабличнаЧастина
    {
        public РозміщенняНоменклатуриПоКоміркам_Objest? ЕлементВласник { get; set; }

        #region Записи

        enum Columns
        {
            НомерРядка,
            Номенклатура,
            Пакування,
            Комірка,
        }

        ListStore Store = new ListStore([

            typeof(int), //НомерРядка
            typeof(string), //Номенклатура
            typeof(string), //Пакування
            typeof(string), //Комірка
        ]);

        List<Запис> Записи = [];

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            public int НомерРядка { get; set; } = 0;
            public Номенклатура_Pointer Номенклатура { get; set; } = new Номенклатура_Pointer();
            public ПакуванняОдиниціВиміру_Pointer Пакування { get; set; } = new ПакуванняОдиниціВиміру_Pointer();
            public СкладськіКомірки_Pointer Комірка { get; set; } = new СкладськіКомірки_Pointer();

            public object[] ToArray()
            {
                return
                [
                    НомерРядка,
                    Номенклатура.Назва,
                    Пакування.Назва,
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
                    Пакування = запис.Пакування.Copy(),
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

            public static async ValueTask ПісляЗміни_Пакування(Запис запис)
            {
                await запис.Пакування.GetPresentation();
            }

            public static async ValueTask ПісляЗміни_Комірка(Запис запис)
            {
                await запис.Комірка.GetPresentation();
            }
        }

        #endregion

        public РозміщенняНоменклатуриПоКоміркам_ТабличнаЧастина_Товари()
        {
            TreeViewGrid.Model = Store;
            AddColumn();

            //
            //
            //

            ToolButton fillDirectoryButton = new ToolButton(new Image(Stock.Add, IconSize.Menu), "Заповнити товарами") { IsImportant = true };
            fillDirectoryButton.Clicked += OnFillDirectory;
            ToolbarTop.Add(fillDirectoryButton);

            ToolButton fillRegisterButton = new ToolButton(new Image(Stock.Add, IconSize.Menu), "Заповнити комірками") { IsImportant = true };
            fillRegisterButton.Clicked += OnFillRegister;
            ToolbarTop.Add(fillRegisterButton);
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

            //Пакування
            {
                TreeViewColumn column = new TreeViewColumn("Пакування", new CellRendererText(), "text", (int)Columns.Пакування) { Resizable = true, MinWidth = 100 };

                SetColIndex(column, Columns.Пакування);
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
                ЕлементВласник.Товари_TablePart.FillJoin([РозміщенняНоменклатуриПоКоміркам_Товари_TablePart.НомерРядка]);
                await ЕлементВласник.Товари_TablePart.Read();

                Записи.Clear();
                Store.Clear();

                foreach (РозміщенняНоменклатуриПоКоміркам_Товари_TablePart.Record record in ЕлементВласник.Товари_TablePart.Records)
                {
                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        НомерРядка = record.НомерРядка,
                        Номенклатура = record.Номенклатура,
                        Пакування = record.Пакування,
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
                int sequenceNumber = 0;
                foreach (Запис запис in Записи)
                {
                    ЕлементВласник.Товари_TablePart.Records.Add(new РозміщенняНоменклатуриПоКоміркам_Товари_TablePart.Record()
                    {
                        UID = запис.ID,
                        НомерРядка = ++sequenceNumber,
                        Номенклатура = запис.Номенклатура,
                        Пакування = запис.Пакування,
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
                    ключовіСлова += $"\n{++sequenceNumber}. {запис.Номенклатура.Назва} {запис.Комірка.Назва} ";
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
                case Columns.Пакування: { запис.Пакування.Clear(); break; }
                case Columns.Комірка: { запис.Комірка.Clear(); break; }

                default: break;
            }
            Store.SetValues(iter, запис.ToArray());
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

        async void OnFillDirectory(object? sender, EventArgs args)
        {
            string query = $@"
SELECT
    Номенклатура.uid AS Номенклатура,
    Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    Номенклатура.{Номенклатура_Const.ОдиницяВиміру} AS Пакування,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS Пакування_Назва,
    (
        SELECT
            {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.Комірка}
        FROM
            {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.TABLE} AS РозміщенняНоменклатуриПоКоміркамНаСкладі
        WHERE
            РозміщенняНоменклатуриПоКоміркамНаСкладі.{РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.Номенклатура} = Номенклатура.uid
        ORDER BY РозміщенняНоменклатуриПоКоміркамНаСкладі.period DESC
        LIMIT 1
    ) AS Комірка,
    Довідник_СкладськіКомірки.{СкладськіКомірки_Const.Назва} AS Комірка_Назва
FROM
    {Номенклатура_Const.TABLE} AS Номенклатура
    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON 
        Довідник_ПакуванняОдиниціВиміру.uid = Номенклатура.{Номенклатура_Const.ОдиницяВиміру}
    LEFT JOIN {СкладськіКомірки_Const.TABLE} AS Довідник_СкладськіКомірки ON 
        Довідник_СкладськіКомірки.uid = 
        (
            SELECT
                {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.Комірка}
            FROM
                {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.TABLE} AS РозміщенняНоменклатуриПоКоміркамНаСкладі
            WHERE
                РозміщенняНоменклатуриПоКоміркамНаСкладі.{РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.Номенклатура} = Номенклатура.uid
            ORDER BY РозміщенняНоменклатуриПоКоміркамНаСкладі.period DESC
            LIMIT 1
        )
WHERE
    Номенклатура.{Номенклатура_Const.ТипНоменклатури} = {(int)ТипиНоменклатури.Товар}
ORDER BY Номенклатура_Назва
";
            Store.Clear();
            Записи.Clear();

            if (ЕлементВласник != null)
            {
                var recordResult = await Config.Kernel.DataBase.SelectRequest(query);

                foreach (Dictionary<string, object> row in recordResult.ListRow)
                {
                    Запис запис = new Запис
                    {
                        ID = Guid.Empty,
                        Номенклатура = new Номенклатура_Pointer(row["Номенклатура"]),
                        Пакування = new ПакуванняОдиниціВиміру_Pointer(row["Пакування"]),
                        Комірка = new СкладськіКомірки_Pointer(row["Комірка"])
                    };

                    запис.Номенклатура.Назва = row["Номенклатура_Назва"].ToString() ?? "";
                    запис.Пакування.Назва = row["Пакування_Назва"].ToString() ?? "";
                    запис.Комірка.Назва = row["Комірка_Назва"].ToString() ?? "";

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }
        }

        async void OnFillRegister(object? sender, EventArgs args)
        {
            string query = $@"
WITH register AS
(
    SELECT DISTINCT 
        {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.Номенклатура} AS Номенклатура
    FROM
        {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.TABLE}
)
SELECT
    register.Номенклатура,
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    Довідник_Номенклатура.{Номенклатура_Const.ОдиницяВиміру} AS Пакування,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS Пакування_Назва,
    (
        SELECT
            {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.Комірка}
        FROM
            {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.TABLE} AS РозміщенняНоменклатуриПоКоміркамНаСкладі
        WHERE
            РозміщенняНоменклатуриПоКоміркамНаСкладі.{РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.Номенклатура} = register.Номенклатура
        ORDER BY РозміщенняНоменклатуриПоКоміркамНаСкладі.period DESC
        LIMIT 1
    ) AS Комірка,
    Довідник_СкладськіКомірки.{СкладськіКомірки_Const.Назва} AS Комірка_Назва
FROM
    register

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON 
        Довідник_Номенклатура.uid = register.Номенклатура
    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON 
        Довідник_ПакуванняОдиниціВиміру.uid = Довідник_Номенклатура.{Номенклатура_Const.ОдиницяВиміру}
    LEFT JOIN {СкладськіКомірки_Const.TABLE} AS Довідник_СкладськіКомірки ON 
        Довідник_СкладськіКомірки.uid = 
        (
            SELECT
                {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.Комірка}
            FROM
                {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.TABLE} AS РозміщенняНоменклатуриПоКоміркамНаСкладі
            WHERE
                РозміщенняНоменклатуриПоКоміркамНаСкладі.{РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.Номенклатура} = register.Номенклатура
            ORDER BY РозміщенняНоменклатуриПоКоміркамНаСкладі.period DESC
            LIMIT 1
        )
ORDER BY
    Номенклатура_Назва
";

            Store.Clear();
            Записи.Clear();

            if (ЕлементВласник != null)
            {
                var recordResult = await Config.Kernel.DataBase.SelectRequest(query);

                foreach (Dictionary<string, object> row in recordResult.ListRow)
                {
                    Запис запис = new Запис
                    {
                        ID = Guid.Empty,
                        Номенклатура = new Номенклатура_Pointer(row["Номенклатура"]),
                        Пакування = new ПакуванняОдиниціВиміру_Pointer(row["Пакування"]),
                        Комірка = new СкладськіКомірки_Pointer(row["Комірка"])
                    };

                    запис.Номенклатура.Назва = row["Номенклатура_Назва"].ToString() ?? "";
                    запис.Пакування.Назва = row["Пакування_Назва"].ToString() ?? "";
                    запис.Комірка.Назва = row["Комірка_Назва"].ToString() ?? "";

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }
        }

        #endregion
    }
}
