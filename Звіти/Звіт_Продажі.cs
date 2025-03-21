
/*
        Звіт_Продажі.cs
*/

using Gtk;
using InterfaceGtk;

using GeneratedCode;
using Константи = GeneratedCode.Константи;
using GeneratedCode.Довідники;
using GeneratedCode.Перелічення;
using GeneratedCode.РегістриНакопичення;

namespace StorageAndTrade
{
    class Звіт_Продажі : ФормаЗвіт
    {
        #region Filters

        CheckButton ГрупуватиПоПеріоду = new CheckButton("Групувати по періоду (День)");
        CheckButton СобівартістьЗакупки = new CheckButton("Собівартість закупки");

        Організації_PointerControl Організація = new Організації_PointerControl();
        Контрагенти_MultiplePointerControl Контрагент = new Контрагенти_MultiplePointerControl();
        Контрагенти_Папки_MultiplePointerControl Контрагент_Папка = new Контрагенти_Папки_MultiplePointerControl() { Caption = "Контрагент папка:" };
        ДоговориКонтрагентів_PointerControl ДоговірКонтрагента = new ДоговориКонтрагентів_PointerControl() { Caption = "Договір:" };
        Номенклатура_MultiplePointerControl Номенклатура = new Номенклатура_MultiplePointerControl();
        Номенклатура_Папки_MultiplePointerControl Номенклатура_Папка = new Номенклатура_Папки_MultiplePointerControl() { Caption = "Номенклатура папка:" };
        ХарактеристикиНоменклатури_MultiplePointerControl ХарактеристикиНоменклатури = new ХарактеристикиНоменклатури_MultiplePointerControl();
        Склади_MultiplePointerControl Склад = new Склади_MultiplePointerControl();
        Склади_Папки_MultiplePointerControl Склад_Папка = new Склади_Папки_MultiplePointerControl() { Caption = "Склад папка:" };
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
            public Контрагенти_Pointer[] Контрагент;
            public Контрагенти_Папки_Pointer[] Контрагент_Папка;
            public ДоговориКонтрагентів_Pointer ДоговірКонтрагента;
            public Номенклатура_Pointer[] Номенклатура;
            public Номенклатура_Папки_Pointer[] Номенклатура_Папка;
            public ХарактеристикиНоменклатури_Pointer[] ХарактеристикиНоменклатури;
            public Склади_Pointer[] Склад;
            public Склади_Папки_Pointer[] Склад_Папка;
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
                Контрагент = Контрагент.GetPointers(),
                Контрагент_Папка = Контрагент_Папка.GetPointers(),
                ДоговірКонтрагента = ДоговірКонтрагента.Pointer,
                Номенклатура = Номенклатура.GetPointers(),
                Номенклатура_Папка = Номенклатура_Папка.GetPointers(),
                ХарактеристикиНоменклатури = ХарактеристикиНоменклатури.GetPointers(),
                Склад = Склад.GetPointers(),
                Склад_Папка = Склад_Папка.GetPointers(),
                ТипНоменклатури = Enum.Parse<ТипиНоменклатури>(ТипНоменклатури.ActiveId),
                ТипНоменклатури_Включено = ТипНоменклатури_Включено.Active,
                ВидНоменклатури = ВидНоменклатури.Pointer
            };
        }

        async ValueTask<string> ВідобразитиФільтр(string typeReport, ПараметриФільтр Фільтр)
        {
            string text = "";

            text += "З " +
                Фільтр.ДатаПочатокПеріоду.ToString("dd.MM.yyyy") + " по " +
                Фільтр.ДатаКінецьПеріоду.ToString("dd.MM.yyyy") + "; ";

            if (!Фільтр.Організація.IsEmpty())
                text += "Організація: " + await Фільтр.Організація.GetPresentation() + "; ";

            if (Фільтр.Контрагент.Length > 0)
            {
                foreach (var item in Фільтр.Контрагент)
                    await item.GetPresentation();

                text += "Контрагент: " + string.Join(", ", Фільтр.Контрагент.Select(x => x.Назва)) + "; ";
            }

            if (!Фільтр.ДоговірКонтрагента.IsEmpty())
                text += "Договір: " + await Фільтр.ДоговірКонтрагента.GetPresentation() + "; ";

            if (Фільтр.Контрагент_Папка.Length > 0)
            {
                foreach (var item in Фільтр.Контрагент_Папка)
                    await item.GetPresentation();

                text += "Контрагент папка: " + string.Join(", ", Фільтр.Контрагент_Папка.Select(x => x.Назва)) + "; ";
            }

            if (Фільтр.Номенклатура.Length > 0)
            {
                foreach (var item in Фільтр.Номенклатура)
                    await item.GetPresentation();

                text += "Номенклатура: " + string.Join(", ", Фільтр.Номенклатура.Select(x => x.Назва)) + "; ";
            }

            if (Фільтр.Номенклатура_Папка.Length > 0)
            {
                foreach (var item in Фільтр.Номенклатура_Папка)
                    await item.GetPresentation();

                text += "Номенклатура папка: " + string.Join(", ", Фільтр.Номенклатура_Папка.Select(x => x.Назва)) + "; ";
            }

            if (Фільтр.ХарактеристикиНоменклатури.Length > 0)
            {
                foreach (var item in Фільтр.ХарактеристикиНоменклатури)
                    await item.GetPresentation();

                text += "Характеристика: " + string.Join(", ", Фільтр.ХарактеристикиНоменклатури.Select(x => x.Назва)) + "; ";
            }

            if (Фільтр.Склад.Length > 0)
            {
                foreach (var item in Фільтр.Склад)
                    await item.GetPresentation();

                text += "Склад: " + string.Join(", ", Фільтр.Склад.Select(x => x.Назва)) + "; ";
            }

            if (Фільтр.Склад_Папка.Length > 0)
            {
                foreach (var item in Фільтр.Склад_Папка)
                    await item.GetPresentation();

                text += "Склад папка: " + string.Join(", ", Фільтр.Склад_Папка.Select(x => x.Назва)) + "; ";
            }

            if (Фільтр.ТипНоменклатури_Включено)
                text += "Тип: " + Фільтр.ТипНоменклатури.ToString() + "; ";

            if (!Фільтр.ВидНоменклатури.IsEmpty())
                text += "Вид: " + await Фільтр.ВидНоменклатури.GetPresentation() + "; ";

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
            if (Фільтр.Склад_Папка.Length > 0)
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
        WHERE {Склади_Папки_Const.TABLE}.uid IN ('{string.Join("', '", Фільтр.Склад_Папка.Select(x => x.UnigueID.UGuid))}')
        UNION ALL
        SELECT {Склади_Папки_Const.TABLE}.uid
        FROM {Склади_Папки_Const.TABLE}
            JOIN r ON {Склади_Папки_Const.TABLE}.{Склади_Папки_Const.Родич} = r.uid
    ) SELECT uid FROM r
)
";
            }

            //Відбір по вибраному елементу Склади
            if (Фільтр.Склад.Length > 0)
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Склади.uid IN ('{string.Join("', '", Фільтр.Склад.Select(x => x.UnigueID.UGuid))}')
";
            }

            //Відбір по всіх вкладених папках вибраної папки Контрагент
            if (Фільтр.Контрагент_Папка.Length > 0)
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
        WHERE {Контрагенти_Папки_Const.TABLE}.uid IN ('{string.Join("', '", Фільтр.Контрагент_Папка.Select(x => x.UnigueID.UGuid))}')
        UNION ALL
        SELECT {Контрагенти_Папки_Const.TABLE}.uid
        FROM {Контрагенти_Папки_Const.TABLE}
            JOIN r ON {Контрагенти_Папки_Const.TABLE}.{Контрагенти_Папки_Const.Родич} = r.uid
    ) SELECT uid FROM r
)
";
            }

            //Відбір по вибраному елементу Контрагент
            if (Фільтр.Контрагент.Length > 0)
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Контрагенти.uid IN ('{string.Join("', '", Фільтр.Контрагент.Select(x => x.UnigueID.UGuid))}')
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
            if (Фільтр.Номенклатура_Папка.Length > 0)
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
        WHERE {Номенклатура_Папки_Const.TABLE}.uid IN ('{string.Join("', '", Фільтр.Номенклатура_Папка.Select(x => x.UnigueID.UGuid))}') 
        UNION ALL
        SELECT {Номенклатура_Папки_Const.TABLE}.uid
        FROM {Номенклатура_Папки_Const.TABLE}
            JOIN r ON {Номенклатура_Папки_Const.TABLE}.{Номенклатура_Папки_Const.Родич} = r.uid
    ) SELECT uid FROM r
)
";
            }

            //Відбір по вибраному елементу Номенклатура
            if (Фільтр.Номенклатура.Length > 0)
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Номенклатура.uid IN ('{string.Join("', '", Фільтр.Номенклатура.Select(x => x.UnigueID.UGuid))}') 
";
            }

            //Відбір по вибраному елементу Характеристики Номенклатури
            if (Фільтр.ХарактеристикиНоменклатури.Length > 0)
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_ХарактеристикиНоменклатури.uid IN ('{string.Join("', '", Фільтр.ХарактеристикиНоменклатури.Select(x => x.UnigueID.UGuid))}') 
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

            //PDF
            {
                Звіт.PDFColumnSettings.Add("Період_Назва", new("Період"));
                Звіт.PDFColumnSettings.Add("Організація_Назва", new("Організація", 2));
                Звіт.PDFColumnSettings.Add("Склад_Назва", new("Склад"));
                Звіт.PDFColumnSettings.Add("Контрагент_Назва", new("Контрагент", 2));
                //Звіт.PDFColumnSettings.Add("Договір_Назва", new("Договір"));
                Звіт.PDFColumnSettings.Add("Номенклатура_Назва", new("Номенклатура", 5));

                if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                    Звіт.PDFColumnSettings.Add("ХарактеристикаНоменклатури_Назва", new("Характеристика", 4));

                Звіт.PDFColumnSettings.Add("ОдиницяВиміру_Назва", new("Пакування"));
                Звіт.PDFColumnSettings.Add("Кількість", new("Кількість", 40, ЗвітСторінка.TypePDFColumn.Constant, 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
                Звіт.PDFColumnSettings.Add("Сума", new("Сума", 40, ЗвітСторінка.TypePDFColumn.Constant, 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
                Звіт.PDFColumnSettings.Add("Дохід", new("Дохід", 40, ЗвітСторінка.TypePDFColumn.Constant, 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
                Звіт.PDFColumnSettings.Add("Собівартість", new("Собівартість", 40, ЗвітСторінка.TypePDFColumn.Constant, 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
            }

            await Звіт.Select();

            Звіт.FillTreeView();
            await Звіт.View(Notebook);
        }
    }
}