
/*
        Звіт_РозрахункиЗПостачальниками.cs
*/

using Gtk;
using InterfaceGtk;

using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade
{
    class Звіт_РозрахункиЗПостачальниками : ФормаЗвіт
    {
        #region Filters

        Контрагенти_MultiplePointerControl Контрагент = new Контрагенти_MultiplePointerControl();
        Контрагенти_Папки_MultiplePointerControl Контрагент_Папка = new Контрагенти_Папки_MultiplePointerControl();
        Валюти_MultiplePointerControl Валюта = new Валюти_MultiplePointerControl();

        struct ПараметриФільтр
        {
            public DateTime ДатаПочатокПеріоду;
            public DateTime ДатаКінецьПеріоду;
            public Контрагенти_Pointer[] Контрагент;
            public Контрагенти_Папки_Pointer[] Контрагент_Папка;
            public Валюти_Pointer[] Валюта;
        }

        #endregion

        public Звіт_РозрахункиЗПостачальниками()
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
            //Контрагент
            CreateField(vBox, null, Контрагент);

            //Контрагент папка
            CreateField(vBox, null, Контрагент_Папка);
        }

        protected override void CreateContainer2(Box vBox)
        {
            //Валюта
            CreateField(vBox, null, Валюта);
        }

        #endregion

        #region Period

        const string КлючНалаштуванняКористувача = "Звіт.РозрахункиЗПостачальниками";

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
                Контрагент = Контрагент.GetPointers(),
                Контрагент_Папка = Контрагент_Папка.GetPointers(),
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

            if (Фільтр.Контрагент.Length > 0)
            {
                foreach (var item in Фільтр.Контрагент)
                    await item.GetPresentation();

                text += "Контрагент: " + string.Join(", ", Фільтр.Контрагент.Select(x => x.Назва)) + "; ";
            }

            if (Фільтр.Контрагент_Папка.Length > 0)
            {
                foreach (var item in Фільтр.Контрагент_Папка)
                    await item.GetPresentation();

                text += "Контрагент папка: " + string.Join(", ", Фільтр.Контрагент_Папка.Select(x => x.Назва)) + "; ";
            }

            if (Фільтр.Валюта.Length > 0)
            {
                foreach (var item in Фільтр.Валюта)
                    await item.GetPresentation();

                text += "Валюта: " + string.Join(", ", Фільтр.Валюта.Select(x => x.Назва)) + "; ";
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
    РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Залишки_TablePart.Контрагент} AS Контрагент,
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS Контрагент_Назва,
    РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Залишки_TablePart.Валюта} AS Валюта, 
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    ROUND(SUM(РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Залишки_TablePart.Сума}), 2) AS Сума
FROM 
    {РозрахункиЗПостачальниками_Залишки_TablePart.TABLE} AS РозрахункиЗПостачальниками
    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = 
        РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Залишки_TablePart.Валюта}
    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Залишки_TablePart.Контрагент}
";

            #region WHERE

            //Відбір по всіх вкладених папках вибраної папки Контрагенти
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

            //Відбір по вибраному елементу Контрагенту
            if (Фільтр.Контрагент.Length > 0)
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Контрагенти.uid IN ('{string.Join("', '", Фільтр.Контрагент.Select(x => x.UnigueID.UGuid))}')
";
            }

            //Відбір по вибраному елементу Валюти
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
    Контрагент, Контрагент_Назва, 
    Валюта, Валюта_Назва
HAVING
    SUM(РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Залишки_TablePart.Сума}) != 0
ORDER BY 
    Контрагент_Назва
";

            #endregion

            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "Розрахунки з постачальниками",
                Caption = "Залишки",
                Query = query,
                GetInfo = () => ВідобразитиФільтр("Залишки", Фільтр)
            };

            Звіт.ColumnSettings.Add("Контрагент_Назва", new("Контрагент", "Контрагент", Контрагенти_Const.POINTER));
            Звіт.ColumnSettings.Add("Валюта_Назва", new("Валюта", "Валюта", Валюти_Const.POINTER));
            Звіт.ColumnSettings.Add("Сума", new("Сума", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиВідємнеЧислоЧервоним));

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
        РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Залишки_TablePart.Контрагент} AS Контрагент,
        РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Залишки_TablePart.Валюта} AS Валюта,
        SUM(РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Залишки_TablePart.Сума}) AS Сума
    FROM 
        {РозрахункиЗПостачальниками_Залишки_TablePart.TABLE} AS РозрахункиЗПостачальниками
    WHERE
        РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Залишки_TablePart.Період} < @ПочатокПеріоду
    GROUP BY Контрагент, Валюта
),
ЗалишкиТаОборотиЗаПеріод AS
(
    SELECT 
        РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_ЗалишкиТаОбороти_TablePart.Контрагент} AS Контрагент,
        РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_ЗалишкиТаОбороти_TablePart.Валюта} AS Валюта,
        SUM(РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_ЗалишкиТаОбороти_TablePart.СумаПрихід}) AS СумаПрихід,
        SUM(РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_ЗалишкиТаОбороти_TablePart.СумаРозхід}) AS СумаРозхід,
        SUM(РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_ЗалишкиТаОбороти_TablePart.СумаЗалишок}) AS СумаЗалишок
    FROM 
        {РозрахункиЗПостачальниками_ЗалишкиТаОбороти_TablePart.TABLE} AS РозрахункиЗПостачальниками
    WHERE
        РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_ЗалишкиТаОбороти_TablePart.Період} >= @ПочатокПеріоду AND
        РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_ЗалишкиТаОбороти_TablePart.Період} <= @КінецьПеріоду
    GROUP BY Контрагент, Валюта
),
КінцевийЗалишок AS
(
    SELECT 
        Контрагент,
        Валюта,
        Сума
    FROM ПочатковийЗалишок

    UNION ALL

    SELECT
        Контрагент,
        Валюта,
        СумаЗалишок
    FROM ЗалишкиТаОборотиЗаПеріод
)
SELECT 
    Контрагент,
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS Контрагент_Назва,
    Валюта,
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    ROUND(SUM(ПочатковийЗалишок), 2) AS ПочатковийЗалишок,
    ROUND(SUM(Прихід), 2) AS Прихід,
    ROUND(SUM(Розхід), 2) AS Розхід,
    ROUND(SUM(КінцевийЗалишок), 2) AS КінцевийЗалишок
FROM 
(
    SELECT 
        Контрагент,
        Валюта,
        Сума AS ПочатковийЗалишок,
        0 AS Прихід,
        0 AS Розхід,
        0 AS КінцевийЗалишок
    FROM ПочатковийЗалишок

    UNION ALL

    SELECT
        Контрагент,
        Валюта,
        0 AS ПочатковийЗалишок,
        СумаПрихід AS Прихід,
        СумаРозхід AS Розхід,
        0 AS КінцевийЗалишок
    FROM ЗалишкиТаОборотиЗаПеріод

    UNION ALL

    SELECT
        Контрагент,
        Валюта,
        0 AS ПочатковийЗалишок,
        0 AS Прихід,
        0 AS Розхід,
        Сума AS КінцевийЗалишок
    FROM КінцевийЗалишок
) AS ЗалишкиТаОбороти
LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = ЗалишкиТаОбороти.Контрагент
LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = ЗалишкиТаОбороти.Валюта
";

            #region WHERE

            //Відбір по всіх вкладених папках вибраної папки Контрагенти
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

            //Відбір по вибраному елементу Контрагенти
            if (Фільтр.Контрагент.Length > 0)
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Контрагенти.uid IN ('{string.Join("', '", Фільтр.Контрагент.Select(x => x.UnigueID.UGuid))}')
";
            }

            //Відбір по вибраному елементу
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
    Контрагент, 
    Контрагент_Назва, 
    Валюта, 
    Валюта_Назва
HAVING 
    SUM(ПочатковийЗалишок) != 0 OR
    SUM(КінцевийЗалишок) != 0 OR
    SUM(Прихід) != 0 OR 
    SUM(Розхід) != 0 
ORDER BY 
    Контрагент_Назва, 
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
                ReportName = "Розрахунки з постачальниками",
                Caption = "Залишки та обороти",
                Query = query,
                ParamQuery = paramQuery,
                GetInfo = () => ВідобразитиФільтр("ЗалишкиТаОбороти", Фільтр)
            };

            Звіт.ColumnSettings.Add("Контрагент_Назва", new("Контрагент", "Контрагент", Контрагенти_Const.POINTER));
            Звіт.ColumnSettings.Add("Валюта_Назва", new("Валюта", "Валюта", Валюти_Const.POINTER));
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
        РозрахункиЗПостачальниками.period AS period,
        РозрахункиЗПостачальниками.owner AS owner,
        РозрахункиЗПостачальниками.income AS income,
        РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Контрагент} AS Контрагент,
        РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Валюта} AS Валюта,
        РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} AS Сума
    FROM
        {РозрахункиЗПостачальниками_Const.TABLE} AS РозрахункиЗПостачальниками
    WHERE
        (РозрахункиЗПостачальниками.period >= @ПочатокПеріоду AND РозрахункиЗПостачальниками.period <= @КінецьПеріоду)
";

            #region WHERE

            isExistParent = true;

            //Відбір по вибраному елементу Контрагенти
            if (Фільтр.Контрагент.Length > 0)
            {
                query += isExistParent ? "AND" : "WHERE";
                query += $@"
РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Контрагент} = IN ('{string.Join("', '", Фільтр.Контрагент.Select(x => x.UnigueID.UGuid))}')
";
            }

            //Відбір по вибраному елементу Валюти
            if (Фільтр.Валюта.Length > 0)
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Валюта} IN ('{string.Join("', '", Фільтр.Валюта.Select(x => x.UnigueID.UGuid))}')
";
            }

            #endregion

            query += $@"
),
documents AS
(";
            int counter = 0;
            foreach (string table in РозрахункиЗПостачальниками_Const.AllowDocumentSpendTable)
            {
                string docType = РозрахункиЗПостачальниками_Const.AllowDocumentSpendType[counter];

                string union = counter > 0 ? "UNION" : "";
                query += $@"
{union}
SELECT 
    CONCAT({table}.uid, ':{docType}') AS Документ,
    {table}.docname AS Документ_Назва,
    register.period,
    register.income,
    register.Сума,
    register.Контрагент,
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS Контрагент_Назва,
    register.Валюта,
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва
FROM register INNER JOIN {table} ON {table}.uid = register.owner
    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = register.Контрагент
    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = register.Валюта
";

                #region WHERE

                isExistParent = false;

                //Відбір по всіх вкладених папках вибраної папки Контрагенти
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

                #endregion

                counter++;
            }

            query += $@"
)
SELECT 
    Документ,
    (CASE WHEN income = true THEN '+' ELSE '-' END) AS Рух,
    Документ_Назва,
    Контрагент,
    Контрагент_Назва,
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
                ReportName = "Розрахунки з постачальниками",
                Caption = "Документи",
                Query = query,
                ParamQuery = paramQuery,
                GetInfo = () => ВідобразитиФільтр("Документи", Фільтр)
            };

            Звіт.ColumnSettings.Add("Рух", new("Рух", "", "", 0.5f));
            Звіт.ColumnSettings.Add("Документ_Назва", new("Документ", "Документ", "Документи.*"));
            Звіт.ColumnSettings.Add("Контрагент_Назва", new("Контрагент", "Контрагент", Контрагенти_Const.POINTER));
            Звіт.ColumnSettings.Add("Валюта_Назва", new("Валюта", "Валюта", Валюти_Const.POINTER));
            Звіт.ColumnSettings.Add("Сума", new("Сума", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиВідємнеЧислоЧервоним));

            await Звіт.Select();

            Звіт.FillTreeView();
            await Звіт.View(Notebook);
        }
    }
}