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

using Константи = StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade
{
    class Звіт_ЗамовленняПостачальникам : ФормаЗвіт2
    {
        #region Filters

        Номенклатура_PointerControl Номенклатура = new Номенклатура_PointerControl();
        Номенклатура_Папки_PointerControl Номенклатура_Папка = new Номенклатура_Папки_PointerControl() { Caption = "Номенклатура папка:" };
        ХарактеристикиНоменклатури_PointerControl ХарактеристикиНоменклатури = new ХарактеристикиНоменклатури_PointerControl();
        Склади_PointerControl Склад = new Склади_PointerControl();
        Склади_Папки_PointerControl Склад_Папка = new Склади_Папки_PointerControl() { Caption = "Склад папка:" };

        struct ПараметриФільтр
        {
            public DateTime ДатаПочатокПеріоду;
            public DateTime ДатаКінецьПеріоду;
            public Номенклатура_Pointer Номенклатура;
            public Номенклатура_Папки_Pointer Номенклатура_Папка;
            public ХарактеристикиНоменклатури_Pointer ХарактеристикиНоменклатури;
            public Склади_Pointer Склад;
            public Склади_Папки_Pointer Склад_Папка;
        }

        #endregion

        public Звіт_ЗамовленняПостачальникам()
        {
            //Кнопки
            Button bOstatok = new Button("Залишки");
            bOstatok.Clicked += (object? sender, EventArgs args) => Залишки();
            HBoxTop.PackStart(bOstatok, false, false, 10);

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
        }

        protected override void CreateContainer2(Box vBox)
        {
            //Номенклатура папка
            CreateField(vBox, null, Номенклатура_Папка);

            //Склад папка
            CreateField(vBox, null, Склад_Папка);
        }

        #endregion

        #region Period

        const string КлючНалаштуванняКористувача = "Звіт.ЗамовленняПостачальникам";

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
                Номенклатура = Номенклатура.Pointer,
                Номенклатура_Папка = Номенклатура_Папка.Pointer,
                ХарактеристикиНоменклатури = ХарактеристикиНоменклатури.Pointer,
                Склад = Склад.Pointer,
                Склад_Папка = Склад_Папка.Pointer
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
                case "Документи":
                    {
                        text += "З <b>" +
                            Фільтр.ДатаПочатокПеріоду.ToString("dd.MM.yyyy") + "</b> по <b>" +
                            Фільтр.ДатаКінецьПеріоду.ToString("dd.MM.yyyy") + "</b>; ";
                        break;
                    }
            }

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

            return text;
        }

        async void Залишки()
        {
            ПараметриФільтр Фільтр = СформуватиФільтр();

            #region SELECT

            bool isExistParent = false;

            string query = $@"
SELECT 
    ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Залишки_TablePart.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Залишки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Залишки_TablePart.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Довідник_ПакуванняОдиниціВиміру.uid AS ОдиницяВиміру,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS ОдиницяВиміру_Назва,
    ROUND(SUM(ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Залишки_TablePart.Замовлено}), 2) AS Замовлено
FROM 
    {ЗамовленняПостачальникам_Залишки_TablePart.TABLE} AS ЗамовленняПостачальникам
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Залишки_TablePart.Номенклатура}
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Залишки_TablePart.ХарактеристикаНоменклатури}
    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
        ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Залишки_TablePart.Склад}
    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON Довідник_ПакуванняОдиниціВиміру.uid = 
        Довідник_Номенклатура.{Номенклатура_Const.ОдиницяВиміру}
";

            #region WHERE

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

            #endregion

            query += $@"
GROUP BY 
    GROUPING SETS (
    (
        Номенклатура, Номенклатура_Назва, 
        ХарактеристикаНоменклатури, ХарактеристикаНоменклатури_Назва,
        Склад, Склад_Назва,
        ОдиницяВиміру, ОдиницяВиміру_Назва
    ), ())
HAVING
    SUM(ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Залишки_TablePart.Замовлено}) != 0 
ORDER BY 
    Номенклатура_Назва
";
            #endregion

            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "Замовлення постачальникам",
                Caption = "Залишки",
                Query = query,
                ParamReport = Фільтр,
                GetInfo = () => ВідобразитиФільтр("Залишки", Фільтр)
            };

            Звіт.ColumnSettings.Add("Номенклатура_Назва", new("Номенклатура", "Номенклатура", Номенклатура_Const.POINTER));

            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                Звіт.ColumnSettings.Add("ХарактеристикаНоменклатури_Назва", new("Характеристика", "ХарактеристикаНоменклатури", ХарактеристикиНоменклатури_Const.POINTER));

            Звіт.ColumnSettings.Add("Склад_Назва", new("Склад", "Склад", Склади_Const.POINTER));
            Звіт.ColumnSettings.Add("ОдиницяВиміру_Назва", new("Пакування", "ОдиницяВиміру", ПакуванняОдиниціВиміру_Const.POINTER));
            Звіт.ColumnSettings.Add("Замовлено", new("Замовлено", "", "", 1, ФункціїДляЗвітів.ФункціяДляКолонкиВідємнеЧислоЧервоним));

            await Звіт.Select();

            Звіт.FillTreeView();
            Звіт.View(Notebook);
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
        ЗамовленняПостачальникам.period AS period,
        ЗамовленняПостачальникам.owner AS owner,
        ЗамовленняПостачальникам.income AS income,
        ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.ЗамовленняПостачальнику} AS ЗамовленняПостачальнику,
        ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Номенклатура} AS Номенклатура,
        ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Склад} AS Склад,
        ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} AS Замовлено
    FROM
        {ЗамовленняПостачальникам_Const.TABLE} AS ЗамовленняПостачальникам
    WHERE
        (ЗамовленняПостачальникам.period >= @ПочатокПеріоду AND ЗамовленняПостачальникам.period <= @КінецьПеріоду)
";

            #region WHERE

            isExistParent = true;

            //Відбір по вибраному елементу Номенклатура
            if (!Фільтр.Номенклатура.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Номенклатура} = '{Фільтр.Номенклатура.UnigueID}'
";
            }

            //Відбір по вибраному елементу Характеристики Номенклатури
            if (!Фільтр.ХарактеристикиНоменклатури.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.ХарактеристикаНоменклатури} = '{Фільтр.ХарактеристикиНоменклатури.UnigueID}'
";
            }

            //Відбір по вибраному елементу Склади
            if (!Фільтр.Склад.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Склад} = '{Фільтр.Склад.UnigueID}'
";
            }

            #endregion

            query += $@"
),
documents AS
(";
            int counter = 0;
            foreach (string table in ЗамовленняПостачальникам_Const.AllowDocumentSpendTable)
            {
                string docType = ЗамовленняПостачальникам_Const.AllowDocumentSpendType[counter];

                string union = counter > 0 ? "UNION" : "";
                query += $@"
{union}
SELECT 
    CONCAT({table}.uid, ':{docType}') AS Документ,
    {table}.docname AS Документ_Назва, 
    register.period, 
    register.income, 
    register.Замовлено,
    register.ЗамовленняПостачальнику,
    Документ_ЗамовленняПостачальнику.{ЗамовленняПостачальнику_Const.Назва} AS ЗамовленняПостачальнику_Назва,
    register.Номенклатура,
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    register.ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    register.Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Довідник_ПакуванняОдиниціВиміру.uid AS ОдиницяВиміру,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS ОдиницяВиміру_Назва
FROM register INNER JOIN {table} ON {table}.uid = register.owner
    LEFT JOIN {ЗамовленняПостачальнику_Const.TABLE} AS Документ_ЗамовленняПостачальнику ON Документ_ЗамовленняПостачальнику.uid = register.ЗамовленняПостачальнику
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = register.Номенклатура
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = register.ХарактеристикаНоменклатури
    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = register.Склад
    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON Довідник_ПакуванняОдиниціВиміру.uid = Довідник_Номенклатура.{Номенклатура_Const.ОдиницяВиміру}
";

                #region WHERE

                isExistParent = false;

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

                #endregion

                counter++;
            }

            query += $@"
)
SELECT 
    Документ,
    (CASE WHEN income = true THEN '+' ELSE '-' END) AS Рух,
    Документ_Назва,
    ЗамовленняПостачальнику, 
    ЗамовленняПостачальнику_Назва,
    Номенклатура,
    Номенклатура_Назва,
    ХарактеристикаНоменклатури,
    ХарактеристикаНоменклатури_Назва,
    Склад,
    Склад_Назва,
    ОдиницяВиміру,
    ОдиницяВиміру_Назва,
    ROUND(Замовлено, 2) AS Замовлено
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
                ReportName = "Замовлення постачальникам",
                Caption = "Документи",
                Query = query,
                ParamQuery = paramQuery,
                ParamReport = Фільтр,
                GetInfo = () => ВідобразитиФільтр("Документи", Фільтр)
            };

            Звіт.ColumnSettings.Add("Рух", new("Рух", "", "", 0.5f));
            Звіт.ColumnSettings.Add("Документ_Назва", new("Документ", "Документ", "Документи.*"));
            Звіт.ColumnSettings.Add("ЗамовленняПостачальнику_Назва", new("Замовлення постачальнику", "ЗамовленняПостачальнику", ЗамовленняПостачальнику_Const.POINTER));
            Звіт.ColumnSettings.Add("Номенклатура_Назва", new("Номенклатура", "Номенклатура", Номенклатура_Const.POINTER));

            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                Звіт.ColumnSettings.Add("ХарактеристикаНоменклатури_Назва", new("Характеристика", "ХарактеристикаНоменклатури", ХарактеристикиНоменклатури_Const.POINTER));

            Звіт.ColumnSettings.Add("Склад_Назва", new("Склад", "Склад", Склади_Const.POINTER));
            Звіт.ColumnSettings.Add("ОдиницяВиміру_Назва", new("Пакування", "ОдиницяВиміру", ПакуванняОдиниціВиміру_Const.POINTER));
            Звіт.ColumnSettings.Add("Замовлено", new("Замовлено", "", "", 1, ФункціїДляЗвітів.ФункціяДляКолонкиВідємнеЧислоЧервоним));

            await Звіт.Select();

            Звіт.FillTreeView();
            Звіт.View(Notebook);
        }
    }
}