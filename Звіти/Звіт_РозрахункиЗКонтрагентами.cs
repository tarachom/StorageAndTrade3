using Gtk;

using AccountingSoftware;
using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade
{
    class Звіт_РозрахункиЗКонтрагентами : VBox
    {
        Notebook reportNotebook;

        #region Filters

        DateTimeControl ДатаПочаткуПеріоду = new DateTimeControl() { Value = DateTime.Parse($"01.{DateTime.Now.Month}.{DateTime.Now.Year}") };
        DateTimeControl ДатаКінцяПеріоду = new DateTimeControl() { Value = DateTime.Now };

        Контрагенти_PointerControl Контрагент = new Контрагенти_PointerControl();
        Контрагенти_Папки_PointerControl Контрагент_Папка = new Контрагенти_Папки_PointerControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();

        #endregion

        public Звіт_РозрахункиЗКонтрагентами() : base()
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

            //Контрагент
            HBox hBoxKontragent = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxKontragent, false, false, 5);

            hBoxKontragent.PackStart(Контрагент, false, false, 5);

            //Контрагент папка
            HBox hBoxKontragentPapka = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxKontragentPapka, false, false, 5);

            hBoxKontragentPapka.PackStart(Контрагент_Папка, false, false, 5);
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
WITH Контрагенти AS
(
    SELECT
         Контрагент,
         Валюта,
         SUM(Сума) AS Сума
    FROM 
    (
        SELECT
            РозрахункиЗКлієнтами_Місяць.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.Контрагент} AS Контрагент,
            РозрахункиЗКлієнтами_Місяць.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.Валюта} AS Валюта, 
            РозрахункиЗКлієнтами_Місяць.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.Сума} AS Сума
        FROM 
            {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.TABLE} AS РозрахункиЗКлієнтами_Місяць
        UNION ALL
        SELECT
            РозрахункиЗПостачальниками_Місяць.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Контрагент} AS Контрагент,
            РозрахункиЗПостачальниками_Місяць.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Валюта} AS Валюта, 
            РозрахункиЗПостачальниками_Місяць.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Сума} AS Сума
        FROM 
            {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.TABLE} AS РозрахункиЗПостачальниками_Місяць
    ) AS КлієнтиТаПостачальники
";

            #region WHERE

            //Відбір по вибраному елементу Контрагенту
            if (!Контрагент.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
КлієнтиТаПостачальники.Контрагент = '{Контрагент.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу Валюти
            if (!Валюта.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
КлієнтиТаПостачальники.Валюта = '{Валюта.Pointer.UnigueID}'
";
            }

            #endregion

            query += $@"
    GROUP BY Контрагент, Валюта
    HAVING SUM(Сума) != 0
)
SELECT 
    Контрагент,
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS Контрагент_Назва,
    Валюта, 
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    ROUND(Сума, 2) AS Сума
FROM 
    Контрагенти
    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = Контрагенти.Валюта
    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = Контрагенти.Контрагент
";

            #region WHERE

            //Відбір по всіх вкладених папках вибраної папки Контрагенти
            if (!Контрагент_Папка.Pointer.IsEmpty())
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
            WHERE {Контрагенти_Папки_Const.TABLE}.uid = '{Контрагент_Папка.Pointer.UnigueID}' 
            UNION ALL
            SELECT {Контрагенти_Папки_Const.TABLE}.uid
            FROM {Контрагенти_Папки_Const.TABLE}
                JOIN r ON {Контрагенти_Папки_Const.TABLE}.{Контрагенти_Папки_Const.Родич} = r.uid
        ) SELECT uid FROM r
    )
";
            }

            #endregion

            query += $@"
ORDER BY Контрагент_Назва
";

            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>();
            ВидиміКолонки.Add("Контрагент_Назва", "Контрагент");
            ВидиміКолонки.Add("Валюта_Назва", "Валюта");
            ВидиміКолонки.Add("Сума", "Сума");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>();
            КолонкиДаних.Add("Контрагент_Назва", "Контрагент");
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
        Контрагент,
        Валюта,
        SUM(Сума) AS Сума
    FROM
    (
        SELECT
            РозрахункиЗПостачальниками_Місяць.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Контрагент} AS Контрагент,
            РозрахункиЗПостачальниками_Місяць.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Валюта} AS Валюта,
            РозрахункиЗПостачальниками_Місяць.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Сума} AS Сума
        FROM 
            {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.TABLE} AS РозрахункиЗПостачальниками_Місяць
        WHERE
            РозрахункиЗПостачальниками_Місяць.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Період} < @period_month_end
        UNION ALL
        SELECT
            РозрахункиЗКлієнтами_Місяць.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.Контрагент} AS Контрагент,
            РозрахункиЗКлієнтами_Місяць.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.Валюта} AS Валюта,
            РозрахункиЗКлієнтами_Місяць.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.Сума} AS Сума
        FROM 
            {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.TABLE} AS РозрахункиЗКлієнтами_Місяць
        WHERE
            РозрахункиЗКлієнтами_Місяць.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.Період} < @period_month_end
    ) AS КлієнтиТаПостачальники
    GROUP BY Контрагент, Валюта
), 
ostatok_day AS
(
    SELECT
        Контрагент,
        Валюта,
        SUM(Сума) AS Сума
    FROM
    (   
        SELECT
            РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Контрагент} AS Контрагент,
            РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Валюта} AS Валюта,
            РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Сума} AS Сума
        FROM 
            {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.TABLE} AS РозрахункиЗПостачальниками_День
        WHERE
            РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Період} >= @period_day_start AND
            РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Період} < @period_day_end
        UNION ALL
        SELECT
            РозрахункиЗКлієнтами_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Контрагент} AS Контрагент,
            РозрахункиЗКлієнтами_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Валюта} AS Валюта,
            РозрахункиЗКлієнтами_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Сума} AS Сума
        FROM 
            {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.TABLE} AS РозрахункиЗКлієнтами_День
        WHERE
            РозрахункиЗКлієнтами_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Період} >= @period_day_start AND
            РозрахункиЗКлієнтами_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Період} < @period_day_end
    ) AS КлієнтиТаПостачальники
    GROUP BY Контрагент, Валюта
), 
ostatok_period AS
(   
    SELECT
        Контрагент,
        Валюта,
        SUM(Сума) AS Сума
    FROM
    (   
        SELECT
            РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Контрагент} AS Контрагент,
            РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Валюта} AS Валюта,
            РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Сума} AS Сума
        FROM 
            {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.TABLE} AS РозрахункиЗПостачальниками_День
        WHERE
            РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Період} >= @period_ostatok_start AND
            РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Період} <= @period_ostatok_end
        UNION ALL        
        SELECT
            РозрахункиЗКлієнтами_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Контрагент} AS Контрагент,
            РозрахункиЗКлієнтами_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Валюта} AS Валюта,
            РозрахункиЗКлієнтами_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Сума} AS Сума
        FROM 
            {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.TABLE} AS РозрахункиЗКлієнтами_День
        WHERE
            РозрахункиЗКлієнтами_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Період} >= @period_ostatok_start AND
            РозрахункиЗКлієнтами_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Період} <= @period_ostatok_end
    ) AS КлієнтиТаПостачальники
    GROUP BY Контрагент, Валюта
),
ostatok_na_potshatok_periodu AS
(
    SELECT
       Контрагент,
       Валюта,
       SUM(Сума) AS Сума
    FROM 
    (
        SELECT * FROM ostatok_month
        UNION ALL
        SELECT * FROM ostatok_day
    ) AS ostatok
    GROUP BY Контрагент, Валюта
),
ostatok_na_kinec_periodu AS
(
    SELECT
       Контрагент,
       Валюта,
       SUM(Сума) AS Сума
    FROM 
    (
        SELECT * FROM ostatok_month
        UNION ALL
        SELECT * FROM ostatok_day
        UNION ALL
        SELECT * FROM ostatok_period
    ) AS ostatok
    GROUP BY Контрагент, Валюта
),
oborot AS
(   
    SELECT
        Контрагент,
        Валюта,
        SUM(Прихід) AS Прихід,
        SUM(Розхід) AS Розхід
    FROM
    (
        SELECT 
            РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Контрагент} AS Контрагент,
            РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Валюта} AS Валюта,
            CASE WHEN РозрахункиЗПостачальниками.income = true THEN РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} END AS Прихід,
            CASE WHEN РозрахункиЗПостачальниками.income = false THEN РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} END AS Розхід
        FROM
            {РозрахункиЗПостачальниками_Const.TABLE} AS РозрахункиЗПостачальниками
        WHERE
            РозрахункиЗПостачальниками.period >= @period_oborot_start AND
            РозрахункиЗПостачальниками.period <= @period_oborot_end
        UNION ALL
        SELECT 
            РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Контрагент} AS Контрагент,
            РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Валюта} AS Валюта,
            CASE WHEN РозрахункиЗКлієнтами.income = true THEN РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} END AS Прихід,
            CASE WHEN РозрахункиЗКлієнтами.income = false THEN РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} END AS Розхід
        FROM
            {РозрахункиЗКлієнтами_Const.TABLE} AS РозрахункиЗКлієнтами
        WHERE
            РозрахункиЗКлієнтами.period >= @period_oborot_start AND
            РозрахункиЗКлієнтами.period <= @period_oborot_end
    ) AS КлієнтиТаПостачальники
    GROUP BY Контрагент, Валюта
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
    FROM ostatok_na_potshatok_periodu
    UNION ALL
    SELECT
        Контрагент,
        Валюта,
        0 AS ПочатковийЗалишок,
        0 AS Прихід,
        0 AS Розхід,
        Сума AS КінцевийЗалишок
    FROM ostatok_na_kinec_periodu
    UNION ALL
    SELECT
        Контрагент,
        Валюта,
        0 AS ПочатковийЗалишок,
        Прихід AS Прихід,
        Розхід AS Розхід,
        0 AS КінцевийЗалишок
    FROM oborot
) AS ЗалишкиТаОбороти
LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = ЗалишкиТаОбороти.Контрагент
LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = ЗалишкиТаОбороти.Валюта
";

            #region WHERE

            //Відбір по всіх вкладених папках вибраної папки Контрагенти
            if (!Контрагент_Папка.Pointer.IsEmpty())
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
            WHERE {Контрагенти_Папки_Const.TABLE}.uid = '{Контрагент_Папка.Pointer.UnigueID}' 
            UNION ALL
            SELECT {Контрагенти_Папки_Const.TABLE}.uid
            FROM {Контрагенти_Папки_Const.TABLE}
                JOIN r ON {Контрагенти_Папки_Const.TABLE}.{Контрагенти_Папки_Const.Родич} = r.uid
        ) SELECT uid FROM r
    )
";
            }

            //Відбір по вибраному елементу Контрагенти
            if (!Контрагент.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Контрагенти.uid = '{Контрагент.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу
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
GROUP BY Контрагент, Контрагент_Назва, Валюта, Валюта_Назва
HAVING SUM(Прихід) != 0 OR SUM(Розхід) != 0
ORDER BY Контрагент_Назва, Валюта_Назва
";

            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>();
            ВидиміКолонки.Add("Контрагент_Назва", "Контрагент");
            ВидиміКолонки.Add("Валюта_Назва", "Валюта");
            ВидиміКолонки.Add("ПочатковийЗалишок", "На початок");
            ВидиміКолонки.Add("Прихід", "Прихід");
            ВидиміКолонки.Add("Розхід", "Розхід");
            ВидиміКолонки.Add("КінцевийЗалишок", "На кінець");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>();
            КолонкиДаних.Add("Контрагент_Назва", "Контрагент");
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
        period,
        owner,
        income,
        Контрагент,
        Валюта,
        Сума
    FROM
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
            (РозрахункиЗПостачальниками.period >= @period_start AND РозрахункиЗПостачальниками.period <= @period_end)
    
        UNION ALL
    
        SELECT 
            РозрахункиЗКлієнтами.period AS period,
            РозрахункиЗКлієнтами.owner AS owner,
            РозрахункиЗКлієнтами.income AS income,
            РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Контрагент} AS Контрагент,
            РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Валюта} AS Валюта,
            РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} AS Сума
        FROM
            {РозрахункиЗКлієнтами_Const.TABLE} AS РозрахункиЗКлієнтами
        WHERE
            (РозрахункиЗКлієнтами.period >= @period_start AND РозрахункиЗКлієнтами.period <= @period_end)
    ) AS КлієнтиТаПостачальники
";

            #region WHERE

            isExistParent = true;

            //Відбір по вибраному елементу Контрагенти
            if (!Контрагент.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                query += $@"
КлієнтиТаПостачальники.Контрагент = '{Контрагент.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу Валюти
            if (!Валюта.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
КлієнтиТаПостачальники.Валюта = '{Валюта.Pointer.UnigueID}'
";
            }

            #endregion

            query += $@"
),
documents AS
(";
            int counter = 0;

            #region Обєднання масивів з унікальними значеннями

            List<string> РозрахункиЗКонтрагентамиTable = new List<string>();
            List<string> РозрахункиЗКонтрагентамиType = new List<string>();

            РозрахункиЗКонтрагентамиTable.AddRange(РозрахункиЗКлієнтами_Const.AllowDocumentSpendTable);
            РозрахункиЗКонтрагентамиType.AddRange(РозрахункиЗКлієнтами_Const.AllowDocumentSpendType);

            foreach (string s in РозрахункиЗПостачальниками_Const.AllowDocumentSpendTable)
                if (!РозрахункиЗКонтрагентамиTable.Contains(s))
                    РозрахункиЗКонтрагентамиTable.Add(s);

            foreach (string s in РозрахункиЗПостачальниками_Const.AllowDocumentSpendType)
                if (!РозрахункиЗКонтрагентамиType.Contains(s))
                    РозрахункиЗКонтрагентамиType.Add(s);

            #endregion

            foreach (string table in РозрахункиЗКонтрагентамиTable)
            {
                string docType = РозрахункиЗКонтрагентамиType[counter];
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
                if (!Контрагент_Папка.Pointer.IsEmpty())
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
            WHERE {Контрагенти_Папки_Const.TABLE}.uid = '{Контрагент_Папка.Pointer.UnigueID}' 
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
    doctype,
    uid,
    period,
    (CASE WHEN income = true THEN '+' ELSE '-' END) AS income,
    docname, 
    Контрагент,
    Контрагент_Назва,
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
            ВидиміКолонки.Add("Контрагент_Назва", "Контрагент");
            ВидиміКолонки.Add("Валюта_Назва", "Валюта");
            ВидиміКолонки.Add("Сума", "Сума");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>();
            КолонкиДаних.Add("Контрагент_Назва", "Контрагент");
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