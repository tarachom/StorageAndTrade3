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

using Константи = StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ВведенняЗалишків_ТабличнаЧастина_Товари : VBox
    {
        public ВведенняЗалишків_Objest? ВведенняЗалишків_Objest { get; set; }

        #region Записи

        enum Columns
        {
            НомерРядка,
            НоменклатураНазва,
            ХарактеристикаНазва,
            СеріяНазва,
            КількістьУпаковок,
            ПакуванняНазва,
            Кількість,
            Ціна,
            Сума
        }

        ListStore Store = new ListStore(
            typeof(int),      //НомерРядка
            typeof(string),   //НоменклатураНазва
            typeof(string),   //ХарактеристикаНазва
            typeof(string),   //СеріяНазва
            typeof(int),      //КількістьУпаковок
            typeof(string),   //ПакуванняНазва
            typeof(float),    //Кількість
            typeof(float),    //Ціна
            typeof(float)     //Сума
        );

        List<Запис> Записи = new List<Запис>();

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            public int НомерРядка { get; set; }
            public Номенклатура_Pointer Номенклатура { get; set; } = new Номенклатура_Pointer();
            public string НоменклатураНазва { get; set; } = "";
            public ХарактеристикиНоменклатури_Pointer Характеристика { get; set; } = new ХарактеристикиНоменклатури_Pointer();
            public string ХарактеристикаНазва { get; set; } = "";
            public СеріїНоменклатури_Pointer Серія { get; set; } = new СеріїНоменклатури_Pointer();
            public string СеріяНазва { get; set; } = "";
            public int КількістьУпаковок { get; set; } = 1;
            public ПакуванняОдиниціВиміру_Pointer Пакування { get; set; } = new ПакуванняОдиниціВиміру_Pointer();
            public string ПакуванняНазва { get; set; } = "";
            public decimal Кількість { get; set; } = 1;
            public decimal Ціна { get; set; }
            public decimal Сума { get; set; }

            public object[] ToArray()
            {
                return new object[]
                {
                    НомерРядка,
                    НоменклатураНазва,
                    ХарактеристикаНазва,
                    СеріяНазва,
                    КількістьУпаковок,
                    ПакуванняНазва,
                    (float)Кількість,
                    (float)Ціна,
                    (float)Сума
                };
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    Номенклатура = запис.Номенклатура,
                    НоменклатураНазва = запис.НоменклатураНазва,
                    Характеристика = запис.Характеристика,
                    ХарактеристикаНазва = запис.ХарактеристикаНазва,
                    Серія = запис.Серія,
                    СеріяНазва = запис.СеріяНазва,
                    КількістьУпаковок = запис.КількістьУпаковок,
                    Пакування = запис.Пакування,
                    ПакуванняНазва = запис.ПакуванняНазва,
                    Кількість = запис.Кількість,
                    Ціна = запис.Ціна,
                    Сума = запис.Сума
                };
            }

            public static void ПісляЗміни_Номенклатура(Запис запис)
            {
                if (запис.Номенклатура.IsEmpty())
                {
                    запис.НоменклатураНазва = "";
                    return;
                }

                Номенклатура_Objest? номенклатура_Objest = запис.Номенклатура.GetDirectoryObject();
                if (номенклатура_Objest != null)
                {
                    запис.НоменклатураНазва = номенклатура_Objest.Назва;

                    if (!номенклатура_Objest.ОдиницяВиміру.IsEmpty())
                        запис.Пакування = номенклатура_Objest.ОдиницяВиміру;
                }
                else
                {
                    запис.НоменклатураНазва = "";
                    запис.Пакування = new ПакуванняОдиниціВиміру_Pointer();
                }

                if (!запис.Пакування.IsEmpty())
                {
                    ПакуванняОдиниціВиміру_Objest? пакуванняОдиниціВиміру_Objest = запис.Пакування.GetDirectoryObject();
                    if (пакуванняОдиниціВиміру_Objest != null)
                    {
                        запис.ПакуванняНазва = пакуванняОдиниціВиміру_Objest.Назва;
                        запис.КількістьУпаковок = пакуванняОдиниціВиміру_Objest.КількістьУпаковок;
                    }
                    else
                    {
                        запис.ПакуванняНазва = "";
                        запис.КількістьУпаковок = 1;
                    }
                }
            }
            public static void ПісляЗміни_Характеристика(Запис запис)
            {
                запис.ХарактеристикаНазва = запис.Характеристика.GetPresentation();
            }
            public static void ПісляЗміни_Серія(Запис запис)
            {
                запис.СеріяНазва = запис.Серія.GetPresentation();
            }
            public static void ПісляЗміни_Пакування(Запис запис)
            {
                запис.ПакуванняНазва = запис.Пакування.GetPresentation();
            }
            public static void ПісляЗміни_КількістьАбоЦіна(Запис запис)
            {
                запис.Сума = запис.Кількість * запис.Ціна;
            }
        }

        #endregion

        TreeView TreeViewGrid;

        public ВведенняЗалишків_ТабличнаЧастина_Товари() : base()
        {
            new VBox();

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
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
            if (args.Event.Type == Gdk.EventType.DoubleButtonPress)
            {
                TreePath itemPath;
                TreeViewColumn treeColumn;

                TreeViewGrid.GetCursor(out itemPath, out treeColumn);

                if (treeColumn.Data.ContainsKey("Column"))
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    int rowNumber = int.Parse(itemPath.ToString());
                    Запис запис = Записи[rowNumber];

                    switch ((Columns)treeColumn.Data["Column"]!)
                    {
                        case Columns.НоменклатураНазва:
                            {
                                Номенклатура page = new Номенклатура(true);

                                page.DirectoryPointerItem = запис.Номенклатура;
                                page.CallBack_OnSelectPointer = (Номенклатура_Pointer selectPointer) =>
                                {
                                    запис.Номенклатура = selectPointer;
                                    Запис.ПісляЗміни_Номенклатура(запис);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                Program.GeneralForm?.CreateNotebookPage("Вибір - Номенклатура", () => { return page; }, true);

                                page.LoadTree();

                                break;
                            }
                        case Columns.ХарактеристикаНазва:
                            {
                                ХарактеристикиНоменклатури page = new ХарактеристикиНоменклатури(true);

                                page.НоменклатураВласник.Pointer = запис.Номенклатура;
                                page.DirectoryPointerItem = запис.Характеристика;
                                page.CallBack_OnSelectPointer = (ХарактеристикиНоменклатури_Pointer selectPointer) =>
                                {
                                    запис.Характеристика = selectPointer;
                                    Запис.ПісляЗміни_Характеристика(запис);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                Program.GeneralForm?.CreateNotebookPage("Вибір - Характеристика", () => { return page; }, true);

                                page.LoadRecords();

                                break;
                            }
                        case Columns.СеріяНазва:
                            {
                                СеріїНоменклатури page = new СеріїНоменклатури(true);

                                page.DirectoryPointerItem = запис.Серія;
                                page.CallBack_OnSelectPointer = (СеріїНоменклатури_Pointer selectPointer) =>
                                {
                                    запис.Серія = selectPointer;
                                    Запис.ПісляЗміни_Серія(запис);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                Program.GeneralForm?.CreateNotebookPage("Вибір - Серія", () => { return page; }, true);

                                page.LoadRecords();

                                break;
                            }
                        case Columns.ПакуванняНазва:
                            {
                                ПакуванняОдиниціВиміру page = new ПакуванняОдиниціВиміру(true);

                                page.DirectoryPointerItem = запис.Пакування;
                                page.CallBack_OnSelectPointer = (ПакуванняОдиниціВиміру_Pointer selectPointer) =>
                                {
                                    запис.Пакування = selectPointer;
                                    Запис.ПісляЗміни_Пакування(запис);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                Program.GeneralForm?.CreateNotebookPage("Вибір - Пакування", () => { return page; }, true);

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

            ToolButton upButton = new ToolButton(Stock.Add) { Label = "Додати", IsImportant = true };
            upButton.Clicked += OnAddClick;
            toolbar.Add(upButton);

            ToolButton copyButton = new ToolButton(Stock.Copy) { Label = "Копіювати", IsImportant = true };
            copyButton.Clicked += OnCopyClick;
            toolbar.Add(copyButton);

            ToolButton deleteButton = new ToolButton(Stock.Delete) { Label = "Видалити", IsImportant = true };
            deleteButton.Clicked += OnDeleteClick;
            toolbar.Add(deleteButton);
        }

        public void LoadRecords()
        {
            Store.Clear();
            Записи.Clear();

            if (ВведенняЗалишків_Objest != null)
            {
                Query querySelect = ВведенняЗалишків_Objest.Товари_TablePart.QuerySelect;
                querySelect.Clear();

                //JOIN 1
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(Номенклатура_Const.TABLE + "." + Номенклатура_Const.Назва, "tovar_name"));
                querySelect.Joins.Add(
                    new Join(Номенклатура_Const.TABLE, ВведенняЗалишків_Товари_TablePart.Номенклатура, querySelect.Table));

                //JOIN 2
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ПакуванняОдиниціВиміру_Const.TABLE + "." + ПакуванняОдиниціВиміру_Const.Назва, "pak_name"));
                querySelect.Joins.Add(
                    new Join(ПакуванняОдиниціВиміру_Const.TABLE, ВведенняЗалишків_Товари_TablePart.Пакування, querySelect.Table));

                //JOIN 3
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ХарактеристикиНоменклатури_Const.TABLE + "." + ХарактеристикиНоменклатури_Const.Назва, "xar_name"));
                querySelect.Joins.Add(
                    new Join(ХарактеристикиНоменклатури_Const.TABLE, ВведенняЗалишків_Товари_TablePart.ХарактеристикаНоменклатури, querySelect.Table));

                //JOIN 4
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(СеріїНоменклатури_Const.TABLE + "." + СеріїНоменклатури_Const.Номер, "seria_number"));
                querySelect.Joins.Add(
                    new Join(СеріїНоменклатури_Const.TABLE, ВведенняЗалишків_Товари_TablePart.Серія, querySelect.Table));

                //ORDER
                querySelect.Order.Add(ВведенняЗалишків_Товари_TablePart.НомерРядка, SelectOrder.ASC);

                ВведенняЗалишків_Objest.Товари_TablePart.Read();

                Dictionary<string, Dictionary<string, string>> join = ВведенняЗалишків_Objest.Товари_TablePart.JoinValue;

                foreach (ВведенняЗалишків_Товари_TablePart.Record record in ВведенняЗалишків_Objest.Товари_TablePart.Records)
                {
                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        НомерРядка = record.НомерРядка,
                        Номенклатура = record.Номенклатура,
                        НоменклатураНазва = join[record.UID.ToString()]["tovar_name"],
                        Характеристика = record.ХарактеристикаНоменклатури,
                        ХарактеристикаНазва = join[record.UID.ToString()]["xar_name"],
                        Серія = record.Серія,
                        СеріяНазва = join[record.UID.ToString()]["seria_number"],
                        КількістьУпаковок = record.КількістьУпаковок,
                        Пакування = record.Пакування,
                        ПакуванняНазва = join[record.UID.ToString()]["pak_name"],
                        Кількість = record.Кількість,
                        Ціна = record.Ціна,
                        Сума = record.Сума
                    };

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }
        }

        public void SaveRecords()
        {
            if (ВведенняЗалишків_Objest != null)
            {
                ВведенняЗалишків_Objest.Товари_TablePart.Records.Clear();

                int sequenceNumber = 0;

                foreach (Запис запис in Записи)
                {
                    ВведенняЗалишків_Товари_TablePart.Record record = new ВведенняЗалишків_Товари_TablePart.Record();

                    record.UID = запис.ID;
                    record.НомерРядка = ++sequenceNumber;
                    record.Номенклатура = запис.Номенклатура;
                    record.ХарактеристикаНоменклатури = запис.Характеристика;
                    record.Серія = запис.Серія;
                    record.КількістьУпаковок = запис.КількістьУпаковок;
                    record.Пакування = запис.Пакування;
                    record.Кількість = запис.Кількість;
                    record.Ціна = запис.Ціна;
                    record.Сума = запис.Сума;

                    ВведенняЗалишків_Objest.Товари_TablePart.Records.Add(record);
                }

                ВведенняЗалишків_Objest.Товари_TablePart.Save(true);
            }
        }

        #region TreeView

        void AddColumn()
        {
            //НомерРядка
            TreeViewGrid.AppendColumn(new TreeViewColumn("№", new CellRendererText(), "text", (int)Columns.НомерРядка) { MinWidth = 30 });

            //НоменклатураНазва
            {
                TreeViewColumn НоменклатураНазва = new TreeViewColumn("Номенклатура", new CellRendererText(), "text", (int)Columns.НоменклатураНазва) { MinWidth = 300 };
                НоменклатураНазва.Data.Add("Column", Columns.НоменклатураНазва);

                TreeViewGrid.AppendColumn(НоменклатураНазва);
            }

            //ХарактеристикаНазва
            {
                TreeViewColumn ХарактеристикаНазва = new TreeViewColumn("Характеристика", new CellRendererText(), "text", (int)Columns.ХарактеристикаНазва) { MinWidth = 300 };
                ХарактеристикаНазва.Visible = Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const;
                ХарактеристикаНазва.Data.Add("Column", Columns.ХарактеристикаНазва);

                TreeViewGrid.AppendColumn(ХарактеристикаНазва);
            }

            //СеріяНазва
            {
                TreeViewColumn СеріяНазва = new TreeViewColumn("Серія", new CellRendererText(), "text", (int)Columns.СеріяНазва) { MinWidth = 300 };
                СеріяНазва.Visible = Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const;
                СеріяНазва.Data.Add("Column", Columns.СеріяНазва);

                TreeViewGrid.AppendColumn(СеріяНазва);
            }

            //КількістьУпаковок
            {
                CellRendererText КількістьУпаковок = new CellRendererText() { Editable = true };
                КількістьУпаковок.Edited += TextChanged;
                КількістьУпаковок.Data.Add("Column", (int)Columns.КількістьУпаковок);

                TreeViewColumn Column = new TreeViewColumn("Пак", КількістьУпаковок, "text", (int)Columns.КількістьУпаковок) { MinWidth = 50 };
                Column.SetCellDataFunc(КількістьУпаковок, new TreeCellDataFunc(NumericCellDataFunc));
                TreeViewGrid.AppendColumn(Column);
            }

            //ПакуванняНазва
            {
                TreeViewColumn ПакуванняНазва = new TreeViewColumn("Пакування", new CellRendererText(), "text", (int)Columns.ПакуванняНазва) { MinWidth = 100 };
                ПакуванняНазва.Data.Add("Column", Columns.ПакуванняНазва);

                TreeViewGrid.AppendColumn(ПакуванняНазва);
            }

            //Кількість
            {
                CellRendererText Кількість = new CellRendererText() { Editable = true };
                Кількість.Edited += TextChanged;
                Кількість.Data.Add("Column", (int)Columns.Кількість);

                TreeViewColumn Column = new TreeViewColumn("Кількість", Кількість, "text", (int)Columns.Кількість) { MinWidth = 100 };
                Column.SetCellDataFunc(Кількість, new TreeCellDataFunc(NumericCellDataFunc));
                TreeViewGrid.AppendColumn(Column);
            }

            //Ціна
            {
                CellRendererText Ціна = new CellRendererText() { Editable = true };
                Ціна.Edited += TextChanged;
                Ціна.Data.Add("Column", (int)Columns.Ціна);

                TreeViewColumn Column = new TreeViewColumn("Ціна", Ціна, "text", (int)Columns.Ціна) { MinWidth = 100 };
                Column.SetCellDataFunc(Ціна, new TreeCellDataFunc(NumericCellDataFunc));
                TreeViewGrid.AppendColumn(Column);
            }

            //Сума
            {
                CellRendererText Сума = new CellRendererText() { Editable = true };
                Сума.Edited += TextChanged;
                Сума.Data.Add("Column", (int)Columns.Сума);

                TreeViewColumn Column = new TreeViewColumn("Сума", Сума, "text", (int)Columns.Сума) { MinWidth = 100 };
                Column.SetCellDataFunc(Сума, new TreeCellDataFunc(NumericCellDataFunc));
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

        #endregion

    }
}