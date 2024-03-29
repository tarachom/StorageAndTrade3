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
    class ВведенняЗалишків_ТабличнаЧастина_Каси : ДокументТабличнаЧастина
    {
        public ВведенняЗалишків_Objest? ВведенняЗалишків_Objest { get; set; }

        #region Записи

        enum Columns
        {
            НомерРядка,
            Каса,
            Сума
        }

        ListStore Store = new ListStore(
            typeof(int),      //НомерРядка
            typeof(string),   //Каса
            typeof(float)     //Сума
        );

        List<Запис> Записи = new List<Запис>();

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            public int НомерРядка { get; set; }
            public Каси_Pointer Каса { get; set; } = new Каси_Pointer();
            public decimal Сума { get; set; }

            public object[] ToArray()
            {
                return
                [
                    НомерРядка,
                    Каса.Назва,
                    (float)Сума
                ];
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    Каса = запис.Каса.Copy(),
                    Сума = запис.Сума
                };
            }

            public static async ValueTask ПісляЗміни_Каса(Запис запис)
            {
                await запис.Каса.GetPresentation();
            }
        }

        #endregion

        Label ПідсумокСума = new Label() { Selectable = true };

        public ВведенняЗалишків_ТабличнаЧастина_Каси() : base()
        {
            TreeViewGrid.Model = Store;
            AddColumn();

            CreateBottomBlock();

            Store.RowChanged += (object? sender, RowChangedArgs args) => { ОбчислитиПідсумки(); };
            Store.RowDeleted += (object? sender, RowDeletedArgs args) => { ОбчислитиПідсумки(); };
        }

        #region Підсумки

        void CreateBottomBlock()
        {
            HBox hBox = new HBox() { Halign = Align.Start };
            hBox.PackStart(new Label("<b>Підсумки</b> ") { UseMarkup = true }, false, false, 2);
            hBox.PackStart(ПідсумокСума, false, false, 2);

            base.PackStart(hBox, false, false, 2);
        }

        void ОбчислитиПідсумки()
        {
            decimal Сума = 0;

            foreach (Запис запис in Записи)
                Сума += запис.Сума;

            ПідсумокСума.Text = $"Сума: <b>{Сума}</b>";
            ПідсумокСума.UseMarkup = true;
        }

        #endregion

        public override async ValueTask LoadRecords()
        {
            Store.Clear();
            Записи.Clear();

            if (ВведенняЗалишків_Objest != null)
            {
                Query querySelect = ВведенняЗалишків_Objest.Каси_TablePart.QuerySelect;
                querySelect.Clear();

                //JOIN Каса
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(Каси_Const.TABLE + "." + Каси_Const.Назва, "Каса"));
                querySelect.Joins.Add(
                    new Join(Каси_Const.TABLE, ВведенняЗалишків_Каси_TablePart.Каса, querySelect.Table));

                //ORDER
                querySelect.Order.Add(ВведенняЗалишків_Каси_TablePart.НомерРядка, SelectOrder.ASC);

                await ВведенняЗалишків_Objest.Каси_TablePart.Read();

                Dictionary<string, Dictionary<string, string>> JoinValue = ВведенняЗалишків_Objest.Каси_TablePart.JoinValue;

                foreach (ВведенняЗалишків_Каси_TablePart.Record record in ВведенняЗалишків_Objest.Каси_TablePart.Records)
                {
                    string uid = record.UID.ToString();

                    record.Каса.Назва = JoinValue[uid]["Каса"];

                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        НомерРядка = record.НомерРядка,
                        Каса = record.Каса,
                        Сума = record.Сума
                    };

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }
        }

        public override async ValueTask SaveRecords()
        {
            if (ВведенняЗалишків_Objest != null)
            {
                ВведенняЗалишків_Objest.Каси_TablePart.Records.Clear();

                int sequenceNumber = 0;

                foreach (Запис запис in Записи)
                {
                    ВведенняЗалишків_Каси_TablePart.Record record = new ВведенняЗалишків_Каси_TablePart.Record()
                    {
                        UID = запис.ID,
                        НомерРядка = ++sequenceNumber,
                        Каса = запис.Каса,
                        Сума = запис.Сума
                    };

                    ВведенняЗалишків_Objest.Каси_TablePart.Records.Add(record);
                }

                await ВведенняЗалишків_Objest.Каси_TablePart.Save(true);

                await LoadRecords();
            }
        }

        public string КлючовіСловаДляПошуку()
        {
            string ключовіСлова = "";

            if (ВведенняЗалишків_Objest != null)
            {
                int sequenceNumber = 0;
                foreach (Запис запис in Записи)
                    ключовіСлова += $"\n{++sequenceNumber}. {запис.Каса.Назва}";
            }

            return ключовіСлова;
        }

        #region TreeView

        void AddColumn()
        {
            //НомерРядка
            TreeViewGrid.AppendColumn(new TreeViewColumn("№", new CellRendererText(), "text", (int)Columns.НомерРядка) { Resizable = true, MinWidth = 30 });

            //Каса
            {
                TreeViewColumn Каса = new TreeViewColumn("Каса", new CellRendererText(), "text", (int)Columns.Каса) { Resizable = true, MinWidth = 200 };
                Каса.Data.Add("Column", Columns.Каса);

                TreeViewGrid.AppendColumn(Каса);
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

        protected override async void ButtonSelect(TreeIter iter, int rowNumber, int colNumber, Popover popoverSmallSelect)
        {
            Запис запис = Записи[rowNumber];

            switch ((Columns)colNumber)
            {
                case Columns.Каса:
                    {
                        Каси_ШвидкийВибір page = new Каси_ШвидкийВибір() { PopoverParent = popoverSmallSelect, DirectoryPointerItem = запис.Каса.UnigueID };
                        page.CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                        {
                            запис.Каса = new Каси_Pointer(selectPointer);
                            await Запис.ПісляЗміни_Каса(запис);

                            Store.SetValues(iter, запис.ToArray());
                        };

                        popoverSmallSelect.Add(page);
                        popoverSmallSelect.ShowAll();

                        await page.LoadRecords();
                        break;
                    }
            }
        }

        protected override void ButtonPopupClear(TreeIter iter, int rowNumber, int colNumber)
        {
            Запис запис = Записи[rowNumber];

            switch ((Columns)colNumber)
            {
                case Columns.Каса:
                    {
                        запис.Каса.Clear();
                        break;
                    }
            }

            Store.SetValues(iter, запис.ToArray());
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

                switch ((Columns)ColumnNum)
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