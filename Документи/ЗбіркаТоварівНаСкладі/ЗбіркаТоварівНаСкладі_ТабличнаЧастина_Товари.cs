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
using StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade
{
    class ЗбіркаТоварівНаСкладі_ТабличнаЧастина_Товари : VBox
    {
        public ЗбіркаТоварівНаСкладі_Objest? ЗбіркаТоварівНаСкладі_Objest { get; set; }
        public System.Action? ОбновитиЗначенняДокумента { get; set; }

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
            КоміркаНазва
        }

        ListStore Store = new ListStore(
            typeof(int),      //НомерРядка
            typeof(string),   //НоменклатураНазва
            typeof(string),   //ХарактеристикаНазва
            typeof(string),   //СеріяНазва
            typeof(int),      //КількістьУпаковок
            typeof(string),   //ПакуванняНазва
            typeof(float),    //Кількість
            typeof(string)    //КоміркаНазва
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
            public СкладськіКомірки_Pointer Комірка { get; set; } = new СкладськіКомірки_Pointer();
            public string КоміркаНазва { get; set; } = "";

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
                    КоміркаНазва
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
                    Комірка = запис.Комірка,
                    КоміркаНазва = запис.КоміркаНазва
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
            public static void ПісляЗміни_Комірка(Запис запис)
            {
                запис.КоміркаНазва = запис.Комірка.GetPresentation();
            }
        }

        #endregion

        TreeView TreeViewGrid;

        public ЗбіркаТоварівНаСкладі_ТабличнаЧастина_Товари() : base()
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

                                    if (ОбновитиЗначенняДокумента != null)
                                        ОбновитиЗначенняДокумента.Invoke();

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
                        case Columns.КоміркаНазва:
                            {
                                СкладськіКомірки page = new СкладськіКомірки(true);

                                page.DirectoryPointerItem = запис.Комірка;
                                page.CallBack_OnSelectPointer = (СкладськіКомірки_Pointer selectPointer) =>
                                {
                                    запис.Комірка = selectPointer;
                                    Запис.ПісляЗміни_Комірка(запис);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                Program.GeneralForm?.CreateNotebookPage("Вибір - Складська комірка", () => { return page; }, true);

                                page.LoadTree();

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

            ToolButton fillButton = new ToolButton(Stock.Convert) { Label = "Розприділити", IsImportant = true };
            fillButton.Clicked += РозприділитиПоКоміркахВідповідноДоЗалишків;
            toolbar.Add(fillButton);
        }

        public void LoadRecords()
        {
            Store.Clear();
            Записи.Clear();

            if (ЗбіркаТоварівНаСкладі_Objest != null)
            {
                Query querySelect = ЗбіркаТоварівНаСкладі_Objest.Товари_TablePart.QuerySelect;
                querySelect.Clear();

                //JOIN 1
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(Номенклатура_Const.TABLE + "." + Номенклатура_Const.Назва, "tovar_name"));
                querySelect.Joins.Add(
                    new Join(Номенклатура_Const.TABLE, ЗбіркаТоварівНаСкладі_Товари_TablePart.Номенклатура, querySelect.Table));

                //JOIN 2
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ПакуванняОдиниціВиміру_Const.TABLE + "." + ПакуванняОдиниціВиміру_Const.Назва, "pak_name"));
                querySelect.Joins.Add(
                    new Join(ПакуванняОдиниціВиміру_Const.TABLE, ЗбіркаТоварівНаСкладі_Товари_TablePart.Пакування, querySelect.Table));

                //JOIN 3
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ХарактеристикиНоменклатури_Const.TABLE + "." + ХарактеристикиНоменклатури_Const.Назва, "xar_name"));
                querySelect.Joins.Add(
                    new Join(ХарактеристикиНоменклатури_Const.TABLE, ЗбіркаТоварівНаСкладі_Товари_TablePart.ХарактеристикаНоменклатури, querySelect.Table));

                //JOIN 4
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(СеріїНоменклатури_Const.TABLE + "." + СеріїНоменклатури_Const.Номер, "seria_number"));
                querySelect.Joins.Add(
                    new Join(СеріїНоменклатури_Const.TABLE, ЗбіркаТоварівНаСкладі_Товари_TablePart.Серія, querySelect.Table));

                //JOIN 5
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(СкладськіКомірки_Const.TABLE + "." + СкладськіКомірки_Const.Назва, "komirka_name"));
                querySelect.Joins.Add(
                    new Join(СкладськіКомірки_Const.TABLE, ЗбіркаТоварівНаСкладі_Товари_TablePart.Комірка, querySelect.Table));

                //ORDER
                querySelect.Order.Add(ЗбіркаТоварівНаСкладі_Товари_TablePart.НомерРядка, SelectOrder.ASC);

                ЗбіркаТоварівНаСкладі_Objest.Товари_TablePart.Read();

                Dictionary<string, Dictionary<string, string>> join = ЗбіркаТоварівНаСкладі_Objest.Товари_TablePart.JoinValue;

                foreach (ЗбіркаТоварівНаСкладі_Товари_TablePart.Record record in ЗбіркаТоварівНаСкладі_Objest.Товари_TablePart.Records)
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
                        Комірка = record.Комірка,
                        КоміркаНазва = join[record.UID.ToString()]["komirka_name"]
                    };

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }
        }

        public void SaveRecords()
        {
            if (ЗбіркаТоварівНаСкладі_Objest != null)
            {
                ЗбіркаТоварівНаСкладі_Objest.Товари_TablePart.Records.Clear();

                int sequenceNumber = 0;

                foreach (Запис запис in Записи)
                {
                    ЗбіркаТоварівНаСкладі_Товари_TablePart.Record record = new ЗбіркаТоварівНаСкладі_Товари_TablePart.Record();

                    record.UID = запис.ID;
                    record.НомерРядка = ++sequenceNumber;
                    record.Номенклатура = запис.Номенклатура;
                    record.ХарактеристикаНоменклатури = запис.Характеристика;
                    record.Серія = запис.Серія;
                    record.КількістьУпаковок = запис.КількістьУпаковок;
                    record.Пакування = запис.Пакування;
                    record.Кількість = запис.Кількість;
                    record.Комірка = запис.Комірка;

                    ЗбіркаТоварівНаСкладі_Objest.Товари_TablePart.Records.Add(record);
                }

                ЗбіркаТоварівНаСкладі_Objest.Товари_TablePart.Save(true);
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

            //КоміркаНазва
            {
                TreeViewColumn КоміркаНазва = new TreeViewColumn("Комірка", new CellRendererText(), "text", (int)Columns.КоміркаНазва) { MinWidth = 100 };
                КоміркаНазва.Data.Add("Column", Columns.КоміркаНазва);

                TreeViewGrid.AppendColumn(КоміркаНазва);
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
                            }

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

        #region ОбробкаТабЧастини Товари

        void РозприділитиПоКоміркахВідповідноДоЗалишків(object? sender, EventArgs args)
        {
            List<Запис> НовіЗаписи = new List<Запис>();

            int sequenceNumber = 0;

            foreach (Запис запис in Записи)
            {
                decimal КількістьЯкуПотрібноРозприділити = запис.Кількість;

                string query = @$"
SELECT
    ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Комірка} AS Комірка,
    SUM(ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.ВНаявності}) AS ВНаявності
FROM
    {ТовариВКомірках_Залишки_TablePart.TABLE} AS ТовариВКомірках
WHERE
    ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Номенклатура} = @Номенклатура AND
    ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.ХарактеристикаНоменклатури} = @ХарактеристикаНоменклатури AND
    ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Пакування} = @Пакування AND
    ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Серія} = @Серія
GROUP BY
    Комірка
HAVING
    SUM(ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.ВНаявності}) > 0 
";

                Dictionary<string, object> paramQuery = new Dictionary<string, object>();
                paramQuery.Add("Номенклатура", запис.Номенклатура.UnigueID.UGuid);
                paramQuery.Add("ХарактеристикаНоменклатури", запис.Характеристика.UnigueID.UGuid);
                paramQuery.Add("Пакування", запис.Пакування.UnigueID.UGuid);
                paramQuery.Add("Серія", запис.Серія.UnigueID.UGuid);

                bool єЗміниВЗаписі = false;

                string[] columnsName;
                List<Dictionary<string, object>> listRow;

                Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

                foreach (Dictionary<string, object> row in listRow)
                {
                    decimal ЗалишокВКомірці = (decimal)row["ВНаявності"];
                    СкладськіКомірки_Pointer складськіКомірки_Pointer = new СкладськіКомірки_Pointer(row["Комірка"]);

                    if (ЗалишокВКомірці >= КількістьЯкуПотрібноРозприділити)
                    {
                        запис.Кількість = КількістьЯкуПотрібноРозприділити;
                        запис.Комірка = складськіКомірки_Pointer;

                        Запис.ПісляЗміни_Комірка(запис);

                        єЗміниВЗаписі = true;

                        break;
                    }
                    else
                    {
                        Запис записНовий = Запис.Clone(запис);
                        НовіЗаписи.Add(записНовий);

                        записНовий.Кількість = ЗалишокВКомірці;
                        записНовий.Комірка = складськіКомірки_Pointer;

                        Запис.ПісляЗміни_Комірка(записНовий);

                        КількістьЯкуПотрібноРозприділити -= ЗалишокВКомірці;
                    }
                }

                //Залишок який невдалось розприділити
                if (запис.Кількість != КількістьЯкуПотрібноРозприділити && КількістьЯкуПотрібноРозприділити > 0)
                {
                    запис.Кількість = КількістьЯкуПотрібноРозприділити;
                    запис.Комірка = new СкладськіКомірки_Pointer();

                    Запис.ПісляЗміни_Комірка(запис);

                    єЗміниВЗаписі = true;
                }

                //Відобразити зміни в записі
                if (єЗміниВЗаписі)
                {
                    TreeIter iter;
                    Store.GetIterFromString(out iter, sequenceNumber.ToString());
                    Store.SetValues(iter, запис.ToArray());
                }

                sequenceNumber++;
            }

            //Додати нові рядки в таблицю
            foreach (Запис запис in НовіЗаписи)
            {
                Записи.Add(запис);
                Store.AppendValues(запис.ToArray());
            }
        }

        #endregion

    }
}