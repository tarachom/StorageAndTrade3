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

using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class Склади_ТабличнаЧастина_Контакти : VBox
    {
        public Склади_Objest? Склади_Objest { get; set; }

        enum Columns
        {
            UID,
            Тип,
            Значення,
            Телефон,
            ЕлектроннаПошта,
            Країна,
            Область,
            Район,
            Місто
        }

        ListStore Store = new ListStore(
            typeof(string), //UID
            typeof(string), //Тип
            typeof(string), //Значення
            typeof(string), //Телефон
            typeof(string), //ЕлектроннаПошта
            typeof(string), //Країна
            typeof(string), //Область
            typeof(string), //Район
            typeof(string)  //Місто
        );

        TreeView TreeViewGrid;

        public Склади_ТабличнаЧастина_Контакти() : base()
        {
            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In, HeightRequest = 300 };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeViewGrid = new TreeView(Store);
            AddColumn();

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;

            scrollTree.Add(TreeViewGrid);

            PackStart(scrollTree, true, false, 0);

            ShowAll();
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

            if (Склади_Objest != null)
            {
                Склади_Objest.Контакти_TablePart.Read();

                foreach (Склади_Контакти_TablePart.Record record in Склади_Objest.Контакти_TablePart.Records)
                {
                    Store.AppendValues(
                        record.UID.ToString(),
                        ПсевдонімиПерелічення.ТипиКонтактноїІнформації_Alias(record.Тип),
                        record.Значення,
                        record.Телефон,
                        record.ЕлектроннаПошта,
                        record.Країна,
                        record.Область,
                        record.Район,
                        record.Місто
                    );
                }
            }
        }

        public void SaveRecords()
        {
            if (Склади_Objest != null)
            {
                Склади_Objest.Контакти_TablePart.Records.Clear();

                TreeIter iter;
                if (Store.GetIterFirst(out iter))
                    do
                    {
                        Склади_Контакти_TablePart.Record record = new Склади_Контакти_TablePart.Record();
                        Склади_Objest.Контакти_TablePart.Records.Add(record);

                        string uid = (string)Store.GetValue(iter, (int)Columns.UID);

                        if (!string.IsNullOrEmpty(uid))
                            record.UID = Guid.Parse(uid);

                        string type = (string)Store.GetValue(iter, (int)Columns.Тип);

                        //Тип
                        {
                            ТипиКонтактноїІнформації? result = ПсевдонімиПерелічення.ТипиКонтактноїІнформації_FindByName(type);
                            record.Тип = result != null ? (ТипиКонтактноїІнформації)result : ТипиКонтактноїІнформації.Адрес;
                        }

                        record.Значення = Store.GetValue(iter, (int)Columns.Значення)?.ToString() ?? "";
                        record.Телефон = Store.GetValue(iter, (int)Columns.Телефон)?.ToString() ?? "";
                        record.ЕлектроннаПошта = Store.GetValue(iter, (int)Columns.ЕлектроннаПошта)?.ToString() ?? "";
                        record.Країна = Store.GetValue(iter, (int)Columns.Країна)?.ToString() ?? "";
                        record.Область = Store.GetValue(iter, (int)Columns.Область)?.ToString() ?? "";
                        record.Район = Store.GetValue(iter, (int)Columns.Район)?.ToString() ?? "";
                        record.Місто = Store.GetValue(iter, (int)Columns.Місто)?.ToString() ?? "";
                    }
                    while (Store.IterNext(ref iter));

                Склади_Objest.Контакти_TablePart.Save(true);
            }
        }

        #region TreeView

        void AddColumn()
        {
            TreeViewGrid.AppendColumn(new TreeViewColumn("UID", new CellRendererText(), "text", (int)Columns.UID) { Visible = false });

            //Тип
            {
                ListStore storeTypeInfo = new ListStore(typeof(string), typeof(string));

                foreach (var field in ПсевдонімиПерелічення.ТипиКонтактноїІнформації_List())
                    storeTypeInfo.AppendValues(field.Value.ToString(), field.Name);

                CellRendererCombo TypeInfo = new CellRendererCombo() { Editable = true, Model = storeTypeInfo, TextColumn = 1 };
                TypeInfo.Edited += TextChanged;
                TypeInfo.Data.Add("Column", (int)Columns.Тип);

                TreeViewGrid.AppendColumn(new TreeViewColumn("Тип", TypeInfo, "text", (int)Columns.Тип) { MinWidth = 100, Resizable = true });
            }

            //Значення
            CellRendererText Значення = new CellRendererText() { Editable = true };
            Значення.Edited += TextChanged;
            Значення.Data.Add("Column", (int)Columns.Значення);

            TreeViewGrid.AppendColumn(new TreeViewColumn("Значення", Значення, "text", (int)Columns.Значення) { MinWidth = 200, Resizable = true });

            //Телефон
            CellRendererText Телефон = new CellRendererText() { Editable = true };
            Телефон.Edited += TextChanged;
            Телефон.Data.Add("Column", (int)Columns.Телефон);

            TreeViewGrid.AppendColumn(new TreeViewColumn("Телефон", Телефон, "text", (int)Columns.Телефон) { MinWidth = 150, Resizable = true });

            //Email
            CellRendererText Email = new CellRendererText() { Editable = true };
            Email.Edited += TextChanged;
            Email.Data.Add("Column", (int)Columns.ЕлектроннаПошта);

            TreeViewGrid.AppendColumn(new TreeViewColumn("Email", Email, "text", (int)Columns.ЕлектроннаПошта) { MinWidth = 150, Resizable = true });

            //Країна
            CellRendererText Країна = new CellRendererText() { Editable = true };
            Країна.Edited += TextChanged;
            Країна.Data.Add("Column", (int)Columns.Країна);

            TreeViewGrid.AppendColumn(new TreeViewColumn("Країна", Країна, "text", (int)Columns.Країна) { MinWidth = 150, Resizable = true });

            //Область
            CellRendererText Область = new CellRendererText() { Editable = true };
            Область.Edited += TextChanged;
            Область.Data.Add("Column", (int)Columns.Область);

            TreeViewGrid.AppendColumn(new TreeViewColumn("Область", Область, "text", (int)Columns.Область) { MinWidth = 150, Resizable = true });

            //Район
            CellRendererText Район = new CellRendererText() { Editable = true };
            Район.Edited += TextChanged;
            Район.Data.Add("Column", (int)Columns.Район);

            TreeViewGrid.AppendColumn(new TreeViewColumn("Район", Район, "text", (int)Columns.Район) { MinWidth = 150, Resizable = true });

            //Місто
            CellRendererText Місто = new CellRendererText() { Editable = true };
            Місто.Edited += TextChanged;
            Місто.Data.Add("Column", (int)Columns.Місто);

            TreeViewGrid.AppendColumn(new TreeViewColumn("Місто", Місто, "text", (int)Columns.Місто) { MinWidth = 150, Resizable = true });
        }

        void TextChanged(object sender, EditedArgs args)
        {
            CellRenderer cellRender = (CellRenderer)sender;

            if (cellRender.Data.Contains("Column"))
            {
                int ColumnNum = (int)cellRender.Data["Column"]!;

                TreeIter iter;
                if (Store.GetIterFromString(out iter, args.Path))
                    Store.SetValue(iter, ColumnNum, args.NewText);
            }
        }

        #endregion

        #region ToolBar

        void OnAddClick(object? sender, EventArgs args)
        {
            Store.AppendValues("", ТипиКонтактноїІнформації.Адрес.ToString());
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

                    Store.Remove(ref iter);
                }
            }
        }

        #endregion

    }
}