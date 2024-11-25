
/*
        Номенклатура_ТабличнаЧастина_Файли.cs
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
    class Номенклатура_ТабличнаЧастина_Файли : ДовідникТабличнаЧастина
    {
        public Номенклатура_Objest? ЕлементВласник { get; set; }

        #region Записи

        enum Columns
        {
            Image,
            Основний,
            Файл,
        }

        ListStore Store = new ListStore([
            typeof(Gdk.Pixbuf), /* Image */
            typeof(bool), //Основний
            typeof(string), //Файл
        ]);

        List<Запис> Записи = [];

        private class Запис
        {
            public Gdk.Pixbuf Image { get; set; } = InterfaceGtk.Іконки.ДляКнопок.Doc;
            public Guid ID { get; set; } = Guid.Empty;
            public bool Основний { get; set; } = false;
            public Файли_Pointer Файл { get; set; } = new Файли_Pointer();


            public object[] ToArray()
            {
                return
                [
                    Image,
                    Основний,
                    Файл.Назва,
                ];
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    Основний = запис.Основний,
                    Файл = запис.Файл.Copy(),
                };
            }

            public static async ValueTask ПісляЗміни_Файл(Запис запис)
            {
                await запис.Файл.GetPresentation();
            }

        }

        #endregion

        public Номенклатура_ТабличнаЧастина_Файли()
        {
            TreeViewGrid.Model = Store;
            AddColumn();
        }

        void AddColumn()
        {
            TreeViewGrid.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", (int)Columns.Image));

            //Основний
            {
                CellRendererToggle cellToggle = new CellRendererToggle() { };
                cellToggle.Toggled += EditCell;
                TreeViewColumn column = new TreeViewColumn("Основний", cellToggle, "active", (int)Columns.Основний);

                column.Data.Add("Column", Columns.Основний);
                TreeViewGrid.AppendColumn(column);
            }

            //Файл
            {
                TreeViewColumn column = new TreeViewColumn("Файл", new CellRendererText(), "text", (int)Columns.Файл) { Resizable = true, MinWidth = 200 };

                column.Data.Add("Column", Columns.Файл);
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
                ЕлементВласник.Файли_TablePart.FillJoin([]);
                await ЕлементВласник.Файли_TablePart.Read();

                foreach (Номенклатура_Файли_TablePart.Record record in ЕлементВласник.Файли_TablePart.Records)
                {
                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        Основний = record.Основний,
                        Файл = record.Файл,

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
                ЕлементВласник.Файли_TablePart.Records.Clear();

                foreach (Запис запис in Записи)
                {
                    ЕлементВласник.Файли_TablePart.Records.Add(new Номенклатура_Файли_TablePart.Record()
                    {
                        UID = запис.ID,
                        Основний = запис.Основний,
                        Файл = запис.Файл,

                    });
                }

                await ЕлементВласник.Файли_TablePart.Save(true);
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
                case Columns.Файл:
                    {
                        Файли_ШвидкийВибір page = new()
                        {
                            DirectoryPointerItem = запис.Файл.UnigueID,
                            CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                            {
                                запис.Файл = new Файли_Pointer(selectPointer);
                                await Запис.ПісляЗміни_Файл(запис);
                                Store.SetValues(iter, запис.ToArray());
                            },
                            CallBack_OnMultipleSelectPointer = async (UnigueID[] selectPointers) =>
                            {
                                foreach (var selectPointer in selectPointers)
                                {
                                    (Запис запис, TreeIter iter) = НовийЗапис();

                                    запис.Файл = new Файли_Pointer(selectPointer);
                                    await Запис.ПісляЗміни_Файл(запис);
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
                case Columns.Файл: { запис.Файл.Clear(); break; }

                default: break;
            }
            Store.SetValues(iter, запис.ToArray());
        }

        protected override void ChangeCell(TreeIter iter, int rowNumber, int colNumber, bool newValue)
        {
            Запис запис = Записи[rowNumber];
            switch ((Columns)colNumber)
            {
                case Columns.Основний: { запис.Основний = newValue; break; }

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

        #endregion
    }
}
