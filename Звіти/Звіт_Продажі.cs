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
    class Звіт_Продажі : VBox
    {
        Notebook reportNotebook;

        #region Filters

        DateTimeControl ДатаПочатокПеріоду = new DateTimeControl() { OnlyDate = true, Value = DateTime.Parse($"01.{DateTime.Now.Month}.{DateTime.Now.Year}") };
        DateTimeControl ДатаКінецьПеріоду = new DateTimeControl() { OnlyDate = true, Value = DateTime.Now };
        CheckButton ГрупуватиПоПеріоду = new CheckButton("Групувати по періоду (День)");
        CheckButton СобівартістьЗакупки = new CheckButton("Собівартість закупки");

        Організації_PointerControl Організація = new Організації_PointerControl();
        Контрагенти_PointerControl Контрагент = new Контрагенти_PointerControl();
        Контрагенти_Папки_PointerControl Контрагент_Папка = new Контрагенти_Папки_PointerControl() { Caption = "Контрагент папка:" };
        ДоговориКонтрагентів_PointerControl ДоговірКонтрагента = new ДоговориКонтрагентів_PointerControl() { Caption = "Договір:" };
        Номенклатура_PointerControl Номенклатура = new Номенклатура_PointerControl();
        Номенклатура_Папки_PointerControl Номенклатура_Папка = new Номенклатура_Папки_PointerControl() { Caption = "Номенклатура папка:" };
        ХарактеристикиНоменклатури_PointerControl ХарактеристикиНоменклатури = new ХарактеристикиНоменклатури_PointerControl();
        Склади_PointerControl Склад = new Склади_PointerControl();
        Склади_Папки_PointerControl Склад_Папка = new Склади_Папки_PointerControl() { Caption = "Склад папка:" };

        #endregion

        public Звіт_Продажі() : base()
        {
            //Кнопки
            HBox hBoxBotton = new HBox();

            //1
            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

            hBoxBotton.PackStart(bClose, false, false, 10);

            //2
            Button bOborot = new Button("Обороти");
            bOborot.Clicked += OnReport_Обороти;

            hBoxBotton.PackStart(bOborot, false, false, 10);

            //4 !!!
            //Button bDocuments = new Button("Документи");
            //bDocuments.Clicked += OnReport_Документи;

            //hBoxBotton.PackStart(bDocuments, false, false, 10);

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
            HBox hBoxOrganisation = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxOrganisation, false, false, 5);

            hBoxOrganisation.PackStart(Організація, false, false, 5);

            //Склад
            HBox hBoxSkald = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSkald, false, false, 5);

            hBoxSkald.PackStart(Склад, false, false, 5);

            //Контрагент
            HBox hBoxKontragent = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxKontragent, false, false, 5);

            hBoxKontragent.PackStart(Контрагент, false, false, 5);

            //ДоговірКонтрагента
            HBox hBoxDogovir = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxDogovir, false, false, 5);

            ДоговірКонтрагента.BeforeClickOpenFunc = () =>
            {
                ДоговірКонтрагента.КонтрагентВласник = Контрагент.Pointer;
            };

            hBoxDogovir.PackStart(ДоговірКонтрагента, false, false, 5);

            //Номенклатура
            HBox hBoxNomenklatura = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxNomenklatura, false, false, 5);

            hBoxNomenklatura.PackStart(Номенклатура, false, false, 5);

            //ХарактеристикиНоменклатури
            HBox hBoxHarakterystyka = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxHarakterystyka, false, false, 5);

            hBoxHarakterystyka.PackStart(ХарактеристикиНоменклатури, false, false, 5);
        }

        void CreateContainer2(VBox vBox)
        {
            //ГрупуватиПоПеріоду
            HBox hBoxGroupPeriod = new HBox() { Halign = Align.Start };
            vBox.PackStart(hBoxGroupPeriod, false, false, 5);

            hBoxGroupPeriod.PackStart(ГрупуватиПоПеріоду, false, false, 5);

            //СобівартістьЗакупки
            HBox hBoxSobivartyst = new HBox() { Halign = Align.Start };
            vBox.PackStart(hBoxSobivartyst, false, false, 5);

            hBoxSobivartyst.PackStart(СобівартістьЗакупки, false, false, 5);

            //Склад папка
            HBox hBoxSkaldPapka = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSkaldPapka, false, false, 5);

            hBoxSkaldPapka.PackStart(Склад_Папка, false, false, 5);

            //Контрагент Папка
            HBox hBoxKontragentPapka = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxKontragentPapka, false, false, 5);

            hBoxKontragentPapka.PackStart(Контрагент_Папка, false, false, 5);

            //Номенклатура папка
            HBox hBoxNomenklaturaPapka = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxNomenklaturaPapka, false, false, 5);

            hBoxNomenklaturaPapka.PackStart(Номенклатура_Папка, false, false, 5);
        }

        #endregion

        void OnReport_Обороти(object? sender, EventArgs args)
        {
            #region SELECT

            bool isExistParent = true;

            string query = $@"
SELECT 
" + (ГрупуватиПоПеріоду.Active ? $@"
    Продажі.{Продажі_Обороти_TablePart.Період} AS Період,
    TO_CHAR(Продажі.{Продажі_Обороти_TablePart.Період}, 'dd.mm.yyyy') AS Період_Назва," : "") + $@" 
    Продажі.{Продажі_Обороти_TablePart.Організація} AS Організація,
    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва,
    Продажі.{Продажі_Обороти_TablePart.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Продажі.{Продажі_Обороти_TablePart.Контрагент} AS Контрагент,
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS Контрагент_Назва,
    Продажі.{Продажі_Обороти_TablePart.Договір} AS Договір,
    CONCAT(Довідник_ДоговориКонтрагентів.{ДоговориКонтрагентів_Const.Назва}, ' ', 
           Довідник_ДоговориКонтрагентів.{ДоговориКонтрагентів_Const.ТипДоговоруПредставлення}) AS Договір_Назва,
    Продажі.{Продажі_Обороти_TablePart.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    Продажі.{Продажі_Обороти_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    ROUND(SUM(Продажі.{Продажі_Обороти_TablePart.Кількість}), 2) AS Кількість,
    ROUND(SUM(Продажі.{Продажі_Обороти_TablePart.Сума}), 2) AS Сума,
    ROUND(SUM(Продажі.{Продажі_Обороти_TablePart.Дохід}), 2) AS Дохід" +
    (СобівартістьЗакупки.Active ? $", Продажі.{Продажі_Обороти_TablePart.Собівартість} AS Собівартість" : "") + $@"
FROM 
    {Продажі_Обороти_TablePart.TABLE} AS Продажі

    LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = 
        Продажі.{Продажі_Обороти_TablePart.Організація}
    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
        Продажі.{Продажі_Обороти_TablePart.Склад}
    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Продажі.{Продажі_Обороти_TablePart.Контрагент}
    LEFT JOIN {ДоговориКонтрагентів_Const.TABLE} AS Довідник_ДоговориКонтрагентів ON Довідник_ДоговориКонтрагентів.uid = 
        Продажі.{Продажі_Обороти_TablePart.Договір}
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        Продажі.{Продажі_Обороти_TablePart.Номенклатура}
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        Продажі.{Продажі_Обороти_TablePart.ХарактеристикаНоменклатури}
WHERE
    Продажі.{Продажі_Обороти_TablePart.Період} >= @ПочатокПеріоду AND
    Продажі.{Продажі_Обороти_TablePart.Період} <= @КінецьПеріоду
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

            //Відбір по всіх вкладених папках вибраної папки Контрагент
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

            //Відбір по вибраному елементу Контрагент
            if (!Контрагент.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Контрагенти.uid = '{Контрагент.Pointer.UnigueID}'
";
            }

            //Відбір по вибраному елементу ДоговірКонтрагента
            if (!ДоговірКонтрагента.Pointer.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_ДоговориКонтрагентів.uid = '{ДоговірКонтрагента.Pointer.UnigueID}'
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

            #endregion

            query += @"
GROUP BY " +
    (ГрупуватиПоПеріоду.Active ? "Період, Період_Назва, " : "") + @"
    Організація, Організація_Назва,
    Склад, Склад_Назва,
    Контрагент, Контрагент_Назва,
    Договір, Договір_Назва,
    Номенклатура, Номенклатура_Назва,
    ХарактеристикаНоменклатури, ХарактеристикаНоменклатури_Назва" +
    (СобівартістьЗакупки.Active ? ", Собівартість" : "") + @$"

HAVING
    SUM(Продажі.{Продажі_Обороти_TablePart.Кількість}) != 0 OR
    SUM(Продажі.{Продажі_Обороти_TablePart.Сума}) != 0 OR
    SUM(Продажі.{Продажі_Обороти_TablePart.Дохід}) != 0 

ORDER BY " +
    (ГрупуватиПоПеріоду.Active ? "Період," : "") + @"
    Організація_Назва, Склад_Назва, Контрагент_Назва, Договір_Назва, Номенклатура_Назва, ХарактеристикаНоменклатури_Назва
";
            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>();
            ВидиміКолонки.Add("Період_Назва", "Період");
            ВидиміКолонки.Add("Організація_Назва", "Організація");
            ВидиміКолонки.Add("Склад_Назва", "Склад");
            ВидиміКолонки.Add("Контрагент_Назва", "Контрагент");
            ВидиміКолонки.Add("Договір_Назва", "Договір");
            ВидиміКолонки.Add("Номенклатура_Назва", "Номенклатура");
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const) ВидиміКолонки.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");
            ВидиміКолонки.Add("Кількість", "Кількість");
            ВидиміКолонки.Add("Сума", "Сума");
            ВидиміКолонки.Add("Дохід", "Дохід");
            ВидиміКолонки.Add("Собівартість", "Собівартість");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>();
            КолонкиДаних.Add("Організація_Назва", "Організація");
            КолонкиДаних.Add("Склад_Назва", "Склад");
            КолонкиДаних.Add("Контрагент_Назва", "Контрагент");
            КолонкиДаних.Add("Договір_Назва", "Договір");
            КолонкиДаних.Add("Номенклатура_Назва", "Номенклатура");
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const) КолонкиДаних.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");

            Dictionary<string, float> ПозиціяТекстуВКолонці = new Dictionary<string, float>();
            ПозиціяТекстуВКолонці.Add("Кількість", 1);
            ПозиціяТекстуВКолонці.Add("Сума", 1);
            ПозиціяТекстуВКолонці.Add("Дохід", 1);
            ПозиціяТекстуВКолонці.Add("Собівартість", 1);

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

            ФункціїДляЗвітів.CreateReportNotebookPage(reportNotebook, "Обороти", treeView);
        }
    }
}