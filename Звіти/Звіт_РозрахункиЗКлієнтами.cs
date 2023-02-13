/*
Copyright (C) 2019-2023 TARAKHOMYN YURIY IVANOVYCH
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

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade
{
    class Звіт_РозрахункиЗКлієнтами : VBox
    {
        Notebook reportNotebook;

        #region Filters

        DateTimeControl ДатаПочатокПеріоду = new DateTimeControl() { OnlyDate = true, Value = DateTime.Parse($"01.{DateTime.Now.Month}.{DateTime.Now.Year}") };
        DateTimeControl ДатаКінецьПеріоду = new DateTimeControl() { OnlyDate = true, Value = DateTime.Now };

        Контрагенти_PointerControl Контрагент = new Контрагенти_PointerControl();
        Контрагенти_Папки_PointerControl Контрагент_Папка = new Контрагенти_Папки_PointerControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();

        #endregion

        public Звіт_РозрахункиЗКлієнтами() : base()
        {
            //Кнопки
            HBox hBoxTop = new HBox();
            PackStart(hBoxTop, false, false, 10);

            //2
            Button bOstatok = new Button("Залишки");
            bOstatok.Clicked += OnReport_Залишки;

            hBoxTop.PackStart(bOstatok, false, false, 10);

            //3
            Button bOborot = new Button("Залишки та обороти");
            bOborot.Clicked += OnReport_ЗалишкиТаОбороти;

            hBoxTop.PackStart(bOborot, false, false, 10);

            //4
            Button bDocuments = new Button("Документи");
            bDocuments.Clicked += OnReport_Документи;

            hBoxTop.PackStart(bDocuments, false, false, 10);

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

        void OnReport_Залишки(object? sender, EventArgs args)
        {
            #region SELECT

            bool isExistParent = false;

            string query = $@"
SELECT 
    РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Залишки_TablePart.Контрагент} AS Контрагент,
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS Контрагент_Назва,
    РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Залишки_TablePart.Валюта} AS Валюта, 
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    ROUND(SUM(РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Залишки_TablePart.Сума}), 2) AS Сума
FROM 
    {РозрахункиЗКлієнтами_Залишки_TablePart.TABLE} AS РозрахункиЗКлієнтами
    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = 
        РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Залишки_TablePart.Валюта}
    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Залишки_TablePart.Контрагент}
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

            //Відбір по вибраному елементу Контрагенту
            if (!Контрагент.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Контрагенти.uid = '{Контрагент.Pointer.UnigueID}'
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

            query += $@"
GROUP BY Контрагент, Контрагент_Назва, 
         Валюта, Валюта_Назва
HAVING
     SUM(РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Залишки_TablePart.Сума}) != 0
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

            Dictionary<string, float> ПозиціяТекстуВКолонці = new Dictionary<string, float>();
            ПозиціяТекстуВКолонці.Add("Сума", 1);

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

            ФункціїДляЗвітів.CreateReportNotebookPage(reportNotebook, "Залишки", treeView);
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
        РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Залишки_TablePart.Контрагент} AS Контрагент,
        РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Залишки_TablePart.Валюта} AS Валюта,
        SUM(РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Залишки_TablePart.Сума}) AS Сума
    FROM 
        {РозрахункиЗКлієнтами_Залишки_TablePart.TABLE} AS РозрахункиЗКлієнтами
    WHERE
        РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Залишки_TablePart.Період} < @ПочатокПеріоду
    GROUP BY Контрагент, Валюта
),
ЗалишкиТаОборотиЗаПеріод AS
(
    SELECT 
        РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.Контрагент} AS Контрагент,
        РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.Валюта} AS Валюта,
        SUM(РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.СумаПрихід}) AS СумаПрихід,
        SUM(РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.СумаРозхід}) AS СумаРозхід,
        SUM(РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.СумаЗалишок}) AS СумаЗалишок
    FROM 
        {РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.TABLE} AS РозрахункиЗКлієнтами
    WHERE
        РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.Період} >= @ПочатокПеріоду AND
        РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.Період} <= @КінецьПеріоду
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

            Dictionary<string, float> ПозиціяТекстуВКолонці = new Dictionary<string, float>();
            ПозиціяТекстуВКолонці.Add("ПочатковийЗалишок", 1);
            ПозиціяТекстуВКолонці.Add("Прихід", 1);
            ПозиціяТекстуВКолонці.Add("Розхід", 1);
            ПозиціяТекстуВКолонці.Add("КінцевийЗалишок", 1);

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

            ФункціїДляЗвітів.CreateReportNotebookPage(reportNotebook, "Залишки та обороти", treeView);
        }

        void OnReport_Документи(object? sender, EventArgs args)
        {
            #region SELECT

            bool isExistParent = false;

            string query = $@"
WITH register AS
(
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
        (РозрахункиЗКлієнтами.period >= @ПочатокПеріоду AND РозрахункиЗКлієнтами.period <= @КінецьПеріоду)
";

            #region WHERE

            isExistParent = true;

            //Відбір по вибраному елементу Контрагенти
            if (!Контрагент.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                query += $@"
РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Контрагент} = '{Контрагент.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу Валюти
            if (!Валюта.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Валюта} = '{Валюта.Pointer.UnigueID}'
";
            }

            #endregion

            query += $@"
),
documents AS
(";
            int counter = 0;
            foreach (string table in РозрахункиЗКлієнтами_Const.AllowDocumentSpendTable)
            {
                string docType = РозрахункиЗКлієнтами_Const.AllowDocumentSpendType[counter];

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
    CONCAT(uid, ':', doctype) AS uid_and_text,
    uid,
    period,
    (CASE WHEN income = true THEN '+' ELSE '-' END) AS income, 
    docname AS Документ, 
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
            ВидиміКолонки.Add("Документ", "Документ");
            ВидиміКолонки.Add("Контрагент_Назва", "Контрагент");
            ВидиміКолонки.Add("Валюта_Назва", "Валюта");
            ВидиміКолонки.Add("Сума", "Сума");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>();
            КолонкиДаних.Add("Документ", "uid_and_text");
            КолонкиДаних.Add("Контрагент_Назва", "Контрагент");
            КолонкиДаних.Add("Валюта_Назва", "Валюта");

            Dictionary<string, float> ПозиціяТекстуВКолонці = new Dictionary<string, float>();
            ПозиціяТекстуВКолонці.Add("income", 0.5f);
            ПозиціяТекстуВКолонці.Add("Сума", 1);

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("ПочатокПеріоду", ДатаПочатокПеріоду.ПочатокДня());
            paramQuery.Add("КінецьПеріоду", ДатаКінецьПеріоду.КінецьДня());

            string[] columnsName;
            List<Dictionary<string, object>> listRow;

            Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

            ListStore listStore;
            ФункціїДляЗвітів.СтворитиМодельДаних(out listStore, columnsName);

            TreeView treeView = new TreeView(listStore);
            treeView.ButtonPressEvent += ФункціїДляЗвітів.OpenPageDirectoryOrDocument;

            ФункціїДляЗвітів.СтворитиКолонкиДляДерева(treeView, columnsName, ВидиміКолонки, КолонкиДаних, ПозиціяТекстуВКолонці);
            ФункціїДляЗвітів.ЗаповнитиМодельДаними(listStore, columnsName, listRow);

            ФункціїДляЗвітів.CreateReportNotebookPage(reportNotebook, "Документи", treeView);
        }

    }
}