
/*
        ВведенняЗалишків_ТабличнаЧастина_РозрахункиЗКонтрагентами.cs
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
    class ВведенняЗалишків_ТабличнаЧастина_РозрахункиЗКонтрагентами : ДокументТабличнаЧастина
    {
        public ВведенняЗалишків_Objest? ЕлементВласник { get; set; }

        #region Записи

        enum Columns
        {
            НомерРядка,
            ТипКонтрагента,
            Контрагент,
            Валюта,
            Сума,
        }

        ListStore Store = new ListStore([

            typeof(int), //НомерРядка
            typeof(string), //ТипКонтрагента
            typeof(string), //Контрагент
            typeof(string), //Валюта
            typeof(float), //Сума
        ]);

        List<Запис> Записи = [];

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            public int НомерРядка { get; set; } = 0;
            public ТипиКонтрагентів ТипКонтрагента { get; set; } = 0;
            public Контрагенти_Pointer Контрагент { get; set; } = new Контрагенти_Pointer();
            public Валюти_Pointer Валюта { get; set; } = new Валюти_Pointer();
            public decimal Сума { get; set; } = 0;

            public object[] ToArray()
            {
                return
                [
                    НомерРядка,
                    ПсевдонімиПерелічення.ТипиКонтрагентів_Alias(ТипКонтрагента),
                    Контрагент.Назва,
                    Валюта.Назва,
                    (float)Сума,
                ];
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    НомерРядка = запис.НомерРядка,
                    ТипКонтрагента = запис.ТипКонтрагента,
                    Контрагент = запис.Контрагент.Copy(),
                    Валюта = запис.Валюта.Copy(),
                    Сума = запис.Сума,
                };
            }

            public static async ValueTask ПісляЗміни_Контрагент(Запис запис)
            {
                await запис.Контрагент.GetPresentation();
            }

            public static async ValueTask ПісляЗміни_Валюта(Запис запис)
            {
                await запис.Валюта.GetPresentation();
            }

        }

        #endregion

        Label ПідсумокСума = new Label() { Selectable = true };

        public ВведенняЗалишків_ТабличнаЧастина_РозрахункиЗКонтрагентами()
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

            base.PackStart(hBox, false, false, 2);
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

            //ТипКонтрагента
            {
                ListStore store = new ListStore(typeof(string), typeof(string));

                foreach (var field in ПсевдонімиПерелічення.ТипиКонтрагентів_List())
                    store.AppendValues(field.Value.ToString(), field.Name);

                CellRendererCombo cellCombo = new CellRendererCombo() { Editable = true, Model = store, TextColumn = 1 };
                cellCombo.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("ТипКонтрагента", cellCombo, "text", (int)Columns.ТипКонтрагента) { Resizable = true, MinWidth = 100 };

                SetColIndex(column, Columns.ТипКонтрагента);
                TreeViewGrid.AppendColumn(column);
            }

            //Контрагент
            {
                TreeViewColumn column = new TreeViewColumn("Контрагент", new CellRendererText(), "text", (int)Columns.Контрагент) { Resizable = true, MinWidth = 200 };

                SetColIndex(column, Columns.Контрагент);
                TreeViewGrid.AppendColumn(column);
            }

            //Валюта
            {
                TreeViewColumn column = new TreeViewColumn("Валюта", new CellRendererText(), "text", (int)Columns.Валюта) { Resizable = true, MinWidth = 200 };

                SetColIndex(column, Columns.Валюта);
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
                ЕлементВласник.РозрахункиЗКонтрагентами_TablePart.FillJoin([ВведенняЗалишків_РозрахункиЗКонтрагентами_TablePart.НомерРядка]);
                await ЕлементВласник.РозрахункиЗКонтрагентами_TablePart.Read();

                foreach (ВведенняЗалишків_РозрахункиЗКонтрагентами_TablePart.Record record in ЕлементВласник.РозрахункиЗКонтрагентами_TablePart.Records)
                {
                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        НомерРядка = record.НомерРядка,
                        ТипКонтрагента = record.ТипКонтрагента,
                        Контрагент = record.Контрагент,
                        Валюта = record.Валюта,
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
                ЕлементВласник.РозрахункиЗКонтрагентами_TablePart.Records.Clear();
                int sequenceNumber = 0;
                foreach (Запис запис in Записи)
                {
                    ЕлементВласник.РозрахункиЗКонтрагентами_TablePart.Records.Add(new ВведенняЗалишків_РозрахункиЗКонтрагентами_TablePart.Record()
                    {
                        UID = запис.ID,
                        НомерРядка = ++sequenceNumber,
                        ТипКонтрагента = запис.ТипКонтрагента,
                        Контрагент = запис.Контрагент,
                        Валюта = запис.Валюта,
                        Сума = запис.Сума,
                    });
                }

                await ЕлементВласник.РозрахункиЗКонтрагентами_TablePart.Save(true);
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
                    ключовіСлова += $"\n{++sequenceNumber}. {запис.ТипКонтрагента} {запис.Контрагент.Назва} {запис.Валюта.Назва}";
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
                case Columns.Контрагент:
                    {
                        Контрагенти_ШвидкийВибір page = new()
                        {
                            DirectoryPointerItem = запис.Контрагент.UnigueID,
                            CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                            {
                                запис.Контрагент = new Контрагенти_Pointer(selectPointer);
                                await Запис.ПісляЗміни_Контрагент(запис);
                                Store.SetValues(iter, запис.ToArray());
                            }
                        };
                        return page;
                    }
                case Columns.Валюта:
                    {
                        Валюти_ШвидкийВибір page = new()
                        {
                            DirectoryPointerItem = запис.Валюта.UnigueID,
                            CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                            {
                                запис.Валюта = new Валюти_Pointer(selectPointer);
                                await Запис.ПісляЗміни_Валюта(запис);
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
                case Columns.Контрагент: { запис.Контрагент.Clear(); break; }
                case Columns.Валюта: { запис.Валюта.Clear(); break; }

                default: break;
            }
            Store.SetValues(iter, запис.ToArray());
        }

        protected override void ChangeCell(TreeIter iter, int rowNumber, int colNumber, string newText)
        {
            Запис запис = Записи[rowNumber];
            switch ((Columns)colNumber)
            {
                case Columns.ТипКонтрагента: { запис.ТипКонтрагента = ПсевдонімиПерелічення.ТипиКонтрагентів_FindByName(newText) ?? 0; break; }
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
