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
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class ВведенняЗалишків_ТабличнаЧастина_РозрахункиЗКонтрагентами : VBox
    {
        ScrolledWindow scrollTree;
        public ВведенняЗалишків_Objest? ВведенняЗалишків_Objest { get; set; }

        #region Записи

        enum Columns
        {
            НомерРядка,
            ТипКонтрагента,
            Контрагент,
            Валюта,
            Сума
        }

        ListStore Store = new ListStore(
            typeof(int),      //НомерРядка
            typeof(string),   //ТипКонтрагента
            typeof(string),   //Контрагент
            typeof(string),   //Валюта
            typeof(float)     //Сума
        );

        List<Запис> Записи = new List<Запис>();

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            public int НомерРядка { get; set; }
            public Контрагенти_Pointer Контрагент { get; set; } = new Контрагенти_Pointer();
            public Валюти_Pointer Валюта { get; set; } = new Валюти_Pointer();
            public decimal Сума { get; set; }
            public ТипиКонтрагентів ТипКонтрагента { get; set; } = ТипиКонтрагентів.Постачальник;

            public object[] ToArray()
            {
                return new object[]
                {
                    НомерРядка,
                    ТипКонтрагента.ToString(),
                    Контрагент.Назва,
                    Валюта.Назва,
                    (float)Сума
                };
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    Контрагент = запис.Контрагент,
                    Валюта = запис.Валюта,
                    Сума = запис.Сума,
                    ТипКонтрагента = запис.ТипКонтрагента
                };
            }

            public static void ПісляЗміни_Контрагент(Запис запис)
            {
                запис.Контрагент.GetPresentation();
            }
            public static void ПісляЗміни_Валюта(Запис запис)
            {
                запис.Валюта.GetPresentation();
            }
        }

        #endregion

        TreeView TreeViewGrid;
        Label ПідсумокСума = new Label() { Selectable = true };

        public ВведенняЗалишків_ТабличнаЧастина_РозрахункиЗКонтрагентами() : base()
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
            PackStart(scrollTree, true, true, 0);

            CreateBottomBlock();

            Store.RowChanged += (object? sender, RowChangedArgs args) => { ОбчислитиПідсумки(); };
            Store.RowDeleted += (object? sender, RowDeletedArgs args) => { ОбчислитиПідсумки(); };

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
                        case Columns.Контрагент:
                            {
                                Контрагенти_ШвидкийВибір page = new Контрагенти_ШвидкийВибір() { PopoverParent = PopoverSmallSelect, DirectoryPointerItem = запис.Контрагент };
                                page.CallBack_OnSelectPointer = (Контрагенти_Pointer selectPointer) =>
                                {
                                    запис.Контрагент = selectPointer;
                                    Запис.ПісляЗміни_Контрагент(запис);

                                    Store.SetValues(iter, запис.ToArray());
                                };

                                PopoverSmallSelect.Add(page);
                                PopoverSmallSelect.ShowAll();

                                page.LoadRecords();
                                break;
                            }
                        case Columns.Валюта:
                            {
                                Валюти_ШвидкийВибір page = new Валюти_ШвидкийВибір() { PopoverParent = PopoverSmallSelect, DirectoryPointerItem = запис.Валюта };
                                page.CallBack_OnSelectPointer = (Валюти_Pointer selectPointer) =>
                                {
                                    запис.Валюта = selectPointer;
                                    Запис.ПісляЗміни_Валюта(запис);

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

        //Блок для підсумків
        void CreateBottomBlock()
        {
            HBox hBox = new HBox() { Halign = Align.Start };
            hBox.PackStart(new Label("<b>Підсумки</b> ") { UseMarkup = true }, false, false, 2);
            hBox.PackStart(ПідсумокСума, false, false, 2);

            PackStart(hBox, false, false, 2);
        }

        public void LoadRecords()
        {
            Store.Clear();
            Записи.Clear();

            if (ВведенняЗалишків_Objest != null)
            {
                Query querySelect = ВведенняЗалишків_Objest.РозрахункиЗКонтрагентами_TablePart.QuerySelect;
                querySelect.Clear();

                //JOIN Контрагент
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(Контрагенти_Const.TABLE + "." + Контрагенти_Const.Назва, "Контрагент"));
                querySelect.Joins.Add(
                    new Join(Контрагенти_Const.TABLE, ВведенняЗалишків_РозрахункиЗКонтрагентами_TablePart.Контрагент, querySelect.Table));

                //JOIN Валюта
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(Валюти_Const.TABLE + "." + Валюти_Const.Назва, "Валюта"));
                querySelect.Joins.Add(
                    new Join(Валюти_Const.TABLE, ВведенняЗалишків_РозрахункиЗКонтрагентами_TablePart.Валюта, querySelect.Table));

                //ORDER
                querySelect.Order.Add(ВведенняЗалишків_РозрахункиЗКонтрагентами_TablePart.НомерРядка, SelectOrder.ASC);

                ВведенняЗалишків_Objest.РозрахункиЗКонтрагентами_TablePart.Read();

                Dictionary<string, Dictionary<string, string>> JoinValue = ВведенняЗалишків_Objest.РозрахункиЗКонтрагентами_TablePart.JoinValue;

                foreach (ВведенняЗалишків_РозрахункиЗКонтрагентами_TablePart.Record record in ВведенняЗалишків_Objest.РозрахункиЗКонтрагентами_TablePart.Records)
                {
                    string uid = record.UID.ToString();

                    record.Контрагент.Назва = JoinValue[uid]["Контрагент"];
                    record.Валюта.Назва = JoinValue[uid]["Валюта"];

                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        НомерРядка = record.НомерРядка,
                        ТипКонтрагента = (ТипиКонтрагентів)record.ТипКонтрагента,
                        Контрагент = record.Контрагент,
                        Валюта = record.Валюта,
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
                ВведенняЗалишків_Objest.РозрахункиЗКонтрагентами_TablePart.Records.Clear();

                int sequenceNumber = 0;

                foreach (Запис запис in Записи)
                {
                    ВведенняЗалишків_РозрахункиЗКонтрагентами_TablePart.Record record = new ВведенняЗалишків_РозрахункиЗКонтрагентами_TablePart.Record();

                    record.UID = запис.ID;
                    record.НомерРядка = ++sequenceNumber;
                    record.ТипКонтрагента = запис.ТипКонтрагента;
                    record.Контрагент = запис.Контрагент;
                    record.Валюта = запис.Валюта;
                    record.Сума = запис.Сума;

                    ВведенняЗалишків_Objest.РозрахункиЗКонтрагентами_TablePart.Records.Add(record);
                }

                ВведенняЗалишків_Objest.РозрахункиЗКонтрагентами_TablePart.Save(true);
            }
        }

        public string КлючовіСловаДляПошуку()
        {
            string ключовіСлова = "";

            if (ВведенняЗалишків_Objest != null)
            {
                int sequenceNumber = 0;
                foreach (Запис запис in Записи)
                    ключовіСлова += $"\n{++sequenceNumber}. {запис.ТипКонтрагента} {запис.Контрагент.Назва} {запис.Валюта.Назва}";
            }

            return ключовіСлова;
        }

        void ОбчислитиПідсумки()
        {
            decimal Сума = 0;

            foreach (Запис запис in Записи)
                Сума += запис.Сума;

            ПідсумокСума.Text = $"Сума: <b>{Сума}</b>";
            ПідсумокСума.UseMarkup = true;
        }

        #region TreeView

        void AddColumn()
        {
            //НомерРядка
            TreeViewGrid.AppendColumn(new TreeViewColumn("№", new CellRendererText(), "text", (int)Columns.НомерРядка) { Resizable = true, MinWidth = 30 });

            //ТипКонтрагента
            {
                ListStore storeTypeContragent = new ListStore(typeof(string), typeof(string));

                if (Config.Kernel != null)
                {
                    ConfigurationEnums ТипиКонтрагентів = Config.Kernel.Conf.Enums["ТипиКонтрагентів"];

                    foreach (ConfigurationEnumField field in ТипиКонтрагентів.Fields.Values)
                        storeTypeContragent.AppendValues(field.Name, field.Desc);
                }

                CellRendererCombo TypeContragent = new CellRendererCombo() { Editable = true, Model = storeTypeContragent, TextColumn = 1 };
                TypeContragent.Edited += TextChanged;
                TypeContragent.Data.Add("Column", (int)Columns.ТипКонтрагента);

                TreeViewGrid.AppendColumn(new TreeViewColumn("Тип контрагента", TypeContragent, "text", (int)Columns.ТипКонтрагента) { Resizable = true, MinWidth = 100 });
            }

            //Контрагент
            {
                TreeViewColumn Контрагент = new TreeViewColumn("Контрагент", new CellRendererText(), "text", (int)Columns.Контрагент) { Resizable = true, MinWidth = 200 };
                Контрагент.Data.Add("Column", Columns.Контрагент);

                TreeViewGrid.AppendColumn(Контрагент);
            }

            //Валюта
            {
                TreeViewColumn Валюта = new TreeViewColumn("Валюта", new CellRendererText(), "text", (int)Columns.Валюта) { Resizable = true, MinWidth = 200 };
                Валюта.Data.Add("Column", Columns.Валюта);

                TreeViewGrid.AppendColumn(Валюта);
            }

            //Сума
            {
                CellRendererText Сума = new CellRendererText() { Editable = true };
                Сума.Edited += TextChanged;
                Сума.Data.Add("Column", (int)Columns.Сума);

                TreeViewColumn Column = new TreeViewColumn("Сума", Сума, "text", (int)Columns.Сума) { Resizable = true, MinWidth = 100 };
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
                    case Columns.Сума:
                        {
                            var (check, value) = Validate.IsDecimal(args.NewText);
                            if (check)
                                запис.Сума = value;

                            break;
                        }
                    case Columns.ТипКонтрагента:
                        {
                            ТипиКонтрагентів result;
                            if (Enum.TryParse<ТипиКонтрагентів>(args.NewText, true, out result))
                                запис.ТипКонтрагента = result;

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