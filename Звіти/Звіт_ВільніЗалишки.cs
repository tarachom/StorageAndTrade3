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
using Константи = StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade
{
    class Звіт_ВільніЗалишки : VBox
    {
        Notebook reportNotebook;

        #region Filters

        DateTimeControl ДатаПочатокПеріоду = new DateTimeControl() { OnlyDate = true, Value = DateTime.Parse($"01.{DateTime.Now.Month}.{DateTime.Now.Year}") };
        DateTimeControl ДатаКінецьПеріоду = new DateTimeControl() { OnlyDate = true, Value = DateTime.Now };

        Номенклатура_PointerControl Номенклатура = new Номенклатура_PointerControl();
        Номенклатура_Папки_PointerControl Номенклатура_Папка = new Номенклатура_Папки_PointerControl() { Caption = "Номенклатура папка:" };
        ХарактеристикиНоменклатури_PointerControl ХарактеристикиНоменклатури = new ХарактеристикиНоменклатури_PointerControl();
        Склади_PointerControl Склад = new Склади_PointerControl();
        Склади_Папки_PointerControl Склад_Папка = new Склади_Папки_PointerControl() { Caption = "Склад папка:" };

        #endregion

        public Звіт_ВільніЗалишки() : base()
        {
            //Кнопки
            HBox hBoxTop = new HBox();
            PackStart(hBoxTop, false, false, 10);

            //2
            Button bOstatok = new Button("Залишки");
            bOstatok.Clicked += OnReport_Залишки;

            hBoxTop.PackStart(bOstatok, false, false, 10);

            //3
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

            //Номенклатура
            HBox hBoxNomenklatura = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxNomenklatura, false, false, 5);

            hBoxNomenklatura.PackStart(Номенклатура, false, false, 5);

            //ХарактеристикиНоменклатури
            HBox hBoxHarakterystyka = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxHarakterystyka, false, false, 5);

            ХарактеристикиНоменклатури.BeforeClickOpenFunc = () =>
            {
                ХарактеристикиНоменклатури.НоменклатураВласник = Номенклатура.Pointer;
            };

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

        void OnReport_Залишки(object? sender, EventArgs args)
        {
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
     SUM(ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.ВНаявності}) != 0
OR
     SUM(ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.ВРезервіЗіСкладу}) != 0
OR
     SUM(ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.ВРезервіПідЗамовлення}) != 0   
ORDER BY Номенклатура_Назва
";

            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>();
            ВидиміКолонки.Add("Номенклатура_Назва", "Номенклатура");
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const) ВидиміКолонки.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");
            ВидиміКолонки.Add("Склад_Назва", "Склад");
            ВидиміКолонки.Add("ВНаявності", "В наявності");
            ВидиміКолонки.Add("ВРезервіЗіСкладу", "В резерві зі складу");
            ВидиміКолонки.Add("ВРезервіПідЗамовлення", "В резерві під замовлення");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>();
            КолонкиДаних.Add("Номенклатура_Назва", "Номенклатура");
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const) КолонкиДаних.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");
            КолонкиДаних.Add("Склад_Назва", "Склад");

            Dictionary<string, float> ПозиціяТекстуВКолонці = new Dictionary<string, float>();
            ПозиціяТекстуВКолонці.Add("ВНаявності", 1);
            ПозиціяТекстуВКолонці.Add("ВРезервіЗіСкладу", 1);
            ПозиціяТекстуВКолонці.Add("ВРезервіПідЗамовлення", 1);

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

        void OnReport_Документи(object? sender, EventArgs args)
        {
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
            if (!Номенклатура.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ВільніЗалишки.{ВільніЗалишки_Const.Номенклатура} = '{Номенклатура.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу Характеристики Номенклатури
            if (!ХарактеристикиНоменклатури.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ВільніЗалишки.{ВільніЗалишки_Const.ХарактеристикаНоменклатури} = '{ХарактеристикиНоменклатури.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу Склади
            if (!Склад.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ВільніЗалишки.{ВільніЗалишки_Const.Склад} = '{Склад.Pointer.UnigueID}'
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
    register.ВРезервіЗіСкладу,
    register.ВРезервіПідЗамовлення,
    register.Номенклатура,
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    register.ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    register.Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва
FROM register INNER JOIN {table} ON {table}.uid = register.owner
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
    CONCAT(uid, ':', doctype) AS uid_and_text,
    uid,
    period,
    (CASE WHEN income = true THEN '+' ELSE '-' END) AS income,
    docname AS Документ, 
    Номенклатура,
    Номенклатура_Назва,
    ХарактеристикаНоменклатури,
    ХарактеристикаНоменклатури_Назва,
    Склад,
    Склад_Назва,
    ВНаявності,
    ВРезервіЗіСкладу, 
    ВРезервіПідЗамовлення
FROM documents
ORDER BY period ASC
";

            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>();
            ВидиміКолонки.Add("income", "Рух");
            ВидиміКолонки.Add("Документ", "Документ");
            ВидиміКолонки.Add("Номенклатура_Назва", "Номенклатура");
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const) ВидиміКолонки.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");
            ВидиміКолонки.Add("Склад_Назва", "Склад");
            ВидиміКолонки.Add("ВНаявності", "В наявності");
            ВидиміКолонки.Add("ВРезервіЗіСкладу", "В резерві зі складу");
            ВидиміКолонки.Add("ВРезервіПідЗамовлення", "В резерві під замовлення");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>();
            КолонкиДаних.Add("Документ", "uid_and_text");
            КолонкиДаних.Add("Номенклатура_Назва", "Номенклатура");
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const) КолонкиДаних.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");
            КолонкиДаних.Add("Склад_Назва", "Склад");

            Dictionary<string, float> ПозиціяТекстуВКолонці = new Dictionary<string, float>();
            ПозиціяТекстуВКолонці.Add("income", 0.5f);
            ПозиціяТекстуВКолонці.Add("ВНаявності", 1);
            ПозиціяТекстуВКолонці.Add("ВРезервіЗіСкладу", 1);
            ПозиціяТекстуВКолонці.Add("ВРезервіПідЗамовлення", 1);

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

            ФункціїДляЗвітів.CreateReportNotebookPage(reportNotebook, "Документи", treeView);
        }

    }
}