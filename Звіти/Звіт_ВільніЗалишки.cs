
/*
        Звіт_ВільніЗалишки.cs
*/

using Gtk;
using InterfaceGtk;

using Константи = GeneratedCode.Константи;
using GeneratedCode.Довідники;
using GeneratedCode.РегістриНакопичення;

namespace StorageAndTrade
{
    class Звіт_ВільніЗалишки : ФормаЗвіт
    {
        #region Filters

        Номенклатура_MultiplePointerControl Номенклатура = new Номенклатура_MultiplePointerControl();
        Номенклатура_Папки_MultiplePointerControl Номенклатура_Папка = new Номенклатура_Папки_MultiplePointerControl() { Caption = "Номенклатура папка:" };
        ХарактеристикиНоменклатури_MultiplePointerControl ХарактеристикиНоменклатури = new ХарактеристикиНоменклатури_MultiplePointerControl();
        Склади_MultiplePointerControl Склад = new Склади_MultiplePointerControl();
        Склади_Папки_MultiplePointerControl Склад_Папка = new Склади_Папки_MultiplePointerControl() { Caption = "Склад папка:" };

        struct ПараметриФільтр
        {
            public DateTime ДатаПочатокПеріоду;
            public DateTime ДатаКінецьПеріоду;
            public Номенклатура_Pointer[] Номенклатура;
            public Номенклатура_Папки_Pointer[] Номенклатура_Папка;
            public ХарактеристикиНоменклатури_Pointer[] ХарактеристикиНоменклатури;
            public Склади_Pointer[] Склад;
            public Склади_Папки_Pointer[] Склад_Папка;
        }

        #endregion

        public Звіт_ВільніЗалишки()
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

        const string КлючНалаштуванняКористувача = "Звіт.ВільніЗалишки";

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
                Склад_Папка = Склад_Папка.GetPointers()
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

            return text;
        }

        async void Залишки()
        {
            ПараметриФільтр Фільтр = СформуватиФільтр();

            #region SELECT

            bool isExistParent = false;

            string query = $@"
SELECT 
    ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Довідник_ПакуванняОдиниціВиміру.uid AS ОдиницяВиміру,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS ОдиницяВиміру_Назва,
    ROUND(SUM(ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.ВНаявності}), 2) AS ВНаявності,
    ROUND(SUM(ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.ВРезервіЗіСкладу}), 2) AS ВРезервіЗіСкладу,
    ROUND(SUM(ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.ВРезервіПідЗамовлення}), 2) AS ВРезервіПідЗамовлення
FROM 
    {ВільніЗалишки_Залишки_TablePart.TABLE} AS ВільніЗалишки
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.Номенклатура}
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.ХарактеристикаНоменклатури}
    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
        ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.Склад}
    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON Довідник_ПакуванняОдиниціВиміру.uid = 
        Довідник_Номенклатура.{Номенклатура_Const.ОдиницяВиміру}
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
    SUM(ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.ВНаявності}) != 0 OR
    SUM(ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.ВРезервіЗіСкладу}) != 0 OR
    SUM(ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.ВРезервіПідЗамовлення}) != 0   
ORDER BY 
    Номенклатура_Назва
";

            #endregion

            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "Вільні залишки",
                Caption = "Залишки",
                Query = query,
                GetInfo = () => ВідобразитиФільтр("Залишки", Фільтр)
            };

            Звіт.ColumnSettings.Add("Номенклатура_Назва", new("Номенклатура", "Номенклатура", Номенклатура_Const.POINTER));

            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                Звіт.ColumnSettings.Add("ХарактеристикаНоменклатури_Назва", new("Характеристика", "ХарактеристикаНоменклатури", ХарактеристикиНоменклатури_Const.POINTER));

            Звіт.ColumnSettings.Add("Склад_Назва", new("Склад", "Склад", Склади_Const.POINTER));
            Звіт.ColumnSettings.Add("ОдиницяВиміру_Назва", new("Пакування", "ОдиницяВиміру", ПакуванняОдиниціВиміру_Const.POINTER));
            Звіт.ColumnSettings.Add("ВНаявності", new("В наявності", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиВідємнеЧислоЧервоним));
            Звіт.ColumnSettings.Add("ВРезервіЗіСкладу", new("В резерві зі складу", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиВідємнеЧислоЧервоним));
            Звіт.ColumnSettings.Add("ВРезервіПідЗамовлення", new("В резерві під замовлення", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиВідємнеЧислоЧервоним));

            //PDF
            {
                Звіт.PDFColumnSettings.Add("Номенклатура_Назва", new("Номенклатура", 5));

                if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                    Звіт.PDFColumnSettings.Add("ХарактеристикаНоменклатури_Назва", new("Характеристика", 4));

                Звіт.PDFColumnSettings.Add("Склад_Назва", new("Склад"));
                Звіт.PDFColumnSettings.Add("ОдиницяВиміру_Назва", new("Пакування"));

                Звіт.PDFColumnSettings.Add("ВНаявності", new("В наявності", 40, ЗвітСторінка.TypePDFColumn.Constant, 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
                Звіт.PDFColumnSettings.Add("ВРезервіЗіСкладу", new("В резерві зі складу", 40, ЗвітСторінка.TypePDFColumn.Constant, 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
                Звіт.PDFColumnSettings.Add("ВРезервіПідЗамовлення", new("В резерві під замовлення", 40, ЗвітСторінка.TypePDFColumn.Constant, 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
            }

            //Excel
            {
                Звіт.ExcelColumnSettings.Add("Номенклатура_Назва", new("Номенклатура"));

                if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                    Звіт.ExcelColumnSettings.Add("ХарактеристикаНоменклатури_Назва", new("Характеристика"));

                Звіт.ExcelColumnSettings.Add("Склад_Назва", new("Склад"));
                Звіт.ExcelColumnSettings.Add("ОдиницяВиміру_Назва", new("Пакування"));

                Звіт.ExcelColumnSettings.Add("ВНаявності", new("В наявності", "N"));
                Звіт.ExcelColumnSettings.Add("ВРезервіЗіСкладу", new("В резерві зі складу", "N"));
                Звіт.ExcelColumnSettings.Add("ВРезервіПідЗамовлення", new("В резерві під замовлення", "N"));
            }

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
        ВільніЗалишки.period AS period,
        ВільніЗалишки.owner AS owner,
        ВільніЗалишки.income AS income,
        ВільніЗалишки.{ВільніЗалишки_Const.Номенклатура} AS Номенклатура,
        ВільніЗалишки.{ВільніЗалишки_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ВільніЗалишки.{ВільніЗалишки_Const.Склад} AS Склад,
        ВільніЗалишки.{ВільніЗалишки_Const.ВНаявності} AS ВНаявності,
        ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіЗіСкладу} AS ВРезервіЗіСкладу,
        ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіПідЗамовлення} AS ВРезервіПідЗамовлення
    FROM
        {ВільніЗалишки_Const.TABLE} AS ВільніЗалишки
    WHERE
        (ВільніЗалишки.period >= @ПочатокПеріоду AND ВільніЗалишки.period <= @КінецьПеріоду)
";

            #region WHERE

            isExistParent = true;

            //Відбір по вибраному елементу Номенклатура
            if (Фільтр.Номенклатура.Length > 0)
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ВільніЗалишки.{ВільніЗалишки_Const.Номенклатура} IN ('{string.Join("', '", Фільтр.Номенклатура.Select(x => x.UnigueID.UGuid))}')
";
            }

            //Відбір по вибраному елементу Характеристики Номенклатури
            if (Фільтр.ХарактеристикиНоменклатури.Length > 0)
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ВільніЗалишки.{ВільніЗалишки_Const.ХарактеристикаНоменклатури} IN ('{string.Join("', '", Фільтр.ХарактеристикиНоменклатури.Select(x => x.UnigueID.UGuid))}')
";
            }

            //Відбір по вибраному елементу Склади
            if (Фільтр.Склад.Length > 0)
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ВільніЗалишки.{ВільніЗалишки_Const.Склад} IN ('{string.Join("', '", Фільтр.Склад.Select(x => x.UnigueID.UGuid))}')
";
            }

            #endregion

            query += $@"
),
documents AS
(";
            int counter = 0;
            foreach (string table in ВільніЗалишки_Const.AllowDocumentSpendTable)
            {
                string docType = ВільніЗалишки_Const.AllowDocumentSpendType[counter];

                string union = counter > 0 ? "UNION" : "";
                query += $@"
{union}
SELECT 
    CONCAT({table}.uid, ':{docType}') AS Документ,
    {table}.docname AS Документ_Назва,
    register.period,
    register.income,
    register.ВНаявності,
    register.ВРезервіЗіСкладу,
    register.ВРезервіПідЗамовлення,
    register.Номенклатура,
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    register.ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    register.Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Довідник_ПакуванняОдиниціВиміру.uid AS ОдиницяВиміру,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS ОдиницяВиміру_Назва
FROM register INNER JOIN {table} ON {table}.uid = register.owner
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = register.Номенклатура
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = register.ХарактеристикаНоменклатури
    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = register.Склад
    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON Довідник_ПакуванняОдиниціВиміру.uid = Довідник_Номенклатура.{Номенклатура_Const.ОдиницяВиміру}
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
    ОдиницяВиміру,
    ОдиницяВиміру_Назва,
    ВНаявності,
    ВРезервіЗіСкладу, 
    ВРезервіПідЗамовлення
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
                ReportName = "Вільні залишки",
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

            Звіт.ColumnSettings.Add("Склад_Назва", new("Склад", "Склад", Склади_Const.POINTER));
            Звіт.ColumnSettings.Add("ОдиницяВиміру_Назва", new("Пакування", "ОдиницяВиміру", ПакуванняОдиниціВиміру_Const.POINTER));

            Звіт.ColumnSettings.Add("ВНаявності", new("В наявності", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиВідємнеЧислоЧервоним));
            Звіт.ColumnSettings.Add("ВРезервіЗіСкладу", new("В резерві зі складу", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиВідємнеЧислоЧервоним));
            Звіт.ColumnSettings.Add("ВРезервіПідЗамовлення", new("В резерві під замовлення", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиВідємнеЧислоЧервоним));

            //PDF
            {
                Звіт.PDFColumnSettings.Add("Рух", new("Рух", 15, ЗвітСторінка.TypePDFColumn.Constant, 0.5f));
                Звіт.PDFColumnSettings.Add("Документ_Назва", new("Документ", 6));
                Звіт.PDFColumnSettings.Add("Номенклатура_Назва", new("Номенклатура", 5));

                if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                    Звіт.PDFColumnSettings.Add("ХарактеристикаНоменклатури_Назва", new("Характеристика", 4));

                Звіт.PDFColumnSettings.Add("Склад_Назва", new("Склад"));
                Звіт.PDFColumnSettings.Add("ОдиницяВиміру_Назва", new("Пакування"));

                Звіт.PDFColumnSettings.Add("ВНаявності", new("В наявності", 40, ЗвітСторінка.TypePDFColumn.Constant, 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
                Звіт.PDFColumnSettings.Add("ВРезервіЗіСкладу", new("В резерві зі складу", 40, ЗвітСторінка.TypePDFColumn.Constant, 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
                Звіт.PDFColumnSettings.Add("ВРезервіПідЗамовлення", new("В резерві під замовлення", 40, ЗвітСторінка.TypePDFColumn.Constant, 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
            }

            //Excel
            {
                Звіт.ExcelColumnSettings.Add("Рух", new("Рух"));
                Звіт.ExcelColumnSettings.Add("Документ_Назва", new("Документ"));
                Звіт.ExcelColumnSettings.Add("Номенклатура_Назва", new("Номенклатура"));

                if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                    Звіт.ExcelColumnSettings.Add("ХарактеристикаНоменклатури_Назва", new("Характеристика"));

                Звіт.ExcelColumnSettings.Add("Склад_Назва", new("Склад"));
                Звіт.ExcelColumnSettings.Add("ОдиницяВиміру_Назва", new("Пакування"));

                Звіт.ExcelColumnSettings.Add("ВНаявності", new("В наявності", "N"));
                Звіт.ExcelColumnSettings.Add("ВРезервіЗіСкладу", new("В резерві зі складу", "N"));
                Звіт.ExcelColumnSettings.Add("ВРезервіПідЗамовлення", new("В резерві під замовлення", "N"));
            }

            await Звіт.Select();

            Звіт.FillTreeView();
            await Звіт.View(Notebook);
        }
    }
}