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
    class Звіт_ТовариВКоміркахНаСкладах : VBox
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
        СкладськіПриміщення_PointerControl СкладськеПриміщення = new СкладськіПриміщення_PointerControl() { Caption = "Приміщення:" };
        СкладськіКомірки_PointerControl СкладськаКомірка = new СкладськіКомірки_PointerControl() { Caption = "Комірка:" };
        СеріїНоменклатури_PointerControl Серія = new СеріїНоменклатури_PointerControl();

        struct ПараметриФільтр
        {
            public DateTime ДатаПочатокПеріоду;
            public DateTime ДатаКінецьПеріоду;
            public Номенклатура_Pointer Номенклатура;
            public Номенклатура_Папки_Pointer Номенклатура_Папка;
            public ХарактеристикиНоменклатури_Pointer ХарактеристикиНоменклатури;
            public Склади_Pointer Склад;
            public Склади_Папки_Pointer Склад_Папка;
            public СкладськіПриміщення_Pointer СкладськеПриміщення;
            public СкладськіКомірки_Pointer СкладськаКомірка;
            public СеріїНоменклатури_Pointer Серія;
        }

        #endregion

        public Звіт_ТовариВКоміркахНаСкладах() : base()
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

            //СкладськеПриміщення
            HBox hBoxSkaldPrem = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSkaldPrem, false, false, 5);

            СкладськеПриміщення.BeforeClickOpenFunc = () =>
            {
                СкладськеПриміщення.СкладВласник = Склад.Pointer;
            };

            hBoxSkaldPrem.PackStart(СкладськеПриміщення, false, false, 5);

            //СкладськаКомірка
            HBox hBoxSkaldCell = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSkaldCell, false, false, 5);

            СкладськаКомірка.BeforeClickOpenFunc = () =>
            {
                СкладськаКомірка.СкладПриміщенняВласник = СкладськеПриміщення.Pointer;
            };

            hBoxSkaldCell.PackStart(СкладськаКомірка, false, false, 5);
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
                Номенклатура = Номенклатура.Pointer,
                Номенклатура_Папка = Номенклатура_Папка.Pointer,
                ХарактеристикиНоменклатури = ХарактеристикиНоменклатури.Pointer,
                Склад = Склад.Pointer,
                Склад_Папка = Склад_Папка.Pointer,
                СкладськеПриміщення = СкладськеПриміщення.Pointer,
                СкладськаКомірка = СкладськаКомірка.Pointer,
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

            if (!Фільтр.СкладськеПриміщення.IsEmpty())
                text += "Складське приміщення: <b>" + await Фільтр.СкладськеПриміщення.GetPresentation() + "</b>; ";

            if (!Фільтр.СкладськаКомірка.IsEmpty())
                text += "Складська комірка: <b>" + await Фільтр.СкладськаКомірка.GetPresentation() + "</b>; ";

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
    ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Пакування} AS Пакування,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS Пакування_Назва,    

    Довідник_СкладськіПриміщення.{СкладськіПриміщення_Const.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Довідник_СкладськіКомірки.{СкладськіКомірки_Const.Приміщення} AS Приміщення,
    Довідник_СкладськіПриміщення.{СкладськіПриміщення_Const.Назва} AS Приміщення_Назва,
    ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Комірка} AS Комірка,
    Довідник_СкладськіКомірки.{СкладськіКомірки_Const.Назва} AS Комірка_Назва,
    
    ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Серія} AS Серія,
    Довідник_СеріїНоменклатури.{СеріїНоменклатури_Const.Номер} AS Серія_Номер,
    ROUND(SUM(ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.ВНаявності}), 2) AS ВНаявності
FROM 
    {ТовариВКомірках_Залишки_TablePart.TABLE} AS ТовариВКомірках
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Номенклатура}
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.ХарактеристикаНоменклатури}
    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON Довідник_ПакуванняОдиниціВиміру.uid = 
        ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Пакування}

    LEFT JOIN {СкладськіКомірки_Const.TABLE} AS Довідник_СкладськіКомірки ON Довідник_СкладськіКомірки.uid = 
        ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Комірка}
    LEFT JOIN {СкладськіПриміщення_Const.TABLE} AS Довідник_СкладськіПриміщення ON Довідник_СкладськіПриміщення.uid = 
        Довідник_СкладськіКомірки.{СкладськіКомірки_Const.Приміщення}
    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
        Довідник_СкладськіПриміщення.{СкладськіПриміщення_Const.Склад}

    LEFT JOIN {СеріїНоменклатури_Const.TABLE} AS Довідник_СеріїНоменклатури ON Довідник_СеріїНоменклатури.uid = 
        ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Серія}
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

            //Відбір по вибраному елементу СкладськеПриміщення
            if (!Фільтр.СкладськеПриміщення.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_СкладськіПриміщення.uid = '{Фільтр.СкладськеПриміщення.UnigueID}'
";
            }

            //Відбір по вибраному елементу СкладськаКомірка
            if (!Фільтр.СкладськаКомірка.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_СкладськіКомірки.uid = '{Фільтр.СкладськаКомірка.UnigueID}'
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
GROUP BY Номенклатура, Номенклатура_Назва, 
         ХарактеристикаНоменклатури, ХарактеристикаНоменклатури_Назва,
         Пакування, Пакування_Назва,
         Склад, Склад_Назва,
         Приміщення, Приміщення_Назва,
         Комірка, Комірка_Назва,
         Серія, Серія_Номер
HAVING
    SUM(ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.ВНаявності}) != 0  
ORDER BY Номенклатура_Назва
";
            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>
            {
                { "Номенклатура_Назва", "Номенклатура" },
                { "Пакування_Назва", "Пакування" },
                { "Склад_Назва", "Склад" },
                { "Приміщення_Назва", "Приміщення" },
                { "Комірка_Назва", "Комірка" },
                { "ВНаявності", "В наявності" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                ВидиміКолонки.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");
            if (Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const)
                ВидиміКолонки.Add("Серія_Номер", "Серія");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>
            {
                { "Номенклатура_Назва", "Номенклатура" },
                { "Пакування_Назва", "Пакування" },
                { "Приміщення_Назва", "Приміщення" },
                { "Склад_Назва", "Склад" },
                { "Комірка_Назва", "Комірка" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                КолонкиДаних.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");
            if (Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const)
                КолонкиДаних.Add("Серія_Номер", "Серія");

            Dictionary<string, string> ТипиДаних = new Dictionary<string, string>
            {
                { "Номенклатура_Назва", Номенклатура_Const.POINTER },
                { "ХарактеристикаНоменклатури_Назва", ХарактеристикиНоменклатури_Const.POINTER },
                { "Пакування_Назва", ПакуванняОдиниціВиміру_Const.POINTER },
                { "Серія_Номер", СеріїНоменклатури_Const.POINTER },
                { "Приміщення_Назва", СкладськіПриміщення_Const.POINTER },
                { "Склад_Назва", Склади_Const.POINTER },
                { "Комірка_Назва", СкладськіКомірки_Const.POINTER }
            };

            Dictionary<string, float> ПозиціяТекстуВКолонці = new Dictionary<string, float>
            {
                { "ВНаявності", 1 }
            };

            Dictionary<string, TreeCellDataFunc> ФункціяДляКолонки = new Dictionary<string, TreeCellDataFunc>
            {
                { "ВНаявності", ФункціїДляЗвітів.ФункціяДляКолонкиВідємнеЧислоЧервоним }
            };

            var recordResult = await Config.Kernel!.DataBase.SelectRequestAsync(query);

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
        ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Номенклатура} AS Номенклатура, 
        ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Пакування} AS Пакування,
        ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Комірка} AS Комірка,
        ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Серія} AS Серія,
        SUM(ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.ВНаявності}) AS ВНаявності
    FROM 
        {ТовариВКомірках_Залишки_TablePart.TABLE} AS ТовариВКомірках
    WHERE
        ТовариВКомірках.{ТовариВКомірках_Залишки_TablePart.Період} < @ПочатокПеріоду
    GROUP BY Номенклатура, ХарактеристикаНоменклатури, Пакування, Комірка, Серія
),
ЗалишкиТаОборотиЗаПеріод AS
(
    SELECT 
        ТовариВКомірках.{ТовариВКомірках_ЗалишкиТаОбороти_TablePart.Номенклатура} AS Номенклатура, 
        ТовариВКомірках.{ТовариВКомірках_ЗалишкиТаОбороти_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ТовариВКомірках.{ТовариВКомірках_ЗалишкиТаОбороти_TablePart.Пакування} AS Пакування,
        ТовариВКомірках.{ТовариВКомірках_ЗалишкиТаОбороти_TablePart.Комірка} AS Комірка,
        ТовариВКомірках.{ТовариВКомірках_ЗалишкиТаОбороти_TablePart.Серія} AS Серія,
        SUM(ТовариВКомірках.{ТовариВКомірках_ЗалишкиТаОбороти_TablePart.ВНаявностіПрихід}) AS ВНаявностіПрихід,
        SUM(ТовариВКомірках.{ТовариВКомірках_ЗалишкиТаОбороти_TablePart.ВНаявностіРозхід}) AS ВНаявностіРозхід,
        SUM(ТовариВКомірках.{ТовариВКомірках_ЗалишкиТаОбороти_TablePart.ВНаявностіЗалишок}) AS ВНаявностіЗалишок
    FROM 
        {ТовариВКомірках_ЗалишкиТаОбороти_TablePart.TABLE} AS ТовариВКомірках
    WHERE
        ТовариВКомірках.{ТовариВКомірках_ЗалишкиТаОбороти_TablePart.Період} >= @ПочатокПеріоду AND
        ТовариВКомірках.{ТовариВКомірках_ЗалишкиТаОбороти_TablePart.Період} <= @КінецьПеріоду
    GROUP BY Номенклатура, ХарактеристикаНоменклатури, Пакування, Комірка, Серія
),
КінцевийЗалишок AS
(
    SELECT 
        Номенклатура,
        ХарактеристикаНоменклатури,
        Пакування,
        Комірка,
        Серія,
        ВНаявності AS Залишок
    FROM ПочатковийЗалишок

    UNION ALL

    SELECT
        Номенклатура,
        ХарактеристикаНоменклатури,
        Пакування,
        Комірка,
        Серія,
        ВНаявностіЗалишок AS Залишок
    FROM ЗалишкиТаОборотиЗаПеріод
)
SELECT 
    Номенклатура,
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    Пакування,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS Пакування_Назва,

    /* склад, приміщення */
    Довідник_СкладськіПриміщення.{СкладськіПриміщення_Const.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Довідник_СкладськіКомірки.{СкладськіКомірки_Const.Приміщення} AS Приміщення,
    Довідник_СкладськіПриміщення.{СкладськіПриміщення_Const.Назва} AS Приміщення_Назва,

    Комірка,
    Довідник_СкладськіКомірки.{СкладськіКомірки_Const.Назва} AS Комірка_Назва,
    Серія,
    Довідник_СеріїНоменклатури.{СеріїНоменклатури_Const.Номер} AS Серія_Номер,
    ROUND(SUM(ПочатковийЗалишок), 2) AS ПочатковийЗалишок,
    ROUND(SUM(Прихід), 2) AS Прихід,
    ROUND(SUM(Розхід), 2) AS Розхід,
    ROUND(SUM(КінцевийЗалишок), 2) AS КінцевийЗалишок
FROM 
(
    SELECT 
        Номенклатура,
        ХарактеристикаНоменклатури,
        Пакування,
        Комірка,
        Серія,
        ВНаявності AS ПочатковийЗалишок,
        0 AS Прихід,
        0 AS Розхід,
        0 AS КінцевийЗалишок
    FROM ПочатковийЗалишок

    UNION ALL

    SELECT
        Номенклатура,
        ХарактеристикаНоменклатури,
        Пакування,
        Комірка,
        Серія,
        0 AS ПочатковийЗалишок,
        ВНаявностіПрихід AS Прихід,
        ВНаявностіРозхід AS Розхід,
        0 AS КінцевийЗалишок
    FROM ЗалишкиТаОборотиЗаПеріод

    UNION ALL

    SELECT 
        Номенклатура,
        ХарактеристикаНоменклатури,
        Пакування,
        Комірка,
        Серія,
        0 AS ПочатковийЗалишок,
        0 AS Прихід,
        0 AS Розхід,
        Залишок AS КінцевийЗалишок
    FROM КінцевийЗалишок
) AS ЗалишкиТаОбороти
LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = ЗалишкиТаОбороти.Номенклатура
LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = ЗалишкиТаОбороти.ХарактеристикаНоменклатури
LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON Довідник_ПакуванняОдиниціВиміру.uid = ЗалишкиТаОбороти.Пакування
LEFT JOIN {СкладськіКомірки_Const.TABLE} AS Довідник_СкладськіКомірки ON Довідник_СкладськіКомірки.uid = ЗалишкиТаОбороти.Комірка

/* склад, приміщення */
LEFT JOIN {СкладськіПриміщення_Const.TABLE} AS Довідник_СкладськіПриміщення ON Довідник_СкладськіПриміщення.uid = Довідник_СкладськіКомірки.{СкладськіКомірки_Const.Приміщення}
LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = Довідник_СкладськіПриміщення.{СкладськіПриміщення_Const.Склад}

LEFT JOIN {СеріїНоменклатури_Const.TABLE} AS Довідник_СеріїНоменклатури ON Довідник_СеріїНоменклатури.uid = ЗалишкиТаОбороти.Серія
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

            //Відбір по вибраному елементу СкладськеПриміщення
            if (!Фільтр.СкладськеПриміщення.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_СкладськіПриміщення.uid = '{Фільтр.СкладськеПриміщення.UnigueID}'
";
            }

            //Відбір по вибраному елементу СкладськаКомірка
            if (!Фільтр.СкладськаКомірка.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_СкладськіКомірки.uid = '{Фільтр.СкладськаКомірка.UnigueID}'
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
    Номенклатура, Номенклатура_Назва, 
    ХарактеристикаНоменклатури, ХарактеристикаНоменклатури_Назва, 
    Пакування, Пакування_Назва,
    Склад, Склад_Назва,
    Приміщення, Приміщення_Назва,
    Комірка, Комірка_Назва, 
    Серія, Серія_Номер

HAVING 
    SUM(Прихід) != 0 OR SUM(Розхід) != 0
ORDER BY 
    Номенклатура_Назва, ХарактеристикаНоменклатури_Назва
";

            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>
            {
                { "Номенклатура_Назва", "Номенклатура" },
                { "Пакування_Назва", "Пакування" },
                { "Склад_Назва", "Склад" },
                { "Приміщення_Назва", "Приміщення" },
                { "Комірка_Назва", "Комірка" },
                { "ПочатковийЗалишок", "На початок" },
                { "Прихід", "Прихід" },
                { "Розхід", "Розхід" },
                { "КінцевийЗалишок", "На кінець" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                ВидиміКолонки.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");
            if (Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const)
                ВидиміКолонки.Add("Серія_Номер", "Серія");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>
            {
                { "Номенклатура_Назва", "Номенклатура" },
                { "Пакування_Назва", "Пакування" },
                { "Склад_Назва", "Склад" },
                { "Приміщення_Назва", "Приміщення" },
                { "Комірка_Назва", "Комірка" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                КолонкиДаних.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");
            if (Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const)
                КолонкиДаних.Add("Серія_Номер", "Серія");

            Dictionary<string, string> ТипиДаних = new Dictionary<string, string>
            {
                { "Номенклатура_Назва", Номенклатура_Const.POINTER },
                { "ХарактеристикаНоменклатури_Назва", ХарактеристикиНоменклатури_Const.POINTER },
                { "Пакування_Назва", ПакуванняОдиниціВиміру_Const.POINTER },
                { "Склад_Назва", Склади_Const.POINTER },
                { "Приміщення_Назва", СкладськіПриміщення_Const.POINTER },
                { "Комірка_Назва", СкладськіКомірки_Const.POINTER }
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

            var recordResult = await Config.Kernel!.DataBase.SelectRequestAsync(query, paramQuery);

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
        ТовариВКомірках.period AS period,
        ТовариВКомірках.owner AS owner,
        ТовариВКомірках.income AS income,
        ТовариВКомірках.{ТовариВКомірках_Const.Номенклатура} AS Номенклатура,
        ТовариВКомірках.{ТовариВКомірках_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        ТовариВКомірках.{ТовариВКомірках_Const.Пакування} AS Пакування,
        ТовариВКомірках.{ТовариВКомірках_Const.Комірка} AS Комірка,
        ТовариВКомірках.{ТовариВКомірках_Const.Серія} AS Серія,
        ТовариВКомірках.{ТовариВКомірках_Const.ВНаявності} AS ВНаявності
    FROM
        {ТовариВКомірках_Const.TABLE} AS ТовариВКомірках
    WHERE
        (ТовариВКомірках.period >= @ПочатокПеріоду AND ТовариВКомірках.period <= @КінецьПеріоду)
";

            #region WHERE

            isExistParent = true;

            //Відбір по вибраному елементу Номенклатура
            if (!Фільтр.Номенклатура.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ТовариВКомірках.{ТовариВКомірках_Const.Номенклатура} = '{Фільтр.Номенклатура.UnigueID}'
";
            }

            //Відбір по вибраному елементу Характеристики Номенклатури
            if (!Фільтр.ХарактеристикиНоменклатури.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ТовариВКомірках.{ТовариВКомірках_Const.ХарактеристикаНоменклатури}= '{Фільтр.ХарактеристикиНоменклатури.UnigueID}'
";
            }

            //Відбір по вибраному елементу СкладськаКомірка
            if (!Фільтр.СкладськаКомірка.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ТовариВКомірках.{ТовариВКомірках_Const.Комірка}= '{Фільтр.СкладськаКомірка.UnigueID}'
";
            }

            //Відбір по вибраному елементу Серії Номенклатури
            if (!Фільтр.Серія.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
ТовариВКомірках.{ТовариВКомірках_Const.Серія} = '{Фільтр.Серія.UnigueID}'
";
            }

            #endregion

            query += $@"
),
documents AS
(";
            int counter = 0;
            foreach (string table in ТовариВКомірках_Const.AllowDocumentSpendTable)
            {
                string docType = ТовариВКомірках_Const.AllowDocumentSpendType[counter];

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
    register.Пакування,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS Пакування_Назва,

    /* склад, приміщення */
    Довідник_СкладськіПриміщення.{СкладськіПриміщення_Const.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Довідник_СкладськіКомірки.{СкладськіКомірки_Const.Приміщення} AS Приміщення,
    Довідник_СкладськіПриміщення.{СкладськіПриміщення_Const.Назва} AS Приміщення_Назва,

    register.Комірка,
    Довідник_СкладськіКомірки.{СкладськіКомірки_Const.Назва} AS Комірка_Назва,
    register.Серія,
    Довідник_СеріїНоменклатури.{СеріїНоменклатури_Const.Номер} AS Серія_Номер
FROM register INNER JOIN {table} ON {table}.uid = register.owner
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = register.Номенклатура
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = register.ХарактеристикаНоменклатури
    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON Довідник_ПакуванняОдиниціВиміру.uid = register.Пакування
    LEFT JOIN {СкладськіКомірки_Const.TABLE} AS Довідник_СкладськіКомірки ON Довідник_СкладськіКомірки.uid = register.Комірка

    /* склад, приміщення */
    LEFT JOIN {СкладськіПриміщення_Const.TABLE} AS Довідник_СкладськіПриміщення ON Довідник_СкладськіПриміщення.uid = Довідник_СкладськіКомірки.{СкладськіКомірки_Const.Приміщення}
    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = Довідник_СкладськіПриміщення.{СкладськіПриміщення_Const.Склад}

    LEFT JOIN {СеріїНоменклатури_Const.TABLE} AS Довідник_СеріїНоменклатури ON Довідник_СеріїНоменклатури.uid = register.Серія
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

                //Відбір по вибраному елементу Склади
                if (!Фільтр.Склад.IsEmpty())
                {
                    query += isExistParent ? "AND" : "WHERE";
                    isExistParent = true;

                    query += $@"
Довідник_Склади.uid = '{Фільтр.Склад.UnigueID}'
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

                //Відбір по вибраному елементу СкладськеПриміщення
                if (!Фільтр.СкладськеПриміщення.IsEmpty())
                {
                    query += isExistParent ? "AND" : "WHERE";
                    isExistParent = true;

                    query += $@"
Довідник_СкладськіПриміщення.uid = '{Фільтр.СкладськеПриміщення.UnigueID}'
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
    Пакування,
    Пакування_Назва,
    Склад,
    Склад_Назва,
    Приміщення,
    Приміщення_Назва,
    Комірка,
    Комірка_Назва,
    Серія,
    Серія_Номер,
    ВНаявності
FROM documents
ORDER BY period ASC
";

            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>
            {
                { "income", "Рух" },
                { "Документ", "Документ" },
                { "Номенклатура_Назва", "Номенклатура" },
                { "Пакування_Назва", "Пакування" },
                { "Склад_Назва", "Склад" },
                { "Приміщення_Назва", "Приміщення" },
                { "Комірка_Назва", "Комірка" },
                { "ВНаявності", "В наявності" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                ВидиміКолонки.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");
            if (Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const)
                ВидиміКолонки.Add("Серія_Номер", "Серія");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>
            {
                { "Документ", "uid_and_text" },
                { "Номенклатура_Назва", "Номенклатура" },
                { "Пакування_Назва", "Пакування" },
                { "Склад_Назва", "Склад" },
                { "Приміщення_Назва", "Приміщення" },
                { "Комірка_Назва", "Комірка" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                КолонкиДаних.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");
            if (Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const)
                КолонкиДаних.Add("Серія_Номер", "Серія");

            Dictionary<string, string> ТипиДаних = new Dictionary<string, string>
            {
                { "Документ", "Документи.*" },
                { "Номенклатура_Назва", Номенклатура_Const.POINTER },
                { "ХарактеристикаНоменклатури_Назва", ХарактеристикиНоменклатури_Const.POINTER },
                { "Серія_Номер", СеріїНоменклатури_Const.POINTER },
                { "Пакування_Назва", ПакуванняОдиниціВиміру_Const.POINTER },
                { "Склад_Назва", Склади_Const.POINTER },
                { "Приміщення_Назва", СкладськіПриміщення_Const.POINTER },
                { "Комірка_Назва", СкладськіКомірки_Const.POINTER }
            };

            Dictionary<string, float> ПозиціяТекстуВКолонці = new Dictionary<string, float>
            {
                { "income", 0.5f },
                { "ВНаявності", 1 }
            };

            Dictionary<string, TreeCellDataFunc> ФункціяДляКолонки = new Dictionary<string, TreeCellDataFunc>
            {
                { "ВНаявності", ФункціїДляЗвітів.ФункціяДляКолонкиВідємнеЧислоЧервоним }
            };

            Dictionary<string, object> paramQuery = new Dictionary<string, object>
            {
                { "ПочатокПеріоду", Фільтр.ДатаПочатокПеріоду },
                { "КінецьПеріоду", Фільтр.ДатаКінецьПеріоду }
            };

            var recordResult = await Config.Kernel!.DataBase.SelectRequestAsync(query, paramQuery);

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