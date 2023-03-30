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
using Константи = StorageAndTrade_1_0.Константи;
using Перелічення = StorageAndTrade_1_0.Перелічення;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.РегістриВідомостей;

namespace StorageAndTrade
{
    class ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари : VBox
    {
        ScrolledWindow scrollTree;
        public ВстановленняЦінНоменклатури_Objest? ВстановленняЦінНоменклатури_Objest { get; set; }
        public System.Action? ОбновитиЗначенняДокумента { get; set; }

        #region Записи

        enum Columns
        {
            НомерРядка,
            Номенклатура,
            Характеристика,
            Пакування,
            ВидЦіни,
            Ціна
        }

        ListStore Store = new ListStore(
            typeof(int),      //НомерРядка
            typeof(string),   //Номенклатура
            typeof(string),   //Характеристика
            typeof(string),   //Пакування
            typeof(string),   //ВидЦіни
            typeof(float)     //Ціна
        );

        List<Запис> Записи = new List<Запис>();

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            public int НомерРядка { get; set; }
            public Номенклатура_Pointer Номенклатура { get; set; } = new Номенклатура_Pointer();
            public ХарактеристикиНоменклатури_Pointer Характеристика { get; set; } = new ХарактеристикиНоменклатури_Pointer();
            public ПакуванняОдиниціВиміру_Pointer Пакування { get; set; } = new ПакуванняОдиниціВиміру_Pointer();
            public ВидиЦін_Pointer ВидЦіни { get; set; } = new ВидиЦін_Pointer();
            public decimal Ціна { get; set; }

            public object[] ToArray()
            {
                return new object[]
                {
                    НомерРядка,
                    Номенклатура.Назва,
                    Характеристика.Назва,
                    Пакування.Назва,
                    ВидЦіни.Назва,
                    (float)Ціна
                };
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    Номенклатура = запис.Номенклатура,
                    Характеристика = запис.Характеристика,
                    Пакування = запис.Пакування,
                    ВидЦіни = запис.ВидЦіни,
                    Ціна = запис.Ціна
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
        }

        #endregion

        TreeView TreeViewGrid;

        public ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари() : base()
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
                        case Columns.Характеристика:
                            {
                                ХарактеристикиНоменклатури_ШвидкийВибір page = new ХарактеристикиНоменклатури_ШвидкийВибір() { PopoverParent = PopoverSmallSelect, DirectoryPointerItem = запис.Характеристика };

                                page.НоменклатураВласник.Pointer = запис.Номенклатура;
                                page.CallBack_OnSelectPointer = (ХарактеристикиНоменклатури_Pointer selectPointer) =>
                                {
                                    запис.Характеристика = selectPointer;
                                    Запис.ПісляЗміни_Характеристика(запис);

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

            //Separator
            ToolItem toolItemSeparator = new ToolItem();
            toolItemSeparator.Add(new Separator(Orientation.Horizontal));
            toolbar.Add(toolItemSeparator);

            ToolButton fillDirectoryButton = new ToolButton(Stock.Add) { Label = "Заповнити товарами", IsImportant = true };
            fillDirectoryButton.Clicked += OnFillDirectory;
            toolbar.Add(fillDirectoryButton);

            ToolButton fillRegisterButton = new ToolButton(Stock.Add) { Label = "Заповнити товарами з цінами", IsImportant = true };
            fillRegisterButton.Clicked += OnFillRegister;
            toolbar.Add(fillRegisterButton);
        }

        public void LoadRecords()
        {
            Store.Clear();
            Записи.Clear();

            if (ВстановленняЦінНоменклатури_Objest != null)
            {
                Query querySelect = ВстановленняЦінНоменклатури_Objest.Товари_TablePart.QuerySelect;
                querySelect.Clear();

                //JOIN Номенклатура
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(Номенклатура_Const.TABLE + "." + Номенклатура_Const.Назва, "Номенклатура"));
                querySelect.Joins.Add(
                    new Join(Номенклатура_Const.TABLE, ВстановленняЦінНоменклатури_Товари_TablePart.Номенклатура, querySelect.Table));

                //JOIN Характеристика
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ХарактеристикиНоменклатури_Const.TABLE + "." + ХарактеристикиНоменклатури_Const.Назва, "Характеристика"));
                querySelect.Joins.Add(
                    new Join(ХарактеристикиНоменклатури_Const.TABLE, ВстановленняЦінНоменклатури_Товари_TablePart.ХарактеристикаНоменклатури, querySelect.Table));

                //JOIN Пакування
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ПакуванняОдиниціВиміру_Const.TABLE + "." + ПакуванняОдиниціВиміру_Const.Назва, "Пакування"));
                querySelect.Joins.Add(
                    new Join(ПакуванняОдиниціВиміру_Const.TABLE, ВстановленняЦінНоменклатури_Товари_TablePart.Пакування, querySelect.Table));

                //JOIN ВидЦін
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ВидиЦін_Const.TABLE + "." + ВидиЦін_Const.Назва, "ВидЦін"));
                querySelect.Joins.Add(
                    new Join(ВидиЦін_Const.TABLE, ВстановленняЦінНоменклатури_Товари_TablePart.ВидЦіни, querySelect.Table));

                //ORDER
                querySelect.Order.Add(ВстановленняЦінНоменклатури_Товари_TablePart.НомерРядка, SelectOrder.ASC);

                ВстановленняЦінНоменклатури_Objest.Товари_TablePart.Read();

                Dictionary<string, Dictionary<string, string>> JoinValue = ВстановленняЦінНоменклатури_Objest.Товари_TablePart.JoinValue;

                foreach (ВстановленняЦінНоменклатури_Товари_TablePart.Record record in ВстановленняЦінНоменклатури_Objest.Товари_TablePart.Records)
                {
                    string uid = record.UID.ToString();

                    record.Номенклатура.Назва = JoinValue[uid]["Номенклатура"];
                    record.ХарактеристикаНоменклатури.Назва = JoinValue[uid]["Характеристика"];
                    record.Пакування.Назва = JoinValue[uid]["Пакування"];
                    record.ВидЦіни.Назва = JoinValue[uid]["ВидЦін"];

                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        НомерРядка = record.НомерРядка,
                        Номенклатура = record.Номенклатура,
                        Характеристика = record.ХарактеристикаНоменклатури,
                        Пакування = record.Пакування,
                        ВидЦіни = record.ВидЦіни,
                        Ціна = record.Ціна
                    };

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }
        }

        public void SaveRecords()
        {
            if (ВстановленняЦінНоменклатури_Objest != null)
            {
                ВстановленняЦінНоменклатури_Objest.Товари_TablePart.Records.Clear();

                int sequenceNumber = 0;

                foreach (Запис запис in Записи)
                {
                    ВстановленняЦінНоменклатури_Товари_TablePart.Record record = new ВстановленняЦінНоменклатури_Товари_TablePart.Record();

                    record.UID = запис.ID;
                    record.НомерРядка = ++sequenceNumber;
                    record.Номенклатура = запис.Номенклатура;
                    record.ХарактеристикаНоменклатури = запис.Характеристика;
                    record.Пакування = запис.Пакування;
                    record.ВидЦіни = запис.ВидЦіни;
                    record.Ціна = запис.Ціна;

                    ВстановленняЦінНоменклатури_Objest.Товари_TablePart.Records.Add(record);
                }

                ВстановленняЦінНоменклатури_Objest.Товари_TablePart.Save(true);
            }
        }

        public string КлючовіСловаДляПошуку()
        {
            string ключовіСлова = "";

            if (ВстановленняЦінНоменклатури_Objest != null)
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

            //Пакування
            {
                TreeViewColumn Пакування = new TreeViewColumn("Пакування", new CellRendererText(), "text", (int)Columns.Пакування) { Resizable = true, MinWidth =  100 };
                Пакування.Data.Add("Column", Columns.Пакування);

                TreeViewGrid.AppendColumn(Пакування);
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
                    case Columns.Ціна:
                        {
                            cellText.Text = запис.Ціна.ToString();
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
                    case Columns.Ціна:
                        {
                            var (check, value) = Validate.IsDecimal(args.NewText);
                            if (check)
                                запис.Ціна = value;

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
            if (ОбновитиЗначенняДокумента != null)
                ОбновитиЗначенняДокумента.Invoke();

            string query = $@"
SELECT
    Номенклатура.uid AS Номенклатура,
    Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    Номенклатура.{Номенклатура_Const.ОдиницяВиміру} AS Пакування,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS Пакування_Назва,
    (
        SELECT
            {ЦіниНоменклатури_Const.Ціна}
        FROM
            {ЦіниНоменклатури_Const.TABLE} AS ЦіниНоменклатури
        WHERE
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.Номенклатура} = Номенклатура.uid AND
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.ХарактеристикаНоменклатури} = '{Guid.Empty}' AND
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.Пакування} = Номенклатура.{Номенклатура_Const.ОдиницяВиміру} AND
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.ВидЦіни} = @vid_cen AND
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.Валюта} = @valuta
        ORDER BY ЦіниНоменклатури.period DESC
        LIMIT 1
    ) AS Ціна
FROM
    {Номенклатура_Const.TABLE} AS Номенклатура
    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON 
        Довідник_ПакуванняОдиниціВиміру.uid = Номенклатура.{Номенклатура_Const.ОдиницяВиміру}
WHERE
    Номенклатура.{Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Товар} OR
    Номенклатура.{Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Послуга}
ORDER BY Номенклатура_Назва, Пакування_Назва
";
            Store.Clear();
            Записи.Clear();

            if (ВстановленняЦінНоменклатури_Objest != null)
            {
                Dictionary<string, object> paramQuery = new Dictionary<string, object>();
                paramQuery.Add("valuta", ВстановленняЦінНоменклатури_Objest.Валюта.UnigueID.UGuid);
                paramQuery.Add("vid_cen", ВстановленняЦінНоменклатури_Objest.ВидЦіни.UnigueID.UGuid);

                string[] columnsName;
                List<Dictionary<string, object>> listRow;

                Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

                string ВидЦіниНазва = ВстановленняЦінНоменклатури_Objest.ВидЦіни.GetPresentation();

                foreach (Dictionary<string, object> row in listRow)
                {
                    Запис запис = new Запис
                    {
                        ID = Guid.Empty,
                        Номенклатура = new Номенклатура_Pointer(row["Номенклатура"]),
                        Характеристика = new ХарактеристикиНоменклатури_Pointer(),
                        Пакування = new ПакуванняОдиниціВиміру_Pointer(row["Пакування"]),
                        ВидЦіни = ВстановленняЦінНоменклатури_Objest.ВидЦіни,
                        Ціна = (row["Ціна"] != DBNull.Value ? (decimal)row["Ціна"] : 0)
                    };

                    запис.Номенклатура.Назва = row["Номенклатура_Назва"].ToString() ?? "";
                    запис.Пакування.Назва = row["Пакування_Назва"].ToString() ?? "";
                    запис.ВидЦіни.Назва = ВидЦіниНазва;

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }
        }

        void OnFillRegister(object? sender, EventArgs args)
        {
            if (ОбновитиЗначенняДокумента != null)
                ОбновитиЗначенняДокумента.Invoke();

            string query = $@"
WITH register AS
(
    SELECT DISTINCT {ЦіниНоменклатури_Const.Номенклатура} AS Номенклатура,
        {ЦіниНоменклатури_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        {ЦіниНоменклатури_Const.Пакування} AS Пакування,
        {ЦіниНоменклатури_Const.ВидЦіни} AS ВидЦіни
    FROM
        {ЦіниНоменклатури_Const.TABLE}
    WHERE
        {ЦіниНоменклатури_Const.Валюта} = @valuta";

            #region WHERE

            if (ВстановленняЦінНоменклатури_Objest != null && !ВстановленняЦінНоменклатури_Objest.ВидЦіни.IsEmpty())
            {
                query += $@"
AND {ЦіниНоменклатури_Const.ВидЦіни} = @vid_cen
";
            }

            #endregion

            query += $@"
)
SELECT
    register.Номенклатура,
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    register.ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    register.Пакування,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS Пакування_Назва,
    register.ВидЦіни,
    Довідник_ВидиЦін.{ВидиЦін_Const.Назва} AS ВидЦіни_Назва,
    (
        SELECT 
            {ЦіниНоменклатури_Const.Ціна}
        FROM 
            {ЦіниНоменклатури_Const.TABLE} AS ЦіниНоменклатури
        WHERE
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.Номенклатура} = register.Номенклатура AND
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.ХарактеристикаНоменклатури} = register.ХарактеристикаНоменклатури AND
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.Пакування} = register.Пакування AND
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.ВидЦіни} = register.ВидЦіни AND
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.Валюта} = @valuta
        ORDER BY ЦіниНоменклатури.period DESC
        LIMIT 1
    ) AS Ціна
FROM
    register
    
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON 
        Довідник_Номенклатура.uid = register.Номенклатура
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON 
        Довідник_ХарактеристикиНоменклатури.uid = register.ХарактеристикаНоменклатури
    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON 
        Довідник_ПакуванняОдиниціВиміру.uid = register.Пакування
    LEFT JOIN {ВидиЦін_Const.TABLE} AS Довідник_ВидиЦін ON 
        Довідник_ВидиЦін.uid = register.ВидЦіни
ORDER BY
    Номенклатура_Назва, ХарактеристикаНоменклатури_Назва, Пакування_Назва, ВидЦіни_Назва
";

            Store.Clear();
            Записи.Clear();

            if (ВстановленняЦінНоменклатури_Objest != null)
            {
                Dictionary<string, object> paramQuery = new Dictionary<string, object>();
                paramQuery.Add("valuta", ВстановленняЦінНоменклатури_Objest.Валюта.UnigueID.UGuid);
                paramQuery.Add("vid_cen", ВстановленняЦінНоменклатури_Objest.ВидЦіни.UnigueID.UGuid);

                string[] columnsName;
                List<Dictionary<string, object>> listRow;

                Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

                foreach (Dictionary<string, object> row in listRow)
                {
                    Запис запис = new Запис
                    {
                        ID = Guid.Empty,
                        Номенклатура = new Номенклатура_Pointer(row["Номенклатура"]),
                        Характеристика = new ХарактеристикиНоменклатури_Pointer(row["ХарактеристикаНоменклатури"]),
                        Пакування = new ПакуванняОдиниціВиміру_Pointer(row["Пакування"]),
                        ВидЦіни = new ВидиЦін_Pointer(row["ВидЦіни"]),
                        Ціна = (decimal)row["Ціна"]
                    };

                    запис.Номенклатура.Назва = row["Номенклатура_Назва"].ToString() ?? "";
                    запис.Пакування.Назва = row["Пакування_Назва"].ToString() ?? "";
                    запис.ВидЦіни.Назва = row["ВидЦіни_Назва"].ToString() ?? "";

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }

        }

        #endregion
    }
}