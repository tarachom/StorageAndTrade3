using Gtk;

using AccountingSoftware;
using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade
{
    class Звіт_ТовариНаСкладах : VBox
    {
        Notebook reportNotebook;

        #region Filters

        DateTimeControl ДатаПочаткуПеріоду = new DateTimeControl() { Value = DateTime.Parse($"01.{DateTime.Now.Month}.{DateTime.Now.Year}") };
        DateTimeControl ДатаКінцяПеріоду = new DateTimeControl() { Value = DateTime.Now };

        Номенклатура_PointerControl Номенклатура = new Номенклатура_PointerControl();
        Номенклатура_Папки_PointerControl Номенклатура_Папка = new Номенклатура_Папки_PointerControl() { Caption = "Номенклатура папка:" };
        ХарактеристикиНоменклатури_PointerControl ХарактеристикиНоменклатури = new ХарактеристикиНоменклатури_PointerControl();
        Склади_PointerControl Склад = new Склади_PointerControl();
        Склади_Папки_PointerControl Склад_Папка = new Склади_Папки_PointerControl() { Caption = "Склад папка:" };
        СеріїНоменклатури_PointerControl Серія = new СеріїНоменклатури_PointerControl();

        #endregion

        public Звіт_ТовариНаСкладах() : base()
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

            //Номенклатура
            HBox hBoxNomenklatura = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxNomenklatura, false, false, 5);

            hBoxNomenklatura.PackStart(Номенклатура, false, false, 5);

            //ХарактеристикиНоменклатури
            HBox hBoxHarakterystyka = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxHarakterystyka, false, false, 5);

            hBoxHarakterystyka.PackStart(ХарактеристикиНоменклатури, false, false, 5);

            //Склад
            HBox hBoxSkald = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSkald, false, false, 5);

            hBoxSkald.PackStart(Склад, false, false, 5);
        }

        void CreateContainer2(VBox vBox)
        {
            //Номенклатура папка
            HBox hBoxNomenklaturaPapka = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxNomenklaturaPapka, false, false, 5);

            hBoxNomenklaturaPapka.PackStart(Номенклатура_Папка, false, false, 5);

            //Склад папка
            HBox hBoxSkaldPapka = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSkaldPapka, false, false, 5);

            hBoxSkaldPapka.PackStart(Склад_Папка, false, false, 5);
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
    ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Серія} AS Серія,
    Довідник_СеріїНоменклатури.{СеріїНоменклатури_Const.Номер} AS Серія_Номер,
    SUM(ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ВНаявності}) AS ВНаявності
FROM 
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.TABLE} AS ТовариНаСкладах_Місяць
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Номенклатура}
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ХарактеристикаНоменклатури}
    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
        ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Склад}
    LEFT JOIN {СеріїНоменклатури_Const.TABLE} AS Довідник_СеріїНоменклатури ON Довідник_СеріїНоменклатури.uid = 
        ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Серія}
";
            #region WHERE

            //Відбір по всіх вкладених папках вибраної папки Номенклатури
            if (!Номенклатура_Папка.Pointer.IsEmpty())
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
            WHERE {Номенклатура_Папки_Const.TABLE}.uid = '{Номенклатура_Папка.Pointer.UnigueID}' 
            UNION ALL
            SELECT {Номенклатура_Папки_Const.TABLE}.uid
            FROM {Номенклатура_Папки_Const.TABLE}
                JOIN r ON {Номенклатура_Папки_Const.TABLE}.{Номенклатура_Папки_Const.Родич} = r.uid
        ) SELECT uid FROM r
    )
";
            }

            //Відбір по вибраному елементу Номенклатура
            if (!Номенклатура.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Номенклатура.uid = '{Номенклатура.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу Характеристики Номенклатури
            if (!ХарактеристикиНоменклатури.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_ХарактеристикиНоменклатури.uid = '{ХарактеристикиНоменклатури.Pointer.UnigueID}'
";
            }

            //Відбір по всіх вкладених папках вибраної папки Склади
            if (!Склад_Папка.Pointer.IsEmpty())
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
            WHERE {Склади_Папки_Const.TABLE}.uid = '{Склад_Папка.Pointer.UnigueID}' 
            UNION ALL
            SELECT {Склади_Папки_Const.TABLE}.uid
            FROM {Склади_Папки_Const.TABLE}
                JOIN r ON {Склади_Папки_Const.TABLE}.{Склади_Папки_Const.Родич} = r.uid
        ) SELECT uid FROM r
    )
";
            }

            //Відбір по вибраному елементу Склади
            if (!Склад.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Склади.uid = '{Склад.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу Серії Номенклатури
            if (!Серія.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_СеріїНоменклатури.uid = '{Серія.Pointer.UnigueID}'
";
            }

            #endregion

            query += $@"
GROUP BY Номенклатура, Номенклатура_Назва, 
         ХарактеристикаНоменклатури, ХарактеристикаНоменклатури_Назва,
         Склад, Склад_Назва,
         Серія, Серія_Номер
HAVING
    SUM(ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ВНаявності}) != 0  
ORDER BY Номенклатура_Назва
";
            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>();
            ВидиміКолонки.Add("Номенклатура_Назва", "Номенклатура");
            ВидиміКолонки.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");
            ВидиміКолонки.Add("Склад_Назва", "Склад");
            ВидиміКолонки.Add("Серія_Номер", "Серія");
            ВидиміКолонки.Add("ВНаявності", "В наявності");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>();
            КолонкиДаних.Add("Номенклатура_Назва", "Номенклатура");
            КолонкиДаних.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");
            КолонкиДаних.Add("Серія_Номер", "Серія");
            КолонкиДаних.Add("Склад_Назва", "Склад");

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
        ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Номенклатура} AS Номенклатура,
        ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Склад} AS Склад,
        ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Серія} AS Серія,
        SUM(ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ВНаявності}) AS ВНаявності
    FROM 
        {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.TABLE} AS ТовариНаСкладах_Місяць
    WHERE
        ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Період} < @period_month_end
    GROUP BY Номенклатура, ХарактеристикаНоменклатури, Склад, Серія
), 
ostatok_day AS
(
    SELECT
        'day' AS block,
        ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Номенклатура} AS Номенклатура,
        ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Склад} AS Склад,
        ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Серія} AS Серія,
        SUM(ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ВНаявності}) AS ВНаявності
    FROM 
        {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.TABLE} AS ТовариНаСкладах_День
    WHERE
        ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Період} >= @period_day_start AND
        ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Період} < @period_day_end
    GROUP BY Номенклатура, ХарактеристикаНоменклатури, Склад, Серія
), 
ostatok_period AS
(   
    SELECT
        'period' AS block,
        ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Номенклатура} AS Номенклатура,
        ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Склад} AS Склад,
        ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Серія} AS Серія,
        SUM(ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ВНаявності}) AS ВНаявності
    FROM 
        {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.TABLE} AS ТовариНаСкладах_День
    WHERE
        ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Період} >= @period_ostatok_start AND
        ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Період} <= @period_ostatok_end
    GROUP BY Номенклатура, ХарактеристикаНоменклатури, Склад, Серія
),
ostatok_na_potshatok_periodu AS
(
    SELECT
       Номенклатура,
       ХарактеристикаНоменклатури,
       Склад,
       Серія,
       SUM(ВНаявності) AS ВНаявності
    FROM 
    (
        SELECT * FROM ostatok_month
        UNION
        SELECT * FROM ostatok_day
    ) AS ostatok
    GROUP BY Номенклатура, ХарактеристикаНоменклатури, Склад, Серія
),
ostatok_na_kinec_periodu AS
(
    SELECT
       Номенклатура,
       ХарактеристикаНоменклатури,
       Склад,
       Серія,
       SUM(ВНаявності) AS ВНаявності
    FROM 
    (
        SELECT * FROM ostatok_month
        UNION
        SELECT * FROM ostatok_day
        UNION
        SELECT * FROM ostatok_period
    ) AS ostatok
    GROUP BY Номенклатура, ХарактеристикаНоменклатури, Склад, Серія
),
oborot AS
(
    SELECT 
        ТовариНаСкладах.{ТовариНаСкладах_Const.Номенклатура} AS Номенклатура,
        ТовариНаСкладах.{ТовариНаСкладах_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ТовариНаСкладах.{ТовариНаСкладах_Const.Склад} AS Склад,
        ТовариНаСкладах.{ТовариНаСкладах_Const.Серія} AS Серія,
        SUM(CASE WHEN ТовариНаСкладах.income = true THEN ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} END) AS Прихід,
        SUM(CASE WHEN ТовариНаСкладах.income = false THEN ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} END) AS Розхід
    FROM
        {ТовариНаСкладах_Const.TABLE} AS ТовариНаСкладах
    WHERE
        ТовариНаСкладах.period >= @period_oborot_start AND
        ТовариНаСкладах.period <= @period_oborot_end
    GROUP BY Номенклатура, ХарактеристикаНоменклатури, Склад, Серія
)
SELECT 
    Номенклатура,
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Серія,
    Довідник_СеріїНоменклатури.{СеріїНоменклатури_Const.Номер} AS Серія_Номер,
    SUM(ПочатковийЗалишок) AS ПочатковийЗалишок,
    SUM(Прихід) AS Прихід,
    SUM(Розхід) AS Розхід,
    SUM(КінцевийЗалишок) AS КінцевийЗалишок
FROM 
(
    SELECT 
        Номенклатура,
        ХарактеристикаНоменклатури,
        Склад,
        Серія,
        ВНаявності AS ПочатковийЗалишок,
        0 AS Прихід,
        0 AS Розхід,
        0 AS КінцевийЗалишок
    FROM ostatok_na_potshatok_periodu
    UNION ALL
    SELECT
        Номенклатура,
        ХарактеристикаНоменклатури,
        Склад,
        Серія,
        0 AS ПочатковийЗалишок,
        0 AS Прихід,
        0 AS Розхід,
        ВНаявності AS КінцевийЗалишок
    FROM ostatok_na_kinec_periodu
    UNION ALL
    SELECT
        Номенклатура,
        ХарактеристикаНоменклатури,
        Склад,
        Серія,
        0 AS ПочатковийЗалишок,
        Прихід AS Прихід,
        Розхід AS Розхід,
        0 AS КінцевийЗалишок
    FROM oborot
) AS ЗалишкиТаОбороти
LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = ЗалишкиТаОбороти.Номенклатура
LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = ЗалишкиТаОбороти.ХарактеристикаНоменклатури
LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = ЗалишкиТаОбороти.Склад
LEFT JOIN {СеріїНоменклатури_Const.TABLE} AS Довідник_СеріїНоменклатури ON Довідник_СеріїНоменклатури.uid = ЗалишкиТаОбороти.Серія
";

            #region WHERE

            //Відбір по всіх вкладених папках вибраної папки Номенклатури
            if (!Номенклатура_Папка.Pointer.IsEmpty())
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
            WHERE {Номенклатура_Папки_Const.TABLE}.uid = '{Номенклатура_Папка.Pointer.UnigueID}' 
            UNION ALL
            SELECT {Номенклатура_Папки_Const.TABLE}.uid
            FROM {Номенклатура_Папки_Const.TABLE}
                JOIN r ON {Номенклатура_Папки_Const.TABLE}.{Номенклатура_Папки_Const.Родич} = r.uid
        ) SELECT uid FROM r
    )
";
            }

            //Відбір по вибраному елементу Номенклатура
            if (!Номенклатура.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Номенклатура.uid = '{Номенклатура.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу Характеристики Номенклатури
            if (!ХарактеристикиНоменклатури.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_ХарактеристикиНоменклатури.uid = '{ХарактеристикиНоменклатури.Pointer.UnigueID}'
";
            }

            //Відбір по всіх вкладених папках вибраної папки Склади
            if (!Склад_Папка.Pointer.IsEmpty())
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
            WHERE {Склади_Папки_Const.TABLE}.uid = '{Склад_Папка.Pointer.UnigueID}' 
            UNION ALL
            SELECT {Склади_Папки_Const.TABLE}.uid
            FROM {Склади_Папки_Const.TABLE}
                JOIN r ON {Склади_Папки_Const.TABLE}.{Склади_Папки_Const.Родич} = r.uid
        ) SELECT uid FROM r
    )
";
            }

            //Відбір по вибраному елементу Склади
            if (!Склад.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Склади.uid = '{Склад.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу Серії Номенклатури
            if (!Серія.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_СеріїНоменклатури.uid = '{Серія.Pointer.UnigueID}'
";
            }

            #endregion

            query += @"
GROUP BY Номенклатура, Номенклатура_Назва, ХарактеристикаНоменклатури, ХарактеристикаНоменклатури_Назва, Склад, Склад_Назва, Серія, Серія_Номер
HAVING SUM(Прихід) != 0 OR SUM(Розхід) != 0
ORDER BY Номенклатура_Назва, ХарактеристикаНоменклатури, Склад_Назва
";

            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>();
            ВидиміКолонки.Add("Номенклатура_Назва", "Номенклатура");
            ВидиміКолонки.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");
            ВидиміКолонки.Add("Склад_Назва", "Склад");
            ВидиміКолонки.Add("Серія_Номер", "Серія");
            ВидиміКолонки.Add("ПочатковийЗалишок", "На початок");
            ВидиміКолонки.Add("Прихід", "Прихід");
            ВидиміКолонки.Add("Розхід", "Розхід");
            ВидиміКолонки.Add("КінцевийЗалишок", "На кінець");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>();
            КолонкиДаних.Add("Номенклатура_Назва", "Номенклатура");
            КолонкиДаних.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");
            КолонкиДаних.Add("Серія_Номер", "Серія");
            КолонкиДаних.Add("Склад_Назва", "Склад");

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
        ТовариНаСкладах.period AS period,
        ТовариНаСкладах.owner AS owner,
        ТовариНаСкладах.income AS income,
        ТовариНаСкладах.{ТовариНаСкладах_Const.Номенклатура} AS Номенклатура,
        ТовариНаСкладах.{ТовариНаСкладах_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ТовариНаСкладах.{ТовариНаСкладах_Const.Склад} AS Склад,
        ТовариНаСкладах.{ТовариНаСкладах_Const.Серія} AS Серія,
        ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} AS ВНаявності
    FROM
        {ТовариНаСкладах_Const.TABLE} AS ТовариНаСкладах
    WHERE
        (ТовариНаСкладах.period >= @period_start AND ТовариНаСкладах.period <= @period_end)
";

            #region WHERE

            isExistParent = true;

            //Відбір по вибраному елементу Номенклатура
            if (!Номенклатура.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ТовариНаСкладах.{ТовариНаСкладах_Const.Номенклатура} = '{Номенклатура.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу Характеристики Номенклатури
            if (!ХарактеристикиНоменклатури.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ТовариНаСкладах.{ТовариНаСкладах_Const.ХарактеристикаНоменклатури}= '{ХарактеристикиНоменклатури.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу Склади
            if (!Склад.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ТовариНаСкладах.{ТовариНаСкладах_Const.Склад} = '{Склад.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу Серії Номенклатури
            if (!Серія.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ТовариНаСкладах.{ТовариНаСкладах_Const.Серія} = '{Серія.Pointer.UnigueID}'
";
            }

            #endregion

            query += $@"
),
documents AS
(";
            int counter = 0;
            foreach (string table in ТовариНаСкладах_Const.AllowDocumentSpendTable)
            {
                string docType = ТовариНаСкладах_Const.AllowDocumentSpendType[counter];

                string union = (counter > 0 ? "UNION" : "");
                query += $@"
{union}
SELECT 
    '{docType}' AS doctype,
    {table}.uid, 
    {table}.docname, 
    register.period, 
    register.income, 
    register.ВНаявності,
    register.Номенклатура,
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    register.ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    register.Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    register.Серія,
    Довідник_СеріїНоменклатури.{СеріїНоменклатури_Const.Номер} AS Серія_Номер
FROM register INNER JOIN {table} ON {table}.uid = register.owner
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = register.Номенклатура
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = register.ХарактеристикаНоменклатури
    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = register.Склад
    LEFT JOIN {СеріїНоменклатури_Const.TABLE} AS Довідник_СеріїНоменклатури ON Довідник_СеріїНоменклатури.uid = register.Серія
";

                #region WHERE

                isExistParent = false;

                //Відбір по всіх вкладених папках вибраної папки Номенклатури
                if (!Номенклатура_Папка.Pointer.IsEmpty())
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
            WHERE {Номенклатура_Папки_Const.TABLE}.uid = '{Номенклатура_Папка.Pointer.UnigueID}' 
            UNION ALL
            SELECT {Номенклатура_Папки_Const.TABLE}.uid
            FROM {Номенклатура_Папки_Const.TABLE}
                JOIN r ON {Номенклатура_Папки_Const.TABLE}.{Номенклатура_Папки_Const.Родич} = r.uid
        ) SELECT uid FROM r
    )
";
                }

                //Відбір по всіх вкладених папках вибраної папки Склади
                if (!Склад_Папка.Pointer.IsEmpty())
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
            WHERE {Склади_Папки_Const.TABLE}.uid = '{Склад_Папка.Pointer.UnigueID}' 
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
    doctype,
    uid,
    period,
    (CASE WHEN income = true THEN '+' ELSE '-' END) AS income,
    docname,  
    Номенклатура,
    Номенклатура_Назва,
    ХарактеристикаНоменклатури,
    ХарактеристикаНоменклатури_Назва,
    Склад,
    Склад_Назва,
    Серія,
    Серія_Номер,
    ВНаявності
FROM documents
ORDER BY period ASC
";

            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>();
            ВидиміКолонки.Add("income", "Рух");
            ВидиміКолонки.Add("docname", "Документ");
            ВидиміКолонки.Add("Номенклатура_Назва", "Номенклатура");
            ВидиміКолонки.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");
            ВидиміКолонки.Add("Склад_Назва", "Склад");
            ВидиміКолонки.Add("Серія_Номер", "Серія");
            ВидиміКолонки.Add("ВНаявності", "В наявності");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>();
            КолонкиДаних.Add("Номенклатура_Назва", "Номенклатура");
            КолонкиДаних.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");
            КолонкиДаних.Add("Серія_Номер", "Серія");
            КолонкиДаних.Add("Склад_Назва", "Склад");

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