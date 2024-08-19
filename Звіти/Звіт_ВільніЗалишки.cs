/*
Copyright (C) 2019-2024 TARAKHOMYN YURIY IVANOVYCH
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
using InterfaceGtk;

using StorageAndTrade_1_0;
using Константи = StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade
{
    class Звіт_ВільніЗалишки : ФормаЕлемент
    {
        Notebook reportNotebook;

        #region Filters

        PeriodControl Період = new PeriodControl() { Period = ПеріодДляЖурналу.ТипПеріоду.Місяць, SensitiveSelectButton = false };
        Номенклатура_PointerControl Номенклатура = new Номенклатура_PointerControl();
        Номенклатура_Папки_PointerControl Номенклатура_Папка = new Номенклатура_Папки_PointerControl() { Caption = "Номенклатура папка:" };
        ХарактеристикиНоменклатури_PointerControl ХарактеристикиНоменклатури = new ХарактеристикиНоменклатури_PointerControl();
        Склади_PointerControl Склад = new Склади_PointerControl();
        Склади_Папки_PointerControl Склад_Папка = new Склади_Папки_PointerControl() { Caption = "Склад папка:" };

        struct ПараметриФільтр
        {
            public DateTime ДатаПочатокПеріоду;
            public DateTime ДатаКінецьПеріоду;
            public Номенклатура_Pointer Номенклатура;
            public Номенклатура_Папки_Pointer Номенклатура_Папка;
            public ХарактеристикиНоменклатури_Pointer ХарактеристикиНоменклатури;
            public Склади_Pointer Склад;
            public Склади_Папки_Pointer Склад_Папка;
        }

        #endregion

        public Звіт_ВільніЗалишки() : base()
        {
            //Кнопки
            Box hBoxTop = new Box(Orientation.Horizontal, 0);
            PackStart(hBoxTop, false, false, 10);

            //2
            Button bOstatok = new Button("Залишки");
            bOstatok.Clicked += OnReport_Залишки;

            hBoxTop.PackStart(bOstatok, false, false, 10);

            //3
            Button bDocuments = new Button("Документи");
            bDocuments.Clicked += OnReport_Документи;

            hBoxTop.PackStart(bDocuments, false, false, 10);

            CreatePeriod();
            CreateFilters();

            reportNotebook = new Notebook() { Scrollable = true, EnablePopup = true, BorderWidth = 0, ShowBorder = false, TabPos = PositionType.Top };
            PackStart(reportNotebook, true, true, 0);

            ShowAll();
        }

        #region Filters

        void CreatePeriod()
        {
            Box hBox = new Box(Orientation.Horizontal, 0);

            //Період
            CreateField(hBox, null, Період);

            PackStart(hBox, false, false, 5);
        }

        void CreateFilters()
        {
            Box hBoxContainer = new Box(Orientation.Horizontal, 0);

            Expander expander = new Expander("Відбори") { Expanded = true };
            expander.Add(hBoxContainer);

            //Container1
            Box vBoxContainer1 = new Box(Orientation.Vertical, 0) { WidthRequest = 500 };
            hBoxContainer.PackStart(vBoxContainer1, false, false, 5);

            CreateContainer1(vBoxContainer1);

            //Container2
            Box vBoxContainer2 = new Box(Orientation.Vertical, 0) { WidthRequest = 500 };
            hBoxContainer.PackStart(vBoxContainer2, false, false, 5);

            CreateContainer2(vBoxContainer2);

            PackStart(expander, false, false, 10);
        }

        void CreateContainer1(Box vBox)
        {
            //Номенклатура
            CreateField(vBox, null, Номенклатура);

            //ХарактеристикиНоменклатури
            CreateField(vBox, null, ХарактеристикиНоменклатури);
            ХарактеристикиНоменклатури.BeforeClickOpenFunc = () =>
            {
                ХарактеристикиНоменклатури.НоменклатураВласник = Номенклатура.Pointer;
            };

            //Склад
            CreateField(vBox, null, Склад);
        }

        void CreateContainer2(Box vBox)
        {
            //Номенклатура папка
            CreateField(vBox, null, Номенклатура_Папка);

            //Склад папка
            CreateField(vBox, null, Склад_Папка);
        }

        #endregion

        ПараметриФільтр СформуватиФільтр()
        {
            return new ПараметриФільтр()
            {
                ДатаПочатокПеріоду = Період.DateStartControl.ПочатокДня(),
                ДатаКінецьПеріоду = Період.DateStopControl.КінецьДня(),
                Номенклатура = Номенклатура.Pointer,
                Номенклатура_Папка = Номенклатура_Папка.Pointer,
                ХарактеристикиНоменклатури = ХарактеристикиНоменклатури.Pointer,
                Склад = Склад.Pointer,
                Склад_Папка = Склад_Папка.Pointer
            };
        }

        async ValueTask<Box> ВідобразитиФільтр(string typeReport, ПараметриФільтр Фільтр)
        {
            Box hBoxCaption = new Box(Orientation.Horizontal, 0);

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

            if (!Фільтр.Номенклатура.IsEmpty())
                text += "Номенклатура: <b>" + await Фільтр.Номенклатура.GetPresentation() + "</b>; ";

            if (!Фільтр.Номенклатура_Папка.IsEmpty())
                text += "Номенклатура папка: <b>" + await Фільтр.Номенклатура_Папка.GetPresentation() + "</b>; ";

            if (!Фільтр.ХарактеристикиНоменклатури.IsEmpty())
                text += "Характеристика: <b>" + await Фільтр.ХарактеристикиНоменклатури.GetPresentation() + "</b>; ";

            if (!Фільтр.Склад.IsEmpty())
                text += "Склад: <b>" + await Фільтр.Склад.GetPresentation() + "</b>; ";

            if (!Фільтр.Склад_Папка.IsEmpty())
                text += "Склад папка: <b>" + await Фільтр.Склад_Папка.GetPresentation() + "</b>; ";

            hBoxCaption.PackStart(new Label(text) { Wrap = true, UseMarkup = true }, false, false, 2);

            return hBoxCaption;
        }

        void OnReport_Залишки(object? sender, EventArgs args)
        {
            Залишки(СформуватиФільтр());
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
    ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    ROUND(SUM(ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.ВНаявності}), 2) AS ВНаявності,
    ROUND(SUM(ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.ВРезервіЗіСкладу}), 2) AS ВРезервіЗіСкладу,
    ROUND(SUM(ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.ВРезервіПідЗамовлення}), 2) AS ВРезервіПідЗамовлення,
    Довідник_ПакуванняОдиниціВиміру.uid AS ОдиницяВиміру,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS ОдиницяВиміру_Назва
FROM 
    {ВільніЗалишки_Залишки_TablePart.TABLE} AS ВільніЗалишки
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.Номенклатура}
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.ХарактеристикаНоменклатури}
    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
        ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.Склад}
    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON Довідник_ПакуванняОдиниціВиміру.uid = 
        Довідник_Номенклатура.{Номенклатура_Const.ОдиницяВиміру}
";
            #region WHERE

            //Відбір по всіх вкладених папках вибраної папки Номенклатури
            if (!Фільтр.Номенклатура_Папка.IsEmpty())
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
            WHERE {Номенклатура_Папки_Const.TABLE}.uid = '{Фільтр.Номенклатура_Папка.UnigueID}' 
            UNION ALL
            SELECT {Номенклатура_Папки_Const.TABLE}.uid
            FROM {Номенклатура_Папки_Const.TABLE}
                JOIN r ON {Номенклатура_Папки_Const.TABLE}.{Номенклатура_Папки_Const.Родич} = r.uid
        ) SELECT uid FROM r
    )
";
            }

            //Відбір по вибраному елементу Номенклатура
            if (!Фільтр.Номенклатура.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Номенклатура.uid = '{Фільтр.Номенклатура.UnigueID}'
";
            }

            //Відбір по вибраному елементу Характеристики Номенклатури
            if (!Фільтр.ХарактеристикиНоменклатури.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_ХарактеристикиНоменклатури.uid = '{Фільтр.ХарактеристикиНоменклатури.UnigueID}'
";
            }

            //Відбір по всіх вкладених папках вибраної папки Склади
            if (!Фільтр.Склад_Папка.IsEmpty())
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
            WHERE {Склади_Папки_Const.TABLE}.uid = '{Фільтр.Склад_Папка.UnigueID}' 
            UNION ALL
            SELECT {Склади_Папки_Const.TABLE}.uid
            FROM {Склади_Папки_Const.TABLE}
                JOIN r ON {Склади_Папки_Const.TABLE}.{Склади_Папки_Const.Родич} = r.uid
        ) SELECT uid FROM r
    )
";
            }

            //Відбір по вибраному елементу Склади
            if (!Фільтр.Склад.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Склади.uid = '{Фільтр.Склад.UnigueID}'
";
            }

            #endregion

            query += $@"
GROUP BY 
    Номенклатура, Номенклатура_Назва, 
    ХарактеристикаНоменклатури, ХарактеристикаНоменклатури_Назва,
    Склад, Склад_Назва,
    ОдиницяВиміру, ОдиницяВиміру_Назва
HAVING
    SUM(ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.ВНаявності}) != 0
OR
    SUM(ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.ВРезервіЗіСкладу}) != 0
OR
    SUM(ВільніЗалишки.{ВільніЗалишки_Залишки_TablePart.ВРезервіПідЗамовлення}) != 0   
ORDER BY 
    Номенклатура_Назва
";

            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>
            {
                { "Номенклатура_Назва", "Номенклатура" },
                { "Склад_Назва", "Склад" },
                { "ВНаявності", "В наявності" },
                { "ВРезервіЗіСкладу", "В резерві зі складу" },
                { "ВРезервіПідЗамовлення", "В резерві під замовлення" },
                { "ОдиницяВиміру_Назва", "Пакування" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                ВидиміКолонки.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>
            {
                { "Номенклатура_Назва", "Номенклатура" },
                { "Склад_Назва", "Склад" },
                { "ОдиницяВиміру_Назва", "ОдиницяВиміру" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                КолонкиДаних.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");

            Dictionary<string, string> ТипиДаних = new Dictionary<string, string>
            {
                { "Номенклатура_Назва", Номенклатура_Const.POINTER },
                { "ХарактеристикаНоменклатури_Назва", ХарактеристикиНоменклатури_Const.POINTER },
                { "Склад_Назва", Склади_Const.POINTER },
                { "ОдиницяВиміру_Назва", ПакуванняОдиниціВиміру_Const.POINTER }
            };

            Dictionary<string, float> ПозиціяТекстуВКолонці = new Dictionary<string, float>
            {
                { "ВНаявності", 1 },
                { "ВРезервіЗіСкладу", 1 },
                { "ВРезервіПідЗамовлення", 1 }
            };

            Dictionary<string, TreeCellDataFunc> ФункціяДляКолонки = new Dictionary<string, TreeCellDataFunc>
            {
                { "ВНаявності", ФункціїДляЗвітів.ФункціяДляКолонкиВідємнеЧислоЧервоним },
                { "ВРезервіЗіСкладу", ФункціїДляЗвітів.ФункціяДляКолонкиВідємнеЧислоЧервоним },
                { "ВРезервіПідЗамовлення", ФункціїДляЗвітів.ФункціяДляКолонкиВідємнеЧислоЧервоним }
            };

            var recordResult = await Config.Kernel.DataBase.SelectRequest(query);

            ФункціїДляЗвітів.СтворитиМодельДаних(out ListStore listStore, recordResult.ColumnsName);

            TreeView treeView = new TreeView(listStore);
            treeView.ButtonPressEvent += ФункціїДляЗвітів.OpenPageDirectoryOrDocument;

            ФункціїДляЗвітів.СтворитиКолонкиДляДерева(treeView, recordResult.ColumnsName, ВидиміКолонки, КолонкиДаних, ТипиДаних, ПозиціяТекстуВКолонці, ФункціяДляКолонки);
            ФункціїДляЗвітів.ЗаповнитиМодельДаними(listStore, recordResult.ColumnsName, recordResult.ListRow);

            ФункціїДляЗвітів.CreateReportNotebookPage(reportNotebook, "Залишки", await ВідобразитиФільтр("Залишки", Фільтр), treeView, Залишки, Фільтр, refreshPage);
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
            if (!Фільтр.Номенклатура.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ВільніЗалишки.{ВільніЗалишки_Const.Номенклатура} = '{Фільтр.Номенклатура.UnigueID}'
";
            }

            //Відбір по вибраному елементу Характеристики Номенклатури
            if (!Фільтр.ХарактеристикиНоменклатури.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ВільніЗалишки.{ВільніЗалишки_Const.ХарактеристикаНоменклатури} = '{Фільтр.ХарактеристикиНоменклатури.UnigueID}'
";
            }

            //Відбір по вибраному елементу Склади
            if (!Фільтр.Склад.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ВільніЗалишки.{ВільніЗалишки_Const.Склад} = '{Фільтр.Склад.UnigueID}'
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
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Довідник_ПакуванняОдиниціВиміру.uid AS ОдиницяВиміру,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS ОдиницяВиміру_Назва
FROM register INNER JOIN {table} ON {table}.uid = register.owner
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = register.Номенклатура
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = register.ХарактеристикаНоменклатури
    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = register.Склад
    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON Довідник_ПакуванняОдиниціВиміру.uid = Довідник_Номенклатура.{Номенклатура_Const.ОдиницяВиміру}
";

                #region WHERE

                isExistParent = false;

                //Відбір по всіх вкладених папках вибраної папки Номенклатури
                if (!Фільтр.Номенклатура_Папка.IsEmpty())
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
            WHERE {Номенклатура_Папки_Const.TABLE}.uid = '{Фільтр.Номенклатура_Папка.UnigueID}' 
            UNION ALL
            SELECT {Номенклатура_Папки_Const.TABLE}.uid
            FROM {Номенклатура_Папки_Const.TABLE}
                JOIN r ON {Номенклатура_Папки_Const.TABLE}.{Номенклатура_Папки_Const.Родич} = r.uid
        ) SELECT uid FROM r
    )
";
                }

                //Відбір по всіх вкладених папках вибраної папки Склади
                if (!Фільтр.Склад_Папка.IsEmpty())
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
            WHERE {Склади_Папки_Const.TABLE}.uid = '{Фільтр.Склад_Папка.UnigueID}' 
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
    ВРезервіПідЗамовлення,
    ОдиницяВиміру,
    ОдиницяВиміру_Назва
FROM documents
ORDER BY period ASC
";

            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>
            {
                { "income", "Рух" },
                { "Документ", "Документ" },
                { "Номенклатура_Назва", "Номенклатура" },
                { "Склад_Назва", "Склад" },
                { "ВНаявності", "В наявності" },
                { "ВРезервіЗіСкладу", "В резерві зі складу" },
                { "ВРезервіПідЗамовлення", "В резерві під замовлення" },
                { "ОдиницяВиміру_Назва", "Пакування" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                ВидиміКолонки.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>
            {
                { "Документ", "uid_and_text" },
                { "Номенклатура_Назва", "Номенклатура" },
                { "Склад_Назва", "Склад" },
                { "ОдиницяВиміру_Назва", "ОдиницяВиміру" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                КолонкиДаних.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");

            Dictionary<string, string> ТипиДаних = new Dictionary<string, string>
            {
                { "Документ", "Документи.*" },
                { "Номенклатура_Назва", Номенклатура_Const.POINTER },
                { "ХарактеристикаНоменклатури_Назва", ХарактеристикиНоменклатури_Const.POINTER },
                { "Склад_Назва", Склади_Const.POINTER },
                { "ОдиницяВиміру_Назва", ПакуванняОдиниціВиміру_Const.POINTER }
            };

            Dictionary<string, float> ПозиціяТекстуВКолонці = new Dictionary<string, float>
            {
                { "income", 0.5f },
                { "ВНаявності", 1 },
                { "ВРезервіЗіСкладу", 1 },
                { "ВРезервіПідЗамовлення", 1 }
            };

            Dictionary<string, TreeCellDataFunc> ФункціяДляКолонки = new Dictionary<string, TreeCellDataFunc>
            {
                { "ВНаявності", ФункціїДляЗвітів.ФункціяДляКолонкиВідємнеЧислоЧервоним },
                { "ВРезервіЗіСкладу", ФункціїДляЗвітів.ФункціяДляКолонкиВідємнеЧислоЧервоним },
                { "ВРезервіПідЗамовлення", ФункціїДляЗвітів.ФункціяДляКолонкиВідємнеЧислоЧервоним }
            };

            Dictionary<string, object> paramQuery = new Dictionary<string, object>
            {
                { "ПочатокПеріоду", Фільтр.ДатаПочатокПеріоду },
                { "КінецьПеріоду", Фільтр.ДатаКінецьПеріоду }
            };

            var recordResult = await Config.Kernel.DataBase.SelectRequest(query, paramQuery);

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