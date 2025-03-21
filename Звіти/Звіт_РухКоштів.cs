
/*
        Звіт_РухКоштів.cs
*/

using Gtk;
using InterfaceGtk;

using GeneratedCode.Довідники;
using GeneratedCode.РегістриНакопичення;

namespace StorageAndTrade
{
    class Звіт_РухКоштів : ФормаЗвіт
    {

        #region Filters

        Організації_PointerControl Організація = new Організації_PointerControl();
        Каси_MultiplePointerControl Каса = new Каси_MultiplePointerControl();
        Валюти_MultiplePointerControl Валюта = new Валюти_MultiplePointerControl();

        struct ПараметриФільтр
        {
            public DateTime ДатаПочатокПеріоду;
            public DateTime ДатаКінецьПеріоду;
            public Організації_Pointer Організація;
            public Каси_Pointer[] Каса;
            public Валюти_Pointer[] Валюта;
        }

        #endregion

        public Звіт_РухКоштів()
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
            //Організація
            CreateField(vBox, null, Організація);

            //Каса
            CreateField(vBox, null, Каса);
        }

        protected override void CreateContainer2(Box vBox)
        {
            //Валюта
            CreateField(vBox, null, Валюта);
        }

        #endregion

        #region Period

        const string КлючНалаштуванняКористувача = "Звіт.РухКоштів";

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
                Організація = Організація.Pointer,
                Каса = Каса.GetPointers(),
                Валюта = Валюта.GetPointers()
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

            if (!Фільтр.Організація.IsEmpty())
                text += "Організація: " + await Фільтр.Організація.GetPresentation() + "; ";

            if (Фільтр.Каса.Length > 0)
            {
                foreach (var item in Фільтр.Каса)
                    await item.GetPresentation();

                text += "Каса: " + string.Join(", ", Фільтр.Каса.Select(x => x.Назва));
            }

            if (Фільтр.Валюта.Length > 0)
            {
                foreach (var item in Фільтр.Валюта)
                    await item.GetPresentation();

                text += "Валюта: " + string.Join(", ", Фільтр.Валюта.Select(x => x.Назва));
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
    РухКоштів.{РухКоштів_Залишки_TablePart.Організація} AS Організація,
    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва,
    РухКоштів.{РухКоштів_Залишки_TablePart.Каса} AS Каса,
    Довідник_Каси.{Каси_Const.Назва} AS Каса_Назва,
    РухКоштів.{РухКоштів_Залишки_TablePart.Валюта} AS Валюта,
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    ROUND(SUM(РухКоштів.{РухКоштів_Залишки_TablePart.Сума}), 2) AS Сума
FROM 
    {РухКоштів_Залишки_TablePart.TABLE} AS РухКоштів
    LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = 
        РухКоштів.{РухКоштів_Залишки_TablePart.Організація}
    LEFT JOIN {Каси_Const.TABLE} AS Довідник_Каси ON Довідник_Каси.uid = 
        РухКоштів.{РухКоштів_Залишки_TablePart.Каса}
    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = 
        РухКоштів.{РухКоштів_Залишки_TablePart.Валюта}
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

            //Відбір по вибраних елементах Каса
            if (Фільтр.Каса.Length > 0)
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Каси.uid IN ('{string.Join("', '", Фільтр.Каса.Select(x => x.UnigueID.UGuid))}')
";

            }

            //Відбір по вибраному елементу Валюта
            if (Фільтр.Валюта.Length > 0)
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Валюти.uid IN ('{string.Join("', '", Фільтр.Валюта.Select(x => x.UnigueID.UGuid))}')
";
            }

            #endregion

            query += $@"
GROUP BY 
    GROUPING SETS (
    (
        Організація, Організація_Назва, 
        Каса, Каса_Назва, 
        Валюта, Валюта_Назва
    ),
    (
        Валюта, Валюта_Назва
    ))
HAVING 
    SUM(РухКоштів.{РухКоштів_Залишки_TablePart.Сума}) != 0
ORDER BY 
    Організація_Назва, 
    Каса_Назва, 
    Валюта_Назва
";
            #endregion

            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "Рух коштів",
                Caption = "Залишки",
                Query = query,
                GetInfo = () => ВідобразитиФільтр("Залишки", Фільтр)
            };

            Звіт.ColumnSettings.Add("Організація_Назва", new("Організація", "Організація", Організації_Const.POINTER));
            Звіт.ColumnSettings.Add("Каса_Назва", new("Каса", "Каса", Каси_Const.POINTER));
            Звіт.ColumnSettings.Add("Валюта_Назва", new("Валюта", "Валюта", Валюти_Const.POINTER));
            Звіт.ColumnSettings.Add("Сума", new("Сума", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиВідємнеЧислоЧервоним));

            //PDF
            {
                Звіт.PDFColumnSettings.Add("Організація_Назва", new("Організація", 3));
                Звіт.PDFColumnSettings.Add("Каса_Назва", new("Каса", 3));
                Звіт.PDFColumnSettings.Add("Валюта_Назва", new("Валюта"));
                Звіт.PDFColumnSettings.Add("Сума", new("Сума", 40, ЗвітСторінка.TypePDFColumn.Constant, 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
            }

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
        РухКоштів.{РухКоштів_Залишки_TablePart.Організація} AS Організація,
        РухКоштів.{РухКоштів_Залишки_TablePart.Каса} AS Каса,
        РухКоштів.{РухКоштів_Залишки_TablePart.Валюта} AS Валюта,
        SUM(РухКоштів.{РухКоштів_Залишки_TablePart.Сума}) AS Сума
    FROM 
        {РухКоштів_Залишки_TablePart.TABLE} AS РухКоштів
    WHERE
        РухКоштів.{РухКоштів_Залишки_TablePart.Період} < @ПочатокПеріоду
    GROUP BY Організація, Каса, Валюта
),
ЗалишкиТаОборотиЗаПеріод AS
(
    SELECT
        РухКоштів.{РухКоштів_ЗалишкиТаОбороти_TablePart.Організація} AS Організація,
        РухКоштів.{РухКоштів_ЗалишкиТаОбороти_TablePart.Каса} AS Каса,
        РухКоштів.{РухКоштів_ЗалишкиТаОбороти_TablePart.Валюта} AS Валюта,
        SUM(РухКоштів.{РухКоштів_ЗалишкиТаОбороти_TablePart.СумаПрихід}) AS СумаПрихід,
        SUM(РухКоштів.{РухКоштів_ЗалишкиТаОбороти_TablePart.СумаРозхід}) AS СумаРозхід,
        SUM(РухКоштів.{РухКоштів_ЗалишкиТаОбороти_TablePart.СумаЗалишок}) AS СумаЗалишок
    FROM 
        {РухКоштів_ЗалишкиТаОбороти_TablePart.TABLE} AS РухКоштів
    WHERE
        РухКоштів.{РухКоштів_ЗалишкиТаОбороти_TablePart.Період} >= @ПочатокПеріоду AND
        РухКоштів.{РухКоштів_ЗалишкиТаОбороти_TablePart.Період} <= @КінецьПеріоду
    GROUP BY Організація, Каса, Валюта
),
КінцевийЗалишок AS
(
    SELECT 
        Організація,
        Каса,
        Валюта,
        Сума AS Сума
    FROM ПочатковийЗалишок

    UNION ALL

    SELECT
        Організація,
        Каса,
        Валюта,
        СумаЗалишок
    FROM ЗалишкиТаОборотиЗаПеріод
)
SELECT 
    Організація,
    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва,
    Каса,
    Довідник_Каси.{Каси_Const.Назва} AS Каса_Назва,
    Валюта,
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    ROUND(SUM(ПочатковийЗалишок), 2) AS ПочатковийЗалишок,
    ROUND(SUM(Прихід), 2) AS Прихід,
    ROUND(SUM(Розхід), 2) AS Розхід,
    ROUND(SUM(КінцевийЗалишок), 2) AS КінцевийЗалишок
FROM 
(
    SELECT 
        Організація,
        Каса,
        Валюта,
        Сума AS ПочатковийЗалишок,
        0 AS Прихід,
        0 AS Розхід,
        0 AS КінцевийЗалишок
    FROM ПочатковийЗалишок

    UNION ALL

    SELECT
        Організація,
        Каса,
        Валюта,
        0 AS ПочатковийЗалишок,
        СумаПрихід AS Прихід,
        СумаРозхід AS Розхід,
        0 AS КінцевийЗалишок
    FROM ЗалишкиТаОборотиЗаПеріод

    UNION ALL

    SELECT
        Організація,
        Каса,
        Валюта,
        0 AS ПочатковийЗалишок,
        0 AS Прихід,
        0 AS Розхід,
        Сума AS КінцевийЗалишок
    FROM КінцевийЗалишок
) AS ЗалишкиТаОбороти
LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = ЗалишкиТаОбороти.Організація
LEFT JOIN {Каси_Const.TABLE} AS Довідник_Каси ON Довідник_Каси.uid = ЗалишкиТаОбороти.Каса
LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = ЗалишкиТаОбороти.Валюта
";

            #region WHERE

            //Відбір по вибраному елементу Організації
            if (!Фільтр.Організація.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Організації.uid = '{Фільтр.Організація.UnigueID}'
";
            }

            //Відбір по вибраних елементах Каса
            if (Фільтр.Каса.Length > 0)
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Каси.uid IN ('{string.Join("', '", Фільтр.Каса.Select(x => x.UnigueID.UGuid))}')
";

            }

            //Відбір по вибраному елементу Валюта
            if (Фільтр.Валюта.Length > 0)
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Валюти.uid IN ('{string.Join("', '", Фільтр.Валюта.Select(x => x.UnigueID.UGuid))}')
";
            }

            #endregion

            query += @"
GROUP BY 
    GROUPING SETS (
    (
        Організація, Організація_Назва, 
        Каса, Каса_Назва, 
        Валюта, Валюта_Назва
    ),
    (
        Валюта, Валюта_Назва
    ))
HAVING 
    SUM(ПочатковийЗалишок) != 0 OR 
    SUM(Прихід) != 0 OR 
    SUM(Розхід) != 0 OR
    SUM(КінцевийЗалишок) != 0
ORDER BY 
    Організація_Назва, 
    Каса_Назва, 
    Валюта_Назва
";

            #endregion

            Dictionary<string, object> paramQuery = new Dictionary<string, object>
            {
                { "ПочатокПеріоду", Фільтр.ДатаПочатокПеріоду },
                { "КінецьПеріоду", Фільтр.ДатаКінецьПеріоду }
            };

            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "Рух коштів",
                Caption = "Залишки та обороти",
                Query = query,
                ParamQuery = paramQuery,
                GetInfo = () => ВідобразитиФільтр("ЗалишкиТаОбороти", Фільтр)
            };

            Звіт.ColumnSettings.Add("Організація_Назва", new("Організація", "Організація", Організації_Const.POINTER));
            Звіт.ColumnSettings.Add("Каса_Назва", new("Каса", "Каса", Каси_Const.POINTER));
            Звіт.ColumnSettings.Add("Валюта_Назва", new("Валюта", "Валюта", Валюти_Const.POINTER));
            Звіт.ColumnSettings.Add("ПочатковийЗалишок", new("На початок", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиВідємнеЧислоЧервоним));
            Звіт.ColumnSettings.Add("Прихід", new("Прихід", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиВідємнеЧислоЧервоним));
            Звіт.ColumnSettings.Add("Розхід", new("Розхід", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиВідємнеЧислоЧервоним));
            Звіт.ColumnSettings.Add("КінцевийЗалишок", new("На кінець", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиВідємнеЧислоЧервоним));

            //PDF
            {
                Звіт.PDFColumnSettings.Add("Організація_Назва", new("Організація", 3));
                Звіт.PDFColumnSettings.Add("Каса_Назва", new("Каса", 3));
                Звіт.PDFColumnSettings.Add("Валюта_Назва", new("Валюта"));
                Звіт.PDFColumnSettings.Add("ПочатковийЗалишок", new("На початок", 40, ЗвітСторінка.TypePDFColumn.Constant, 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
                Звіт.PDFColumnSettings.Add("Прихід", new("Прихід", 40, ЗвітСторінка.TypePDFColumn.Constant, 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
                Звіт.PDFColumnSettings.Add("Розхід", new("Розхід", 40, ЗвітСторінка.TypePDFColumn.Constant, 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
                Звіт.PDFColumnSettings.Add("КінцевийЗалишок", new("На кінець", 40, ЗвітСторінка.TypePDFColumn.Constant, 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
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
        РухКоштів.period AS period,
        РухКоштів.owner AS owner,
        РухКоштів.income AS income,
        РухКоштів.{РухКоштів_Const.Сума} AS Сума,
        РухКоштів.{РухКоштів_Const.Організація} AS Організація,
        РухКоштів.{РухКоштів_Const.Каса} AS Каса,
        РухКоштів.{РухКоштів_Const.Валюта} AS Валюта
    FROM
        {РухКоштів_Const.TABLE} AS РухКоштів
    WHERE
        (РухКоштів.period >= @ПочатокПеріоду AND РухКоштів.period <= @КінецьПеріоду)
";

            #region WHERE

            isExistParent = true;

            //Відбір по вибраному елементу Організації
            if (!Фільтр.Організація.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                query += $@"
РухКоштів.{РухКоштів_Const.Організація} = '{Фільтр.Організація.UnigueID}'
";
            }

            //Відбір по вибраних елементах Каса
            if (Фільтр.Каса.Length > 0)
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
РухКоштів.{РухКоштів_Const.Каса} IN ('{string.Join("', '", Фільтр.Каса.Select(x => x.UnigueID.UGuid))}')
";

            }

            //Відбір по вибраному елементу Валюти
            if (Фільтр.Валюта.Length > 0)
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
РухКоштів.{РухКоштів_Const.Валюта} IN ('{string.Join("', '", Фільтр.Валюта.Select(x => x.UnigueID.UGuid))}')
";
            }

            #endregion

            query += $@"
),
documents AS
(";
            int counter = 0;
            foreach (string table in РухКоштів_Const.AllowDocumentSpendTable)
            {
                string docType = РухКоштів_Const.AllowDocumentSpendType[counter];

                string union = counter > 0 ? "UNION" : "";
                query += $@"
{union}
SELECT 
    CONCAT({table}.uid, ':{docType}') AS Документ, 
    {table}.docname AS Документ_Назва, 
    register.period, 
    register.income, 
    register.Сума,
    register.Організація,
    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва,
    register.Каса,
    Довідник_Каси.{Каси_Const.Назва} AS Каса_Назва,
    register.Валюта,
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва
FROM register INNER JOIN {table} ON {table}.uid = register.owner
    LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = register.Організація
    LEFT JOIN {Каси_Const.TABLE} AS Довідник_Каси ON Довідник_Каси.uid = register.Каса
    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = register.Валюта
";

                counter++;
            }

            query += $@"
)
SELECT 
    Документ,
    (CASE WHEN income = true THEN '+' ELSE '-' END) AS Рух,
    Документ_Назва, 
    Організація,
    Організація_Назва,
    Каса,
    Каса_Назва,
    Валюта,
    Валюта_Назва,
    ROUND(Сума, 2) AS Сума
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
                ReportName = "Рух коштів",
                Caption = "Документи",
                Query = query,
                ParamQuery = paramQuery,
                GetInfo = () => ВідобразитиФільтр("Документи", Фільтр)
            };

            Звіт.ColumnSettings.Add("Рух", new("Рух", "", "", 0.5f));
            Звіт.ColumnSettings.Add("Документ_Назва", new("Документ", "Документ", "Документи.*"));
            Звіт.ColumnSettings.Add("Організація_Назва", new("Організація", "Організація", Організації_Const.POINTER));
            Звіт.ColumnSettings.Add("Каса_Назва", new("Каса", "Каса", Каси_Const.POINTER));
            Звіт.ColumnSettings.Add("Валюта_Назва", new("Валюта", "Валюта", Валюти_Const.POINTER));
            Звіт.ColumnSettings.Add("Сума", new("Сума", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиВідємнеЧислоЧервоним));

            //PDF
            {
                Звіт.PDFColumnSettings.Add("Рух", new("Рух", 15, ЗвітСторінка.TypePDFColumn.Constant, 0.5f));
                Звіт.PDFColumnSettings.Add("Документ_Назва", new("Документ", 6));
                Звіт.PDFColumnSettings.Add("Організація_Назва", new("Організація", 3));
                Звіт.PDFColumnSettings.Add("Каса_Назва", new("Каса", 3));
                Звіт.PDFColumnSettings.Add("Валюта_Назва", new("Валюта"));
                Звіт.PDFColumnSettings.Add("Сума", new("Сума", 40, ЗвітСторінка.TypePDFColumn.Constant, 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
            }

            await Звіт.Select();

            Звіт.FillTreeView();
            await Звіт.View(Notebook);
        }
    }
}