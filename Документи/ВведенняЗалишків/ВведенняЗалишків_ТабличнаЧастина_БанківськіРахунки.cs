
/*
        ВведенняЗалишків_ТабличнаЧастина_БанківськіРахунки.cs
        Таблична Частина
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ВведенняЗалишків_ТабличнаЧастина_БанківськіРахунки : ДокументТабличнаЧастина
    {
        public ВведенняЗалишків_Objest? ЕлементВласник { get; set; }

        #region Записи

        enum Columns
        {
            НомерРядка,
            БанківськийРахунок,
            Сума,
        }

        ListStore Store = new ListStore([

            typeof(int), //НомерРядка
            typeof(string), //БанківськийРахунок
            typeof(float), //Сума
        ]);

        List<Запис> Записи = [];

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            public int НомерРядка { get; set; } = 0;
            public БанківськіРахункиОрганізацій_Pointer БанківськийРахунок { get; set; } = new БанківськіРахункиОрганізацій_Pointer();
            public decimal Сума { get; set; } = 0;

            public object[] ToArray()
            {
                return
                [
                    НомерРядка,
                    БанківськийРахунок.Назва,
                    (float)Сума,
                ];
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    НомерРядка = запис.НомерРядка,
                    БанківськийРахунок = запис.БанківськийРахунок.Copy(),
                    Сума = запис.Сума,
                };
            }

            public static async ValueTask ПісляЗміни_БанківськийРахунок(Запис запис)
            {
                await запис.БанківськийРахунок.GetPresentation();
            }
        }

        #endregion

        Label ПідсумокСума = new Label() { Selectable = true };

        public ВведенняЗалишків_ТабличнаЧастина_БанківськіРахунки()
        {
            TreeViewGrid.Model = Store;
            AddColumn();

            CreateBottomBlock();

            Store.RowChanged += (object? sender, RowChangedArgs args) => ОбчислитиПідсумки();
            Store.RowDeleted += (object? sender, RowDeletedArgs args) => ОбчислитиПідсумки();
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

        void AddColumn()
        {
            //НомерРядка
            {
                CellRendererText cellNumber = new CellRendererText() { Xalign = 1 };
                TreeViewColumn column = new TreeViewColumn("№", cellNumber, "text", (int)Columns.НомерРядка) { Resizable = true, MinWidth = 30 };
                TreeViewGrid.AppendColumn(column);
            }

            //БанківськийРахунок
            {
                TreeViewColumn column = new TreeViewColumn("Банківський рахунок", new CellRendererText(), "text", (int)Columns.БанківськийРахунок) { Resizable = true, MinWidth = 200 };

                SetColIndex(column, Columns.БанківськийРахунок);
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
                ЕлементВласник.БанківськіРахунки_TablePart.FillJoin([ВведенняЗалишків_БанківськіРахунки_TablePart.НомерРядка]);
                await ЕлементВласник.БанківськіРахунки_TablePart.Read();

                foreach (ВведенняЗалишків_БанківськіРахунки_TablePart.Record record in ЕлементВласник.БанківськіРахунки_TablePart.Records)
                {
                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        НомерРядка = record.НомерРядка,
                        БанківськийРахунок = record.БанківськийРахунок,
                        Сума = record.Сума,
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
                ЕлементВласник.БанківськіРахунки_TablePart.Records.Clear();
                int sequenceNumber = 0;
                foreach (Запис запис in Записи)
                {
                    ЕлементВласник.БанківськіРахунки_TablePart.Records.Add(new ВведенняЗалишків_БанківськіРахунки_TablePart.Record()
                    {
                        UID = запис.ID,
                        НомерРядка = ++sequenceNumber,
                        БанківськийРахунок = запис.БанківськийРахунок,
                        Сума = запис.Сума,
                    });
                }

                await ЕлементВласник.БанківськіРахунки_TablePart.Save(true);
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
                    ключовіСлова += $"\n{++sequenceNumber}. {запис.БанківськийРахунок.Назва}";
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
                case Columns.БанківськийРахунок:
                    {
                        БанківськіРахункиОрганізацій_ШвидкийВибір page = new()
                        {
                            DirectoryPointerItem = запис.БанківськийРахунок.UnigueID,
                            CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                            {
                                запис.БанківськийРахунок = new БанківськіРахункиОрганізацій_Pointer(selectPointer);
                                await Запис.ПісляЗміни_БанківськийРахунок(запис);
                                Store.SetValues(iter, запис.ToArray());
                            },
                            CallBack_OnMultipleSelectPointer = async (UnigueID[] selectPointers) =>
                            {
                                foreach (var selectPointer in selectPointers)
                                {
                                    (Запис запис, TreeIter iter) = НовийЗапис();

                                    запис.БанківськийРахунок = new БанківськіРахункиОрганізацій_Pointer(selectPointer);
                                    await Запис.ПісляЗміни_БанківськийРахунок(запис);
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
                case Columns.БанківськийРахунок: { запис.БанківськийРахунок.Clear(); break; }
                default: break;
            }
            Store.SetValues(iter, запис.ToArray());
        }

        protected override void ChangeCell(TreeIter iter, int rowNumber, int colNumber, string newText)
        {
            Запис запис = Записи[rowNumber];
            switch ((Columns)colNumber)
            {
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
