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

namespace StorageAndTrade
{
    class Номенклатура_ТабличнаЧастина_Файли : VBox
    {
        public Номенклатура_Objest? Номенклатура_Objest { get; set; }

        #region Запис

        enum Columns
        {
            Image,
            UID,
            Основний,
            Файл
        }

        ListStore Store = new ListStore(
            typeof(Gdk.Pixbuf), /* Image */
            typeof(string),     //UID
            typeof(bool),       //Основний
            typeof(string)      //ФайлНазва
        );

        List<Запис> Записи = new List<Запис>();

        private class Запис
        {
            public string Image { get; set; } = AppContext.BaseDirectory + "images/doc.png";
            public Guid ID { get; set; } = Guid.Empty;
            public Файли_Pointer Файл { get; set; } = new Файли_Pointer();
            public bool Основний { get; set; } = false;

            public object[] ToArray()
            {
                return new object[]
                {
                    new Gdk.Pixbuf(Image),
                    ID.ToString(),
                    Основний,
                    Файл.Назва
                };
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    Файл = запис.Файл.Copy(),
                    Основний = запис.Основний
                };
            }

            public static void ПісляЗміни_Файл(Запис запис)
            {
                запис.Файл.GetPresentation();
            }
        }

        #endregion

        TreeView TreeViewGrid;

        public Номенклатура_ТабличнаЧастина_Файли() : base()
        {
            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In, HeightRequest = 300 };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeViewGrid = new TreeView(Store);
            AddColumn();

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;
            TreeViewGrid.ButtonPressEvent += OnButtonPressEvent;

            scrollTree.Add(TreeViewGrid);

            PackStart(scrollTree, true, false, 0);

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
                        case Columns.Файл:
                            {
                                Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Файли", () =>
                                {
                                    Файли page = new Файли();

                                    page.DirectoryPointerItem = запис.Файл.UnigueID;
                                    page.CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                                    {
                                        запис.Файл = new Файли_Pointer(selectPointer);
                                        Запис.ПісляЗміни_Файл(запис);
                                        Store.SetValues(iter, запис.ToArray());
                                    };

                                    page.LoadRecords();

                                    return page;
                                }, true);

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

            ToolButton deleteButton = new ToolButton(Stock.Delete) { TooltipText = "Видалити" };
            deleteButton.Clicked += OnDeleteClick;
            toolbar.Add(deleteButton);
        }

        public void LoadRecords()
        {
            Store.Clear();
            Записи.Clear();

            if (Номенклатура_Objest != null)
            {
                Query querySelect = Номенклатура_Objest.Файли_TablePart.QuerySelect;
                querySelect.Clear();

                //JOIN 1
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(Файли_Const.TABLE + "." + Файли_Const.Назва, "file_name"));
                querySelect.Joins.Add(
                    new Join(Файли_Const.TABLE, Номенклатура_Файли_TablePart.Файл, querySelect.Table));

                Номенклатура_Objest.Файли_TablePart.Read();

                Dictionary<string, Dictionary<string, string>> join = Номенклатура_Objest.Файли_TablePart.JoinValue;

                foreach (Номенклатура_Файли_TablePart.Record record in Номенклатура_Objest.Файли_TablePart.Records)
                {
                    record.Файл.Назва = join[record.UID.ToString()]["file_name"];

                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        Файл = record.Файл,
                        Основний = record.Основний
                    };

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }
        }

        public void SaveRecords()
        {
            if (Номенклатура_Objest != null)
            {
                Номенклатура_Objest.Файли_TablePart.Records.Clear();

                foreach (Запис запис in Записи)
                {
                    Номенклатура_Файли_TablePart.Record record = new Номенклатура_Файли_TablePart.Record();
                    Номенклатура_Objest.Файли_TablePart.Records.Add(record);

                    record.UID = запис.ID;
                    record.Файл = запис.Файл;
                    record.Основний = запис.Основний;
                }

                Номенклатура_Objest.Файли_TablePart.Save(true);
            }
        }

        #region TreeView

        void AddColumn()
        {
            TreeViewGrid.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));

            //Основний
            CellRendererToggle Основний = new CellRendererToggle() { };
            Основний.Toggled += Edited;
            Основний.Data.Add("Column", (int)Columns.Основний);

            TreeViewGrid.AppendColumn(new TreeViewColumn("Основний", Основний, "active", (int)Columns.Основний));

            //ФайлНазва
            TreeViewColumn Файл = new TreeViewColumn("Файл", new CellRendererText(), "text", (int)Columns.Файл) { MinWidth = 300, Resizable = true };
            Файл.Data.Add("Column", Columns.Файл);

            TreeViewGrid.AppendColumn(Файл);
        }

        void Edited(object sender, ToggledArgs args)
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
                    case Columns.Основний:
                        {
                            запис.Основний = !(bool)Store.GetValue(iter, (int)Columns.Основний);
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