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

using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ВведенняЗалишків_ТабличнаЧастина_БанківськіРахунки : VBox
    {
        ScrolledWindow scrollTree;
        public ВведенняЗалишків_Objest? ВведенняЗалишків_Objest { get; set; }

        #region Записи

        enum Columns
        {
            НомерРядка,
            БанківськийРахунок,
            Сума
        }

        ListStore Store = new ListStore(
            typeof(int),      //НомерРядка
            typeof(string),   //БанківськийРахунок
            typeof(float)     //Сума
        );

        List<Запис> Записи = new List<Запис>();

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            public int НомерРядка { get; set; }
            public БанківськіРахункиОрганізацій_Pointer БанківськийРахунок { get; set; } = new БанківськіРахункиОрганізацій_Pointer();
            public decimal Сума { get; set; }

            public object[] ToArray()
            {
                return new object[]
                {
                    НомерРядка,
                    БанківськийРахунок.Назва,
                    (float)Сума
                };
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    БанківськийРахунок = запис.БанківськийРахунок,
                    Сума = запис.Сума
                };
            }

            public static void ПісляЗміни_БанківськийРахунок(Запис запис)
            {
                запис.БанківськийРахунок.GetPresentation();
            }
        }

        #endregion

        TreeView TreeViewGrid;

        public ВведенняЗалишків_ТабличнаЧастина_БанківськіРахунки() : base()
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
                        case Columns.БанківськийРахунок:
                            {
                                БанківськіРахункиОрганізацій_ШвидкийВибір page = new БанківськіРахункиОрганізацій_ШвидкийВибір() { PopoverParent = PopoverSmallSelect, DirectoryPointerItem = запис.БанківськийРахунок };
                                page.CallBack_OnSelectPointer = (БанківськіРахункиОрганізацій_Pointer selectPointer) =>
                                {
                                    запис.БанківськийРахунок = selectPointer;
                                    Запис.ПісляЗміни_БанківськийРахунок(запис);

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

            if (ВведенняЗалишків_Objest != null)
            {
                Query querySelect = ВведенняЗалишків_Objest.БанківськіРахунки_TablePart.QuerySelect;
                querySelect.Clear();

                //JOIN БанківськійРахунок
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(БанківськіРахункиОрганізацій_Const.TABLE + "." + БанківськіРахункиОрганізацій_Const.Назва, "БанківськійРахунок"));
                querySelect.Joins.Add(
                    new Join(БанківськіРахункиОрганізацій_Const.TABLE, ВведенняЗалишків_БанківськіРахунки_TablePart.БанківськийРахунок, querySelect.Table));

                //ORDER
                querySelect.Order.Add(ВведенняЗалишків_БанківськіРахунки_TablePart.НомерРядка, SelectOrder.ASC);

                ВведенняЗалишків_Objest.БанківськіРахунки_TablePart.Read();

                Dictionary<string, Dictionary<string, string>> JoinValue = ВведенняЗалишків_Objest.БанківськіРахунки_TablePart.JoinValue;

                foreach (ВведенняЗалишків_БанківськіРахунки_TablePart.Record record in ВведенняЗалишків_Objest.БанківськіРахунки_TablePart.Records)
                {
                    string uid = record.UID.ToString();

                    record.БанківськийРахунок.Назва = JoinValue[uid]["БанківськійРахунок"];

                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        НомерРядка = record.НомерРядка,
                        БанківськийРахунок = record.БанківськийРахунок,
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
                ВведенняЗалишків_Objest.БанківськіРахунки_TablePart.Records.Clear();

                int sequenceNumber = 0;

                foreach (Запис запис in Записи)
                {
                    ВведенняЗалишків_БанківськіРахунки_TablePart.Record record = new ВведенняЗалишків_БанківськіРахунки_TablePart.Record();

                    record.UID = запис.ID;
                    record.НомерРядка = ++sequenceNumber;
                    record.БанківськийРахунок = запис.БанківськийРахунок;
                    record.Сума = запис.Сума;

                    ВведенняЗалишків_Objest.БанківськіРахунки_TablePart.Records.Add(record);
                }

                ВведенняЗалишків_Objest.БанківськіРахунки_TablePart.Save(true);
            }
        }

        public string КлючовіСловаДляПошуку()
        {
            string ключовіСлова = "";

            if (ВведенняЗалишків_Objest != null)
            {
                int sequenceNumber = 0;
                foreach (Запис запис in Записи)
                    ключовіСлова += $"\n{++sequenceNumber}. {запис.БанківськийРахунок.Назва}";
            }

            return ключовіСлова;
        }

        #region TreeView

        void AddColumn()
        {
            //НомерРядка
            TreeViewGrid.AppendColumn(new TreeViewColumn("№", new CellRendererText(), "text", (int)Columns.НомерРядка) { Resizable = true, MinWidth =  30 });

            //БанківськийРахунок
            {
                TreeViewColumn БанківськийРахунок = new TreeViewColumn("Банківський рахунок", new CellRendererText(), "text", (int)Columns.БанківськийРахунок) { Resizable = true, MinWidth =  200 };
                БанківськийРахунок.Data.Add("Column", Columns.БанківськийРахунок);

                TreeViewGrid.AppendColumn(БанківськийРахунок);
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