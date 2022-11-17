using Gtk;

using AccountingSoftware;
using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade
{
    class Звіт_РухКоштів : VBox
    {
        Notebook reportNotebook;

        #region Filters

        DateTimeControl ДатаПочаткуПеріоду = new DateTimeControl() { Value = DateTime.Parse($"01.{DateTime.Now.Month}.{DateTime.Now.Year}") };
        DateTimeControl ДатаКінцяПеріоду = new DateTimeControl() { Value = DateTime.Now };

        Організації_PointerControl Організація = new Організації_PointerControl();
        Каси_PointerControl Каса = new Каси_PointerControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();

        #endregion

        public Звіт_РухКоштів() : base()
        {
            //Кнопки
            HBox hBoxBotton = new HBox();

            //1
            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

            hBoxBotton.PackStart(bClose, false, false, 10);

            //2
            Button bOstatok = new Button("Залишки");
            bOstatok.Clicked += OnReportOstatok;

            hBoxBotton.PackStart(bOstatok, false, false, 10);

            //3
            Button bOborot = new Button("Залишки та обороти");
            bOborot.Clicked += OnReportOborot;

            hBoxBotton.PackStart(bOborot, false, false, 10);

            //4
            Button bDocuments = new Button("Документи");
            bDocuments.Clicked += OnReportDocuments;

            hBoxBotton.PackStart(bDocuments, false, false, 10);

            PackStart(hBoxBotton, false, false, 10);

            CreateFilters();

            reportNotebook = new Notebook() { Scrollable = true, EnablePopup = true, BorderWidth = 0, ShowBorder = false };
            reportNotebook.TabPos = PositionType.Top;
            PackStart(reportNotebook, true, true, 0);

            ShowAll();
        }

        #region Filters

        void CreateFilters()
        {
            HBox hBoxContainer = new HBox();

            Expander expander = new Expander("Відбори") { Expanded = true };
            expander.Add(hBoxContainer);

            //Container1
            VBox vBoxContainer1 = new VBox() { WidthRequest = 500 };
            hBoxContainer.PackStart(vBoxContainer1, false, false, 5);

            CreateContainer1(vBoxContainer1);

            //Container2
            VBox vBoxContainer2 = new VBox() { WidthRequest = 500 };
            hBoxContainer.PackStart(vBoxContainer2, false, false, 5);

            CreateContainer2(vBoxContainer2);

            PackStart(expander, false, false, 10);
        }

        void CreateContainer1(VBox vBox)
        {
            //Період
            HBox hBoxPeriod = new HBox() { Halign = Align.End };
            hBoxPeriod.PackStart(new Label("Період з "), false, false, 5);
            hBoxPeriod.PackStart(ДатаПочаткуПеріоду, false, false, 5);
            hBoxPeriod.PackStart(new Label(" по "), false, false, 5);
            hBoxPeriod.PackStart(ДатаКінцяПеріоду, false, false, 5);
            vBox.PackStart(hBoxPeriod, false, false, 5);

            //Організація
            HBox hBoxOrganization = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxOrganization, false, false, 5);

            hBoxOrganization.PackStart(Організація, false, false, 5);

            //Каса
            HBox hBoxKasa = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxKasa, false, false, 5);

            hBoxKasa.PackStart(Каса, false, false, 5);
        }

        void CreateContainer2(VBox vBox)
        {
            //Валюта
            HBox hBoxValuta = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxValuta, false, false, 5);

            hBoxValuta.PackStart(Валюта, false, false, 5);
        }

        #endregion

        #region Notebook

        void CloseCurrentPageNotebook()
        {
            reportNotebook.RemovePage(reportNotebook.CurrentPage);
        }

        void CreateNotebookPage(string tabName, System.Func<Widget>? pageWidget)
        {
            ScrolledWindow scroll = new ScrolledWindow() { ShadowType = ShadowType.In };
            scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            int numPage = reportNotebook.AppendPage(scroll, new Label { Text = tabName, Expand = false, Halign = Align.Start });

            if (pageWidget != null)
                scroll.Add((Widget)pageWidget.Invoke());

            reportNotebook.ShowAll();
            reportNotebook.CurrentPage = numPage;
        }

        #endregion

        void CreateReportNotebookPage(string caption, Widget wgTree)
        {
            VBox vBox = new VBox();
            HBox hBoxButton = new HBox();

            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { CloseCurrentPageNotebook(); };

            hBoxButton.PackStart(bClose, false, false, 10);

            vBox.PackStart(hBoxButton, false, false, 10);

            ScrolledWindow scrol = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrol.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrol.Add(wgTree);

            vBox.PackStart(scrol, true, true, 0);

            CreateNotebookPage(caption, () => { return vBox; });
        }

        void OnReportOstatok(object? sender, EventArgs args)
        {
            #region SELECT

            bool isExistParent = false;

            string query = $@"
SELECT 
    РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Організація} AS Організація,
    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва,
    РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Каса} AS Каса,
    Довідник_Каси.{Каси_Const.Назва} AS Каса_Назва,
    РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Валюта} AS Валюта,
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    ROUND(SUM(РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Сума}), 2) AS Сума
FROM 
    {ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.TABLE} AS РухКоштів_Місяць
    LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = 
        РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Організація}
    LEFT JOIN {Каси_Const.TABLE} AS Довідник_Каси ON Довідник_Каси.uid = 
        РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Каса}
    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = 
        РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Валюта}
";

            #region WHERE

            //Відбір по вибраному елементу Організація
            if (!Організація.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Організації.uid = '{Організація.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу Каса
            if (!Каса.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Каси.uid = '{Каса.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу Склади
            if (!Валюта.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Валюти.uid = '{Валюта.Pointer.UnigueID}'
";
            }

            #endregion

            query += $@"
GROUP BY Організація, Організація_Назва, Каса, Каса_Назва, Валюта, Валюта_Назва
HAVING SUM(РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Сума}) != 0
ORDER BY Організація_Назва, Каса_Назва, Валюта_Назва
";
            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>();
            ВидиміКолонки.Add("Організація_Назва", "Організація");
            ВидиміКолонки.Add("Каса_Назва", "Каса");
            ВидиміКолонки.Add("Валюта_Назва", "Валюта");
            ВидиміКолонки.Add("Сума", "Сума");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>();
            КолонкиДаних.Add("Організація_Назва", "Організація");
            КолонкиДаних.Add("Каса_Назва", "Каса");
            КолонкиДаних.Add("Валюта_Назва", "Валюта");

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();

            string[] columnsName;
            List<Dictionary<string, object>> listRow;

            Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

            ListStore listStore;
            ФункціїДляЗвітів.СтворитиМодельДаних(out listStore, columnsName);

            TreeView treeView = new TreeView(listStore);
            treeView.ButtonPressEvent += ФункціїДляЗвітів.OpenPageDirectoryOrDocument;

            ФункціїДляЗвітів.СтворитиКолонкиДляДерева(treeView, columnsName, ВидиміКолонки, КолонкиДаних);
            ФункціїДляЗвітів.ЗаповнитиМодельДаними(listStore, columnsName, listRow);

            CreateReportNotebookPage("Залишки", treeView);
        }

        void OnReportOborot(object? sender, EventArgs args)
        {
            #region SELECT

            bool isExistParent = false;

            string query = $@"
WITH ostatok_month AS
(
    SELECT
        'month' AS block,
        РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Організація} AS Організація,
        РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Каса} AS Каса,
        РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Валюта} AS Валюта,
        SUM(РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Сума}) AS Залишок
    FROM 
        {ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.TABLE} AS РухКоштів_Місяць
    WHERE
        РухКоштів_Місяць.{ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Період} < @period_month_end
    GROUP BY Організація, Каса, Валюта
), 
ostatok_day AS
(
    SELECT
        'day' AS block,
        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Організація} AS Організація,
        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Каса} AS Каса,
        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Валюта} AS Валюта,
        SUM(РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Сума}) AS Залишок
    FROM 
        {ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.TABLE} AS РухКоштів_День
    WHERE
        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Період} >= @period_day_start AND
        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Період} < @period_day_end
    GROUP BY Організація, Каса, Валюта
), 
ostatok_period AS
(   
    SELECT
        'period' AS block,
        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Організація} AS Організація,
        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Каса} AS Каса,
        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Валюта} AS Валюта,
        SUM(РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Сума}) AS Залишок
    FROM 
        {ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.TABLE} AS РухКоштів_День
    WHERE
        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Період} >= @period_ostatok_start AND
        РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Період} <= @period_ostatok_end
    GROUP BY Організація, Каса, Валюта
),
ostatok_na_potshatok_periodu AS
(
    SELECT
       Організація,
       Каса,
       Валюта,
       SUM(Залишок) AS Залишок
    FROM 
    (
        SELECT * FROM ostatok_month
        UNION
        SELECT * FROM ostatok_day
    ) AS ostatok
    GROUP BY Організація, Каса, Валюта
),
ostatok_na_kinec_periodu AS
(
    SELECT
       Організація,
       Каса,
       Валюта,
       SUM(Залишок) AS Залишок
    FROM 
    (
        SELECT * FROM ostatok_month
        UNION
        SELECT * FROM ostatok_day
        UNION
        SELECT * FROM ostatok_period
    ) AS ostatok
    GROUP BY Організація, Каса, Валюта
),
oborot AS
(
    SELECT 
        РухКоштів.{РухКоштів_Const.Організація} AS Організація,
        РухКоштів.{РухКоштів_Const.Каса} AS Каса,
        РухКоштів.{РухКоштів_Const.Валюта} AS Валюта,
        SUM(CASE WHEN РухКоштів.income = true THEN РухКоштів.{РухКоштів_Const.Сума} END) AS Прихід,
        SUM(CASE WHEN РухКоштів.income = false THEN РухКоштів.{РухКоштів_Const.Сума} END) AS Розхід
    FROM
        {РухКоштів_Const.TABLE} AS РухКоштів
    WHERE
        РухКоштів.period >= @period_oborot_start AND
        РухКоштів.period <= @period_oborot_end
    GROUP BY Організація, Каса, Валюта
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
    ROUND(SUM(Прихід) - SUM(Розхід), 2) AS Оборот,
    ROUND(SUM(КінцевийЗалишок), 2) AS КінцевийЗалишок
FROM 
(
    SELECT 
        'A',
        Організація,
        Каса,
        Валюта,
        Залишок AS ПочатковийЗалишок,
        0 AS Прихід,
        0 AS Розхід,
        0 AS КінцевийЗалишок
    FROM ostatok_na_potshatok_periodu
    UNION
    SELECT
        'B',
        Організація,
        Каса,
        Валюта,
        0 AS ПочатковийЗалишок,
        0 AS Прихід,
        0 AS Розхід,
        Залишок AS КінцевийЗалишок
    FROM ostatok_na_kinec_periodu
    UNION
    SELECT
        'C',
        Організація,
        Каса,
        Валюта,
        0 AS ПочатковийЗалишок,
        Прихід AS Прихід,
        Розхід AS Розхід,
        0 AS КінцевийЗалишок
    FROM oborot
) AS ЗалишкиТаОбороти
LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = ЗалишкиТаОбороти.Організація
LEFT JOIN {Каси_Const.TABLE} AS Довідник_Каси ON Довідник_Каси.uid = ЗалишкиТаОбороти.Каса
LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = ЗалишкиТаОбороти.Валюта
";

            #region WHERE

            //Відбір по вибраному елементу Організації
            if (!Організація.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Організації.uid = '{Організація.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу Валюти
            if (!Каса.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Каси.uid = '{Каса.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу Валюти
            if (!Валюта.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Валюти.uid = '{Валюта.Pointer.UnigueID}'
";
            }

            #endregion

            query += @"
GROUP BY Організація, Організація_Назва, Каса, Каса_Назва, Валюта, Валюта_Назва
HAVING SUM(Прихід) != 0 OR SUM(Розхід) != 0
ORDER BY Організація_Назва, Каса_Назва, Валюта_Назва
";

            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>();
            ВидиміКолонки.Add("Організація_Назва", "Організація");
            ВидиміКолонки.Add("Каса_Назва", "Каса");
            ВидиміКолонки.Add("Валюта_Назва", "Валюта");
            ВидиміКолонки.Add("ПочатковийЗалишок", "На початок");
            ВидиміКолонки.Add("Прихід", "Прихід");
            ВидиміКолонки.Add("Розхід", "Розхід");
            ВидиміКолонки.Add("КінцевийЗалишок", "На кінець");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>();
            КолонкиДаних.Add("Організація_Назва", "Організація");
            КолонкиДаних.Add("Каса_Назва", "Каса");
            КолонкиДаних.Add("Валюта_Назва", "Валюта");

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("period_month_end", DateTime.Parse($"01.{ДатаПочаткуПеріоду.Value.Month}.{ДатаПочаткуПеріоду.Value.Year} 00:00:00"));

            paramQuery.Add("period_day_start", DateTime.Parse($"01.{ДатаПочаткуПеріоду.Value.Month}.{ДатаПочаткуПеріоду.Value.Year} 00:00:00"));
            paramQuery.Add("period_day_end", DateTime.Parse($"{ДатаПочаткуПеріоду.Value.Day}.{ДатаПочаткуПеріоду.Value.Month}.{ДатаПочаткуПеріоду.Value.Year} 00:00:00"));

            paramQuery.Add("period_ostatok_start", DateTime.Parse($"{ДатаПочаткуПеріоду.Value.Day}.{ДатаПочаткуПеріоду.Value.Month}.{ДатаПочаткуПеріоду.Value.Year} 00:00:00"));
            paramQuery.Add("period_ostatok_end", DateTime.Parse($"{ДатаКінцяПеріоду.Value.Day}.{ДатаКінцяПеріоду.Value.Month}.{ДатаКінцяПеріоду.Value.Year} 00:00:00"));

            paramQuery.Add("period_oborot_start", DateTime.Parse($"{ДатаПочаткуПеріоду.Value.Day}.{ДатаПочаткуПеріоду.Value.Month}.{ДатаПочаткуПеріоду.Value.Year} 00:00:00"));
            paramQuery.Add("period_oborot_end", DateTime.Parse($"{ДатаКінцяПеріоду.Value.Day}.{ДатаКінцяПеріоду.Value.Month}.{ДатаКінцяПеріоду.Value.Year} 23:59:59"));

            string[] columnsName;
            List<Dictionary<string, object>> listRow;

            Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

            ListStore listStore;
            ФункціїДляЗвітів.СтворитиМодельДаних(out listStore, columnsName);

            TreeView treeView = new TreeView(listStore);
            treeView.ButtonPressEvent += ФункціїДляЗвітів.OpenPageDirectoryOrDocument;

            ФункціїДляЗвітів.СтворитиКолонкиДляДерева(treeView, columnsName, ВидиміКолонки, КолонкиДаних);
            ФункціїДляЗвітів.ЗаповнитиМодельДаними(listStore, columnsName, listRow);

            CreateReportNotebookPage("Залишки та обороти", treeView);
        }

        void OnReportDocuments(object? sender, EventArgs args)
        {
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
        (РухКоштів.period >= @period_start AND РухКоштів.period <= @period_end)
";

            #region WHERE

            isExistParent = true;

            //Відбір по вибраному елементу Організації
            if (!Організація.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                query += $@"
РухКоштів.{РухКоштів_Const.Організація} = '{Організація.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу Валюти
            if (!Каса.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
РухКоштів.{РухКоштів_Const.Каса} = '{Каса.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу Валюти
            if (!Валюта.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
РухКоштів.{РухКоштів_Const.Валюта} = '{Валюта.Pointer.UnigueID}'
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

                string union = (counter > 0 ? "UNION" : "");
                query += $@"
{union}
SELECT 
    '{docType}' AS doctype,
    {table}.uid, 
    {table}.docname, 
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
    doctype,
    uid,
    period, 
    (CASE WHEN income = true THEN '+' ELSE '-' END) AS income,
    docname,
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

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>();
            ВидиміКолонки.Add("income", "Рух");
            ВидиміКолонки.Add("docname", "Документ");
            ВидиміКолонки.Add("Організація_Назва", "Організація");
            ВидиміКолонки.Add("Каса_Назва", "Каса");
            ВидиміКолонки.Add("Валюта_Назва", "Валюта");
            ВидиміКолонки.Add("Сума", "Сума");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>();
            КолонкиДаних.Add("Організація_Назва", "Організація");
            КолонкиДаних.Add("Каса_Назва", "Каса");
            КолонкиДаних.Add("Валюта_Назва", "Валюта");

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("period_start", DateTime.Parse($"{ДатаПочаткуПеріоду.Value.Day}.{ДатаПочаткуПеріоду.Value.Month}.{ДатаПочаткуПеріоду.Value.Year} 00:00:00"));
            paramQuery.Add("period_end", DateTime.Parse($"{ДатаКінцяПеріоду.Value.Day}.{ДатаКінцяПеріоду.Value.Month}.{ДатаКінцяПеріоду.Value.Year} 23:59:59"));

            string[] columnsName;
            List<Dictionary<string, object>> listRow;

            Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

            ListStore listStore;
            ФункціїДляЗвітів.СтворитиМодельДаних(out listStore, columnsName);

            TreeView treeView = new TreeView(listStore);
            treeView.ButtonPressEvent += ФункціїДляЗвітів.OpenPageDirectoryOrDocument;

            ФункціїДляЗвітів.СтворитиКолонкиДляДерева(treeView, columnsName, ВидиміКолонки, КолонкиДаних);
            ФункціїДляЗвітів.ЗаповнитиМодельДаними(listStore, columnsName, listRow);

            CreateReportNotebookPage("Документи", treeView);
        }

    }
}