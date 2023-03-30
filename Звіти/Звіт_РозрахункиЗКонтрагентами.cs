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
    class Звіт_РозрахункиЗКонтрагентами : VBox
    {
        Notebook reportNotebook;

        #region Filters

        DateTimeControl ДатаПочатокПеріоду = new DateTimeControl() { OnlyDate = true, Value = DateTime.Parse($"01.{DateTime.Now.Month}.{DateTime.Now.Year}") };
        DateTimeControl ДатаКінецьПеріоду = new DateTimeControl() { OnlyDate = true, Value = DateTime.Now };

        Контрагенти_PointerControl Контрагент = new Контрагенти_PointerControl();
        Контрагенти_Папки_PointerControl Контрагент_Папка = new Контрагенти_Папки_PointerControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();

        struct ПараметриФільтр
        {
            public DateTime ДатаПочатокПеріоду;
            public DateTime ДатаКінецьПеріоду;
            public Контрагенти_Pointer Контрагент;
            public Контрагенти_Папки_Pointer Контрагент_Папка;
            public Валюти_Pointer Валюта;
        }

        #endregion

        public Звіт_РозрахункиЗКонтрагентами() : base()
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

        ПараметриФільтр СформуватиФільтр()
        {
            return new ПараметриФільтр()
            {
                ДатаПочатокПеріоду = ДатаПочатокПеріоду.ПочатокДня(),
                ДатаКінецьПеріоду = ДатаКінецьПеріоду.КінецьДня(),
                Контрагент = Контрагент.Pointer,
                Контрагент_Папка = Контрагент_Папка.Pointer,
                Валюта = Валюта.Pointer
            };
        }

        HBox ВідобразитиФільтр(string typeReport, ПараметриФільтр Фільтр)
        {
            HBox hBoxCaption = new HBox();

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
                        text += "З <b>" +
                            Фільтр.ДатаПочатокПеріоду.ToString("dd.MM.yyyy") + "</b> по <b>" +
                            Фільтр.ДатаКінецьПеріоду.ToString("dd.MM.yyyy") + "</b>; ";
                        break;
                    }
            }

            if (!Фільтр.Контрагент.IsEmpty())
                text += "Контрагент: <b>" + Фільтр.Контрагент.GetPresentation() + "</b>; ";

            if (!Фільтр.Контрагент_Папка.IsEmpty())
                text += "Контрагент папка: <b>" + Фільтр.Контрагент_Папка.GetPresentation() + "</b>; ";

            if (!Фільтр.Валюта.IsEmpty())
                text += "Валюта: <b>" + Фільтр.Валюта.GetPresentation() + "</b>; ";

            hBoxCaption.PackStart(new Label(text) { Wrap = true, UseMarkup = true }, false, false, 2);

            return hBoxCaption;
        }

        void OnReport_Залишки(object? sender, EventArgs args)
        {
            Залишки(СформуватиФільтр());
        }

        void OnReport_ЗалишкиТаОбороти(object? sender, EventArgs args)
        {
            ЗалишкиТаОбороти(СформуватиФільтр());
        }

        void OnReport_Документи(object? sender, EventArgs args)
        {
            Документи(СформуватиФільтр());
        }

        void Залишки(object? Параметри, bool refreshPage = false)
        {
            ПараметриФільтр Фільтр = Параметри != null ? (ПараметриФільтр)Параметри : new ПараметриФільтр();

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
            РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Залишки_TablePart.Контрагент} AS Контрагент,
            РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Залишки_TablePart.Валюта} AS Валюта, 
            РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Залишки_TablePart.Сума} AS Сума
        FROM 
            {РозрахункиЗКлієнтами_Залишки_TablePart.TABLE} AS РозрахункиЗКлієнтами
        UNION ALL
        SELECT
            РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Залишки_TablePart.Контрагент} AS Контрагент,
            РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Залишки_TablePart.Валюта} AS Валюта, 
            РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Залишки_TablePart.Сума} AS Сума
        FROM 
            {РозрахункиЗПостачальниками_Залишки_TablePart.TABLE} AS РозрахункиЗПостачальниками
    ) AS КлієнтиТаПостачальники
";

            #region WHERE

            //Відбір по вибраному елементу Контрагенту
            if (!Фільтр.Контрагент.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
КлієнтиТаПостачальники.Контрагент = '{Фільтр.Контрагент.UnigueID}'
";
            }

            //Відбір по вибраному елементу Валюти
            if (!Фільтр.Валюта.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
КлієнтиТаПостачальники.Валюта = '{Фільтр.Валюта.UnigueID}'
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
            if (!Фільтр.Контрагент_Папка.IsEmpty())
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
            WHERE {Контрагенти_Папки_Const.TABLE}.uid = '{Фільтр.Контрагент_Папка.UnigueID}' 
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

            ФункціїДляЗвітів.CreateReportNotebookPage(reportNotebook, "Залишки", ВідобразитиФільтр("Залишки", Фільтр), treeView, Залишки, Фільтр, refreshPage);
        }

        void ЗалишкиТаОбороти(object? Параметри, bool refreshPage = false)
        {
            ПараметриФільтр Фільтр = Параметри != null ? (ПараметриФільтр)Параметри : new ПараметриФільтр();

            #region SELECT

            bool isExistParent = false;

            string query = $@"
WITH ПочатковийЗалишок AS
(
    SELECT
        Контрагент,
        Валюта,
        SUM(Сума) AS Сума
    FROM
    (
        SELECT
            РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Залишки_TablePart.Контрагент} AS Контрагент,
            РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Залишки_TablePart.Валюта} AS Валюта,
            РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Залишки_TablePart.Сума} AS Сума
        FROM 
            {РозрахункиЗПостачальниками_Залишки_TablePart.TABLE} AS РозрахункиЗПостачальниками
        WHERE
            РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Залишки_TablePart.Період} < @ПочатокПеріоду

        UNION ALL

        SELECT
            РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Залишки_TablePart.Контрагент} AS Контрагент,
            РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Залишки_TablePart.Валюта} AS Валюта,
            РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Залишки_TablePart.Сума} AS Сума
        FROM 
            {РозрахункиЗКлієнтами_Залишки_TablePart.TABLE} AS РозрахункиЗКлієнтами
        WHERE
            РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Залишки_TablePart.Період} < @ПочатокПеріоду
    ) AS КлієнтиТаПостачальники
    GROUP BY Контрагент, Валюта
), 
ЗалишкиТаОборотиЗаПеріод AS
(
    SELECT
        Контрагент,
        Валюта,
        SUM(СумаПрихід) AS СумаПрихід,
        SUM(СумаРозхід) AS СумаРозхід,
        SUM(СумаЗалишок) AS СумаЗалишок
    FROM
    (
        SELECT
            РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_ЗалишкиТаОбороти_TablePart.Контрагент} AS Контрагент,
            РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_ЗалишкиТаОбороти_TablePart.Валюта} AS Валюта,
            РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_ЗалишкиТаОбороти_TablePart.СумаПрихід} AS СумаПрихід,
            РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_ЗалишкиТаОбороти_TablePart.СумаРозхід} AS СумаРозхід,
            РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_ЗалишкиТаОбороти_TablePart.СумаЗалишок} AS СумаЗалишок
        FROM 
            {РозрахункиЗПостачальниками_ЗалишкиТаОбороти_TablePart.TABLE} AS РозрахункиЗПостачальниками
        WHERE
            РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_ЗалишкиТаОбороти_TablePart.Період} >= @ПочатокПеріоду AND
            РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_ЗалишкиТаОбороти_TablePart.Період} <= @КінецьПеріоду

        UNION ALL

        SELECT
            РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.Контрагент} AS Контрагент,
            РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.Валюта} AS Валюта,
            РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.СумаПрихід} AS СумаПрихід,
            РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.СумаРозхід} AS СумаРозхід,
            РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.СумаЗалишок} AS СумаЗалишок
        FROM 
            {РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.TABLE} AS РозрахункиЗКлієнтами
        WHERE
            РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.Період} >= @ПочатокПеріоду AND
            РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.Період} <= @КінецьПеріоду
    ) AS КлієнтиТаПостачальники
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
            if (!Фільтр.Контрагент_Папка.IsEmpty())
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
            WHERE {Контрагенти_Папки_Const.TABLE}.uid = '{Фільтр.Контрагент_Папка.UnigueID}' 
            UNION ALL
            SELECT {Контрагенти_Папки_Const.TABLE}.uid
            FROM {Контрагенти_Папки_Const.TABLE}
                JOIN r ON {Контрагенти_Папки_Const.TABLE}.{Контрагенти_Папки_Const.Родич} = r.uid
        ) SELECT uid FROM r
    )
";
            }

            //Відбір по вибраному елементу Контрагенти
            if (!Фільтр.Контрагент.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Контрагенти.uid = '{Фільтр.Контрагент.UnigueID}'
";
            }

            //Відбір по вибраному елементу
            if (!Фільтр.Валюта.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Валюти.uid = '{Фільтр.Валюта.UnigueID}'
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
            paramQuery.Add("ПочатокПеріоду", Фільтр.ДатаПочатокПеріоду);
            paramQuery.Add("КінецьПеріоду", Фільтр.ДатаКінецьПеріоду);

            string[] columnsName;
            List<Dictionary<string, object>> listRow;

            Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

            ListStore listStore;
            ФункціїДляЗвітів.СтворитиМодельДаних(out listStore, columnsName);

            TreeView treeView = new TreeView(listStore);
            treeView.ButtonPressEvent += ФункціїДляЗвітів.OpenPageDirectoryOrDocument;

            ФункціїДляЗвітів.СтворитиКолонкиДляДерева(treeView, columnsName, ВидиміКолонки, КолонкиДаних, ПозиціяТекстуВКолонці);
            ФункціїДляЗвітів.ЗаповнитиМодельДаними(listStore, columnsName, listRow);

            ФункціїДляЗвітів.CreateReportNotebookPage(reportNotebook, "Залишки та обороти", ВідобразитиФільтр("ЗалишкиТаОбороти", Фільтр), treeView, ЗалишкиТаОбороти, Фільтр, refreshPage);
        }

        void Документи(object? Параметри, bool refreshPage = false)
        {
            ПараметриФільтр Фільтр = Параметри != null ? (ПараметриФільтр)Параметри : new ПараметриФільтр();

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
            (РозрахункиЗПостачальниками.period >= @ПочатокПеріоду AND РозрахункиЗПостачальниками.period <= @КінецьПеріоду)
    
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
            (РозрахункиЗКлієнтами.period >= @ПочатокПеріоду AND РозрахункиЗКлієнтами.period <= @КінецьПеріоду)
    ) AS КлієнтиТаПостачальники
";

            #region WHERE

            //Відбір по вибраному елементу Контрагенти
            if (!Фільтр.Контрагент.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
КлієнтиТаПостачальники.Контрагент = '{Фільтр.Контрагент.UnigueID}'
";
            }

            //Відбір по вибраному елементу Валюти
            if (!Фільтр.Валюта.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
КлієнтиТаПостачальники.Валюта = '{Фільтр.Валюта.UnigueID}'
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
                if (!Фільтр.Контрагент_Папка.IsEmpty())
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
            WHERE {Контрагенти_Папки_Const.TABLE}.uid = '{Фільтр.Контрагент_Папка.UnigueID}' 
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
            //ВидиміКолонки.Add("uid_and_text", "uid_and_text");
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
            paramQuery.Add("ПочатокПеріоду", Фільтр.ДатаПочатокПеріоду);
            paramQuery.Add("КінецьПеріоду", Фільтр.ДатаКінецьПеріоду);

            string[] columnsName;
            List<Dictionary<string, object>> listRow;

            Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

            ListStore listStore;
            ФункціїДляЗвітів.СтворитиМодельДаних(out listStore, columnsName);

            TreeView treeView = new TreeView(listStore);
            treeView.ButtonPressEvent += ФункціїДляЗвітів.OpenPageDirectoryOrDocument;

            ФункціїДляЗвітів.СтворитиКолонкиДляДерева(treeView, columnsName, ВидиміКолонки, КолонкиДаних, ПозиціяТекстуВКолонці);
            ФункціїДляЗвітів.ЗаповнитиМодельДаними(listStore, columnsName, listRow);

            ФункціїДляЗвітів.CreateReportNotebookPage(reportNotebook, "Документи", ВідобразитиФільтр("Документи", Фільтр), treeView, Документи, Фільтр, refreshPage);
        }

    }
}