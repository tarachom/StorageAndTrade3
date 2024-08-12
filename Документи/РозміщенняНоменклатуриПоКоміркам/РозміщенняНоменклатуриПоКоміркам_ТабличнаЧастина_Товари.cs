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

using AccountingSoftware;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using Перелічення = StorageAndTrade_1_0.Перелічення;
using StorageAndTrade_1_0.РегістриВідомостей;

namespace StorageAndTrade
{
    class РозміщенняНоменклатуриПоКоміркам_ТабличнаЧастина_Товари : ДокументТабличнаЧастина
    {
        public РозміщенняНоменклатуриПоКоміркам_Objest? РозміщенняНоменклатуриПоКоміркам_Objest { get; set; }

        #region Записи

        enum Columns
        {
            НомерРядка,
            Номенклатура,
            Пакування,
            Комірка
        }

        ListStore Store = new ListStore(
            typeof(int),      //НомерРядка
            typeof(string),   //Номенклатура
            typeof(string),   //Пакування
            typeof(string)    //Комірка
        );

        List<Запис> Записи = new List<Запис>();

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            public int НомерРядка { get; set; }
            public Номенклатура_Pointer Номенклатура { get; set; } = new Номенклатура_Pointer();
            public ПакуванняОдиниціВиміру_Pointer Пакування { get; set; } = new ПакуванняОдиниціВиміру_Pointer();
            public СкладськіКомірки_Pointer Комірка { get; set; } = new СкладськіКомірки_Pointer();

            public object[] ToArray()
            {
                return
                [
                    НомерРядка,
                    Номенклатура.Назва,
                    Пакування.Назва,
                    Комірка.Назва
                ];
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    Номенклатура = запис.Номенклатура.Copy(),
                    Пакування = запис.Пакування.Copy(),
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
            public static async ValueTask ПісляЗміни_Пакування(Запис запис)
            {
                await запис.Пакування.GetPresentation();
            }
            public static async ValueTask ПісляЗміни_Комірка(Запис запис)
            {
                await запис.Комірка.GetPresentation();
            }
        }

        #endregion

        public РозміщенняНоменклатуриПоКоміркам_ТабличнаЧастина_Товари() : base()
        {
            TreeViewGrid.Model = Store;
            AddColumn();

            //
            //
            //

            ToolButton fillDirectoryButton = new ToolButton(new Image(Stock.Add, IconSize.Menu), "Заповнити товарами") { IsImportant = true };
            fillDirectoryButton.Clicked += OnFillDirectory;
            ToolbarTop.Add(fillDirectoryButton);

            ToolButton fillRegisterButton = new ToolButton(new Image(Stock.Add, IconSize.Menu), "Заповнити комірками") { IsImportant = true };
            fillRegisterButton.Clicked += OnFillRegister;
            ToolbarTop.Add(fillRegisterButton);
        }

        public override async ValueTask LoadRecords()
        {
            Store.Clear();
            Записи.Clear();

            if (РозміщенняНоменклатуриПоКоміркам_Objest != null)
            {
                Query querySelect = РозміщенняНоменклатуриПоКоміркам_Objest.Товари_TablePart.QuerySelect;
                querySelect.Clear();

                //JOIN Номенклатура
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(Номенклатура_Const.TABLE + "." + Номенклатура_Const.Назва, "Номенклатура"));
                querySelect.Joins.Add(
                    new Join(Номенклатура_Const.TABLE, РозміщенняНоменклатуриПоКоміркам_Товари_TablePart.Номенклатура, querySelect.Table));

                //JOIN Пакування
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ПакуванняОдиниціВиміру_Const.TABLE + "." + ПакуванняОдиниціВиміру_Const.Назва, "Пакування"));
                querySelect.Joins.Add(
                    new Join(ПакуванняОдиниціВиміру_Const.TABLE, РозміщенняНоменклатуриПоКоміркам_Товари_TablePart.Пакування, querySelect.Table));

                //JOIN Комірка
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(СкладськіКомірки_Const.TABLE + "." + СкладськіКомірки_Const.Назва, "Комірка"));
                querySelect.Joins.Add(
                    new Join(СкладськіКомірки_Const.TABLE, РозміщенняНоменклатуриПоКоміркам_Товари_TablePart.Комірка, querySelect.Table));

                //ORDER
                querySelect.Order.Add(РозміщенняНоменклатуриПоКоміркам_Товари_TablePart.НомерРядка, SelectOrder.ASC);

                await РозміщенняНоменклатуриПоКоміркам_Objest.Товари_TablePart.Read();

                Dictionary<string, Dictionary<string, string>> JoinValue = РозміщенняНоменклатуриПоКоміркам_Objest.Товари_TablePart.JoinValue;

                foreach (РозміщенняНоменклатуриПоКоміркам_Товари_TablePart.Record record in РозміщенняНоменклатуриПоКоміркам_Objest.Товари_TablePart.Records)
                {
                    string uid = record.UID.ToString();

                    record.Номенклатура.Назва = JoinValue[uid]["Номенклатура"];
                    record.Пакування.Назва = JoinValue[uid]["Пакування"];
                    record.Комірка.Назва = JoinValue[uid]["Комірка"];

                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        НомерРядка = record.НомерРядка,
                        Номенклатура = record.Номенклатура,
                        Пакування = record.Пакування,
                        Комірка = record.Комірка
                    };

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }
        }

        public override async ValueTask SaveRecords()
        {
            if (РозміщенняНоменклатуриПоКоміркам_Objest != null)
            {
                РозміщенняНоменклатуриПоКоміркам_Objest.Товари_TablePart.Records.Clear();

                int sequenceNumber = 0;

                foreach (Запис запис in Записи)
                {
                    РозміщенняНоменклатуриПоКоміркам_Товари_TablePart.Record record = new РозміщенняНоменклатуриПоКоміркам_Товари_TablePart.Record
                    {
                        UID = запис.ID,
                        НомерРядка = ++sequenceNumber,
                        Номенклатура = запис.Номенклатура,
                        Пакування = запис.Пакування,
                        Комірка = запис.Комірка
                    };

                    РозміщенняНоменклатуриПоКоміркам_Objest.Товари_TablePart.Records.Add(record);
                }

                await РозміщенняНоменклатуриПоКоміркам_Objest.Товари_TablePart.Save(true);

                await LoadRecords();
            }
        }

        public string КлючовіСловаДляПошуку()
        {
            string ключовіСлова = "";

            if (РозміщенняНоменклатуриПоКоміркам_Objest != null)
            {
                int sequenceNumber = 0;
                foreach (Запис запис in Записи)
                    ключовіСлова += $"\n{++sequenceNumber}. {запис.Номенклатура.Назва} {запис.Комірка.Назва} ";
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

            //Пакування
            {
                TreeViewColumn Пакування = new TreeViewColumn("Пакування", new CellRendererText(), "text", (int)Columns.Пакування) { Resizable = true, MinWidth = 100 };
                Пакування.Data.Add("Column", Columns.Пакування);

                TreeViewGrid.AppendColumn(Пакування);
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

                            Store.SetValues(iter, запис.ToArray());
                        };

                        popoverSmallSelect.Add(page);
                        popoverSmallSelect.ShowAll();

                        await page.LoadRecords();
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

                        await page.LoadRecords();
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
                case Columns.Номенклатура:
                    {
                        запис.Номенклатура.Clear();
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

        /*

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

                switch ((Columns)ColumnNum)
                {
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
        
        */

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
            string query = $@"
SELECT
    Номенклатура.uid AS Номенклатура,
    Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    Номенклатура.{Номенклатура_Const.ОдиницяВиміру} AS Пакування,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS Пакування_Назва,
    (
        SELECT
            {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.Комірка}
        FROM
            {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.TABLE} AS РозміщенняНоменклатуриПоКоміркамНаСкладі
        WHERE
            РозміщенняНоменклатуриПоКоміркамНаСкладі.{РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.Номенклатура} = Номенклатура.uid
        ORDER BY РозміщенняНоменклатуриПоКоміркамНаСкладі.period DESC
        LIMIT 1
    ) AS Комірка,
    Довідник_СкладськіКомірки.{СкладськіКомірки_Const.Назва} AS Комірка_Назва
FROM
    {Номенклатура_Const.TABLE} AS Номенклатура
    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON 
        Довідник_ПакуванняОдиниціВиміру.uid = Номенклатура.{Номенклатура_Const.ОдиницяВиміру}
    LEFT JOIN {СкладськіКомірки_Const.TABLE} AS Довідник_СкладськіКомірки ON 
        Довідник_СкладськіКомірки.uid = 
        (
            SELECT
                {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.Комірка}
            FROM
                {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.TABLE} AS РозміщенняНоменклатуриПоКоміркамНаСкладі
            WHERE
                РозміщенняНоменклатуриПоКоміркамНаСкладі.{РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.Номенклатура} = Номенклатура.uid
            ORDER BY РозміщенняНоменклатуриПоКоміркамНаСкладі.period DESC
            LIMIT 1
        )
WHERE
    Номенклатура.{Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Товар}
ORDER BY Номенклатура_Назва
";
            Store.Clear();
            Записи.Clear();

            if (РозміщенняНоменклатуриПоКоміркам_Objest != null)
            {
                var recordResult = await Config.Kernel.DataBase.SelectRequest(query);

                foreach (Dictionary<string, object> row in recordResult.ListRow)
                {
                    Запис запис = new Запис
                    {
                        ID = Guid.Empty,
                        Номенклатура = new Номенклатура_Pointer(row["Номенклатура"]),
                        Пакування = new ПакуванняОдиниціВиміру_Pointer(row["Пакування"]),
                        Комірка = new СкладськіКомірки_Pointer(row["Комірка"])
                    };

                    запис.Номенклатура.Назва = row["Номенклатура_Назва"].ToString() ?? "";
                    запис.Пакування.Назва = row["Пакування_Назва"].ToString() ?? "";
                    запис.Комірка.Назва = row["Комірка_Назва"].ToString() ?? "";

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }
        }

        async void OnFillRegister(object? sender, EventArgs args)
        {
            string query = $@"
WITH register AS
(
    SELECT DISTINCT 
        {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.Номенклатура} AS Номенклатура
    FROM
        {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.TABLE}
)
SELECT
    register.Номенклатура,
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    Довідник_Номенклатура.{Номенклатура_Const.ОдиницяВиміру} AS Пакування,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS Пакування_Назва,
    (
        SELECT
            {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.Комірка}
        FROM
            {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.TABLE} AS РозміщенняНоменклатуриПоКоміркамНаСкладі
        WHERE
            РозміщенняНоменклатуриПоКоміркамНаСкладі.{РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.Номенклатура} = register.Номенклатура
        ORDER BY РозміщенняНоменклатуриПоКоміркамНаСкладі.period DESC
        LIMIT 1
    ) AS Комірка,
    Довідник_СкладськіКомірки.{СкладськіКомірки_Const.Назва} AS Комірка_Назва
FROM
    register

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON 
        Довідник_Номенклатура.uid = register.Номенклатура
    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON 
        Довідник_ПакуванняОдиниціВиміру.uid = Довідник_Номенклатура.{Номенклатура_Const.ОдиницяВиміру}
    LEFT JOIN {СкладськіКомірки_Const.TABLE} AS Довідник_СкладськіКомірки ON 
        Довідник_СкладськіКомірки.uid = 
        (
            SELECT
                {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.Комірка}
            FROM
                {РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.TABLE} AS РозміщенняНоменклатуриПоКоміркамНаСкладі
            WHERE
                РозміщенняНоменклатуриПоКоміркамНаСкладі.{РозміщенняНоменклатуриПоКоміркамНаСкладі_Const.Номенклатура} = register.Номенклатура
            ORDER BY РозміщенняНоменклатуриПоКоміркамНаСкладі.period DESC
            LIMIT 1
        )
ORDER BY
    Номенклатура_Назва
";

            Store.Clear();
            Записи.Clear();

            if (РозміщенняНоменклатуриПоКоміркам_Objest != null)
            {
                var recordResult = await Config.Kernel.DataBase.SelectRequest(query);

                foreach (Dictionary<string, object> row in recordResult.ListRow)
                {
                    Запис запис = new Запис
                    {
                        ID = Guid.Empty,
                        Номенклатура = new Номенклатура_Pointer(row["Номенклатура"]),
                        Пакування = new ПакуванняОдиниціВиміру_Pointer(row["Пакування"]),
                        Комірка = new СкладськіКомірки_Pointer(row["Комірка"])
                    };

                    запис.Номенклатура.Назва = row["Номенклатура_Назва"].ToString() ?? "";
                    запис.Пакування.Назва = row["Пакування_Назва"].ToString() ?? "";
                    запис.Комірка.Назва = row["Комірка_Назва"].ToString() ?? "";

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }

        }

        #endregion

    }
}