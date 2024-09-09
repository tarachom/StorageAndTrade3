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

using StorageAndTrade_1_0;
using Константи = StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade
{
    class ЗбіркаТоварівНаСкладі_ТабличнаЧастина_Товари : ДокументТабличнаЧастина
    {
        public ЗбіркаТоварівНаСкладі_Objest? ЕлементВласник { get; set; }
        public System.Action? ОбновитиЗначенняДокумента { get; set; }

        #region Записи

        enum Columns
        {
            НомерРядка,
            Номенклатура,
            Характеристика,
            Серія,
            КількістьУпаковок,
            Пакування,
            Кількість,
            КількістьФакт,
            Комірка
        }

        ListStore Store = new ListStore(
            typeof(int),      //НомерРядка
            typeof(string),   //Номенклатура
            typeof(string),   //Характеристика
            typeof(string),   //Серія
            typeof(int),      //КількістьУпаковок
            typeof(string),   //Пакування
            typeof(float),    //Кількість
            typeof(float),    //КількістьФакт
            typeof(string)    //Комірка
        );

        List<Запис> Записи = new List<Запис>();

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            public int НомерРядка { get; set; }
            public Номенклатура_Pointer Номенклатура { get; set; } = new Номенклатура_Pointer();
            public ХарактеристикиНоменклатури_Pointer Характеристика { get; set; } = new ХарактеристикиНоменклатури_Pointer();
            public СеріїНоменклатури_Pointer Серія { get; set; } = new СеріїНоменклатури_Pointer();
            public int КількістьУпаковок { get; set; } = 1;
            public ПакуванняОдиниціВиміру_Pointer Пакування { get; set; } = new ПакуванняОдиниціВиміру_Pointer();
            public decimal Кількість { get; set; } = 1;
            public decimal КількістьФакт { get; set; } = 1;
            public СкладськіКомірки_Pointer Комірка { get; set; } = new СкладськіКомірки_Pointer();

            public object[] ToArray()
            {
                return
                [
                    НомерРядка,
                    Номенклатура.Назва,
                    Характеристика.Назва,
                    Серія.Назва,
                    КількістьУпаковок,
                    Пакування.Назва,
                    (float)Кількість,
                    (float)КількістьФакт,
                    Комірка.Назва
                ];
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    Номенклатура = запис.Номенклатура.Copy(),
                    Характеристика = запис.Характеристика.Copy(),
                    Серія = запис.Серія.Copy(),
                    КількістьУпаковок = запис.КількістьУпаковок,
                    Пакування = запис.Пакування.Copy(),
                    Кількість = запис.Кількість,
                    КількістьФакт = запис.КількістьФакт,
                    Комірка = запис.Комірка.Copy()
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
            }
            public static async ValueTask ПісляЗміни_Характеристика(Запис запис)
            {
                await запис.Характеристика.GetPresentation();
            }
            public static async ValueTask ПісляЗміни_Серія(Запис запис)
            {
                await запис.Серія.GetPresentation();
            }
            public static async ValueTask ПісляЗміни_Пакування(Запис запис)
            {
                await запис.Пакування.GetPresentation();

                if (!запис.Пакування.IsEmpty())
                {
                    ПакуванняОдиниціВиміру_Objest? пакуванняОдиниціВиміру_Objest = await запис.Пакування.GetDirectoryObject();
                    if (пакуванняОдиниціВиміру_Objest != null)
                        запис.КількістьУпаковок = (пакуванняОдиниціВиміру_Objest.КількістьУпаковок > 0) ? пакуванняОдиниціВиміру_Objest.КількістьУпаковок : 1;
                    else
                        запис.КількістьУпаковок = 1;
                }

                Запис.ПісляЗміни_Кількість(запис);
            }
            public static async ValueTask ПісляЗміни_Комірка(Запис запис)
            {
                await запис.Комірка.GetPresentation();
            }
            public static void ПісляЗміни_Кількість(Запис запис)
            {
                запис.КількістьФакт = запис.Кількість * запис.КількістьУпаковок;
            }
        }

        #endregion

        public ЗбіркаТоварівНаСкладі_ТабличнаЧастина_Товари() 
        {
            TreeViewGrid.Model = Store;
            AddColumn();

            //Separator
            ToolItem toolItemSeparator = new ToolItem
            {
                new Separator(Orientation.Horizontal)
            };
            ToolbarTop.Add(toolItemSeparator);

            ToolButton fillButton = new ToolButton(new Image(Stock.Convert, IconSize.Menu), "Розприділити") { IsImportant = true };
            fillButton.Clicked += РозприділитиПоКоміркахВідповідноДоЗалишків;
            ToolbarTop.Add(fillButton);
        }

        #region Підсумки

        #endregion

        public override async ValueTask LoadRecords()
        {
            Store.Clear();
            Записи.Clear();

            if (ЕлементВласник != null)
            {
                ЕлементВласник.Товари_TablePart.FillJoin([ЗбіркаТоварівНаСкладі_Товари_TablePart.НомерРядка]);
                await ЕлементВласник.Товари_TablePart.Read();

                foreach (ЗбіркаТоварівНаСкладі_Товари_TablePart.Record record in ЕлементВласник.Товари_TablePart.Records)
                {
                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        НомерРядка = record.НомерРядка,
                        Номенклатура = record.Номенклатура,
                        Характеристика = record.ХарактеристикаНоменклатури,
                        Серія = record.Серія,
                        КількістьУпаковок = record.КількістьУпаковок,
                        Пакування = record.Пакування,
                        Кількість = record.Кількість,
                        КількістьФакт = record.Кількість * record.КількістьУпаковок,
                        Комірка = record.Комірка
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
                    ЗбіркаТоварівНаСкладі_Товари_TablePart.Record record = new ЗбіркаТоварівНаСкладі_Товари_TablePart.Record
                    {
                        UID = запис.ID,
                        НомерРядка = ++sequenceNumber,
                        Номенклатура = запис.Номенклатура,
                        ХарактеристикаНоменклатури = запис.Характеристика,
                        Серія = запис.Серія,
                        КількістьУпаковок = запис.КількістьУпаковок,
                        Пакування = запис.Пакування,
                        Кількість = запис.Кількість,
                        Комірка = запис.Комірка
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
                    ключовіСлова += $"\n{++sequenceNumber}. {запис.Номенклатура.Назва} {запис.Характеристика.Назва} {запис.Серія.Назва} {запис.Комірка.Назва}";
            }

            return ключовіСлова;
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
                TreeViewColumn Характеристика = new TreeViewColumn("Характеристика", new CellRendererText(), "text", (int)Columns.Характеристика)
                {
                    Resizable = true,
                    MinWidth = 200,
                    Visible = Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const
                };

                Характеристика.Data.Add("Column", Columns.Характеристика);

                TreeViewGrid.AppendColumn(Характеристика);
            }

            //Серія
            {
                TreeViewColumn Серія = new TreeViewColumn("Серія", new CellRendererText(), "text", (int)Columns.Серія)
                {
                    Resizable = true,
                    MinWidth = 200,
                    Visible = Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const
                };

                Серія.Data.Add("Column", Columns.Серія);

                TreeViewGrid.AppendColumn(Серія);
            }

            //КількістьУпаковок
            {
                CellRendererText КількістьУпаковок = new CellRendererText() { Editable = true };
                КількістьУпаковок.Edited += TextChanged;
                КількістьУпаковок.Data.Add("Column", (int)Columns.КількістьУпаковок);

                TreeViewColumn Column = new TreeViewColumn("Коєфіціент", КількістьУпаковок, "text", (int)Columns.КількістьУпаковок) { Resizable = true, MinWidth = 50 };
                Column.SetCellDataFunc(КількістьУпаковок, new TreeCellDataFunc(NumericCellDataFunc));
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
                CellRendererText КількістьФакт = new CellRendererText() { Editable = false };
                КількістьФакт.Data.Add("Column", Columns.КількістьФакт);

                TreeViewColumn Column = new TreeViewColumn("Кільк.факт", КількістьФакт, "text", (int)Columns.КількістьФакт) { Resizable = true, MinWidth = 100 };
                Column.SetCellDataFunc(КількістьФакт, new TreeCellDataFunc(NumericCellDataFunc));
                TreeViewGrid.AppendColumn(Column);
            }

            //Комірка
            {
                TreeViewColumn Комірка = new TreeViewColumn("Комірка", new CellRendererText(), "text", (int)Columns.Комірка) { Resizable = true, MinWidth = 100 };
                Комірка.Data.Add("Column", Columns.Комірка);

                TreeViewGrid.AppendColumn(Комірка);
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

                            if (ОбновитиЗначенняДокумента != null)
                                ОбновитиЗначенняДокумента.Invoke();

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
                case Columns.Серія:
                    {
                        СеріїНоменклатури_ШвидкийВибір page = new СеріїНоменклатури_ШвидкийВибір() { PopoverParent = popoverSmallSelect, DirectoryPointerItem = запис.Серія.UnigueID };
                        page.CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                        {
                            запис.Серія = new СеріїНоменклатури_Pointer(selectPointer);
                            await Запис.ПісляЗміни_Серія(запис);

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
                case Columns.Комірка:
                    {
                        СкладськіКомірки_ШвидкийВибір page = new СкладськіКомірки_ШвидкийВибір() { PopoverParent = popoverSmallSelect, DirectoryPointerItem = запис.Комірка.UnigueID };
                        page.CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                        {
                            запис.Комірка = new СкладськіКомірки_Pointer(selectPointer);
                            await Запис.ПісляЗміни_Комірка(запис);

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
                case Columns.Серія:
                    {
                        запис.Серія.Clear();
                        break;
                    }
                case Columns.Пакування:
                    {
                        запис.Пакування.Clear();
                        break;
                    }
                case Columns.Комірка:
                    {
                        запис.Комірка.Clear();
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
                            {
                                if (value <= 0) value = 1;

                                запис.КількістьУпаковок = value;
                                Запис.ПісляЗміни_Кількість(запис);
                            }

                            break;
                        }
                    case Columns.Кількість:
                        {
                            var (check, value) = Validate.IsDecimal(args.NewText);
                            if (check)
                            {
                                запис.Кількість = value;
                                Запис.ПісляЗміни_Кількість(запис);
                            }

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

        #region ОбробкаТабЧастини Товари

        async void РозприділитиПоКоміркахВідповідноДоЗалишків(object? sender, EventArgs args)
        {
            List<Запис> НовіЗаписи = new List<Запис>();
            int sequenceNumber = 0;

            foreach (Запис запис in Записи)
            {
                if (запис.КількістьУпаковок <= 0)
                {
                    Message.Error(null, $"В рядку {sequenceNumber + 1} коефіцієнт з помилкою! Має бути 1 або більше.");
                    return;
                }

                //
                // Для номенклатури зчитується одиниця виміру і з цією одиницею виміру шукаються залишки
                // Одиниця виміру з табличної частини служить тільки для перерахунку в основну одиницю виміру
                //

                ПакуванняОдиниціВиміру_Pointer ОдиницяВиміруНомеклатури = new();
                if (!запис.Номенклатура.IsEmpty())
                    ОдиницяВиміруНомеклатури = (await запис.Номенклатура.GetDirectoryObject())?.ОдиницяВиміру ?? new();
                else
                    continue;

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
                Dictionary<string, object> paramQuery = new Dictionary<string, object>
                {
                    { "Номенклатура", запис.Номенклатура.UnigueID.UGuid },
                    { "ХарактеристикаНоменклатури", запис.Характеристика.UnigueID.UGuid },
                    { "Пакування", ОдиницяВиміруНомеклатури.UnigueID.UGuid },
                    { "Серія", запис.Серія.UnigueID.UGuid }
                };

                decimal КількістьФакт = запис.Кількість * запис.КількістьУпаковок;
                decimal КількістьЯкуПотрібноРозприділити = КількістьФакт;
                bool єЗміниВЗаписі = false;

                var recordResult = await Config.Kernel.DataBase.SelectRequest(query, paramQuery);
                foreach (Dictionary<string, object> row in recordResult.ListRow)
                {
                    decimal ЗалишокВКомірці = (decimal)row["ВНаявності"];
                    СкладськіКомірки_Pointer складськіКомірки_Pointer = new СкладськіКомірки_Pointer(row["Комірка"]);

                    if (ЗалишокВКомірці >= КількістьЯкуПотрібноРозприділити)
                    {
                        КількістьФакт = КількістьЯкуПотрібноРозприділити;

                        запис.Кількість = Math.Round(КількістьЯкуПотрібноРозприділити / запис.КількістьУпаковок, 2);
                        Запис.ПісляЗміни_Кількість(запис);

                        запис.Комірка = складськіКомірки_Pointer;
                        await Запис.ПісляЗміни_Комірка(запис);

                        єЗміниВЗаписі = true;
                        break;
                    }
                    else
                    {
                        Запис записНовий = Запис.Clone(запис);
                        НовіЗаписи.Add(записНовий);

                        записНовий.Кількість = Math.Round(ЗалишокВКомірці / запис.КількістьУпаковок, 2);
                        Запис.ПісляЗміни_Кількість(записНовий);

                        записНовий.Комірка = складськіКомірки_Pointer;
                        await Запис.ПісляЗміни_Комірка(записНовий);

                        КількістьЯкуПотрібноРозприділити -= ЗалишокВКомірці;
                    }
                }

                //Залишок який невдалось розприділити
                if (КількістьФакт != КількістьЯкуПотрібноРозприділити && КількістьЯкуПотрібноРозприділити > 0)
                {
                    запис.Кількість = Math.Round(КількістьЯкуПотрібноРозприділити / запис.КількістьУпаковок, 2);
                    Запис.ПісляЗміни_Кількість(запис);

                    запис.Комірка = new СкладськіКомірки_Pointer();
                    await Запис.ПісляЗміни_Комірка(запис);

                    єЗміниВЗаписі = true;
                }

                //Відобразити зміни в записі
                if (єЗміниВЗаписі)
                {
                    Store.GetIterFromString(out TreeIter iter, sequenceNumber.ToString());
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