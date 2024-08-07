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
using InterfaceGtk;

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

        struct ПараметриФільтр
        {
            public DateTime ДатаПочатокПеріоду;
            public DateTime ДатаКінецьПеріоду;
            public Організації_Pointer Організація;
            public Номенклатура_Pointer Номенклатура;
            public Номенклатура_Папки_Pointer Номенклатура_Папка;
            public ХарактеристикиНоменклатури_Pointer ХарактеристикиНоменклатури;
            public Склади_Pointer Склад;
            public Склади_Папки_Pointer Склад_Папка;
            public СеріїНоменклатури_Pointer Серія;
        }

        #endregion

        public Звіт_ПартіїТоварів() : base()
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

            //Серія
            HBox hBoxSeria = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSeria, false, false, 5);

            hBoxSeria.PackStart(Серія, false, false, 5);
        }

        #endregion

        ПараметриФільтр СформуватиФільтр()
        {
            return new ПараметриФільтр()
            {
                ДатаПочатокПеріоду = ДатаПочатокПеріоду.ПочатокДня(),
                ДатаКінецьПеріоду = ДатаКінецьПеріоду.КінецьДня(),
                Організація = Організація.Pointer,
                Номенклатура = Номенклатура.Pointer,
                Номенклатура_Папка = Номенклатура_Папка.Pointer,
                ХарактеристикиНоменклатури = ХарактеристикиНоменклатури.Pointer,
                Склад = Склад.Pointer,
                Склад_Папка = Склад_Папка.Pointer,
                Серія = Серія.Pointer
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

            if (!Фільтр.Серія.IsEmpty())
                text += "Серія: <b>" + await Фільтр.Серія.GetPresentation() + "</b>; ";

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
    ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Рядок} AS Рядок,
    ROUND(SUM(ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Кількість}), 2) AS Кількість,
    ROUND(SUM(ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Собівартість}), 2) AS Собівартість,
    Довідник_ПакуванняОдиниціВиміру.uid AS ОдиницяВиміру,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS ОдиницяВиміру_Назва
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
    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON Довідник_ПакуванняОдиниціВиміру.uid = 
        Довідник_Номенклатура.{Номенклатура_Const.ОдиницяВиміру}
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

            //Відбір по вибраному елементу Серії Номенклатури
            if (!Фільтр.Серія.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_СеріїНоменклатури.uid = '{Фільтр.Серія.UnigueID}'
";
            }

            #endregion

            query += $@"
GROUP BY 
    Організація, Організація_Назва, 
    ПартіяТоварівКомпозит, ПартіяТоварівКомпозит_Назва, ПартіяТоварівКомпозит_Дата,
    Номенклатура, Номенклатура_Назва, 
    ХарактеристикаНоменклатури, ХарактеристикаНоменклатури_Назва,
    Серія, Серія_Номер,
    Склад, Склад_Назва,
    Рядок,
    ОдиницяВиміру, ОдиницяВиміру_Назва
HAVING
    SUM(ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Кількість}) != 0 OR
    SUM(ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Собівартість}) != 0   
ORDER BY 
    Організація_Назва, 
    ПартіяТоварівКомпозит_Дата ASC, 
    Номенклатура_Назва, 
    ХарактеристикаНоменклатури_Назва
";

            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>
            {
                { "Організація_Назва", "Організація" },
                { "ПартіяТоварівКомпозит_Назва", "Партія" },
                { "Номенклатура_Назва", "Номенклатура" },
                { "Склад_Назва", "Склад" },
                { "Рядок", "Рядок" },
                { "Кількість", "Кількість" },
                { "Собівартість", "Собівартість" },
                { "ОдиницяВиміру_Назва", "Пакування" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                ВидиміКолонки.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");
            if (Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const)
                ВидиміКолонки.Add("Серія_Номер", "Серія");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>
            {
                { "Організація_Назва", "Організація" },
                { "ПартіяТоварівКомпозит_Назва", "ПартіяТоварівКомпозит" },
                { "Номенклатура_Назва", "Номенклатура" },
                { "Склад_Назва", "Склад" },
                { "ОдиницяВиміру_Назва", "ОдиницяВиміру" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                КолонкиДаних.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");
            if (Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const)
                КолонкиДаних.Add("Серія_Номер", "Серія");

            Dictionary<string, string> ТипиДаних = new Dictionary<string, string>
            {
                { "Організація_Назва", Організації_Const.POINTER },
                { "ПартіяТоварівКомпозит_Назва", ПартіяТоварівКомпозит_Const.POINTER },
                { "Номенклатура_Назва", Номенклатура_Const.POINTER },
                { "ХарактеристикаНоменклатури_Назва", ХарактеристикиНоменклатури_Const.POINTER },
                { "Серія_Номер", СеріїНоменклатури_Const.POINTER },
                { "Склад_Назва", Склади_Const.POINTER },
                { "ОдиницяВиміру_Назва", ПакуванняОдиниціВиміру_Const.POINTER }
            };

            Dictionary<string, float> ПозиціяТекстуВКолонці = new Dictionary<string, float>
            {
                { "Рядок", 0.1f },
                { "Кількість", 1 },
                { "Собівартість", 1 }
            };

            Dictionary<string, TreeCellDataFunc> ФункціяДляКолонки = new Dictionary<string, TreeCellDataFunc>
            {
                { "Рядок", CellDataFunc },
                { "Кількість", CellDataFunc },
                { "Собівартість", CellDataFunc }
            };

            var recordResult = await Config.Kernel.DataBase.SelectRequest(query);

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
        ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Організація} AS Організація, 
        ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.ПартіяТоварівКомпозит} AS ПартіяТоварівКомпозит,
        ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Номенклатура} AS Номенклатура, 
        ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Серія} AS Серія,
        ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Склад} AS Склад,
        ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Рядок} AS Рядок,
        SUM(ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Кількість}) AS Кількість,
        SUM(ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Собівартість}) AS Собівартість
    FROM 
        {ПартіїТоварів_Залишки_TablePart.TABLE} AS ПартіїТоварів
    WHERE
        ПартіїТоварів.{ПартіїТоварів_Залишки_TablePart.Період} < @ПочатокПеріоду
    GROUP BY Організація, ПартіяТоварівКомпозит, Номенклатура, ХарактеристикаНоменклатури, Серія, Склад, Рядок
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
        ПартіїТоварів.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.Рядок} AS Рядок,
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
    GROUP BY Організація, ПартіяТоварівКомпозит, Номенклатура, ХарактеристикаНоменклатури, Серія, Склад, Рядок
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
        Рядок,
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
        Рядок,
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
    Рядок,
    ROUND(SUM(КількістьПочатковийЗалишок), 2) AS КількістьПочатковийЗалишок,
    ROUND(SUM(СобівартістьПочатковийЗалишок), 2) AS СобівартістьПочатковийЗалишок,
    ROUND(SUM(КількістьПрихід), 2) AS КількістьПрихід,
    ROUND(SUM(КількістьРозхід), 2) AS КількістьРозхід,
    ROUND(SUM(СобівартістьПрихід), 2) AS СобівартістьПрихід,
    ROUND(SUM(СобівартістьРозхід), 2) AS СобівартістьРозхід,
    ROUND(SUM(КількістьКінцевийЗалишок), 2) AS КількістьКінцевийЗалишок,
    ROUND(SUM(СобівартістьКінцевийЗалишок), 2) AS СобівартістьКінцевийЗалишок,
    Довідник_ПакуванняОдиниціВиміру.uid AS ОдиницяВиміру,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS ОдиницяВиміру_Назва
FROM 
(
    SELECT 
        Організація,
        ПартіяТоварівКомпозит,
        Номенклатура,
        ХарактеристикаНоменклатури,
        Серія,
        Склад,
        Рядок,
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
        Рядок,
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
        Рядок,
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
LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON Довідник_ПакуванняОдиниціВиміру.uid = Довідник_Номенклатура.{Номенклатура_Const.ОдиницяВиміру}
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

            //Відбір по вибраному елементу Серії Номенклатури
            if (!Фільтр.Серія.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_СеріїНоменклатури.uid = '{Фільтр.Серія.UnigueID}'
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
    Склад, Склад_Назва,
    Рядок,
    ОдиницяВиміру, ОдиницяВиміру_Назва
HAVING 
    SUM(КількістьПрихід) != 0 OR SUM(КількістьРозхід) != 0 OR
    SUM(СобівартістьПрихід) != 0 OR SUM(СобівартістьРозхід) != 0
ORDER BY 
    Організація_Назва, 
    ПартіяТоварівКомпозит_Дата, 
    Номенклатура_Назва, 
    ХарактеристикаНоменклатури_Назва
";

            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>
            {
                { "Організація_Назва", "Організація" },
                { "ПартіяТоварівКомпозит_Назва", "Партія" },
                { "Номенклатура_Назва", "Номенклатура" },
                { "Склад_Назва", "Склад" },
                { "Рядок", "Рядок" },
                { "КількістьПочатковийЗалишок", "Кількість" },
                { "СобівартістьПочатковийЗалишок", "Собівартість" },
                { "КількістьПрихід", "Кількість +" },
                { "КількістьРозхід", "Кількість -" },
                { "СобівартістьПрихід", "Собівартість +" },
                { "СобівартістьРозхід", "Собівартість -" },
                { "КількістьКінцевийЗалишок", "Кількість" },
                { "СобівартістьКінцевийЗалишок", "Собівартість" },
                { "ОдиницяВиміру_Назва", "Пакування" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                ВидиміКолонки.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");
            if (Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const)
                ВидиміКолонки.Add("Серія_Номер", "Серія");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>
            {
                { "Номенклатура_Назва", "Номенклатура" },
                { "Склад_Назва", "Склад" },
                { "ОдиницяВиміру_Назва", "ОдиницяВиміру" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                КолонкиДаних.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");
            if (Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const)
                КолонкиДаних.Add("Серія_Номер", "Серія");

            Dictionary<string, string> ТипиДаних = new Dictionary<string, string>
            {
                { "Номенклатура_Назва", Номенклатура_Const.POINTER },
                { "ХарактеристикаНоменклатури_Назва", ХарактеристикиНоменклатури_Const.POINTER },
                { "Серія_Номер", СеріїНоменклатури_Const.POINTER },
                { "Склад_Назва", Склади_Const.POINTER },
                { "ОдиницяВиміру_Назва", ПакуванняОдиниціВиміру_Const.POINTER }
            };

            Dictionary<string, float> ПозиціяТекстуВКолонці = new Dictionary<string, float>
            {
                { "Рядок", 0.1f },
                { "КількістьПочатковийЗалишок", 1 },
                { "СобівартістьПочатковийЗалишок", 1 },
                { "КількістьПрихід", 1 },
                { "КількістьРозхід", 1 },
                { "СобівартістьПрихід", 1 },
                { "СобівартістьРозхід", 1 },
                { "КількістьКінцевийЗалишок", 1 },
                { "СобівартістьКінцевийЗалишок", 1 }
            };

            Dictionary<string, TreeCellDataFunc> ФункціяДляКолонки = new Dictionary<string, TreeCellDataFunc>
            {
                { "Рядок", CellDataFunc },
                { "КількістьПочатковийЗалишок", CellDataFunc },
                { "СобівартістьПочатковийЗалишок", CellDataFunc },
                { "КількістьПрихід", CellDataFunc },
                { "КількістьРозхід", CellDataFunc },
                { "СобівартістьПрихід", CellDataFunc },
                { "СобівартістьРозхід", CellDataFunc },
                { "КількістьКінцевийЗалишок", CellDataFunc },
                { "СобівартістьКінцевийЗалишок", CellDataFunc }
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
        ПартіїТоварів.period AS period,
        ПартіїТоварів.owner AS owner,
        ПартіїТоварів.income AS income,
        ПартіїТоварів.{ПартіїТоварів_Const.Організація} AS Організація,
        ПартіїТоварів.{ПартіїТоварів_Const.ПартіяТоварівКомпозит} AS ПартіяТоварівКомпозит,
        ПартіїТоварів.{ПартіїТоварів_Const.Номенклатура} AS Номенклатура,
        ПартіїТоварів.{ПартіїТоварів_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ПартіїТоварів.{ПартіїТоварів_Const.Серія} AS Серія,
        ПартіїТоварів.{ПартіїТоварів_Const.Склад} AS Склад,
        ПартіїТоварів.{ПартіїТоварів_Const.Рядок} AS Рядок,
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
            if (!Фільтр.Організація.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ПартіїТоварів.{ПартіїТоварів_Const.Організація} = '{Фільтр.Організація.UnigueID}'
";
            }

            //Відбір по вибраному елементу Номенклатура
            if (!Фільтр.Номенклатура.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ПартіїТоварів.{ПартіїТоварів_Const.Номенклатура} = '{Фільтр.Номенклатура.UnigueID}'
";
            }

            //Відбір по вибраному елементу Характеристики Номенклатури
            if (!Фільтр.ХарактеристикиНоменклатури.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ПартіїТоварів.{ПартіїТоварів_Const.ХарактеристикаНоменклатури}= '{Фільтр.ХарактеристикиНоменклатури.UnigueID}'
";
            }

            //Відбір по вибраному елементу Склади
            if (!Фільтр.Склад.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ПартіїТоварів.{ПартіїТоварів_Const.Склад} = '{Фільтр.Склад.UnigueID}'
";
            }

            //Відбір по вибраному елементу Серії Номенклатури
            if (!Фільтр.Серія.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ПартіїТоварів.{ПартіїТоварів_Const.Серія} = '{Фільтр.Серія.UnigueID}'
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
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Рядок,
    Довідник_ПакуванняОдиниціВиміру.uid AS ОдиницяВиміру,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS ОдиницяВиміру_Назва
FROM register INNER JOIN {table} ON {table}.uid = register.owner
    LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = register.Організація
    LEFT JOIN {ПартіяТоварівКомпозит_Const.TABLE} AS Довідник_ПартіяТоварівКомпозит ON Довідник_ПартіяТоварівКомпозит.uid = register.ПартіяТоварівКомпозит
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = register.Номенклатура
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = register.ХарактеристикаНоменклатури
    LEFT JOIN {СеріїНоменклатури_Const.TABLE} AS Довідник_СеріїНоменклатури ON Довідник_СеріїНоменклатури.uid = register.Серія
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
    Рядок,
    ROUND(Кількість, 2) AS Кількість, 
    ROUND(Собівартість, 2) AS Собівартість,
    ОдиницяВиміру,
    ОдиницяВиміру_Назва
FROM documents
ORDER BY 
    period ASC, 
    Організація_Назва, 
    Номенклатура_Назва, 
    ХарактеристикаНоменклатури_Назва
";

            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>
            {
                { "income", "Рух" },
                { "Документ", "Документ" },
                { "Організація_Назва", "Організація" },
                { "ПартіяТоварівКомпозит_Назва", "Партія" },
                { "Номенклатура_Назва", "Номенклатура" },
                { "Склад_Назва", "Склад" },
                { "Рядок", "Рядок" },
                { "Кількість", "Кількість" },
                { "Собівартість", "Собівартість" },
                { "ОдиницяВиміру_Назва", "Пакування" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                ВидиміКолонки.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");
            if (Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const)
                ВидиміКолонки.Add("Серія_Номер", "Серія");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>
            {
                { "Документ", "uid_and_text" },
                { "Організація_Назва", "Організація" },
                { "ПартіяТоварівКомпозит_Назва", "ПартіяТоварівКомпозит" },
                { "Номенклатура_Назва", "Номенклатура" },
                { "Склад_Назва", "Склад" },
                { "ОдиницяВиміру_Назва", "ОдиницяВиміру" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                КолонкиДаних.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");
            if (Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const)
                КолонкиДаних.Add("Серія_Номер", "Серія");

            Dictionary<string, string> ТипиДаних = new Dictionary<string, string>
            {
                { "Документ", "Документи.*" },
                { "Організація_Назва", Організації_Const.POINTER },
                { "ПартіяТоварівКомпозит_Назва", ПартіяТоварівКомпозит_Const.POINTER },
                { "Номенклатура_Назва", Номенклатура_Const.POINTER },
                { "ХарактеристикаНоменклатури_Назва", ХарактеристикиНоменклатури_Const.POINTER },
                { "Серія_Номер", СеріїНоменклатури_Const.POINTER },
                { "Склад_Назва", Склади_Const.POINTER },
                { "ОдиницяВиміру_Назва", ПакуванняОдиниціВиміру_Const.POINTER }
            };

            Dictionary<string, float> ПозиціяТекстуВКолонці = new Dictionary<string, float>
            {
                { "income", 0.5f },
                { "Рядок", 0.1f },
                { "Кількість", 1 },
                { "Собівартість", 1 }
            };

            Dictionary<string, TreeCellDataFunc> ФункціяДляКолонки = new Dictionary<string, TreeCellDataFunc>
            {
                { "Рядок", CellDataFunc },
                { "Кількість", CellDataFunc },
                { "Собівартість", CellDataFunc }
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

        //Функція Для Колонки
        //Викликається для кожної ячейки колонки
        void CellDataFunc(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            CellRendererText cellText = (CellRendererText)cell;
            if (column.Data.Contains("CellDataFunc"))
            {
                switch ((string)column.Data["CellDataFunc"]!)
                {
                    case "Рядок":
                        {
                            cellText.Text = "№ " + cellText.Text;
                            break;
                        }
                    default:
                        {
                            float result;
                            if (float.TryParse(cellText.Text, out result))
                                cellText.Foreground = (result >= 0) ? "green" : "red";

                            break;
                        }
                }
            }
        }

    }
}