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
using StorageAndTrade_1_0.Перелічення;
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
        ComboBoxText ТипНоменклатури = new ComboBoxText();
        Switch ТипНоменклатури_Включено = new Switch();
        ВидиНоменклатури_PointerControl ВидНоменклатури = new ВидиНоменклатури_PointerControl() { Caption = "Вид:" };

        struct ПараметриФільтр
        {
            public DateTime ДатаПочатокПеріоду;
            public DateTime ДатаКінецьПеріоду;
            public bool ГрупуватиПоПеріоду;
            public bool СобівартістьЗакупки;
            public Організації_Pointer Організація;
            public Контрагенти_Pointer Контрагент;
            public Контрагенти_Папки_Pointer Контрагент_Папка;
            public ДоговориКонтрагентів_Pointer ДоговірКонтрагента;
            public Номенклатура_Pointer Номенклатура;
            public Номенклатура_Папки_Pointer Номенклатура_Папка;
            public ХарактеристикиНоменклатури_Pointer ХарактеристикиНоменклатури;
            public Склади_Pointer Склад;
            public Склади_Папки_Pointer Склад_Папка;
            public ТипиНоменклатури ТипНоменклатури;
            public bool ТипНоменклатури_Включено;
            public ВидиНоменклатури_Pointer ВидНоменклатури;
        }

        #endregion

        public Звіт_Продажі() : base()
        {
            //Кнопки
            HBox hBoxTop = new HBox();
            PackStart(hBoxTop, false, false, 10);

            //2
            Button bOborot = new Button("Обороти");
            bOborot.Clicked += OnReport_Обороти;

            hBoxTop.PackStart(bOborot, false, false, 10);

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

            ХарактеристикиНоменклатури.BeforeClickOpenFunc = () =>
            {
                ХарактеристикиНоменклатури.НоменклатураВласник = Номенклатура.Pointer;
            };

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

            //ТипНоменклатури
            HBox hBoxTypNomenklatury = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxTypNomenklatury, false, false, 5);

            hBoxTypNomenklatury.PackStart(new Label("Тип:"), false, false, 5);
            hBoxTypNomenklatury.PackStart(ТипНоменклатури, false, false, 5);

            VBox hBoxTypNomenklaturyVkl = new VBox();
            hBoxTypNomenklatury.PackStart(hBoxTypNomenklaturyVkl, false, false, 5);
            hBoxTypNomenklaturyVkl.PackStart(ТипНоменклатури_Включено, false, false, 5);

            //ВидНоменклатури
            HBox hBoxVydNomenklatury = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxVydNomenklatury, false, false, 5);
            hBoxVydNomenklatury.PackStart(ВидНоменклатури, false, false, 5);

            //Заповнення
            foreach (var field in ПсевдонімиПерелічення.ТипиНоменклатури_List())
                ТипНоменклатури.Append(field.Value.ToString(), field.Name);

            ТипНоменклатури.Active = 0;
        }

        #endregion

        ПараметриФільтр СформуватиФільтр()
        {
            return new ПараметриФільтр()
            {
                ДатаПочатокПеріоду = ДатаПочатокПеріоду.ПочатокДня(),
                ДатаКінецьПеріоду = ДатаКінецьПеріоду.КінецьДня(),
                ГрупуватиПоПеріоду = ГрупуватиПоПеріоду.Active,
                СобівартістьЗакупки = СобівартістьЗакупки.Active,
                Організація = Організація.Pointer,
                Контрагент = Контрагент.Pointer,
                Контрагент_Папка = Контрагент_Папка.Pointer,
                ДоговірКонтрагента = ДоговірКонтрагента.Pointer,
                Номенклатура = Номенклатура.Pointer,
                Номенклатура_Папка = Номенклатура_Папка.Pointer,
                ХарактеристикиНоменклатури = ХарактеристикиНоменклатури.Pointer,
                Склад = Склад.Pointer,
                Склад_Папка = Склад_Папка.Pointer,
                ТипНоменклатури = Enum.Parse<ТипиНоменклатури>(ТипНоменклатури.ActiveId),
                ТипНоменклатури_Включено = ТипНоменклатури_Включено.Active,
                ВидНоменклатури = ВидНоменклатури.Pointer
            };
        }

        async ValueTask<HBox> ВідобразитиФільтр(string typeReport, ПараметриФільтр Фільтр)
        {
            HBox hBoxCaption = new HBox();

            string text = "";

            text += "З <b>" +
                Фільтр.ДатаПочатокПеріоду.ToString("dd.MM.yyyy") + "</b> по <b>" +
                Фільтр.ДатаКінецьПеріоду.ToString("dd.MM.yyyy") + "</b>; ";

            if (!Фільтр.Організація.IsEmpty())
                text += "Організація: <b>" + await Фільтр.Організація.GetPresentation() + "</b>; ";

            if (!Фільтр.Контрагент.IsEmpty())
                text += "Контрагент: <b>" + await Фільтр.Контрагент.GetPresentation() + "</b>; ";

            if (!Фільтр.ДоговірКонтрагента.IsEmpty())
                text += "Договір: <b>" + await Фільтр.ДоговірКонтрагента.GetPresentation() + "</b>; ";

            if (!Фільтр.Контрагент_Папка.IsEmpty())
                text += "Контрагент папка: <b>" + await Фільтр.Контрагент_Папка.GetPresentation() + "</b>; ";

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

            if (Фільтр.ТипНоменклатури_Включено)
                text += "Тип: <b>" + Фільтр.ТипНоменклатури.ToString() + "</b>; ";

            if (!Фільтр.ВидНоменклатури.IsEmpty())
                text += "Вид: <b>" + await Фільтр.ВидНоменклатури.GetPresentation() + "</b>; ";

            hBoxCaption.PackStart(new Label(text) { Wrap = true, UseMarkup = true }, false, false, 2);

            return hBoxCaption;
        }

        void OnReport_Обороти(object? sender, EventArgs args)
        {
            Обороти(СформуватиФільтр());
        }

        async void Обороти(object? Параметри, bool refreshPage = false)
        {
            ПараметриФільтр Фільтр = Параметри != null ? (ПараметриФільтр)Параметри : new ПараметриФільтр();

            #region SELECT

            bool isExistParent = true;

            string query = $@"
SELECT 
" + (Фільтр.ГрупуватиПоПеріоду ? $@"
    Продажі.{Продажі_Обороти_TablePart.Період} AS Період,
    TO_CHAR(Продажі.{Продажі_Обороти_TablePart.Період}, 'dd.mm.yyyy') AS Період_Назва," : "") +
$@" 
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
    (Фільтр.СобівартістьЗакупки ? $", Продажі.{Продажі_Обороти_TablePart.Собівартість} AS Собівартість" : "") + $@",
    Довідник_ПакуванняОдиниціВиміру.uid AS ОдиницяВиміру,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS ОдиницяВиміру_Назва
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
    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON Довідник_ПакуванняОдиниціВиміру.uid = 
        Довідник_Номенклатура.{Номенклатура_Const.ОдиницяВиміру}
WHERE
    Продажі.{Продажі_Обороти_TablePart.Період} >= @ПочатокПеріоду AND
    Продажі.{Продажі_Обороти_TablePart.Період} <= @КінецьПеріоду" +
    (Фільтр.ТипНоменклатури_Включено ? $@" AND Довідник_Номенклатура.{Номенклатура_Const.ТипНоменклатури} = @ТипНоменклатури" : "") + $@"
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

            //Відбір по всіх вкладених папках вибраної папки Контрагент
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

            //Відбір по вибраному елементу Контрагент
            if (!Фільтр.Контрагент.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Контрагенти.uid = '{Фільтр.Контрагент.UnigueID}'
";
            }

            //Відбір по вибраному елементу ДоговірКонтрагента
            if (!Фільтр.ДоговірКонтрагента.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_ДоговориКонтрагентів.uid = '{Фільтр.ДоговірКонтрагента.UnigueID}'
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

            //Відбір по вибраному елементу ВидиНоменклатури
            if (!Фільтр.ВидНоменклатури.IsEmpty())
            {
                query += isExistParent ? "AND" : "WHERE";
                isExistParent = true;

                query += $@"
Довідник_Номенклатура.{Номенклатура_Const.ВидНоменклатури} = '{Фільтр.ВидНоменклатури.UnigueID}'
";
            }

            #endregion

            query += @"
GROUP BY " +
    (Фільтр.ГрупуватиПоПеріоду ? "Період, Період_Назва, " : "") + @"
    Організація, Організація_Назва,
    Склад, Склад_Назва,
    Контрагент, Контрагент_Назва,
    Договір, Договір_Назва,
    Номенклатура, Номенклатура_Назва,
    ХарактеристикаНоменклатури, ХарактеристикаНоменклатури_Назва" +
    (Фільтр.СобівартістьЗакупки ? ", Собівартість" : "") + @$",
    ОдиницяВиміру, ОдиницяВиміру_Назва

HAVING
    SUM(Продажі.{Продажі_Обороти_TablePart.Кількість}) != 0 OR
    SUM(Продажі.{Продажі_Обороти_TablePart.Сума}) != 0 OR
    SUM(Продажі.{Продажі_Обороти_TablePart.Дохід}) != 0 

ORDER BY " +
    (Фільтр.ГрупуватиПоПеріоду ? "Період," : "") + @"
    Організація_Назва, 
    Склад_Назва, 
    Контрагент_Назва, 
    Договір_Назва, 
    Номенклатура_Назва, 
    ХарактеристикаНоменклатури_Назва
";
            #endregion

            Dictionary<string, string> ВидиміКолонки = new Dictionary<string, string>
            {
                { "Період_Назва", "Період" },
                { "Організація_Назва", "Організація" },
                { "Склад_Назва", "Склад" },
                { "Контрагент_Назва", "Контрагент" },
                { "Договір_Назва", "Договір" },
                { "Номенклатура_Назва", "Номенклатура" },
                { "Кількість", "Кількість" },
                { "Сума", "Сума" },
                { "Дохід", "Дохід" },
                { "Собівартість", "Собівартість" },
                { "ОдиницяВиміру_Назва", "Пакування" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                ВидиміКолонки.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");

            Dictionary<string, string> КолонкиДаних = new Dictionary<string, string>
            {
                { "Організація_Назва", "Організація" },
                { "Склад_Назва", "Склад" },
                { "Контрагент_Назва", "Контрагент" },
                { "Договір_Назва", "Договір" },
                { "Номенклатура_Назва", "Номенклатура" },
                { "ОдиницяВиміру_Назва", "ОдиницяВиміру" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                КолонкиДаних.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");

            Dictionary<string, string> ТипиДаних = new Dictionary<string, string>
            {
                { "Організація_Назва", Організації_Const.POINTER },
                { "Склад_Назва", Склади_Const.POINTER },
                { "Контрагент_Назва", Контрагенти_Const.POINTER },
                { "Договір_Назва", ДоговориКонтрагентів_Const.POINTER },
                { "Номенклатура_Назва", Номенклатура_Const.POINTER },
                { "ХарактеристикаНоменклатури_Назва", ХарактеристикиНоменклатури_Const.POINTER },
                { "ОдиницяВиміру_Назва", ПакуванняОдиниціВиміру_Const.POINTER }
            };

            Dictionary<string, float> ПозиціяТекстуВКолонці = new Dictionary<string, float>
            {
                { "Кількість", 1 },
                { "Сума", 1 },
                { "Дохід", 1 },
                { "Собівартість", 1 }
            };

            Dictionary<string, TreeCellDataFunc> ФункціяДляКолонки = new Dictionary<string, TreeCellDataFunc>
            {
                { "Кількість", ФункціїДляЗвітів.ФункціяДляКолонкиВідємнеЧислоЧервоним },
                { "Сума", ФункціїДляЗвітів.ФункціяДляКолонкиВідємнеЧислоЧервоним },
                { "Дохід", ФункціїДляЗвітів.ФункціяДляКолонкиВідємнеЧислоЧервоним },
                { "Собівартість", ФункціїДляЗвітів.ФункціяДляКолонкиВідємнеЧислоЧервоним }
            };

            Dictionary<string, object> paramQuery = new Dictionary<string, object>
            {
                { "ПочатокПеріоду", Фільтр.ДатаПочатокПеріоду },
                { "КінецьПеріоду", Фільтр.ДатаКінецьПеріоду },
                { "ТипНоменклатури", (int)Фільтр.ТипНоменклатури }
            };

            var recordResult = await Config.Kernel.DataBase.SelectRequestAsync(query, paramQuery);

            ListStore listStore;
            ФункціїДляЗвітів.СтворитиМодельДаних(out listStore, recordResult.ColumnsName);

            TreeView treeView = new TreeView(listStore);
            treeView.ButtonPressEvent += ФункціїДляЗвітів.OpenPageDirectoryOrDocument;

            ФункціїДляЗвітів.СтворитиКолонкиДляДерева(treeView, recordResult.ColumnsName, ВидиміКолонки, КолонкиДаних, ТипиДаних, ПозиціяТекстуВКолонці, ФункціяДляКолонки);
            ФункціїДляЗвітів.ЗаповнитиМодельДаними(listStore, recordResult.ColumnsName, recordResult.ListRow);

            ФункціїДляЗвітів.CreateReportNotebookPage(reportNotebook, "Обороти", await ВідобразитиФільтр("Обороти", Фільтр), treeView, Обороти, Фільтр, refreshPage);
        }

    }
}