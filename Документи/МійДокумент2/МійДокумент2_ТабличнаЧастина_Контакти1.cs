
/*
        МійДокумент2_ТабличнаЧастина_Контакти1.cs
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
    class МійДокумент2_ТабличнаЧастина_Контакти1 : ДокументТабличнаЧастина
    {
        public МійДокумент2_Objest? ЕлементВласник { get; set; }

        #region Записи

        enum Columns
        {
            Поле1,
            Поле2,
            Поле3,
            Поле4,
            Поле5,
            Поле6,
            Поле7,
            Поле8,
            Поле9,
            ПолеПоступлення,
            КомбоБох,

        }

        ListStore Store = new ListStore([

            typeof(string), //Поле1
            typeof(string), //Поле2
            typeof(int), //Поле3
            typeof(float), //Поле4
            typeof(bool), //Поле5
            typeof(string), //Поле6
            typeof(string), //Поле7
            typeof(string), //Поле8
            typeof(string), //Поле9
            typeof(string), //ПолеПоступлення
            typeof(string), //КомбоБох
        ]);

        List<Запис> Записи = [];

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            public string Поле1 { get; set; } = "";
            public string Поле2 { get; set; } = "";
            public int Поле3 { get; set; } = 0;
            public decimal Поле4 { get; set; } = 0;
            public bool Поле5 { get; set; } = false;
            public Номенклатура_Pointer Поле6 { get; set; } = new Номенклатура_Pointer();
            public DateTime Поле7 { get; set; } = DateTime.MinValue;
            public DateTime Поле8 { get; set; } = DateTime.MinValue;
            public TimeSpan Поле9 { get; set; } = DateTime.MinValue.TimeOfDay;
            public ПоступленняТоварівТаПослуг_Pointer ПолеПоступлення { get; set; } = new ПоступленняТоварівТаПослуг_Pointer();
            public ТипиКонтактноїІнформації КомбоБох { get; set; } = 0;


            public object[] ToArray()
            {
                return
                [
                    Поле1,
                    Поле2,
                    Поле3,
                    (float)Поле4,
                    Поле5,
                    Поле6.Назва,
                    Поле7.ToString(),
                    Поле8.ToString(),
                    Поле9.ToString(),
                    ПолеПоступлення.Назва,
                    ПсевдонімиПерелічення.ТипиКонтактноїІнформації_Alias(КомбоБох),

                ];
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    Поле1 = запис.Поле1,
                    Поле2 = запис.Поле2,
                    Поле3 = запис.Поле3,
                    Поле4 = запис.Поле4,
                    Поле5 = запис.Поле5,
                    Поле6 = запис.Поле6.Copy(),
                    Поле7 = запис.Поле7,
                    Поле8 = запис.Поле8,
                    Поле9 = запис.Поле9,
                    ПолеПоступлення = запис.ПолеПоступлення.Copy(),
                    КомбоБох = запис.КомбоБох,

                };
            }

            public static async ValueTask ПісляЗміни_Поле6(Запис запис)
            {
                await запис.Поле6.GetPresentation();
            }

            public static async ValueTask ПісляЗміни_ПолеПоступлення(Запис запис)
            {
                await запис.ПолеПоступлення.GetPresentation();
            }

        }

        #endregion

        public МійДокумент2_ТабличнаЧастина_Контакти1()
        {
            TreeViewGrid.Model = Store;
            AddColumn();
        }

        void AddColumn()
        {

            //Поле1
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("Поле1", cellText, "text", (int)Columns.Поле1) { Resizable = true, MinWidth = 100 };

                column.Data.Add("Column", Columns.Поле1);
                TreeViewGrid.AppendColumn(column);
            }

            //Поле2
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("Поле2", cellText, "text", (int)Columns.Поле2) { Resizable = true, MinWidth = 100 };

                column.Data.Add("Column", Columns.Поле2);
                TreeViewGrid.AppendColumn(column);
            }

            //Поле3
            {

                CellRendererText cellNumber = new CellRendererText() { Editable = true };
                cellNumber.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("Поле3", cellNumber, "text", (int)Columns.Поле3) { Resizable = true, MinWidth = 100 };
                column.SetCellDataFunc(cellNumber, new TreeCellDataFunc(NumericCellDataFunc));

                column.Data.Add("Column", Columns.Поле3);
                TreeViewGrid.AppendColumn(column);
            }

            //Поле4
            {

                CellRendererText cellNumber = new CellRendererText() { Editable = true };
                cellNumber.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("Поле4", cellNumber, "text", (int)Columns.Поле4) { Resizable = true, MinWidth = 100 };
                column.SetCellDataFunc(cellNumber, new TreeCellDataFunc(NumericCellDataFunc));

                column.Data.Add("Column", Columns.Поле4);
                TreeViewGrid.AppendColumn(column);
            }

            //Поле5
            {

                CellRendererToggle cellToggle = new CellRendererToggle() { };
                cellToggle.Toggled += EditCell;
                TreeViewColumn column = new TreeViewColumn("Поле5", cellToggle, "active", (int)Columns.Поле5);

                column.Data.Add("Column", Columns.Поле5);
                TreeViewGrid.AppendColumn(column);
            }

            //Поле6
            {

                TreeViewColumn column = new TreeViewColumn("Поле6", new CellRendererText(), "text", (int)Columns.Поле6) { Resizable = true, MinWidth = 200 };

                column.Data.Add("Column", Columns.Поле6);
                TreeViewGrid.AppendColumn(column);
            }

            //Поле7
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("Поле7", cellText, "text", (int)Columns.Поле7) { Resizable = true, MinWidth = 100 };

                column.Data.Add("Column", Columns.Поле7);
                TreeViewGrid.AppendColumn(column);
            }

            //Поле8
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("Поле8", cellText, "text", (int)Columns.Поле8) { Resizable = true, MinWidth = 100 };

                column.Data.Add("Column", Columns.Поле8);
                TreeViewGrid.AppendColumn(column);
            }

            //Поле9
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("Поле9", cellText, "text", (int)Columns.Поле9) { Resizable = true, MinWidth = 100 };

                column.Data.Add("Column", Columns.Поле9);
                TreeViewGrid.AppendColumn(column);
            }

            //ПолеПоступлення
            {

                TreeViewColumn column = new TreeViewColumn("ПолеПоступлення", new CellRendererText(), "text", (int)Columns.ПолеПоступлення) { Resizable = true, MinWidth = 200 };

                column.Data.Add("Column", Columns.ПолеПоступлення);
                TreeViewGrid.AppendColumn(column);
            }

            //КомбоБох
            {

                ListStore store = new ListStore(typeof(string), typeof(string));

                foreach (var field in ПсевдонімиПерелічення.ТипиКонтактноїІнформації_List())
                    store.AppendValues(field.Value.ToString(), field.Name);

                CellRendererCombo cellCombo = new CellRendererCombo() { Editable = true, Model = store, TextColumn = 1 };
                cellCombo.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("КомбоБох", cellCombo, "text", (int)Columns.КомбоБох) { Resizable = true, MinWidth = 100 };

                column.Data.Add("Column", Columns.КомбоБох);
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
                ЕлементВласник.Контакти1_TablePart.FillJoin([]);
                await ЕлементВласник.Контакти1_TablePart.Read();

                foreach (МійДокумент2_Контакти1_TablePart.Record record in ЕлементВласник.Контакти1_TablePart.Records)
                {
                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        Поле1 = record.Поле1,
                        Поле2 = record.Поле2,
                        Поле3 = record.Поле3,
                        Поле4 = record.Поле4,
                        Поле5 = record.Поле5,
                        Поле6 = record.Поле6,
                        Поле7 = record.Поле7,
                        Поле8 = record.Поле8,
                        Поле9 = record.Поле9,
                        ПолеПоступлення = record.ПолеПоступлення,
                        КомбоБох = record.КомбоБох,

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
                ЕлементВласник.Контакти1_TablePart.Records.Clear();

                foreach (Запис запис in Записи)
                {
                    ЕлементВласник.Контакти1_TablePart.Records.Add(new МійДокумент2_Контакти1_TablePart.Record()
                    {
                        UID = запис.ID,
                        Поле1 = запис.Поле1,
                        Поле2 = запис.Поле2,
                        Поле3 = запис.Поле3,
                        Поле4 = запис.Поле4,
                        Поле5 = запис.Поле5,
                        Поле6 = запис.Поле6,
                        Поле7 = запис.Поле7,
                        Поле8 = запис.Поле8,
                        Поле9 = запис.Поле9,
                        ПолеПоступлення = запис.ПолеПоступлення,
                        КомбоБох = запис.КомбоБох,

                    });
                }

                await ЕлементВласник.Контакти1_TablePart.Save(true);
                await LoadRecords();
            }
        }

        #endregion

        #region Func

        protected override ФормаЖурнал? OpenSelect(TreeIter iter, int rowNumber, int colNumber)
        {
            Запис запис = Записи[rowNumber];
            switch ((Columns)colNumber)
            {
                case Columns.Поле6:
                    {
                        Номенклатура_ШвидкийВибір page = new()
                        {
                            DirectoryPointerItem = запис.Поле6.UnigueID,
                            CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                            {
                                запис.Поле6 = new Номенклатура_Pointer(selectPointer);
                                await Запис.ПісляЗміни_Поле6(запис);
                                Store.SetValues(iter, запис.ToArray());
                            }
                        };
                        return page;
                    }
                case Columns.ПолеПоступлення:
                    {
                        ПоступленняТоварівТаПослуг page = new()
                        {
                            DocumentPointerItem = запис.ПолеПоступлення.UnigueID,
                            CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                            {
                                запис.ПолеПоступлення = new ПоступленняТоварівТаПослуг_Pointer(selectPointer);
                                await Запис.ПісляЗміни_ПолеПоступлення(запис);
                                Store.SetValues(iter, запис.ToArray());
                            }
                        };
                        return page;
                    }

                default: return null;
            }
        }

        protected override void ChangeCell(TreeIter iter, int rowNumber, int colNumber, string newText)
        {
            Запис запис = Записи[rowNumber];
            switch ((Columns)colNumber)
            {
                case Columns.Поле1: { запис.Поле1 = newText; break; }
                case Columns.Поле2: { запис.Поле2 = newText; break; }
                case Columns.Поле3: { var (check, value) = Validate.IsInt(newText); if (check) запис.Поле3 = value; break; }
                case Columns.Поле4: { var (check, value) = Validate.IsDecimal(newText); if (check) запис.Поле4 = value; break; }
                case Columns.КомбоБох: { запис.КомбоБох = ПсевдонімиПерелічення.ТипиКонтактноїІнформації_FindByName(newText) ?? 0; break; }

                default: break;
            }
            Store.SetValues(iter, запис.ToArray());
        }

        protected override void ChangeCell(TreeIter iter, int rowNumber, int colNumber, bool newValue)
        {
            Запис запис = Записи[rowNumber];
            switch ((Columns)colNumber)
            {
                case Columns.Поле5: { запис.Поле5 = newValue; break; }

                default: break;
            }
            Store.SetValues(iter, запис.ToArray());
        }

        protected override void ClearCell(TreeIter iter, int rowNumber, int colNumber)
        {
            Запис запис = Записи[rowNumber];
            switch ((Columns)colNumber)
            {
                case Columns.Поле6: { запис.Поле6.Clear(); break; }
                case Columns.ПолеПоступлення: { запис.ПолеПоступлення.Clear(); break; }

                default: break;
            }
            Store.SetValues(iter, запис.ToArray());
        }


        void NumericCellDataFunc(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            object? objColumn = column.Data["Column"];
            if (objColumn != null)
            {
                int colNumber = (int)objColumn;
                int rowNumber = int.Parse(Store.GetPath(iter).ToString());
                Запис запис = Записи[rowNumber];

                CellRendererText cellText = (CellRendererText)cell;
                cellText.Foreground = "green";

                switch ((Columns)colNumber)
                {

                    case Columns.Поле3:
                        {
                            cellText.Text = запис.Поле3.ToString();
                            break;
                        }

                    case Columns.Поле4:
                        {
                            cellText.Text = запис.Поле4.ToString();
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

        #endregion

    }
}
