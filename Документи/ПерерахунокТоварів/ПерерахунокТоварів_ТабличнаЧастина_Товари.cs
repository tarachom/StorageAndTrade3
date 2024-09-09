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

using Константи = StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ПерерахунокТоварів_ТабличнаЧастина_Товари : ДокументТабличнаЧастина
    {
        public ПерерахунокТоварів_Objest? ЕлементВласник { get; set; }

        #region Записи

        enum Columns
        {
            НомерРядка,
            Номенклатура,
            Характеристика,
            КількістьУпаковок,
            КількістьУпаковокФакт,
            Пакування,
            Кількість,
            КількістьФакт,
            Коментар
        }

        ListStore Store = new ListStore(
            typeof(int),      //НомерРядка
            typeof(string),   //Номенклатура
            typeof(string),   //Характеристика
            typeof(int),      //КількістьУпаковок
            typeof(int),      //КількістьУпаковокФакт
            typeof(string),   //Пакування
            typeof(float),    //Кількість
            typeof(float),    //КількістьФакт
            typeof(string)    //Коментар
        );

        List<Запис> Записи = new List<Запис>();

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            public int НомерРядка { get; set; }
            public Номенклатура_Pointer Номенклатура { get; set; } = new Номенклатура_Pointer();
            public ХарактеристикиНоменклатури_Pointer Характеристика { get; set; } = new ХарактеристикиНоменклатури_Pointer();
            public int КількістьУпаковок { get; set; } = 1;
            public int КількістьУпаковокФакт { get; set; } = 1;
            public ПакуванняОдиниціВиміру_Pointer Пакування { get; set; } = new ПакуванняОдиниціВиміру_Pointer();
            public decimal Кількість { get; set; } = 1;
            public decimal КількістьФакт { get; set; } = 1;
            public string Коментар { get; set; } = "";

            public object[] ToArray()
            {
                return
                [
                    НомерРядка,
                    Номенклатура.Назва,
                    Характеристика.Назва,
                    КількістьУпаковок,
                    КількістьУпаковокФакт,
                    Пакування.Назва,
                    (float)Кількість,
                    (float)КількістьФакт,
                    Коментар
                ];
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    Номенклатура = запис.Номенклатура.Copy(),
                    Характеристика = запис.Характеристика.Copy(),
                    КількістьУпаковок = запис.КількістьУпаковок,
                    КількістьУпаковокФакт = запис.КількістьУпаковокФакт,
                    Пакування = запис.Пакування.Copy(),
                    Кількість = запис.Кількість,
                    КількістьФакт = запис.КількістьФакт,
                    Коментар = запис.Коментар
                };
            }

            public static async ValueTask ПісляЗміни_Номенклатура(Запис запис)
            {
                await запис.Номенклатура.GetPresentation();

                Номенклатура_Objest? номенклатура_Objest = await запис.Номенклатура.GetDirectoryObject();
                if (номенклатура_Objest != null && !номенклатура_Objest.ОдиницяВиміру.IsEmpty())
                {
                    запис.Пакування = номенклатура_Objest.ОдиницяВиміру;
                    await Запис.ПісляЗміни_Пакування(запис);
                }

                if (!запис.Пакування.IsEmpty())
                {
                    ПакуванняОдиниціВиміру_Objest? пакуванняОдиниціВиміру_Objest = await запис.Пакування.GetDirectoryObject();
                    if (пакуванняОдиниціВиміру_Objest != null)
                        запис.КількістьУпаковок = пакуванняОдиниціВиміру_Objest.КількістьУпаковок;
                    else
                        запис.КількістьУпаковок = 1;
                }
            }
            public static async ValueTask ПісляЗміни_Характеристика(Запис запис)
            {
                await запис.Характеристика.GetPresentation();
            }
            public static async ValueTask ПісляЗміни_Пакування(Запис запис)
            {
                await запис.Пакування.GetPresentation();
            }
        }

        #endregion

        Label ПідсумокСума = new Label() { Selectable = true };

        public ПерерахунокТоварів_ТабличнаЧастина_Товари() 
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

            // foreach (Запис запис in Записи)
            //     Сума += запис.Сума;

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
                ЕлементВласник.Товари_TablePart.FillJoin([ПерерахунокТоварів_Товари_TablePart.НомерРядка]);
                await ЕлементВласник.Товари_TablePart.Read();

                foreach (ПерерахунокТоварів_Товари_TablePart.Record record in ЕлементВласник.Товари_TablePart.Records)
                {
                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        НомерРядка = record.НомерРядка,
                        Номенклатура = record.Номенклатура,
                        Характеристика = record.ХарактеристикаНоменклатури,
                        КількістьУпаковок = record.КількістьУпаковок,
                        КількістьУпаковокФакт = record.КількістьУпаковокФакт,
                        Пакування = record.Пакування,
                        Кількість = record.Кількість,
                        КількістьФакт = record.КількістьФакт,
                        Коментар = record.Коментар
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
                ЕлементВласник.Товари_TablePart.Records.Clear();

                int sequenceNumber = 0;

                foreach (Запис запис in Записи)
                {
                    ПерерахунокТоварів_Товари_TablePart.Record record = new ПерерахунокТоварів_Товари_TablePart.Record
                    {
                        UID = запис.ID,
                        НомерРядка = ++sequenceNumber,
                        Номенклатура = запис.Номенклатура,
                        ХарактеристикаНоменклатури = запис.Характеристика,
                        КількістьУпаковок = запис.КількістьУпаковок,
                        КількістьУпаковокФакт = запис.КількістьУпаковокФакт,
                        Пакування = запис.Пакування,
                        Кількість = запис.Кількість,
                        КількістьФакт = запис.КількістьФакт,
                        Коментар = запис.Коментар
                    };

                    ЕлементВласник.Товари_TablePart.Records.Add(record);
                }

                await ЕлементВласник.Товари_TablePart.Save(true);

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
                    ключовіСлова += $"\n{++sequenceNumber}. {запис.Номенклатура.Назва} {запис.Характеристика.Назва} {запис.Коментар}";
            }

            return ключовіСлова;
        }

        public decimal СумаДокументу()
        {
            decimal Сума = 0;

            // foreach (Запис запис in Записи)
            //     Сума += запис.Сума;

            return Math.Round(Сума, 2);
        }

        #region TreeView

        void AddColumn()
        {
            //НомерРядка
            TreeViewGrid.AppendColumn(new TreeViewColumn("№", new CellRendererText(), "text", (int)Columns.НомерРядка) { Resizable = true, MinWidth = 30 });

            //Номенклатура
            {
                TreeViewColumn Номенклатура = new TreeViewColumn("Номенклатура", new CellRendererText(), "text", (int)Columns.Номенклатура) { Resizable = true, MinWidth = 200 };
                Номенклатура.Data.Add("Column", Columns.Номенклатура);

                TreeViewGrid.AppendColumn(Номенклатура);
            }

            //Характеристика
            {
                TreeViewColumn Характеристика = new TreeViewColumn("Характеристика", new CellRendererText(), "text", (int)Columns.Характеристика) { Resizable = true, MinWidth = 200 };
                Характеристика.Visible = Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const;
                Характеристика.Data.Add("Column", Columns.Характеристика);

                TreeViewGrid.AppendColumn(Характеристика);
            }

            //КількістьУпаковок
            {
                CellRendererText КількістьУпаковок = new CellRendererText() { Editable = true };
                КількістьУпаковок.Edited += TextChanged;
                КількістьУпаковок.Data.Add("Column", (int)Columns.КількістьУпаковок);

                TreeViewColumn Column = new TreeViewColumn("Пак", КількістьУпаковок, "text", (int)Columns.КількістьУпаковок) { Resizable = true, MinWidth = 50 };
                Column.SetCellDataFunc(КількістьУпаковок, new TreeCellDataFunc(NumericCellDataFunc));
                TreeViewGrid.AppendColumn(Column);
            }

            //КількістьУпаковокФакт
            {
                CellRendererText КількістьУпаковокФакт = new CellRendererText() { Editable = true };
                КількістьУпаковокФакт.Edited += TextChanged;
                КількістьУпаковокФакт.Data.Add("Column", (int)Columns.КількістьУпаковокФакт);

                TreeViewColumn Column = new TreeViewColumn("Пак факт", КількістьУпаковокФакт, "text", (int)Columns.КількістьУпаковокФакт) { Resizable = true, MinWidth = 50 };
                Column.SetCellDataFunc(КількістьУпаковокФакт, new TreeCellDataFunc(NumericCellDataFunc));
                TreeViewGrid.AppendColumn(Column);
            }

            //Пакування
            {
                TreeViewColumn Пакування = new TreeViewColumn("Пакування", new CellRendererText(), "text", (int)Columns.Пакування) { Resizable = true, MinWidth = 100 };
                Пакування.Data.Add("Column", Columns.Пакування);

                TreeViewGrid.AppendColumn(Пакування);
            }

            //Кількість
            {
                CellRendererText Кількість = new CellRendererText() { Editable = true };
                Кількість.Edited += TextChanged;
                Кількість.Data.Add("Column", (int)Columns.Кількість);

                TreeViewColumn Column = new TreeViewColumn("Кількість", Кількість, "text", (int)Columns.Кількість) { Resizable = true, MinWidth = 100 };
                Column.SetCellDataFunc(Кількість, new TreeCellDataFunc(NumericCellDataFunc));
                TreeViewGrid.AppendColumn(Column);
            }

            //КількістьФакт
            {
                CellRendererText КількістьФакт = new CellRendererText() { Editable = true };
                КількістьФакт.Edited += TextChanged;
                КількістьФакт.Data.Add("Column", (int)Columns.КількістьФакт);

                TreeViewColumn Column = new TreeViewColumn("Кількість факт", КількістьФакт, "text", (int)Columns.Кількість) { Resizable = true, MinWidth = 100 };
                Column.SetCellDataFunc(КількістьФакт, new TreeCellDataFunc(NumericCellDataFunc));
                TreeViewGrid.AppendColumn(Column);
            }

            //Коментар
            {
                CellRendererText Коментар = new CellRendererText() { Editable = true };
                Коментар.Edited += TextChanged;
                Коментар.Data.Add("Column", (int)Columns.Коментар);

                TreeViewColumn Column = new TreeViewColumn("Коментар", Коментар, "text", (int)Columns.Коментар) { Resizable = true, MinWidth = 100 };
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
                case Columns.Номенклатура:
                    {
                        Номенклатура_ШвидкийВибір page = new Номенклатура_ШвидкийВибір() { PopoverParent = popoverSmallSelect, DirectoryPointerItem = запис.Номенклатура.UnigueID };
                        page.CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                        {
                            запис.Номенклатура = new Номенклатура_Pointer(selectPointer);
                            await Запис.ПісляЗміни_Номенклатура(запис);

                            Store.SetValues(iter, запис.ToArray());
                        };

                        popoverSmallSelect.Add(page);
                        popoverSmallSelect.ShowAll();

                        await page.SetValue();
                        break;
                    }
                case Columns.Характеристика:
                    {
                        ХарактеристикиНоменклатури_ШвидкийВибір page = new ХарактеристикиНоменклатури_ШвидкийВибір() { PopoverParent = popoverSmallSelect, DirectoryPointerItem = запис.Характеристика.UnigueID };

                        page.НоменклатураВласник.Pointer = запис.Номенклатура;
                        page.CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                        {
                            запис.Характеристика = new ХарактеристикиНоменклатури_Pointer(selectPointer);
                            await Запис.ПісляЗміни_Характеристика(запис);

                            Store.SetValues(iter, запис.ToArray());
                        };

                        popoverSmallSelect.Add(page);
                        popoverSmallSelect.ShowAll();

                        await page.SetValue();
                        break;
                    }
                case Columns.Пакування:
                    {
                        ПакуванняОдиниціВиміру_ШвидкийВибір page = new ПакуванняОдиниціВиміру_ШвидкийВибір() { PopoverParent = popoverSmallSelect, DirectoryPointerItem = запис.Пакування.UnigueID };
                        page.CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                        {
                            запис.Пакування = new ПакуванняОдиниціВиміру_Pointer(selectPointer);
                            await Запис.ПісляЗміни_Пакування(запис);

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
                case Columns.Номенклатура:
                    {
                        запис.Номенклатура.Clear();
                        break;
                    }
                case Columns.Характеристика:
                    {
                        запис.Характеристика.Clear();
                        break;
                    }
                case Columns.Пакування:
                    {
                        запис.Пакування.Clear();
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
                    case Columns.КількістьУпаковок:
                        {
                            cellText.Text = запис.КількістьУпаковок.ToString();
                            break;
                        }
                    case Columns.КількістьУпаковокФакт:
                        {
                            cellText.Text = запис.КількістьУпаковокФакт.ToString();
                            break;
                        }
                    case Columns.Кількість:
                        {
                            cellText.Text = запис.Кількість.ToString();
                            break;
                        }
                    case Columns.КількістьФакт:
                        {
                            cellText.Text = запис.КількістьФакт.ToString();
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
                    case Columns.КількістьУпаковок:
                        {
                            var (check, value) = Validate.IsInt(args.NewText);
                            if (check)
                                запис.КількістьУпаковок = value;

                            break;
                        }
                    case Columns.КількістьУпаковокФакт:
                        {
                            var (check, value) = Validate.IsInt(args.NewText);
                            if (check)
                                запис.КількістьУпаковокФакт = value;

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
                    case Columns.КількістьФакт:
                        {
                            var (check, value) = Validate.IsDecimal(args.NewText);
                            if (check)
                            {
                                запис.КількістьФакт = value;
                            }

                            break;
                        }
                    case Columns.Коментар:
                        {
                            запис.Коментар = args.NewText;
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