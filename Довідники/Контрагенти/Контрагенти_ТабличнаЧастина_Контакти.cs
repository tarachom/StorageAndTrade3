
/*
        Контрагенти_ТабличнаЧастина_Контакти.cs
        Таблична Частина
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using GeneratedCode.Довідники;
using GeneratedCode.Документи;
using GeneratedCode.Перелічення;

namespace StorageAndTrade
{
    class Контрагенти_ТабличнаЧастина_Контакти : ДовідникТабличнаЧастина
    {
        public Контрагенти_Objest? ЕлементВласник { get; set; }

        #region Записи

        enum Columns
        {
            Тип,
            Значення,
            Телефон,
            ЕлектроннаПошта,
            Країна,
            Область,
            Район,
            Місто,
        }

        ListStore Store = new ListStore([

            typeof(string), //Тип
            typeof(string), //Значення
            typeof(string), //Телефон
            typeof(string), //ЕлектроннаПошта
            typeof(string), //Країна
            typeof(string), //Область
            typeof(string), //Район
            typeof(string), //Місто
        ]);

        List<Запис> Записи = [];

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            public ТипиКонтактноїІнформації Тип { get; set; } = 0;
            public string Значення { get; set; } = "";
            public string Телефон { get; set; } = "";
            public string ЕлектроннаПошта { get; set; } = "";
            public string Країна { get; set; } = "";
            public string Область { get; set; } = "";
            public string Район { get; set; } = "";
            public string Місто { get; set; } = "";

            public object[] ToArray()
            {
                return
                [
                    ПсевдонімиПерелічення.ТипиКонтактноїІнформації_Alias(Тип),
                    Значення,
                    Телефон,
                    ЕлектроннаПошта,
                    Країна,
                    Область,
                    Район,
                    Місто,
                ];
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    Тип = запис.Тип,
                    Значення = запис.Значення,
                    Телефон = запис.Телефон,
                    ЕлектроннаПошта = запис.ЕлектроннаПошта,
                    Країна = запис.Країна,
                    Область = запис.Область,
                    Район = запис.Район,
                    Місто = запис.Місто,
                };
            }

        }

        #endregion

        public Контрагенти_ТабличнаЧастина_Контакти()
        {
            TreeViewGrid.Model = Store;
            AddColumn();
        }

        void AddColumn()
        {
            //Тип
            {
                ListStore store = new ListStore(typeof(string), typeof(string));

                foreach (var field in ПсевдонімиПерелічення.ТипиКонтактноїІнформації_List())
                    store.AppendValues(field.Value.ToString(), field.Name);

                CellRendererCombo cellCombo = new CellRendererCombo() { Editable = true, Model = store, TextColumn = 1 };
                cellCombo.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("Тип", cellCombo, "text", (int)Columns.Тип) { Resizable = true, MinWidth = 100 };

                column.Data.Add("Column", Columns.Тип);
                TreeViewGrid.AppendColumn(column);
            }

            //Значення
            {
                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("Значення", cellText, "text", (int)Columns.Значення) { Resizable = true, MinWidth = 100 };

                column.Data.Add("Column", Columns.Значення);
                TreeViewGrid.AppendColumn(column);
            }

            //Телефон
            {
                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("Телефон", cellText, "text", (int)Columns.Телефон) { Resizable = true, MinWidth = 100 };

                column.Data.Add("Column", Columns.Телефон);
                TreeViewGrid.AppendColumn(column);
            }

            //ЕлектроннаПошта
            {
                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("Email", cellText, "text", (int)Columns.ЕлектроннаПошта) { Resizable = true, MinWidth = 100 };

                column.Data.Add("Column", Columns.ЕлектроннаПошта);
                TreeViewGrid.AppendColumn(column);
            }

            //Країна
            {
                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("Країна", cellText, "text", (int)Columns.Країна) { Resizable = true, MinWidth = 100 };

                column.Data.Add("Column", Columns.Країна);
                TreeViewGrid.AppendColumn(column);
            }

            //Область
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("Область", cellText, "text", (int)Columns.Область) { Resizable = true, MinWidth = 100 };

                column.Data.Add("Column", Columns.Область);
                TreeViewGrid.AppendColumn(column);
            }

            //Район
            {
                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("Район", cellText, "text", (int)Columns.Район) { Resizable = true, MinWidth = 100 };

                column.Data.Add("Column", Columns.Район);
                TreeViewGrid.AppendColumn(column);
            }

            //Місто
            {
                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("Місто", cellText, "text", (int)Columns.Місто) { Resizable = true, MinWidth = 100 };

                column.Data.Add("Column", Columns.Місто);
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
                ЕлементВласник.Контакти_TablePart.FillJoin([]);
                await ЕлементВласник.Контакти_TablePart.Read();

                Записи.Clear();
                Store.Clear();

                foreach (Контрагенти_Контакти_TablePart.Record record in ЕлементВласник.Контакти_TablePart.Records)
                {
                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        Тип = record.Тип,
                        Значення = record.Значення,
                        Телефон = record.Телефон,
                        ЕлектроннаПошта = record.ЕлектроннаПошта,
                        Країна = record.Країна,
                        Область = record.Область,
                        Район = record.Район,
                        Місто = record.Місто,

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
                ЕлементВласник.Контакти_TablePart.Records.Clear();

                foreach (Запис запис in Записи)
                {
                    ЕлементВласник.Контакти_TablePart.Records.Add(new Контрагенти_Контакти_TablePart.Record()
                    {
                        UID = запис.ID,
                        Тип = запис.Тип,
                        Значення = запис.Значення,
                        Телефон = запис.Телефон,
                        ЕлектроннаПошта = запис.ЕлектроннаПошта,
                        Країна = запис.Країна,
                        Область = запис.Область,
                        Район = запис.Район,
                        Місто = запис.Місто,

                    });
                }

                await ЕлементВласник.Контакти_TablePart.Save(true);
                await LoadRecords();
            }
        }

        public string КлючовіСловаДляПошуку()
        {
            string ключовіСлова = "";
            foreach (Запис запис in Записи)
                ключовіСлова += $"\n {запис.Значення} {запис.Телефон} {запис.ЕлектроннаПошта} {запис.Країна} {запис.Область} {запис.Район} {запис.Місто}";
            return ключовіСлова;
        }

        #endregion

        #region Func


        protected override void ChangeCell(TreeIter iter, int rowNumber, int colNumber, string newText)
        {
            Запис запис = Записи[rowNumber];
            switch ((Columns)colNumber)
            {
                case Columns.Тип: { запис.Тип = ПсевдонімиПерелічення.ТипиКонтактноїІнформації_FindByName(newText) ?? 0; break; }
                case Columns.Значення: { запис.Значення = newText; break; }
                case Columns.Телефон: { запис.Телефон = newText; break; }
                case Columns.ЕлектроннаПошта: { запис.ЕлектроннаПошта = newText; break; }
                case Columns.Країна: { запис.Країна = newText; break; }
                case Columns.Область: { запис.Область = newText; break; }
                case Columns.Район: { запис.Район = newText; break; }
                case Columns.Місто: { запис.Місто = newText; break; }

                default: break;
            }
            Store.SetValues(iter, запис.ToArray());
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
