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

using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Довідники;
using Перелічення = StorageAndTrade_1_0.Перелічення;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.РегістриВідомостей;

namespace StorageAndTrade
{
    class РозміщенняНоменклатуриПоКоміркам_ТабличнаЧастина_Товари : VBox
    {
        ScrolledWindow scrollTree;
        public РозміщенняНоменклатуриПоКоміркам_Objest? РозміщенняНоменклатуриПоКоміркам_Objest { get; set; }

        #region Записи

        enum Columns
        {
            НомерРядка,
            Номенклатура,
            Пакування,
            Комірка
        }

        ListStore Store = new ListStore(
            typeof(int),      //НомерРядка
            typeof(string),   //Номенклатура
            typeof(string),   //Пакування
            typeof(string)    //Комірка
        );

        List<Запис> Записи = new List<Запис>();

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            public int НомерРядка { get; set; }
            public Номенклатура_Pointer Номенклатура { get; set; } = new Номенклатура_Pointer();
            public ПакуванняОдиниціВиміру_Pointer Пакування { get; set; } = new ПакуванняОдиниціВиміру_Pointer();
            public СкладськіКомірки_Pointer Комірка { get; set; } = new СкладськіКомірки_Pointer();

            public object[] ToArray()
            {
                return new object[]
                {
                    НомерРядка,
                    Номенклатура.Назва,
                    Пакування.Назва,
                    Комірка.Назва
                };
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    Номенклатура = запис.Номенклатура,
                    Пакування = запис.Пакування,
                    Комірка = запис.Комірка
                };
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
            }
            public static void ПісляЗміни_Пакування(Запис запис)
            {
                запис.Пакування.GetPresentation();
            }
            public static void ПісляЗміни_Комірка(Запис запис)
            {
                запис.Комірка.GetPresentation();
            }
        }

        #endregion

        TreeView TreeViewGrid;

        public РозміщенняНоменклатуриПоКоміркам_ТабличнаЧастина_Товари() : base()
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
                        case Columns.Комірка:
                            {
                                СкладськіКомірки_ШвидкийВибір page = new СкладськіКомірки_ШвидкийВибір() { PopoverParent = PopoverSmallSelect, DirectoryPointerItem = запис.Комірка };
                                page.CallBack_OnSelectPointer = (СкладськіКомірки_Pointer selectPointer) =>
                                {
                                    запис.Комірка = selectPointer;
                                    Запис.ПісляЗміни_Комірка(запис);

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

            //
            //
            //

            ToolButton fillDirectoryButton = new ToolButton(Stock.Add) { Label = "Заповнити товарами", IsImportant = true };
            fillDirectoryButton.Clicked += OnFillDirectory;
            toolbar.Add(fillDirectoryButton);

            ToolButton fillRegisterButton = new ToolButton(Stock.Add) { Label = "Заповнити комірками", IsImportant = true };
            fillRegisterButton.Clicked += OnFillRegister;
            toolbar.Add(fillRegisterButton);
        }

        public void LoadRecords()
        {
            Store.Clear();
            Записи.Clear();

            if (РозміщенняНоменклатуриПоКоміркам_Objest != null)
            {
                Query querySelect = РозміщенняНоменклатуриПоКоміркам_Objest.Товари_TablePart.QuerySelect;
                querySelect.Clear();

                //JOIN Номенклатура
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(Номенклатура_Const.TABLE + "." + Номенклатура_Const.Назва, "Номенклатура"));
                querySelect.Joins.Add(
                    new Join(Номенклатура_Const.TABLE, РозміщенняНоменклатуриПоКоміркам_Товари_TablePart.Номенклатура, querySelect.Table));

                //JOIN Пакування
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ПакуванняОдиниціВиміру_Const.TABLE + "." + ПакуванняОдиниціВиміру_Const.Назва, "Пакування"));
                querySelect.Joins.Add(
                    new Join(ПакуванняОдиниціВиміру_Const.TABLE, РозміщенняНоменклатуриПоКоміркам_Товари_TablePart.Пакування, querySelect.Table));

                //JOIN Комірка
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(СкладськіКомірки_Const.TABLE + "." + СкладськіКомірки_Const.Назва, "Комірка"));
                querySelect.Joins.Add(
                    new Join(СкладськіКомірки_Const.TABLE, РозміщенняНоменклатуриПоКоміркам_Товари_TablePart.Комірка, querySelect.Table));

                //ORDER
                querySelect.Order.Add(РозміщенняНоменклатуриПоКоміркам_Товари_TablePart.НомерРядка, SelectOrder.ASC);

                РозміщенняНоменклатуриПоКоміркам_Objest.Товари_TablePart.Read();

                Dictionary<string, Dictionary<string, string>> JoinValue = РозміщенняНоменклатуриПоКоміркам_Objest.Товари_TablePart.JoinValue;

                foreach (РозміщенняНоменклатуриПоКоміркам_Товари_TablePart.Record record in РозміщенняНоменклатуриПоКоміркам_Objest.Товари_TablePart.Records)
                {
                    string uid = record.UID.ToString();

                    record.Номенклатура.Назва = JoinValue[uid]["Номенклатура"];
                    record.Пакування.Назва = JoinValue[uid]["Пакування"];
                    record.Комірка.Назва = JoinValue[uid]["Комірка"];

                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        НомерРядка = record.НомерРядка,
                        Номенклатура = record.Номенклатура,
                        Пакування = record.Пакування,
                        Комірка = record.Комірка
                    };

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }
        }

        public void SaveRecords()
        {
            if (РозміщенняНоменклатуриПоКоміркам_Objest != null)
            {
                РозміщенняНоменклатуриПоКоміркам_Objest.Товари_TablePart.Records.Clear();

                int sequenceNumber = 0;

                foreach (Запис запис in Записи)
                {
                    РозміщенняНоменклатуриПоКоміркам_Товари_TablePart.Record record = new РозміщенняНоменклатуриПоКоміркам_Товари_TablePart.Record();

                    record.UID = запис.ID;
                    record.НомерРядка = ++sequenceNumber;
                    record.Номенклатура = запис.Номенклатура;
                    record.Пакування = запис.Пакування;
                    record.Комірка = запис.Комірка;

                    РозміщенняНоменклатуриПоКоміркам_Objest.Товари_TablePart.Records.Add(record);
                }

                РозміщенняНоменклатуриПоКоміркам_Objest.Товари_TablePart.Save(true);
            }
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

            //Пакування
            {
                TreeViewColumn Пакування = new TreeViewColumn("Пакування", new CellRendererText(), "text", (int)Columns.Пакування) { Resizable = true, MinWidth =  100 };
                Пакування.Data.Add("Column", Columns.Пакування);

                TreeViewGrid.AppendColumn(Пакування);
            }

            //Комірка
            {
                TreeViewColumn Комірка = new TreeViewColumn("Комірка", new CellRendererText(), "text", (int)Columns.Комірка) { Resizable = true, MinWidth =  100 };
                Комірка.Data.Add("Column", Columns.Комірка);

                TreeViewGrid.AppendColumn(Комірка);
            }

            //Колонка пустишка для заповнення вільного простору
            TreeViewGrid.AppendColumn(new TreeViewColumn());
        }

        /*

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
                    case Columns.Кількість:
                        {
                            cellText.Text = запис.Кількість.ToString();
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
                    case Columns.Кількість:
                        {
                            var (check, value) = Validate.IsDecimal(args.NewText);
                            if (check)
                            {
                                запис.Кількість = value;
                            }

                            break;
                        }
                }

                Store.SetValues(iter, запис.ToArray());
            }
        }
        
        */

        #endregion

        #region ToolBar

        void OnAddClick(object? sender, EventArgs args)
        {
            Запис запис = new Запис();
            Записи.Add(запис);

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

        void OnFillDirectory(object? sender, EventArgs args)
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
    Номенклатура.{Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Товар}
ORDER BY Номенклатура_Назва
";
            Store.Clear();
            Записи.Clear();

            if (РозміщенняНоменклатуриПоКоміркам_Objest != null)
            {
                Dictionary<string, object> paramQuery = new Dictionary<string, object>();

                string[] columnsName;
                List<Dictionary<string, object>> listRow;

                Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

                foreach (Dictionary<string, object> row in listRow)
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

        void OnFillRegister(object? sender, EventArgs args)
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

            if (РозміщенняНоменклатуриПоКоміркам_Objest != null)
            {
                Dictionary<string, object> paramQuery = new Dictionary<string, object>();

                string[] columnsName;
                List<Dictionary<string, object>> listRow;

                Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

                foreach (Dictionary<string, object> row in listRow)
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