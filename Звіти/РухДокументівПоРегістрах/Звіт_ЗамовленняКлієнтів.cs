using Gtk;

using AccountingSoftware;
using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade
{
    class Звіт_ЗамовленняКлієнтів : VBox
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

        #endregion

        public Звіт_ЗамовленняКлієнтів() : base()
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
    Рег_ЗамовленняКлієнтів.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    Рег_ЗамовленняКлієнтів.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    Рег_ЗамовленняКлієнтів.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва, 
    SUM(Рег_ЗамовленняКлієнтів.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Замовлено}) AS Замовлено,
    ROUND(SUM(Рег_ЗамовленняКлієнтів.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Сума}), 2) AS Сума
FROM 
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.TABLE} AS Рег_ЗамовленняКлієнтів
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        Рег_ЗамовленняКлієнтів.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Номенклатура}
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        Рег_ЗамовленняКлієнтів.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.ХарактеристикаНоменклатури}
    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
        Рег_ЗамовленняКлієнтів.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Склад}
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

            #endregion

            query += $@"
GROUP BY Номенклатура, Номенклатура_Назва, 
         ХарактеристикаНоменклатури, ХарактеристикаНоменклатури_Назва,
         Склад, Склад_Назва
HAVING
    SUM(Рег_ЗамовленняКлієнтів.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Замовлено}) != 0
OR
    SUM(Рег_ЗамовленняКлієнтів.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Сума}) != 0   
ORDER BY Номенклатура_Назва
";
            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>();
            ВидиміКолонки.Add("Номенклатура_Назва", "Номенклатура");
            ВидиміКолонки.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");
            ВидиміКолонки.Add("Склад_Назва", "Склад");
            ВидиміКолонки.Add("Замовлено", "Замовлено");
            ВидиміКолонки.Add("Сума", "Сума");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>();
            КолонкиДаних.Add("Номенклатура_Назва", "Номенклатура");
            КолонкиДаних.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");
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

        void OnReportDocuments(object? sender, EventArgs args)
        {
            #region SELECT

            bool isExistParent = false;

            string query = $@"
WITH register AS
(
     SELECT 
        ЗамовленняКлієнтів.period AS period,
        ЗамовленняКлієнтів.owner AS owner,
        ЗамовленняКлієнтів.income AS income,
        ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.ЗамовленняКлієнта} AS ЗамовленняКлієнта,
        ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Номенклатура} AS Номенклатура,
        ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Склад} AS Склад,
        ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Замовлено} AS Замовлено,
        ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Сума} AS Сума
    FROM
        {ЗамовленняКлієнтів_Const.TABLE} AS ЗамовленняКлієнтів
    WHERE
        (ЗамовленняКлієнтів.period >= @period_start AND ЗамовленняКлієнтів.period <= @period_end)
";

            #region WHERE

            isExistParent = true;

            //Відбір по вибраному елементу Номенклатура
            if (!Номенклатура.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Номенклатура} = '{Номенклатура.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу Характеристики Номенклатури
            if (!ХарактеристикиНоменклатури.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.ХарактеристикаНоменклатури} = '{ХарактеристикиНоменклатури.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу Склади
            if (!Склад.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Склад} = '{Склад.Pointer.UnigueID}'
";
            }

            #endregion

            query += $@"
),
documents AS
(";
            int counter = 0;
            foreach (string table in ЗамовленняКлієнтів_Const.AllowDocumentSpendTable)
            {
                string docType = ЗамовленняКлієнтів_Const.AllowDocumentSpendType[counter];

                string union = (counter > 0 ? "UNION" : "");
                query += $@"
{union}
SELECT 
    '{docType}' AS doctype,
    {table}.uid, 
    {table}.docname, 
    register.period, 
    register.income, 
    register.Замовлено,
    register.Сума,
    register.ЗамовленняКлієнта,
    Документ_ЗамовленняКлієнта.{ЗамовленняКлієнта_Const.Назва} AS ЗамовленняКлієнта_Назва,
    register.Номенклатура,
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    register.ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    register.Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва
FROM register INNER JOIN {table} ON {table}.uid = register.owner
    LEFT JOIN {ЗамовленняКлієнта_Const.TABLE} AS Документ_ЗамовленняКлієнта ON Документ_ЗамовленняКлієнта.uid = register.ЗамовленняКлієнта
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = register.Номенклатура
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = register.ХарактеристикаНоменклатури
    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = register.Склад
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
    ЗамовленняКлієнта, 
    ЗамовленняКлієнта_Назва,
    Номенклатура,
    Номенклатура_Назва,
    ХарактеристикаНоменклатури,
    ХарактеристикаНоменклатури_Назва,
    Склад,
    Склад_Назва,
    Замовлено, 
    ROUND(Сума, 2) AS Сума
FROM documents
ORDER BY period ASC
";

            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>();
            ВидиміКолонки.Add("income", "Рух");
            ВидиміКолонки.Add("docname", "Документ");
            ВидиміКолонки.Add("ЗамовленняКлієнта_Назва", "Замовлення клієнта");
            ВидиміКолонки.Add("Номенклатура_Назва", "Номенклатура");
            ВидиміКолонки.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");
            ВидиміКолонки.Add("Склад_Назва", "Склад");
            ВидиміКолонки.Add("Замовлено", "Замовлено");
            ВидиміКолонки.Add("Сума", "Сума");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>();
            КолонкиДаних.Add("ЗамовленняКлієнта_Назва", "ЗамовленняКлієнта");
            КолонкиДаних.Add("Номенклатура_Назва", "Номенклатура");
            КолонкиДаних.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");
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