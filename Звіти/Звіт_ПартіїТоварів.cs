using Gtk;

using StorageAndTrade_1_0;
using Константи = StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade
{
    class Звіт_ПартіїТоварів : VBox
    {
        Notebook reportNotebook;

        #region Filters

        DateTimeControl ДатаПочатокПеріоду = new DateTimeControl() { OnlyDate = true, Value = DateTime.Parse($"01.{DateTime.Now.Month}.{DateTime.Now.Year}") };
        DateTimeControl ДатаКінецьПеріоду = new DateTimeControl() { OnlyDate = true, Value = DateTime.Now };

        Організації_PointerControl Організація = new Організації_PointerControl();
        Номенклатура_PointerControl Номенклатура = new Номенклатура_PointerControl();
        Номенклатура_Папки_PointerControl Номенклатура_Папка = new Номенклатура_Папки_PointerControl() { Caption = "Номенклатура папка:" };
        ХарактеристикиНоменклатури_PointerControl ХарактеристикиНоменклатури = new ХарактеристикиНоменклатури_PointerControl();
        Склади_PointerControl Склад = new Склади_PointerControl();
        Склади_Папки_PointerControl Склад_Папка = new Склади_Папки_PointerControl() { Caption = "Склад папка:" };
        СеріїНоменклатури_PointerControl Серія = new СеріїНоменклатури_PointerControl();

        #endregion

        public Звіт_ПартіїТоварів() : base()
        {
            //Кнопки
            HBox hBoxBotton = new HBox();

            //1
            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

            hBoxBotton.PackStart(bClose, false, false, 10);

            //2
            Button bOstatok = new Button("Залишки");
            bOstatok.Clicked += OnReport_Залишки;

            hBoxBotton.PackStart(bOstatok, false, false, 10);

            //3
            Button bOborot = new Button("Залишки та обороти");
            bOborot.Clicked += OnReport_ЗалишкиТаОбороти;

            hBoxBotton.PackStart(bOborot, false, false, 10);

            //4
            Button bDocuments = new Button("Документи");
            bDocuments.Clicked += OnReport_Документи;

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
            hBoxPeriod.PackStart(ДатаПочатокПеріоду, false, false, 5);
            hBoxPeriod.PackStart(new Label(" по "), false, false, 5);
            hBoxPeriod.PackStart(ДатаКінецьПеріоду, false, false, 5);
            vBox.PackStart(hBoxPeriod, false, false, 5);

            //Організація
            HBox hBoxOrganization = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxOrganization, false, false, 5);

            hBoxOrganization.PackStart(Організація, false, false, 5);

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

            //Серія
            HBox hBoxSeria = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSeria, false, false, 5);

            hBoxSeria.PackStart(Серія, false, false, 5);
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

        void OnReport_Залишки(object? sender, EventArgs args)
        {
            #region SELECT

            bool isExistParent = false;

            string query = $@"
SELECT 
    ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Організація} AS Організація, 
    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва, 
    ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.ПартіяТоварівКомпозит} AS ПартіяТоварівКомпозит,
    Довідник_ПартіяТоварівКомпозит.{ПартіяТоварівКомпозит_Const.Назва} AS ПартіяТоварівКомпозит_Назва,
    Довідник_ПартіяТоварівКомпозит.{ПартіяТоварівКомпозит_Const.Дата} AS ПартіяТоварівКомпозит_Дата,
    ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Серія} AS Серія,
    Довідник_СеріїНоменклатури.{СеріїНоменклатури_Const.Номер} AS Серія_Номер,
    ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    ROUND(SUM(ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Кількість}), 2) AS Кількість,
    ROUND(SUM(ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Собівартість}), 2) AS Собівартість
FROM 
    {ПартіїТоварів_Залишки_TablePart.TABLE} AS ПартіїТоварів
    LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = 
        ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Організація}
    LEFT JOIN {ПартіяТоварівКомпозит_Const.TABLE} AS Довідник_ПартіяТоварівКомпозит ON Довідник_ПартіяТоварівКомпозит.uid = 
        ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.ПартіяТоварівКомпозит}
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Номенклатура}
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.ХарактеристикаНоменклатури}
    LEFT JOIN {СеріїНоменклатури_Const.TABLE} AS Довідник_СеріїНоменклатури ON Довідник_СеріїНоменклатури.uid = 
        ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Серія}
    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
        ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Склад}
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
GROUP BY Організація, Організація_Назва, 
         ПартіяТоварівКомпозит, ПартіяТоварівКомпозит_Назва, ПартіяТоварівКомпозит_Дата,
         Номенклатура, Номенклатура_Назва, 
         ХарактеристикаНоменклатури, ХарактеристикаНоменклатури_Назва,
         Серія, Серія_Номер,
         Склад, Склад_Назва
HAVING
    SUM(ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Кількість}) != 0 OR
    SUM(ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Собівартість}) != 0   
ORDER BY Організація_Назва, ПартіяТоварівКомпозит_Дата ASC, Номенклатура_Назва, ХарактеристикаНоменклатури_Назва
";

            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>();
            ВидиміКолонки.Add("Організація_Назва", "Організація");
            ВидиміКолонки.Add("ПартіяТоварівКомпозит_Назва", "Партія");
            ВидиміКолонки.Add("Номенклатура_Назва", "Номенклатура");
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const) ВидиміКолонки.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");
            if (Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const) ВидиміКолонки.Add("Серія_Номер", "Серія");
            ВидиміКолонки.Add("Склад_Назва", "Склад");
            ВидиміКолонки.Add("Кількість", "Кількість");
            ВидиміКолонки.Add("Собівартість", "Собівартість");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>();
            КолонкиДаних.Add("Організація_Назва", "Організація");
            КолонкиДаних.Add("ПартіяТоварівКомпозит_Назва", "ПартіяТоварівКомпозит");
            КолонкиДаних.Add("Номенклатура_Назва", "Номенклатура");
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const) КолонкиДаних.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");
            if (Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const) КолонкиДаних.Add("Серія_Номер", "Серія");
            КолонкиДаних.Add("Склад_Назва", "Склад");

            Dictionary<string, float> ПозиціяТекстуВКолонці = new Dictionary<string, float>();
            ПозиціяТекстуВКолонці.Add("Кількість", 1);
            ПозиціяТекстуВКолонці.Add("Собівартість", 1);

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();

            string[] columnsName;
            List<Dictionary<string, object>> listRow;

            Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

            ListStore listStore;
            ФункціїДляЗвітів.СтворитиМодельДаних(out listStore, columnsName);

            TreeView treeView = new TreeView(listStore);
            treeView.ButtonPressEvent += ФункціїДляЗвітів.OpenPageDirectoryOrDocument;

            ФункціїДляЗвітів.СтворитиКолонкиДляДерева(treeView, columnsName, ВидиміКолонки, КолонкиДаних, ПозиціяТекстуВКолонці);
            ФункціїДляЗвітів.ЗаповнитиМодельДаними(listStore, columnsName, listRow);

            CreateReportNotebookPage("Залишки", treeView);
        }

        void OnReport_ЗалишкиТаОбороти(object? sender, EventArgs args)
        {
            #region SELECT

            bool isExistParent = false;

            string query = $@"
WITH 
ПочатковийЗалишок AS
(
    SELECT 
        ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Організація} AS Організація, 
        ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.ПартіяТоварівКомпозит} AS ПартіяТоварівКомпозит,
        ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Номенклатура} AS Номенклатура, 
        ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Серія} AS Серія,
        ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Склад} AS Склад,
        SUM(ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Кількість}) AS Кількість,
        SUM(ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Собівартість}) AS Собівартість
    FROM 
        {ПартіїТоварів_Залишки_TablePart.TABLE} AS ПартіїТоварів
    WHERE
        ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Період} < @ПочатокПеріоду
    GROUP BY Організація, ПартіяТоварівКомпозит, Номенклатура, ХарактеристикаНоменклатури, Серія, Склад
),
ЗалишкиТаОборотиЗаПеріод AS
(
    SELECT 
        ПартіїТоварів.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.Організація} AS Організація, 
        ПартіїТоварів.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.ПартіяТоварівКомпозит} AS ПартіяТоварівКомпозит,
        ПартіїТоварів.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.Номенклатура} AS Номенклатура, 
        ПартіїТоварів.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ПартіїТоварів.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.Серія} AS Серія,
        ПартіїТоварів.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.Склад} AS Склад,
        SUM(ПартіїТоварів.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.КількістьПрихід}) AS КількістьПрихід,
        SUM(ПартіїТоварів.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.КількістьРозхід}) AS КількістьРозхід,
        SUM(ПартіїТоварів.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.КількістьЗалишок}) AS КількістьЗалишок,
        SUM(ПартіїТоварів.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.СобівартістьПрихід}) AS СобівартістьПрихід,
        SUM(ПартіїТоварів.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.СобівартістьРозхід}) AS СобівартістьРозхід,
        SUM(ПартіїТоварів.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.СобівартістьЗалишок}) AS СобівартістьЗалишок
    FROM 
        {ПартіїТоварів_ЗалишкиТаОбороти_TablePart.TABLE} AS ПартіїТоварів
    WHERE
        ПартіїТоварів.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.Період} >= @ПочатокПеріоду AND
        ПартіїТоварів.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.Період} <= @КінецьПеріоду
    GROUP BY Організація, ПартіяТоварівКомпозит, Номенклатура, ХарактеристикаНоменклатури, Серія, Склад
),
КінцевийЗалишок AS
(
    SELECT 
        Організація,
        ПартіяТоварівКомпозит,
        Номенклатура,
        ХарактеристикаНоменклатури,
        Серія,
        Склад,
        Кількість AS КількістьЗалишок,
        Собівартість AS СобівартістьЗалишок
    FROM ПочатковийЗалишок

    UNION ALL

    SELECT
        Організація,
        ПартіяТоварівКомпозит,
        Номенклатура,
        ХарактеристикаНоменклатури,
        Серія,
        Склад,
        КількістьЗалишок,
        СобівартістьЗалишок
    FROM ЗалишкиТаОборотиЗаПеріод
)
SELECT 
    Організація,
    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва, 
    ПартіяТоварівКомпозит,
    Довідник_ПартіяТоварівКомпозит.{ПартіяТоварівКомпозит_Const.Назва} AS ПартіяТоварівКомпозит_Назва,
    Довідник_ПартіяТоварівКомпозит.{ПартіяТоварівКомпозит_Const.Дата} AS ПартіяТоварівКомпозит_Дата,
    Номенклатура,
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    Серія,
    Довідник_СеріїНоменклатури.{СеріїНоменклатури_Const.Номер} AS Серія_Номер,
    Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    ROUND(SUM(КількістьПочатковийЗалишок), 2) AS КількістьПочатковийЗалишок,
    ROUND(SUM(СобівартістьПочатковийЗалишок), 2) AS СобівартістьПочатковийЗалишок,
    ROUND(SUM(КількістьПрихід), 2) AS КількістьПрихід,
    ROUND(SUM(КількістьРозхід), 2) AS КількістьРозхід,
    ROUND(SUM(СобівартістьПрихід), 2) AS СобівартістьПрихід,
    ROUND(SUM(СобівартістьРозхід), 2) AS СобівартістьРозхід,
    ROUND(SUM(КількістьКінцевийЗалишок), 2) AS КількістьКінцевийЗалишок,
    ROUND(SUM(СобівартістьКінцевийЗалишок), 2) AS СобівартістьКінцевийЗалишок
FROM 
(
    SELECT 
        Організація,
        ПартіяТоварівКомпозит,
        Номенклатура,
        ХарактеристикаНоменклатури,
        Серія,
        Склад,
        Кількість AS КількістьПочатковийЗалишок,
        Собівартість AS СобівартістьПочатковийЗалишок,
        0 AS КількістьПрихід,
        0 AS КількістьРозхід,
        0 AS СобівартістьПрихід,
        0 AS СобівартістьРозхід,
        0 AS КількістьКінцевийЗалишок,
        0 AS СобівартістьКінцевийЗалишок
    FROM ПочатковийЗалишок

    UNION ALL

    SELECT
        Організація,
        ПартіяТоварівКомпозит,
        Номенклатура,
        ХарактеристикаНоменклатури,
        Серія,
        Склад,
        0 AS КількістьПочатковийЗалишок,
        0 AS СобівартістьПочатковийЗалишок,
        КількістьПрихід AS КількістьПрихід,
        КількістьРозхід AS КількістьРозхід,
        СобівартістьПрихід AS СобівартістьПрихід,
        СобівартістьРозхід AS СобівартістьРозхід,
        0 AS КількістьКінцевийЗалишок,
        0 AS СобівартістьКінцевийЗалишок
    FROM ЗалишкиТаОборотиЗаПеріод

    UNION ALL

    SELECT 
        Організація,
        ПартіяТоварівКомпозит,
        Номенклатура,
        ХарактеристикаНоменклатури,
        Серія,
        Склад,
        0 AS КількістьПочатковийЗалишок,
        0 AS СобівартістьПочатковийЗалишок,
        0 AS КількістьПрихід,
        0 AS КількістьРозхід,
        0 AS СобівартістьПрихід,
        0 AS СобівартістьРозхід,
        КількістьЗалишок AS КількістьКінцевийЗалишок,
        СобівартістьЗалишок AS СобівартістьКінцевийЗалишок
    FROM КінцевийЗалишок
) AS ЗалишкиТаОбороти
LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = ЗалишкиТаОбороти.Організація
LEFT JOIN {ПартіяТоварівКомпозит_Const.TABLE} AS Довідник_ПартіяТоварівКомпозит ON Довідник_ПартіяТоварівКомпозит.uid = ЗалишкиТаОбороти.ПартіяТоварівКомпозит
LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = ЗалишкиТаОбороти.Номенклатура
LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = ЗалишкиТаОбороти.ХарактеристикаНоменклатури
LEFT JOIN {СеріїНоменклатури_Const.TABLE} AS Довідник_СеріїНоменклатури ON Довідник_СеріїНоменклатури.uid = ЗалишкиТаОбороти.Серія
LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = ЗалишкиТаОбороти.Склад
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
GROUP BY 
    Організація, Організація_Назва, 
    ПартіяТоварівКомпозит, ПартіяТоварівКомпозит_Назва, ПартіяТоварівКомпозит_Дата,
    Номенклатура, Номенклатура_Назва, 
    ХарактеристикаНоменклатури, ХарактеристикаНоменклатури_Назва, 
    Серія, Серія_Номер,
    Склад, Склад_Назва
HAVING 
    SUM(КількістьПрихід) != 0 OR SUM(КількістьРозхід) != 0 OR
    SUM(СобівартістьПрихід) != 0 OR SUM(СобівартістьРозхід) != 0
ORDER BY Організація_Назва, ПартіяТоварівКомпозит_Дата, Номенклатура_Назва, ХарактеристикаНоменклатури_Назва
";

            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>();
            ВидиміКолонки.Add("Організація_Назва", "Організація");
            ВидиміКолонки.Add("ПартіяТоварівКомпозит_Назва", "Партія");
            ВидиміКолонки.Add("Номенклатура_Назва", "Номенклатура");
            ВидиміКолонки.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");
            ВидиміКолонки.Add("Серія_Номер", "Серія");
            ВидиміКолонки.Add("Склад_Назва", "Склад");
            ВидиміКолонки.Add("КількістьПочатковийЗалишок", "Кількість");
            ВидиміКолонки.Add("СобівартістьПочатковийЗалишок", "Собівартість");
            ВидиміКолонки.Add("КількістьПрихід", "Кількість +");
            ВидиміКолонки.Add("КількістьРозхід", "Кількість -");
            ВидиміКолонки.Add("СобівартістьПрихід", "Собівартість +");
            ВидиміКолонки.Add("СобівартістьРозхід", "Собівартість -");
            ВидиміКолонки.Add("КількістьКінцевийЗалишок", "Кількість");
            ВидиміКолонки.Add("СобівартістьКінцевийЗалишок", "Собівартість");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>();
            КолонкиДаних.Add("Номенклатура_Назва", "Номенклатура");
            КолонкиДаних.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");
            КолонкиДаних.Add("Серія_Номер", "Серія");
            КолонкиДаних.Add("Склад_Назва", "Склад");

            Dictionary<string, float> ПозиціяТекстуВКолонці = new Dictionary<string, float>();
            ПозиціяТекстуВКолонці.Add("КількістьПочатковийЗалишок", 1);
            ПозиціяТекстуВКолонці.Add("СобівартістьПочатковийЗалишок", 1);
            ПозиціяТекстуВКолонці.Add("КількістьПрихід", 1);
            ПозиціяТекстуВКолонці.Add("КількістьРозхід", 1);
            ПозиціяТекстуВКолонці.Add("СобівартістьПрихід", 1);
            ПозиціяТекстуВКолонці.Add("СобівартістьРозхід", 1);
            ПозиціяТекстуВКолонці.Add("КількістьКінцевийЗалишок", 1);
            ПозиціяТекстуВКолонці.Add("СобівартістьКінцевийЗалишок", 1);

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("ПочатокПеріоду", ДатаПочатокПеріоду.Value);
            paramQuery.Add("КінецьПеріоду", ДатаКінецьПеріоду.Value);

            string[] columnsName;
            List<Dictionary<string, object>> listRow;

            Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

            ListStore listStore;
            ФункціїДляЗвітів.СтворитиМодельДаних(out listStore, columnsName);

            TreeView treeView = new TreeView(listStore);
            treeView.ButtonPressEvent += ФункціїДляЗвітів.OpenPageDirectoryOrDocument;

            ФункціїДляЗвітів.СтворитиКолонкиДляДерева(treeView, columnsName, ВидиміКолонки, КолонкиДаних, ПозиціяТекстуВКолонці);
            ФункціїДляЗвітів.ЗаповнитиМодельДаними(listStore, columnsName, listRow);

            CreateReportNotebookPage("Залишки та обороти", treeView);
        }

        void OnReport_Документи(object? sender, EventArgs args)
        {
            #region SELECT

            bool isExistParent = false;

            string query = $@"
WITH register AS
(
     SELECT 
        ПартіїТоварів.period AS period,
        ПартіїТоварів.owner AS owner,
        ПартіїТоварів.income AS income,
        ПартіїТоварів.{ПартіїТоварів_Const.Організація} AS Організація,
        ПартіїТоварів.{ПартіїТоварів_Const.ПартіяТоварівКомпозит} AS ПартіяТоварівКомпозит,
        ПартіїТоварів.{ПартіїТоварів_Const.Номенклатура} AS Номенклатура,
        ПартіїТоварів.{ПартіїТоварів_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ПартіїТоварів.{ПартіїТоварів_Const.Серія} AS Серія,
        ПартіїТоварів.{ПартіїТоварів_Const.Склад} AS Склад,
        ПартіїТоварів.{ПартіїТоварів_Const.Кількість} AS Кількість,
        CASE WHEN ПартіїТоварів.{ПартіїТоварів_Const.СписанаСобівартість} != 0 THEN 
            ПартіїТоварів.{ПартіїТоварів_Const.СписанаСобівартість} ELSE 
            ПартіїТоварів.{ПартіїТоварів_Const.Собівартість} END AS Собівартість 
    FROM
        {ПартіїТоварів_Const.TABLE} AS ПартіїТоварів
    WHERE
        (ПартіїТоварів.period >= @ПочатокПеріоду AND ПартіїТоварів.period <= @КінецьПеріоду)
";

            #region WHERE

            isExistParent = true;

            //Відбір по вибраному елементу Організація
            if (!Організація.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ПартіїТоварів.{ПартіїТоварів_Const.Організація} = '{Організація.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу Номенклатура
            if (!Номенклатура.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ПартіїТоварів.{ПартіїТоварів_Const.Номенклатура} = '{Номенклатура.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу Характеристики Номенклатури
            if (!ХарактеристикиНоменклатури.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ПартіїТоварів.{ПартіїТоварів_Const.ХарактеристикаНоменклатури}= '{ХарактеристикиНоменклатури.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу Склади
            if (!Склад.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ПартіїТоварів.{ПартіїТоварів_Const.Склад} = '{Склад.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу Серії Номенклатури
            if (!Серія.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ПартіїТоварів.{ПартіїТоварів_Const.Серія} = '{Серія.Pointer.UnigueID}'
";
            }

            #endregion

            query += $@"
),
documents AS
(";
            int counter = 0;
            foreach (string table in ПартіїТоварів_Const.AllowDocumentSpendTable)
            {
                string docType = ПартіїТоварів_Const.AllowDocumentSpendType[counter];

                string union = (counter > 0 ? "UNION" : "");
                query += $@"
{union}
SELECT 
    '{docType}' AS doctype,
    {table}.uid, 
    {table}.docname, 
    register.period, 
    register.income, 
    register.Кількість,
    register.Собівартість,
    register.Організація,
    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва,
    register.ПартіяТоварівКомпозит,
    Довідник_ПартіяТоварівКомпозит.{ПартіяТоварівКомпозит_Const.Назва} AS ПартіяТоварівКомпозит_Назва,
    register.Номенклатура,
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    register.ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    register.Серія,
    Довідник_СеріїНоменклатури.{СеріїНоменклатури_Const.Номер} AS Серія_Номер,
    register.Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва
FROM register INNER JOIN {table} ON {table}.uid = register.owner
    LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = register.Організація
    LEFT JOIN {ПартіяТоварівКомпозит_Const.TABLE} AS Довідник_ПартіяТоварівКомпозит ON Довідник_ПартіяТоварівКомпозит.uid = register.ПартіяТоварівКомпозит
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = register.Номенклатура
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = register.ХарактеристикаНоменклатури
    LEFT JOIN {СеріїНоменклатури_Const.TABLE} AS Довідник_СеріїНоменклатури ON Довідник_СеріїНоменклатури.uid = register.Серія
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
    Організація,
    Організація_Назва,
    ПартіяТоварівКомпозит,
    ПартіяТоварівКомпозит_Назва,
    Номенклатура,
    Номенклатура_Назва,
    ХарактеристикаНоменклатури,
    ХарактеристикаНоменклатури_Назва,
    Серія,
    Серія_Номер,
    Склад,
    Склад_Назва,
    ROUND(Кількість, 2) AS Кількість, 
    ROUND(Собівартість, 2) AS Собівартість
FROM documents
ORDER BY period ASC, Організація_Назва, 
         Номенклатура_Назва, 
         ХарактеристикаНоменклатури_Назва
";

            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>();
            ВидиміКолонки.Add("income", "Рух");
            ВидиміКолонки.Add("docname", "Документ");
            ВидиміКолонки.Add("Організація_Назва", "Організація");
            ВидиміКолонки.Add("ПартіяТоварівКомпозит_Назва", "Партія");
            ВидиміКолонки.Add("Номенклатура_Назва", "Номенклатура");
            ВидиміКолонки.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");
            ВидиміКолонки.Add("Склад_Назва", "Склад");
            ВидиміКолонки.Add("Серія_Номер", "Серія");
            ВидиміКолонки.Add("Кількість", "Кількість");
            ВидиміКолонки.Add("Собівартість", "Собівартість");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>();
            КолонкиДаних.Add("Організація_Назва", "Організація");
            КолонкиДаних.Add("ПартіяТоварівКомпозит_Назва", "ПартіяТоварівКомпозит");
            КолонкиДаних.Add("Номенклатура_Назва", "Номенклатура");
            КолонкиДаних.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");
            КолонкиДаних.Add("Серія_Номер", "Серія");
            КолонкиДаних.Add("Склад_Назва", "Склад");

            Dictionary<string, float> ПозиціяТекстуВКолонці = new Dictionary<string, float>();
            ПозиціяТекстуВКолонці.Add("Кількість", 1);
            ПозиціяТекстуВКолонці.Add("Собівартість", 1);

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("ПочатокПеріоду", DateTime.Parse($"{ДатаПочатокПеріоду.Value.Day}.{ДатаПочатокПеріоду.Value.Month}.{ДатаПочатокПеріоду.Value.Year} 00:00:00"));
            paramQuery.Add("КінецьПеріоду", DateTime.Parse($"{ДатаКінецьПеріоду.Value.Day}.{ДатаКінецьПеріоду.Value.Month}.{ДатаКінецьПеріоду.Value.Year} 23:59:59"));

            string[] columnsName;
            List<Dictionary<string, object>> listRow;

            Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

            ListStore listStore;
            ФункціїДляЗвітів.СтворитиМодельДаних(out listStore, columnsName);

            TreeView treeView = new TreeView(listStore);
            treeView.ButtonPressEvent += ФункціїДляЗвітів.OpenPageDirectoryOrDocument;

            ФункціїДляЗвітів.СтворитиКолонкиДляДерева(treeView, columnsName, ВидиміКолонки, КолонкиДаних, ПозиціяТекстуВКолонці);
            ФункціїДляЗвітів.ЗаповнитиМодельДаними(listStore, columnsName, listRow);

            CreateReportNotebookPage("Документи", treeView);
        }

    }
}