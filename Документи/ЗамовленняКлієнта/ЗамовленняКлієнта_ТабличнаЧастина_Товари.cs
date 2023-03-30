#region Info

/*
Copyright (C) 2019-2023 TARAKHOMYN YURIY IVANOVYCH
All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

/*
Автор:    Тарахомин Юрій Іванович
Адреса:   Україна, м. Львів
Сайт:     accounting.org.ua
*/

#endregion

using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0;
using Константи = StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.РегістриВідомостей;

namespace StorageAndTrade
{
    class ЗамовленняКлієнта_ТабличнаЧастина_Товари : VBox
    {
        ScrolledWindow scrollTree;
        public ЗамовленняКлієнта_Objest? ЗамовленняКлієнта_Objest { get; set; }

        #region Записи

        enum Columns
        {
            НомерРядка,
            Номенклатура,
            Характеристика,
            КількістьУпаковок,
            Пакування,
            Кількість,
            ВидЦіни,
            Ціна,
            Сума,
            Скидка,
            СкладНазва
        }

        ListStore Store = new ListStore(
            typeof(int),      //НомерРядка
            typeof(string),   //Номенклатура
            typeof(string),   //Характеристика
            typeof(int),      //КількістьУпаковок
            typeof(string),   //Пакування
            typeof(float),    //Кількість
            typeof(string),   //ВидЦіни
            typeof(float),    //Ціна
            typeof(float),    //Сума
            typeof(float),    //Скидка
            typeof(string)    //СкладНазва
        );

        List<Запис> Записи = new List<Запис>();

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            public int НомерРядка { get; set; }
            public Номенклатура_Pointer Номенклатура { get; set; } = new Номенклатура_Pointer();
            public ХарактеристикиНоменклатури_Pointer Характеристика { get; set; } = new ХарактеристикиНоменклатури_Pointer();
            public int КількістьУпаковок { get; set; } = 1;
            public ПакуванняОдиниціВиміру_Pointer Пакування { get; set; } = new ПакуванняОдиниціВиміру_Pointer();
            public decimal Кількість { get; set; } = 1;
            public ВидиЦін_Pointer ВидЦіни { get; set; } = new ВидиЦін_Pointer();
            public decimal Ціна { get; set; }
            public decimal Сума { get; set; }
            public decimal Скидка { get; set; }
            public Склади_Pointer Склад { get; set; } = new Склади_Pointer();

            public object[] ToArray()
            {
                return new object[]
                {
                    НомерРядка,
                    Номенклатура.Назва,
                    Характеристика.Назва,
                    КількістьУпаковок,
                    Пакування.Назва,
                    (float)Кількість,
                    ВидЦіни.Назва,
                    (float)Ціна,
                    (float)Сума,
                    (float)Скидка,
                    Склад.Назва
                };
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    Номенклатура = запис.Номенклатура,
                    Характеристика = запис.Характеристика,
                    КількістьУпаковок = запис.КількістьУпаковок,
                    Пакування = запис.Пакування,
                    Кількість = запис.Кількість,
                    ВидЦіни = запис.ВидЦіни,
                    Ціна = запис.Ціна,
                    Сума = запис.Сума,
                    Скидка = запис.Скидка,
                    Склад = запис.Склад
                };
            }

            public static void ПісляДодаванняНового(Запис запис)
            {
                запис.ВидЦіни = Константи.ЗначенняЗаЗамовчуванням.ОсновнийВидЦіни_Const;
                ПісляЗміни_ВидЦіни(запис);
            }
            public static void ПісляЗміни_Номенклатура(Запис запис)
            {
                запис.Номенклатура.GetPresentation();

                Номенклатура_Objest? номенклатура_Objest = запис.Номенклатура.GetDirectoryObject();
                if (номенклатура_Objest != null && !номенклатура_Objest.ОдиницяВиміру.IsEmpty())
                {
                    запис.Пакування = номенклатура_Objest.ОдиницяВиміру;
                    Запис.ПісляЗміни_Пакування(запис);
                }

                if (!запис.Пакування.IsEmpty())
                {
                    ПакуванняОдиниціВиміру_Objest? пакуванняОдиниціВиміру_Objest = запис.Пакування.GetDirectoryObject();
                    if (пакуванняОдиниціВиміру_Objest != null)
                        запис.КількістьУпаковок = пакуванняОдиниціВиміру_Objest.КількістьУпаковок;
                    else
                        запис.КількістьУпаковок = 1;
                }
            }
            public static void ПісляЗміни_Характеристика(Запис запис)
            {
                запис.Характеристика.GetPresentation();
            }
            public static void ПісляЗміни_Пакування(Запис запис)
            {
                запис.Пакування.GetPresentation();
            }
            public static void ПісляЗміни_ВидЦіни(Запис запис)
            {
                запис.ВидЦіни.GetPresentation();
            }
            public static void ПісляЗміни_Склад(Запис запис)
            {
                запис.Склад.GetPresentation();
            }
            public static void ПісляЗміни_КількістьАбоЦіна(Запис запис)
            {
                запис.Сума = запис.Кількість * запис.Ціна;
            }
            public static void ОтриматиЦіну(Запис запис)
            {
                if (запис.Номенклатура.IsEmpty())
                    return;

                if (запис.ВидЦіни.IsEmpty())
                    return;

                string query = $@"
SELECT
    ЦіниНоменклатури.{ЦіниНоменклатури_Const.Ціна} AS Ціна
FROM 
    {ЦіниНоменклатури_Const.TABLE} AS ЦіниНоменклатури
WHERE
    ЦіниНоменклатури.{ЦіниНоменклатури_Const.ВидЦіни} = '{запис.ВидЦіни.UnigueID}' AND
    ЦіниНоменклатури.{ЦіниНоменклатури_Const.Номенклатура} = '{запис.Номенклатура.UnigueID}'
";

                #region WHERE

                if (!запис.Характеристика.IsEmpty())
                {
                    query += $@"
AND ЦіниНоменклатури.{ЦіниНоменклатури_Const.ХарактеристикаНоменклатури} = '{запис.Характеристика.UnigueID}'
";
                }

                #endregion

                query += $@"
ORDER BY 
    ЦіниНоменклатури.period DESC 
LIMIT 1
";
                Dictionary<string, object> paramQuery = new Dictionary<string, object>();

                string[] columnsName;
                List<Dictionary<string, object>> listRow;

                Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

                if (listRow.Count > 0)
                    foreach (Dictionary<string, object> row in listRow)
                    {
                        запис.Ціна = (decimal)row["Ціна"];
                        запис.Сума = запис.Кількість * запис.Ціна;
                    }
            }
        }

        #endregion

        TreeView TreeViewGrid;

        public ЗамовленняКлієнта_ТабличнаЧастина_Товари() : base()
        {
            CreateToolbar();

            scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeViewGrid = new TreeView(Store);
            AddColumn();

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;
            TreeViewGrid.ButtonPressEvent += OnButtonPressEvent;

            scrollTree.Add(TreeViewGrid);

            Add(scrollTree);

            ShowAll();
        }

        void OnButtonPressEvent(object sender, ButtonPressEventArgs args)
        {
            if (args.Event.Type == Gdk.EventType.DoubleButtonPress && TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath itemPath;
                TreeViewColumn treeColumn;

                TreeViewGrid.GetCursor(out itemPath, out treeColumn);

                if (treeColumn.Data.ContainsKey("Column"))
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    Gdk.Rectangle rectangleCell = TreeViewGrid.GetCellArea(itemPath, treeColumn);
                    rectangleCell.Offset(-(int)scrollTree.Hadjustment.Value, rectangleCell.Height);

                    Popover PopoverSmallSelect = new Popover(TreeViewGrid) { Position = PositionType.Bottom, BorderWidth = 2 };
                    PopoverSmallSelect.PointingTo = rectangleCell;

                    int rowNumber = int.Parse(itemPath.ToString());
                    Запис запис = Записи[rowNumber];

                    switch ((Columns)treeColumn.Data["Column"]!)
                    {
                        case Columns.Номенклатура:
                            {
                                Номенклатура_ШвидкийВибір page = new Номенклатура_ШвидкийВибір() { PopoverParent = PopoverSmallSelect, DirectoryPointerItem = запис.Номенклатура };
                                page.CallBack_OnSelectPointer = (Номенклатура_Pointer selectPointer) =>
                                {
                                    запис.Номенклатура = selectPointer;
                                    Запис.ПісляЗміни_Номенклатура(запис);
                                    Запис.ОтриматиЦіну(запис);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                PopoverSmallSelect.Add(page);
                                PopoverSmallSelect.ShowAll();

                                page.LoadRecords();
                                break;
                            }
                        case Columns.Характеристика:
                            {
                                ХарактеристикиНоменклатури_ШвидкийВибір page = new ХарактеристикиНоменклатури_ШвидкийВибір() { PopoverParent = PopoverSmallSelect, DirectoryPointerItem = запис.Характеристика };

                                page.НоменклатураВласник.Pointer = запис.Номенклатура;
                                page.CallBack_OnSelectPointer = (ХарактеристикиНоменклатури_Pointer selectPointer) =>
                                {
                                    запис.Характеристика = selectPointer;
                                    Запис.ПісляЗміни_Характеристика(запис);
                                    Запис.ОтриматиЦіну(запис);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                PopoverSmallSelect.Add(page);
                                PopoverSmallSelect.ShowAll();

                                page.LoadRecords();

                                break;
                            }
                        case Columns.Пакування:
                            {
                                ПакуванняОдиниціВиміру_ШвидкийВибір page = new ПакуванняОдиниціВиміру_ШвидкийВибір() { PopoverParent = PopoverSmallSelect, DirectoryPointerItem = запис.Пакування };
                                page.CallBack_OnSelectPointer = (ПакуванняОдиниціВиміру_Pointer selectPointer) =>
                                {
                                    запис.Пакування = selectPointer;
                                    Запис.ПісляЗміни_Пакування(запис);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                PopoverSmallSelect.Add(page);
                                PopoverSmallSelect.ShowAll();

                                page.LoadRecords();
                                break;
                            }
                        case Columns.ВидЦіни:
                            {
                                ВидиЦін_ШвидкийВибір page = new ВидиЦін_ШвидкийВибір() { PopoverParent = PopoverSmallSelect, DirectoryPointerItem = запис.ВидЦіни };
                                page.CallBack_OnSelectPointer = (ВидиЦін_Pointer selectPointer) =>
                                {
                                    запис.ВидЦіни = selectPointer;
                                    Запис.ПісляЗміни_ВидЦіни(запис);
                                    Запис.ОтриматиЦіну(запис);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                PopoverSmallSelect.Add(page);
                                PopoverSmallSelect.ShowAll();

                                page.LoadRecords();
                                break;
                            }
                        case Columns.СкладНазва:
                            {
                                Склади_ШвидкийВибір page = new Склади_ШвидкийВибір() { PopoverParent = PopoverSmallSelect, DirectoryPointerItem = запис.Склад };
                                page.CallBack_OnSelectPointer = (Склади_Pointer selectPointer) =>
                                {
                                    запис.Склад = selectPointer;
                                    Запис.ПісляЗміни_Склад(запис);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                PopoverSmallSelect.Add(page);
                                PopoverSmallSelect.ShowAll();

                                page.LoadRecords();
                                break;
                            }
                    }
                }
            }
        }

        void CreateToolbar()
        {
            Toolbar toolbar = new Toolbar();
            PackStart(toolbar, false, false, 0);

            ToolButton upButton = new ToolButton(Stock.Add) { TooltipText = "Додати" };
            upButton.Clicked += OnAddClick;
            toolbar.Add(upButton);

            ToolButton copyButton = new ToolButton(Stock.Copy) { TooltipText = "Копіювати" };
            copyButton.Clicked += OnCopyClick;
            toolbar.Add(copyButton);

            ToolButton deleteButton = new ToolButton(Stock.Delete) { TooltipText = "Видалити" };
            deleteButton.Clicked += OnDeleteClick;
            toolbar.Add(deleteButton);
        }

        public void LoadRecords()
        {
            Store.Clear();
            Записи.Clear();

            if (ЗамовленняКлієнта_Objest != null)
            {
                Query querySelect = ЗамовленняКлієнта_Objest.Товари_TablePart.QuerySelect;
                querySelect.Clear();

                //JOIN Номенклатура
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(Номенклатура_Const.TABLE + "." + Номенклатура_Const.Назва, "Номенклатура"));
                querySelect.Joins.Add(
                    new Join(Номенклатура_Const.TABLE, ЗамовленняКлієнта_Товари_TablePart.Номенклатура, querySelect.Table));

                //JOIN Характеристика
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ХарактеристикиНоменклатури_Const.TABLE + "." + ХарактеристикиНоменклатури_Const.Назва, "Характеристика"));
                querySelect.Joins.Add(
                    new Join(ХарактеристикиНоменклатури_Const.TABLE, ЗамовленняКлієнта_Товари_TablePart.ХарактеристикаНоменклатури, querySelect.Table));

                //JOIN Пакування
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ПакуванняОдиниціВиміру_Const.TABLE + "." + ПакуванняОдиниціВиміру_Const.Назва, "Пакування"));
                querySelect.Joins.Add(
                    new Join(ПакуванняОдиниціВиміру_Const.TABLE, ЗамовленняКлієнта_Товари_TablePart.Пакування, querySelect.Table));

                //JOIN ВидЦін
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ВидиЦін_Const.TABLE + "." + ВидиЦін_Const.Назва, "ВидЦін"));
                querySelect.Joins.Add(
                    new Join(ВидиЦін_Const.TABLE, ЗамовленняКлієнта_Товари_TablePart.ВидЦіни, querySelect.Table));

                //JOIN Склад
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(Склади_Const.TABLE + "." + Склади_Const.Назва, "Склад"));
                querySelect.Joins.Add(
                    new Join(Склади_Const.TABLE, ЗамовленняКлієнта_Товари_TablePart.Склад, querySelect.Table));

                //ORDER
                querySelect.Order.Add(ЗамовленняКлієнта_Товари_TablePart.НомерРядка, SelectOrder.ASC);

                ЗамовленняКлієнта_Objest.Товари_TablePart.Read();

                Dictionary<string, Dictionary<string, string>> JoinValue = ЗамовленняКлієнта_Objest.Товари_TablePart.JoinValue;

                foreach (ЗамовленняКлієнта_Товари_TablePart.Record record in ЗамовленняКлієнта_Objest.Товари_TablePart.Records)
                {
                    string uid = record.UID.ToString();

                    record.Номенклатура.Назва = JoinValue[uid]["Номенклатура"];
                    record.ХарактеристикаНоменклатури.Назва = JoinValue[uid]["Характеристика"];
                    record.Пакування.Назва = JoinValue[uid]["Пакування"];
                    record.ВидЦіни.Назва = JoinValue[uid]["ВидЦін"];
                    record.Склад.Назва = JoinValue[uid]["Склад"];

                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        НомерРядка = record.НомерРядка,
                        Номенклатура = record.Номенклатура,
                        Характеристика = record.ХарактеристикаНоменклатури,
                        КількістьУпаковок = record.КількістьУпаковок,
                        Пакування = record.Пакування,
                        Кількість = record.Кількість,
                        ВидЦіни = record.ВидЦіни,
                        Ціна = record.Ціна,
                        Сума = record.Сума,
                        Скидка = record.Скидка,
                        Склад = record.Склад
                    };

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }
        }

        public void SaveRecords()
        {
            if (ЗамовленняКлієнта_Objest != null)
            {
                ЗамовленняКлієнта_Objest.Товари_TablePart.Records.Clear();

                int sequenceNumber = 0;

                foreach (Запис запис in Записи)
                {
                    ЗамовленняКлієнта_Товари_TablePart.Record record = new ЗамовленняКлієнта_Товари_TablePart.Record();

                    record.UID = запис.ID;
                    record.НомерРядка = ++sequenceNumber;
                    record.Номенклатура = запис.Номенклатура;
                    record.ХарактеристикаНоменклатури = запис.Характеристика;
                    record.КількістьУпаковок = запис.КількістьУпаковок;
                    record.Пакування = запис.Пакування;
                    record.Кількість = запис.Кількість;
                    record.ВидЦіни = запис.ВидЦіни;
                    record.Ціна = запис.Ціна;
                    record.Сума = запис.Сума;
                    record.Скидка = запис.Скидка;
                    record.Склад = запис.Склад;

                    ЗамовленняКлієнта_Objest.Товари_TablePart.Records.Add(record);
                }

                ЗамовленняКлієнта_Objest.Товари_TablePart.Save(true);
            }
        }

        public decimal СумаДокументу()
        {
            decimal Сума = 0;

            foreach (Запис запис in Записи)
                Сума += запис.Сума;

            return Math.Round(Сума, 2);
        }

        public string КлючовіСловаДляПошуку()
        {
            string ключовіСлова = "";

            if (ЗамовленняКлієнта_Objest != null)
            {
                int sequenceNumber = 0;
                foreach (Запис запис in Записи)
                    ключовіСлова += $"\n{++sequenceNumber}. {запис.Номенклатура.Назва} {запис.Характеристика.Назва} ";
            }

            return ключовіСлова;
        }

        #region TreeView

        void AddColumn()
        {
            //НомерРядка
            TreeViewGrid.AppendColumn(new TreeViewColumn("№", new CellRendererText(), "text", (int)Columns.НомерРядка) { Resizable = true, MinWidth =  30 });

            //Номенклатура
            {
                TreeViewColumn Номенклатура = new TreeViewColumn("Номенклатура", new CellRendererText(), "text", (int)Columns.Номенклатура) { Resizable = true, MinWidth =  200 };
                Номенклатура.Data.Add("Column", Columns.Номенклатура);

                TreeViewGrid.AppendColumn(Номенклатура);
            }

            //Характеристика
            {
                TreeViewColumn Характеристика = new TreeViewColumn("Характеристика", new CellRendererText(), "text", (int)Columns.Характеристика) { Resizable = true, MinWidth =  200 };
                Характеристика.Visible = Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const;
                Характеристика.Data.Add("Column", Columns.Характеристика);

                TreeViewGrid.AppendColumn(Характеристика);
            }

            //КількістьУпаковок
            {
                CellRendererText КількістьУпаковок = new CellRendererText() { Editable = true };
                КількістьУпаковок.Edited += TextChanged;
                КількістьУпаковок.Data.Add("Column", (int)Columns.КількістьУпаковок);

                TreeViewColumn Column = new TreeViewColumn("Пак", КількістьУпаковок, "text", (int)Columns.КількістьУпаковок) { Resizable = true, MinWidth =  50 };
                Column.SetCellDataFunc(КількістьУпаковок, new TreeCellDataFunc(NumericCellDataFunc));
                TreeViewGrid.AppendColumn(Column);
            }

            //Пакування
            {
                TreeViewColumn Пакування = new TreeViewColumn("Пакування", new CellRendererText(), "text", (int)Columns.Пакування) { Resizable = true, MinWidth =  100 };
                Пакування.Data.Add("Column", Columns.Пакування);

                TreeViewGrid.AppendColumn(Пакування);
            }

            //Кількість
            {
                CellRendererText Кількість = new CellRendererText() { Editable = true };
                Кількість.Edited += TextChanged;
                Кількість.Data.Add("Column", (int)Columns.Кількість);

                TreeViewColumn Column = new TreeViewColumn("Кількість", Кількість, "text", (int)Columns.Кількість) { Resizable = true, MinWidth =  100 };
                Column.SetCellDataFunc(Кількість, new TreeCellDataFunc(NumericCellDataFunc));
                TreeViewGrid.AppendColumn(Column);
            }

            //ВидЦіни
            {
                TreeViewColumn ВидЦіни = new TreeViewColumn("Вид ціни", new CellRendererText(), "text", (int)Columns.ВидЦіни) { Resizable = true, MinWidth =  100 };
                ВидЦіни.Data.Add("Column", Columns.ВидЦіни);

                TreeViewGrid.AppendColumn(ВидЦіни);
            }

            //Ціна
            {
                CellRendererText Ціна = new CellRendererText() { Editable = true };
                Ціна.Edited += TextChanged;
                Ціна.Data.Add("Column", (int)Columns.Ціна);

                TreeViewColumn Column = new TreeViewColumn("Ціна", Ціна, "text", (int)Columns.Ціна) { Resizable = true, MinWidth =  100 };
                Column.SetCellDataFunc(Ціна, new TreeCellDataFunc(NumericCellDataFunc));
                TreeViewGrid.AppendColumn(Column);
            }

            //Сума
            {
                CellRendererText Сума = new CellRendererText() { Editable = true };
                Сума.Edited += TextChanged;
                Сума.Data.Add("Column", (int)Columns.Сума);

                TreeViewColumn Column = new TreeViewColumn("Сума", Сума, "text", (int)Columns.Сума) { Resizable = true, MinWidth =  100 };
                Column.SetCellDataFunc(Сума, new TreeCellDataFunc(NumericCellDataFunc));
                TreeViewGrid.AppendColumn(Column);
            }

            //Скидка
            {
                CellRendererText Скидка = new CellRendererText() { Editable = true };
                Скидка.Edited += TextChanged;
                Скидка.Data.Add("Column", (int)Columns.Скидка);

                TreeViewColumn Column = new TreeViewColumn("Скидка", Скидка, "text", (int)Columns.Скидка) { Resizable = true, MinWidth =  100 };
                Column.SetCellDataFunc(Скидка, new TreeCellDataFunc(NumericCellDataFunc));
                TreeViewGrid.AppendColumn(Column);
            }

            //СкладНазва
            {
                TreeViewColumn СкладНазва = new TreeViewColumn("Склад", new CellRendererText(), "text", (int)Columns.СкладНазва) { Resizable = true, MinWidth =  200 };
                СкладНазва.Data.Add("Column", Columns.СкладНазва);

                TreeViewGrid.AppendColumn(СкладНазва);
            }

            //Колонка пустишка для заповнення вільного простору
            TreeViewGrid.AppendColumn(new TreeViewColumn());
        }

        void NumericCellDataFunc(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            CellRendererText cellText = (CellRendererText)cell;
            if (cellText.Data.Contains("Column"))
            {
                int rowNumber = int.Parse(Store.GetPath(iter).ToString());
                Запис запис = Записи[rowNumber];

                cellText.Foreground = "green";

                switch ((Columns)cellText.Data["Column"]!)
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
                    case Columns.Ціна:
                        {
                            cellText.Text = запис.Ціна.ToString();
                            break;
                        }
                    case Columns.Сума:
                        {
                            cellText.Text = запис.Сума.ToString();
                            break;
                        }
                    case Columns.Скидка:
                        {
                            cellText.Text = запис.Скидка.ToString();
                            break;
                        }
                }
            }
        }

        void TextChanged(object sender, EditedArgs args)
        {
            CellRenderer cellRender = (CellRenderer)sender;

            if (cellRender.Data.Contains("Column"))
            {
                int ColumnNum = (int)cellRender.Data["Column"]!;

                TreeIter iter;
                Store.GetIterFromString(out iter, args.Path);

                int rowNumber = int.Parse(args.Path);
                Запис запис = Записи[rowNumber];

                switch ((Columns)cellRender.Data["Column"]!)
                {
                    case Columns.КількістьУпаковок:
                        {
                            var (check, value) = Validate.IsInt(args.NewText);
                            if (check)
                                запис.КількістьУпаковок = value;

                            break;
                        }
                    case Columns.Кількість:
                        {
                            var (check, value) = Validate.IsDecimal(args.NewText);
                            if (check)
                            {
                                запис.Кількість = value;
                                Запис.ПісляЗміни_КількістьАбоЦіна(запис);
                            }

                            break;
                        }
                    case Columns.Ціна:
                        {
                            var (check, value) = Validate.IsDecimal(args.NewText);
                            if (check)
                            {
                                запис.Ціна = value;
                                Запис.ПісляЗміни_КількістьАбоЦіна(запис);
                            }

                            break;
                        }
                    case Columns.Сума:
                        {
                            var (check, value) = Validate.IsDecimal(args.NewText);
                            if (check)
                                запис.Сума = value;

                            break;
                        }
                    case Columns.Скидка:
                        {
                            var (check, value) = Validate.IsDecimal(args.NewText);
                            if (check)
                                запис.Скидка = value;

                            break;
                        }
                }

                Store.SetValues(iter, запис.ToArray());
            }
        }

        #endregion

        #region ToolBar

        void OnAddClick(object? sender, EventArgs args)
        {
            Запис запис = new Запис();
            Записи.Add(запис);

            Запис.ПісляДодаванняНового(запис);

            Store.AppendValues(запис.ToArray());
        }

        void OnCopyClick(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    int rowNumber = int.Parse(itemPath.ToString());
                    Запис запис = Записи[rowNumber];

                    Запис записНовий = Запис.Clone(запис);

                    Записи.Add(записНовий);
                    Store.AppendValues(записНовий.ToArray());
                }
            }
        }

        void OnDeleteClick(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();
                for (int i = selectionRows.Length - 1; i >= 0; i--)
                {
                    TreePath itemPath = selectionRows[i];

                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    int rowNumber = int.Parse(itemPath.ToString());
                    Запис запис = Записи[rowNumber];

                    Записи.Remove(запис);
                    Store.Remove(ref iter);
                }
            }
        }

        #endregion

    }
}