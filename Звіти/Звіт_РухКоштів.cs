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
    class Звіт_РухКоштів : VBox
    {
        Notebook reportNotebook;

        #region Filters

        DateTimeControl ДатаПочатокПеріоду = new DateTimeControl() { OnlyDate = true, Value = DateTime.Parse($"01.{DateTime.Now.Month}.{DateTime.Now.Year}") };
        DateTimeControl ДатаКінецьПеріоду = new DateTimeControl() { OnlyDate = true, Value = DateTime.Now };

        Організації_PointerControl Організація = new Організації_PointerControl();
        Каси_PointerControl Каса = new Каси_PointerControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();

        struct ПараметриФільтр
        {
            public DateTime ДатаПочатокПеріоду;
            public DateTime ДатаКінецьПеріоду;
            public Організації_Pointer Організація;
            public Каси_Pointer Каса;
            public Валюти_Pointer Валюта;
        }

        #endregion

        public Звіт_РухКоштів() : base()
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

            reportNotebook = new Notebook() { Scrollable = true, EnablePopup = true, BorderWidth = 0, ShowBorder = false, TabPos = PositionType.Top };
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

        ПараметриФільтр СформуватиФільтр()
        {
            return new ПараметриФільтр()
            {
                ДатаПочатокПеріоду = ДатаПочатокПеріоду.ПочатокДня(),
                ДатаКінецьПеріоду = ДатаКінецьПеріоду.КінецьДня(),
                Організація = Організація.Pointer,
                Каса = Каса.Pointer,
                Валюта = Валюта.Pointer
            };
        }

        async ValueTask<HBox> ВідобразитиФільтр(string typeReport, ПараметриФільтр Фільтр)
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

            if (!Фільтр.Організація.IsEmpty())
                text += "Організація: <b>" + await Фільтр.Організація.GetPresentation() + "</b>; ";

            if (!Фільтр.Каса.IsEmpty())
                text += "Каса: <b>" + await Фільтр.Каса.GetPresentation() + "</b>; ";

            if (!Фільтр.Валюта.IsEmpty())
                text += "Валюта: <b>" + await Фільтр.Валюта.GetPresentation() + "</b>; ";

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

        async void Залишки(object? Параметри, bool refreshPage = false)
        {
            ПараметриФільтр Фільтр = Параметри != null ? (ПараметриФільтр)Параметри : new ПараметриФільтр();

            #region SELECT

            bool isExistParent = false;

            string query = $@"
SELECT 
    РухКоштів.{РухКоштів_Залишки_TablePart.Організація} AS Організація,
    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва,
    РухКоштів.{РухКоштів_Залишки_TablePart.Каса} AS Каса,
    Довідник_Каси.{Каси_Const.Назва} AS Каса_Назва,
    РухКоштів.{РухКоштів_Залишки_TablePart.Валюта} AS Валюта,
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    ROUND(SUM(РухКоштів.{РухКоштів_Залишки_TablePart.Сума}), 2) AS Сума
FROM 
    {РухКоштів_Залишки_TablePart.TABLE} AS РухКоштів
    LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = 
        РухКоштів.{РухКоштів_Залишки_TablePart.Організація}
    LEFT JOIN {Каси_Const.TABLE} AS Довідник_Каси ON Довідник_Каси.uid = 
        РухКоштів.{РухКоштів_Залишки_TablePart.Каса}
    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = 
        РухКоштів.{РухКоштів_Залишки_TablePart.Валюта}
";

            #region WHERE

            //Відбір по вибраному елементу Організація
            if (!Фільтр.Організація.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Організації.uid = '{Фільтр.Організація.UnigueID}'
";
            }

            //Відбір по вибраному елементу Каса
            if (!Фільтр.Каса.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Каси.uid = '{Фільтр.Каса.UnigueID}'
";
            }

            //Відбір по вибраному елементу Склади
            if (!Фільтр.Валюта.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Валюти.uid = '{Фільтр.Валюта.UnigueID}'
";
            }

            #endregion

            query += $@"
GROUP BY 
    Організація, Організація_Назва, 
    Каса, Каса_Назва, 
    Валюта, Валюта_Назва
HAVING 
    SUM(РухКоштів.{РухКоштів_Залишки_TablePart.Сума}) != 0
ORDER BY 
    Організація_Назва, 
    Каса_Назва, 
    Валюта_Назва
";
            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>
            {
                { "Організація_Назва", "Організація" },
                { "Каса_Назва", "Каса" },
                { "Валюта_Назва", "Валюта" },
                { "Сума", "Сума" }
            };

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>
            {
                { "Організація_Назва", "Організація" },
                { "Каса_Назва", "Каса" },
                { "Валюта_Назва", "Валюта" }
            };

            Dictionary<string, string> ТипиДаних = new Dictionary<string, string>
            {
                { "Організація_Назва", Організації_Const.POINTER },
                { "Каса_Назва", Каси_Const.POINTER },
                { "Валюта_Назва", Валюти_Const.POINTER }
            };

            Dictionary<string, float> ПозиціяТекстуВКолонці = new Dictionary<string, float>
            {
                { "Сума", 1 }
            };

            Dictionary<string, TreeCellDataFunc> ФункціяДляКолонки = new Dictionary<string, TreeCellDataFunc>
            {
                { "Сума", ФункціїДляЗвітів.ФункціяДляКолонкиВідємнеЧислоЧервоним }
            };

            var recordResult = await Config.Kernel.DataBase.SelectRequestAsync(query);

            ListStore listStore;
            ФункціїДляЗвітів.СтворитиМодельДаних(out listStore, recordResult.ColumnsName);

            TreeView treeView = new TreeView(listStore);
            treeView.ButtonPressEvent += ФункціїДляЗвітів.OpenPageDirectoryOrDocument;

            ФункціїДляЗвітів.СтворитиКолонкиДляДерева(treeView, recordResult.ColumnsName, ВидиміКолонки, КолонкиДаних, ТипиДаних, ПозиціяТекстуВКолонці, ФункціяДляКолонки);
            ФункціїДляЗвітів.ЗаповнитиМодельДаними(listStore, recordResult.ColumnsName, recordResult.ListRow);

            ФункціїДляЗвітів.CreateReportNotebookPage(reportNotebook, "Залишки", await ВідобразитиФільтр("Залишки", Фільтр), treeView, Залишки, Фільтр, refreshPage);
        }

        async void ЗалишкиТаОбороти(object? Параметри, bool refreshPage = false)
        {
            ПараметриФільтр Фільтр = Параметри != null ? (ПараметриФільтр)Параметри : new ПараметриФільтр();

            #region SELECT

            bool isExistParent = false;

            string query = $@"
WITH
ПочатковийЗалишок AS
(
    SELECT
        РухКоштів.{РухКоштів_Залишки_TablePart.Організація} AS Організація,
        РухКоштів.{РухКоштів_Залишки_TablePart.Каса} AS Каса,
        РухКоштів.{РухКоштів_Залишки_TablePart.Валюта} AS Валюта,
        SUM(РухКоштів.{РухКоштів_Залишки_TablePart.Сума}) AS Сума
    FROM 
        {РухКоштів_Залишки_TablePart.TABLE} AS РухКоштів
    WHERE
        РухКоштів.{РухКоштів_Залишки_TablePart.Період} < @ПочатокПеріоду
    GROUP BY Організація, Каса, Валюта
),
ЗалишкиТаОборотиЗаПеріод AS
(
    SELECT
        РухКоштів.{РухКоштів_ЗалишкиТаОбороти_TablePart.Організація} AS Організація,
        РухКоштів.{РухКоштів_ЗалишкиТаОбороти_TablePart.Каса} AS Каса,
        РухКоштів.{РухКоштів_ЗалишкиТаОбороти_TablePart.Валюта} AS Валюта,
        SUM(РухКоштів.{РухКоштів_ЗалишкиТаОбороти_TablePart.СумаПрихід}) AS СумаПрихід,
        SUM(РухКоштів.{РухКоштів_ЗалишкиТаОбороти_TablePart.СумаРозхід}) AS СумаРозхід,
        SUM(РухКоштів.{РухКоштів_ЗалишкиТаОбороти_TablePart.СумаЗалишок}) AS СумаЗалишок
    FROM 
        {РухКоштів_ЗалишкиТаОбороти_TablePart.TABLE} AS РухКоштів
    WHERE
        РухКоштів.{РухКоштів_ЗалишкиТаОбороти_TablePart.Період} >= @ПочатокПеріоду AND
        РухКоштів.{РухКоштів_ЗалишкиТаОбороти_TablePart.Період} <= @КінецьПеріоду
    GROUP BY Організація, Каса, Валюта
),
КінцевийЗалишок AS
(
    SELECT 
        Організація,
        Каса,
        Валюта,
        Сума AS Сума
    FROM ПочатковийЗалишок

    UNION ALL

    SELECT
        Організація,
        Каса,
        Валюта,
        СумаЗалишок
    FROM ЗалишкиТаОборотиЗаПеріод
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
    ROUND(SUM(КінцевийЗалишок), 2) AS КінцевийЗалишок
FROM 
(
    SELECT 
        Організація,
        Каса,
        Валюта,
        Сума AS ПочатковийЗалишок,
        0 AS Прихід,
        0 AS Розхід,
        0 AS КінцевийЗалишок
    FROM ПочатковийЗалишок

    UNION ALL

    SELECT
        Організація,
        Каса,
        Валюта,
        0 AS ПочатковийЗалишок,
        СумаПрихід AS Прихід,
        СумаРозхід AS Розхід,
        0 AS КінцевийЗалишок
    FROM ЗалишкиТаОборотиЗаПеріод

    UNION ALL

    SELECT
        Організація,
        Каса,
        Валюта,
        0 AS ПочатковийЗалишок,
        0 AS Прихід,
        0 AS Розхід,
        Сума AS КінцевийЗалишок
    FROM КінцевийЗалишок
) AS ЗалишкиТаОбороти
LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = ЗалишкиТаОбороти.Організація
LEFT JOIN {Каси_Const.TABLE} AS Довідник_Каси ON Довідник_Каси.uid = ЗалишкиТаОбороти.Каса
LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = ЗалишкиТаОбороти.Валюта
";

            #region WHERE

            //Відбір по вибраному елементу Організації
            if (!Фільтр.Організація.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Організації.uid = '{Фільтр.Організація.UnigueID}'
";
            }

            //Відбір по вибраному елементу Валюти
            if (!Фільтр.Каса.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Каси.uid = '{Фільтр.Каса.UnigueID}'
";
            }

            //Відбір по вибраному елементу Валюти
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
GROUP BY 
    Організація, Організація_Назва, 
    Каса, Каса_Назва, 
    Валюта, Валюта_Назва
HAVING 
    SUM(Прихід) != 0 OR SUM(Розхід) != 0
ORDER BY 
    Організація_Назва, 
    Каса_Назва, 
    Валюта_Назва
";

            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>
            {
                { "Організація_Назва", "Організація" },
                { "Каса_Назва", "Каса" },
                { "Валюта_Назва", "Валюта" },
                { "ПочатковийЗалишок", "На початок" },
                { "Прихід", "Прихід" },
                { "Розхід", "Розхід" },
                { "КінцевийЗалишок", "На кінець" }
            };

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>
            {
                { "Організація_Назва", "Організація" },
                { "Каса_Назва", "Каса" },
                { "Валюта_Назва", "Валюта" }
            };

            Dictionary<string, string> ТипиДаних = new Dictionary<string, string>
            {
                { "Організація_Назва", Організації_Const.POINTER },
                { "Каса_Назва", Каси_Const.POINTER },
                { "Валюта_Назва", Валюти_Const.POINTER }
            };

            Dictionary<string, float> ПозиціяТекстуВКолонці = new Dictionary<string, float>
            {
                { "ПочатковийЗалишок", 1 },
                { "Прихід", 1 },
                { "Розхід", 1 },
                { "КінцевийЗалишок", 1 }
            };

            Dictionary<string, TreeCellDataFunc> ФункціяДляКолонки = new Dictionary<string, TreeCellDataFunc>
            {
                { "ПочатковийЗалишок", ФункціїДляЗвітів.ФункціяДляКолонкиВідємнеЧислоЧервоним },
                { "Прихід", ФункціїДляЗвітів.ФункціяДляКолонкиВідємнеЧислоЧервоним },
                { "Розхід", ФункціїДляЗвітів.ФункціяДляКолонкиВідємнеЧислоЧервоним },
                { "КінцевийЗалишок", ФункціїДляЗвітів.ФункціяДляКолонкиВідємнеЧислоЧервоним }
            };

            Dictionary<string, object> paramQuery = new Dictionary<string, object>
            {
                { "ПочатокПеріоду", Фільтр.ДатаПочатокПеріоду },
                { "КінецьПеріоду", Фільтр.ДатаКінецьПеріоду }
            };

            var recordResult = await Config.Kernel.DataBase.SelectRequestAsync(query, paramQuery);

            ListStore listStore;
            ФункціїДляЗвітів.СтворитиМодельДаних(out listStore, recordResult.ColumnsName);

            TreeView treeView = new TreeView(listStore);
            treeView.ButtonPressEvent += ФункціїДляЗвітів.OpenPageDirectoryOrDocument;

            ФункціїДляЗвітів.СтворитиКолонкиДляДерева(treeView, recordResult.ColumnsName, ВидиміКолонки, КолонкиДаних, КолонкиДаних, ПозиціяТекстуВКолонці, ФункціяДляКолонки);
            ФункціїДляЗвітів.ЗаповнитиМодельДаними(listStore, recordResult.ColumnsName, recordResult.ListRow);

            ФункціїДляЗвітів.CreateReportNotebookPage(reportNotebook, "Залишки та обороти", await ВідобразитиФільтр("ЗалишкиТаОбороти", Фільтр), treeView, ЗалишкиТаОбороти, Фільтр, refreshPage);
        }

        async void Документи(object? Параметри, bool refreshPage = false)
        {
            ПараметриФільтр Фільтр = Параметри != null ? (ПараметриФільтр)Параметри : new ПараметриФільтр();

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
        (РухКоштів.period >= @ПочатокПеріоду AND РухКоштів.period <= @КінецьПеріоду)
";

            #region WHERE

            isExistParent = true;

            //Відбір по вибраному елементу Організації
            if (!Фільтр.Організація.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                query += $@"
РухКоштів.{РухКоштів_Const.Організація} = '{Фільтр.Організація.UnigueID}'
";
            }

            //Відбір по вибраному елементу Валюти
            if (!Фільтр.Каса.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
РухКоштів.{РухКоштів_Const.Каса} = '{Фільтр.Каса.UnigueID}'
";
            }

            //Відбір по вибраному елементу Валюти
            if (!Фільтр.Валюта.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
РухКоштів.{РухКоштів_Const.Валюта} = '{Фільтр.Валюта.UnigueID}'
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
    CONCAT(uid, ':', doctype) AS uid_and_text,
    uid,
    period, 
    (CASE WHEN income = true THEN '+' ELSE '-' END) AS income,
    docname AS Документ,
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

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>
            {
                { "income", "Рух" },
                { "Документ", "Документ" },
                { "Організація_Назва", "Організація" },
                { "Каса_Назва", "Каса" },
                { "Валюта_Назва", "Валюта" },
                { "Сума", "Сума" }
            };

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>
            {
                { "Документ", "uid_and_text" },
                { "Організація_Назва", "Організація" },
                { "Каса_Назва", "Каса" },
                { "Валюта_Назва", "Валюта" }
            };

            Dictionary<string, string> ТипиДаних = new Dictionary<string, string>
            {
                { "Документ", "Документи.*" },
                { "Організація_Назва", Організації_Const.POINTER },
                { "Каса_Назва", Каси_Const.POINTER },
                { "Валюта_Назва", Валюти_Const.POINTER }
            };

            Dictionary<string, float> ПозиціяТекстуВКолонці = new Dictionary<string, float>
            {
                { "income", 0.5f },
                { "Сума", 1 }
            };

            Dictionary<string, TreeCellDataFunc> ФункціяДляКолонки = new Dictionary<string, TreeCellDataFunc>
            {
                { "Сума", ФункціїДляЗвітів.ФункціяДляКолонкиВідємнеЧислоЧервоним }
            };

            Dictionary<string, object> paramQuery = new Dictionary<string, object>
            {
                { "ПочатокПеріоду", Фільтр.ДатаПочатокПеріоду },
                { "КінецьПеріоду", Фільтр.ДатаКінецьПеріоду }
            };

            var recordResult = await Config.Kernel.DataBase.SelectRequestAsync(query, paramQuery);

            ListStore listStore;
            ФункціїДляЗвітів.СтворитиМодельДаних(out listStore, recordResult.ColumnsName);

            TreeView treeView = new TreeView(listStore);
            treeView.ButtonPressEvent += ФункціїДляЗвітів.OpenPageDirectoryOrDocument;

            ФункціїДляЗвітів.СтворитиКолонкиДляДерева(treeView, recordResult.ColumnsName, ВидиміКолонки, КолонкиДаних, ТипиДаних, ПозиціяТекстуВКолонці, ФункціяДляКолонки);
            ФункціїДляЗвітів.ЗаповнитиМодельДаними(listStore, recordResult.ColumnsName, recordResult.ListRow);

            ФункціїДляЗвітів.CreateReportNotebookPage(reportNotebook, "Документи", await ВідобразитиФільтр("Документи", Фільтр), treeView, Документи, Фільтр, refreshPage);
        }

    }
}