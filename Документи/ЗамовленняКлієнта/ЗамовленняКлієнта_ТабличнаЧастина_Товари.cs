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
using Константи = StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.РегістриВідомостей;

namespace StorageAndTrade
{
    class ЗамовленняКлієнта_ТабличнаЧастина_Товари : ДокументТабличнаЧастина
    {
        public ЗамовленняКлієнта_Objest? ЗамовленняКлієнта_Objest { get; set; }

        #region Записи

        enum Columns
        {
            НомерРядка,
            Номенклатура,
            Характеристика,
            КількістьУпаковок,
            Пакування,
            Кількість,
            КількістьФакт,
            ВидЦіни,
            Ціна,
            Сума,
            Скидка,
            Склад
        }

        ListStore Store = new ListStore(
            typeof(int),      //НомерРядка
            typeof(string),   //Номенклатура
            typeof(string),   //Характеристика
            typeof(int),      //КількістьУпаковок
            typeof(string),   //Пакування
            typeof(float),    //Кількість
            typeof(float),    //КількістьФакт
            typeof(string),   //ВидЦіни
            typeof(float),    //Ціна
            typeof(float),    //Сума
            typeof(float),    //Скидка
            typeof(string)    //Склад
        );

        List<Запис> Записи = new List<Запис>();

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            public int НомерРядка { get; set; }
            public Номенклатура_Pointer Номенклатура { get; set; } = new Номенклатура_Pointer();
            public ХарактеристикиНоменклатури_Pointer Характеристика { get; set; } = new ХарактеристикиНоменклатури_Pointer();
            public int КількістьУпаковок { get; set; } = 1;
            public ПакуванняОдиниціВиміру_Pointer Пакування { get; set; } = new ПакуванняОдиниціВиміру_Pointer();
            public decimal Кількість { get; set; } = 1;
            public decimal КількістьФакт { get; set; } = 1;
            public ВидиЦін_Pointer ВидЦіни { get; set; } = new ВидиЦін_Pointer();
            public decimal Ціна { get; set; }
            public decimal Сума { get; set; }
            public decimal Скидка { get; set; }
            public Склади_Pointer Склад { get; set; } = new Склади_Pointer();

            public object[] ToArray()
            {
                return
                [
                    НомерРядка,
                    Номенклатура.Назва,
                    Характеристика.Назва,
                    КількістьУпаковок,
                    Пакування.Назва,
                    (float)Кількість,
                    (float)КількістьФакт,
                    ВидЦіни.Назва,
                    (float)Ціна,
                    (float)Сума,
                    (float)Скидка,
                    Склад.Назва
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
                    Пакування = запис.Пакування.Copy(),
                    Кількість = запис.Кількість,
                    КількістьФакт = запис.КількістьФакт,
                    ВидЦіни = запис.ВидЦіни.Copy(),
                    Ціна = запис.Ціна,
                    Сума = запис.Сума,
                    Скидка = запис.Скидка,
                    Склад = запис.Склад.Copy()
                };
            }

            public static async ValueTask ПісляДодаванняНового(Запис запис)
            {
                запис.ВидЦіни = Константи.ЗначенняЗаЗамовчуванням.ОсновнийВидЦіни_Const;
                await ПісляЗміни_ВидЦіни(запис);
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

                if (!запис.Пакування.IsEmpty())
                {
                    ПакуванняОдиниціВиміру_Objest? пакуванняОдиниціВиміру_Objest = await запис.Пакування.GetDirectoryObject();
                    if (пакуванняОдиниціВиміру_Objest != null)
                        запис.КількістьУпаковок = (пакуванняОдиниціВиміру_Objest.КількістьУпаковок > 0) ? пакуванняОдиниціВиміру_Objest.КількістьУпаковок : 1;
                    else
                        запис.КількістьУпаковок = 1;
                }

                Запис.ПісляЗміни_КількістьАбоЦіна(запис);
            }
            public static async ValueTask ПісляЗміни_ВидЦіни(Запис запис)
            {
                await запис.ВидЦіни.GetPresentation();
            }
            public static async ValueTask ПісляЗміни_Склад(Запис запис)
            {
                await запис.Склад.GetPresentation();
            }
            public static void ПісляЗміни_КількістьАбоЦіна(Запис запис)
            {
                запис.КількістьФакт = запис.Кількість * запис.КількістьУпаковок;
                запис.Сума = запис.Кількість * запис.Ціна;
            }
            public static async ValueTask ОтриматиЦіну(Запис запис)
            {
                if (запис.Номенклатура.IsEmpty())
                    return;

                if (запис.ВидЦіни.IsEmpty())
                    return;

                string query = $@"
SELECT
    ЦіниНоменклатури.{ЦіниНоменклатури_Const.Ціна} AS Ціна
FROM 
    {ЦіниНоменклатури_Const.TABLE} AS ЦіниНоменклатури
WHERE
    ЦіниНоменклатури.{ЦіниНоменклатури_Const.ВидЦіни} = '{запис.ВидЦіни.UnigueID}' AND
    ЦіниНоменклатури.{ЦіниНоменклатури_Const.Номенклатура} = '{запис.Номенклатура.UnigueID}'
";

                #region WHERE

                if (!запис.Характеристика.IsEmpty())
                {
                    query += $@"
AND ЦіниНоменклатури.{ЦіниНоменклатури_Const.ХарактеристикаНоменклатури} = '{запис.Характеристика.UnigueID}'
";
                }

                #endregion

                query += $@"
ORDER BY 
    ЦіниНоменклатури.period DESC 
LIMIT 1
";

                var recordResult = await Config.Kernel.DataBase.SelectRequest(query);
                if (recordResult.Result)
                    foreach (Dictionary<string, object> row in recordResult.ListRow)
                    {
                        запис.Ціна = (decimal)row["Ціна"];
                        запис.Сума = запис.Кількість * запис.Ціна;
                    }
            }
        }

        #endregion

        Label ПідсумокСума = new Label() { Selectable = true };
        Label ПідсумокСкидка = new Label() { Selectable = true };

        public ЗамовленняКлієнта_ТабличнаЧастина_Товари() : base()
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
            hBox.PackStart(ПідсумокСкидка, false, false, 2);

            base.PackStart(hBox, false, false, 2);
        }

        void ОбчислитиПідсумки()
        {
            decimal Сума = 0;
            decimal Скидка = 0;

            foreach (Запис запис in Записи)
            {
                Сума += запис.Сума;
                Скидка += запис.Скидка;
            }

            ПідсумокСума.Text = $"Сума: <b>{Сума}</b>";
            ПідсумокСума.UseMarkup = true;

            ПідсумокСкидка.Text = $"Скидка: <b>{Скидка}</b>";
            ПідсумокСкидка.UseMarkup = true;
        }

        #endregion

        public override async ValueTask LoadRecords()
        {
            Store.Clear();
            Записи.Clear();

            if (ЗамовленняКлієнта_Objest != null)
            {
                Query querySelect = ЗамовленняКлієнта_Objest.Товари_TablePart.QuerySelect;
                querySelect.Clear();

                //JOIN Номенклатура
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(Номенклатура_Const.TABLE + "." + Номенклатура_Const.Назва, "Номенклатура"));
                querySelect.Joins.Add(
                    new Join(Номенклатура_Const.TABLE, ЗамовленняКлієнта_Товари_TablePart.Номенклатура, querySelect.Table));

                //JOIN Характеристика
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ХарактеристикиНоменклатури_Const.TABLE + "." + ХарактеристикиНоменклатури_Const.Назва, "Характеристика"));
                querySelect.Joins.Add(
                    new Join(ХарактеристикиНоменклатури_Const.TABLE, ЗамовленняКлієнта_Товари_TablePart.ХарактеристикаНоменклатури, querySelect.Table));

                //JOIN Пакування
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ПакуванняОдиниціВиміру_Const.TABLE + "." + ПакуванняОдиниціВиміру_Const.Назва, "Пакування"));
                querySelect.Joins.Add(
                    new Join(ПакуванняОдиниціВиміру_Const.TABLE, ЗамовленняКлієнта_Товари_TablePart.Пакування, querySelect.Table));

                //JOIN ВидЦін
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(ВидиЦін_Const.TABLE + "." + ВидиЦін_Const.Назва, "ВидЦін"));
                querySelect.Joins.Add(
                    new Join(ВидиЦін_Const.TABLE, ЗамовленняКлієнта_Товари_TablePart.ВидЦіни, querySelect.Table));

                //JOIN Склад
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(Склади_Const.TABLE + "." + Склади_Const.Назва, "Склад"));
                querySelect.Joins.Add(
                    new Join(Склади_Const.TABLE, ЗамовленняКлієнта_Товари_TablePart.Склад, querySelect.Table));

                //ORDER
                querySelect.Order.Add(ЗамовленняКлієнта_Товари_TablePart.НомерРядка, SelectOrder.ASC);

                await ЗамовленняКлієнта_Objest.Товари_TablePart.Read();

                Dictionary<string, Dictionary<string, string>> JoinValue = ЗамовленняКлієнта_Objest.Товари_TablePart.JoinValue;

                foreach (ЗамовленняКлієнта_Товари_TablePart.Record record in ЗамовленняКлієнта_Objest.Товари_TablePart.Records)
                {
                    string uid = record.UID.ToString();

                    record.Номенклатура.Назва = JoinValue[uid]["Номенклатура"];
                    record.ХарактеристикаНоменклатури.Назва = JoinValue[uid]["Характеристика"];
                    record.Пакування.Назва = JoinValue[uid]["Пакування"];
                    record.ВидЦіни.Назва = JoinValue[uid]["ВидЦін"];
                    record.Склад.Назва = JoinValue[uid]["Склад"];

                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        НомерРядка = record.НомерРядка,
                        Номенклатура = record.Номенклатура,
                        Характеристика = record.ХарактеристикаНоменклатури,
                        КількістьУпаковок = record.КількістьУпаковок,
                        Пакування = record.Пакування,
                        Кількість = record.Кількість,
                        КількістьФакт = record.Кількість * record.КількістьУпаковок,
                        ВидЦіни = record.ВидЦіни,
                        Ціна = record.Ціна,
                        Сума = record.Сума,
                        Скидка = record.Скидка,
                        Склад = record.Склад
                    };

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }
        }

        public override async ValueTask SaveRecords()
        {
            if (ЗамовленняКлієнта_Objest != null)
            {
                ЗамовленняКлієнта_Objest.Товари_TablePart.Records.Clear();

                int sequenceNumber = 0;

                foreach (Запис запис in Записи)
                {
                    ЗамовленняКлієнта_Товари_TablePart.Record record = new ЗамовленняКлієнта_Товари_TablePart.Record
                    {
                        UID = запис.ID,
                        НомерРядка = ++sequenceNumber,
                        Номенклатура = запис.Номенклатура,
                        ХарактеристикаНоменклатури = запис.Характеристика,
                        КількістьУпаковок = запис.КількістьУпаковок,
                        Пакування = запис.Пакування,
                        Кількість = запис.Кількість,
                        ВидЦіни = запис.ВидЦіни,
                        Ціна = запис.Ціна,
                        Сума = запис.Сума,
                        Скидка = запис.Скидка,
                        Склад = запис.Склад
                    };

                    ЗамовленняКлієнта_Objest.Товари_TablePart.Records.Add(record);
                }

                await ЗамовленняКлієнта_Objest.Товари_TablePart.Save(true);

                await LoadRecords();
            }
        }

        public decimal СумаДокументу()
        {
            decimal Сума = 0;

            foreach (Запис запис in Записи)
                Сума += запис.Сума;

            return Math.Round(Сума, 2);
        }

        public string КлючовіСловаДляПошуку()
        {
            string ключовіСлова = "";

            if (ЗамовленняКлієнта_Objest != null)
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

            //Сума
            {
                CellRendererText Сума = new CellRendererText() { Editable = true };
                Сума.Edited += TextChanged;
                Сума.Data.Add("Column", (int)Columns.Сума);

                TreeViewColumn Column = new TreeViewColumn("Сума", Сума, "text", (int)Columns.Сума) { Resizable = true, MinWidth = 100 };
                Column.SetCellDataFunc(Сума, new TreeCellDataFunc(NumericCellDataFunc));
                TreeViewGrid.AppendColumn(Column);
            }

            //Скидка
            {
                CellRendererText Скидка = new CellRendererText() { Editable = true };
                Скидка.Edited += TextChanged;
                Скидка.Data.Add("Column", (int)Columns.Скидка);

                TreeViewColumn Column = new TreeViewColumn("Скидка", Скидка, "text", (int)Columns.Скидка) { Resizable = true, MinWidth = 100 };
                Column.SetCellDataFunc(Скидка, new TreeCellDataFunc(NumericCellDataFunc));
                TreeViewGrid.AppendColumn(Column);
            }

            //СкладНазва
            {
                TreeViewColumn СкладНазва = new TreeViewColumn("Склад", new CellRendererText(), "text", (int)Columns.Склад) { Resizable = true, MinWidth = 200 };
                СкладНазва.Data.Add("Column", Columns.Склад);

                TreeViewGrid.AppendColumn(СкладНазва);
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
                            await Запис.ОтриматиЦіну(запис);

                            Store.SetValues(iter, запис.ToArray());
                        };

                        popoverSmallSelect.Add(page);
                        popoverSmallSelect.ShowAll();

                        await page.LoadRecords();
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
                            await Запис.ОтриматиЦіну(запис);

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
                case Columns.ВидЦіни:
                    {
                        ВидиЦін_ШвидкийВибір page = new ВидиЦін_ШвидкийВибір() { PopoverParent = popoverSmallSelect, DirectoryPointerItem = запис.ВидЦіни.UnigueID };
                        page.CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                        {
                            запис.ВидЦіни = new ВидиЦін_Pointer(selectPointer);
                            await Запис.ПісляЗміни_ВидЦіни(запис);
                            await Запис.ОтриматиЦіну(запис);

                            Store.SetValues(iter, запис.ToArray());
                        };

                        popoverSmallSelect.Add(page);
                        popoverSmallSelect.ShowAll();

                        await page.LoadRecords();
                        break;
                    }
                case Columns.Склад:
                    {
                        Склади_ШвидкийВибір page = new Склади_ШвидкийВибір() { PopoverParent = popoverSmallSelect, DirectoryPointerItem = запис.Склад.UnigueID };
                        page.CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                        {
                            запис.Склад = new Склади_Pointer(selectPointer);
                            await Запис.ПісляЗміни_Склад(запис);

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
                case Columns.Склад:
                    {
                        запис.Склад.Clear();
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
                    case Columns.Ціна:
                        {
                            cellText.Text = запис.Ціна.ToString();
                            break;
                        }
                    case Columns.Сума:
                        {
                            cellText.Text = запис.Сума.ToString();
                            break;
                        }
                    case Columns.Скидка:
                        {
                            cellText.Text = запис.Скидка.ToString();
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
                    case Columns.КількістьУпаковок:
                        {
                            var (check, value) = Validate.IsInt(args.NewText);
                            if (check)
                            {
                                if (value <= 0) value = 1;

                                запис.КількістьУпаковок = value;
                                Запис.ПісляЗміни_КількістьАбоЦіна(запис);
                            }

                            break;
                        }
                    case Columns.Кількість:
                        {
                            var (check, value) = Validate.IsDecimal(args.NewText);
                            if (check)
                            {
                                запис.Кількість = value;
                                Запис.ПісляЗміни_КількістьАбоЦіна(запис);
                            }

                            break;
                        }
                    case Columns.Ціна:
                        {
                            var (check, value) = Validate.IsDecimal(args.NewText);
                            if (check)
                            {
                                запис.Ціна = value;
                                Запис.ПісляЗміни_КількістьАбоЦіна(запис);
                            }

                            break;
                        }
                    case Columns.Сума:
                        {
                            var (check, value) = Validate.IsDecimal(args.NewText);
                            if (check)
                                запис.Сума = value;

                            break;
                        }
                    case Columns.Скидка:
                        {
                            var (check, value) = Validate.IsDecimal(args.NewText);
                            if (check)
                                запис.Скидка = value;

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

            await Запис.ПісляДодаванняНового(запис);

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