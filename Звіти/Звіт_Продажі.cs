
/*
        Звіт_Продажі.cs
*/

using Gtk;
using InterfaceGtk;

using StorageAndTrade_1_0;
using Константи = StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Перелічення;
using StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade
{
    class Звіт_Продажі : ФормаЗвіт
    {
        #region Filters

        CheckButton ГрупуватиПоПеріоду = new CheckButton("Групувати по періоду (День)");
        CheckButton СобівартістьЗакупки = new CheckButton("Собівартість закупки");

        Організації_PointerControl Організація = new Організації_PointerControl();
        Контрагенти_PointerControl Контрагент = new Контрагенти_PointerControl();
        Контрагенти_Папки_PointerControl Контрагент_Папка = new Контрагенти_Папки_PointerControl() { Caption = "Контрагент папка:" };
        ДоговориКонтрагентів_PointerControl ДоговірКонтрагента = new ДоговориКонтрагентів_PointerControl() { Caption = "Договір:" };
        Номенклатура_PointerControl Номенклатура = new Номенклатура_PointerControl();
        Номенклатура_Папки_PointerControl Номенклатура_Папка = new Номенклатура_Папки_PointerControl() { Caption = "Номенклатура папка:" };
        ХарактеристикиНоменклатури_PointerControl ХарактеристикиНоменклатури = new ХарактеристикиНоменклатури_PointerControl();
        Склади_PointerControl Склад = new Склади_PointerControl();
        Склади_Папки_PointerControl Склад_Папка = new Склади_Папки_PointerControl() { Caption = "Склад папка:" };
        ComboBoxText ТипНоменклатури = new ComboBoxText();
        Switch ТипНоменклатури_Включено = new Switch();
        ВидиНоменклатури_PointerControl ВидНоменклатури = new ВидиНоменклатури_PointerControl() { Caption = "Вид:" };

        struct ПараметриФільтр
        {
            public DateTime ДатаПочатокПеріоду;
            public DateTime ДатаКінецьПеріоду;
            public bool ГрупуватиПоПеріоду;
            public bool СобівартістьЗакупки;
            public Організації_Pointer Організація;
            public Контрагенти_Pointer Контрагент;
            public Контрагенти_Папки_Pointer Контрагент_Папка;
            public ДоговориКонтрагентів_Pointer ДоговірКонтрагента;
            public Номенклатура_Pointer Номенклатура;
            public Номенклатура_Папки_Pointer Номенклатура_Папка;
            public ХарактеристикиНоменклатури_Pointer ХарактеристикиНоменклатури;
            public Склади_Pointer Склад;
            public Склади_Папки_Pointer Склад_Папка;
            public ТипиНоменклатури ТипНоменклатури;
            public bool ТипНоменклатури_Включено;
            public ВидиНоменклатури_Pointer ВидНоменклатури;
        }

        #endregion

        public Звіт_Продажі() 
        {
            //Кнопки
            Button bOborot = new Button("Обороти");
            bOborot.Clicked += (object? sender, EventArgs args) => Обороти();
            HBoxTop.PackStart(bOborot, false, false, 10);

            ShowAll();
        }

        #region Filters

        protected override void CreateContainer1(Box vBox)
        {
            //Організація
            CreateField(vBox, null, Організація);

            //Склад
            CreateField(vBox, null, Склад);

            //Контрагент
            CreateField(vBox, null, Контрагент);

            //ДоговірКонтрагента
            CreateField(vBox, null, ДоговірКонтрагента);
            ДоговірКонтрагента.BeforeClickOpenFunc = () => ДоговірКонтрагента.КонтрагентВласник = Контрагент.Pointer;

            //Номенклатура
            CreateField(vBox, null, Номенклатура);

            //ХарактеристикиНоменклатури
            CreateField(vBox, null, ХарактеристикиНоменклатури);
            ХарактеристикиНоменклатури.BeforeClickOpenFunc = () => ХарактеристикиНоменклатури.Власник = Номенклатура.Pointer;
        }

        protected override void CreateContainer2(Box vBox)
        {
            //ГрупуватиПоПеріоду
            CreateField(vBox, null, ГрупуватиПоПеріоду);

            //СобівартістьЗакупки
            CreateField(vBox, null, СобівартістьЗакупки);

            //Склад папка
            CreateField(vBox, null, Склад_Папка);

            //Контрагент Папка
            CreateField(vBox, null, Контрагент_Папка);

            //Номенклатура папка
            CreateField(vBox, null, Номенклатура_Папка);

            //ТипНоменклатури
            Box hBoxTypNomenklatury = CreateField(vBox, "Тип:", ТипНоменклатури);
            CreateField(hBoxTypNomenklatury, null, ТипНоменклатури_Включено, Align.End, true);

            foreach (var field in ПсевдонімиПерелічення.ТипиНоменклатури_List())
                ТипНоменклатури.Append(field.Value.ToString(), field.Name);
            ТипНоменклатури.Active = 0;

            //ВидНоменклатури
            CreateField(vBox, null, ВидНоменклатури);
        }

        #endregion

        #region Period

        const string КлючНалаштуванняКористувача = "Звіт.Продажі";

        public override async ValueTask SetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період);
        }

        protected override void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
        }

        #endregion

        ПараметриФільтр СформуватиФільтр()
        {
            return new ПараметриФільтр()
            {
                ДатаПочатокПеріоду = Період.DateStartControl.ПочатокДня(),
                ДатаКінецьПеріоду = Період.DateStopControl.КінецьДня(),
                ГрупуватиПоПеріоду = ГрупуватиПоПеріоду.Active,
                СобівартістьЗакупки = СобівартістьЗакупки.Active,
                Організація = Організація.Pointer,
                Контрагент = Контрагент.Pointer,
                Контрагент_Папка = Контрагент_Папка.Pointer,
                ДоговірКонтрагента = ДоговірКонтрагента.Pointer,
                Номенклатура = Номенклатура.Pointer,
                Номенклатура_Папка = Номенклатура_Папка.Pointer,
                ХарактеристикиНоменклатури = ХарактеристикиНоменклатури.Pointer,
                Склад = Склад.Pointer,
                Склад_Папка = Склад_Папка.Pointer,
                ТипНоменклатури = Enum.Parse<ТипиНоменклатури>(ТипНоменклатури.ActiveId),
                ТипНоменклатури_Включено = ТипНоменклатури_Включено.Active,
                ВидНоменклатури = ВидНоменклатури.Pointer
            };
        }

        async ValueTask<string> ВідобразитиФільтр(string typeReport, ПараметриФільтр Фільтр)
        {
            string text = "";

            text += "З <b>" +
                Фільтр.ДатаПочатокПеріоду.ToString("dd.MM.yyyy") + "</b> по <b>" +
                Фільтр.ДатаКінецьПеріоду.ToString("dd.MM.yyyy") + "</b>; ";

            if (!Фільтр.Організація.IsEmpty())
                text += "Організація: <b>" + await Фільтр.Організація.GetPresentation() + "</b>; ";

            if (!Фільтр.Контрагент.IsEmpty())
                text += "Контрагент: <b>" + await Фільтр.Контрагент.GetPresentation() + "</b>; ";

            if (!Фільтр.ДоговірКонтрагента.IsEmpty())
                text += "Договір: <b>" + await Фільтр.ДоговірКонтрагента.GetPresentation() + "</b>; ";

            if (!Фільтр.Контрагент_Папка.IsEmpty())
                text += "Контрагент папка: <b>" + await Фільтр.Контрагент_Папка.GetPresentation() + "</b>; ";

            if (!Фільтр.Номенклатура.IsEmpty())
                text += "Номенклатура: <b>" + await Фільтр.Номенклатура.GetPresentation() + "</b>; ";

            if (!Фільтр.Номенклатура_Папка.IsEmpty())
                text += "Номенклатура папка: <b>" + await Фільтр.Номенклатура_Папка.GetPresentation() + "</b>; ";

            if (!Фільтр.ХарактеристикиНоменклатури.IsEmpty())
                text += "Характеристика: <b>" + await Фільтр.ХарактеристикиНоменклатури.GetPresentation() + "</b>; ";

            if (!Фільтр.Склад.IsEmpty())
                text += "Склад: <b>" + await Фільтр.Склад.GetPresentation() + "</b>; ";

            if (!Фільтр.Склад_Папка.IsEmpty())
                text += "Склад папка: <b>" + await Фільтр.Склад_Папка.GetPresentation() + "</b>; ";

            if (Фільтр.ТипНоменклатури_Включено)
                text += "Тип: <b>" + Фільтр.ТипНоменклатури.ToString() + "</b>; ";

            if (!Фільтр.ВидНоменклатури.IsEmpty())
                text += "Вид: <b>" + await Фільтр.ВидНоменклатури.GetPresentation() + "</b>; ";

            return text;
        }

        async void Обороти()
        {
            ПараметриФільтр Фільтр = СформуватиФільтр();

            #region SELECT

            bool isExistParent = true;

            string query = $@"
SELECT 
" + (Фільтр.ГрупуватиПоПеріоду ? $@"Продажі.{Продажі_Обороти_TablePart.Період} AS Період, TO_CHAR(Продажі.{Продажі_Обороти_TablePart.Період}, 'dd.mm.yyyy') AS Період_Назва," : "") + $@" 
    Продажі.{Продажі_Обороти_TablePart.Організація} AS Організація,
    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва,
    Продажі.{Продажі_Обороти_TablePart.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Продажі.{Продажі_Обороти_TablePart.Контрагент} AS Контрагент,
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS Контрагент_Назва,
    Продажі.{Продажі_Обороти_TablePart.Договір} AS Договір,
    CONCAT(Довідник_ДоговориКонтрагентів.{ДоговориКонтрагентів_Const.Назва}, ' ', 
           Довідник_ДоговориКонтрагентів.{ДоговориКонтрагентів_Const.ТипДоговоруПредставлення}) AS Договір_Назва,
    Продажі.{Продажі_Обороти_TablePart.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    Продажі.{Продажі_Обороти_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    Довідник_ПакуванняОдиниціВиміру.uid AS ОдиницяВиміру,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS ОдиницяВиміру_Назва,
    ROUND(SUM(Продажі.{Продажі_Обороти_TablePart.Кількість}), 2) AS Кількість,
    ROUND(SUM(Продажі.{Продажі_Обороти_TablePart.Сума}), 2) AS Сума,
    ROUND(SUM(Продажі.{Продажі_Обороти_TablePart.Дохід}), 2) AS Дохід" +
    (Фільтр.СобівартістьЗакупки ? $", Продажі.{Продажі_Обороти_TablePart.Собівартість} AS Собівартість" : "") + $@"
FROM 
    {Продажі_Обороти_TablePart.TABLE} AS Продажі

    LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = 
        Продажі.{Продажі_Обороти_TablePart.Організація}
    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
        Продажі.{Продажі_Обороти_TablePart.Склад}
    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Продажі.{Продажі_Обороти_TablePart.Контрагент}
    LEFT JOIN {ДоговориКонтрагентів_Const.TABLE} AS Довідник_ДоговориКонтрагентів ON Довідник_ДоговориКонтрагентів.uid = 
        Продажі.{Продажі_Обороти_TablePart.Договір}
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        Продажі.{Продажі_Обороти_TablePart.Номенклатура}
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        Продажі.{Продажі_Обороти_TablePart.ХарактеристикаНоменклатури}
    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON Довідник_ПакуванняОдиниціВиміру.uid = 
        Довідник_Номенклатура.{Номенклатура_Const.ОдиницяВиміру}
WHERE
    Продажі.{Продажі_Обороти_TablePart.Період} >= @ПочатокПеріоду AND
    Продажі.{Продажі_Обороти_TablePart.Період} <= @КінецьПеріоду" +
    (Фільтр.ТипНоменклатури_Включено ? $@" AND Довідник_Номенклатура.{Номенклатура_Const.ТипНоменклатури} = @ТипНоменклатури" : "") + $@"
";

            #region WHERE

            //Відбір по вибраному елементу Організація
            if (!Фільтр.Організація.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Організації.uid = '{Фільтр.Організація.UnigueID}'
";
            }

            //Відбір по всіх вкладених папках вибраної папки Склади
            if (!Фільтр.Склад_Папка.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Склади.{Склади_Const.Папка} IN 
(
    WITH RECURSIVE r AS 
    (
        SELECT uid
        FROM {Склади_Папки_Const.TABLE}
        WHERE {Склади_Папки_Const.TABLE}.uid = '{Фільтр.Склад_Папка.UnigueID}' 
        UNION ALL
        SELECT {Склади_Папки_Const.TABLE}.uid
        FROM {Склади_Папки_Const.TABLE}
            JOIN r ON {Склади_Папки_Const.TABLE}.{Склади_Папки_Const.Родич} = r.uid
    ) SELECT uid FROM r
)
";
            }

            //Відбір по вибраному елементу Склади
            if (!Фільтр.Склад.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Склади.uid = '{Фільтр.Склад.UnigueID}'
";
            }

            //Відбір по всіх вкладених папках вибраної папки Контрагент
            if (!Фільтр.Контрагент_Папка.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Контрагенти.{Контрагенти_Const.Папка} IN 
(
    WITH RECURSIVE r AS 
    (
        SELECT uid
        FROM {Контрагенти_Папки_Const.TABLE}
        WHERE {Контрагенти_Папки_Const.TABLE}.uid = '{Фільтр.Контрагент_Папка.UnigueID}' 
        UNION ALL
        SELECT {Контрагенти_Папки_Const.TABLE}.uid
        FROM {Контрагенти_Папки_Const.TABLE}
            JOIN r ON {Контрагенти_Папки_Const.TABLE}.{Контрагенти_Папки_Const.Родич} = r.uid
    ) SELECT uid FROM r
)
";
            }

            //Відбір по вибраному елементу Контрагент
            if (!Фільтр.Контрагент.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Контрагенти.uid = '{Фільтр.Контрагент.UnigueID}'
";
            }

            //Відбір по вибраному елементу ДоговірКонтрагента
            if (!Фільтр.ДоговірКонтрагента.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_ДоговориКонтрагентів.uid = '{Фільтр.ДоговірКонтрагента.UnigueID}'
";
            }

            //Відбір по всіх вкладених папках вибраної папки Номенклатури
            if (!Фільтр.Номенклатура_Папка.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Номенклатура.{Номенклатура_Const.Папка} IN 
(
    WITH RECURSIVE r AS 
    (
        SELECT uid
        FROM {Номенклатура_Папки_Const.TABLE}
        WHERE {Номенклатура_Папки_Const.TABLE}.uid = '{Фільтр.Номенклатура_Папка.UnigueID}' 
        UNION ALL
        SELECT {Номенклатура_Папки_Const.TABLE}.uid
        FROM {Номенклатура_Папки_Const.TABLE}
            JOIN r ON {Номенклатура_Папки_Const.TABLE}.{Номенклатура_Папки_Const.Родич} = r.uid
    ) SELECT uid FROM r
)
";
            }

            //Відбір по вибраному елементу Номенклатура
            if (!Фільтр.Номенклатура.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Номенклатура.uid = '{Фільтр.Номенклатура.UnigueID}'
";
            }

            //Відбір по вибраному елементу Характеристики Номенклатури
            if (!Фільтр.ХарактеристикиНоменклатури.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_ХарактеристикиНоменклатури.uid = '{Фільтр.ХарактеристикиНоменклатури.UnigueID}'
";
            }

            //Відбір по вибраному елементу ВидиНоменклатури
            if (!Фільтр.ВидНоменклатури.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Номенклатура.{Номенклатура_Const.ВидНоменклатури} = '{Фільтр.ВидНоменклатури.UnigueID}'
";
            }

            #endregion

            query += @"
GROUP BY 
    GROUPING SETS (
    (" +
        (Фільтр.ГрупуватиПоПеріоду ? "Період, Період_Назва, " : "") + @"
        Організація, Організація_Назва,
        Склад, Склад_Назва,
        Контрагент, Контрагент_Назва,
        Договір, Договір_Назва,
        Номенклатура, Номенклатура_Назва,
        ХарактеристикаНоменклатури, ХарактеристикаНоменклатури_Назва,
        ОдиницяВиміру, ОдиницяВиміру_Назва" +
        (Фільтр.СобівартістьЗакупки ? ", Собівартість" : "") + @$"
    ), ())

HAVING
    SUM(Продажі.{Продажі_Обороти_TablePart.Кількість}) != 0 OR
    SUM(Продажі.{Продажі_Обороти_TablePart.Сума}) != 0 OR
    SUM(Продажі.{Продажі_Обороти_TablePart.Дохід}) != 0 

ORDER BY " +
    (Фільтр.ГрупуватиПоПеріоду ? "Період," : "") + @"
    Організація_Назва, 
    Склад_Назва, 
    Контрагент_Назва, 
    Договір_Назва, 
    Номенклатура_Назва, 
    ХарактеристикаНоменклатури_Назва
";
            #endregion

            Dictionary<string, object> paramQuery = new Dictionary<string, object>
            {
                { "ПочатокПеріоду", Фільтр.ДатаПочатокПеріоду },
                { "КінецьПеріоду", Фільтр.ДатаКінецьПеріоду },
                { "ТипНоменклатури", (int)Фільтр.ТипНоменклатури }
            };

            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "Продажі",
                Caption = "Обороти",
                Query = query,
                ParamQuery = paramQuery,
                GetInfo = () => ВідобразитиФільтр("Обороти", Фільтр)
            };

            Звіт.ColumnSettings.Add("Період_Назва", new("Період"));
            Звіт.ColumnSettings.Add("Організація_Назва", new("Організація", "Організація", Організації_Const.POINTER));
            Звіт.ColumnSettings.Add("Склад_Назва", new("Склад", "Склад", Склади_Const.POINTER));
            Звіт.ColumnSettings.Add("Контрагент_Назва", new("Контрагент", "Контрагент", Контрагенти_Const.POINTER));
            Звіт.ColumnSettings.Add("Договір_Назва", new("Договір", "Договір", ДоговориКонтрагентів_Const.POINTER));
            Звіт.ColumnSettings.Add("Номенклатура_Назва", new("Номенклатура", "Номенклатура", Номенклатура_Const.POINTER));

            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                Звіт.ColumnSettings.Add("ХарактеристикаНоменклатури_Назва", new("Характеристика", "ХарактеристикаНоменклатури", ХарактеристикиНоменклатури_Const.POINTER));

            Звіт.ColumnSettings.Add("ОдиницяВиміру_Назва", new("Пакування", "ОдиницяВиміру", ПакуванняОдиниціВиміру_Const.POINTER));
            Звіт.ColumnSettings.Add("Кількість", new("Кількість", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиВідємнеЧислоЧервоним));
            Звіт.ColumnSettings.Add("Сума", new("Сума", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиВідємнеЧислоЧервоним));
            Звіт.ColumnSettings.Add("Дохід", new("Дохід", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиВідємнеЧислоЧервоним));
            Звіт.ColumnSettings.Add("Собівартість", new("Собівартість", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиВідємнеЧислоЧервоним));

            await Звіт.Select();

            Звіт.FillTreeView();
            await Звіт.View(Notebook);
        }

    }
}