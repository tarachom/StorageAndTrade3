
/*
        Звіт_ТовариВКоміркахНаСкладах.cs
*/

using Gtk;
using InterfaceGtk;

using Константи = GeneratedCode.Константи;
using GeneratedCode.Довідники;
using GeneratedCode.РегістриНакопичення;

namespace StorageAndTrade
{
    class Звіт_ТовариВКоміркахНаСкладах : ФормаЗвіт
    {
        #region Filters

        Номенклатура_MultiplePointerControl Номенклатура = new Номенклатура_MultiplePointerControl();
        Номенклатура_Папки_MultiplePointerControl Номенклатура_Папка = new Номенклатура_Папки_MultiplePointerControl() { Caption = "Номенклатура папка:" };
        ХарактеристикиНоменклатури_MultiplePointerControl ХарактеристикиНоменклатури = new ХарактеристикиНоменклатури_MultiplePointerControl();
        Склади_MultiplePointerControl Склад = new Склади_MultiplePointerControl();
        Склади_Папки_MultiplePointerControl Склад_Папка = new Склади_Папки_MultiplePointerControl() { Caption = "Склад папка:" };
        СкладськіПриміщення_PointerControl СкладськеПриміщення = new СкладськіПриміщення_PointerControl() { Caption = "Приміщення:" };
        СкладськіКомірки_PointerControl СкладськаКомірка = new СкладськіКомірки_PointerControl() { Caption = "Комірка:" };
        СеріїНоменклатури_PointerControl Серія = new СеріїНоменклатури_PointerControl();

        struct ПараметриФільтр
        {
            public DateTime ДатаПочатокПеріоду;
            public DateTime ДатаКінецьПеріоду;
            public Номенклатура_Pointer[] Номенклатура;
            public Номенклатура_Папки_Pointer[] Номенклатура_Папка;
            public ХарактеристикиНоменклатури_Pointer[] ХарактеристикиНоменклатури;
            public Склади_Pointer[] Склад;
            public Склади_Папки_Pointer[] Склад_Папка;
            public СкладськіПриміщення_Pointer СкладськеПриміщення;
            public СкладськіКомірки_Pointer СкладськаКомірка;
            public СеріїНоменклатури_Pointer Серія;
        }

        #endregion

        public Звіт_ТовариВКоміркахНаСкладах()
        {
            //Кнопки
            Button bOstatok = new Button("Залишки");
            bOstatok.Clicked += (object? sender, EventArgs args) => Залишки();
            HBoxTop.PackStart(bOstatok, false, false, 10);

            Button bOborot = new Button("Залишки та обороти");
            bOborot.Clicked += (object? sender, EventArgs args) => ЗалишкиТаОбороти();
            HBoxTop.PackStart(bOborot, false, false, 10);

            Button bDocuments = new Button("Документи");
            bDocuments.Clicked += (object? sender, EventArgs args) => Документи();
            HBoxTop.PackStart(bDocuments, false, false, 10);

            ShowAll();
        }

        #region Filters

        protected override void CreateContainer1(Box vBox)
        {
            //Номенклатура
            CreateField(vBox, null, Номенклатура);

            //ХарактеристикиНоменклатури
            CreateField(vBox, null, ХарактеристикиНоменклатури);
            ХарактеристикиНоменклатури.BeforeClickOpenFunc = () => ХарактеристикиНоменклатури.Власник = Номенклатура.Pointer;

            //Склад
            CreateField(vBox, null, Склад);

            //СкладськеПриміщення
            CreateField(vBox, null, СкладськеПриміщення);
            СкладськеПриміщення.BeforeClickOpenFunc = () => СкладськеПриміщення.СкладВласник = Склад.Pointer;

            //СкладськаКомірка
            CreateField(vBox, null, СкладськаКомірка);
            СкладськаКомірка.BeforeClickOpenFunc = () => СкладськаКомірка.Власник = СкладськеПриміщення.Pointer;
        }

        protected override void CreateContainer2(Box vBox)
        {
            //Номенклатура папка
            CreateField(vBox, null, Номенклатура_Папка);

            //Склад папка
            CreateField(vBox, null, Склад_Папка);

            //Серія
            CreateField(vBox, null, Серія);
        }

        #endregion

        #region Period

        const string КлючНалаштуванняКористувача = "Звіт.ТовариВКоміркахНаСкладах";

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
                Номенклатура = Номенклатура.GetPointers(),
                Номенклатура_Папка = Номенклатура_Папка.GetPointers(),
                ХарактеристикиНоменклатури = ХарактеристикиНоменклатури.GetPointers(),
                Склад = Склад.GetPointers(),
                Склад_Папка = Склад_Папка.GetPointers(),
                СкладськеПриміщення = СкладськеПриміщення.Pointer,
                СкладськаКомірка = СкладськаКомірка.Pointer,
                Серія = Серія.Pointer
            };
        }

        async ValueTask<string> ВідобразитиФільтр(string typeReport, ПараметриФільтр Фільтр)
        {
            string text = "";

            switch (typeReport)
            {
                case "Залишки":
                    {
                        text += "Без періоду; ";
                        break;
                    }
                case "ЗалишкиТаОбороти":
                case "Документи":
                    {
                        text += "З " +
                            Фільтр.ДатаПочатокПеріоду.ToString("dd.MM.yyyy") + " по " +
                            Фільтр.ДатаКінецьПеріоду.ToString("dd.MM.yyyy") + "; ";
                        break;
                    }
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

            if (!Фільтр.СкладськеПриміщення.IsEmpty())
                text += "Складське приміщення: " + await Фільтр.СкладськеПриміщення.GetPresentation() + "; ";

            if (!Фільтр.СкладськаКомірка.IsEmpty())
                text += "Складська комірка: " + await Фільтр.СкладськаКомірка.GetPresentation() + "; ";

            if (!Фільтр.Серія.IsEmpty())
                text += "Серія: " + await Фільтр.Серія.GetPresentation() + "; ";

            return text;
        }

        async void Залишки()
        {
            ПараметриФільтр Фільтр = СформуватиФільтр();

            #region SELECT

            bool isExistParent = false;

            string query = $@"
SELECT 
    ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    Довідник_СкладськіПриміщення.{СкладськіПриміщення_Const.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Довідник_СкладськіКомірки.{СкладськіКомірки_Const.Приміщення} AS Приміщення,
    Довідник_СкладськіПриміщення.{СкладськіПриміщення_Const.Назва} AS Приміщення_Назва,
    ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Комірка} AS Комірка,
    Довідник_СкладськіКомірки.{СкладськіКомірки_Const.Назва} AS Комірка_Назва,
    ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Серія} AS Серія,
    Довідник_СеріїНоменклатури.{СеріїНоменклатури_Const.Номер} AS Серія_Номер,
    ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Пакування} AS ОдиницяВиміру,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS ОдиницяВиміру_Назва,
    ROUND(SUM(ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.ВНаявності}), 2) AS ВНаявності
FROM 
    {ТовариВКомірках_Залишки_TablePart.TABLE} AS ТовариВКомірках
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Номенклатура}
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.ХарактеристикаНоменклатури}
    LEFT JOIN {СкладськіКомірки_Const.TABLE} AS Довідник_СкладськіКомірки ON Довідник_СкладськіКомірки.uid = 
        ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Комірка}
    LEFT JOIN {СкладськіПриміщення_Const.TABLE} AS Довідник_СкладськіПриміщення ON Довідник_СкладськіПриміщення.uid = 
        Довідник_СкладськіКомірки.{СкладськіКомірки_Const.Приміщення}
    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
        Довідник_СкладськіПриміщення.{СкладськіПриміщення_Const.Склад}
    LEFT JOIN {СеріїНоменклатури_Const.TABLE} AS Довідник_СеріїНоменклатури ON Довідник_СеріїНоменклатури.uid = 
        ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Серія}
    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON Довідник_ПакуванняОдиниціВиміру.uid = 
        ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Пакування}
";

            #region WHERE

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

            //Відбір по вибраному елементу СкладськеПриміщення
            if (!Фільтр.СкладськеПриміщення.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_СкладськіПриміщення.uid = '{Фільтр.СкладськеПриміщення.UnigueID}'
";
            }

            //Відбір по вибраному елементу СкладськаКомірка
            if (!Фільтр.СкладськаКомірка.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_СкладськіКомірки.uid = '{Фільтр.СкладськаКомірка.UnigueID}'
";
            }

            //Відбір по вибраному елементу Серії Номенклатури
            if (!Фільтр.Серія.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_СеріїНоменклатури.uid = '{Фільтр.Серія.UnigueID}'
";
            }

            #endregion

            query += $@"
GROUP BY 
    GROUPING SETS (
    (
        Номенклатура, Номенклатура_Назва, 
        ХарактеристикаНоменклатури, ХарактеристикаНоменклатури_Назва,
        Склад, Склад_Назва,
        Приміщення, Приміщення_Назва,
        Комірка, Комірка_Назва,
        Серія, Серія_Номер,
        ОдиницяВиміру, ОдиницяВиміру_Назва
    ), ())
HAVING
    SUM(ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.ВНаявності}) != 0  
ORDER BY 
    Номенклатура_Назва
";
            #endregion

            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "Товари в комірках на складах",
                Caption = "Залишки",
                Query = query,
                GetInfo = () => ВідобразитиФільтр("Залишки", Фільтр)
            };

            Звіт.ColumnSettings.Add("Номенклатура_Назва", new("Номенклатура", "Номенклатура", Номенклатура_Const.POINTER));

            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                Звіт.ColumnSettings.Add("ХарактеристикаНоменклатури_Назва", new("Характеристика", "ХарактеристикаНоменклатури", ХарактеристикиНоменклатури_Const.POINTER));

            if (Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const)
                Звіт.ColumnSettings.Add("Серія_Номер", new("Серія", "Серія", СеріїНоменклатури_Const.POINTER));

            Звіт.ColumnSettings.Add("Склад_Назва", new("Склад", "Склад", Склади_Const.POINTER));
            Звіт.ColumnSettings.Add("Приміщення_Назва", new("Приміщення", "Приміщення", СкладськіПриміщення_Const.POINTER));
            Звіт.ColumnSettings.Add("Комірка_Назва", new("Комірка", "Комірка", СкладськіКомірки_Const.POINTER));
            Звіт.ColumnSettings.Add("ОдиницяВиміру_Назва", new("Пакування", "ОдиницяВиміру", ПакуванняОдиниціВиміру_Const.POINTER));
            Звіт.ColumnSettings.Add("ВНаявності", new("В наявності", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиВідємнеЧислоЧервоним));

            await Звіт.Select();

            Звіт.FillTreeView();
            await Звіт.View(Notebook);
        }

        async void ЗалишкиТаОбороти()
        {
            ПараметриФільтр Фільтр = СформуватиФільтр();

            #region SELECT

            bool isExistParent = false;

            string query = $@"
WITH 
ПочатковийЗалишок AS
(
    SELECT 
        ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Номенклатура} AS Номенклатура, 
        ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Комірка} AS Комірка,
        ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Серія} AS Серія,
        ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Пакування} AS ОдиницяВиміру,
        SUM(ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.ВНаявності}) AS ВНаявності
    FROM 
        {ТовариВКомірках_Залишки_TablePart.TABLE} AS ТовариВКомірках
    WHERE
        ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Період} < @ПочатокПеріоду
    GROUP BY Номенклатура, ХарактеристикаНоменклатури, Комірка, Серія, ОдиницяВиміру
),
ЗалишкиТаОборотиЗаПеріод AS
(
    SELECT 
        ТовариВКомірках.{ТовариВКомірках_ЗалишкиТаОбороти_TablePart.Номенклатура} AS Номенклатура, 
        ТовариВКомірках.{ТовариВКомірках_ЗалишкиТаОбороти_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ТовариВКомірках.{ТовариВКомірках_ЗалишкиТаОбороти_TablePart.Комірка} AS Комірка,
        ТовариВКомірках.{ТовариВКомірках_ЗалишкиТаОбороти_TablePart.Серія} AS Серія,
        ТовариВКомірках.{ТовариВКомірках_ЗалишкиТаОбороти_TablePart.Пакування} AS ОдиницяВиміру,
        SUM(ТовариВКомірках.{ТовариВКомірках_ЗалишкиТаОбороти_TablePart.ВНаявностіПрихід}) AS ВНаявностіПрихід,
        SUM(ТовариВКомірках.{ТовариВКомірках_ЗалишкиТаОбороти_TablePart.ВНаявностіРозхід}) AS ВНаявностіРозхід,
        SUM(ТовариВКомірках.{ТовариВКомірках_ЗалишкиТаОбороти_TablePart.ВНаявностіЗалишок}) AS ВНаявностіЗалишок
    FROM 
        {ТовариВКомірках_ЗалишкиТаОбороти_TablePart.TABLE} AS ТовариВКомірках
    WHERE
        ТовариВКомірках.{ТовариВКомірках_ЗалишкиТаОбороти_TablePart.Період} >= @ПочатокПеріоду AND
        ТовариВКомірках.{ТовариВКомірках_ЗалишкиТаОбороти_TablePart.Період} <= @КінецьПеріоду
    GROUP BY Номенклатура, ХарактеристикаНоменклатури, Комірка, Серія, ОдиницяВиміру
),
КінцевийЗалишок AS
(
    SELECT 
        Номенклатура,
        ХарактеристикаНоменклатури,
        Комірка,
        Серія,
        ОдиницяВиміру,
        ВНаявності AS Залишок
    FROM ПочатковийЗалишок

    UNION ALL

    SELECT
        Номенклатура,
        ХарактеристикаНоменклатури,
        Комірка,
        Серія,
        ОдиницяВиміру,
        ВНаявностіЗалишок AS Залишок
    FROM ЗалишкиТаОборотиЗаПеріод
)
SELECT 
    Номенклатура,
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,

    /* склад, приміщення */
    Довідник_СкладськіПриміщення.{СкладськіПриміщення_Const.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Довідник_СкладськіКомірки.{СкладськіКомірки_Const.Приміщення} AS Приміщення,
    Довідник_СкладськіПриміщення.{СкладськіПриміщення_Const.Назва} AS Приміщення_Назва,

    Комірка,
    Довідник_СкладськіКомірки.{СкладськіКомірки_Const.Назва} AS Комірка_Назва,
    Серія,
    Довідник_СеріїНоменклатури.{СеріїНоменклатури_Const.Номер} AS Серія_Номер,
    ОдиницяВиміру,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS ОдиницяВиміру_Назва,
    ROUND(SUM(ПочатковийЗалишок), 2) AS ПочатковийЗалишок,
    ROUND(SUM(Прихід), 2) AS Прихід,
    ROUND(SUM(Розхід), 2) AS Розхід,
    ROUND(SUM(КінцевийЗалишок), 2) AS КінцевийЗалишок
FROM 
(
    SELECT 
        Номенклатура,
        ХарактеристикаНоменклатури,
        Комірка,
        Серія,
        ОдиницяВиміру,
        ВНаявності AS ПочатковийЗалишок,
        0 AS Прихід,
        0 AS Розхід,
        0 AS КінцевийЗалишок
    FROM ПочатковийЗалишок

    UNION ALL

    SELECT
        Номенклатура,
        ХарактеристикаНоменклатури,
        Комірка,
        Серія,
        ОдиницяВиміру,
        0 AS ПочатковийЗалишок,
        ВНаявностіПрихід AS Прихід,
        ВНаявностіРозхід AS Розхід,
        0 AS КінцевийЗалишок
    FROM ЗалишкиТаОборотиЗаПеріод

    UNION ALL

    SELECT 
        Номенклатура,
        ХарактеристикаНоменклатури,
        Комірка,
        Серія,
        ОдиницяВиміру,
        0 AS ПочатковийЗалишок,
        0 AS Прихід,
        0 AS Розхід,
        Залишок AS КінцевийЗалишок
    FROM КінцевийЗалишок
) AS ЗалишкиТаОбороти
LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = ЗалишкиТаОбороти.Номенклатура
LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = ЗалишкиТаОбороти.ХарактеристикаНоменклатури
LEFT JOIN {СкладськіКомірки_Const.TABLE} AS Довідник_СкладськіКомірки ON Довідник_СкладськіКомірки.uid = ЗалишкиТаОбороти.Комірка

/* склад, приміщення */
LEFT JOIN {СкладськіПриміщення_Const.TABLE} AS Довідник_СкладськіПриміщення ON Довідник_СкладськіПриміщення.uid = Довідник_СкладськіКомірки.{СкладськіКомірки_Const.Приміщення}
LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = Довідник_СкладськіПриміщення.{СкладськіПриміщення_Const.Склад}

LEFT JOIN {СеріїНоменклатури_Const.TABLE} AS Довідник_СеріїНоменклатури ON Довідник_СеріїНоменклатури.uid = ЗалишкиТаОбороти.Серія
LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON Довідник_ПакуванняОдиниціВиміру.uid = ЗалишкиТаОбороти.ОдиницяВиміру
";

            #region WHERE

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

            //Відбір по вибраному елементу СкладськеПриміщення
            if (!Фільтр.СкладськеПриміщення.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_СкладськіПриміщення.uid = '{Фільтр.СкладськеПриміщення.UnigueID}'
";
            }

            //Відбір по вибраному елементу СкладськаКомірка
            if (!Фільтр.СкладськаКомірка.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_СкладськіКомірки.uid = '{Фільтр.СкладськаКомірка.UnigueID}'
";
            }

            //Відбір по вибраному елементу Серії Номенклатури
            if (!Фільтр.Серія.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_СеріїНоменклатури.uid = '{Фільтр.Серія.UnigueID}'
";
            }

            #endregion

            query += @"
GROUP BY 
    GROUPING SETS (
    (
        Номенклатура, Номенклатура_Назва, 
        ХарактеристикаНоменклатури, ХарактеристикаНоменклатури_Назва,
        Склад, Склад_Назва,
        Приміщення, Приміщення_Назва,
        Комірка, Комірка_Назва,
        Серія, Серія_Номер,
        ОдиницяВиміру, ОдиницяВиміру_Назва
    ), ())
HAVING 
    SUM(ПочатковийЗалишок) != 0 OR 
    SUM(Прихід) != 0 OR 
    SUM(Розхід) != 0 OR
    SUM(КінцевийЗалишок) != 0 
ORDER BY 
    Номенклатура_Назва, 
    ХарактеристикаНоменклатури_Назва
";

            #endregion

            Dictionary<string, object> paramQuery = new Dictionary<string, object>
            {
                { "ПочатокПеріоду", Фільтр.ДатаПочатокПеріоду },
                { "КінецьПеріоду", Фільтр.ДатаКінецьПеріоду }
            };

            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "Товари в комірках на складах",
                Caption = "Залишки та обороти",
                Query = query,
                ParamQuery = paramQuery,
                GetInfo = () => ВідобразитиФільтр("ЗалишкиТаОбороти", Фільтр)
            };

            Звіт.ColumnSettings.Add("Номенклатура_Назва", new("Номенклатура", "Номенклатура", Номенклатура_Const.POINTER));

            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                Звіт.ColumnSettings.Add("ХарактеристикаНоменклатури_Назва", new("Характеристика", "ХарактеристикаНоменклатури", ХарактеристикиНоменклатури_Const.POINTER));

            if (Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const)
                Звіт.ColumnSettings.Add("Серія_Номер", new("Серія", "Серія", СеріїНоменклатури_Const.POINTER));

            Звіт.ColumnSettings.Add("Склад_Назва", new("Склад", "Склад", Склади_Const.POINTER));
            Звіт.ColumnSettings.Add("Приміщення_Назва", new("Приміщення", "Приміщення", СкладськіПриміщення_Const.POINTER));
            Звіт.ColumnSettings.Add("Комірка_Назва", new("Комірка", "Комірка", СкладськіКомірки_Const.POINTER));
            Звіт.ColumnSettings.Add("ОдиницяВиміру_Назва", new("Пакування", "ОдиницяВиміру", ПакуванняОдиниціВиміру_Const.POINTER));

            Звіт.ColumnSettings.Add("ПочатковийЗалишок", new("На початок", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиВідємнеЧислоЧервоним));
            Звіт.ColumnSettings.Add("Прихід", new("Прихід", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиВідємнеЧислоЧервоним));
            Звіт.ColumnSettings.Add("Розхід", new("Розхід", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиВідємнеЧислоЧервоним));
            Звіт.ColumnSettings.Add("КінцевийЗалишок", new("На кінець", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиВідємнеЧислоЧервоним));

            await Звіт.Select();

            Звіт.FillTreeView();
            await Звіт.View(Notebook);
        }

        async void Документи()
        {
            ПараметриФільтр Фільтр = СформуватиФільтр();

            #region SELECT

            bool isExistParent = false;

            string query = $@"
WITH register AS
(
     SELECT 
        ТовариВКомірках.period AS period,
        ТовариВКомірках.owner AS owner,
        ТовариВКомірках.income AS income,
        ТовариВКомірках.{ТовариВКомірках_Const.Номенклатура} AS Номенклатура,
        ТовариВКомірках.{ТовариВКомірках_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ТовариВКомірках.{ТовариВКомірках_Const.Комірка} AS Комірка,
        ТовариВКомірках.{ТовариВКомірках_Const.Серія} AS Серія,
        ТовариВКомірках.{ТовариВКомірках_Const.Пакування} AS ОдиницяВиміру,
        ТовариВКомірках.{ТовариВКомірках_Const.ВНаявності} AS ВНаявності
    FROM
        {ТовариВКомірках_Const.TABLE} AS ТовариВКомірках
    WHERE
        (ТовариВКомірках.period >= @ПочатокПеріоду AND ТовариВКомірках.period <= @КінецьПеріоду)
";

            #region WHERE

            isExistParent = true;

            //Відбір по вибраному елементу Номенклатура
            if (Фільтр.Номенклатура.Length > 0)
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ТовариВКомірках.{ТовариВКомірках_Const.Номенклатура} IN ('{string.Join("', '", Фільтр.Номенклатура.Select(x => x.UnigueID.UGuid))}')
";
            }

            //Відбір по вибраному елементу Характеристики Номенклатури
            if (Фільтр.ХарактеристикиНоменклатури.Length > 0)
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ТовариВКомірках.{ТовариВКомірках_Const.ХарактеристикаНоменклатури} IN ('{string.Join("', '", Фільтр.ХарактеристикиНоменклатури.Select(x => x.UnigueID.UGuid))}')
";
            }

            //Відбір по вибраному елементу СкладськаКомірка
            if (!Фільтр.СкладськаКомірка.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ТовариВКомірках.{ТовариВКомірках_Const.Комірка}= '{Фільтр.СкладськаКомірка.UnigueID}'
";
            }

            //Відбір по вибраному елементу Серії Номенклатури
            if (!Фільтр.Серія.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ТовариВКомірках.{ТовариВКомірках_Const.Серія} = '{Фільтр.Серія.UnigueID}'
";
            }

            #endregion

            query += $@"
),
documents AS
(";
            int counter = 0;
            foreach (string table in ТовариВКомірках_Const.AllowDocumentSpendTable)
            {
                string docType = ТовариВКомірках_Const.AllowDocumentSpendType[counter];

                string union = counter > 0 ? "UNION" : "";
                query += $@"
{union}
SELECT 
    CONCAT({table}.uid, ':{docType}') AS Документ,
    {table}.docname AS Документ_Назва,
    register.period, 
    register.income, 
    register.ВНаявності,
    register.Номенклатура,
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    register.ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,

    /* склад, приміщення */
    Довідник_СкладськіПриміщення.{СкладськіПриміщення_Const.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Довідник_СкладськіКомірки.{СкладськіКомірки_Const.Приміщення} AS Приміщення,
    Довідник_СкладськіПриміщення.{СкладськіПриміщення_Const.Назва} AS Приміщення_Назва,

    register.Комірка,
    Довідник_СкладськіКомірки.{СкладськіКомірки_Const.Назва} AS Комірка_Назва,
    register.Серія,
    Довідник_СеріїНоменклатури.{СеріїНоменклатури_Const.Номер} AS Серія_Номер,
    register.ОдиницяВиміру,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS ОдиницяВиміру_Назва
FROM register INNER JOIN {table} ON {table}.uid = register.owner
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = register.Номенклатура
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = register.ХарактеристикаНоменклатури
    LEFT JOIN {СкладськіКомірки_Const.TABLE} AS Довідник_СкладськіКомірки ON Довідник_СкладськіКомірки.uid = register.Комірка

    /* склад, приміщення */
    LEFT JOIN {СкладськіПриміщення_Const.TABLE} AS Довідник_СкладськіПриміщення ON Довідник_СкладськіПриміщення.uid = Довідник_СкладськіКомірки.{СкладськіКомірки_Const.Приміщення}
    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = Довідник_СкладськіПриміщення.{СкладськіПриміщення_Const.Склад}

    LEFT JOIN {СеріїНоменклатури_Const.TABLE} AS Довідник_СеріїНоменклатури ON Довідник_СеріїНоменклатури.uid = register.Серія
    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON Довідник_ПакуванняОдиниціВиміру.uid = register.ОдиницяВиміру
";

                #region WHERE

                isExistParent = false;

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

                //Відбір по вибраному елементу Склади
                if (Фільтр.Склад.Length > 0)
                {
                    query += isExistParent ? "AND" : "WHERE";
                    isExistParent = true;

                    query += $@"
Довідник_Склади.uid IN ('{string.Join("', '", Фільтр.Склад.Select(x => x.UnigueID.UGuid))}')
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

                //Відбір по вибраному елементу СкладськеПриміщення
                if (!Фільтр.СкладськеПриміщення.IsEmpty())
                {
                    query += isExistParent ? "AND" : "WHERE";
                    isExistParent = true;

                    query += $@"
Довідник_СкладськіПриміщення.uid = '{Фільтр.СкладськеПриміщення.UnigueID}'
";
                }

                #endregion

                counter++;
            }

            query += $@"
)
SELECT 
    Документ,
    (CASE WHEN income = true THEN '+' ELSE '-' END) AS Рух,
    Документ_Назва, 
    Номенклатура,
    Номенклатура_Назва,
    ХарактеристикаНоменклатури,
    ХарактеристикаНоменклатури_Назва,
    Склад,
    Склад_Назва,
    Приміщення,
    Приміщення_Назва,
    Комірка,
    Комірка_Назва,
    Серія,
    Серія_Номер,
    ОдиницяВиміру,
    ОдиницяВиміру_Назва,
    ВНаявності
FROM documents
ORDER BY period ASC
";

            #endregion

            Dictionary<string, object> paramQuery = new Dictionary<string, object>
            {
                { "ПочатокПеріоду", Фільтр.ДатаПочатокПеріоду },
                { "КінецьПеріоду", Фільтр.ДатаКінецьПеріоду }
            };

            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "Товари в комірках на складах",
                Caption = "Документи",
                Query = query,
                ParamQuery = paramQuery,
                GetInfo = () => ВідобразитиФільтр("Документи", Фільтр)
            };

            Звіт.ColumnSettings.Add("Рух", new("Рух", "", "", 0.5f));
            Звіт.ColumnSettings.Add("Документ_Назва", new("Документ", "Документ", "Документи.*"));
            Звіт.ColumnSettings.Add("Номенклатура_Назва", new("Номенклатура", "Номенклатура", Номенклатура_Const.POINTER));

            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                Звіт.ColumnSettings.Add("ХарактеристикаНоменклатури_Назва", new("Характеристика", "ХарактеристикаНоменклатури", ХарактеристикиНоменклатури_Const.POINTER));

            if (Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const)
                Звіт.ColumnSettings.Add("Серія_Номер", new("Серія", "Серія", СеріїНоменклатури_Const.POINTER));

            Звіт.ColumnSettings.Add("Склад_Назва", new("Склад", "Склад", Склади_Const.POINTER));
            Звіт.ColumnSettings.Add("Приміщення_Назва", new("Приміщення", "Приміщення", СкладськіПриміщення_Const.POINTER));
            Звіт.ColumnSettings.Add("Комірка_Назва", new("Комірка", "Комірка", СкладськіКомірки_Const.POINTER));
            Звіт.ColumnSettings.Add("ОдиницяВиміру_Назва", new("Пакування", "ОдиницяВиміру", ПакуванняОдиниціВиміру_Const.POINTER));

            Звіт.ColumnSettings.Add("ВНаявності", new("В наявності", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиВідємнеЧислоЧервоним));

            await Звіт.Select();

            Звіт.FillTreeView();
            await Звіт.View(Notebook);
        }
    }
}