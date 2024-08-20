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
using Перелічення = StorageAndTrade_1_0.Перелічення;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.РегістриВідомостей;

namespace StorageAndTrade
{
    class ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари : ДокументТабличнаЧастина
    {
        public ВстановленняЦінНоменклатури_Objest? ВстановленняЦінНоменклатури_Objest { get; set; }
        public System.Action? ОбновитиЗначенняДокумента { get; set; }

        #region Записи

        enum Columns
        {
            НомерРядка,
            Номенклатура,
            Характеристика,
            Пакування,
            ВидЦіни,
            Ціна
        }

        ListStore Store = new ListStore(
            typeof(int),      //НомерРядка
            typeof(string),   //Номенклатура
            typeof(string),   //Характеристика
            typeof(string),   //Пакування
            typeof(string),   //ВидЦіни
            typeof(float)     //Ціна
        );

        List<Запис> Записи = new List<Запис>();

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            public int НомерРядка { get; set; }
            public Номенклатура_Pointer Номенклатура { get; set; } = new Номенклатура_Pointer();
            public ХарактеристикиНоменклатури_Pointer Характеристика { get; set; } = new ХарактеристикиНоменклатури_Pointer();
            public ПакуванняОдиниціВиміру_Pointer Пакування { get; set; } = new ПакуванняОдиниціВиміру_Pointer();
            public ВидиЦін_Pointer ВидЦіни { get; set; } = new ВидиЦін_Pointer();
            public decimal Ціна { get; set; }

            public object[] ToArray()
            {
                return
                [
                    НомерРядка,
                    Номенклатура.Назва,
                    Характеристика.Назва,
                    Пакування.Назва,
                    ВидЦіни.Назва,
                    (float)Ціна
                ];
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    Номенклатура = запис.Номенклатура.Copy(),
                    Характеристика = запис.Характеристика.Copy(),
                    Пакування = запис.Пакування.Copy(),
                    ВидЦіни = запис.ВидЦіни.Copy(),
                    Ціна = запис.Ціна
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
            public static async ValueTask ПісляЗміни_Пакування(Запис запис)
            {
                await запис.Пакування.GetPresentation();
            }
            public static async ValueTask ПісляЗміни_ВидЦіни(Запис запис)
            {
                await запис.ВидЦіни.GetPresentation();
            }
        }

        #endregion

        public ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари() : base()
        {
            TreeViewGrid.Model = Store;
            AddColumn();

            //Доповнення ToolbarTop новими функціями
            ToolItem toolItemSeparator = new ToolItem
            {
                new Separator(Orientation.Horizontal)
            };

            ToolbarTop.Add(toolItemSeparator);

            ToolButton fillDirectoryButton = new ToolButton(new Image(Stock.Add, IconSize.Menu), "Заповнити товарами") { IsImportant = true };
            fillDirectoryButton.Clicked += OnFillDirectory;
            ToolbarTop.Add(fillDirectoryButton);

            ToolButton fillRegisterButton = new ToolButton(new Image(Stock.Add, IconSize.Menu), "Заповнити товарами з цінами") { IsImportant = true };
            fillRegisterButton.Clicked += OnFillRegister;
            ToolbarTop.Add(fillRegisterButton);
        }

        public override async ValueTask LoadRecords()
        {
            Store.Clear();
            Записи.Clear();

            if (ВстановленняЦінНоменклатури_Objest != null)
            {
                ВстановленняЦінНоменклатури_Objest.Товари_TablePart.FillJoin([ВстановленняЦінНоменклатури_Товари_TablePart.НомерРядка]);
                await ВстановленняЦінНоменклатури_Objest.Товари_TablePart.Read();

                foreach (ВстановленняЦінНоменклатури_Товари_TablePart.Record record in ВстановленняЦінНоменклатури_Objest.Товари_TablePart.Records)
                {
                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        НомерРядка = record.НомерРядка,
                        Номенклатура = record.Номенклатура,
                        Характеристика = record.ХарактеристикаНоменклатури,
                        Пакування = record.Пакування,
                        ВидЦіни = record.ВидЦіни,
                        Ціна = record.Ціна
                    };

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }
        }

        public override async ValueTask SaveRecords()
        {
            if (ВстановленняЦінНоменклатури_Objest != null)
            {
                ВстановленняЦінНоменклатури_Objest.Товари_TablePart.Records.Clear();

                int sequenceNumber = 0;

                foreach (Запис запис in Записи)
                {
                    ВстановленняЦінНоменклатури_Товари_TablePart.Record record = new ВстановленняЦінНоменклатури_Товари_TablePart.Record
                    {
                        UID = запис.ID,
                        НомерРядка = ++sequenceNumber,
                        Номенклатура = запис.Номенклатура,
                        ХарактеристикаНоменклатури = запис.Характеристика,
                        Пакування = запис.Пакування,
                        ВидЦіни = запис.ВидЦіни,
                        Ціна = запис.Ціна
                    };

                    ВстановленняЦінНоменклатури_Objest.Товари_TablePart.Records.Add(record);
                }

                await ВстановленняЦінНоменклатури_Objest.Товари_TablePart.Save(true);

                await LoadRecords();
            }
        }

        public string КлючовіСловаДляПошуку()
        {
            string ключовіСлова = "";

            if (ВстановленняЦінНоменклатури_Objest != null)
            {
                int sequenceNumber = 0;
                foreach (Запис запис in Записи)
                    ключовіСлова += $"\n{++sequenceNumber}. {запис.Номенклатура.Назва} {запис.Характеристика.Назва} ";
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

            //Пакування
            {
                TreeViewColumn Пакування = new TreeViewColumn("Пакування", new CellRendererText(), "text", (int)Columns.Пакування) { Resizable = true, MinWidth = 100 };
                Пакування.Data.Add("Column", Columns.Пакування);

                TreeViewGrid.AppendColumn(Пакування);
            }

            //ВидЦіни
            {
                TreeViewColumn ВидЦіни = new TreeViewColumn("Вид ціни", new CellRendererText(), "text", (int)Columns.ВидЦіни) { Resizable = true, MinWidth = 100 };
                ВидЦіни.Data.Add("Column", Columns.ВидЦіни);

                TreeViewGrid.AppendColumn(ВидЦіни);
            }

            //Ціна
            {
                CellRendererText Ціна = new CellRendererText() { Editable = true };
                Ціна.Edited += TextChanged;
                Ціна.Data.Add("Column", (int)Columns.Ціна);

                TreeViewColumn Column = new TreeViewColumn("Ціна", Ціна, "text", (int)Columns.Ціна) { Resizable = true, MinWidth = 100 };
                Column.SetCellDataFunc(Ціна, new TreeCellDataFunc(NumericCellDataFunc));
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
                case Columns.ВидЦіни:
                    {
                        ВидиЦін_ШвидкийВибір page = new ВидиЦін_ШвидкийВибір() { PopoverParent = popoverSmallSelect, DirectoryPointerItem = запис.ВидЦіни.UnigueID };
                        page.CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                        {
                            запис.ВидЦіни = new ВидиЦін_Pointer(selectPointer);
                            await Запис.ПісляЗміни_ВидЦіни(запис);

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
                case Columns.ВидЦіни:
                    {
                        запис.ВидЦіни.Clear();
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
                    case Columns.Ціна:
                        {
                            cellText.Text = запис.Ціна.ToString();
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
                    case Columns.Ціна:
                        {
                            var (check, value) = Validate.IsDecimal(args.NewText);
                            if (check)
                                запис.Ціна = value;

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

        async void OnFillDirectory(object? sender, EventArgs args)
        {
            ОбновитиЗначенняДокумента?.Invoke();

            string query = $@"
SELECT
    Номенклатура.uid AS Номенклатура,
    Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    Номенклатура.{Номенклатура_Const.ОдиницяВиміру} AS Пакування,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS Пакування_Назва,
    (
        SELECT
            {ЦіниНоменклатури_Const.Ціна}
        FROM
            {ЦіниНоменклатури_Const.TABLE} AS ЦіниНоменклатури
        WHERE
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.Номенклатура} = Номенклатура.uid AND
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.ХарактеристикаНоменклатури} = '{Guid.Empty}' AND
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.Пакування} = Номенклатура.{Номенклатура_Const.ОдиницяВиміру} AND
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.ВидЦіни} = @vid_cen AND
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.Валюта} = @valuta
        ORDER BY ЦіниНоменклатури.period DESC
        LIMIT 1
    ) AS Ціна
FROM
    {Номенклатура_Const.TABLE} AS Номенклатура
    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON 
        Довідник_ПакуванняОдиниціВиміру.uid = Номенклатура.{Номенклатура_Const.ОдиницяВиміру}
WHERE
    Номенклатура.{Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Товар} OR
    Номенклатура.{Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Послуга}
ORDER BY Номенклатура_Назва, Пакування_Назва
";
            Store.Clear();
            Записи.Clear();

            if (ВстановленняЦінНоменклатури_Objest != null)
            {
                Dictionary<string, object> paramQuery = new Dictionary<string, object>
                {
                    { "valuta", ВстановленняЦінНоменклатури_Objest.Валюта.UnigueID.UGuid },
                    { "vid_cen", ВстановленняЦінНоменклатури_Objest.ВидЦіни.UnigueID.UGuid }
                };

                var recordResult = await Config.Kernel.DataBase.SelectRequest(query, paramQuery);

                string ВидЦіниНазва = await ВстановленняЦінНоменклатури_Objest.ВидЦіни.GetPresentation();

                foreach (Dictionary<string, object> row in recordResult.ListRow)
                {
                    Запис запис = new Запис
                    {
                        ID = Guid.Empty,
                        Номенклатура = new Номенклатура_Pointer(row["Номенклатура"]),
                        Характеристика = new ХарактеристикиНоменклатури_Pointer(),
                        Пакування = new ПакуванняОдиниціВиміру_Pointer(row["Пакування"]),
                        ВидЦіни = ВстановленняЦінНоменклатури_Objest.ВидЦіни,
                        Ціна = row["Ціна"] != DBNull.Value ? (decimal)row["Ціна"] : 0
                    };

                    запис.Номенклатура.Назва = row["Номенклатура_Назва"].ToString() ?? "";
                    запис.Пакування.Назва = row["Пакування_Назва"].ToString() ?? "";
                    запис.ВидЦіни.Назва = ВидЦіниНазва;

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }
        }

        async void OnFillRegister(object? sender, EventArgs args)
        {
            ОбновитиЗначенняДокумента?.Invoke();

            string query = $@"
WITH register AS
(
    SELECT DISTINCT {ЦіниНоменклатури_Const.Номенклатура} AS Номенклатура,
        {ЦіниНоменклатури_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        {ЦіниНоменклатури_Const.Пакування} AS Пакування,
        {ЦіниНоменклатури_Const.ВидЦіни} AS ВидЦіни
    FROM
        {ЦіниНоменклатури_Const.TABLE}
    WHERE
        {ЦіниНоменклатури_Const.Валюта} = @valuta";

            #region WHERE

            if (ВстановленняЦінНоменклатури_Objest != null && !ВстановленняЦінНоменклатури_Objest.ВидЦіни.IsEmpty())
            {
                query += $@"
AND {ЦіниНоменклатури_Const.ВидЦіни} = @vid_cen
";
            }

            #endregion

            query += $@"
)
SELECT
    register.Номенклатура,
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    register.ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    register.Пакування,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS Пакування_Назва,
    register.ВидЦіни,
    Довідник_ВидиЦін.{ВидиЦін_Const.Назва} AS ВидЦіни_Назва,
    (
        SELECT 
            {ЦіниНоменклатури_Const.Ціна}
        FROM 
            {ЦіниНоменклатури_Const.TABLE} AS ЦіниНоменклатури
        WHERE
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.Номенклатура} = register.Номенклатура AND
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.ХарактеристикаНоменклатури} = register.ХарактеристикаНоменклатури AND
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.Пакування} = register.Пакування AND
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.ВидЦіни} = register.ВидЦіни AND
            ЦіниНоменклатури.{ЦіниНоменклатури_Const.Валюта} = @valuta
        ORDER BY ЦіниНоменклатури.period DESC
        LIMIT 1
    ) AS Ціна
FROM
    register
    
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON 
        Довідник_Номенклатура.uid = register.Номенклатура
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON 
        Довідник_ХарактеристикиНоменклатури.uid = register.ХарактеристикаНоменклатури
    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON 
        Довідник_ПакуванняОдиниціВиміру.uid = register.Пакування
    LEFT JOIN {ВидиЦін_Const.TABLE} AS Довідник_ВидиЦін ON 
        Довідник_ВидиЦін.uid = register.ВидЦіни
ORDER BY
    Номенклатура_Назва, ХарактеристикаНоменклатури_Назва, Пакування_Назва, ВидЦіни_Назва
";

            Store.Clear();
            Записи.Clear();

            if (ВстановленняЦінНоменклатури_Objest != null)
            {
                Dictionary<string, object> paramQuery = new Dictionary<string, object>
                {
                    { "valuta", ВстановленняЦінНоменклатури_Objest.Валюта.UnigueID.UGuid },
                    { "vid_cen", ВстановленняЦінНоменклатури_Objest.ВидЦіни.UnigueID.UGuid }
                };

                var recordResult = await Config.Kernel.DataBase.SelectRequest(query, paramQuery);

                foreach (Dictionary<string, object> row in recordResult.ListRow)
                {
                    Запис запис = new Запис
                    {
                        ID = Guid.Empty,
                        Номенклатура = new Номенклатура_Pointer(row["Номенклатура"]),
                        Характеристика = new ХарактеристикиНоменклатури_Pointer(row["ХарактеристикаНоменклатури"]),
                        Пакування = new ПакуванняОдиниціВиміру_Pointer(row["Пакування"]),
                        ВидЦіни = new ВидиЦін_Pointer(row["ВидЦіни"]),
                        Ціна = (decimal)row["Ціна"]
                    };

                    запис.Номенклатура.Назва = row["Номенклатура_Назва"].ToString() ?? "";
                    запис.Пакування.Назва = row["Пакування_Назва"].ToString() ?? "";
                    запис.ВидЦіни.Назва = row["ВидЦіни_Назва"].ToString() ?? "";

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }

        }

        #endregion
    }
}