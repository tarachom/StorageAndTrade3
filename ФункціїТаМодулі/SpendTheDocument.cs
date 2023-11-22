﻿/*
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

/*
 
Модуль проведення документів
 
*/

using AccountingSoftware;
using StorageAndTrade;

using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.РегістриНакопичення;
using StorageAndTrade_1_0.РегістриВідомостей;

namespace StorageAndTrade_1_0.Документи
{
    class СпільніФункції
    {
        /// <summary>
        /// Список наявних партій на дату документу не враховуючи сам документ
        /// </summary>
        /// <param name="Організація">Організація</param>
        /// <param name="Номенклатура">Номенклатура</param>
        /// <param name="ХарактеристикаНоменклатури">ХарактеристикаНоменклатури</param>
        /// <param name="Серія">Серія</param>
        /// <param name="Склад">Склад</param>
        /// <param name="Owner">UID Документу який проводиться</param>
        /// <param name="OwnerDateDoc">Дата документу який проводиться</param>
        /// <param name="Кількість">Кількість яку потрібно списати</param>
        /// <returns>Іменований список</returns>
        public static List<Dictionary<string, object>> ОтриматиСписокНаявнихПартій(
            Організації_Pointer Організація,
            Номенклатура_Pointer Номенклатура,
            ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури,
            СеріїНоменклатури_Pointer Серія,
            Склади_Pointer Склад,
            ПартіяТоварівКомпозит_Pointer Партія,
            UnigueID Owner,
            DateTime OwnerDateDoc,
            decimal Кількість)
        {
            Перелічення.МетодиСписанняПартій МетодСписання;

            if (Константи.ПартіїТоварів.МетодСписанняПартій_Const == Перелічення.МетодиСписанняПартій.FIFO)
                МетодСписання = Перелічення.МетодиСписанняПартій.FIFO;
            else if (Константи.ПартіїТоварів.МетодСписанняПартій_Const == Перелічення.МетодиСписанняПартій.LIFO)
                МетодСписання = Перелічення.МетодиСписанняПартій.LIFO;
            else
                МетодСписання = Перелічення.МетодиСписанняПартій.FIFO;

            string МетодСортування = МетодСписання == Перелічення.МетодиСписанняПартій.FIFO ? "ASC" : "DESC";

            //
            //Вибірка партій з регістру з відборами по параметрах
            //

            string query = $@"
WITH register AS
(
	SELECT 
		ПартіїТоварів.{ПартіїТоварів_Const.ПартіяТоварівКомпозит} AS ПартіяТоварівКомпозит,
		ПартіїТоварів.{ПартіїТоварів_Const.Рядок} AS Рядок,
		SUM(CASE WHEN ПартіїТоварів.income = true THEN 
			ПартіїТоварів.{ПартіїТоварів_Const.Кількість} ELSE 
			-ПартіїТоварів.{ПартіїТоварів_Const.Кількість} END) AS Кількість,
		SUM(CASE WHEN ПартіїТоварів.income = true THEN 
			ПартіїТоварів.{ПартіїТоварів_Const.Собівартість} ELSE 
			-ПартіїТоварів.{ПартіїТоварів_Const.Собівартість} END) AS Собівартість
	FROM
		{ПартіїТоварів_Const.TABLE} AS ПартіїТоварів
	WHERE
		ПартіїТоварів.period <= @period_end
		AND ПартіїТоварів.{ПартіїТоварів_Const.Організація} = '{Організація.UnigueID}'
		AND ПартіїТоварів.{ПартіїТоварів_Const.Номенклатура} = '{Номенклатура.UnigueID}'
		AND ПартіїТоварів.{ПартіїТоварів_Const.ХарактеристикаНоменклатури} = '{ХарактеристикаНоменклатури.UnigueID}'
		AND ПартіїТоварів.{ПартіїТоварів_Const.Серія} = '{Серія.UnigueID}'
        AND ПартіїТоварів.{ПартіїТоварів_Const.Склад} = '{Склад.UnigueID}'";

            if (!Партія.IsEmpty())
            {
                query += $@"
        AND ПартіїТоварів.{ПартіїТоварів_Const.ПартіяТоварівКомпозит} = '{Партія.UnigueID}'";
            }

            query += $@"
        AND ПартіїТоварів.owner != '{Owner}'

	GROUP BY ПартіяТоварівКомпозит, Рядок

	HAVING
		SUM(CASE WHEN ПартіїТоварів.income = true THEN 
			ПартіїТоварів.{ПартіїТоварів_Const.Кількість} ELSE 
			-ПартіїТоварів.{ПартіїТоварів_Const.Кількість} END) > 0
)";

            //
            //ДостатняКількість обчислюється для того щоб вибирати тільки потрібні партії, а не всі наявні партії.
            //ДостатняКількість = Накопичена кількість >= Потрібній кількості
            //

            query += $@"
, Обчислення AS
(
	SELECT
	   Довідник_ПартіяТоварівКомпозит.{ПартіяТоварівКомпозит_Const.Дата} AS ДатаПоступлення,
	   register.ПартіяТоварівКомпозит,
       register.Рядок,
	   register.Кількість,
	   register.Собівартість,
	   SUM(register.Кількість) OVER(ORDER BY Довідник_ПартіяТоварівКомпозит.{ПартіяТоварівКомпозит_Const.Дата} {МетодСортування}, register.Рядок) >= @Кількість AS ДостатняКількість
	FROM
	   register
            LEFT JOIN {ПартіяТоварівКомпозит_Const.TABLE} AS Довідник_ПартіяТоварівКомпозит ON Довідник_ПартіяТоварівКомпозит.uid = 
                register.ПартіяТоварівКомпозит
	ORDER BY ДатаПоступлення {МетодСортування}
)";

            //
            //Вибираються дві групи
            //1. Партії які мають кількість меншу потрібній кількості
            //2. Одну партію яка закриває потрібну кількість
            //

            query += $@"
(
	SELECT 
		ПартіяТоварівКомпозит,
        Рядок,
		Кількість,
		Собівартість
	FROM Обчислення
	WHERE ДостатняКількість = false
)
UNION ALL
(
	SELECT 
		ПартіяТоварівКомпозит,
        Рядок,
		Кількість,
		Собівартість
	FROM Обчислення
	WHERE ДостатняКількість = true
	LIMIT 1
)
";
            Dictionary<string, object> paramQuery = new Dictionary<string, object>()
            {
                { "period_end", OwnerDateDoc }, { "Кількість", Кількість }
            };

            string[] columnsName;
            List<Dictionary<string, object>> listNameRow;

            Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listNameRow);

            return listNameRow;
        }

        /// <summary>
        /// Залишки товару на складі на дату документу не враховуючи сам документ
        /// </summary>
        /// <param name="Номенклатура">Номенклатура</param>
        /// <param name="ХарактеристикаНоменклатури">ХарактеристикаНоменклатури</param>
        /// <param name="Серія">Серія</param>
        /// <param name="Склад">Склад</param>
        /// <param name="Owner">UID Документу який проводиться</param>
        /// <param name="OwnerDateDoc">Дата документу який проводиться</param>
        /// <returns>Іменований список</returns>
        public static List<Dictionary<string, object>> ОтриматиЗалишкиТоваруНаСкладі(
            Номенклатура_Pointer Номенклатура, ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури,
            СеріїНоменклатури_Pointer Серія, Склади_Pointer Склад,
            UnigueID Owner, DateTime OwnerDateDoc)
        {
            string query = $@"
WITH register AS
(
	SELECT 
		'Залишок' AS Група,
		SUM(CASE WHEN ТовариНаСкладах.income = true THEN 
			ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} ELSE 
			-ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} END) AS ВНаявності
	FROM
		{ТовариНаСкладах_Const.TABLE} AS ТовариНаСкладах
	WHERE
		ТовариНаСкладах.period <= @period_end
		AND ТовариНаСкладах.{ТовариНаСкладах_Const.Номенклатура} = '{Номенклатура.UnigueID}'
		AND ТовариНаСкладах.{ТовариНаСкладах_Const.ХарактеристикаНоменклатури} = '{ХарактеристикаНоменклатури.UnigueID}'
        AND ТовариНаСкладах.{ТовариНаСкладах_Const.Серія} = '{Серія.UnigueID}'
        AND ТовариНаСкладах.{ТовариНаСкладах_Const.Склад} = '{Склад.UnigueID}'
        AND ТовариНаСкладах.owner != '{Owner}'

	GROUP BY Група
)
SELECT
    ВНаявності
FROM
    register
";

            Dictionary<string, object> paramQuery = new Dictionary<string, object>()
            {
                { "period_end", OwnerDateDoc }
            };

            string[] columnsName;
            List<Dictionary<string, object>> listNameRow;

            Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listNameRow);

            return listNameRow;
        }

        /// <summary>
        /// Залишки товару в комірці на дату документу не враховуючи сам документ
        /// </summary>
        /// <param name="Номенклатура">Номенклатура</param>
        /// <param name="ХарактеристикаНоменклатури">ХарактеристикаНоменклатури</param>
        /// <param name="Пакування">Пакування</param>
        /// <param name="Комірка">Комірка</param>
        /// <param name="Серія">Серія</param>
        /// <param name="Owner">UID Документу який проводиться</param>
        /// <param name="OwnerDateDoc">Дата документу який проводиться</param>
        /// <returns>Іменований список</returns>
        public static List<Dictionary<string, object>> ОтриматиЗалишкиТоваруВКомірці(
            Номенклатура_Pointer Номенклатура, ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури,
            ПакуванняОдиниціВиміру_Pointer Пакування, СкладськіКомірки_Pointer Комірка,
            СеріїНоменклатури_Pointer Серія,
            UnigueID Owner, DateTime OwnerDateDoc)
        {
            string query = $@"
WITH register AS
(
	SELECT 
		'Залишок' AS Група,
		SUM(CASE WHEN ТовариВКомірках.income = true THEN 
			ТовариВКомірках.{ТовариВКомірках_Const.ВНаявності} ELSE 
			-ТовариВКомірках.{ТовариВКомірках_Const.ВНаявності} END) AS ВНаявності
	FROM
		{ТовариВКомірках_Const.TABLE} AS ТовариВКомірках
	WHERE
		ТовариВКомірках.period <= @period_end
		AND ТовариВКомірках.{ТовариВКомірках_Const.Номенклатура} = '{Номенклатура.UnigueID}'
		AND ТовариВКомірках.{ТовариВКомірках_Const.ХарактеристикаНоменклатури} = '{ХарактеристикаНоменклатури.UnigueID}'
        AND ТовариВКомірках.{ТовариВКомірках_Const.Пакування} = '{Пакування.UnigueID}'
        AND ТовариВКомірках.{ТовариВКомірках_Const.Комірка} = '{Комірка.UnigueID}'
        AND ТовариВКомірках.{ТовариВКомірках_Const.Серія} = '{Серія.UnigueID}'
        AND ТовариВКомірках.owner != '{Owner}'

	GROUP BY Група
)
SELECT
    ВНаявності
FROM
    register
";

            Dictionary<string, object> paramQuery = new Dictionary<string, object>()
            {
                { "period_end", OwnerDateDoc }
            };

            string[] columnsName;
            List<Dictionary<string, object>> listNameRow;

            Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listNameRow);

            return listNameRow;
        }

        /// <summary>
        /// Зарезервовані та наявні товари на складі на дату документу не враховуючи сам документ
        /// </summary>
        /// <param name="Номенклатура">Номенклатура</param>
        /// <param name="ХарактеристикаНоменклатури">ХарактеристикаНоменклатури</param>
        /// <param name="Склад">Склад</param>
        /// <param name="Owner">UID Документу який проводиться</param>
        /// <param name="OwnerDateDoc">Дата документу який проводиться</param>
        /// <returns>Іменований список</returns>
        public static List<Dictionary<string, object>> ОтриматиРезервТоваруНаСкладі(
            Номенклатура_Pointer Номенклатура, ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури, Склади_Pointer Склад,
            UnigueID Owner, DateTime OwnerDateDoc)
        {
            string query = $@"
WITH register AS
(
	SELECT 
		'Резерв' AS Група,

        SUM(CASE WHEN ВільніЗалишки.income = true THEN 
			ВільніЗалишки.{ВільніЗалишки_Const.ВНаявності} ELSE 
			-ВільніЗалишки.{ВільніЗалишки_Const.ВНаявності} END) AS ВНаявності,

		SUM(CASE WHEN ВільніЗалишки.income = true THEN 
			ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіЗіСкладу} ELSE 
			-ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіЗіСкладу} END) AS ВРезервіЗіСкладу,
       
        SUM(CASE WHEN ВільніЗалишки.income = true THEN 
			ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіПідЗамовлення} ELSE 
			-ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіПідЗамовлення} END) AS ВРезервіПідЗамовлення
	FROM
		{ВільніЗалишки_Const.TABLE} AS ВільніЗалишки
	WHERE
		ВільніЗалишки.period <= @period_end
		AND ВільніЗалишки.{ВільніЗалишки_Const.Номенклатура} = '{Номенклатура.UnigueID}'
		AND ВільніЗалишки.{ВільніЗалишки_Const.ХарактеристикаНоменклатури} = '{ХарактеристикаНоменклатури.UnigueID}'
        AND ВільніЗалишки.{ВільніЗалишки_Const.Склад} = '{Склад.UnigueID}'
        AND ВільніЗалишки.owner != '{Owner}'

	GROUP BY Група
)
SELECT
    ВНаявності,
    ВРезервіЗіСкладу,
    ВРезервіПідЗамовлення
FROM
    register
";

            Dictionary<string, object> paramQuery = new Dictionary<string, object>()
            {
                { "period_end", OwnerDateDoc }
            };

            string[] columnsName;
            List<Dictionary<string, object>> listNameRow;

            Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listNameRow);

            return listNameRow;
        }

        /// <summary>
        /// Ортримати кількість зарезервованого товару конкретним документом
        /// </summary>
        /// <param name="Номенклатура">Номенклатура</param>
        /// <param name="ХарактеристикаНоменклатури">ХарактеристикаНоменклатури</param>
        /// <param name="Склад">Склад</param>
        /// <param name="Owner">Документ</param>
        /// <returns>Іменований список</returns>
        public static List<Dictionary<string, object>> ОтриматиРезервТоваруПоДокументу(
            Номенклатура_Pointer Номенклатура, ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури, Склади_Pointer Склад,
            UnigueID ДокументРезерву, UnigueID Owner, DateTime OwnerDateDoc)
        {
            string query = $@"
WITH register AS
(
	SELECT 
		'Резерв' AS Група,

		SUM(CASE WHEN ВільніЗалишки.income = true THEN 
			ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіЗіСкладу} ELSE 
			-ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіЗіСкладу} END) AS ВРезервіЗіСкладу
	FROM
		{ВільніЗалишки_Const.TABLE} AS ВільніЗалишки
	WHERE
        ВільніЗалишки.period <= @period_end
        AND ВільніЗалишки.{ВільніЗалишки_Const.ДокументРезерву} = '{ДокументРезерву}'
		AND ВільніЗалишки.{ВільніЗалишки_Const.Номенклатура} = '{Номенклатура.UnigueID}'
		AND ВільніЗалишки.{ВільніЗалишки_Const.ХарактеристикаНоменклатури} = '{ХарактеристикаНоменклатури.UnigueID}'
        AND ВільніЗалишки.{ВільніЗалишки_Const.Склад} = '{Склад.UnigueID}'
        AND ВільніЗалишки.owner != '{Owner}'

	GROUP BY Група
)
SELECT
    ВРезервіЗіСкладу
FROM
    register
";

            Dictionary<string, object> paramQuery = new Dictionary<string, object>()
            {
                { "period_end", OwnerDateDoc }
            };

            string[] columnsName;
            List<Dictionary<string, object>> listNameRow;

            Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listNameRow);

            return listNameRow;
        }

        /// <summary>
        /// Замовлення клієнта для товару по документу
        /// </summary>
        /// <param name="Номенклатура">Номенклатура</param>
        /// <param name="ХарактеристикаНоменклатури">ХарактеристикаНоменклатури</param>
        /// <param name="Склад">Склад</param>
        /// <param name="ЗамовленняКлієнта">ЗамовленняКлієнта</param>
        /// <param name="Owner">Документ</param>
        /// <param name="OwnerDateDoc">Дата документу</param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> ОтриматиЗамовленняКлієнтаДляТоваруПоДокументу(
            Номенклатура_Pointer Номенклатура, ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури, Склади_Pointer Склад,
            UnigueID ЗамовленняКлієнта, UnigueID Owner, DateTime OwnerDateDoc)
        {
            string query = $@"
WITH register AS
(
	SELECT 
		'Замовлення' AS Група,

		SUM(CASE WHEN ЗамовленняКлієнтів.income = true THEN 
			ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Замовлено} ELSE 
			-ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Замовлено} END) AS Замовлено,

        SUM(CASE WHEN ЗамовленняКлієнтів.income = true THEN 
			ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Сума} ELSE 
			-ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Сума} END) AS Сума
	FROM
		{ЗамовленняКлієнтів_Const.TABLE} AS ЗамовленняКлієнтів
	WHERE
        ЗамовленняКлієнтів.period <= @period_end
        AND ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.ЗамовленняКлієнта} = '{ЗамовленняКлієнта}'
		AND ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Номенклатура} = '{Номенклатура.UnigueID}'
		AND ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.ХарактеристикаНоменклатури} = '{ХарактеристикаНоменклатури.UnigueID}'
        AND ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Склад} = '{Склад.UnigueID}'
        AND ЗамовленняКлієнтів.owner != '{Owner}'

	GROUP BY Група
)
SELECT
    Замовлено,
    Сума
FROM
    register
";

            Dictionary<string, object> paramQuery = new Dictionary<string, object>()
            {
                { "period_end", OwnerDateDoc }
            };

            string[] columnsName;
            List<Dictionary<string, object>> listNameRow;

            Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listNameRow);

            return listNameRow;
        }

        /// <summary>
        /// Замовлення постачальника для товару по документу
        /// </summary>
        /// <param name="Номенклатура">Номенклатура</param>
        /// <param name="ХарактеристикаНоменклатури">ХарактеристикаНоменклатури</param>
        /// <param name="Склад">Склад</param>
        /// <param name="ЗамовленняПостачальнику">ЗамовленняПостачальнику</param>
        /// <param name="Owner">Документ</param>
        /// <param name="OwnerDateDoc">Дата документу</param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> ОтриматиЗамовленняПостачальникуДляТоваруПоДокументу(
            Номенклатура_Pointer Номенклатура, ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури, Склади_Pointer Склад,
            UnigueID ЗамовленняПостачальнику, UnigueID Owner, DateTime OwnerDateDoc)
        {
            string query = $@"
WITH register AS
(
	SELECT 
		'Замовлення' AS Група,

		SUM(CASE WHEN ЗамовленняПостачальникам.income = true THEN 
			ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} ELSE 
			-ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} END) AS Замовлено
	FROM
		{ЗамовленняПостачальникам_Const.TABLE} AS ЗамовленняПостачальникам
	WHERE
        ЗамовленняПостачальникам.period <= @period_end
        AND ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.ЗамовленняПостачальнику} = '{ЗамовленняПостачальнику}'
		AND ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Номенклатура} = '{Номенклатура.UnigueID}'
		AND ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.ХарактеристикаНоменклатури} = '{ХарактеристикаНоменклатури.UnigueID}'
        AND ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Склад} = '{Склад.UnigueID}'
        AND ЗамовленняПостачальникам.owner != '{Owner}'

	GROUP BY Група
)
SELECT
    Замовлено
FROM
    register
";

            Dictionary<string, object> paramQuery = new Dictionary<string, object>()
            {
                { "period_end", OwnerDateDoc }
            };

            string[] columnsName;
            List<Dictionary<string, object>> listNameRow;

            Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listNameRow);

            return listNameRow;
        }

        /// <summary>
		/// Перервати проведення документу
		/// </summary>
		/// <param name="ДокументОбєкт">Документ обєкт</param>
		/// <param name="НазваДокументу">Назва документу</param>
		/// <param name="СписокПомилок">Список помилок</param>
        public static async void ДокументНеПроводиться(DocumentObject ДокументОбєкт, string НазваДокументу, string СписокПомилок)
        {
            await ФункціїДляПовідомлень.ДодатиПовідомленняПроПомилку(
                DateTime.Now, "Проведення документу", ДокументОбєкт.UnigueID.UGuid, $"Документи.{ДокументОбєкт.TypeDocument}", НазваДокументу,
                СписокПомилок + "\n\nДокумент [" + НазваДокументу + "] не проводиться!");
        }
    }

    #region Продажі

    class ЗамовленняКлієнта_SpendTheDocument
    {
        public static async ValueTask<bool> Spend(ЗамовленняКлієнта_Objest ДокументОбєкт)
        {
            try
            {
                #region Підготовка

                Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();
                Dictionary<int, decimal> ВільнийЗалишокНоменклатури = new Dictionary<int, decimal>();

                foreach (ЗамовленняКлієнта_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (ТовариРядок.Номенклатура.IsEmpty())
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");

                    if (!(ТовариРядок.Кількість > 0))
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");

                    СписокНоменклатури.Add(ТовариРядок.НомерРядка, (await ТовариРядок.Номенклатура.GetDirectoryObject())!);

                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        //
                        //Резерви та вільні залишки
                        //

                        List<Dictionary<string, object>> Резерв = СпільніФункції.ОтриматиРезервТоваруНаСкладі(
                            ТовариРядок.Номенклатура, ТовариРядок.ХарактеристикаНоменклатури,
                            !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад,
                            ДокументОбєкт.UnigueID, ДокументОбєкт.ДатаДок);

                        if (Резерв.Count > 0)
                        {
                            decimal ВНаявності = (decimal)Резерв[0]["ВНаявності"];
                            decimal ВРезервіЗіСкладу = (decimal)Резерв[0]["ВРезервіЗіСкладу"];

                            ВільнийЗалишокНоменклатури.Add(ТовариРядок.НомерРядка, ВНаявності - ВРезервіЗіСкладу);
                        }
                        else
                            ВільнийЗалишокНоменклатури.Add(ТовариРядок.НомерРядка, 0);
                    }
                }

                #endregion

                #region ЗамовленняКлієнтів

                ЗамовленняКлієнтів_RecordsSet замовленняКлієнтів_RecordsSet = new ЗамовленняКлієнтів_RecordsSet();

                await ДокументОбєкт.Товари_TablePart.Read();

                foreach (ЗамовленняКлієнта_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        ЗамовленняКлієнтів_RecordsSet.Record record = new ЗамовленняКлієнтів_RecordsSet.Record()
                        {
                            Income = true,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            ЗамовленняКлієнта = ДокументОбєкт.GetDocumentPointer(),
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Склад = !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад,
                            Замовлено = ТовариРядок.Кількість,
                            Сума = ТовариРядок.Сума
                        };

                        замовленняКлієнтів_RecordsSet.Records.Add(record);
                    }
                }

                await замовленняКлієнтів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region ВільніЗалишки

                ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();

                foreach (ЗамовленняКлієнта_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        ВільніЗалишки_RecordsSet.Record record = new ВільніЗалишки_RecordsSet.Record()
                        {
                            Income = true,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Склад = !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад,
                            ВРезервіЗіСкладу = ВільнийЗалишокНоменклатури[ТовариРядок.НомерРядка],
                            ДокументРезерву = ДокументОбєкт.UnigueID.UGuid
                        };

                        decimal КількістьЯкуПотрібноРозприділити = ТовариРядок.Кількість;

                        if (ВільнийЗалишокНоменклатури[ТовариРядок.НомерРядка] > 0)
                            if (КількістьЯкуПотрібноРозприділити <= ВільнийЗалишокНоменклатури[ТовариРядок.НомерРядка])
                            {
                                record.ВРезервіЗіСкладу = КількістьЯкуПотрібноРозприділити;
                                КількістьЯкуПотрібноРозприділити = 0;
                            }
                            else
                            {
                                record.ВРезервіЗіСкладу = ВільнийЗалишокНоменклатури[ТовариРядок.НомерРядка];
                                КількістьЯкуПотрібноРозприділити = КількістьЯкуПотрібноРозприділити - ВільнийЗалишокНоменклатури[ТовариРядок.НомерРядка];
                            }

                        if (КількістьЯкуПотрібноРозприділити > 0)
                            record.ВРезервіПідЗамовлення = КількістьЯкуПотрібноРозприділити;

                        вільніЗалишки_RecordsSet.Records.Add(record);
                    }
                }

                await вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, ex.Message);
                await ClearSpend(ДокументОбєкт);

                return false;
            }
        }

        public static async ValueTask ClearSpend(ЗамовленняКлієнта_Objest ДокументОбєкт)
        {
            ЗамовленняКлієнтів_RecordsSet замовленняКлієнтів_RecordsSet = new ЗамовленняКлієнтів_RecordsSet();
            await замовленняКлієнтів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();
            await вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class РахунокФактура_SpendTheDocument
    {
        public static async ValueTask<bool> Spend(РахунокФактура_Objest ДокументОбєкт)
        {
            try
            {
                #region Підготовка

                Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();

                foreach (РахунокФактура_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (ТовариРядок.Номенклатура.IsEmpty())
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");

                    if (!(ТовариРядок.Кількість > 0))
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");

                    СписокНоменклатури.Add(ТовариРядок.НомерРядка, (await ТовариРядок.Номенклатура.GetDirectoryObject())!);

                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        //
                        //Резерви та вільні залишки
                        //

                        List<Dictionary<string, object>> Резерв = СпільніФункції.ОтриматиРезервТоваруНаСкладі(
                            ТовариРядок.Номенклатура, ТовариРядок.ХарактеристикаНоменклатури,
                            !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад,
                            ДокументОбєкт.UnigueID, ДокументОбєкт.ДатаДок);

                        if (Резерв.Count > 0)
                        {
                            decimal ВНаявності = (decimal)Резерв[0]["ВНаявності"];
                            decimal ВРезервіЗіСкладу = (decimal)Резерв[0]["ВРезервіЗіСкладу"];

                            if (ВНаявності - ВРезервіЗіСкладу < ТовариРядок.Кількість)
                            {
                                throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                    $"В наявності {ВНаявності}, " +
                                    $"в резерві {ВРезервіЗіСкладу}, " +
                                    $"вільний залишок {ВНаявності - ВРезервіЗіСкладу}, " +
                                    $"потрібно зарезервувати {ТовариРядок.Кількість}");
                            }
                        }
                        else
                        {
                            throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                 $"Відсутній товар на залишку");
                        }
                    }
                }
                #endregion

                #region ВільніЗалишки

                ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();

                foreach (РахунокФактура_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        ВільніЗалишки_RecordsSet.Record record = new ВільніЗалишки_RecordsSet.Record()
                        {
                            Income = true,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Склад = !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад,
                            ВРезервіЗіСкладу = ТовариРядок.Кількість,
                            ДокументРезерву = ДокументОбєкт.UnigueID.UGuid
                        };

                        вільніЗалишки_RecordsSet.Records.Add(record);
                    }
                }

                await вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, ex.Message);
                await ClearSpend(ДокументОбєкт);

                return false;
            }
        }

        public static async ValueTask ClearSpend(РахунокФактура_Objest ДокументОбєкт)
        {
            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();
            await вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class РеалізаціяТоварівТаПослуг_SpendTheDocument
    {
        public async static ValueTask<bool> Spend(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {
            try
            {
                #region Підготовка

                Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();
                Dictionary<int, decimal> ЗалишокНоменклатури = new Dictionary<int, decimal>();
                Dictionary<int, decimal> РезервНоменклатури = new Dictionary<int, decimal>();
                Dictionary<int, decimal> РезервРахунку = new Dictionary<int, decimal>();
                Dictionary<int, decimal> РезервЗамовлення = new Dictionary<int, decimal>();

                Dictionary<int, decimal> ЗамволенняКлієнта = new Dictionary<int, decimal>();
                Dictionary<int, decimal> ЗамволенняКлієнтаСума = new Dictionary<int, decimal>();

                foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (ТовариРядок.Номенклатура.IsEmpty())
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");

                    if (!(ТовариРядок.Кількість > 0))
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");

                    СписокНоменклатури.Add(ТовариРядок.НомерРядка, (await ТовариРядок.Номенклатура.GetDirectoryObject())!);

                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        //
                        //Для товарів отримуємо залишки
                        //

                        List<Dictionary<string, object>> Залишок_Список = СпільніФункції.ОтриматиЗалишкиТоваруНаСкладі(
                            ТовариРядок.Номенклатура, ТовариРядок.ХарактеристикаНоменклатури, ТовариРядок.Серія,
                            !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад,
                            ДокументОбєкт.UnigueID, ДокументОбєкт.ДатаДок);

                        ЗалишокНоменклатури.Add(ТовариРядок.НомерРядка, Залишок_Список.Count > 0 ? (decimal)Залишок_Список[0]["ВНаявності"] : 0);

                        if (ЗалишокНоменклатури[ТовариРядок.НомерРядка] > 0)
                        {
                            if (ЗалишокНоменклатури[ТовариРядок.НомерРядка] < ТовариРядок.Кількість)
                            {
                                throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                    $"Залишок товару {ЗалишокНоменклатури[ТовариРядок.НомерРядка]}, потрібно {ТовариРядок.Кількість}");
                            }

                            //
                            // Рахуємо всі резерви
                            //

                            List<Dictionary<string, object>> Резерв_Список = СпільніФункції.ОтриматиРезервТоваруНаСкладі(
                            ТовариРядок.Номенклатура, ТовариРядок.ХарактеристикаНоменклатури,
                            !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад,
                            ДокументОбєкт.UnigueID, ДокументОбєкт.ДатаДок);

                            РезервНоменклатури.Add(ТовариРядок.НомерРядка, Резерв_Список.Count > 0 ? (decimal)Резерв_Список[0]["ВРезервіЗіСкладу"] : 0);

                            //
                            // Перевірка резервів по документах
                            //

                            if (!ТовариРядок.ЗамовленняКлієнта.IsEmpty())
                            {
                                //
                                // Якщо заданий документ Замовлення Клієнта
                                // потрібно дізнатися скільки цей документ зарезервував товарів
                                //

                                List<Dictionary<string, object>> РезервЗамовленняКлієнта_Список = СпільніФункції.ОтриматиРезервТоваруПоДокументу(
                                    ТовариРядок.Номенклатура, ТовариРядок.ХарактеристикаНоменклатури,
                                    !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад,
                                    ТовариРядок.ЗамовленняКлієнта.UnigueID, ДокументОбєкт.UnigueID, ДокументОбєкт.ДатаДок);

                                if (РезервЗамовленняКлієнта_Список.Count > 0)
                                    РезервЗамовлення.Add(ТовариРядок.НомерРядка, (decimal)РезервЗамовленняКлієнта_Список[0]["ВРезервіЗіСкладу"]);

                                //
                                // Перевірити замовлення
                                //

                                List<Dictionary<string, object>> Замовлення_Список = СпільніФункції.ОтриматиЗамовленняКлієнтаДляТоваруПоДокументу(
                                    ТовариРядок.Номенклатура, ТовариРядок.ХарактеристикаНоменклатури,
                                    !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад,
                                    ТовариРядок.ЗамовленняКлієнта.UnigueID, ДокументОбєкт.UnigueID, ДокументОбєкт.ДатаДок);

                                if (Замовлення_Список.Count > 0)
                                {
                                    ЗамволенняКлієнта.Add(ТовариРядок.НомерРядка, (decimal)Замовлення_Список[0]["Замовлено"]);
                                    ЗамволенняКлієнтаСума.Add(ТовариРядок.НомерРядка, (decimal)Замовлення_Список[0]["Сума"]);

                                    if (!(ЗамволенняКлієнта[ТовариРядок.НомерРядка] > 0))
                                    {
                                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                            $"Залишок по документу Замовлення {ЗамволенняКлієнта[ТовариРядок.НомерРядка]}");
                                    }
                                }
                                else
                                {
                                    throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                        $"Відсутнє замовлення для товару {СписокНоменклатури[ТовариРядок.НомерРядка].Назва} в документі Замовлення клієнта");
                                }
                            }

                            if (!ТовариРядок.РахунокФактура.IsEmpty())
                            {
                                //
                                // Якщо заданий документ Рахунок фактура
                                // потрібно дізнатися скільки цей документ зарезервував товарів
                                //

                                List<Dictionary<string, object>> РезервРахункуФактури_Список = СпільніФункції.ОтриматиРезервТоваруПоДокументу(
                                    ТовариРядок.Номенклатура, ТовариРядок.ХарактеристикаНоменклатури,
                                    !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад,
                                    ТовариРядок.РахунокФактура.UnigueID, ДокументОбєкт.UnigueID, ДокументОбєкт.ДатаДок);

                                if (РезервРахункуФактури_Список.Count > 0)
                                    РезервРахунку.Add(ТовариРядок.НомерРядка, (decimal)РезервРахункуФактури_Список[0]["ВРезервіЗіСкладу"]);
                                else
                                {
                                    throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                        $"Вказаний рахунок фактура, не зарезервував товар {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                                }
                            }

                            if (РезервНоменклатури[ТовариРядок.НомерРядка] > 0)
                            {
                                decimal ВРезервіЗамовленняКлієнта = РезервЗамовлення.ContainsKey(ТовариРядок.НомерРядка) ? РезервЗамовлення[ТовариРядок.НомерРядка] : 0;
                                decimal ВРезервіРахунокФактура = РезервРахунку.ContainsKey(ТовариРядок.НомерРядка) ? РезервРахунку[ТовариРядок.НомерРядка] : 0;
                                decimal РезервДокументівСума = ВРезервіРахунокФактура + ВРезервіЗамовленняКлієнта;
                                decimal РезервЗагальний = РезервНоменклатури[ТовариРядок.НомерРядка] - РезервДокументівСума;

                                if (ЗалишокНоменклатури[ТовариРядок.НомерРядка] - РезервЗагальний < ТовариРядок.Кількість)
                                {
                                    if (РезервДокументівСума == 0)
                                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                            $"Залишок товару {ЗалишокНоменклатури[ТовариРядок.НомерРядка]}, " +
                                            $"в резерві {РезервЗагальний}, " +
                                            $"потрібно {ТовариРядок.Кількість}");
                                    else
                                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                            $"Залишок товару {ЗалишокНоменклатури[ТовариРядок.НомерРядка]}, " +
                                            $"в резерві по вибраних документах (замовлення або рахунок) {РезервДокументівСума}, " +
                                            $"потрібно {ТовариРядок.Кількість}");
                                }
                            }
                        }
                        else
                        {
                            throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Відсутній товар {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        }
                    }
                }

                #endregion

                #region ЗамовленняКлієнтів

                ЗамовленняКлієнтів_RecordsSet замовленняКлієнтів_RecordsSet = new ЗамовленняКлієнтів_RecordsSet();

                foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        if (!ТовариРядок.ЗамовленняКлієнта.IsEmpty())
                        {
                            decimal Замовлено = ЗамволенняКлієнта.ContainsKey(ТовариРядок.НомерРядка) ? ЗамволенняКлієнта[ТовариРядок.НомерРядка] : 0;
                            decimal ЗамовленняСума = ЗамволенняКлієнтаСума.ContainsKey(ТовариРядок.НомерРядка) ? ЗамволенняКлієнтаСума[ТовариРядок.НомерРядка] : 0;

                            ЗамовленняКлієнтів_RecordsSet.Record record = new ЗамовленняКлієнтів_RecordsSet.Record()
                            {
                                Income = false,
                                Owner = ДокументОбєкт.UnigueID.UGuid,
                                ЗамовленняКлієнта = ТовариРядок.ЗамовленняКлієнта,
                                Номенклатура = ТовариРядок.Номенклатура,
                                ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                                Склад = !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад,
                                Замовлено = Замовлено > ТовариРядок.Кількість ? ТовариРядок.Кількість : Замовлено,
                                Сума = Замовлено > ТовариРядок.Кількість ? 0 : ЗамовленняСума
                            };

                            замовленняКлієнтів_RecordsSet.Records.Add(record);
                        }
                    }
                }

                await замовленняКлієнтів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region ВільніЗалишки

                ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();

                foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        decimal КількістьЯкуПотрібноРозприділити = ТовариРядок.Кількість;

                        //
                        // Якщо є документи замовлення або рахунок
                        // кількість потрібно розприділии між документами.
                        // В першу чергу списується ЗамовленняКлієнта, потім Рахунок фактура
                        //

                        if (ТовариРядок.ЗамовленняКлієнта.IsEmpty() || ТовариРядок.РахунокФактура.IsEmpty())
                        {
                            decimal ВРезервіЗамовленняКлієнта = РезервЗамовлення.ContainsKey(ТовариРядок.НомерРядка) ? РезервЗамовлення[ТовариРядок.НомерРядка] : 0;
                            decimal ВРезервіРахунокФактура = РезервРахунку.ContainsKey(ТовариРядок.НомерРядка) ? РезервРахунку[ТовариРядок.НомерРядка] : 0;

                            if (!ТовариРядок.ЗамовленняКлієнта.IsEmpty() && ВРезервіЗамовленняКлієнта > 0)
                            {
                                decimal вНаявності = КількістьЯкуПотрібноРозприділити >= ВРезервіЗамовленняКлієнта ?
                                    ВРезервіЗамовленняКлієнта : КількістьЯкуПотрібноРозприділити;

                                ВільніЗалишки_RecordsSet.Record record = new ВільніЗалишки_RecordsSet.Record()
                                {
                                    Income = false,
                                    Owner = ДокументОбєкт.UnigueID.UGuid,
                                    Номенклатура = ТовариРядок.Номенклатура,
                                    ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                                    Склад = !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад,
                                    ВНаявності = вНаявності,
                                    ВРезервіЗіСкладу = вНаявності,
                                    ДокументРезерву = ТовариРядок.ЗамовленняКлієнта.UnigueID.UGuid
                                };

                                вільніЗалишки_RecordsSet.Records.Add(record);

                                КількістьЯкуПотрібноРозприділити -= вНаявності;
                            }

                            if (!ТовариРядок.РахунокФактура.IsEmpty() && КількістьЯкуПотрібноРозприділити > 0 && ВРезервіРахунокФактура > 0)
                            {
                                decimal вНаявності = КількістьЯкуПотрібноРозприділити >= ВРезервіРахунокФактура ?
                                  ВРезервіРахунокФактура : КількістьЯкуПотрібноРозприділити;

                                ВільніЗалишки_RecordsSet.Record record = new ВільніЗалишки_RecordsSet.Record()
                                {
                                    Income = false,
                                    Owner = ДокументОбєкт.UnigueID.UGuid,
                                    Номенклатура = ТовариРядок.Номенклатура,
                                    ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                                    Склад = !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад,
                                    ВНаявності = вНаявності,
                                    ВРезервіЗіСкладу = вНаявності,
                                    ДокументРезерву = ТовариРядок.РахунокФактура.UnigueID.UGuid
                                };

                                вільніЗалишки_RecordsSet.Records.Add(record);

                                КількістьЯкуПотрібноРозприділити -= record.ВНаявності;
                            }
                        }

                        if (КількістьЯкуПотрібноРозприділити > 0)
                        {
                            ВільніЗалишки_RecordsSet.Record record = new ВільніЗалишки_RecordsSet.Record()
                            {
                                Income = false,
                                Owner = ДокументОбєкт.UnigueID.UGuid,
                                Номенклатура = ТовариРядок.Номенклатура,
                                ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                                Склад = !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад,
                                ВНаявності = КількістьЯкуПотрібноРозприділити
                            };

                            вільніЗалишки_RecordsSet.Records.Add(record);
                        }
                    }
                }

                await вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region Товари на складах

                ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();

                foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        ТовариНаСкладах_RecordsSet.Record record = new ТовариНаСкладах_RecordsSet.Record()
                        {
                            Income = false,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Серія = ТовариРядок.Серія,
                            Склад = !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад,
                            ВНаявності = ТовариРядок.Кількість
                        };

                        товариНаСкладах_RecordsSet.Records.Add(record);
                    }
                }

                await товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region Партії товарів & Продажі

                ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();
                Продажі_RecordsSet продажі_RecordsSet = new Продажі_RecordsSet();

                foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        List<Dictionary<string, object>> listNameRow = СпільніФункції.ОтриматиСписокНаявнихПартій(
                            ДокументОбєкт.Організація, ТовариРядок.Номенклатура, ТовариРядок.ХарактеристикаНоменклатури,
                            ТовариРядок.Серія, ДокументОбєкт.Склад, ТовариРядок.Партія, ДокументОбєкт.UnigueID, ДокументОбєкт.ДатаДок, ТовариРядок.Кількість);

                        if (listNameRow.Count == 0)
                        {
                            throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Немає доступних партій для товару {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        }

                        decimal КількістьЯкуПотрібноСписати = ТовариРядок.Кількість;

                        foreach (Dictionary<string, object> nameRow in listNameRow)
                        {
                            decimal КількістьВПартії = (decimal)nameRow["Кількість"];
                            decimal СобівартістьПартії = (decimal)nameRow["Собівартість"];
                            ПартіяТоварівКомпозит_Pointer ПартіяТоварівКомпозит = new ПартіяТоварівКомпозит_Pointer(nameRow["ПартіяТоварівКомпозит"]);
                            int Рядок = (int)nameRow["Рядок"];

                            decimal КількістьЩоСписується = 0;
                            bool ЗакритиПартію = КількістьЯкуПотрібноСписати >= КількістьВПартії;

                            if (КількістьВПартії >= КількістьЯкуПотрібноСписати)
                            {
                                КількістьЩоСписується = КількістьЯкуПотрібноСписати;
                                КількістьЯкуПотрібноСписати = 0;
                            }
                            else
                            {
                                КількістьЩоСписується = КількістьВПартії;
                                КількістьЯкуПотрібноСписати -= КількістьВПартії;
                            }

                            //ПартіїТоварів
                            {
                                ПартіїТоварів_RecordsSet.Record record = new ПартіїТоварів_RecordsSet.Record()
                                {
                                    Income = false,
                                    Owner = ДокументОбєкт.UnigueID.UGuid,
                                    Організація = ДокументОбєкт.Організація,
                                    ПартіяТоварівКомпозит = ПартіяТоварівКомпозит,
                                    Кількість = КількістьЩоСписується,
                                    Собівартість = ЗакритиПартію ? СобівартістьПартії : 0,
                                    СписанаСобівартість = СобівартістьПартії,
                                    Номенклатура = ТовариРядок.Номенклатура,
                                    ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                                    Серія = ТовариРядок.Серія,
                                    Склад = ДокументОбєкт.Склад,
                                    Рядок = Рядок
                                };

                                партіїТоварів_RecordsSet.Records.Add(record);
                            }

                            //Продажі
                            {
                                Продажі_RecordsSet.Record record = new Продажі_RecordsSet.Record()
                                {
                                    Owner = ДокументОбєкт.UnigueID.UGuid,
                                    Організація = ДокументОбєкт.Організація,
                                    Склад = ДокументОбєкт.Склад,
                                    Контрагент = ДокументОбєкт.Контрагент,
                                    Договір = ДокументОбєкт.Договір,
                                    Номенклатура = ТовариРядок.Номенклатура,
                                    ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                                    Кількість = КількістьЩоСписується,
                                    Сума = ТовариРядок.Ціна * КількістьЩоСписується,
                                    Собівартість = СобівартістьПартії
                                };

                                record.Дохід = record.Сума - (СобівартістьПартії * КількістьЩоСписується);

                                продажі_RecordsSet.Records.Add(record);
                            }

                            if (КількістьЯкуПотрібноСписати == 0)
                                break;
                        }

                        if (КількістьЯкуПотрібноСписати > 0)
                        {
                            throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Невистачило списати {КількістьЯкуПотрібноСписати} товарів");
                        }
                    }
                }

                await партіїТоварів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);
                await продажі_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region РозрахункиЗКлієнтами

                //
                //РозрахункиЗКлієнтами
                //

                РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РозрахункиЗКлієнтами_RecordsSet();

                РозрахункиЗКлієнтами_RecordsSet.Record розрахункиЗКлієнтами_Record = new РозрахункиЗКлієнтами_RecordsSet.Record()
                {
                    Income = true,
                    Owner = ДокументОбєкт.UnigueID.UGuid,
                    Контрагент = ДокументОбєкт.Контрагент,
                    Валюта = ДокументОбєкт.Валюта,
                    Сума = ДокументОбєкт.СумаДокументу
                };

                розрахункиЗКлієнтами_RecordsSet.Records.Add(розрахункиЗКлієнтами_Record);

                await розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, ex.Message);
                await ClearSpend(ДокументОбєкт);

                return false;
            }
        }

        public static async ValueTask ClearSpend(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {
            ЗамовленняКлієнтів_RecordsSet замовленняКлієнтів_RecordsSet = new ЗамовленняКлієнтів_RecordsSet();
            await замовленняКлієнтів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();
            await вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();
            await товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();
            await партіїТоварів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РозрахункиЗКлієнтами_RecordsSet();
            await розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            //
            // Обороти
            //

            Продажі_RecordsSet продажі_RecordsSet = new Продажі_RecordsSet();
            await продажі_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class АктВиконанихРобіт_SpendTheDocument
    {
        public static async ValueTask<bool> Spend(АктВиконанихРобіт_Objest ДокументОбєкт)
        {
            try
            {
                #region Підготовка

                foreach (АктВиконанихРобіт_Послуги_TablePart.Record ПослугиРядок in ДокументОбєкт.Послуги_TablePart.Records)
                {
                    if (ПослугиРядок.Номенклатура.IsEmpty())
                        throw new Exception($"Рядок {ПослугиРядок.НомерРядка}. Не заповнене поле Номенклатура");

                    if (!(ПослугиРядок.Кількість > 0))
                        throw new Exception($"Рядок {ПослугиРядок.НомерРядка}. Кількість має бути більшою 0");
                }

                #endregion

                #region РозрахункиЗКлієнтами

                РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РозрахункиЗКлієнтами_RecordsSet();
                РозрахункиЗКлієнтами_RecordsSet.Record розрахункиЗКлієнтами_Record = new РозрахункиЗКлієнтами_RecordsSet.Record()
                {
                    Income = true,
                    Owner = ДокументОбєкт.UnigueID.UGuid,
                    Контрагент = ДокументОбєкт.Контрагент,
                    Валюта = ДокументОбєкт.Валюта,
                    Сума = ДокументОбєкт.СумаДокументу,

                };

                розрахункиЗКлієнтами_RecordsSet.Records.Add(розрахункиЗКлієнтами_Record);
                await розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region Продажі

                Продажі_RecordsSet продажі_RecordsSet = new Продажі_RecordsSet();

                foreach (АктВиконанихРобіт_Послуги_TablePart.Record ТовариРядок in ДокументОбєкт.Послуги_TablePart.Records)
                {
                    Продажі_RecordsSet.Record record = new Продажі_RecordsSet.Record()
                    {
                        Owner = ДокументОбєкт.UnigueID.UGuid,
                        Організація = ДокументОбєкт.Організація,
                        Контрагент = ДокументОбєкт.Контрагент,
                        Договір = ДокументОбєкт.Договір,
                        Номенклатура = ТовариРядок.Номенклатура,
                        ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                        Кількість = ТовариРядок.Кількість,
                        Сума = ТовариРядок.Сума,
                        Дохід = ТовариРядок.Сума
                    };

                    продажі_RecordsSet.Records.Add(record);
                }

                await продажі_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, ex.Message);
                await ClearSpend(ДокументОбєкт);

                return false;
            }
        }

        public static async ValueTask ClearSpend(АктВиконанихРобіт_Objest ДокументОбєкт)
        {
            РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РозрахункиЗКлієнтами_RecordsSet();
            await розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            //
            // Обороти
            //

            Продажі_RecordsSet продажі_RecordsSet = new Продажі_RecordsSet();
            await продажі_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    #endregion

    #region Закупки

    class ПоступленняТоварівТаПослуг_SpendTheDocument
    {
        public static async ValueTask<bool> Spend(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
        {
            try
            {
                #region Підготовка

                Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();
                Dictionary<int, decimal> ЗамовленняПостачальнику = new Dictionary<int, decimal>();

                foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (ТовариРядок.Номенклатура.IsEmpty())
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");

                    if (!(ТовариРядок.Кількість > 0))
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");

                    СписокНоменклатури.Add(ТовариРядок.НомерРядка, (await ТовариРядок.Номенклатура.GetDirectoryObject())!);

                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        if (!ТовариРядок.ЗамовленняПостачальнику.IsEmpty())
                        {
                            //
                            // Якщо заданий документ Замовлення Постачальнику
                            // Перевірити замовлення
                            //

                            List<Dictionary<string, object>> Замовлення_Список = СпільніФункції.ОтриматиЗамовленняПостачальникуДляТоваруПоДокументу(
                                ТовариРядок.Номенклатура, ТовариРядок.ХарактеристикаНоменклатури,
                                !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад,
                                ТовариРядок.ЗамовленняПостачальнику.UnigueID, ДокументОбєкт.UnigueID, ДокументОбєкт.ДатаДок);

                            if (Замовлення_Список.Count > 0)
                            {
                                ЗамовленняПостачальнику.Add(ТовариРядок.НомерРядка, (decimal)Замовлення_Список[0]["Замовлено"]);

                                if (!(ЗамовленняПостачальнику[ТовариРядок.НомерРядка] > 0))
                                {
                                    throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                        $"Залишок по документу Замовлення {ЗамовленняПостачальнику[ТовариРядок.НомерРядка]}");
                                }
                            }
                            else
                            {
                                throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                    $"Відсутнє замовлення для товару {СписокНоменклатури[ТовариРядок.НомерРядка].Назва} в документі Замовлення постачальнику");
                            }
                        }
                    }
                }

                #endregion

                #region Замовлення постачальникам

                ЗамовленняПостачальникам_RecordsSet замовленняПостачальникам_RecordsSet = new ЗамовленняПостачальникам_RecordsSet();

                foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        if (!ТовариРядок.ЗамовленняПостачальнику.IsEmpty())
                        {
                            decimal Замовлено = ЗамовленняПостачальнику.ContainsKey(ТовариРядок.НомерРядка) ? ЗамовленняПостачальнику[ТовариРядок.НомерРядка] : 0;

                            ЗамовленняПостачальникам_RecordsSet.Record record = new ЗамовленняПостачальникам_RecordsSet.Record()
                            {
                                Income = false,
                                Owner = ДокументОбєкт.UnigueID.UGuid,
                                ЗамовленняПостачальнику = ТовариРядок.ЗамовленняПостачальнику,
                                Номенклатура = ТовариРядок.Номенклатура,
                                ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                                Склад = !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад,
                                Замовлено = Замовлено > ТовариРядок.Кількість ? ТовариРядок.Кількість : Замовлено
                            };

                            замовленняПостачальникам_RecordsSet.Records.Add(record);
                        }
                    }
                }

                await замовленняПостачальникам_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region Товари на складах

                ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();

                foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        ТовариНаСкладах_RecordsSet.Record record = new ТовариНаСкладах_RecordsSet.Record()
                        {
                            Income = true,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Склад = !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад,
                            Серія = ТовариРядок.Серія,
                            ВНаявності = ТовариРядок.Кількість
                        };

                        товариНаСкладах_RecordsSet.Records.Add(record);
                    }
                }

                await товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region Партії товарів

                ПартіяТоварівКомпозит_Pointer ПартіяТоварівКомпозит = await ФункціїДляДокументів.ОтриматиПартіюТоварівКомпозит(
                     ДокументОбєкт.UnigueID.UGuid,
                     Перелічення.ТипДокументуПартіяТоварівКомпозит.ПоступленняТоварівТаПослуг,
                     ДокументОбєкт, null
                );

                ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();

                foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        ПартіїТоварів_RecordsSet.Record record = new ПартіїТоварів_RecordsSet.Record()
                        {
                            Income = true,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Організація = ДокументОбєкт.Організація,
                            ПартіяТоварівКомпозит = ПартіяТоварівКомпозит,
                            Кількість = ТовариРядок.Кількість,
                            Собівартість = ТовариРядок.Ціна,
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Серія = ТовариРядок.Серія,
                            Склад = ДокументОбєкт.Склад,
                            Рядок = ТовариРядок.НомерРядка
                        };

                        партіїТоварів_RecordsSet.Records.Add(record);
                    }
                }

                await партіїТоварів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region ВільніЗалишки

                ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();

                foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        ВільніЗалишки_RecordsSet.Record record = new ВільніЗалишки_RecordsSet.Record()
                        {
                            Income = true,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Склад = !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад,
                            ВНаявності = ТовариРядок.Кількість
                        };

                        вільніЗалишки_RecordsSet.Records.Add(record);
                    }
                }

                await вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region РозрахункиЗПостачальниками

                РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РозрахункиЗПостачальниками_RecordsSet();

                if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ЗакупівляВПостачальника)
                {
                    РозрахункиЗПостачальниками_RecordsSet.Record recordContragent = new РозрахункиЗПостачальниками_RecordsSet.Record()
                    {
                        Income = false,
                        Owner = ДокументОбєкт.UnigueID.UGuid,
                        Контрагент = ДокументОбєкт.Контрагент,
                        Валюта = ДокументОбєкт.Валюта,
                        Сума = ДокументОбєкт.СумаДокументу
                    };

                    розрахункиЗПостачальниками_RecordsSet.Records.Add(recordContragent);
                }

                await розрахункиЗПостачальниками_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region Закупівлі

                Закупівлі_RecordsSet закупівлі_RecordsSet = new Закупівлі_RecordsSet();

                foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    Закупівлі_RecordsSet.Record record = new Закупівлі_RecordsSet.Record()
                    {
                        Owner = ДокументОбєкт.UnigueID.UGuid,
                        Організація = ДокументОбєкт.Організація,
                        Склад = !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад,
                        Контрагент = ДокументОбєкт.Контрагент,
                        Договір = ДокументОбєкт.Договір,
                        Номенклатура = ТовариРядок.Номенклатура,
                        ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                        Кількість = ТовариРядок.Кількість,
                        Сума = ТовариРядок.Сума,
                        Собівартість = Math.Round(ТовариРядок.Сума / ТовариРядок.Кількість, 2)
                    };

                    закупівлі_RecordsSet.Records.Add(record);
                }

                await закупівлі_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, ex.Message);
                await ClearSpend(ДокументОбєкт);

                return false;
            }
        }

        public static async ValueTask ClearSpend(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
        {
            ЗамовленняПостачальникам_RecordsSet замовленняПостачальникам_RecordsSet = new ЗамовленняПостачальникам_RecordsSet();
            await замовленняПостачальникам_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();
            await товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();
            await партіїТоварів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();
            await вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РозрахункиЗПостачальниками_RecordsSet();
            await розрахункиЗПостачальниками_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            //
            // Обороти
            //

            Закупівлі_RecordsSet закупівлі_RecordsSet = new Закупівлі_RecordsSet();
            await закупівлі_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class ЗамовленняПостачальнику_SpendTheDocument
    {
        public static async ValueTask<bool> Spend(ЗамовленняПостачальнику_Objest ДокументОбєкт)
        {
            try
            {
                #region Підготовка

                Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();

                foreach (ЗамовленняПостачальнику_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (ТовариРядок.Номенклатура.IsEmpty())
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");

                    if (!(ТовариРядок.Кількість > 0))
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");

                    СписокНоменклатури.Add(ТовариРядок.НомерРядка, (await ТовариРядок.Номенклатура.GetDirectoryObject())!);
                }

                #endregion

                #region Замовлення постачальникам

                ЗамовленняПостачальникам_RecordsSet замовленняПостачальникам_RecordsSet = new ЗамовленняПостачальникам_RecordsSet();

                foreach (ЗамовленняПостачальнику_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        ЗамовленняПостачальникам_RecordsSet.Record record = new ЗамовленняПостачальникам_RecordsSet.Record()
                        {
                            Income = true,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            ЗамовленняПостачальнику = ДокументОбєкт.GetDocumentPointer(),
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Склад = ДокументОбєкт.Склад,
                            Замовлено = ТовариРядок.Кількість
                        };

                        замовленняПостачальникам_RecordsSet.Records.Add(record);
                    }
                }

                await замовленняПостачальникам_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, ex.Message);
                await ClearSpend(ДокументОбєкт);

                return false;
            }
        }

        public static async ValueTask ClearSpend(ЗамовленняПостачальнику_Objest ДокументОбєкт)
        {
            ЗамовленняПостачальникам_RecordsSet замовленняПостачальникам_RecordsSet = new ЗамовленняПостачальникам_RecordsSet();
            await замовленняПостачальникам_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class ПоверненняТоварівВідКлієнта_SpendTheDocument
    {
        private static List<Dictionary<string, object>> ОтриматиПартіїТоваруЗДокументуРеалізації(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт,
            ПоверненняТоварівВідКлієнта_Товари_TablePart.Record ТовариРядок)
        {
            string query = $@"
SELECT 
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.ПартіяТоварівКомпозит} AS ПартіяТоварівКомпозит,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Рядок} AS Рядок,
    Довідник_ПартіяТоварівКомпозит.{ПартіяТоварівКомпозит_Const.Дата} AS ПартіяТоварівКомпозит_Дата,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Кількість} AS Кількість,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Собівартість} AS Собівартість,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.СписанаСобівартість} AS СписанаСобівартість
FROM 
    {ПартіїТоварів_Const.TABLE} AS Рег_ПартіїТоварів
        
    LEFT JOIN {ПартіяТоварівКомпозит_Const.TABLE} AS Довідник_ПартіяТоварівКомпозит ON Довідник_ПартіяТоварівКомпозит.uid = 
        Рег_ПартіїТоварів.{ПартіїТоварів_Const.ПартіяТоварівКомпозит}

WHERE
    Рег_ПартіїТоварів.Owner = @ДокументРеалізації AND
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Організація} = @Організація AND
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Номенклатура} = @Товар AND
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.ХарактеристикаНоменклатури} = @Характеристика AND
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Серія} = @Серія AND
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Склад} = @Склад

ORDER BY ПартіяТоварівКомпозит_Дата ASC
";

            Dictionary<string, object> paramQuery = new Dictionary<string, object>()
            {
                { "ДокументРеалізації", ТовариРядок.ДокументРеалізації.UnigueID.UGuid },
                { "Організація", ДокументОбєкт.Організація.UnigueID.UGuid },
                { "Товар", ТовариРядок.Номенклатура.UnigueID.UGuid },
                { "Характеристика", ТовариРядок.ХарактеристикаНоменклатури.UnigueID.UGuid },
                { "Серія", ТовариРядок.Серія.UnigueID.UGuid },
                { "Склад", ДокументОбєкт.Склад.UnigueID.UGuid }
            };

            string[] columnsName;
            List<Dictionary<string, object>> listNameRow;

            Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listNameRow);

            return listNameRow;
        }

        public static async ValueTask<bool> Spend(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
        {
            try
            {
                #region Підготовка

                Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();

                foreach (ПоверненняТоварівВідКлієнта_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (ТовариРядок.Номенклатура.IsEmpty())
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");

                    if (!(ТовариРядок.Кількість > 0))
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");

                    СписокНоменклатури.Add(ТовариРядок.НомерРядка, (await ТовариРядок.Номенклатура.GetDirectoryObject())!);
                }

                #endregion

                #region ВільніЗалишки

                ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();

                foreach (ПоверненняТоварівВідКлієнта_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        ВільніЗалишки_RecordsSet.Record record = new ВільніЗалишки_RecordsSet.Record()
                        {
                            Income = true,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Склад = ДокументОбєкт.Склад,
                            ВНаявності = ТовариРядок.Кількість
                        };

                        вільніЗалишки_RecordsSet.Records.Add(record);
                    }
                }

                await вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region Товари на складах

                ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();

                foreach (ПоверненняТоварівВідКлієнта_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        ТовариНаСкладах_RecordsSet.Record record = new ТовариНаСкладах_RecordsSet.Record()
                        {
                            Income = true,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Серія = ТовариРядок.Серія,
                            Склад = ДокументОбєкт.Склад,
                            ВНаявності = ТовариРядок.Кількість
                        };

                        товариНаСкладах_RecordsSet.Records.Add(record);
                    }
                }

                await товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region Партії товарів  & Продажі

                ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();
                Продажі_RecordsSet продажі_RecordsSet = new Продажі_RecordsSet();

                foreach (ПоверненняТоварівВідКлієнта_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        List<Dictionary<string, object>> listNameRow = ОтриматиПартіїТоваруЗДокументуРеалізації(ДокументОбєкт, ТовариРядок);

                        if (listNameRow.Count == 0)
                        {
                            throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Немає доступних партій для товару {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        }

                        decimal КількістьЯкуПотрібноПовернути = ТовариРядок.Кількість;

                        foreach (Dictionary<string, object> nameRow in listNameRow)
                        {
                            decimal КількістьВПартії = (decimal)nameRow["Кількість"];
                            decimal СобівартістьПартії = (decimal)nameRow["Собівартість"];
                            decimal СписанаСобівартістьПартії = (decimal)nameRow["СписанаСобівартість"];
                            ПартіяТоварівКомпозит_Pointer ПартіяТоварівКомпозит = new ПартіяТоварівКомпозит_Pointer(nameRow["ПартіяТоварівКомпозит"]);
                            int Рядок = (int)nameRow["Рядок"];

                            decimal КількістьЩоПовертається = 0;

                            if (КількістьВПартії <= КількістьЯкуПотрібноПовернути)
                            {
                                КількістьЩоПовертається = КількістьВПартії;
                                КількістьЯкуПотрібноПовернути -= КількістьВПартії;
                            }
                            else
                            {
                                КількістьЩоПовертається = КількістьЯкуПотрібноПовернути;
                                КількістьЯкуПотрібноПовернути = 0;
                            }

                            //ПартіїТоварів
                            {
                                ПартіїТоварів_RecordsSet.Record record = new ПартіїТоварів_RecordsSet.Record()
                                {
                                    Income = true,
                                    Owner = ДокументОбєкт.UnigueID.UGuid,
                                    Організація = ДокументОбєкт.Організація,
                                    ПартіяТоварівКомпозит = ПартіяТоварівКомпозит,
                                    Кількість = КількістьЩоПовертається,
                                    Собівартість = СобівартістьПартії,
                                    СписанаСобівартість = СписанаСобівартістьПартії,
                                    Номенклатура = ТовариРядок.Номенклатура,
                                    ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                                    Серія = ТовариРядок.Серія,
                                    Склад = ДокументОбєкт.Склад,
                                    Рядок = Рядок
                                };

                                партіїТоварів_RecordsSet.Records.Add(record);
                            }

                            //Продажі
                            {
                                Продажі_RecordsSet.Record record = new Продажі_RecordsSet.Record()
                                {
                                    Owner = ДокументОбєкт.UnigueID.UGuid,
                                    Організація = ДокументОбєкт.Організація,
                                    Склад = ДокументОбєкт.Склад,
                                    Контрагент = ДокументОбєкт.Контрагент,
                                    Договір = ДокументОбєкт.Договір,
                                    Номенклатура = ТовариРядок.Номенклатура,
                                    ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                                    Кількість = -КількістьЩоПовертається,
                                    Сума = -(ТовариРядок.Ціна * КількістьЩоПовертається),
                                    Дохід = -(ТовариРядок.Ціна * КількістьЩоПовертається - СобівартістьПартії * КількістьЩоПовертається),
                                    Собівартість = СобівартістьПартії
                                };

                                продажі_RecordsSet.Records.Add(record);
                            }

                            if (КількістьЯкуПотрібноПовернути == 0)
                                break;
                        }

                        if (КількістьЯкуПотрібноПовернути > 0)
                        {
                            throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Невистачило повернути {КількістьЯкуПотрібноПовернути} товарів");
                        }
                    }
                }

                await партіїТоварів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);
                await продажі_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region РозрахункиЗКлієнтами

                РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РозрахункиЗКлієнтами_RecordsSet();
                РозрахункиЗКлієнтами_RecordsSet.Record розрахункиЗКлієнтами_Record = new РозрахункиЗКлієнтами_RecordsSet.Record()
                {
                    Income = false,
                    Owner = ДокументОбєкт.UnigueID.UGuid,
                    Контрагент = ДокументОбєкт.Контрагент,
                    Валюта = ДокументОбєкт.Валюта,
                    Сума = ДокументОбєкт.СумаДокументу
                };

                розрахункиЗКлієнтами_RecordsSet.Records.Add(розрахункиЗКлієнтами_Record);
                await розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, ex.Message);
                await ClearSpend(ДокументОбєкт);

                return false;
            }
        }

        public static async ValueTask ClearSpend(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
        {
            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();
            await вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();
            await товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();
            await партіїТоварів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РозрахункиЗКлієнтами_RecordsSet();
            await розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            //
            // Обороти
            //

            Продажі_RecordsSet продажі_RecordsSet = new Продажі_RecordsSet();
            await продажі_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class ПоверненняТоварівПостачальнику_SpendTheDocument
    {
        private static List<Dictionary<string, object>> ОтриматиПартіїТоваруЗДокументуПоступлення(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт,
            ПоверненняТоварівПостачальнику_Товари_TablePart.Record ТовариРядок)
        {
            string query = $@"
SELECT 
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.ПартіяТоварівКомпозит} AS ПартіяТоварівКомпозит,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Рядок} AS Рядок,
    Довідник_ПартіяТоварівКомпозит.{ПартіяТоварівКомпозит_Const.Дата} AS ПартіяТоварівКомпозит_Дата,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Кількість} AS Кількість,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Собівартість} AS Собівартість
FROM 
    {ПартіїТоварів_Const.TABLE} AS Рег_ПартіїТоварів
        
    LEFT JOIN {ПартіяТоварівКомпозит_Const.TABLE} AS Довідник_ПартіяТоварівКомпозит ON Довідник_ПартіяТоварівКомпозит.uid = 
        Рег_ПартіїТоварів.{ПартіїТоварів_Const.ПартіяТоварівКомпозит}

WHERE
    Рег_ПартіїТоварів.Owner = @ДокументПоступлення AND
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Організація} = @Організація AND
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Номенклатура} = @Товар AND
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.ХарактеристикаНоменклатури} = @Характеристика AND
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Серія} = @Серія AND
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Склад} = @Склад

ORDER BY ПартіяТоварівКомпозит_Дата ASC
";

            Dictionary<string, object> paramQuery = new Dictionary<string, object>()
            {
                { "ДокументПоступлення", ТовариРядок.ДокументПоступлення.UnigueID.UGuid },
                { "Організація", ДокументОбєкт.Організація.UnigueID.UGuid },
                { "Товар", ТовариРядок.Номенклатура.UnigueID.UGuid },
                { "Характеристика", ТовариРядок.ХарактеристикаНоменклатури.UnigueID.UGuid },
                { "Серія", ТовариРядок.Серія.UnigueID.UGuid },
                { "Склад", ДокументОбєкт.Склад.UnigueID.UGuid }
            };

            string[] columnsName;
            List<Dictionary<string, object>> listNameRow;

            Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listNameRow);

            return listNameRow;
        }

        public static async ValueTask<bool> Spend(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
        {
            try
            {
                #region Підготовка

                Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();

                foreach (ПоверненняТоварівПостачальнику_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (ТовариРядок.Номенклатура.IsEmpty())
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");

                    if (!(ТовариРядок.Кількість > 0))
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");

                    СписокНоменклатури.Add(ТовариРядок.НомерРядка, (await ТовариРядок.Номенклатура.GetDirectoryObject())!);
                }

                #endregion

                #region Товари на складах

                ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();

                foreach (ПоверненняТоварівПостачальнику_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        ТовариНаСкладах_RecordsSet.Record record = new ТовариНаСкладах_RecordsSet.Record()
                        {
                            Income = false,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Серія = ТовариРядок.Серія,
                            Склад = ДокументОбєкт.Склад,
                            ВНаявності = ТовариРядок.Кількість
                        };

                        товариНаСкладах_RecordsSet.Records.Add(record);
                    }
                }

                await товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region Партії товарів

                ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();

                foreach (ПоверненняТоварівПостачальнику_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        List<Dictionary<string, object>> listNameRow = ОтриматиПартіїТоваруЗДокументуПоступлення(ДокументОбєкт, ТовариРядок);

                        if (listNameRow.Count == 0)
                        {
                            throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Немає доступних партій для товару {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        }

                        decimal КількістьЯкуПотрібноПовернути = ТовариРядок.Кількість;

                        if (listNameRow.Count > 0)
                        {
                            Dictionary<string, object> nameRow = listNameRow[0];

                            decimal КількістьВПартії = (decimal)nameRow["Кількість"];
                            decimal СобівартістьПартії = (decimal)nameRow["Собівартість"];
                            ПартіяТоварівКомпозит_Pointer ПартіяТоварівКомпозит = new ПартіяТоварівКомпозит_Pointer(nameRow["ПартіяТоварівКомпозит"]);
                            int Рядок = (int)nameRow["Рядок"];

                            if (КількістьЯкуПотрібноПовернути <= КількістьВПартії)
                            {
                                ПартіїТоварів_RecordsSet.Record record = new ПартіїТоварів_RecordsSet.Record()
                                {
                                    Income = false,
                                    Owner = ДокументОбєкт.UnigueID.UGuid,
                                    Організація = ДокументОбєкт.Організація,
                                    ПартіяТоварівКомпозит = ПартіяТоварівКомпозит,
                                    Кількість = КількістьЯкуПотрібноПовернути,
                                    Собівартість = КількістьЯкуПотрібноПовернути == КількістьВПартії ? СобівартістьПартії : 0,
                                    СписанаСобівартість = СобівартістьПартії,
                                    Номенклатура = ТовариРядок.Номенклатура,
                                    ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                                    Серія = ТовариРядок.Серія,
                                    Склад = ДокументОбєкт.Склад,
                                    Рядок = Рядок
                                };

                                партіїТоварів_RecordsSet.Records.Add(record);
                            }
                            else
                            {
                                throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                    $"Кількість в партії менша чим кількість яку повертають для товару {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                            }
                        }
                        else
                        {
                            throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Не знайдені партії для товару {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        }
                    }
                }

                await партіїТоварів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region ВільніЗалишки

                ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();

                foreach (ПоверненняТоварівПостачальнику_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        ВільніЗалишки_RecordsSet.Record record = new ВільніЗалишки_RecordsSet.Record()
                        {
                            Income = false,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Склад = ДокументОбєкт.Склад,
                            ВНаявності = ТовариРядок.Кількість
                        };

                        вільніЗалишки_RecordsSet.Records.Add(record);
                    }
                }

                await вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region РозрахункиЗПостачальниками

                РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РозрахункиЗПостачальниками_RecordsSet();
                РозрахункиЗПостачальниками_RecordsSet.Record розрахункиЗПостачальниками_Record = new РозрахункиЗПостачальниками_RecordsSet.Record()
                {
                    Income = false,
                    Owner = ДокументОбєкт.UnigueID.UGuid,
                    Контрагент = ДокументОбєкт.Контрагент,
                    Валюта = ДокументОбєкт.Валюта,
                    Сума = ДокументОбєкт.СумаДокументу
                };

                розрахункиЗПостачальниками_RecordsSet.Records.Add(розрахункиЗПостачальниками_Record);
                await розрахункиЗПостачальниками_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region Закупівлі

                Закупівлі_RecordsSet закупівлі_RecordsSet = new Закупівлі_RecordsSet();

                foreach (ПоверненняТоварівПостачальнику_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    Закупівлі_RecordsSet.Record record = new Закупівлі_RecordsSet.Record()
                    {
                        Owner = ДокументОбєкт.UnigueID.UGuid,
                        Організація = ДокументОбєкт.Організація,
                        Склад = ДокументОбєкт.Склад,
                        Контрагент = ДокументОбєкт.Контрагент,
                        Договір = ДокументОбєкт.Договір,
                        Номенклатура = ТовариРядок.Номенклатура,
                        ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                        Кількість = -ТовариРядок.Кількість,
                        Сума = -ТовариРядок.Сума,
                        Собівартість = Math.Round(ТовариРядок.Сума / ТовариРядок.Кількість, 2)
                    };

                    закупівлі_RecordsSet.Records.Add(record);
                }

                await закупівлі_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, ex.Message);
                await ClearSpend(ДокументОбєкт);

                return false;
            }
        }

        public static async ValueTask ClearSpend(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
        {
            ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();
            await товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();
            await партіїТоварів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();
            await вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РозрахункиЗПостачальниками_RecordsSet();
            await розрахункиЗПостачальниками_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            //
            // Обороти
            //

            Закупівлі_RecordsSet закупівлі_RecordsSet = new Закупівлі_RecordsSet();
            await закупівлі_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    #endregion

    #region Каса

    class ПрихіднийКасовийОрдер_SpendTheDocument
    {
        public static async ValueTask<bool> Spend(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            try
            {
                #region Підготовка

                #endregion

                #region РозрахункиЗКлієнтами

                РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РозрахункиЗКлієнтами_RecordsSet();

                if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ПоступленняОплатиВідКлієнта)
                {
                    РозрахункиЗКлієнтами_RecordsSet.Record record_Клієнт = new РозрахункиЗКлієнтами_RecordsSet.Record()
                    {
                        Income = false,
                        Owner = ДокументОбєкт.UnigueID.UGuid,
                        Контрагент = ДокументОбєкт.Контрагент,
                        Валюта = ДокументОбєкт.Валюта,
                        Сума = ДокументОбєкт.СумаДокументу
                    };

                    розрахункиЗКлієнтами_RecordsSet.Records.Add(record_Клієнт);
                }

                await розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region РозрахункиЗПостачальниками

                РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РозрахункиЗПостачальниками_RecordsSet();

                if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ПоверненняКоштівПостачальнику)
                {
                    РозрахункиЗПостачальниками_RecordsSet.Record record_Постачальник = new РозрахункиЗПостачальниками_RecordsSet.Record()
                    {
                        Income = false,
                        Owner = ДокументОбєкт.UnigueID.UGuid,
                        Контрагент = ДокументОбєкт.Контрагент,
                        Валюта = ДокументОбєкт.Валюта,
                        Сума = ДокументОбєкт.СумаДокументу
                    };

                    розрахункиЗПостачальниками_RecordsSet.Records.Add(record_Постачальник);
                }

                await розрахункиЗПостачальниками_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region РухКоштів

                РухКоштів_RecordsSet рухКоштів_RecordsSet = new РухКоштів_RecordsSet();

                //Списання коштів з КасаВідправник
                if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ПоступленняКоштівЗІншоїКаси)
                {
                    РухКоштів_RecordsSet.Record record_ІншаКаса = new РухКоштів_RecordsSet.Record()
                    {
                        Income = false,
                        Owner = ДокументОбєкт.UnigueID.UGuid,
                        Організація = ДокументОбєкт.Організація,
                        Каса = ДокументОбєкт.КасаВідправник,
                        Валюта = ДокументОбєкт.Валюта,
                        Сума = ДокументОбєкт.СумаДокументу
                    };

                    рухКоштів_RecordsSet.Records.Add(record_ІншаКаса);
                }

                //Списання коштів з банківського рахунку
                if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ПоступленняКоштівЗБанку)
                {
                    // ...
                }

                //Поступлення коштів в касу
                РухКоштів_RecordsSet.Record record_РухКоштів = new РухКоштів_RecordsSet.Record()
                {
                    Income = true,
                    Owner = ДокументОбєкт.UnigueID.UGuid,
                    Організація = ДокументОбєкт.Організація,
                    Каса = ДокументОбєкт.Каса,
                    Валюта = ДокументОбєкт.Валюта,
                    Сума = ДокументОбєкт.СумаДокументу
                };

                рухКоштів_RecordsSet.Records.Add(record_РухКоштів);
                await рухКоштів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, ex.Message);
                await ClearSpend(ДокументОбєкт);

                return false;
            }
        }

        public static async ValueTask ClearSpend(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РозрахункиЗПостачальниками_RecordsSet();
            await розрахункиЗПостачальниками_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РозрахункиЗКлієнтами_RecordsSet();
            await розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            РухКоштів_RecordsSet рухКоштів_RecordsSet = new РухКоштів_RecordsSet();
            await рухКоштів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class РозхіднийКасовийОрдер_SpendTheDocument
    {
        public static async ValueTask<bool> Spend(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            try
            {
                #region Підготовка

                #endregion

                #region РозрахункиЗКлієнтами

                РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РозрахункиЗКлієнтами_RecordsSet();

                if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ПоверненняОплатиКлієнту)
                {
                    РозрахункиЗКлієнтами_RecordsSet.Record record_Клієнт = new РозрахункиЗКлієнтами_RecordsSet.Record()
                    {
                        Income = false,
                        Owner = ДокументОбєкт.UnigueID.UGuid,
                        Контрагент = ДокументОбєкт.Контрагент,
                        Валюта = ДокументОбєкт.Валюта,
                        Сума = ДокументОбєкт.СумаДокументу
                    };

                    розрахункиЗКлієнтами_RecordsSet.Records.Add(record_Клієнт);
                }

                await розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region РозрахункиЗПостачальниками

                РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РозрахункиЗПостачальниками_RecordsSet();

                if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ОплатаПостачальнику)
                {
                    РозрахункиЗПостачальниками_RecordsSet.Record record_Постачальник = new РозрахункиЗПостачальниками_RecordsSet.Record()
                    {
                        Income = true,
                        Owner = ДокументОбєкт.UnigueID.UGuid,
                        Контрагент = ДокументОбєкт.Контрагент,
                        Валюта = ДокументОбєкт.Валюта,
                        Сума = ДокументОбєкт.СумаДокументу
                    };

                    розрахункиЗПостачальниками_RecordsSet.Records.Add(record_Постачальник);
                }

                await розрахункиЗПостачальниками_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region РухКоштів

                РухКоштів_RecordsSet рухКоштів_RecordsSet = new РухКоштів_RecordsSet();

                //Поступлення коштів в КасаОтримувач
                if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ВидачаКоштівВІншуКасу)
                {
                    РухКоштів_RecordsSet.Record record_ІншаКаса = new РухКоштів_RecordsSet.Record()
                    {
                        Income = true,
                        Owner = ДокументОбєкт.UnigueID.UGuid,
                        Організація = ДокументОбєкт.Організація,
                        Каса = ДокументОбєкт.КасаОтримувач,
                        Валюта = ДокументОбєкт.Валюта,
                        Сума = ДокументОбєкт.СумаДокументу
                    };

                    рухКоштів_RecordsSet.Records.Add(record_ІншаКаса);
                }

                //Поступлення коштів на банківський рахунок
                if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ЗдачаКоштівВБанк)
                {
                    // ...
                }

                //Списання коштів з каси
                РухКоштів_RecordsSet.Record record_РухКоштів = new РухКоштів_RecordsSet.Record()
                {
                    Income = false,
                    Owner = ДокументОбєкт.UnigueID.UGuid,
                    Організація = ДокументОбєкт.Організація,
                    Каса = ДокументОбєкт.Каса,
                    Валюта = ДокументОбєкт.Валюта,
                    Сума = ДокументОбєкт.СумаДокументу
                };

                рухКоштів_RecordsSet.Records.Add(record_РухКоштів);
                await рухКоштів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, ex.Message);
                await ClearSpend(ДокументОбєкт);

                return false;
            }
        }

        public static async ValueTask ClearSpend(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РозрахункиЗПостачальниками_RecordsSet();
            await розрахункиЗПостачальниками_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РозрахункиЗКлієнтами_RecordsSet();
            await розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            РухКоштів_RecordsSet рухКоштів_RecordsSet = new РухКоштів_RecordsSet();
            await рухКоштів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    #endregion

    #region Інші

    class ПереміщенняТоварів_SpendTheDocument
    {
        public static async ValueTask<bool> Spend(ПереміщенняТоварів_Objest ДокументОбєкт)
        {
            try
            {
                #region Підготовка

                Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();
                Dictionary<int, decimal> ЗалишокНоменклатури = new Dictionary<int, decimal>();
                Dictionary<int, decimal> РезервНоменклатури = new Dictionary<int, decimal>();

                foreach (ПереміщенняТоварів_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (ТовариРядок.Номенклатура.IsEmpty())
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");

                    if (!(ТовариРядок.Кількість > 0))
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");

                    СписокНоменклатури.Add(ТовариРядок.НомерРядка, (await ТовариРядок.Номенклатура.GetDirectoryObject())!);

                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        //
                        //Для товарів отримуємо залишки
                        //

                        List<Dictionary<string, object>> Залишок_Список = СпільніФункції.ОтриматиЗалишкиТоваруНаСкладі(
                            ТовариРядок.Номенклатура, ТовариРядок.ХарактеристикаНоменклатури, ТовариРядок.Серія,
                            ДокументОбєкт.СкладВідправник, ДокументОбєкт.UnigueID, ДокументОбєкт.ДатаДок);

                        ЗалишокНоменклатури.Add(ТовариРядок.НомерРядка, Залишок_Список.Count > 0 ? (decimal)Залишок_Список[0]["ВНаявності"] : 0);

                        if (ЗалишокНоменклатури[ТовариРядок.НомерРядка] > 0)
                        {
                            if (ЗалишокНоменклатури[ТовариРядок.НомерРядка] < ТовариРядок.Кількість)
                            {
                                throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                    $"Залишок товару {ЗалишокНоменклатури[ТовариРядок.НомерРядка]}, потрібно {ТовариРядок.Кількість}");
                            }

                            //
                            // Рахуємо всі резерви
                            //

                            List<Dictionary<string, object>> Резерв_Список = СпільніФункції.ОтриматиРезервТоваруНаСкладі(
                            ТовариРядок.Номенклатура, ТовариРядок.ХарактеристикаНоменклатури,
                            ДокументОбєкт.СкладВідправник, ДокументОбєкт.UnigueID, ДокументОбєкт.ДатаДок);

                            РезервНоменклатури.Add(ТовариРядок.НомерРядка, Резерв_Список.Count > 0 ? (decimal)Резерв_Список[0]["ВРезервіЗіСкладу"] : 0);

                            if (РезервНоменклатури[ТовариРядок.НомерРядка] > 0)
                            {
                                if (ЗалишокНоменклатури[ТовариРядок.НомерРядка] - РезервНоменклатури[ТовариРядок.НомерРядка] < ТовариРядок.Кількість)
                                {
                                    throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                        $"Залишок товару {ЗалишокНоменклатури[ТовариРядок.НомерРядка]}, " +
                                        $"в резерві {РезервНоменклатури[ТовариРядок.НомерРядка]}, " +
                                        $"потрібно {ТовариРядок.Кількість}");
                                }
                            }
                        }
                        else
                        {
                            throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Відсутній товар {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        }
                    }
                }

                #endregion

                #region Товари на складах

                ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();

                foreach (ПереміщенняТоварів_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        //
                        //СкладВідправник
                        //

                        ТовариНаСкладах_RecordsSet.Record record1 = new ТовариНаСкладах_RecordsSet.Record()
                        {
                            Income = false,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Склад = ДокументОбєкт.СкладВідправник,
                            Серія = ТовариРядок.Серія,
                            ВНаявності = ТовариРядок.Кількість
                        };

                        товариНаСкладах_RecordsSet.Records.Add(record1);

                        //
                        //СкладОтримувач
                        //

                        ТовариНаСкладах_RecordsSet.Record record2 = new ТовариНаСкладах_RecordsSet.Record()
                        {
                            Income = true,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Склад = ДокументОбєкт.СкладОтримувач,
                            Серія = ТовариРядок.Серія,
                            ВНаявності = ТовариРядок.Кількість
                        };

                        товариНаСкладах_RecordsSet.Records.Add(record2);
                    }
                }

                await товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region ВільніЗалишки

                ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();

                foreach (ПереміщенняТоварів_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        //
                        //СкладВідправник
                        //

                        ВільніЗалишки_RecordsSet.Record record1 = new ВільніЗалишки_RecordsSet.Record()
                        {
                            Income = false,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Склад = ДокументОбєкт.СкладВідправник,
                            ВНаявності = ТовариРядок.Кількість
                        };

                        вільніЗалишки_RecordsSet.Records.Add(record1);

                        //
                        //СкладОтримувач
                        //

                        ВільніЗалишки_RecordsSet.Record record2 = new ВільніЗалишки_RecordsSet.Record()
                        {
                            Income = true,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Склад = ДокументОбєкт.СкладОтримувач,
                            ВНаявності = ТовариРядок.Кількість
                        };

                        вільніЗалишки_RecordsSet.Records.Add(record2);
                    }
                }

                await вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region Партії товарів

                ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();

                foreach (ПереміщенняТоварів_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        List<Dictionary<string, object>> listNameRow = СпільніФункції.ОтриматиСписокНаявнихПартій(
                            ДокументОбєкт.Організація, ТовариРядок.Номенклатура, ТовариРядок.ХарактеристикаНоменклатури,
                            ТовариРядок.Серія, ДокументОбєкт.СкладВідправник, ТовариРядок.Партія, ДокументОбєкт.UnigueID, ДокументОбєкт.ДатаДок, ТовариРядок.Кількість);

                        if (listNameRow.Count == 0)
                        {
                            throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Немає доступних партій для товару {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        }

                        decimal КількістьЯкуПотрібноСписати = ТовариРядок.Кількість;

                        foreach (Dictionary<string, object> nameRow in listNameRow)
                        {
                            decimal КількістьВПартії = (decimal)nameRow["Кількість"];
                            decimal СобівартістьПартії = (decimal)nameRow["Собівартість"];
                            ПартіяТоварівКомпозит_Pointer ПартіяТоварівКомпозит = new ПартіяТоварівКомпозит_Pointer(nameRow["ПартіяТоварівКомпозит"]);
                            int Рядок = (int)nameRow["Рядок"];

                            decimal КількістьЩоСписується = 0;
                            //bool ЗакритиПартію = КількістьЯкуПотрібноСписати >= КількістьВПартії;

                            if (КількістьВПартії >= КількістьЯкуПотрібноСписати)
                            {
                                КількістьЩоСписується = КількістьЯкуПотрібноСписати;
                                КількістьЯкуПотрібноСписати = 0;
                            }
                            else
                            {
                                КількістьЩоСписується = КількістьВПартії;
                                КількістьЯкуПотрібноСписати -= КількістьВПартії;
                            }

                            ПартіїТоварів_RecordsSet.Record record1 = new ПартіїТоварів_RecordsSet.Record()
                            {
                                Income = false,
                                Owner = ДокументОбєкт.UnigueID.UGuid,
                                Організація = ДокументОбєкт.Організація,
                                ПартіяТоварівКомпозит = ПартіяТоварівКомпозит,
                                Кількість = КількістьЩоСписується,
                                Собівартість = СобівартістьПартії,
                                СписанаСобівартість = СобівартістьПартії,
                                Номенклатура = ТовариРядок.Номенклатура,
                                ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                                Серія = ТовариРядок.Серія,
                                Склад = ДокументОбєкт.СкладВідправник,
                                Рядок = Рядок
                            };

                            партіїТоварів_RecordsSet.Records.Add(record1);

                            ПартіїТоварів_RecordsSet.Record record2 = new ПартіїТоварів_RecordsSet.Record()
                            {
                                Income = true,
                                Owner = ДокументОбєкт.UnigueID.UGuid,
                                Організація = ДокументОбєкт.Організація,
                                ПартіяТоварівКомпозит = ПартіяТоварівКомпозит,
                                Кількість = КількістьЩоСписується,
                                Собівартість = record1.Собівартість,
                                СписанаСобівартість = 0,
                                Номенклатура = ТовариРядок.Номенклатура,
                                ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                                Серія = ТовариРядок.Серія,
                                Склад = ДокументОбєкт.СкладОтримувач,
                                Рядок = Рядок
                            };

                            партіїТоварів_RecordsSet.Records.Add(record2);

                            if (КількістьЯкуПотрібноСписати == 0)
                                break;
                        }

                        if (КількістьЯкуПотрібноСписати > 0)
                        {
                            throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Невистачило списати {КількістьЯкуПотрібноСписати} товарів");
                        }
                    }
                }

                await партіїТоварів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, ex.Message);
                await ClearSpend(ДокументОбєкт);

                return false;
            }
        }

        public static async ValueTask ClearSpend(ПереміщенняТоварів_Objest ДокументОбєкт)
        {
            ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();
            await товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();
            await вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();
            await партіїТоварів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class ВстановленняЦінНоменклатури_SpendTheDocument
    {
        public static async ValueTask<bool> Spend(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
        {
            try
            {
                #region Рух по регістрах

                ЦіниНоменклатури_RecordsSet ціниНоменклатури_RecordsSet = new ЦіниНоменклатури_RecordsSet();

                foreach (ВстановленняЦінНоменклатури_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (Товари_Record.Ціна > 0)
                    {
                        ЦіниНоменклатури_RecordsSet.Record record = new ЦіниНоменклатури_RecordsSet.Record()
                        {
                            Номенклатура = Товари_Record.Номенклатура,
                            ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури,
                            ВидЦіни = Товари_Record.ВидЦіни,
                            Ціна = Товари_Record.Ціна,
                            Пакування = Товари_Record.Пакування,
                            Валюта = ДокументОбєкт.Валюта
                        };

                        ціниНоменклатури_RecordsSet.Records.Add(record);
                    }
                }

                await ціниНоменклатури_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, ex.Message);
                await ClearSpend(ДокументОбєкт);

                return false;
            }
        }

        public static async ValueTask ClearSpend(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
        {
            ЦіниНоменклатури_RecordsSet ціниНоменклатури_RecordsSet = new ЦіниНоменклатури_RecordsSet();
            await ціниНоменклатури_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class ВведенняЗалишків_SpendTheDocument
    {
        public static async ValueTask<bool> Spend(ВведенняЗалишків_Objest ДокументОбєкт)
        {
            try
            {
                #region Підготовка

                Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();

                foreach (ВведенняЗалишків_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (ТовариРядок.Номенклатура.IsEmpty())
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");

                    if (!(ТовариРядок.Кількість > 0))
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");

                    СписокНоменклатури.Add(ТовариРядок.НомерРядка, (await ТовариРядок.Номенклатура.GetDirectoryObject())!);
                }

                #endregion

                #region Товари на складах

                ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();

                foreach (ВведенняЗалишків_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        ТовариНаСкладах_RecordsSet.Record record = new ТовариНаСкладах_RecordsSet.Record()
                        {
                            Income = true,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Серія = ТовариРядок.Серія,
                            Склад = ДокументОбєкт.Склад,
                            ВНаявності = ТовариРядок.Кількість
                        };

                        товариНаСкладах_RecordsSet.Records.Add(record);
                    }
                }

                await товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region ВільніЗалишки

                ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();

                foreach (ВведенняЗалишків_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        ВільніЗалишки_RecordsSet.Record record = new ВільніЗалишки_RecordsSet.Record()
                        {
                            Income = true,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Склад = ДокументОбєкт.Склад,
                            ВНаявності = ТовариРядок.Кількість
                        };

                        вільніЗалишки_RecordsSet.Records.Add(record);
                    }
                }

                await вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region Партії товарів

                ПартіяТоварівКомпозит_Pointer ПартіяТоварівКомпозит = await StorageAndTrade.ФункціїДляДокументів.ОтриматиПартіюТоварівКомпозит(
                     ДокументОбєкт.UnigueID.UGuid,
                     Перелічення.ТипДокументуПартіяТоварівКомпозит.ВведенняЗалишків,
                     null, ДокументОбєкт
                );

                ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();

                foreach (ВведенняЗалишків_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        ПартіїТоварів_RecordsSet.Record record = new ПартіїТоварів_RecordsSet.Record()
                        {
                            Income = true,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Організація = ДокументОбєкт.Організація,
                            ПартіяТоварівКомпозит = ПартіяТоварівКомпозит,
                            Кількість = ТовариРядок.Кількість,
                            Собівартість = ТовариРядок.Ціна,
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Серія = ТовариРядок.Серія,
                            Склад = ДокументОбєкт.Склад,
                            Рядок = ТовариРядок.НомерРядка
                        };

                        партіїТоварів_RecordsSet.Records.Add(record);
                    }
                }

                await партіїТоварів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region РухКоштів

                РухКоштів_RecordsSet рухКоштів_RecordsSet = new РухКоштів_RecordsSet();

                foreach (ВведенняЗалишків_Каси_TablePart.Record Каси_Record in ДокументОбєкт.Каси_TablePart.Records)
                {
                    Валюти_Pointer валютаКаси = !Каси_Record.Каса.IsEmpty() ? (await Каси_Record.Каса.GetDirectoryObject())!.Валюта : ДокументОбєкт.Валюта;

                    РухКоштів_RecordsSet.Record record_Каса = new РухКоштів_RecordsSet.Record()
                    {
                        Income = true,
                        Owner = ДокументОбєкт.UnigueID.UGuid,
                        Організація = ДокументОбєкт.Організація,
                        Каса = Каси_Record.Каса,
                        Валюта = валютаКаси,
                        Сума = Каси_Record.Сума
                    };

                    рухКоштів_RecordsSet.Records.Add(record_Каса);
                }

                await рухКоштів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region РозрахункиЗКлієнтами та РозрахункиЗПостачальниками

                РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РозрахункиЗКлієнтами_RecordsSet();
                РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РозрахункиЗПостачальниками_RecordsSet();

                foreach (ВведенняЗалишків_РозрахункиЗКонтрагентами_TablePart.Record РозрахункиЗКонтрагентами_Record in ДокументОбєкт.РозрахункиЗКонтрагентами_TablePart.Records)
                {
                    if (РозрахункиЗКонтрагентами_Record.ТипКонтрагента == Перелічення.ТипиКонтрагентів.Клієнт)
                    {
                        РозрахункиЗКлієнтами_RecordsSet.Record record_Клієнт = new РозрахункиЗКлієнтами_RecordsSet.Record()
                        {
                            Income = false,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Контрагент = РозрахункиЗКонтрагентами_Record.Контрагент,
                            Валюта = РозрахункиЗКонтрагентами_Record.Валюта,
                            Сума = РозрахункиЗКонтрагентами_Record.Сума
                        };

                        розрахункиЗКлієнтами_RecordsSet.Records.Add(record_Клієнт);
                    }

                    if (РозрахункиЗКонтрагентами_Record.ТипКонтрагента == Перелічення.ТипиКонтрагентів.Постачальник)
                    {
                        РозрахункиЗПостачальниками_RecordsSet.Record record_Постачальник = new РозрахункиЗПостачальниками_RecordsSet.Record()
                        {
                            Income = true,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Контрагент = РозрахункиЗКонтрагентами_Record.Контрагент,
                            Валюта = РозрахункиЗКонтрагентами_Record.Валюта,
                            Сума = РозрахункиЗКонтрагентами_Record.Сума,
                        };

                        розрахункиЗПостачальниками_RecordsSet.Records.Add(record_Постачальник);
                    }
                }

                await розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);
                await розрахункиЗПостачальниками_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, ex.Message);
                await ClearSpend(ДокументОбєкт);

                return false;
            }
        }

        public static async ValueTask ClearSpend(ВведенняЗалишків_Objest ДокументОбєкт)
        {
            ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();
            await товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();
            await вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();
            await партіїТоварів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            РухКоштів_RecordsSet рухКоштів_RecordsSet = new РухКоштів_RecordsSet();
            await рухКоштів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РозрахункиЗПостачальниками_RecordsSet();
            await розрахункиЗПостачальниками_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РозрахункиЗКлієнтами_RecordsSet();
            await розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class ВнутрішнєСпоживанняТоварів_SpendTheDocument
    {
        public static async ValueTask<bool> Spend(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
        {
            try
            {
                #region Підготовка

                Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();
                Dictionary<int, decimal> ЗалишокНоменклатури = new Dictionary<int, decimal>();
                Dictionary<int, decimal> РезервНоменклатури = new Dictionary<int, decimal>();

                foreach (ВнутрішнєСпоживанняТоварів_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (ТовариРядок.Номенклатура.IsEmpty())
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");

                    if (!(ТовариРядок.Кількість > 0))
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");

                    СписокНоменклатури.Add(ТовариРядок.НомерРядка, (await ТовариРядок.Номенклатура.GetDirectoryObject())!);

                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        //
                        //Для товарів отримуємо залишки
                        //

                        List<Dictionary<string, object>> Залишок_Список = СпільніФункції.ОтриматиЗалишкиТоваруНаСкладі(
                            ТовариРядок.Номенклатура, ТовариРядок.ХарактеристикаНоменклатури, ТовариРядок.Серія,
                            ДокументОбєкт.Склад, ДокументОбєкт.UnigueID, ДокументОбєкт.ДатаДок);

                        ЗалишокНоменклатури.Add(ТовариРядок.НомерРядка, Залишок_Список.Count > 0 ? (decimal)Залишок_Список[0]["ВНаявності"] : 0);

                        if (ЗалишокНоменклатури[ТовариРядок.НомерРядка] > 0)
                        {
                            if (ЗалишокНоменклатури[ТовариРядок.НомерРядка] < ТовариРядок.Кількість)
                            {
                                throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                    $"Залишок товару {ЗалишокНоменклатури[ТовариРядок.НомерРядка]}, потрібно {ТовариРядок.Кількість}");
                            }

                            //
                            // Рахуємо всі резерви
                            //

                            List<Dictionary<string, object>> Резерв_Список = СпільніФункції.ОтриматиРезервТоваруНаСкладі(
                            ТовариРядок.Номенклатура, ТовариРядок.ХарактеристикаНоменклатури,
                            ДокументОбєкт.Склад, ДокументОбєкт.UnigueID, ДокументОбєкт.ДатаДок);

                            РезервНоменклатури.Add(ТовариРядок.НомерРядка, Резерв_Список.Count > 0 ? (decimal)Резерв_Список[0]["ВРезервіЗіСкладу"] : 0);

                            if (РезервНоменклатури[ТовариРядок.НомерРядка] > 0)
                            {
                                if (ЗалишокНоменклатури[ТовариРядок.НомерРядка] - РезервНоменклатури[ТовариРядок.НомерРядка] < ТовариРядок.Кількість)
                                {
                                    throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                        $"Залишок товару {ЗалишокНоменклатури[ТовариРядок.НомерРядка]}, " +
                                        $"в резерві {РезервНоменклатури[ТовариРядок.НомерРядка]}, " +
                                        $"потрібно {ТовариРядок.Кількість}");
                                }
                            }
                        }
                        else
                        {
                            throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Відсутній товар {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        }
                    }
                }

                #endregion

                #region ТовариНаСкладах

                ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();

                foreach (ВнутрішнєСпоживанняТоварів_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        ТовариНаСкладах_RecordsSet.Record record = new ТовариНаСкладах_RecordsSet.Record()
                        {
                            Income = false,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Серія = ТовариРядок.Серія,
                            Склад = ДокументОбєкт.Склад,
                            ВНаявності = ТовариРядок.Кількість,
                        };

                        товариНаСкладах_RecordsSet.Records.Add(record);
                    }
                }

                await товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region ВільніЗалишки

                ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();

                foreach (ВнутрішнєСпоживанняТоварів_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        ВільніЗалишки_RecordsSet.Record record = new ВільніЗалишки_RecordsSet.Record()
                        {
                            Income = false,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Склад = ДокументОбєкт.Склад,
                            ВНаявності = ТовариРядок.Кількість,
                        };

                        вільніЗалишки_RecordsSet.Records.Add(record);
                    }
                }

                await вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region Партії товарів

                ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();

                foreach (ВнутрішнєСпоживанняТоварів_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        List<Dictionary<string, object>> listNameRow = СпільніФункції.ОтриматиСписокНаявнихПартій(
                            ДокументОбєкт.Організація, ТовариРядок.Номенклатура, ТовариРядок.ХарактеристикаНоменклатури,
                            ТовариРядок.Серія, ДокументОбєкт.Склад, ТовариРядок.Партія, ДокументОбєкт.UnigueID, ДокументОбєкт.ДатаДок, ТовариРядок.Кількість);

                        if (listNameRow.Count == 0)
                        {
                            throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Немає доступних партій для товару {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        }

                        decimal КількістьЯкуПотрібноСписати = ТовариРядок.Кількість;

                        foreach (Dictionary<string, object> nameRow in listNameRow)
                        {
                            decimal КількістьВПартії = (decimal)nameRow["Кількість"];
                            decimal СобівартістьПартії = (decimal)nameRow["Собівартість"];
                            ПартіяТоварівКомпозит_Pointer ПартіяТоварівКомпозит = new ПартіяТоварівКомпозит_Pointer(nameRow["ПартіяТоварівКомпозит"]);
                            int Рядок = (int)nameRow["Рядок"];

                            decimal КількістьЩоСписується = 0;
                            bool ЗакритиПартію = КількістьЯкуПотрібноСписати >= КількістьВПартії;

                            if (КількістьВПартії >= КількістьЯкуПотрібноСписати)
                            {
                                КількістьЩоСписується = КількістьЯкуПотрібноСписати;
                                КількістьЯкуПотрібноСписати = 0;
                            }
                            else
                            {
                                КількістьЩоСписується = КількістьВПартії;
                                КількістьЯкуПотрібноСписати -= КількістьВПартії;
                            }

                            ПартіїТоварів_RecordsSet.Record record = new ПартіїТоварів_RecordsSet.Record()
                            {
                                Income = false,
                                Owner = ДокументОбєкт.UnigueID.UGuid,
                                Організація = ДокументОбєкт.Організація,
                                ПартіяТоварівКомпозит = ПартіяТоварівКомпозит,
                                Кількість = КількістьЩоСписується,
                                Собівартість = ЗакритиПартію ? СобівартістьПартії : 0,
                                СписанаСобівартість = СобівартістьПартії,
                                Номенклатура = ТовариРядок.Номенклатура,
                                ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                                Серія = ТовариРядок.Серія,
                                Склад = ДокументОбєкт.Склад,
                                Рядок = Рядок,
                            };

                            партіїТоварів_RecordsSet.Records.Add(record);

                            if (КількістьЯкуПотрібноСписати == 0)
                                break;
                        }

                        if (КількістьЯкуПотрібноСписати > 0)
                        {
                            throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Невистачило списати {КількістьЯкуПотрібноСписати} товарів");
                        }
                    }
                }

                await партіїТоварів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, ex.Message);
                await ClearSpend(ДокументОбєкт);

                return false;
            }
        }

        public static async ValueTask ClearSpend(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
        {
            ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();
            await товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();
            await вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();
            await партіїТоварів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class ПсуванняТоварів_SpendTheDocument
    {
        public static async ValueTask<bool> Spend(ПсуванняТоварів_Objest ДокументОбєкт)
        {
            try
            {
                #region Підготовка

                Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();
                Dictionary<int, decimal> ЗалишокНоменклатури = new Dictionary<int, decimal>();
                Dictionary<int, decimal> РезервНоменклатури = new Dictionary<int, decimal>();

                foreach (ПсуванняТоварів_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (ТовариРядок.Номенклатура.IsEmpty())
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");

                    if (!(ТовариРядок.Кількість > 0))
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");

                    СписокНоменклатури.Add(ТовариРядок.НомерРядка, (await ТовариРядок.Номенклатура.GetDirectoryObject())!);

                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        //
                        //Для товарів отримуємо залишки
                        //

                        List<Dictionary<string, object>> Залишок_Список = СпільніФункції.ОтриматиЗалишкиТоваруНаСкладі(
                            ТовариРядок.Номенклатура, ТовариРядок.ХарактеристикаНоменклатури, ТовариРядок.Серія,
                            ДокументОбєкт.Склад, ДокументОбєкт.UnigueID, ДокументОбєкт.ДатаДок);

                        ЗалишокНоменклатури.Add(ТовариРядок.НомерРядка, Залишок_Список.Count > 0 ? (decimal)Залишок_Список[0]["ВНаявності"] : 0);

                        if (ЗалишокНоменклатури[ТовариРядок.НомерРядка] > 0)
                        {
                            if (ЗалишокНоменклатури[ТовариРядок.НомерРядка] < ТовариРядок.Кількість)
                            {
                                throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                    $"Залишок товару {ЗалишокНоменклатури[ТовариРядок.НомерРядка]}, потрібно {ТовариРядок.Кількість}");
                            }

                            //
                            // Рахуємо всі резерви
                            //

                            List<Dictionary<string, object>> Резерв_Список = СпільніФункції.ОтриматиРезервТоваруНаСкладі(
                            ТовариРядок.Номенклатура, ТовариРядок.ХарактеристикаНоменклатури,
                            ДокументОбєкт.Склад, ДокументОбєкт.UnigueID, ДокументОбєкт.ДатаДок);

                            РезервНоменклатури.Add(ТовариРядок.НомерРядка, Резерв_Список.Count > 0 ? (decimal)Резерв_Список[0]["ВРезервіЗіСкладу"] : 0);

                            if (РезервНоменклатури[ТовариРядок.НомерРядка] > 0)
                            {
                                if (ЗалишокНоменклатури[ТовариРядок.НомерРядка] - РезервНоменклатури[ТовариРядок.НомерРядка] < ТовариРядок.Кількість)
                                {
                                    throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                        $"Залишок товару {ЗалишокНоменклатури[ТовариРядок.НомерРядка]}, " +
                                        $"в резерві {РезервНоменклатури[ТовариРядок.НомерРядка]}, " +
                                        $"потрібно {ТовариРядок.Кількість}");
                                }
                            }
                        }
                        else
                        {
                            throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Відсутній товар {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        }
                    }
                }

                #endregion

                #region ТовариНаСкладах

                ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();

                foreach (ПсуванняТоварів_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        ТовариНаСкладах_RecordsSet.Record record = new ТовариНаСкладах_RecordsSet.Record()
                        {
                            Income = false,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Серія = ТовариРядок.Серія,
                            Склад = ДокументОбєкт.Склад,
                            ВНаявності = ТовариРядок.Кількість,
                        };

                        товариНаСкладах_RecordsSet.Records.Add(record);
                    }
                }

                await товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region ВільніЗалишки

                ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();

                foreach (ПсуванняТоварів_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        ВільніЗалишки_RecordsSet.Record record = new ВільніЗалишки_RecordsSet.Record()
                        {
                            Income = false,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Склад = ДокументОбєкт.Склад,
                            ВНаявності = ТовариРядок.Кількість,
                        };

                        вільніЗалишки_RecordsSet.Records.Add(record);
                    }
                }

                await вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                #region Партії товарів

                ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();

                foreach (ПсуванняТоварів_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        List<Dictionary<string, object>> listNameRow = СпільніФункції.ОтриматиСписокНаявнихПартій(
                            ДокументОбєкт.Організація, ТовариРядок.Номенклатура, ТовариРядок.ХарактеристикаНоменклатури,
                            ТовариРядок.Серія, ДокументОбєкт.Склад, ТовариРядок.Партія, ДокументОбєкт.UnigueID, ДокументОбєкт.ДатаДок, ТовариРядок.Кількість);

                        if (listNameRow.Count == 0)
                        {
                            throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Немає доступних партій для товару {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        }

                        decimal КількістьЯкуПотрібноСписати = ТовариРядок.Кількість;

                        foreach (Dictionary<string, object> nameRow in listNameRow)
                        {
                            decimal КількістьВПартії = (decimal)nameRow["Кількість"];
                            decimal СобівартістьПартії = (decimal)nameRow["Собівартість"];
                            ПартіяТоварівКомпозит_Pointer ПартіяТоварівКомпозит = new ПартіяТоварівКомпозит_Pointer(nameRow["ПартіяТоварівКомпозит"]);
                            int Рядок = (int)nameRow["Рядок"];

                            decimal КількістьЩоСписується = 0;
                            bool ЗакритиПартію = КількістьЯкуПотрібноСписати >= КількістьВПартії;

                            if (КількістьВПартії >= КількістьЯкуПотрібноСписати)
                            {
                                КількістьЩоСписується = КількістьЯкуПотрібноСписати;
                                КількістьЯкуПотрібноСписати = 0;
                            }
                            else
                            {
                                КількістьЩоСписується = КількістьВПартії;
                                КількістьЯкуПотрібноСписати -= КількістьВПартії;
                            }

                            ПартіїТоварів_RecordsSet.Record record = new ПартіїТоварів_RecordsSet.Record()
                            {
                                Income = false,
                                Owner = ДокументОбєкт.UnigueID.UGuid,
                                Організація = ДокументОбєкт.Організація,
                                ПартіяТоварівКомпозит = ПартіяТоварівКомпозит,
                                Кількість = КількістьЩоСписується,
                                Собівартість = ЗакритиПартію ? СобівартістьПартії : 0,
                                СписанаСобівартість = СобівартістьПартії,
                                Номенклатура = ТовариРядок.Номенклатура,
                                ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                                Серія = ТовариРядок.Серія,
                                Склад = ДокументОбєкт.Склад,
                                Рядок = Рядок,
                            };

                            партіїТоварів_RecordsSet.Records.Add(record);

                            if (КількістьЯкуПотрібноСписати == 0)
                                break;
                        }

                        if (КількістьЯкуПотрібноСписати > 0)
                        {
                            throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Невистачило списати {КількістьЯкуПотрібноСписати} товарів");
                        }
                    }
                }

                await партіїТоварів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, ex.Message);
                await ClearSpend(ДокументОбєкт);

                return false;
            }
        }

        public static async ValueTask ClearSpend(ПсуванняТоварів_Objest ДокументОбєкт)
        {
            ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();
            await товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();
            await вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();
            await партіїТоварів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class ПерерахунокТоварів_SpendTheDocument
    {
        public static async ValueTask<bool> Spend(ПерерахунокТоварів_Objest ДокументОбєкт)
        {
            try
            {
                #region Підготовка

                Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>(); //?
                Dictionary<int, decimal> ЗалишокНоменклатури = new Dictionary<int, decimal>();
                Dictionary<int, decimal> РезервНоменклатури = new Dictionary<int, decimal>();

                foreach (ПерерахунокТоварів_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (ТовариРядок.Номенклатура.IsEmpty())
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");

                    // if (!(ТовариРядок.Кількість > 0))
                    //     throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");

                    СписокНоменклатури.Add(ТовариРядок.НомерРядка, (await ТовариРядок.Номенклатура.GetDirectoryObject())!);
                }

                #endregion
                


                return true;
            }
            catch (Exception ex)
            {
                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, ex.Message);
                await ClearSpend(ДокументОбєкт);
                return false;
            }
        }

        public static async ValueTask ClearSpend(ПерерахунокТоварів_Objest ДокументОбєкт)
        {
            // код очищення проводок
            await ValueTask.FromResult(true); //Заглушка
        }
    }


    #endregion

    #region Адресне розміщення на складі

    class РозміщенняТоварівНаСкладі_SpendTheDocument
    {
        public static async ValueTask<bool> Spend(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            try
            {
                #region Підготовка

                Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();

                foreach (РозміщенняТоварівНаСкладі_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (ТовариРядок.Номенклатура.IsEmpty())
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");

                    if (ТовариРядок.Комірка.IsEmpty())
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Комірка");

                    if (!(ТовариРядок.Кількість > 0))
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");

                    СписокНоменклатури.Add(ТовариРядок.НомерРядка, (await ТовариРядок.Номенклатура.GetDirectoryObject())!);
                }

                #endregion

                #region ТовариВКомірках

                ТовариВКомірках_RecordsSet товариВКомірках_RecordsSet = new ТовариВКомірках_RecordsSet();

                foreach (РозміщенняТоварівНаСкладі_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        ТовариВКомірках_RecordsSet.Record record = new ТовариВКомірках_RecordsSet.Record()
                        {
                            Income = true,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Пакування = ТовариРядок.Пакування,
                            Комірка = ТовариРядок.Комірка,
                            Серія = ТовариРядок.Серія,
                            ВНаявності = ТовариРядок.Кількість,
                        };

                        товариВКомірках_RecordsSet.Records.Add(record);
                    }
                }

                await товариВКомірках_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, ex.Message);
                await ClearSpend(ДокументОбєкт);

                return false;
            }
        }

        public static async ValueTask ClearSpend(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ТовариВКомірках_RecordsSet товариВКомірках_RecordsSet = new ТовариВКомірках_RecordsSet();
            await товариВКомірках_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class ПереміщенняТоварівНаСкладі_SpendTheDocument
    {
        public static async ValueTask<bool> Spend(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            try
            {
                #region Підготовка

                Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();
                Dictionary<int, decimal> ЗалишокНоменклатури = new Dictionary<int, decimal>();

                foreach (ПереміщенняТоварівНаСкладі_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (ТовариРядок.Номенклатура.IsEmpty())
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");

                    if (!(ТовариРядок.Кількість > 0))
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");

                    СписокНоменклатури.Add(ТовариРядок.НомерРядка, (await ТовариРядок.Номенклатура.GetDirectoryObject())!);

                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        //
                        //Для товарів отримуємо залишки
                        //

                        List<Dictionary<string, object>> Залишок_Список = СпільніФункції.ОтриматиЗалишкиТоваруВКомірці(
                            ТовариРядок.Номенклатура, ТовариРядок.ХарактеристикаНоменклатури, ТовариРядок.Пакування,
                            ТовариРядок.КоміркаВідправник, ТовариРядок.Серія,
                            ДокументОбєкт.UnigueID, ДокументОбєкт.ДатаДок);

                        ЗалишокНоменклатури.Add(ТовариРядок.НомерРядка, Залишок_Список.Count > 0 ? (decimal)Залишок_Список[0]["ВНаявності"] : 0);

                        if (ЗалишокНоменклатури[ТовариРядок.НомерРядка] > 0)
                        {
                            if (ЗалишокНоменклатури[ТовариРядок.НомерРядка] < ТовариРядок.Кількість)
                            {
                                throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                    $"Залишок товару {ЗалишокНоменклатури[ТовариРядок.НомерРядка]}, потрібно {ТовариРядок.Кількість}\n" +
                                    $"Номенклатура {ТовариРядок.Номенклатура.GetPresentation()}\n" +
                                    $"Характеристика {ТовариРядок.ХарактеристикаНоменклатури.GetPresentation()}\n" +
                                    $"Пакування {ТовариРядок.Пакування.GetPresentation()}\n" +
                                    $"Комірка {ТовариРядок.КоміркаВідправник.GetPresentation()}\n" +
                                    $"Серія {ТовариРядок.Серія.GetPresentation()}\n");
                            }
                        }
                        else
                        {
                            throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Відсутній товар {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        }
                    }
                }

                #endregion

                #region ТовариВКомірках

                ТовариВКомірках_RecordsSet товариВКомірках_RecordsSet = new ТовариВКомірках_RecordsSet();

                foreach (ПереміщенняТоварівНаСкладі_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        //КоміркаВідправник
                        {
                            ТовариВКомірках_RecordsSet.Record record = new ТовариВКомірках_RecordsSet.Record()
                            {
                                Income = false,
                                Owner = ДокументОбєкт.UnigueID.UGuid,
                                Номенклатура = ТовариРядок.Номенклатура,
                                ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                                Пакування = ТовариРядок.Пакування,
                                Комірка = ТовариРядок.КоміркаВідправник,
                                Серія = ТовариРядок.Серія,
                                ВНаявності = ТовариРядок.Кількість,
                            };

                            товариВКомірках_RecordsSet.Records.Add(record);
                        }

                        //КоміркаОтримувач
                        {
                            ТовариВКомірках_RecordsSet.Record record = new ТовариВКомірках_RecordsSet.Record()
                            {
                                Income = true,
                                Owner = ДокументОбєкт.UnigueID.UGuid,
                                Номенклатура = ТовариРядок.Номенклатура,
                                ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                                Пакування = ТовариРядок.Пакування,
                                Комірка = ТовариРядок.КоміркаОтримувач,
                                Серія = ТовариРядок.Серія,
                                ВНаявності = ТовариРядок.Кількість,
                            };

                            товариВКомірках_RecordsSet.Records.Add(record);
                        }
                    }
                }

                await товариВКомірках_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, ex.Message);
                await ClearSpend(ДокументОбєкт);

                return false;
            }
        }

        public static async ValueTask ClearSpend(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ТовариВКомірках_RecordsSet товариВКомірках_RecordsSet = new ТовариВКомірках_RecordsSet();
            await товариВКомірках_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class ЗбіркаТоварівНаСкладі_SpendTheDocument
    {
        public static async ValueTask<bool> Spend(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт)
        {
            try
            {
                #region Підготовка ОтриматиЗалишкиТоваруВКомірці

                Dictionary<int, decimal> ЗалишокНоменклатури = new Dictionary<int, decimal>();
                Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();

                foreach (ЗбіркаТоварівНаСкладі_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (ТовариРядок.Номенклатура.IsEmpty())
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");

                    if (ТовариРядок.Комірка.IsEmpty())
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Комірка");

                    if (!(ТовариРядок.Кількість > 0))
                        throw new Exception($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");

                    СписокНоменклатури.Add(ТовариРядок.НомерРядка, (await ТовариРядок.Номенклатура.GetDirectoryObject())!);

                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        //
                        //Для товарів отримуємо залишки
                        //

                        List<Dictionary<string, object>> Залишок_Список = СпільніФункції.ОтриматиЗалишкиТоваруВКомірці(
                            ТовариРядок.Номенклатура, ТовариРядок.ХарактеристикаНоменклатури, ТовариРядок.Пакування,
                            ТовариРядок.Комірка, ТовариРядок.Серія,
                            ДокументОбєкт.UnigueID, ДокументОбєкт.ДатаДок);

                        ЗалишокНоменклатури.Add(ТовариРядок.НомерРядка, Залишок_Список.Count > 0 ? (decimal)Залишок_Список[0]["ВНаявності"] : 0);

                        if (ЗалишокНоменклатури[ТовариРядок.НомерРядка] > 0)
                        {
                            if (ЗалишокНоменклатури[ТовариРядок.НомерРядка] < ТовариРядок.Кількість)
                            {
                                throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                    $"Залишок товару {ЗалишокНоменклатури[ТовариРядок.НомерРядка]}, потрібно {ТовариРядок.Кількість}\n" +
                                    $"Номенклатура {ТовариРядок.Номенклатура.GetPresentation()}\n" +
                                    $"Характеристика {ТовариРядок.ХарактеристикаНоменклатури.GetPresentation()}\n" +
                                    $"Пакування {ТовариРядок.Пакування.GetPresentation()}\n" +
                                    $"Комірка {ТовариРядок.Комірка.GetPresentation()}\n" +
                                    $"Серія {ТовариРядок.Серія.GetPresentation()}");
                            }
                        }
                        else
                        {
                            throw new Exception($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Відсутній товар {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        }
                    }
                }

                #endregion

                #region ТовариВКомірках

                ТовариВКомірках_RecordsSet товариВКомірках_RecordsSet = new ТовариВКомірках_RecordsSet();

                foreach (ЗбіркаТоварівНаСкладі_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
                {
                    if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                    {
                        ТовариВКомірках_RecordsSet.Record record = new ТовариВКомірках_RecordsSet.Record()
                        {
                            Income = false,
                            Owner = ДокументОбєкт.UnigueID.UGuid,
                            Номенклатура = ТовариРядок.Номенклатура,
                            ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури,
                            Пакування = ТовариРядок.Пакування,
                            Комірка = ТовариРядок.Комірка,
                            Серія = ТовариРядок.Серія,
                            ВНаявності = ТовариРядок.Кількість,
                        };

                        товариВКомірках_RecordsSet.Records.Add(record);
                    }
                }

                await товариВКомірках_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, ex.Message);
                await ClearSpend(ДокументОбєкт);

                return false;
            }
        }

        public static async ValueTask ClearSpend(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ТовариВКомірках_RecordsSet товариВКомірках_RecordsSet = new ТовариВКомірках_RecordsSet();
            await товариВКомірках_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class РозміщенняНоменклатуриПоКоміркам_SpendTheDocument
    {
        public static async ValueTask<bool> Spend(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт)
        {
            try
            {
                #region Рух по регістрах

                РозміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet розміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet = new РозміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet();

                foreach (РозміщенняНоменклатуриПоКоміркам_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
                {
                    РозміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet.Record record = new РозміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet.Record()
                    {
                        Номенклатура = Товари_Record.Номенклатура,
                        //Склад = Товари_Record.Склад,
                        //Приміщення = Товари_Record.Приміщення,
                        Комірка = Товари_Record.Комірка,
                    };

                    розміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet.Records.Add(record);
                }

                await розміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, ex.Message);
                await ClearSpend(ДокументОбєкт);

                return false;
            }
        }

        public static async ValueTask ClearSpend(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт)
        {
            РозміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet розміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet = new РозміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet();
            await розміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    #endregion

}
