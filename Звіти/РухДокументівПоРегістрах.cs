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

using Константи = StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade
{
    class РухДокументівПоРегістрах
    {
        #region ТовариНаСкладах

        public static Dictionary<string, string> ТовариНаСкладах_ВидиміКолонки()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "income", "Рух" },
                { "period", "Період" },
                { "Номенклатура_Назва", "Номенклатура" },
                { "Склад_Назва", "Склад" },
                { "ВНаявності", "В наявності" },
                { "ДоВідвантаження", "До відвантаження" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                columns.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");
            if (Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const)
                columns.Add("Серія_Номер", "Серія");

            return columns;
        }

        public static Dictionary<string, string> ТовариНаСкладах_КолонкиДаних()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "Номенклатура_Назва", "Номенклатура" },
                { "Склад_Назва", "Склад" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                columns.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");
            if (Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const)
                columns.Add("Серія_Номер", "Серія");

            return columns;
        }

        public static Dictionary<string, string> ТовариНаСкладах_ТипиДаних()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "Номенклатура_Назва", Номенклатура_Const.POINTER },
                { "ХарактеристикаНоменклатури_Назва", ХарактеристикиНоменклатури_Const.POINTER },
                { "Серія_Номер", СеріїНоменклатури_Const.POINTER },
                { "Склад_Назва", Склади_Const.POINTER }
            };

            return columns;
        }

        public static Dictionary<string, float> ТовариНаСкладах_ПозиціяТекстуВКолонці()
        {
            Dictionary<string, float> columns = new Dictionary<string, float>
            {
                { "income", 0.5f },
                { "ВНаявності", 1 },
                { "ДоВідвантаження", 1 }
            };

            return columns;
        }

        public static Dictionary<string, TreeCellDataFunc> ТовариНаСкладах_ФункціяДляКолонки()
        {
            Dictionary<string, TreeCellDataFunc> columns = new Dictionary<string, TreeCellDataFunc>
            {
                { "ВНаявності", ФункціїДляЗвітів.ФункціяДляКолонкиБазоваДляЧисла },
                { "ДоВідвантаження", ФункціїДляЗвітів.ФункціяДляКолонкиБазоваДляЧисла }
            };

            return columns;
        }

        public static string ТовариНаСкладах_Запит = $@"
SELECT 
    (CASE WHEN Рег_ТовариНаСкладах.income = true THEN '+' ELSE '-' END) AS income,
    Рег_ТовариНаСкладах.period, 
    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва, 
    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Серія} AS Серія,
    Довідник_СеріїНоменклатури.{СеріїНоменклатури_Const.Номер} AS Серія_Номер,
    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} AS ВНаявності,
    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ДоВідвантаження} AS ДоВідвантаження
FROM 
    {ТовариНаСкладах_Const.TABLE} AS Рег_ТовариНаСкладах

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Номенклатура}

    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ХарактеристикаНоменклатури}

    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
       Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Склад}

    LEFT JOIN {СеріїНоменклатури_Const.TABLE} AS Довідник_СеріїНоменклатури ON Довідник_СеріїНоменклатури.uid = 
       Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Серія}
WHERE
    Рег_ТовариНаСкладах.Owner = @ДокументВказівник
ORDER BY Номенклатура_Назва
";

        #endregion

        #region ПартіїТоварів

        public static Dictionary<string, string> ПартіїТоварів_ВидиміКолонки()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "income", "Рух" },
                { "period", "Період" },
                { "Організація_Назва", "Організація" },
                { "ПартіяТоварівКомпозит_Назва", "Партія" },
                { "Номенклатура_Назва", "Номенклатура" },
                { "Склад_Назва", "Склад" },
                { "Рядок", "Рядок" },
                { "Кількість", "Кількість" },
                { "Собівартість", "Собівартість" },
                { "СписанаСобівартість", "Списана cобівартість" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                columns.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");
            if (Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const)
                columns.Add("Серія_Номер", "Серія");

            return columns;
        }

        public static Dictionary<string, string> ПартіїТоварів_КолонкиДаних()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "Організація_Назва", "Організація" },
                { "ПартіяТоварівКомпозит_Назва", "ПартіяТоварівКомпозит" },
                { "Номенклатура_Назва", "Номенклатура" },
                { "Склад_Назва", "Склад" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                columns.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");
            if (Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const)
                columns.Add("Серія_Номер", "Серія");

            return columns;
        }

        public static Dictionary<string, string> ПартіїТоварів_ТипиДаних()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "Організація_Назва", Організації_Const.POINTER },
                { "ПартіяТоварівКомпозит_Назва", ПартіяТоварівКомпозит_Const.POINTER },
                { "Номенклатура_Назва", Номенклатура_Const.POINTER },
                { "ХарактеристикаНоменклатури_Назва", ХарактеристикиНоменклатури_Const.POINTER },
                { "Серія_Номер", СеріїНоменклатури_Const.POINTER },
                { "Склад_Назва", Склади_Const.POINTER }
            };

            return columns;
        }

        public static Dictionary<string, float> ПартіїТоварів_ПозиціяТекстуВКолонці()
        {
            Dictionary<string, float> columns = new Dictionary<string, float>
            {
                { "income", 0.5f },
                { "Рядок", 0.1f },
                { "Кількість", 1 },
                { "Собівартість", 1 },
                { "СписанаСобівартість", 1 }
            };

            return columns;
        }

        public static Dictionary<string, TreeCellDataFunc> ПартіїТоварів_ФункціяДляКолонки()
        {
            Dictionary<string, TreeCellDataFunc> columns = new Dictionary<string, TreeCellDataFunc>
            {
                { "Кількість", ФункціїДляЗвітів.ФункціяДляКолонкиБазоваДляЧисла },
                { "Собівартість", ФункціїДляЗвітів.ФункціяДляКолонкиБазоваДляЧисла },
                { "СписанаСобівартість", ФункціїДляЗвітів.ФункціяДляКолонкиБазоваДляЧисла }
            };

            return columns;
        }

        public static string ПартіїТоварів_Запит = $@"
SELECT 
    (CASE WHEN Рег_ПартіїТоварів.income = true THEN '+' ELSE '-' END) AS income, 
    Рег_ПартіїТоварів.period,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Організація} AS Організація, 
    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.ПартіяТоварівКомпозит} AS ПартіяТоварівКомпозит,
    Довідник_ПартіяТоварівКомпозит.{ПартіяТоварівКомпозит_Const.Назва} AS ПартіяТоварівКомпозит_Назва, 
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва, 
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Серія} AS Серія,
    Довідник_СеріїНоменклатури.{СеріїНоменклатури_Const.Номер} AS Серія_Номер,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Рядок} AS Рядок,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Кількість} AS Кількість,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Собівартість} AS Собівартість,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.СписанаСобівартість} AS СписанаСобівартість
FROM 
    {ПартіїТоварів_Const.TABLE} AS Рег_ПартіїТоварів

    LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = 
        Рег_ПартіїТоварів.{ПартіїТоварів_Const.Організація}

    LEFT JOIN {ПартіяТоварівКомпозит_Const.TABLE} AS Довідник_ПартіяТоварівКомпозит ON Довідник_ПартіяТоварівКомпозит.uid = 
        Рег_ПартіїТоварів.{ПартіїТоварів_Const.ПартіяТоварівКомпозит}

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        Рег_ПартіїТоварів.{ПартіїТоварів_Const.Номенклатура}

    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        Рег_ПартіїТоварів.{ПартіїТоварів_Const.ХарактеристикаНоменклатури}

    LEFT JOIN {СеріїНоменклатури_Const.TABLE} AS Довідник_СеріїНоменклатури ON Довідник_СеріїНоменклатури.uid = 
       Рег_ПартіїТоварів.{ПартіїТоварів_Const.Серія}

    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
       Рег_ПартіїТоварів.{ПартіїТоварів_Const.Склад}
WHERE
    Рег_ПартіїТоварів.Owner = @ДокументВказівник
ORDER BY Організація_Назва
";

        #endregion

        #region РухТоварів
        /*
                public static Dictionary<string, string> РухТоварів_ВидиміКолонки()
                {
                    Dictionary<string, string> columns = new Dictionary<string, string>();

                    columns.Add("income", "Рух");
                    columns.Add("period", "Період");
                    columns.Add("Номенклатура_Назва", "Номенклатура");
                    if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const) columns.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");
                    columns.Add("Склад_Назва", "Склад");
                    columns.Add("Кількість", "Кількість");

                    return columns;
                }

                public static Dictionary<string, string> РухТоварів_КолонкиДаних()
                {
                    Dictionary<string, string> columns = new Dictionary<string, string>();

                    columns.Add("Номенклатура_Назва", "Номенклатура");
                    if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const) columns.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");
                    columns.Add("Склад_Назва", "Склад");

                    return columns;
                }

                public static Dictionary<string, float> РухТоварів_ПозиціяТекстуВКолонці()
                {
                    Dictionary<string, float> columns = new Dictionary<string, float>();

                    columns.Add("income", 0.5f);
                    columns.Add("Кількість", 1);

                    return columns;
                }

                public static string РухТоварів_Запит = $@"
        SELECT 
            (CASE WHEN Рег_РухТоварів.income = true THEN '+' ELSE '-' END) AS income,
            Рег_РухТоварів.period, 
            Рег_РухТоварів.{РухТоварів_Const.Номенклатура} AS Номенклатура, 
            Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
            Рег_РухТоварів.{РухТоварів_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
            Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва, 
            Рег_РухТоварів.{РухТоварів_Const.Склад} AS Склад,
            Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
            Рег_РухТоварів.{РухТоварів_Const.Кількість} AS Кількість
        FROM 
            {РухТоварів_Const.TABLE} AS Рег_РухТоварів

            LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
                Рег_РухТоварів.{РухТоварів_Const.Номенклатура}

            LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
                Рег_РухТоварів.{РухТоварів_Const.ХарактеристикаНоменклатури}

            LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
               Рег_РухТоварів.{РухТоварів_Const.Склад}
        WHERE
            Рег_РухТоварів.Owner = @ДокументВказівник
        ORDER BY Номенклатура_Назва
        ";

        */
        #endregion

        #region ЗамовленняКлієнтів

        public static Dictionary<string, string> ЗамовленняКлієнтів_ВидиміКолонки()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "income", "Рух" },
                { "period", "Період" },
                { "ЗамовленняКлієнта_Назва", "Замовлення" },
                { "Номенклатура_Назва", "Номенклатура" },
                { "Склад_Назва", "Склад" },
                { "Замовлено", "Замовлено" },
                { "Сума", "Сума" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                columns.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");

            return columns;
        }

        public static Dictionary<string, string> ЗамовленняКлієнтів_КолонкиДаних()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "ЗамовленняКлієнта_Назва", "ЗамовленняКлієнта" },
                { "Номенклатура_Назва", "Номенклатура" },
                { "Склад_Назва", "Склад" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const) columns.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");


            return columns;
        }

        public static Dictionary<string, string> ЗамовленняКлієнтів_ТипиДаних()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "ЗамовленняКлієнта_Назва", ЗамовленняКлієнта_Const.POINTER },
                { "Номенклатура_Назва", Номенклатура_Const.POINTER },
                { "ХарактеристикаНоменклатури_Назва", ХарактеристикиНоменклатури_Const.POINTER },
                { "Склад_Назва", Склади_Const.POINTER }
            };

            return columns;
        }

        public static Dictionary<string, float> ЗамовленняКлієнтів_ПозиціяТекстуВКолонці()
        {
            Dictionary<string, float> columns = new Dictionary<string, float>
            {
                { "income", 0.5f },
                { "Замовлено", 1 },
                { "Сума", 1 }
            };

            return columns;
        }

        public static Dictionary<string, TreeCellDataFunc> ЗамовленняКлієнтів_ФункціяДляКолонки()
        {
            Dictionary<string, TreeCellDataFunc> columns = new Dictionary<string, TreeCellDataFunc>
            {
                { "Замовлено", ФункціїДляЗвітів.ФункціяДляКолонкиБазоваДляЧисла },
                { "Сума", ФункціїДляЗвітів.ФункціяДляКолонкиБазоваДляЧисла }
            };

            return columns;
        }

        public static string ЗамовленняКлієнтів_Запит = $@"
SELECT 
    (CASE WHEN Рег_ЗамовленняКлієнтів.income = true THEN '+' ELSE '-' END) AS income,
    Рег_ЗамовленняКлієнтів.period,
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.ЗамовленняКлієнта} AS ЗамовленняКлієнта, 
    Документ_ЗамовленняКлієнта.{ЗамовленняКлієнта_Const.Назва} AS ЗамовленняКлієнта_Назва, 
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва, 
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Замовлено} AS Замовлено,
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Сума} AS Сума
FROM 
    {ЗамовленняКлієнтів_Const.TABLE} AS Рег_ЗамовленняКлієнтів

    LEFT JOIN {ЗамовленняКлієнта_Const.TABLE} AS Документ_ЗамовленняКлієнта ON Документ_ЗамовленняКлієнта.uid = 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.ЗамовленняКлієнта}

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Номенклатура}

    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.ХарактеристикаНоменклатури}

    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
       Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Склад}
WHERE
    Рег_ЗамовленняКлієнтів.Owner = @ДокументВказівник
ORDER BY Номенклатура_Назва
";

        #endregion

        #region РозрахункиЗКлієнтами

        public static Dictionary<string, string> РозрахункиЗКлієнтами_ВидиміКолонки()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "income", "Рух" },
                { "period", "Період" },
                { "Контрагент_Назва", "Контрагент" },
                { "Валюта_Назва", "Валюта" },
                { "Сума", "Сума" }
            };

            return columns;
        }

        public static Dictionary<string, string> РозрахункиЗКлієнтами_КолонкиДаних()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "Контрагент_Назва", "Контрагент" },
                { "Валюта_Назва", "Валюта" }
            };

            return columns;
        }

        public static Dictionary<string, string> РозрахункиЗКлієнтами_ТипиДаних()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "Контрагент_Назва", Контрагенти_Const.POINTER },
                { "Валюта_Назва", Валюти_Const.POINTER }
            };

            return columns;
        }

        public static Dictionary<string, float> РозрахункиЗКлієнтами_ПозиціяТекстуВКолонці()
        {
            Dictionary<string, float> columns = new Dictionary<string, float>
            {
                { "income", 0.5f },
                { "Сума", 1 }
            };

            return columns;
        }

        public static Dictionary<string, TreeCellDataFunc> РозрахункиЗКлієнтами_ФункціяДляКолонки()
        {
            Dictionary<string, TreeCellDataFunc> columns = new Dictionary<string, TreeCellDataFunc>
            {
                { "Сума", ФункціїДляЗвітів.ФункціяДляКолонкиБазоваДляЧисла }
            };

            return columns;
        }

        public static string РозрахункиЗКлієнтами_Запит = $@"
SELECT 
    (CASE WHEN Рег_РозрахункиЗКлієнтами.income = true THEN '+' ELSE '-' END) AS income,
    Рег_РозрахункиЗКлієнтами.period, 
    Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Контрагент} AS Контрагент, 
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS Контрагент_Назва,
    Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Валюта} AS Валюта,
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} AS Сума
FROM 
    {РозрахункиЗКлієнтами_Const.TABLE} AS Рег_РозрахункиЗКлієнтами

    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Контрагент}

    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = 
       Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Валюта}
WHERE
    Рег_РозрахункиЗКлієнтами.Owner = @ДокументВказівник
";

        #endregion

        #region ВільніЗалишки

        public static Dictionary<string, string> ВільніЗалишки_ВидиміКолонки()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "income", "Рух" },
                { "period", "Період" },
                { "Номенклатура_Назва", "Номенклатура" },
                { "Склад_Назва", "Склад" },
                { "ВНаявності", "В наявності" },
                { "ВРезервіЗіСкладу", "В резерві зі складу" },
                { "ВРезервіПідЗамовлення", "В резерві під замовлення" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                columns.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");

            return columns;
        }

        public static Dictionary<string, string> ВільніЗалишки_КолонкиДаних()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "Номенклатура_Назва", "Номенклатура" },
                { "Склад_Назва", "Склад" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                columns.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");

            return columns;
        }

        public static Dictionary<string, string> ВільніЗалишки_ТипиДаних()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "Номенклатура_Назва", Номенклатура_Const.POINTER },
                { "ХарактеристикаНоменклатури_Назва", ХарактеристикиНоменклатури_Const.POINTER },
                { "Склад_Назва", Склади_Const.POINTER }
            };

            return columns;
        }

        public static Dictionary<string, float> ВільніЗалишки_ПозиціяТекстуВКолонці()
        {
            Dictionary<string, float> columns = new Dictionary<string, float>
            {
                { "income", 0.5f },
                { "ВНаявності", 1 },
                { "ВРезервіЗіСкладу", 1 },
                { "ВРезервіПідЗамовлення", 1 }
            };

            return columns;
        }

        public static Dictionary<string, TreeCellDataFunc> ВільніЗалишки_ФункціяДляКолонки()
        {
            Dictionary<string, TreeCellDataFunc> columns = new Dictionary<string, TreeCellDataFunc>
            {
                { "ВНаявності", ФункціїДляЗвітів.ФункціяДляКолонкиБазоваДляЧисла },
                { "ВРезервіЗіСкладу", ФункціїДляЗвітів.ФункціяДляКолонкиБазоваДляЧисла },
                { "ВРезервіПідЗамовлення", ФункціїДляЗвітів.ФункціяДляКолонкиБазоваДляЧисла }
            };

            return columns;
        }

        public static string ВільніЗалишки_Запит = $@"
SELECT 
    (CASE WHEN Рег_ВільніЗалишки.income = true THEN '+' ELSE '-' END) AS income, 
    Рег_ВільніЗалишки.period,
    Рег_ВільніЗалишки.{ВільніЗалишки_Const.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    Рег_ВільніЗалишки.{ВільніЗалишки_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва, 
    Рег_ВільніЗалишки.{ВільніЗалишки_Const.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВНаявності} AS ВНаявності,
    Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіЗіСкладу} AS ВРезервіЗіСкладу,
    Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіПідЗамовлення} AS ВРезервіПідЗамовлення
FROM 
    {ВільніЗалишки_Const.TABLE} AS Рег_ВільніЗалишки

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        Рег_ВільніЗалишки.{ВільніЗалишки_Const.Номенклатура}

    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ХарактеристикаНоменклатури}

    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
       Рег_ВільніЗалишки.{ВільніЗалишки_Const.Склад}
WHERE
    Рег_ВільніЗалишки.Owner = @ДокументВказівник
ORDER BY Номенклатура_Назва
";

        #endregion

        #region ЗамовленняПостачальникам

        public static Dictionary<string, string> ЗамовленняПостачальникам_ВидиміКолонки()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "income", "Рух" },
                { "period", "Період" },
                { "ЗамовленняПостачальнику_Назва", "Замовлення" },
                { "Номенклатура_Назва", "Номенклатура" },
                { "Склад_Назва", "Склад" },
                { "Замовлено", "Замовлено" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                columns.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");

            return columns;
        }

        public static Dictionary<string, string> ЗамовленняПостачальникам_КолонкиДаних()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "ЗамовленняПостачальнику_Назва", "ЗамовленняПостачальнику" },
                { "Номенклатура_Назва", "Номенклатура" },
                { "Склад_Назва", "Склад" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                columns.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");

            return columns;
        }

        public static Dictionary<string, string> ЗамовленняПостачальникам_ТипиДаних()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "ЗамовленняПостачальнику_Назва", ЗамовленняПостачальнику_Const.POINTER },
                { "Номенклатура_Назва", Номенклатура_Const.POINTER },
                { "ХарактеристикаНоменклатури_Назва", ХарактеристикиНоменклатури_Const.POINTER },
                { "Склад_Назва", Склади_Const.POINTER }
            };

            return columns;
        }

        public static Dictionary<string, float> ЗамовленняПостачальникам_ПозиціяТекстуВКолонці()
        {
            Dictionary<string, float> columns = new Dictionary<string, float>
            {
                { "income", 0.5f },
                { "Замовлено", 1 }
            };

            return columns;
        }

        public static Dictionary<string, TreeCellDataFunc> ЗамовленняПостачальникам_ФункціяДляКолонки()
        {
            Dictionary<string, TreeCellDataFunc> columns = new Dictionary<string, TreeCellDataFunc>
            {
                { "Замовлено", ФункціїДляЗвітів.ФункціяДляКолонкиБазоваДляЧисла }
            };

            return columns;
        }

        public static string ЗамовленняПостачальникам_Запит = $@"
SELECT 
    (CASE WHEN Рег_ЗамовленняПостачальникам.income = true THEN '+' ELSE '-' END) AS income, 
    Рег_ЗамовленняПостачальникам.period,
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.ЗамовленняПостачальнику} AS ЗамовленняПостачальнику, 
    Документ_ЗамовленняПостачальнику.{ЗамовленняПостачальнику_Const.Назва} AS ЗамовленняПостачальнику_Назва,
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва, 
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} AS Замовлено
FROM 
    {ЗамовленняПостачальникам_Const.TABLE} AS Рег_ЗамовленняПостачальникам

    LEFT JOIN {ЗамовленняПостачальнику_Const.TABLE} AS Документ_ЗамовленняПостачальнику ON Документ_ЗамовленняПостачальнику.uid = 
        Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.ЗамовленняПостачальнику}

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Номенклатура}

    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.ХарактеристикаНоменклатури}

    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
       Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Склад}
WHERE
    Рег_ЗамовленняПостачальникам.Owner = @ДокументВказівник
ORDER BY Номенклатура_Назва
";

        #endregion

        #region РозрахункиЗПостачальниками

        public static Dictionary<string, string> РозрахункиЗПостачальниками_ВидиміКолонки()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "income", "Рух" },
                { "period", "Період" },
                { "Контрагент_Назва", "Контрагент" },
                { "Валюта_Назва", "Валюта" },
                { "Сума", "Сума" }
            };

            return columns;
        }

        public static Dictionary<string, string> РозрахункиЗПостачальниками_КолонкиДаних()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "Контрагент_Назва", "Контрагент" },
                { "Валюта_Назва", "Валюта" }
            };

            return columns;
        }

        public static Dictionary<string, string> РозрахункиЗПостачальниками_ТипиДаних()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "Контрагент_Назва", Контрагенти_Const.POINTER },
                { "Валюта_Назва", Валюти_Const.POINTER }
            };

            return columns;
        }

        public static Dictionary<string, float> РозрахункиЗПостачальниками_ПозиціяТекстуВКолонці()
        {
            Dictionary<string, float> columns = new Dictionary<string, float>
            {
                { "income", 0.5f },
                { "Сума", 1 }
            };

            return columns;
        }

        public static Dictionary<string, TreeCellDataFunc> РозрахункиЗПостачальниками_ФункціяДляКолонки()
        {
            Dictionary<string, TreeCellDataFunc> columns = new Dictionary<string, TreeCellDataFunc>
            {
                { "Сума", ФункціїДляЗвітів.ФункціяДляКолонкиБазоваДляЧисла }
            };

            return columns;
        }

        public static string РозрахункиЗПостачальниками_Запит = $@"
SELECT 
    (CASE WHEN Рег_РозрахункиЗПостачальниками.income = true THEN '+' ELSE '-' END) AS income, 
    Рег_РозрахункиЗПостачальниками.period,
    Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Контрагент} AS Контрагент, 
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS Контрагент_Назва,
    Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Валюта} AS Валюта,
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} AS Сума
FROM 
    {РозрахункиЗПостачальниками_Const.TABLE} AS Рег_РозрахункиЗПостачальниками

    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Контрагент}

    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = 
       Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Валюта}
WHERE
    Рег_РозрахункиЗПостачальниками.Owner = @ДокументВказівник
";

        #endregion

        #region ТовариДоПоступлення
        /*
                public static Dictionary<string, string> ТовариДоПоступлення_ВидиміКолонки()
                {
                    Dictionary<string, string> columns = new Dictionary<string, string>();

                    columns.Add("income", "Рух");
                    columns.Add("period", "Період");
                    columns.Add("Номенклатура_Назва", "Номенклатура");
                    if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const) columns.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");
                    columns.Add("Склад_Назва", "Склад");
                    columns.Add("ВНаявності", "В наявності");
                    columns.Add("ДоПоступлення", "До поступлення");

                    return columns;
                }

                public static Dictionary<string, string> ТовариДоПоступлення_КолонкиДаних()
                {
                    Dictionary<string, string> columns = new Dictionary<string, string>();

                    columns.Add("Номенклатура_Назва", "Номенклатура");
                    if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const) columns.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");
                    columns.Add("Склад_Назва", "Склад");

                    return columns;
                }

                public static Dictionary<string, float> ТовариДоПоступлення_ПозиціяТекстуВКолонці()
                {
                    Dictionary<string, float> columns = new Dictionary<string, float>();

                    columns.Add("income", 0.5f);
                    columns.Add("ВНаявності", 1);
                    columns.Add("ДоПоступлення", 1);

                    return columns;
                }

                public static string ТовариДоПоступлення_Запит = $@"
        SELECT 
            (CASE WHEN Рег_ТовариДоПоступлення.income = true THEN '+' ELSE '-' END) AS income,
            Рег_ТовариДоПоступлення.period,
            Рег_ТовариДоПоступлення.{ТовариДоПоступлення_Const.Номенклатура} AS Номенклатура, 
            Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
            Рег_ТовариДоПоступлення.{ТовариДоПоступлення_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
            Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва, 
            Рег_ТовариДоПоступлення.{ТовариДоПоступлення_Const.Склад} AS Склад,
            Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
            Рег_ТовариДоПоступлення.{ТовариДоПоступлення_Const.ДоПоступлення} AS ДоПоступлення
        FROM 
            {ТовариДоПоступлення_Const.TABLE} AS Рег_ТовариДоПоступлення

            LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
                Рег_ТовариДоПоступлення.{ТовариДоПоступлення_Const.Номенклатура}

            LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
                Рег_ТовариДоПоступлення.{ТовариДоПоступлення_Const.ХарактеристикаНоменклатури}

            LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
               Рег_ТовариДоПоступлення.{ТовариДоПоступлення_Const.Склад}
        WHERE
            Рег_ТовариДоПоступлення.Owner = @ДокументВказівник
        ORDER BY Номенклатура_Назва
        ";

        */
        #endregion

        #region РухКоштів

        public static Dictionary<string, string> РухКоштів_ВидиміКолонки()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "income", "Рух" },
                { "period", "Період" },
                { "Організація_Назва", "Організація" },
                { "Каса_Назва", "Каса" },
                { "Валюта_Назва", "Валюта" },
                { "Сума", "Сума" }
            };

            return columns;
        }

        public static Dictionary<string, string> РухКоштів_КолонкиДаних()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "Організація_Назва", "Організація" },
                { "Каса_Назва", "Каса" },
                { "Валюта_Назва", "Валюта" }
            };

            return columns;
        }

        public static Dictionary<string, string> РухКоштів_ТипиДаних()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "Організація_Назва", Організації_Const.POINTER },
                { "Каса_Назва", Каси_Const.POINTER },
                { "Валюта_Назва", Валюти_Const.POINTER }
            };

            return columns;
        }

        public static Dictionary<string, float> РухКоштів_ПозиціяТекстуВКолонці()
        {
            Dictionary<string, float> columns = new Dictionary<string, float>
            {
                { "income", 0.5f },
                { "Сума", 1 }
            };

            return columns;
        }

        public static Dictionary<string, TreeCellDataFunc> РухКоштів_ФункціяДляКолонки()
        {
            Dictionary<string, TreeCellDataFunc> columns = new Dictionary<string, TreeCellDataFunc>
            {
                { "Сума", ФункціїДляЗвітів.ФункціяДляКолонкиБазоваДляЧисла }
            };

            return columns;
        }

        public static string РухКоштів_Запит = $@"
SELECT 
    (CASE WHEN Рег_РухКоштів.income = true THEN '+' ELSE '-' END) AS income, 
    Рег_РухКоштів.period,
    Рег_РухКоштів.{РухКоштів_Const.Організація} AS Організація, 
    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва, 
    Рег_РухКоштів.{РухКоштів_Const.Каса} AS Каса,
    Довідник_Каси.{Каси_Const.Назва} AS Каса_Назва, 
    Рег_РухКоштів.{РухКоштів_Const.Валюта} AS Валюта,
    Довідник_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    Рег_РухКоштів.{РухКоштів_Const.Сума} AS Сума
FROM 
    {РухКоштів_Const.TABLE} AS Рег_РухКоштів

    LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = 
        Рег_РухКоштів.{РухКоштів_Const.Організація}

    LEFT JOIN {Каси_Const.TABLE} AS Довідник_Каси ON Довідник_Каси.uid = 
        Рег_РухКоштів.{РухКоштів_Const.Каса}

    LEFT JOIN {Валюти_Const.TABLE} AS Довідник_Валюти ON Довідник_Валюти.uid = 
       Рег_РухКоштів.{РухКоштів_Const.Валюта}
WHERE
    Рег_РухКоштів.Owner = @ДокументВказівник
ORDER BY Організація_Назва
";

        #endregion

        #region Закупівлі

        public static Dictionary<string, string> Закупівлі_ВидиміКолонки()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "period", "Період" },
                { "Організація_Назва", "Організація" },
                { "Склад_Назва", "Склад" },
                { "Контрагент_Назва", "Контрагент" },
                { "Договір_Назва", "Договір" },
                { "Номенклатура_Назва", "Номенклатура" },
                { "Кількість", "Кількість" },
                { "Сума", "Сума" },
                { "Собівартість", "Собівартість" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                columns.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");

            return columns;
        }

        public static Dictionary<string, string> Закупівлі_КолонкиДаних()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "Організація_Назва", "Організація" },
                { "Склад_Назва", "Склад" },
                { "Контрагент_Назва", "Контрагент" },
                { "Договір_Назва", "Договір" },
                { "Номенклатура_Назва", "Номенклатура" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                columns.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");

            return columns;
        }

        public static Dictionary<string, string> Закупівлі_ТипиДаних()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "Організація_Назва", Організації_Const.POINTER },
                { "Склад_Назва", Склади_Const.POINTER },
                { "Контрагент_Назва", Контрагенти_Const.POINTER },
                { "Договір_Назва", ДоговориКонтрагентів_Const.POINTER },
                { "Номенклатура_Назва", Номенклатура_Const.POINTER },
                { "ХарактеристикаНоменклатури_Назва", ХарактеристикиНоменклатури_Const.POINTER }
            };

            return columns;
        }

        public static Dictionary<string, float> Закупівлі_ПозиціяТекстуВКолонці()
        {
            Dictionary<string, float> columns = new Dictionary<string, float>
            {
                { "Кількість", 1 },
                { "Сума", 1 },
                { "Собівартість", 1 }
            };

            return columns;
        }

        public static Dictionary<string, TreeCellDataFunc> Закупівлі_ФункціяДляКолонки()
        {
            Dictionary<string, TreeCellDataFunc> columns = new Dictionary<string, TreeCellDataFunc>
            {
                { "Кількість", ФункціїДляЗвітів.ФункціяДляКолонкиБазоваДляЧисла },
                { "Сума", ФункціїДляЗвітів.ФункціяДляКолонкиБазоваДляЧисла },
                { "Собівартість", ФункціїДляЗвітів.ФункціяДляКолонкиБазоваДляЧисла }
            };

            return columns;
        }

        public static string Закупівлі_Запит = $@"
SELECT 
    Рег_Закупівлі.period, 
    Рег_Закупівлі.{Закупівлі_Const.Організація} AS Організація, 
    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва,
    Рег_Закупівлі.{Закупівлі_Const.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Рег_Закупівлі.{Закупівлі_Const.Контрагент} AS Контрагент, 
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS Контрагент_Назва,
    Рег_Закупівлі.{Закупівлі_Const.Договір} AS Договір, 
    Довідник_ДоговориКонтрагентів.{ДоговориКонтрагентів_Const.Назва} AS Договір_Назва,
    Рег_Закупівлі.{Закупівлі_Const.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    Рег_Закупівлі.{Закупівлі_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва, 
    Рег_Закупівлі.{Закупівлі_Const.Кількість} AS Кількість,
    Рег_Закупівлі.{Закупівлі_Const.Сума} AS Сума,
    Рег_Закупівлі.{Закупівлі_Const.Собівартість} AS Собівартість
FROM 
    {Закупівлі_Const.TABLE} AS Рег_Закупівлі

    LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = 
        Рег_Закупівлі.{Закупівлі_Const.Організація}

    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
       Рег_Закупівлі.{Закупівлі_Const.Склад}

    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Рег_Закупівлі.{Закупівлі_Const.Контрагент}

    LEFT JOIN {ДоговориКонтрагентів_Const.TABLE} AS Довідник_ДоговориКонтрагентів ON Довідник_ДоговориКонтрагентів.uid = 
        Рег_Закупівлі.{Закупівлі_Const.Договір}

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        Рег_Закупівлі.{Закупівлі_Const.Номенклатура}

    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        Рег_Закупівлі.{Закупівлі_Const.ХарактеристикаНоменклатури}
WHERE
    Рег_Закупівлі.Owner = @ДокументВказівник
";

        #endregion

        #region Продажі

        public static Dictionary<string, string> Продажі_ВидиміКолонки()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "period", "Період" },
                { "Організація_Назва", "Організація" },
                { "Склад_Назва", "Склад" },
                { "Контрагент_Назва", "Контрагент" },
                { "Договір_Назва", "Договір" },
                { "Номенклатура_Назва", "Номенклатура" },
                { "Кількість", "Кількість" },
                { "Сума", "Сума" },
                { "Дохід", "Дохід" },
                { "Собівартість", "Собівартість" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                columns.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");

            return columns;
        }

        public static Dictionary<string, string> Продажі_КолонкиДаних()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "Організація_Назва", "Організація" },
                { "Склад_Назва", "Склад" },
                { "Контрагент_Назва", "Контрагент" },
                { "Договір_Назва", "Договір" },
                { "Номенклатура_Назва", "Номенклатура" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                columns.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");

            return columns;
        }

        public static Dictionary<string, string> Продажі_ТипиДаних()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "Організація_Назва", Організації_Const.POINTER },
                { "Склад_Назва", Склади_Const.POINTER },
                { "Контрагент_Назва", Контрагенти_Const.POINTER },
                { "Договір_Назва", ДоговориКонтрагентів_Const.POINTER },
                { "Номенклатура_Назва", Номенклатура_Const.POINTER },
                { "ХарактеристикаНоменклатури_Назва", ХарактеристикиНоменклатури_Const.POINTER }
            };

            return columns;
        }

        public static Dictionary<string, float> Продажі_ПозиціяТекстуВКолонці()
        {
            Dictionary<string, float> columns = new Dictionary<string, float>
            {
                { "Кількість", 1 },
                { "Сума", 1 },
                { "Дохід", 1 },
                { "Собівартість", 1 }
            };

            return columns;
        }

        public static Dictionary<string, TreeCellDataFunc> Продажі_ФункціяДляКолонки()
        {
            Dictionary<string, TreeCellDataFunc> columns = new Dictionary<string, TreeCellDataFunc>
            {
                { "Кількість", ФункціїДляЗвітів.ФункціяДляКолонкиБазоваДляЧисла },
                { "Сума", ФункціїДляЗвітів.ФункціяДляКолонкиБазоваДляЧисла },
                { "Дохід", ФункціїДляЗвітів.ФункціяДляКолонкиБазоваДляЧисла },
                { "Собівартість", ФункціїДляЗвітів.ФункціяДляКолонкиБазоваДляЧисла }
            };

            return columns;
        }

        public static string Продажі_Запит = $@"
SELECT 
    Рег_Продажі.period, 
    Рег_Продажі.{Продажі_Const.Організація} AS Організація, 
    Довідник_Організації.{Організації_Const.Назва} AS Організація_Назва,
    Рег_Продажі.{Продажі_Const.Склад} AS Склад,
    Довідник_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Рег_Продажі.{Продажі_Const.Контрагент} AS Контрагент, 
    Довідник_Контрагенти.{Контрагенти_Const.Назва} AS Контрагент_Назва,
    Рег_Продажі.{Продажі_Const.Договір} AS Договір, 
    Довідник_ДоговориКонтрагентів.{ДоговориКонтрагентів_Const.Назва} AS Договір_Назва,
    Рег_Продажі.{Продажі_Const.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    Рег_Продажі.{Продажі_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва, 
    Рег_Продажі.{Продажі_Const.Кількість} AS Кількість,
    Рег_Продажі.{Продажі_Const.Сума} AS Сума,
    Рег_Продажі.{Продажі_Const.Дохід} AS Дохід,
    Рег_Продажі.{Продажі_Const.Собівартість} AS Собівартість
FROM 
    {Продажі_Const.TABLE} AS Рег_Продажі

    LEFT JOIN {Організації_Const.TABLE} AS Довідник_Організації ON Довідник_Організації.uid = 
        Рег_Продажі.{Продажі_Const.Організація}

    LEFT JOIN {Склади_Const.TABLE} AS Довідник_Склади ON Довідник_Склади.uid = 
       Рег_Продажі.{Продажі_Const.Склад}

    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідник_Контрагенти ON Довідник_Контрагенти.uid = 
        Рег_Продажі.{Продажі_Const.Контрагент}

    LEFT JOIN {ДоговориКонтрагентів_Const.TABLE} AS Довідник_ДоговориКонтрагентів ON Довідник_ДоговориКонтрагентів.uid = 
        Рег_Продажі.{Продажі_Const.Договір}

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        Рег_Продажі.{Продажі_Const.Номенклатура}

    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        Рег_Продажі.{Продажі_Const.ХарактеристикаНоменклатури}
WHERE
    Рег_Продажі.Owner = @ДокументВказівник
";

        #endregion

        #region ТовариВКомірках

        public static Dictionary<string, string> ТовариВКомірках_ВидиміКолонки()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "income", "Рух" },
                { "period", "Період" },
                { "Номенклатура_Назва", "Номенклатура" },
                { "Пакування_Назва", "Пакування" },
                { "Комірка_Назва", "Комірка" },
                { "ВНаявності", "ВНаявності" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                columns.Add("ХарактеристикаНоменклатури_Назва", "Характеристика");
            if (Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const)
                columns.Add("Серія_Номер", "Серія");

            return columns;
        }

        public static Dictionary<string, string> ТовариВКомірках_КолонкиДаних()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "Номенклатура_Назва", "Номенклатура" },
                { "Пакування_Назва", "Пакування" },
                { "Комірка_Назва", "Комірка" }
            };
            if (Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                columns.Add("ХарактеристикаНоменклатури_Назва", "ХарактеристикаНоменклатури");
            if (Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const)
                columns.Add("Серія_Номер", "Серія");

            return columns;
        }

        public static Dictionary<string, string> ТовариВКомірках_ТипиДаних()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
            {
                { "Номенклатура_Назва", Номенклатура_Const.POINTER },
                { "ХарактеристикаНоменклатури_Назва", ХарактеристикиНоменклатури_Const.POINTER },
                { "Пакування_Назва", ПакуванняОдиниціВиміру_Const.POINTER },
                { "Комірка_Назва", СкладськіКомірки_Const.POINTER },
                { "Серія_Номер", СеріїНоменклатури_Const.POINTER }
            };

            return columns;
        }

        public static Dictionary<string, float> ТовариВКомірках_ПозиціяТекстуВКолонці()
        {
            Dictionary<string, float> columns = new Dictionary<string, float>
            {
                { "income", 0.5f },
                { "ВНаявності", 1 }
            };

            return columns;
        }

        public static Dictionary<string, TreeCellDataFunc> ТовариВКомірках_ФункціяДляКолонки()
        {
            Dictionary<string, TreeCellDataFunc> columns = new Dictionary<string, TreeCellDataFunc>
            {
                { "ВНаявності", ФункціїДляЗвітів.ФункціяДляКолонкиБазоваДляЧисла }
            };

            return columns;
        }

        public static string ТовариВКомірках_Запит = $@"
SELECT 
    (CASE WHEN Рег_ТовариВКомірках.income = true THEN '+' ELSE '-' END) AS income,
    Рег_ТовариВКомірках.period, 
    Рег_ТовариВКомірках.{ТовариВКомірках_Const.Номенклатура} AS Номенклатура, 
    Довідник_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва, 
    Рег_ТовариВКомірках.{ТовариВКомірках_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Довідник_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва, 
    Рег_ТовариВКомірках.{ТовариВКомірках_Const.Пакування} AS Пакування,
    Довідник_ПакуванняОдиниціВиміру.{ПакуванняОдиниціВиміру_Const.Назва} AS Пакування_Назва,
    Рег_ТовариВКомірках.{ТовариВКомірках_Const.Комірка} AS Комірка,
    Довідник_СкладськіКомірки.{СкладськіКомірки_Const.Назва} AS Комірка_Назва,
    Рег_ТовариВКомірках.{ТовариВКомірках_Const.Серія} AS Серія,
    Довідник_СеріїНоменклатури.{СеріїНоменклатури_Const.Номер} AS Серія_Номер,
    Рег_ТовариВКомірках.{ТовариВКомірках_Const.ВНаявності} AS ВНаявності
FROM 
    {ТовариВКомірках_Const.TABLE} AS Рег_ТовариВКомірках

    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
        Рег_ТовариВКомірках.{ТовариВКомірках_Const.Номенклатура}

    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідник_ХарактеристикиНоменклатури ON Довідник_ХарактеристикиНоменклатури.uid = 
        Рег_ТовариВКомірках.{ТовариВКомірках_Const.ХарактеристикаНоменклатури}

    LEFT JOIN {ПакуванняОдиниціВиміру_Const.TABLE} AS Довідник_ПакуванняОдиниціВиміру ON Довідник_ПакуванняОдиниціВиміру.uid = 
       Рег_ТовариВКомірках.{ТовариВКомірках_Const.Пакування}
    
    LEFT JOIN {СкладськіКомірки_Const.TABLE} AS Довідник_СкладськіКомірки ON Довідник_СкладськіКомірки.uid = 
       Рег_ТовариВКомірках.{ТовариВКомірках_Const.Комірка}

    LEFT JOIN {СеріїНоменклатури_Const.TABLE} AS Довідник_СеріїНоменклатури ON Довідник_СеріїНоменклатури.uid = 
       Рег_ТовариВКомірках.{ТовариВКомірках_Const.Серія}
WHERE
    Рег_ТовариВКомірках.Owner = @ДокументВказівник
ORDER BY Номенклатура_Назва
";

        #endregion
    }
}