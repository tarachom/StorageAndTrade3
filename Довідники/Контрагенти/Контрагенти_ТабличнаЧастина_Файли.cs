
/*
        Контрагенти_ТабличнаЧастина_Файли.cs
        Таблична Частина
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;
using GeneratedCode.Документи;
using GeneratedCode.Перелічення;

namespace StorageAndTrade
{
    class Контрагенти_ТабличнаЧастина_Файли : ДовідникТабличнаЧастина
    {
        public Контрагенти_Objest? ЕлементВласник { get; set; }

        #region Записи

        enum Columns
        {
            Image,
            Файл,
        }

        ListStore Store = new ListStore([
            typeof(Gdk.Pixbuf), /* Image */
            typeof(string), //Файл
        ]);

        List<Запис> Записи = [];

        private class Запис
        {
            public Gdk.Pixbuf Image { get; set; } = InterfaceGtk3.Іконки.ДляКнопок.Doc;
            public Guid ID { get; set; } = Guid.Empty;
            public Файли_Pointer Файл { get; set; } = new Файли_Pointer();


            public object[] ToArray()
            {
                return
                [
                    Image,
                    Файл.Назва,
                ];
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    Файл = запис.Файл.Copy(),
                };
            }

            public static async ValueTask ПісляЗміни_Файл(Запис запис)
            {
                await запис.Файл.GetPresentation();
            }

        }

        #endregion

        public Контрагенти_ТабличнаЧастина_Файли()
        {
            TreeViewGrid.Model = Store;
            AddColumn();
        }

        void AddColumn()
        {
            TreeViewGrid.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", (int)Columns.Image));

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
            if (ЕлементВласник != null)
            {
                ЕлементВласник.Файли_TablePart.FillJoin([]);
                await ЕлементВласник.Файли_TablePart.Read();

                Записи.Clear();
                Store.Clear();

                foreach (Контрагенти_Файли_TablePart.Record record in ЕлементВласник.Файли_TablePart.Records)
                {
                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        Файл = record.Файл,
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
                ЕлементВласник.Файли_TablePart.Records.Clear();

                foreach (Запис запис in Записи)
                {
                    ЕлементВласник.Файли_TablePart.Records.Add(new Контрагенти_Файли_TablePart.Record()
                    {
                        UID = запис.ID,
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
