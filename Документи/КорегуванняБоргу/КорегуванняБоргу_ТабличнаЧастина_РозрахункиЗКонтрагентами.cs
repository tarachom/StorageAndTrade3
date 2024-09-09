/*
Copyright (C) 2019-2024 TARAKHOMYN YURIY IVANOVYCH
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
using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class КорегуванняБоргу_ТабличнаЧастина_РозрахункиЗКонтрагентами : ДокументТабличнаЧастина
    {
        public КорегуванняБоргу_Objest? ЕлементВласник { get; set; }

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

        List<Запис> Записи = [];

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
                return
                [
                    НомерРядка,
                    ПсевдонімиПерелічення.ТипиКонтрагентів_Alias(ТипКонтрагента),
                    Контрагент.Назва,
                    Валюта.Назва,
                    (float)Сума
                ];
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    Контрагент = запис.Контрагент.Copy(),
                    Валюта = запис.Валюта.Copy(),
                    Сума = запис.Сума,
                    ТипКонтрагента = запис.ТипКонтрагента
                };
            }

            public static async ValueTask ПісляЗміни_Контрагент(Запис запис)
            {
                await запис.Контрагент.GetPresentation();
            }
            public static async ValueTask ПісляЗміни_Валюта(Запис запис)
            {
                await запис.Валюта.GetPresentation();
            }
        }

        #endregion

        Label ПідсумокСума = new Label() { Selectable = true };

        public КорегуванняБоргу_ТабличнаЧастина_РозрахункиЗКонтрагентами() 
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
            Box hBox = new Box(Orientation.Horizontal, 0) { Halign = Align.Start };
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

            if (ЕлементВласник != null)
            {
                ЕлементВласник.РозрахункиЗКонтрагентами_TablePart.FillJoin([КорегуванняБоргу_РозрахункиЗКонтрагентами_TablePart.НомерРядка]);
                await ЕлементВласник.РозрахункиЗКонтрагентами_TablePart.Read();

                foreach (КорегуванняБоргу_РозрахункиЗКонтрагентами_TablePart.Record record in ЕлементВласник.РозрахункиЗКонтрагентами_TablePart.Records)
                {
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

        public override async ValueTask SaveRecords()
        {
            if (ЕлементВласник != null)
            {
                ЕлементВласник.РозрахункиЗКонтрагентами_TablePart.Records.Clear();

                int sequenceNumber = 0;

                foreach (Запис запис in Записи)
                {
                    КорегуванняБоргу_РозрахункиЗКонтрагентами_TablePart.Record record = new КорегуванняБоргу_РозрахункиЗКонтрагентами_TablePart.Record()
                    {
                        UID = запис.ID,
                        НомерРядка = ++sequenceNumber,
                        ТипКонтрагента = запис.ТипКонтрагента,
                        Контрагент = запис.Контрагент,
                        Валюта = запис.Валюта,
                        Сума = запис.Сума
                    };

                    ЕлементВласник.РозрахункиЗКонтрагентами_TablePart.Records.Add(record);
                }

                await ЕлементВласник.РозрахункиЗКонтрагентами_TablePart.Save(true);

                await LoadRecords();
            }
        }

        public string КлючовіСловаДляПошуку()
        {
            string ключовіСлова = "";

            if (ЕлементВласник != null)
            {
                int sequenceNumber = 0;
                foreach (Запис запис in Записи)
                    ключовіСлова += $"\n{++sequenceNumber}. {запис.ТипКонтрагента} {запис.Контрагент.Назва} {запис.Валюта.Назва}";
            }

            return ключовіСлова;
        }

        #region TreeView

        void AddColumn()
        {
            //НомерРядка
            TreeViewGrid.AppendColumn(new TreeViewColumn("№", new CellRendererText(), "text", (int)Columns.НомерРядка) { Resizable = true, MinWidth = 30 });

            //ТипКонтрагента
            {
                ListStore storeTypeContragent = new ListStore(typeof(string), typeof(string));

                foreach (NameValue<ТипиКонтрагентів> field in ПсевдонімиПерелічення.ТипиКонтрагентів_List())
                    storeTypeContragent.AppendValues(field.Value.ToString(), field.Name);

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

        protected override async void ButtonSelect(TreeIter iter, int rowNumber, int colNumber, Popover popoverSmallSelect)
        {
            Запис запис = Записи[rowNumber];

            switch ((Columns)colNumber)
            {
                case Columns.Контрагент:
                    {
                        Контрагенти_ШвидкийВибір page = new Контрагенти_ШвидкийВибір() { PopoverParent = popoverSmallSelect, DirectoryPointerItem = запис.Контрагент.UnigueID };
                        page.CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                        {
                            запис.Контрагент = new Контрагенти_Pointer(selectPointer);
                            await Запис.ПісляЗміни_Контрагент(запис);

                            Store.SetValues(iter, запис.ToArray());
                        };

                        popoverSmallSelect.Add(page);
                        popoverSmallSelect.ShowAll();

                        await page.SetValue();
                        break;
                    }
                case Columns.Валюта:
                    {
                        Валюти_ШвидкийВибір page = new Валюти_ШвидкийВибір() { PopoverParent = popoverSmallSelect, DirectoryPointerItem = запис.Валюта.UnigueID };
                        page.CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                        {
                            запис.Валюта = new Валюти_Pointer(selectPointer);
                            await Запис.ПісляЗміни_Валюта(запис);

                            Store.SetValues(iter, запис.ToArray());
                        };

                        popoverSmallSelect.Add(page);
                        popoverSmallSelect.ShowAll();

                        await page.SetValue();
                        break;
                    }
            }
        }

        protected override void ButtonPopupClear(TreeIter iter, int rowNumber, int colNumber)
        {
            Запис запис = Записи[rowNumber];

            switch ((Columns)colNumber)
            {
                case Columns.Контрагент:
                    {
                        запис.Контрагент.Clear();
                        break;
                    }
                case Columns.Валюта:
                    {
                        запис.Валюта.Clear();
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

                Store.GetIterFromString(out TreeIter iter, args.Path);

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
                    case Columns.ТипКонтрагента:
                        {
                            ТипиКонтрагентів? result = ПсевдонімиПерелічення.ТипиКонтрагентів_FindByName(args.NewText);
                            if (result != null)
                                запис.ТипКонтрагента = (ТипиКонтрагентів)result;

                            break;
                        }
                }

                Store.SetValues(iter, запис.ToArray());
            }
        }

        #endregion

        #region ToolBar

        protected override async void AddRecord()
        {
            Запис запис = new Запис();
            Записи.Add(запис);

            запис.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
            await Запис.ПісляЗміни_Валюта(запис);

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