
/*
        ЗбереженіЗвіти_ТабличнаЧастина_ЗвітСторінка.cs
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
    class ЗбереженіЗвіти_ТабличнаЧастина_ЗвітСторінка : ДовідникТабличнаЧастина
    {
        public ЗбереженіЗвіти_Objest? ЕлементВласник { get; set; }

        #region Записи

        enum Columns
        {
            А,
            Б,
            В,
            Г,
            Ґ,
            Д,
            Е,
            Є,
            Ж,
            З,
            И,
            І,
            Ї,
            Й,
            К,
            Л,
            М,
            Н,
            О,
            П,
            Р,
            С,
            Т,
            У,
            Ф,
            Х,
            Ц,
            Ч,
            Ш,
            Щ,
            Ь,
            Ю,
            Я,

        }

        ListStore Store = new ListStore([

            typeof(string), //А
            typeof(string), //Б
            typeof(string), //В
            typeof(string), //Г
            typeof(string), //Ґ
            typeof(string), //Д
            typeof(string), //Е
            typeof(string), //Є
            typeof(string), //Ж
            typeof(string), //З
            typeof(string), //И
            typeof(string), //І
            typeof(string), //Ї
            typeof(string), //Й
            typeof(string), //К
            typeof(string), //Л
            typeof(string), //М
            typeof(string), //Н
            typeof(string), //О
            typeof(string), //П
            typeof(string), //Р
            typeof(string), //С
            typeof(string), //Т
            typeof(string), //У
            typeof(string), //Ф
            typeof(string), //Х
            typeof(string), //Ц
            typeof(string), //Ч
            typeof(string), //Ш
            typeof(string), //Щ
            typeof(string), //Ь
            typeof(string), //Ю
            typeof(string), //Я
        ]);

        List<Запис> Записи = [];

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            public string А { get; set; } = "";
            public string Б { get; set; } = "";
            public string В { get; set; } = "";
            public string Г { get; set; } = "";
            public string Ґ { get; set; } = "";
            public string Д { get; set; } = "";
            public string Е { get; set; } = "";
            public string Є { get; set; } = "";
            public string Ж { get; set; } = "";
            public string З { get; set; } = "";
            public string И { get; set; } = "";
            public string І { get; set; } = "";
            public string Ї { get; set; } = "";
            public string Й { get; set; } = "";
            public string К { get; set; } = "";
            public string Л { get; set; } = "";
            public string М { get; set; } = "";
            public string Н { get; set; } = "";
            public string О { get; set; } = "";
            public string П { get; set; } = "";
            public string Р { get; set; } = "";
            public string С { get; set; } = "";
            public string Т { get; set; } = "";
            public string У { get; set; } = "";
            public string Ф { get; set; } = "";
            public string Х { get; set; } = "";
            public string Ц { get; set; } = "";
            public string Ч { get; set; } = "";
            public string Ш { get; set; } = "";
            public string Щ { get; set; } = "";
            public string Ь { get; set; } = "";
            public string Ю { get; set; } = "";
            public string Я { get; set; } = "";


            public object[] ToArray()
            {
                return
                [
                    А,
                    Б,
                    В,
                    Г,
                    Ґ,
                    Д,
                    Е,
                    Є,
                    Ж,
                    З,
                    И,
                    І,
                    Ї,
                    Й,
                    К,
                    Л,
                    М,
                    Н,
                    О,
                    П,
                    Р,
                    С,
                    Т,
                    У,
                    Ф,
                    Х,
                    Ц,
                    Ч,
                    Ш,
                    Щ,
                    Ь,
                    Ю,
                    Я,

                ];
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    А = запис.А,
                    Б = запис.Б,
                    В = запис.В,
                    Г = запис.Г,
                    Ґ = запис.Ґ,
                    Д = запис.Д,
                    Е = запис.Е,
                    Є = запис.Є,
                    Ж = запис.Ж,
                    З = запис.З,
                    И = запис.И,
                    І = запис.І,
                    Ї = запис.Ї,
                    Й = запис.Й,
                    К = запис.К,
                    Л = запис.Л,
                    М = запис.М,
                    Н = запис.Н,
                    О = запис.О,
                    П = запис.П,
                    Р = запис.Р,
                    С = запис.С,
                    Т = запис.Т,
                    У = запис.У,
                    Ф = запис.Ф,
                    Х = запис.Х,
                    Ц = запис.Ц,
                    Ч = запис.Ч,
                    Ш = запис.Ш,
                    Щ = запис.Щ,
                    Ь = запис.Ь,
                    Ю = запис.Ю,
                    Я = запис.Я,

                };
            }

        }

        #endregion

        public ЗбереженіЗвіти_ТабличнаЧастина_ЗвітСторінка()
        {
            TreeViewGrid.Model = Store;
            TreeViewGrid.StyleContext.AddClass("report");
            AddColumn();
        }

        void AddColumn()
        {

            //А
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("А", cellText, "text", (int)Columns.А) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.А);
                TreeViewGrid.AppendColumn(column);
            }

            //Б
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("Б", cellText, "text", (int)Columns.Б) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.Б);
                TreeViewGrid.AppendColumn(column);
            }

            //В
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("В", cellText, "text", (int)Columns.В) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.В);
                TreeViewGrid.AppendColumn(column);
            }

            //Г
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("Г", cellText, "text", (int)Columns.Г) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.Г);
                TreeViewGrid.AppendColumn(column);
            }

            //Ґ
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("Ґ", cellText, "text", (int)Columns.Ґ) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.Ґ);
                TreeViewGrid.AppendColumn(column);
            }

            //Д
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("Д", cellText, "text", (int)Columns.Д) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.Д);
                TreeViewGrid.AppendColumn(column);
            }

            //Е
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("Е", cellText, "text", (int)Columns.Е) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.Е);
                TreeViewGrid.AppendColumn(column);
            }

            //Є
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("Є", cellText, "text", (int)Columns.Є) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.Є);
                TreeViewGrid.AppendColumn(column);
            }

            //Ж
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("Ж", cellText, "text", (int)Columns.Ж) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.Ж);
                TreeViewGrid.AppendColumn(column);
            }

            //З
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("З", cellText, "text", (int)Columns.З) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.З);
                TreeViewGrid.AppendColumn(column);
            }

            //И
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("И", cellText, "text", (int)Columns.И) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.И);
                TreeViewGrid.AppendColumn(column);
            }

            //І
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("І", cellText, "text", (int)Columns.І) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.І);
                TreeViewGrid.AppendColumn(column);
            }

            //Ї
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("Ї", cellText, "text", (int)Columns.Ї) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.Ї);
                TreeViewGrid.AppendColumn(column);
            }

            //Й
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("Й", cellText, "text", (int)Columns.Й) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.Й);
                TreeViewGrid.AppendColumn(column);
            }

            //К
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("К", cellText, "text", (int)Columns.К) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.К);
                TreeViewGrid.AppendColumn(column);
            }

            //Л
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("Л", cellText, "text", (int)Columns.Л) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.Л);
                TreeViewGrid.AppendColumn(column);
            }

            //М
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("М", cellText, "text", (int)Columns.М) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.М);
                TreeViewGrid.AppendColumn(column);
            }

            //Н
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("Н", cellText, "text", (int)Columns.Н) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.Н);
                TreeViewGrid.AppendColumn(column);
            }

            //О
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("О", cellText, "text", (int)Columns.О) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.О);
                TreeViewGrid.AppendColumn(column);
            }

            //П
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("П", cellText, "text", (int)Columns.П) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.П);
                TreeViewGrid.AppendColumn(column);
            }

            //Р
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("Р", cellText, "text", (int)Columns.Р) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.Р);
                TreeViewGrid.AppendColumn(column);
            }

            //С
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("С", cellText, "text", (int)Columns.С) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.С);
                TreeViewGrid.AppendColumn(column);
            }

            //Т
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("Т", cellText, "text", (int)Columns.Т) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.Т);
                TreeViewGrid.AppendColumn(column);
            }

            //У
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("У", cellText, "text", (int)Columns.У) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.У);
                TreeViewGrid.AppendColumn(column);
            }

            //Ф
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("Ф", cellText, "text", (int)Columns.Ф) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.Ф);
                TreeViewGrid.AppendColumn(column);
            }

            //Х
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("Х", cellText, "text", (int)Columns.Х) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.Х);
                TreeViewGrid.AppendColumn(column);
            }

            //Ц
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("Ц", cellText, "text", (int)Columns.Ц) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.Ц);
                TreeViewGrid.AppendColumn(column);
            }

            //Ч
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("Ч", cellText, "text", (int)Columns.Ч) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.Ч);
                TreeViewGrid.AppendColumn(column);
            }

            //Ш
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("Ш", cellText, "text", (int)Columns.Ш) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.Ш);
                TreeViewGrid.AppendColumn(column);
            }

            //Щ
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("Щ", cellText, "text", (int)Columns.Щ) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.Щ);
                TreeViewGrid.AppendColumn(column);
            }

            //Ь
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("Ь", cellText, "text", (int)Columns.Ь) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.Ь);
                TreeViewGrid.AppendColumn(column);
            }

            //Ю
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("Ю", cellText, "text", (int)Columns.Ю) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.Ю);
                TreeViewGrid.AppendColumn(column);
            }

            //Я
            {

                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("Я", cellText, "text", (int)Columns.Я) { Resizable = true, MinWidth = 50 };

                SetColIndex(column, Columns.Я);
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
                ЕлементВласник.ЗвітСторінка_TablePart.FillJoin([]);
                await ЕлементВласник.ЗвітСторінка_TablePart.Read();

                Записи.Clear();
                Store.Clear();

                foreach (ЗбереженіЗвіти_ЗвітСторінка_TablePart.Record record in ЕлементВласник.ЗвітСторінка_TablePart.Records)
                {
                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        А = record.А,
                        Б = record.Б,
                        В = record.В,
                        Г = record.Г,
                        Ґ = record.Ґ,
                        Д = record.Д,
                        Е = record.Е,
                        Є = record.Є,
                        Ж = record.Ж,
                        З = record.З,
                        И = record.И,
                        І = record.І,
                        Ї = record.Ї,
                        Й = record.Й,
                        К = record.К,
                        Л = record.Л,
                        М = record.М,
                        Н = record.Н,
                        О = record.О,
                        П = record.П,
                        Р = record.Р,
                        С = record.С,
                        Т = record.Т,
                        У = record.У,
                        Ф = record.Ф,
                        Х = record.Х,
                        Ц = record.Ц,
                        Ч = record.Ч,
                        Ш = record.Ш,
                        Щ = record.Щ,
                        Ь = record.Ь,
                        Ю = record.Ю,
                        Я = record.Я,

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
                ЕлементВласник.ЗвітСторінка_TablePart.Records.Clear();
                foreach (Запис запис in Записи)
                {
                    ЕлементВласник.ЗвітСторінка_TablePart.Records.Add(new ЗбереженіЗвіти_ЗвітСторінка_TablePart.Record()
                    {
                        UID = запис.ID,
                        А = запис.А,
                        Б = запис.Б,
                        В = запис.В,
                        Г = запис.Г,
                        Ґ = запис.Ґ,
                        Д = запис.Д,
                        Е = запис.Е,
                        Є = запис.Є,
                        Ж = запис.Ж,
                        З = запис.З,
                        И = запис.И,
                        І = запис.І,
                        Ї = запис.Ї,
                        Й = запис.Й,
                        К = запис.К,
                        Л = запис.Л,
                        М = запис.М,
                        Н = запис.Н,
                        О = запис.О,
                        П = запис.П,
                        Р = запис.Р,
                        С = запис.С,
                        Т = запис.Т,
                        У = запис.У,
                        Ф = запис.Ф,
                        Х = запис.Х,
                        Ц = запис.Ц,
                        Ч = запис.Ч,
                        Ш = запис.Ш,
                        Щ = запис.Щ,
                        Ь = запис.Ь,
                        Ю = запис.Ю,
                        Я = запис.Я,

                    });
                }
                await ЕлементВласник.ЗвітСторінка_TablePart.Save(true);
                await LoadRecords();
            }
        }

        /*public string КлючовіСловаДляПошуку()
        {
            
            string keyWords = "";
            foreach (Запис запис in Записи)
                keyWords += $"\n {запис.А} {запис.Б} {запис.В} {запис.Г} {запис.Ґ} {запис.Д} {запис.Е} {запис.Є} {запис.Ж} {запис.З} {запис.И} {запис.І} {запис.Ї} {запис.Й} {запис.К} {запис.Л} {запис.М} {запис.Н} {запис.О} {запис.П} {запис.Р} {запис.С} {запис.Т} {запис.У} {запис.Ф} {запис.Х} {запис.Ц} {запис.Ч} {запис.Ш} {запис.Щ} {запис.Ь} {запис.Ю} {запис.Я}";
            
            return keyWords;
        }*/

        #endregion

        #region Func


        protected override void ChangeCell(TreeIter iter, int rowNumber, int colNumber, string newText)
        {
            Запис запис = Записи[rowNumber];
            switch ((Columns)colNumber)
            {
                case Columns.А: { запис.А = newText; break; }
                case Columns.Б: { запис.Б = newText; break; }
                case Columns.В: { запис.В = newText; break; }
                case Columns.Г: { запис.Г = newText; break; }
                case Columns.Ґ: { запис.Ґ = newText; break; }
                case Columns.Д: { запис.Д = newText; break; }
                case Columns.Е: { запис.Е = newText; break; }
                case Columns.Є: { запис.Є = newText; break; }
                case Columns.Ж: { запис.Ж = newText; break; }
                case Columns.З: { запис.З = newText; break; }
                case Columns.И: { запис.И = newText; break; }
                case Columns.І: { запис.І = newText; break; }
                case Columns.Ї: { запис.Ї = newText; break; }
                case Columns.Й: { запис.Й = newText; break; }
                case Columns.К: { запис.К = newText; break; }
                case Columns.Л: { запис.Л = newText; break; }
                case Columns.М: { запис.М = newText; break; }
                case Columns.Н: { запис.Н = newText; break; }
                case Columns.О: { запис.О = newText; break; }
                case Columns.П: { запис.П = newText; break; }
                case Columns.Р: { запис.Р = newText; break; }
                case Columns.С: { запис.С = newText; break; }
                case Columns.Т: { запис.Т = newText; break; }
                case Columns.У: { запис.У = newText; break; }
                case Columns.Ф: { запис.Ф = newText; break; }
                case Columns.Х: { запис.Х = newText; break; }
                case Columns.Ц: { запис.Ц = newText; break; }
                case Columns.Ч: { запис.Ч = newText; break; }
                case Columns.Ш: { запис.Ш = newText; break; }
                case Columns.Щ: { запис.Щ = newText; break; }
                case Columns.Ь: { запис.Ь = newText; break; }
                case Columns.Ю: { запис.Ю = newText; break; }
                case Columns.Я: { запис.Я = newText; break; }

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
