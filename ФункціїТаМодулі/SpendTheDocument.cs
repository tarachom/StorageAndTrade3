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

            string query = $@"
WITH register AS
(
	SELECT 
		ПартіїТоварів.{ПартіїТоварів_Const.ПартіяТоварівКомпозит} AS ПартіяТоварівКомпозит,
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
        AND ПартіїТоварів.{ПартіїТоварів_Const.Склад} = '{Склад.UnigueID}'
        AND ПартіїТоварів.owner != '{Owner}'

	GROUP BY ПартіяТоварівКомпозит

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
	   register.Кількість,
	   register.Собівартість,
	   SUM(register.Кількість) OVER(ORDER BY Довідник_ПартіяТоварівКомпозит.{ПартіяТоварівКомпозит_Const.Дата} {МетодСортування}) >= @Кількість AS ДостатняКількість
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
		Кількість,
		Собівартість
	FROM Обчислення
	WHERE ДостатняКількість = false
)
UNION ALL
(
	SELECT 
		ПартіяТоварівКомпозит,
		Кількість,
		Собівартість
	FROM Обчислення
	WHERE ДостатняКількість = true
	LIMIT 1
)
";
            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("period_end", OwnerDateDoc);
            paramQuery.Add("Кількість", Кількість);

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

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();

            paramQuery.Add("period_end", OwnerDateDoc);

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

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();

            paramQuery.Add("period_end", OwnerDateDoc);

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

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("period_end", OwnerDateDoc);

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

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("period_end", OwnerDateDoc);

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

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("period_end", OwnerDateDoc);

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

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("period_end", OwnerDateDoc);

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
        public static void ДокументНеПроводиться(DocumentObject ДокументОбєкт, string НазваДокументу, List<string> СписокПомилок)
        {
            ФункціїДляПовідомлень.ДодатиПовідомленняПроПомилку(
                DateTime.Now, "Проведення документу", ДокументОбєкт.UnigueID.UGuid,
                $"Документ.{ДокументОбєкт.TypeDocument}", НазваДокументу,
                string.Join("\n", СписокПомилок.ToArray()) + "\n\nДокумент [" + НазваДокументу + "] не проводиться!");
        }
    }

    #region Продажі

    class ЗамовленняКлієнта_SpendTheDocument
    {
        public static bool Spend(ЗамовленняКлієнта_Objest ДокументОбєкт)
        {
            #region Підготовка

            List<string> СписокПомилок = new List<string>();
            Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();
            Dictionary<int, decimal> ВільнийЗалишокНоменклатури = new Dictionary<int, decimal>();

            foreach (ЗамовленняКлієнта_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (ТовариРядок.Номенклатура.IsEmpty())
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                if (!(ТовариРядок.Кількість > 0))
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                СписокНоменклатури.Add(ТовариРядок.НомерРядка, ТовариРядок.Номенклатура.GetDirectoryObject()!);

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
                        ВільнийЗалишокНоменклатури.Add(ТовариРядок.НомерРядка, (decimal)Резерв[0]["ВНаявності"] - (decimal)Резерв[0]["ВРезервіЗіСкладу"]);
                    else
                        ВільнийЗалишокНоменклатури.Add(ТовариРядок.НомерРядка, 0);
                }
            }

            #endregion

            #region ЗамовленняКлієнтів

            ЗамовленняКлієнтів_RecordsSet замовленняКлієнтів_RecordsSet = new ЗамовленняКлієнтів_RecordsSet();

            ДокументОбєкт.Товари_TablePart.Read();

            foreach (ЗамовленняКлієнта_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                {
                    ЗамовленняКлієнтів_RecordsSet.Record record = new ЗамовленняКлієнтів_RecordsSet.Record();
                    замовленняКлієнтів_RecordsSet.Records.Add(record);

                    record.Income = true;
                    record.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record.ЗамовленняКлієнта = ДокументОбєкт.GetDocumentPointer();
                    record.Номенклатура = ТовариРядок.Номенклатура;
                    record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record.Склад = !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад;
                    record.Замовлено = ТовариРядок.Кількість;
                    record.Сума = ТовариРядок.Сума;
                }
            }

            замовленняКлієнтів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region ВільніЗалишки

            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();

            foreach (ЗамовленняКлієнта_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                {
                    ВільніЗалишки_RecordsSet.Record record = new ВільніЗалишки_RecordsSet.Record();
                    вільніЗалишки_RecordsSet.Records.Add(record);

                    record.Income = true;
                    record.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record.Номенклатура = ТовариРядок.Номенклатура;
                    record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record.Склад = !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад;
                    record.ВРезервіЗіСкладу = ВільнийЗалишокНоменклатури[ТовариРядок.НомерРядка];

                    decimal КількістьЯкуПотрібноРозприділити = ТовариРядок.Кількість;

                    if (ВільнийЗалишокНоменклатури[ТовариРядок.НомерРядка] > 0)
                    {
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
                    }

                    if (КількістьЯкуПотрібноРозприділити > 0)
                        record.ВРезервіПідЗамовлення = КількістьЯкуПотрібноРозприділити;

                    record.ДокументРезерву = ДокументОбєкт.UnigueID.UGuid;
                }
            }

            вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            return true;
        }

        public static void ClearSpend(ЗамовленняКлієнта_Objest ДокументОбєкт)
        {
            ЗамовленняКлієнтів_RecordsSet замовленняКлієнтів_RecordsSet = new ЗамовленняКлієнтів_RecordsSet();
            замовленняКлієнтів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();
            вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class РахунокФактура_SpendTheDocument
    {
        public static bool Spend(РахунокФактура_Objest ДокументОбєкт)
        {
            #region Підготовка

            List<string> СписокПомилок = new List<string>();
            Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();

            foreach (РахунокФактура_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (ТовариРядок.Номенклатура.IsEmpty())
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                if (!(ТовариРядок.Кількість > 0))
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                СписокНоменклатури.Add(ТовариРядок.НомерРядка, ТовариРядок.Номенклатура.GetDirectoryObject()!);

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
                            СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"В наявності {ВНаявності}, " +
                                $"в резерві {ВРезервіЗіСкладу}, " +
                                $"вільний залишок {ВНаявності - ВРезервіЗіСкладу}, " +
                                $"потрібно зарезервувати {ТовариРядок.Кількість}");
                            СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                            return false;
                        }
                    }
                    else
                    {
                        СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                             $"Відсутній товар на залишку");
                        СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                        return false;
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
                    ВільніЗалишки_RecordsSet.Record record = new ВільніЗалишки_RecordsSet.Record();
                    вільніЗалишки_RecordsSet.Records.Add(record);

                    record.Income = true;
                    record.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record.Номенклатура = ТовариРядок.Номенклатура;
                    record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record.Склад = !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад;
                    record.ВРезервіЗіСкладу = ТовариРядок.Кількість;
                    record.ДокументРезерву = ДокументОбєкт.UnigueID.UGuid;
                }
            }

            вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            return true;
        }

        public static void ClearSpend(РахунокФактура_Objest ДокументОбєкт)
        {
            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();
            вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class РеалізаціяТоварівТаПослуг_SpendTheDocument
    {
        public static bool Spend(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {
            #region Підготовка

            List<string> СписокПомилок = new List<string>();

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
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                if (!(ТовариРядок.Кількість > 0))
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                СписокНоменклатури.Add(ТовариРядок.НомерРядка, ТовариРядок.Номенклатура.GetDirectoryObject()!);

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
                            СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Залишок товару {ЗалишокНоменклатури[ТовариРядок.НомерРядка]}, потрібно {ТовариРядок.Кількість}");
                            СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                            return false;
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
                                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                                        $"Залишок по документу Замовлення {ЗамволенняКлієнта[ТовариРядок.НомерРядка]}");
                                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                                    return false;
                                }
                            }
                            else
                            {
                                СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                                    $"Відсутнє замовлення для товару {СписокНоменклатури[ТовариРядок.НомерРядка].Назва} в документі Замовлення клієнта");
                                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                                return false;
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
                                СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                                    $"Вказаний рахунок фактура, не зарезервував товар {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                                return false;
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
                                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                                        $"Залишок товару {ЗалишокНоменклатури[ТовариРядок.НомерРядка]}, " +
                                        $"в резерві {РезервЗагальний}, " +
                                        $"потрібно {ТовариРядок.Кількість}");
                                else
                                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                                        $"Залишок товару {ЗалишокНоменклатури[ТовариРядок.НомерРядка]}, " +
                                        $"в резерві по вибраних документах (замовлення або рахунок) {РезервДокументівСума}, " +
                                        $"потрібно {ТовариРядок.Кількість}");

                                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                                return false;
                            }
                        }
                    }
                    else
                    {
                        СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                            $"Відсутній товар {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                        return false;
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

                        ЗамовленняКлієнтів_RecordsSet.Record record = new ЗамовленняКлієнтів_RecordsSet.Record();
                        замовленняКлієнтів_RecordsSet.Records.Add(record);

                        record.Income = false;
                        record.Owner = ДокументОбєкт.UnigueID.UGuid;

                        record.ЗамовленняКлієнта = ТовариРядок.ЗамовленняКлієнта;
                        record.Номенклатура = ТовариРядок.Номенклатура;
                        record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                        record.Склад = !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад;
                        record.Замовлено = Замовлено > ТовариРядок.Кількість ? ТовариРядок.Кількість : Замовлено;
                        record.Сума = Замовлено > ТовариРядок.Кількість ? 0 : ЗамовленняСума;
                    }
                }
            }

            замовленняКлієнтів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

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
                            ВільніЗалишки_RecordsSet.Record record = new ВільніЗалишки_RecordsSet.Record();
                            вільніЗалишки_RecordsSet.Records.Add(record);

                            record.Income = false;
                            record.Owner = ДокументОбєкт.UnigueID.UGuid;

                            record.Номенклатура = ТовариРядок.Номенклатура;
                            record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                            record.Склад = !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад;

                            record.ВНаявності = КількістьЯкуПотрібноРозприділити >= ВРезервіЗамовленняКлієнта ? ВРезервіЗамовленняКлієнта : КількістьЯкуПотрібноРозприділити;
                            record.ВРезервіЗіСкладу = record.ВНаявності;
                            record.ДокументРезерву = ТовариРядок.ЗамовленняКлієнта.UnigueID.UGuid;

                            КількістьЯкуПотрібноРозприділити -= record.ВНаявності;
                        }

                        if (!ТовариРядок.РахунокФактура.IsEmpty() && КількістьЯкуПотрібноРозприділити > 0 && ВРезервіРахунокФактура > 0)
                        {
                            ВільніЗалишки_RecordsSet.Record record = new ВільніЗалишки_RecordsSet.Record();
                            вільніЗалишки_RecordsSet.Records.Add(record);

                            record.Income = false;
                            record.Owner = ДокументОбєкт.UnigueID.UGuid;

                            record.Номенклатура = ТовариРядок.Номенклатура;
                            record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                            record.Склад = !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад;

                            record.ВНаявності = КількістьЯкуПотрібноРозприділити >= ВРезервіРахунокФактура ? ВРезервіРахунокФактура : КількістьЯкуПотрібноРозприділити;
                            record.ВРезервіЗіСкладу = record.ВНаявності;
                            record.ДокументРезерву = ТовариРядок.РахунокФактура.UnigueID.UGuid;

                            КількістьЯкуПотрібноРозприділити -= record.ВНаявності;
                        }
                    }

                    if (КількістьЯкуПотрібноРозприділити > 0)
                    {
                        ВільніЗалишки_RecordsSet.Record record = new ВільніЗалишки_RecordsSet.Record();
                        вільніЗалишки_RecordsSet.Records.Add(record);

                        record.Income = false;
                        record.Owner = ДокументОбєкт.UnigueID.UGuid;

                        record.Номенклатура = ТовариРядок.Номенклатура;
                        record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                        record.Склад = !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад;

                        record.ВНаявності = КількістьЯкуПотрібноРозприділити;
                    }
                }
            }

            вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region Товари на складах

            ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();

            foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                {
                    ТовариНаСкладах_RecordsSet.Record record = new ТовариНаСкладах_RecordsSet.Record();
                    товариНаСкладах_RecordsSet.Records.Add(record);

                    record.Income = false;
                    record.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record.Номенклатура = ТовариРядок.Номенклатура;
                    record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record.Серія = ТовариРядок.Серія;
                    record.Склад = !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад;
                    record.ВНаявності = ТовариРядок.Кількість;
                }
            }

            товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

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
                        ТовариРядок.Серія, ДокументОбєкт.Склад, ДокументОбєкт.UnigueID, ДокументОбєкт.ДатаДок, ТовариРядок.Кількість);

                    if (listNameRow.Count == 0)
                    {
                        СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                            $"Немає доступних партій для товару {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                        return false;
                    }

                    decimal КількістьЯкуПотрібноСписати = ТовариРядок.Кількість;

                    foreach (Dictionary<string, object> nameRow in listNameRow)
                    {
                        decimal КількістьВПартії = (decimal)nameRow["Кількість"];
                        decimal СобівартістьПартії = (decimal)nameRow["Собівартість"];
                        ПартіяТоварівКомпозит_Pointer ПартіяТоварівКомпозит = new ПартіяТоварівКомпозит_Pointer(nameRow["ПартіяТоварівКомпозит"]);

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
                            ПартіїТоварів_RecordsSet.Record record = new ПартіїТоварів_RecordsSet.Record();
                            партіїТоварів_RecordsSet.Records.Add(record);

                            record.Income = false;
                            record.Owner = ДокументОбєкт.UnigueID.UGuid;

                            record.Організація = ДокументОбєкт.Організація;
                            record.ПартіяТоварівКомпозит = ПартіяТоварівКомпозит;
                            record.Кількість = КількістьЩоСписується;
                            record.Собівартість = (ЗакритиПартію ? СобівартістьПартії : 0);
                            record.СписанаСобівартість = СобівартістьПартії;
                            record.Номенклатура = ТовариРядок.Номенклатура;
                            record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                            record.Серія = ТовариРядок.Серія;
                            record.Склад = ДокументОбєкт.Склад;
                        }

                        //Продажі
                        {
                            Продажі_RecordsSet.Record record = new Продажі_RecordsSet.Record();
                            продажі_RecordsSet.Records.Add(record);

                            record.Owner = ДокументОбєкт.UnigueID.UGuid;
                            record.Організація = ДокументОбєкт.Організація;
                            record.Склад = ДокументОбєкт.Склад; ;
                            record.Контрагент = ДокументОбєкт.Контрагент;
                            record.Договір = ДокументОбєкт.Договір;
                            record.Номенклатура = ТовариРядок.Номенклатура;
                            record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                            record.Кількість = КількістьЩоСписується;
                            record.Сума = ТовариРядок.Ціна * КількістьЩоСписується;
                            record.Дохід = record.Сума - СобівартістьПартії * КількістьЩоСписується;
                            record.Собівартість = СобівартістьПартії;
                        }

                        if (КількістьЯкуПотрібноСписати == 0)
                            break;
                    }

                    if (КількістьЯкуПотрібноСписати > 0)
                    {
                        СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                            $"Невистачило списати {КількістьЯкуПотрібноСписати} товарів");
                        СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                        return false;
                    }
                }
            }

            партіїТоварів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);
            продажі_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region РозрахункиЗКлієнтами

            //??? Договір

            //
            //РозрахункиЗКлієнтами
            //

            РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РозрахункиЗКлієнтами_RecordsSet();

            РозрахункиЗКлієнтами_RecordsSet.Record розрахункиЗКлієнтами_Record = new РозрахункиЗКлієнтами_RecordsSet.Record();
            розрахункиЗКлієнтами_RecordsSet.Records.Add(розрахункиЗКлієнтами_Record);

            розрахункиЗКлієнтами_Record.Income = true;
            розрахункиЗКлієнтами_Record.Owner = ДокументОбєкт.UnigueID.UGuid;

            розрахункиЗКлієнтами_Record.Контрагент = ДокументОбєкт.Контрагент;
            розрахункиЗКлієнтами_Record.Валюта = ДокументОбєкт.Валюта;
            розрахункиЗКлієнтами_Record.Сума = ДокументОбєкт.СумаДокументу;

            розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            return true;
        }

        public static void ClearSpend(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {
            ЗамовленняКлієнтів_RecordsSet замовленняКлієнтів_RecordsSet = new ЗамовленняКлієнтів_RecordsSet();
            замовленняКлієнтів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();
            вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();
            товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();
            партіїТоварів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РозрахункиЗКлієнтами_RecordsSet();
            розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            //
            // Обороти
            //

            Продажі_RecordsSet продажі_RecordsSet = new Продажі_RecordsSet();
            продажі_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class АктВиконанихРобіт_SpendTheDocument
    {
        public static bool Spend(АктВиконанихРобіт_Objest ДокументОбєкт)
        {
            #region Підготовка

            List<string> СписокПомилок = new List<string>();

            foreach (АктВиконанихРобіт_Послуги_TablePart.Record ПослугиРядок in ДокументОбєкт.Послуги_TablePart.Records)
            {
                if (ПослугиРядок.Номенклатура.IsEmpty())
                {
                    СписокПомилок.Add($"Рядок {ПослугиРядок.НомерРядка}. Не заповнене поле Номенклатура");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                if (!(ПослугиРядок.Кількість > 0))
                {
                    СписокПомилок.Add($"Рядок {ПослугиРядок.НомерРядка}. Кількість має бути більшою 0");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }
            }

            #endregion

            #region РозрахункиЗКлієнтами

            РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РозрахункиЗКлієнтами_RecordsSet();

            РозрахункиЗКлієнтами_RecordsSet.Record розрахункиЗКлієнтами_Record = new РозрахункиЗКлієнтами_RecordsSet.Record();
            розрахункиЗКлієнтами_RecordsSet.Records.Add(розрахункиЗКлієнтами_Record);

            розрахункиЗКлієнтами_Record.Income = true;
            розрахункиЗКлієнтами_Record.Owner = ДокументОбєкт.UnigueID.UGuid;

            розрахункиЗКлієнтами_Record.Контрагент = ДокументОбєкт.Контрагент;
            розрахункиЗКлієнтами_Record.Валюта = ДокументОбєкт.Валюта;
            розрахункиЗКлієнтами_Record.Сума = ДокументОбєкт.СумаДокументу;

            розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region Продажі

            Продажі_RecordsSet продажі_RecordsSet = new Продажі_RecordsSet();

            foreach (АктВиконанихРобіт_Послуги_TablePart.Record ТовариРядок in ДокументОбєкт.Послуги_TablePart.Records)
            {
                Продажі_RecordsSet.Record record = new Продажі_RecordsSet.Record();
                продажі_RecordsSet.Records.Add(record);

                record.Owner = ДокументОбєкт.UnigueID.UGuid;
                record.Організація = ДокументОбєкт.Організація;
                //record.Склад = ДокументОбєкт.Склад;
                record.Контрагент = ДокументОбєкт.Контрагент;
                record.Договір = ДокументОбєкт.Договір;
                record.Номенклатура = ТовариРядок.Номенклатура;
                record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                record.Кількість = ТовариРядок.Кількість;
                record.Сума = ТовариРядок.Сума;
                record.Дохід = ТовариРядок.Сума;
                //record.Собівартість = 0;
            }

            продажі_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            return true;
        }

        public static void ClearSpend(АктВиконанихРобіт_Objest ДокументОбєкт)
        {
            РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РозрахункиЗКлієнтами_RecordsSet();
            розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            //
            // Обороти
            //

            Продажі_RecordsSet продажі_RecordsSet = new Продажі_RecordsSet();
            продажі_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    #endregion

    #region Продажі

    class ПоступленняТоварівТаПослуг_SpendTheDocument
    {
        public static bool Spend(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
        {
            #region Підготовка

            List<string> СписокПомилок = new List<string>();

            Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();

            Dictionary<int, decimal> ЗамовленняПостачальнику = new Dictionary<int, decimal>();

            foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (ТовариРядок.Номенклатура.IsEmpty())
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                if (!(ТовариРядок.Кількість > 0))
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                СписокНоменклатури.Add(ТовариРядок.НомерРядка, ТовариРядок.Номенклатура.GetDirectoryObject()!);

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
                                СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                                    $"Залишок по документу Замовлення {ЗамовленняПостачальнику[ТовариРядок.НомерРядка]}");
                                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                                return false;
                            }
                        }
                        else
                        {
                            СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Відсутнє замовлення для товару {СписокНоменклатури[ТовариРядок.НомерРядка].Назва} в документі Замовлення постачальнику");
                            СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                            return false;
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

                        ЗамовленняПостачальникам_RecordsSet.Record record = new ЗамовленняПостачальникам_RecordsSet.Record();
                        замовленняПостачальникам_RecordsSet.Records.Add(record);

                        record.Income = false;
                        record.Owner = ДокументОбєкт.UnigueID.UGuid;

                        record.ЗамовленняПостачальнику = ТовариРядок.ЗамовленняПостачальнику;
                        record.Номенклатура = ТовариРядок.Номенклатура;
                        record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                        record.Склад = !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад;
                        record.Замовлено = Замовлено > ТовариРядок.Кількість ? ТовариРядок.Кількість : Замовлено;
                    }
                }
            }

            замовленняПостачальникам_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region Товари на складах

            ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();

            foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                {
                    ТовариНаСкладах_RecordsSet.Record record = new ТовариНаСкладах_RecordsSet.Record();
                    товариНаСкладах_RecordsSet.Records.Add(record);

                    record.Income = true;
                    record.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record.Номенклатура = ТовариРядок.Номенклатура;
                    record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record.Склад = !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад;
                    record.Серія = ТовариРядок.Серія;
                    record.ВНаявності = ТовариРядок.Кількість;
                }
            }

            товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region Партії товарів

            ПартіяТоварівКомпозит_Pointer ПартіяТоварівКомпозит = ФункціїДляДокументів.ОтриматиПартіюТоварівКомпозит(
                 ДокументОбєкт.UnigueID.UGuid,
                 Перелічення.ТипДокументуПартіяТоварівКомпозит.ПоступленняТоварівТаПослуг,
                 ДокументОбєкт, null
            );

            ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();

            foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                {
                    ПартіїТоварів_RecordsSet.Record record = new ПартіїТоварів_RecordsSet.Record();
                    партіїТоварів_RecordsSet.Records.Add(record);

                    record.Income = true;
                    record.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record.Організація = ДокументОбєкт.Організація;
                    record.ПартіяТоварівКомпозит = ПартіяТоварівКомпозит;
                    record.Кількість = ТовариРядок.Кількість;
                    record.Собівартість = ТовариРядок.Ціна;
                    record.Номенклатура = ТовариРядок.Номенклатура;
                    record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record.Серія = ТовариРядок.Серія;
                    record.Склад = ДокументОбєкт.Склад;
                }
            }

            партіїТоварів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region ВільніЗалишки

            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();

            foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                {
                    ВільніЗалишки_RecordsSet.Record record = new ВільніЗалишки_RecordsSet.Record();
                    вільніЗалишки_RecordsSet.Records.Add(record);

                    record.Income = true;
                    record.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record.Номенклатура = ТовариРядок.Номенклатура;
                    record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record.Склад = !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад;
                    record.ВНаявності = ТовариРядок.Кількість;
                }
            }

            вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region ТовариДоПоступлення (не використовується) 
            //
            //ТовариДоПоступлення
            //

            //РегістриНакопичення.ТовариДоПоступлення_RecordsSet товариДоПоступлення_RecordsSet = new РегістриНакопичення.ТовариДоПоступлення_RecordsSet();

            //foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
            //{
            //    Довідники.Номенклатура_Objest номенклатура_Objest = Товари_Record.Номенклатура.GetDirectoryObject();

            //    //Товар
            //    if (номенклатура_Objest.ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
            //    {
            //        РегістриНакопичення.ТовариДоПоступлення_RecordsSet.Record record = new РегістриНакопичення.ТовариДоПоступлення_RecordsSet.Record();
            //        товариДоПоступлення_RecordsSet.Records.Add(record);

            //        record.Income = false; // -
            //        record.Owner = ДокументОбєкт.UnigueID.UGuid;

            //        record.Номенклатура = Товари_Record.Номенклатура;
            //        record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
            //        record.Склад = (!Товари_Record.Склад.IsEmpty() ? Товари_Record.Склад : ДокументОбєкт.Склад);
            //        record.ДоПоступлення = Товари_Record.Кількість;
            //    }
            //}

            //товариДоПоступлення_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            //??? Договір
            // !!! Замовлення постачальнику робить рухи по регістру РозрахункиЗПостачальниками
            // значить ПрихНакладна не повинна ще раз робити ці рухи якщо є замовлення

            #endregion

            #region РозрахункиЗПостачальниками

            РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РозрахункиЗПостачальниками_RecordsSet();

            if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ЗакупівляВПостачальника)
            {
                РозрахункиЗПостачальниками_RecordsSet.Record recordContragent = new РозрахункиЗПостачальниками_RecordsSet.Record();
                розрахункиЗПостачальниками_RecordsSet.Records.Add(recordContragent);

                recordContragent.Income = false;
                recordContragent.Owner = ДокументОбєкт.UnigueID.UGuid;

                recordContragent.Контрагент = ДокументОбєкт.Контрагент;
                recordContragent.Валюта = ДокументОбєкт.Валюта;
                recordContragent.Сума = ДокументОбєкт.СумаДокументу;
            }

            розрахункиЗПостачальниками_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region Закупівлі

            Закупівлі_RecordsSet закупівлі_RecordsSet = new Закупівлі_RecordsSet();

            foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                Закупівлі_RecordsSet.Record record = new Закупівлі_RecordsSet.Record();
                закупівлі_RecordsSet.Records.Add(record);

                record.Owner = ДокументОбєкт.UnigueID.UGuid;
                record.Організація = ДокументОбєкт.Організація;
                record.Склад = !ТовариРядок.Склад.IsEmpty() ? ТовариРядок.Склад : ДокументОбєкт.Склад;
                record.Контрагент = ДокументОбєкт.Контрагент;
                record.Договір = ДокументОбєкт.Договір;
                record.Номенклатура = ТовариРядок.Номенклатура;
                record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                record.Кількість = ТовариРядок.Кількість;
                record.Сума = ТовариРядок.Сума;
                record.Собівартість = Math.Round(ТовариРядок.Сума / ТовариРядок.Кількість, 2);
            }

            закупівлі_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            return true;
        }

        public static void ClearSpend(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
        {
            ЗамовленняПостачальникам_RecordsSet замовленняПостачальникам_RecordsSet = new ЗамовленняПостачальникам_RecordsSet();
            замовленняПостачальникам_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();
            товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();
            партіїТоварів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();
            вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            //ТовариДоПоступлення_RecordsSet товариДоПоступлення_RecordsSet = new ТовариДоПоступлення_RecordsSet();
            //товариДоПоступлення_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РозрахункиЗПостачальниками_RecordsSet();
            розрахункиЗПостачальниками_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            //
            // Обороти
            //

            Закупівлі_RecordsSet закупівлі_RecordsSet = new Закупівлі_RecordsSet();
            закупівлі_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class ЗамовленняПостачальнику_SpendTheDocument
    {
        public static bool Spend(ЗамовленняПостачальнику_Objest ДокументОбєкт)
        {
            #region Підготовка

            List<string> СписокПомилок = new List<string>();

            Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();

            foreach (ЗамовленняПостачальнику_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (ТовариРядок.Номенклатура.IsEmpty())
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                if (!(ТовариРядок.Кількість > 0))
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                СписокНоменклатури.Add(ТовариРядок.НомерРядка, ТовариРядок.Номенклатура.GetDirectoryObject()!);
            }

            #endregion

            #region Замовлення постачальникам

            ЗамовленняПостачальникам_RecordsSet замовленняПостачальникам_RecordsSet = new ЗамовленняПостачальникам_RecordsSet();

            foreach (ЗамовленняПостачальнику_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                {
                    ЗамовленняПостачальникам_RecordsSet.Record record = new ЗамовленняПостачальникам_RecordsSet.Record();
                    замовленняПостачальникам_RecordsSet.Records.Add(record);

                    record.Income = true;
                    record.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record.ЗамовленняПостачальнику = ДокументОбєкт.GetDocumentPointer();
                    record.Номенклатура = ТовариРядок.Номенклатура;
                    record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record.Склад = ДокументОбєкт.Склад;
                    record.Замовлено = ТовариРядок.Кількість;
                }
            }

            замовленняПостачальникам_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            return true;
        }

        public static void ClearSpend(ЗамовленняПостачальнику_Objest ДокументОбєкт)
        {
            ЗамовленняПостачальникам_RecordsSet замовленняПостачальникам_RecordsSet = new ЗамовленняПостачальникам_RecordsSet();
            замовленняПостачальникам_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
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

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("ДокументРеалізації", ТовариРядок.ДокументРеалізації.UnigueID.UGuid);
            paramQuery.Add("Організація", ДокументОбєкт.Організація.UnigueID.UGuid);
            paramQuery.Add("Товар", ТовариРядок.Номенклатура.UnigueID.UGuid);
            paramQuery.Add("Характеристика", ТовариРядок.ХарактеристикаНоменклатури.UnigueID.UGuid);
            paramQuery.Add("Серія", ТовариРядок.Серія.UnigueID.UGuid);
            paramQuery.Add("Склад", ДокументОбєкт.Склад.UnigueID.UGuid);

            string[] columnsName;
            List<Dictionary<string, object>> listNameRow;

            Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listNameRow);

            return listNameRow;
        }

        public static bool Spend(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
        {
            #region Підготовка

            List<string> СписокПомилок = new List<string>();

            Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();

            foreach (ПоверненняТоварівВідКлієнта_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (ТовариРядок.Номенклатура.IsEmpty())
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                if (!(ТовариРядок.Кількість > 0))
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                СписокНоменклатури.Add(ТовариРядок.НомерРядка, ТовариРядок.Номенклатура.GetDirectoryObject()!);
            }

            #endregion

            #region ВільніЗалишки

            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();

            foreach (ПоверненняТоварівВідКлієнта_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                {
                    ВільніЗалишки_RecordsSet.Record record = new ВільніЗалишки_RecordsSet.Record();
                    вільніЗалишки_RecordsSet.Records.Add(record);

                    record.Income = true;
                    record.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record.Номенклатура = ТовариРядок.Номенклатура;
                    record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record.Склад = ДокументОбєкт.Склад;
                    record.ВНаявності = ТовариРядок.Кількість;
                }
            }

            вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region Товари на складах

            ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();

            foreach (ПоверненняТоварівВідКлієнта_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                {
                    ТовариНаСкладах_RecordsSet.Record record = new ТовариНаСкладах_RecordsSet.Record();
                    товариНаСкладах_RecordsSet.Records.Add(record);

                    record.Income = true;
                    record.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record.Номенклатура = ТовариРядок.Номенклатура;
                    record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record.Серія = ТовариРядок.Серія;
                    record.Склад = ДокументОбєкт.Склад;
                    record.ВНаявності = ТовариРядок.Кількість;
                }
            }

            товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

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
                        СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                            $"Немає доступних партій для товару {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                        return false;
                    }

                    decimal КількістьЯкуПотрібноПовернути = ТовариРядок.Кількість;

                    foreach (Dictionary<string, object> nameRow in listNameRow)
                    {
                        decimal КількістьВПартії = (decimal)nameRow["Кількість"];
                        decimal СобівартістьПартії = (decimal)nameRow["Собівартість"];
                        decimal СписанаСобівартістьПартії = (decimal)nameRow["СписанаСобівартість"];
                        ПартіяТоварівКомпозит_Pointer ПартіяТоварівКомпозит = new ПартіяТоварівКомпозит_Pointer(nameRow["ПартіяТоварівКомпозит"]);

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
                            ПартіїТоварів_RecordsSet.Record record = new ПартіїТоварів_RecordsSet.Record();
                            партіїТоварів_RecordsSet.Records.Add(record);

                            record.Income = true;
                            record.Owner = ДокументОбєкт.UnigueID.UGuid;

                            record.Організація = ДокументОбєкт.Організація;
                            record.ПартіяТоварівКомпозит = ПартіяТоварівКомпозит;
                            record.Кількість = КількістьЩоПовертається;
                            record.Собівартість = СобівартістьПартії;
                            record.СписанаСобівартість = СписанаСобівартістьПартії;
                            record.Номенклатура = ТовариРядок.Номенклатура;
                            record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                            record.Серія = ТовариРядок.Серія;
                            record.Склад = ДокументОбєкт.Склад;
                        }

                        //Продажі
                        {
                            Продажі_RecordsSet.Record record = new Продажі_RecordsSet.Record();
                            продажі_RecordsSet.Records.Add(record);

                            record.Owner = ДокументОбєкт.UnigueID.UGuid;
                            record.Організація = ДокументОбєкт.Організація;
                            record.Склад = ДокументОбєкт.Склад; ;
                            record.Контрагент = ДокументОбєкт.Контрагент;
                            record.Договір = ДокументОбєкт.Договір;
                            record.Номенклатура = ТовариРядок.Номенклатура;
                            record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                            record.Кількість = -КількістьЩоПовертається;
                            record.Сума = -(ТовариРядок.Ціна * КількістьЩоПовертається);
                            record.Дохід = -(ТовариРядок.Ціна * КількістьЩоПовертається - СобівартістьПартії * КількістьЩоПовертається);
                            record.Собівартість = СобівартістьПартії;
                        }

                        if (КількістьЯкуПотрібноПовернути == 0)
                            break;
                    }

                    if (КількістьЯкуПотрібноПовернути > 0)
                    {
                        СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Невистачило повернути {КількістьЯкуПотрібноПовернути} товарів");
                        СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                        return false;
                    }
                }
            }

            партіїТоварів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);
            продажі_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region РозрахункиЗКлієнтами

            РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РозрахункиЗКлієнтами_RecordsSet();

            РозрахункиЗКлієнтами_RecordsSet.Record розрахункиЗКлієнтами_Record = new РозрахункиЗКлієнтами_RecordsSet.Record();
            розрахункиЗКлієнтами_RecordsSet.Records.Add(розрахункиЗКлієнтами_Record);

            розрахункиЗКлієнтами_Record.Income = false;
            розрахункиЗКлієнтами_Record.Owner = ДокументОбєкт.UnigueID.UGuid;

            розрахункиЗКлієнтами_Record.Контрагент = ДокументОбєкт.Контрагент;
            розрахункиЗКлієнтами_Record.Валюта = ДокументОбєкт.Валюта;
            розрахункиЗКлієнтами_Record.Сума = ДокументОбєкт.СумаДокументу;

            розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            return true;
        }

        public static void ClearSpend(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
        {
            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();
            вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();
            товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();
            партіїТоварів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РозрахункиЗКлієнтами_RecordsSet();
            розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            //
            // Обороти
            //

            Продажі_RecordsSet продажі_RecordsSet = new Продажі_RecordsSet();
            продажі_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
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

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("ДокументПоступлення", ТовариРядок.ДокументПоступлення.UnigueID.UGuid);
            paramQuery.Add("Організація", ДокументОбєкт.Організація.UnigueID.UGuid);
            paramQuery.Add("Товар", ТовариРядок.Номенклатура.UnigueID.UGuid);
            paramQuery.Add("Характеристика", ТовариРядок.ХарактеристикаНоменклатури.UnigueID.UGuid);
            paramQuery.Add("Серія", ТовариРядок.Серія.UnigueID.UGuid);
            paramQuery.Add("Склад", ДокументОбєкт.Склад.UnigueID.UGuid);

            string[] columnsName;
            List<Dictionary<string, object>> listNameRow;

            Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listNameRow);

            return listNameRow;
        }

        public static bool Spend(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
        {
            #region Підготовка

            List<string> СписокПомилок = new List<string>();

            Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();

            foreach (ПоверненняТоварівПостачальнику_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (ТовариРядок.Номенклатура.IsEmpty())
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                if (!(ТовариРядок.Кількість > 0))
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                СписокНоменклатури.Add(ТовариРядок.НомерРядка, ТовариРядок.Номенклатура.GetDirectoryObject()!);
            }

            #endregion

            #region Товари на складах

            ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();

            foreach (ПоверненняТоварівПостачальнику_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                {
                    ТовариНаСкладах_RecordsSet.Record record = new ТовариНаСкладах_RecordsSet.Record();
                    товариНаСкладах_RecordsSet.Records.Add(record);

                    record.Income = false;
                    record.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record.Номенклатура = ТовариРядок.Номенклатура;
                    record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record.Серія = ТовариРядок.Серія;
                    record.Склад = ДокументОбєкт.Склад;
                    record.ВНаявності = ТовариРядок.Кількість;
                }
            }

            товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

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
                        СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                            $"Немає доступних партій для товару {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                        return false;
                    }

                    decimal КількістьЯкуПотрібноПовернути = ТовариРядок.Кількість;

                    if (listNameRow.Count > 0)
                    {
                        Dictionary<string, object> nameRow = listNameRow[0];

                        decimal КількістьВПартії = (decimal)nameRow["Кількість"];
                        decimal СобівартістьПартії = (decimal)nameRow["Собівартість"];
                        ПартіяТоварівКомпозит_Pointer ПартіяТоварівКомпозит = new ПартіяТоварівКомпозит_Pointer(nameRow["ПартіяТоварівКомпозит"]);

                        if (КількістьЯкуПотрібноПовернути <= КількістьВПартії)
                        {
                            ПартіїТоварів_RecordsSet.Record record = new ПартіїТоварів_RecordsSet.Record();
                            партіїТоварів_RecordsSet.Records.Add(record);

                            record.Income = false;
                            record.Owner = ДокументОбєкт.UnigueID.UGuid;

                            record.Організація = ДокументОбєкт.Організація;
                            record.ПартіяТоварівКомпозит = ПартіяТоварівКомпозит;
                            record.Кількість = КількістьЯкуПотрібноПовернути;
                            record.Собівартість = (КількістьЯкуПотрібноПовернути == КількістьВПартії ? СобівартістьПартії : 0);
                            record.СписанаСобівартість = СобівартістьПартії;
                            record.Номенклатура = ТовариРядок.Номенклатура;
                            record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                            record.Серія = ТовариРядок.Серія;
                            record.Склад = ДокументОбєкт.Склад;
                        }
                        else
                        {
                            СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Кількість в партії менша чим кількість яку повертають для товару {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                            СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                            return false;
                        }
                    }
                    else
                    {
                        СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                             $"Не знайдені партії для товару {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                        return false;
                    }
                }
            }

            партіїТоварів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region ВільніЗалишки

            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();

            foreach (ПоверненняТоварівПостачальнику_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                {
                    ВільніЗалишки_RecordsSet.Record record = new ВільніЗалишки_RecordsSet.Record();
                    вільніЗалишки_RecordsSet.Records.Add(record);

                    record.Income = false;
                    record.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record.Номенклатура = ТовариРядок.Номенклатура;
                    record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record.Склад = ДокументОбєкт.Склад;
                    record.ВНаявності = ТовариРядок.Кількість;
                }
            }

            вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region РозрахункиЗПостачальниками

            РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РозрахункиЗПостачальниками_RecordsSet();

            РозрахункиЗПостачальниками_RecordsSet.Record розрахункиЗПостачальниками_Record = new РозрахункиЗПостачальниками_RecordsSet.Record();
            розрахункиЗПостачальниками_RecordsSet.Records.Add(розрахункиЗПостачальниками_Record);

            розрахункиЗПостачальниками_Record.Income = false;
            розрахункиЗПостачальниками_Record.Owner = ДокументОбєкт.UnigueID.UGuid;

            розрахункиЗПостачальниками_Record.Контрагент = ДокументОбєкт.Контрагент;
            розрахункиЗПостачальниками_Record.Валюта = ДокументОбєкт.Валюта;
            розрахункиЗПостачальниками_Record.Сума = ДокументОбєкт.СумаДокументу;

            розрахункиЗПостачальниками_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region Закупівлі

            Закупівлі_RecordsSet закупівлі_RecordsSet = new Закупівлі_RecordsSet();

            foreach (ПоверненняТоварівПостачальнику_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                Закупівлі_RecordsSet.Record record = new Закупівлі_RecordsSet.Record();
                закупівлі_RecordsSet.Records.Add(record);

                record.Owner = ДокументОбєкт.UnigueID.UGuid;
                record.Організація = ДокументОбєкт.Організація;
                record.Склад = ДокументОбєкт.Склад;
                record.Контрагент = ДокументОбєкт.Контрагент;
                record.Договір = ДокументОбєкт.Договір;
                record.Номенклатура = ТовариРядок.Номенклатура;
                record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                record.Кількість = -ТовариРядок.Кількість;
                record.Сума = -ТовариРядок.Сума;
                record.Собівартість = Math.Round(ТовариРядок.Сума / ТовариРядок.Кількість, 2);
            }

            закупівлі_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            return true;
        }

        public static void ClearSpend(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
        {
            ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();
            товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();
            партіїТоварів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();
            вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РозрахункиЗПостачальниками_RecordsSet();
            розрахункиЗПостачальниками_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            //
            // Обороти
            //

            Закупівлі_RecordsSet закупівлі_RecordsSet = new Закупівлі_RecordsSet();
            закупівлі_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    #endregion

    #region Каса

    class ПрихіднийКасовийОрдер_SpendTheDocument
    {
        public static bool Spend(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            #region Підготовка



            #endregion

            #region РозрахункиЗКлієнтами

            РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РозрахункиЗКлієнтами_RecordsSet();

            if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ПоступленняОплатиВідКлієнта)
            {
                РозрахункиЗКлієнтами_RecordsSet.Record record_Клієнт = new РозрахункиЗКлієнтами_RecordsSet.Record();
                розрахункиЗКлієнтами_RecordsSet.Records.Add(record_Клієнт);

                record_Клієнт.Income = false;
                record_Клієнт.Owner = ДокументОбєкт.UnigueID.UGuid;

                record_Клієнт.Контрагент = ДокументОбєкт.Контрагент;
                record_Клієнт.Валюта = ДокументОбєкт.Валюта;
                record_Клієнт.Сума = ДокументОбєкт.СумаДокументу;
            }

            розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region РозрахункиЗПостачальниками

            РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РозрахункиЗПостачальниками_RecordsSet();

            if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ПоверненняКоштівПостачальнику)
            {
                РозрахункиЗПостачальниками_RecordsSet.Record record_Постачальник = new РозрахункиЗПостачальниками_RecordsSet.Record();
                розрахункиЗПостачальниками_RecordsSet.Records.Add(record_Постачальник);

                record_Постачальник.Income = false;
                record_Постачальник.Owner = ДокументОбєкт.UnigueID.UGuid;

                record_Постачальник.Контрагент = ДокументОбєкт.Контрагент;
                record_Постачальник.Валюта = ДокументОбєкт.Валюта;
                record_Постачальник.Сума = ДокументОбєкт.СумаДокументу;
            }

            розрахункиЗПостачальниками_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region РухКоштів

            РухКоштів_RecordsSet рухКоштів_RecordsSet = new РухКоштів_RecordsSet();

            //Списання коштів з КасаВідправник
            if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ПоступленняКоштівЗІншоїКаси)
            {
                РухКоштів_RecordsSet.Record record_ІншаКаса = new РухКоштів_RecordsSet.Record();
                рухКоштів_RecordsSet.Records.Add(record_ІншаКаса);

                record_ІншаКаса.Income = false;
                record_ІншаКаса.Owner = ДокументОбєкт.UnigueID.UGuid;

                record_ІншаКаса.Організація = ДокументОбєкт.Організація;
                record_ІншаКаса.Каса = ДокументОбєкт.КасаВідправник;
                record_ІншаКаса.Валюта = ДокументОбєкт.Валюта;
                record_ІншаКаса.Сума = ДокументОбєкт.СумаДокументу;
            }

            //Списання коштів з банківського рахунку
            if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ПоступленняКоштівЗБанку)
            {
                // ...
            }

            //Поступлення коштів в касу
            РухКоштів_RecordsSet.Record record_РухКоштів = new РухКоштів_RecordsSet.Record();
            рухКоштів_RecordsSet.Records.Add(record_РухКоштів);

            record_РухКоштів.Income = true;
            record_РухКоштів.Owner = ДокументОбєкт.UnigueID.UGuid;

            record_РухКоштів.Організація = ДокументОбєкт.Організація;
            record_РухКоштів.Каса = ДокументОбєкт.Каса;
            record_РухКоштів.Валюта = ДокументОбєкт.Валюта;
            record_РухКоштів.Сума = ДокументОбєкт.СумаДокументу;

            рухКоштів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            return true;
        }

        public static void ClearSpend(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РозрахункиЗПостачальниками_RecordsSet();
            розрахункиЗПостачальниками_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РозрахункиЗКлієнтами_RecordsSet();
            розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            РухКоштів_RecordsSet рухКоштів_RecordsSet = new РухКоштів_RecordsSet();
            рухКоштів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class РозхіднийКасовийОрдер_SpendTheDocument
    {
        public static bool Spend(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            #region Підготовка



            #endregion

            #region РозрахункиЗКлієнтами

            РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РозрахункиЗКлієнтами_RecordsSet();

            if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ПоверненняОплатиКлієнту)
            {
                РозрахункиЗКлієнтами_RecordsSet.Record record_Клієнт = new РозрахункиЗКлієнтами_RecordsSet.Record();
                розрахункиЗКлієнтами_RecordsSet.Records.Add(record_Клієнт);

                record_Клієнт.Income = false;
                record_Клієнт.Owner = ДокументОбєкт.UnigueID.UGuid;

                record_Клієнт.Контрагент = ДокументОбєкт.Контрагент;
                record_Клієнт.Валюта = ДокументОбєкт.Валюта;
                record_Клієнт.Сума = ДокументОбєкт.СумаДокументу;
            }

            розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region РозрахункиЗПостачальниками

            РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РозрахункиЗПостачальниками_RecordsSet();

            if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ОплатаПостачальнику)
            {
                РозрахункиЗПостачальниками_RecordsSet.Record record_Постачальник = new РозрахункиЗПостачальниками_RecordsSet.Record();
                розрахункиЗПостачальниками_RecordsSet.Records.Add(record_Постачальник);

                record_Постачальник.Income = true;
                record_Постачальник.Owner = ДокументОбєкт.UnigueID.UGuid;

                record_Постачальник.Контрагент = ДокументОбєкт.Контрагент;
                record_Постачальник.Валюта = ДокументОбєкт.Валюта;
                record_Постачальник.Сума = ДокументОбєкт.СумаДокументу;
            }

            розрахункиЗПостачальниками_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region РухКоштів

            РухКоштів_RecordsSet рухКоштів_RecordsSet = new РухКоштів_RecordsSet();

            //Поступлення коштів в КасаОтримувач
            if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ВидачаКоштівВІншуКасу)
            {
                РухКоштів_RecordsSet.Record record_ІншаКаса = new РухКоштів_RecordsSet.Record();
                рухКоштів_RecordsSet.Records.Add(record_ІншаКаса);

                record_ІншаКаса.Income = true;
                record_ІншаКаса.Owner = ДокументОбєкт.UnigueID.UGuid;

                record_ІншаКаса.Організація = ДокументОбєкт.Організація;
                record_ІншаКаса.Каса = ДокументОбєкт.КасаОтримувач;
                record_ІншаКаса.Валюта = ДокументОбєкт.Валюта;
                record_ІншаКаса.Сума = ДокументОбєкт.СумаДокументу;
            }

            //Поступлення коштів на банківський рахунок
            if (ДокументОбєкт.ГосподарськаОперація == Перелічення.ГосподарськіОперації.ЗдачаКоштівВБанк)
            {
                // ...
            }

            //Списання коштів з каси
            РухКоштів_RecordsSet.Record record_РухКоштів = new РухКоштів_RecordsSet.Record();
            рухКоштів_RecordsSet.Records.Add(record_РухКоштів);

            record_РухКоштів.Income = false;
            record_РухКоштів.Owner = ДокументОбєкт.UnigueID.UGuid;

            record_РухКоштів.Організація = ДокументОбєкт.Організація;
            record_РухКоштів.Каса = ДокументОбєкт.Каса;
            record_РухКоштів.Валюта = ДокументОбєкт.Валюта;
            record_РухКоштів.Сума = ДокументОбєкт.СумаДокументу;

            рухКоштів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            return true;
        }

        public static void ClearSpend(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РозрахункиЗПостачальниками_RecordsSet();
            розрахункиЗПостачальниками_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РозрахункиЗКлієнтами_RecordsSet();
            розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            РухКоштів_RecordsSet рухКоштів_RecordsSet = new РухКоштів_RecordsSet();
            рухКоштів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    #endregion

    #region Інші

    class ПереміщенняТоварів_SpendTheDocument
    {
        public static bool Spend(ПереміщенняТоварів_Objest ДокументОбєкт)
        {
            #region Підготовка

            List<string> СписокПомилок = new List<string>();

            Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();
            Dictionary<int, decimal> ЗалишокНоменклатури = new Dictionary<int, decimal>();
            Dictionary<int, decimal> РезервНоменклатури = new Dictionary<int, decimal>();

            foreach (ПереміщенняТоварів_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (ТовариРядок.Номенклатура.IsEmpty())
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                if (!(ТовариРядок.Кількість > 0))
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                СписокНоменклатури.Add(ТовариРядок.НомерРядка, ТовариРядок.Номенклатура.GetDirectoryObject()!);

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
                            СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Залишок товару {ЗалишокНоменклатури[ТовариРядок.НомерРядка]}, потрібно {ТовариРядок.Кількість}");
                            СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                            return false;
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
                                СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                                    $"Залишок товару {ЗалишокНоменклатури[ТовариРядок.НомерРядка]}, " +
                                    $"в резерві {РезервНоменклатури[ТовариРядок.НомерРядка]}, " +
                                    $"потрібно {ТовариРядок.Кількість}");

                                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                                return false;
                            }
                        }
                    }
                    else
                    {
                        СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                            $"Відсутній товар {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                        return false;
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

                    ТовариНаСкладах_RecordsSet.Record record1 = new ТовариНаСкладах_RecordsSet.Record();
                    товариНаСкладах_RecordsSet.Records.Add(record1);

                    record1.Income = false;
                    record1.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record1.Номенклатура = ТовариРядок.Номенклатура;
                    record1.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record1.Склад = ДокументОбєкт.СкладВідправник;
                    record1.Серія = ТовариРядок.Серія;
                    record1.ВНаявності = ТовариРядок.Кількість;

                    //
                    //СкладОтримувач
                    //

                    ТовариНаСкладах_RecordsSet.Record record2 = new ТовариНаСкладах_RecordsSet.Record();
                    товариНаСкладах_RecordsSet.Records.Add(record2);

                    record2.Income = true;
                    record2.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record2.Номенклатура = ТовариРядок.Номенклатура;
                    record2.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record2.Склад = ДокументОбєкт.СкладОтримувач;
                    record2.Серія = ТовариРядок.Серія;
                    record2.ВНаявності = ТовариРядок.Кількість;
                }
            }

            товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

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

                    ВільніЗалишки_RecordsSet.Record record1 = new ВільніЗалишки_RecordsSet.Record();
                    вільніЗалишки_RecordsSet.Records.Add(record1);

                    record1.Income = false;
                    record1.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record1.Номенклатура = ТовариРядок.Номенклатура;
                    record1.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record1.Склад = ДокументОбєкт.СкладВідправник;
                    record1.ВНаявності = ТовариРядок.Кількість;

                    //
                    //СкладОтримувач
                    //

                    ВільніЗалишки_RecordsSet.Record record2 = new ВільніЗалишки_RecordsSet.Record();
                    вільніЗалишки_RecordsSet.Records.Add(record2);

                    record2.Income = true;
                    record2.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record2.Номенклатура = ТовариРядок.Номенклатура;
                    record2.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record2.Склад = ДокументОбєкт.СкладОтримувач;
                    record2.ВНаявності = ТовариРядок.Кількість;
                }
            }

            вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region Партії товарів

            ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();

            foreach (ПереміщенняТоварів_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                {
                    List<Dictionary<string, object>> listNameRow = СпільніФункції.ОтриматиСписокНаявнихПартій(
                        ДокументОбєкт.Організація, ТовариРядок.Номенклатура, ТовариРядок.ХарактеристикаНоменклатури,
                        ТовариРядок.Серія, ДокументОбєкт.СкладВідправник, ДокументОбєкт.UnigueID, ДокументОбєкт.ДатаДок, ТовариРядок.Кількість);

                    if (listNameRow.Count == 0)
                    {
                        СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                            $"Немає доступних партій для товару {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                        return false;
                    }

                    decimal КількістьЯкуПотрібноСписати = ТовариРядок.Кількість;

                    foreach (Dictionary<string, object> nameRow in listNameRow)
                    {
                        decimal КількістьВПартії = (decimal)nameRow["Кількість"];
                        decimal СобівартістьПартії = (decimal)nameRow["Собівартість"];
                        ПартіяТоварівКомпозит_Pointer ПартіяТоварівКомпозит = new ПартіяТоварівКомпозит_Pointer(nameRow["ПартіяТоварівКомпозит"]);

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

                        ПартіїТоварів_RecordsSet.Record record1 = new ПартіїТоварів_RecordsSet.Record();
                        партіїТоварів_RecordsSet.Records.Add(record1);

                        record1.Income = false;
                        record1.Owner = ДокументОбєкт.UnigueID.UGuid;

                        record1.Організація = ДокументОбєкт.Організація;
                        record1.ПартіяТоварівКомпозит = ПартіяТоварівКомпозит;
                        record1.Кількість = КількістьЩоСписується;
                        record1.Собівартість = ЗакритиПартію ? СобівартістьПартії : 0;
                        record1.СписанаСобівартість = СобівартістьПартії;
                        record1.Номенклатура = ТовариРядок.Номенклатура;
                        record1.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                        record1.Серія = ТовариРядок.Серія;
                        record1.Склад = ДокументОбєкт.СкладВідправник;

                        ПартіїТоварів_RecordsSet.Record record2 = new ПартіїТоварів_RecordsSet.Record();
                        партіїТоварів_RecordsSet.Records.Add(record2);

                        record2.Income = true;
                        record2.Owner = ДокументОбєкт.UnigueID.UGuid;

                        record2.Організація = ДокументОбєкт.Організація;
                        record2.ПартіяТоварівКомпозит = ПартіяТоварівКомпозит;
                        record2.Кількість = КількістьЩоСписується;
                        record2.Собівартість = record1.Собівартість;
                        record2.СписанаСобівартість = 0;
                        record2.Номенклатура = ТовариРядок.Номенклатура;
                        record2.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                        record2.Серія = ТовариРядок.Серія;
                        record2.Склад = ДокументОбєкт.СкладОтримувач;

                        if (КількістьЯкуПотрібноСписати == 0)
                            break;
                    }

                    if (КількістьЯкуПотрібноСписати > 0)
                    {
                        СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                            $"Невистачило списати {КількістьЯкуПотрібноСписати} товарів");
                        СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                        return false;
                    }
                }
            }

            партіїТоварів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            return true;
        }

        public static void ClearSpend(ПереміщенняТоварів_Objest ДокументОбєкт)
        {
            ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();
            товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();
            вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();
            партіїТоварів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class ВстановленняЦінНоменклатури_SpendTheDocument
    {
        public static bool Spend(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
        {
            #region Рух по регістрах

            ЦіниНоменклатури_RecordsSet ціниНоменклатури_RecordsSet = new ЦіниНоменклатури_RecordsSet();

            foreach (ВстановленняЦінНоменклатури_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (Товари_Record.Ціна > 0)
                {
                    ЦіниНоменклатури_RecordsSet.Record record = new ЦіниНоменклатури_RecordsSet.Record();
                    ціниНоменклатури_RecordsSet.Records.Add(record);

                    record.Номенклатура = Товари_Record.Номенклатура;
                    record.ХарактеристикаНоменклатури = Товари_Record.ХарактеристикаНоменклатури;
                    record.ВидЦіни = Товари_Record.ВидЦіни;

                    record.Ціна = Товари_Record.Ціна;
                    record.Пакування = Товари_Record.Пакування;
                    record.Валюта = ДокументОбєкт.Валюта;
                }
            }

            ціниНоменклатури_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            return true;
        }

        public static void ClearSpend(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
        {
            ЦіниНоменклатури_RecordsSet ціниНоменклатури_RecordsSet = new ЦіниНоменклатури_RecordsSet();
            ціниНоменклатури_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class ВведенняЗалишків_SpendTheDocument
    {
        public static bool Spend(ВведенняЗалишків_Objest ДокументОбєкт)
        {
            #region Підготовка

            List<string> СписокПомилок = new List<string>();

            Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();

            foreach (ВведенняЗалишків_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (ТовариРядок.Номенклатура.IsEmpty())
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                if (!(ТовариРядок.Кількість > 0))
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                СписокНоменклатури.Add(ТовариРядок.НомерРядка, ТовариРядок.Номенклатура.GetDirectoryObject()!);
            }

            #endregion

            #region Товари на складах

            ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();

            foreach (ВведенняЗалишків_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                {
                    ТовариНаСкладах_RecordsSet.Record record = new ТовариНаСкладах_RecordsSet.Record();
                    товариНаСкладах_RecordsSet.Records.Add(record);

                    record.Income = true;
                    record.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record.Номенклатура = ТовариРядок.Номенклатура;
                    record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record.Серія = ТовариРядок.Серія;
                    record.Склад = ДокументОбєкт.Склад;
                    record.ВНаявності = ТовариРядок.Кількість;
                }
            }

            товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region ВільніЗалишки

            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();

            foreach (ВведенняЗалишків_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                {
                    ВільніЗалишки_RecordsSet.Record record = new ВільніЗалишки_RecordsSet.Record();
                    вільніЗалишки_RecordsSet.Records.Add(record);

                    record.Income = true;
                    record.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record.Номенклатура = ТовариРядок.Номенклатура;
                    record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record.Склад = ДокументОбєкт.Склад;
                    record.ВНаявності = ТовариРядок.Кількість;
                }
            }

            вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region Партії товарів

            ПартіяТоварівКомпозит_Pointer ПартіяТоварівКомпозит = StorageAndTrade.ФункціїДляДокументів.ОтриматиПартіюТоварівКомпозит(
                 ДокументОбєкт.UnigueID.UGuid,
                 Перелічення.ТипДокументуПартіяТоварівКомпозит.ВведенняЗалишків,
                 null, ДокументОбєкт
            );

            ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();

            foreach (ВведенняЗалишків_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                {
                    ПартіїТоварів_RecordsSet.Record record = new ПартіїТоварів_RecordsSet.Record();
                    партіїТоварів_RecordsSet.Records.Add(record);

                    record.Income = true;
                    record.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record.Організація = ДокументОбєкт.Організація;
                    record.ПартіяТоварівКомпозит = ПартіяТоварівКомпозит;
                    record.Кількість = ТовариРядок.Кількість;
                    record.Собівартість = ТовариРядок.Ціна;
                    record.Номенклатура = ТовариРядок.Номенклатура;
                    record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record.Серія = ТовариРядок.Серія;
                    record.Склад = ДокументОбєкт.Склад;
                }
            }

            партіїТоварів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region РухКоштів

            РухКоштів_RecordsSet рухКоштів_RecordsSet = new РухКоштів_RecordsSet();

            foreach (ВведенняЗалишків_Каси_TablePart.Record Каси_Record in ДокументОбєкт.Каси_TablePart.Records)
            {
                РухКоштів_RecordsSet.Record record_Каса = new РухКоштів_RecordsSet.Record();
                рухКоштів_RecordsSet.Records.Add(record_Каса);

                Валюти_Pointer валютаКаси =
                    !Каси_Record.Каса.IsEmpty() ? Каси_Record.Каса.GetDirectoryObject()!.Валюта : ДокументОбєкт.Валюта;

                record_Каса.Income = true;
                record_Каса.Owner = ДокументОбєкт.UnigueID.UGuid;

                record_Каса.Організація = ДокументОбєкт.Організація;
                record_Каса.Каса = Каси_Record.Каса;
                record_Каса.Валюта = валютаКаси;
                record_Каса.Сума = Каси_Record.Сума;
            }

            рухКоштів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region РозрахункиЗКлієнтами та РозрахункиЗПостачальниками

            РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РозрахункиЗКлієнтами_RecordsSet();
            РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РозрахункиЗПостачальниками_RecordsSet();

            foreach (ВведенняЗалишків_РозрахункиЗКонтрагентами_TablePart.Record РозрахункиЗКонтрагентами_Record in ДокументОбєкт.РозрахункиЗКонтрагентами_TablePart.Records)
            {
                if (РозрахункиЗКонтрагентами_Record.ТипКонтрагента == Перелічення.ТипиКонтрагентів.Клієнт)
                {
                    РозрахункиЗКлієнтами_RecordsSet.Record record_Клієнт = new РозрахункиЗКлієнтами_RecordsSet.Record();
                    розрахункиЗКлієнтами_RecordsSet.Records.Add(record_Клієнт);

                    record_Клієнт.Income = false;
                    record_Клієнт.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record_Клієнт.Контрагент = РозрахункиЗКонтрагентами_Record.Контрагент;
                    record_Клієнт.Валюта = РозрахункиЗКонтрагентами_Record.Валюта;
                    record_Клієнт.Сума = РозрахункиЗКонтрагентами_Record.Сума;
                }

                if (РозрахункиЗКонтрагентами_Record.ТипКонтрагента == Перелічення.ТипиКонтрагентів.Постачальник)
                {
                    РозрахункиЗПостачальниками_RecordsSet.Record record_Постачальник = new РозрахункиЗПостачальниками_RecordsSet.Record();
                    розрахункиЗПостачальниками_RecordsSet.Records.Add(record_Постачальник);

                    record_Постачальник.Income = true;
                    record_Постачальник.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record_Постачальник.Контрагент = РозрахункиЗКонтрагентами_Record.Контрагент;
                    record_Постачальник.Валюта = РозрахункиЗКонтрагентами_Record.Валюта;
                    record_Постачальник.Сума = РозрахункиЗКонтрагентами_Record.Сума;
                }
            }

            розрахункиЗКлієнтами_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);
            розрахункиЗПостачальниками_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            return true;
        }

        public static void ClearSpend(ВведенняЗалишків_Objest ДокументОбєкт)
        {
            ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();
            товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();
            вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();
            партіїТоварів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            РухКоштів_RecordsSet рухКоштів_RecordsSet = new РухКоштів_RecordsSet();
            рухКоштів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            РозрахункиЗПостачальниками_RecordsSet розрахункиЗПостачальниками_RecordsSet = new РозрахункиЗПостачальниками_RecordsSet();
            розрахункиЗПостачальниками_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            РозрахункиЗКлієнтами_RecordsSet розрахункиЗКлієнтами_RecordsSet = new РозрахункиЗКлієнтами_RecordsSet();
            розрахункиЗКлієнтами_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class ВнутрішнєСпоживанняТоварів_SpendTheDocument
    {
        public static bool Spend(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
        {
            #region Підготовка

            List<string> СписокПомилок = new List<string>();

            Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();
            Dictionary<int, decimal> ЗалишокНоменклатури = new Dictionary<int, decimal>();
            Dictionary<int, decimal> РезервНоменклатури = new Dictionary<int, decimal>();

            foreach (ВнутрішнєСпоживанняТоварів_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (ТовариРядок.Номенклатура.IsEmpty())
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                if (!(ТовариРядок.Кількість > 0))
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                СписокНоменклатури.Add(ТовариРядок.НомерРядка, ТовариРядок.Номенклатура.GetDirectoryObject()!);

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
                            СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Залишок товару {ЗалишокНоменклатури[ТовариРядок.НомерРядка]}, потрібно {ТовариРядок.Кількість}");
                            СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                            return false;
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
                                СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                                    $"Залишок товару {ЗалишокНоменклатури[ТовариРядок.НомерРядка]}, " +
                                    $"в резерві {РезервНоменклатури[ТовариРядок.НомерРядка]}, " +
                                    $"потрібно {ТовариРядок.Кількість}");

                                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                                return false;
                            }
                        }
                    }
                    else
                    {
                        СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                            $"Відсутній товар {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                        return false;
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
                    ТовариНаСкладах_RecordsSet.Record record = new ТовариНаСкладах_RecordsSet.Record();
                    товариНаСкладах_RecordsSet.Records.Add(record);

                    record.Income = false;
                    record.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record.Номенклатура = ТовариРядок.Номенклатура;
                    record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record.Серія = ТовариРядок.Серія;
                    record.Склад = ДокументОбєкт.Склад;
                    record.ВНаявності = ТовариРядок.Кількість;
                }
            }

            товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region ВільніЗалишки

            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();

            foreach (ВнутрішнєСпоживанняТоварів_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                {
                    ВільніЗалишки_RecordsSet.Record record = new ВільніЗалишки_RecordsSet.Record();
                    вільніЗалишки_RecordsSet.Records.Add(record);

                    record.Income = false;
                    record.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record.Номенклатура = ТовариРядок.Номенклатура;
                    record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record.Склад = ДокументОбєкт.Склад;
                    record.ВНаявності = ТовариРядок.Кількість;
                }
            }

            вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region Партії товарів

            ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();

            foreach (ВнутрішнєСпоживанняТоварів_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                {
                    List<Dictionary<string, object>> listNameRow = СпільніФункції.ОтриматиСписокНаявнихПартій(
                        ДокументОбєкт.Організація, ТовариРядок.Номенклатура, ТовариРядок.ХарактеристикаНоменклатури,
                        ТовариРядок.Серія, ДокументОбєкт.Склад, ДокументОбєкт.UnigueID, ДокументОбєкт.ДатаДок, ТовариРядок.Кількість);

                    if (listNameRow.Count == 0)
                    {
                        СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                            $"Немає доступних партій для товару {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                        return false;
                    }

                    decimal КількістьЯкуПотрібноСписати = ТовариРядок.Кількість;

                    foreach (Dictionary<string, object> nameRow in listNameRow)
                    {
                        decimal КількістьВПартії = (decimal)nameRow["Кількість"];
                        decimal СобівартістьПартії = (decimal)nameRow["Собівартість"];
                        ПартіяТоварівКомпозит_Pointer ПартіяТоварівКомпозит = new ПартіяТоварівКомпозит_Pointer(nameRow["ПартіяТоварівКомпозит"]);

                        decimal КількістьЩоСписується = 0;
                        bool ЗакритиПартію = (КількістьЯкуПотрібноСписати >= КількістьВПартії);

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

                        ПартіїТоварів_RecordsSet.Record record = new ПартіїТоварів_RecordsSet.Record();
                        партіїТоварів_RecordsSet.Records.Add(record);

                        record.Income = false;
                        record.Owner = ДокументОбєкт.UnigueID.UGuid;

                        record.Організація = ДокументОбєкт.Організація;
                        record.ПартіяТоварівКомпозит = ПартіяТоварівКомпозит;
                        record.Кількість = КількістьЩоСписується;
                        record.Собівартість = ЗакритиПартію ? СобівартістьПартії : 0;
                        record.СписанаСобівартість = СобівартістьПартії;
                        record.Номенклатура = ТовариРядок.Номенклатура;
                        record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                        record.Серія = ТовариРядок.Серія;
                        record.Склад = ДокументОбєкт.Склад;

                        if (КількістьЯкуПотрібноСписати == 0)
                            break;
                    }

                    if (КількістьЯкуПотрібноСписати > 0)
                    {
                        СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                            $"Невистачило списати {КількістьЯкуПотрібноСписати} товарів");
                        СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                        return false;
                    }
                }
            }

            партіїТоварів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            return true;
        }

        public static void ClearSpend(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
        {
            ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();
            товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();
            вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();
            партіїТоварів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class ПсуванняТоварів_SpendTheDocument
    {
        public static bool Spend(ПсуванняТоварів_Objest ДокументОбєкт)
        {
            #region Підготовка

            List<string> СписокПомилок = new List<string>();

            Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();
            Dictionary<int, decimal> ЗалишокНоменклатури = new Dictionary<int, decimal>();
            Dictionary<int, decimal> РезервНоменклатури = new Dictionary<int, decimal>();

            foreach (ПсуванняТоварів_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (ТовариРядок.Номенклатура.IsEmpty())
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                if (!(ТовариРядок.Кількість > 0))
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                СписокНоменклатури.Add(ТовариРядок.НомерРядка, ТовариРядок.Номенклатура.GetDirectoryObject()!);

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
                            СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Залишок товару {ЗалишокНоменклатури[ТовариРядок.НомерРядка]}, потрібно {ТовариРядок.Кількість}");
                            СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                            return false;
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
                                СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                                    $"Залишок товару {ЗалишокНоменклатури[ТовариРядок.НомерРядка]}, " +
                                    $"в резерві {РезервНоменклатури[ТовариРядок.НомерРядка]}, " +
                                    $"потрібно {ТовариРядок.Кількість}");

                                СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                                return false;
                            }
                        }
                    }
                    else
                    {
                        СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                            $"Відсутній товар {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                        return false;
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
                    ТовариНаСкладах_RecordsSet.Record record = new ТовариНаСкладах_RecordsSet.Record();
                    товариНаСкладах_RecordsSet.Records.Add(record);

                    record.Income = false;
                    record.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record.Номенклатура = ТовариРядок.Номенклатура;
                    record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record.Серія = ТовариРядок.Серія;
                    record.Склад = ДокументОбєкт.Склад;
                    record.ВНаявності = ТовариРядок.Кількість;
                }
            }

            товариНаСкладах_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region ВільніЗалишки

            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();

            foreach (ПсуванняТоварів_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                {
                    ВільніЗалишки_RecordsSet.Record record = new ВільніЗалишки_RecordsSet.Record();
                    вільніЗалишки_RecordsSet.Records.Add(record);

                    record.Income = false;
                    record.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record.Номенклатура = ТовариРядок.Номенклатура;
                    record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record.Склад = ДокументОбєкт.Склад;
                    record.ВНаявності = ТовариРядок.Кількість;
                }
            }

            вільніЗалишки_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            #region Партії товарів

            ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();

            foreach (ПсуванняТоварів_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                {
                    List<Dictionary<string, object>> listNameRow = СпільніФункції.ОтриматиСписокНаявнихПартій(
                        ДокументОбєкт.Організація, ТовариРядок.Номенклатура, ТовариРядок.ХарактеристикаНоменклатури,
                        ТовариРядок.Серія, ДокументОбєкт.Склад, ДокументОбєкт.UnigueID, ДокументОбєкт.ДатаДок, ТовариРядок.Кількість);

                    if (listNameRow.Count == 0)
                    {
                        СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                            $"Немає доступних партій для товару {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                        return false;
                    }

                    decimal КількістьЯкуПотрібноСписати = ТовариРядок.Кількість;

                    foreach (Dictionary<string, object> nameRow in listNameRow)
                    {
                        decimal КількістьВПартії = (decimal)nameRow["Кількість"];
                        decimal СобівартістьПартії = (decimal)nameRow["Собівартість"];
                        ПартіяТоварівКомпозит_Pointer ПартіяТоварівКомпозит = new ПартіяТоварівКомпозит_Pointer(nameRow["ПартіяТоварівКомпозит"]);

                        decimal КількістьЩоСписується = 0;
                        bool ЗакритиПартію = (КількістьЯкуПотрібноСписати >= КількістьВПартії);

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

                        ПартіїТоварів_RecordsSet.Record record = new ПартіїТоварів_RecordsSet.Record();
                        партіїТоварів_RecordsSet.Records.Add(record);

                        record.Income = false;
                        record.Owner = ДокументОбєкт.UnigueID.UGuid;

                        record.Організація = ДокументОбєкт.Організація;
                        record.ПартіяТоварівКомпозит = ПартіяТоварівКомпозит;
                        record.Кількість = КількістьЩоСписується;
                        record.Собівартість = ЗакритиПартію ? СобівартістьПартії : 0;
                        record.СписанаСобівартість = СобівартістьПартії;
                        record.Номенклатура = ТовариРядок.Номенклатура;
                        record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                        record.Серія = ТовариРядок.Серія;
                        record.Склад = ДокументОбєкт.Склад;

                        if (КількістьЯкуПотрібноСписати == 0)
                            break;
                    }

                    if (КількістьЯкуПотрібноСписати > 0)
                    {
                        СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                            $"Невистачило списати {КількістьЯкуПотрібноСписати} товарів");
                        СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                        return false;
                    }
                }
            }

            партіїТоварів_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            return true;
        }

        public static void ClearSpend(ПсуванняТоварів_Objest ДокументОбєкт)
        {
            ТовариНаСкладах_RecordsSet товариНаСкладах_RecordsSet = new ТовариНаСкладах_RecordsSet();
            товариНаСкладах_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ВільніЗалишки_RecordsSet вільніЗалишки_RecordsSet = new ВільніЗалишки_RecordsSet();
            вільніЗалишки_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);

            ПартіїТоварів_RecordsSet партіїТоварів_RecordsSet = new ПартіїТоварів_RecordsSet();
            партіїТоварів_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    #endregion

    #region Адресне розміщення на складі

    class РозміщенняТоварівНаСкладі_SpendTheDocument
    {
        public static bool Spend(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            #region Підготовка

            List<string> СписокПомилок = new List<string>();

            Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();

            foreach (РозміщенняТоварівНаСкладі_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (ТовариРядок.Номенклатура.IsEmpty())
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                if (ТовариРядок.Комірка.IsEmpty())
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Комірка");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                if (!(ТовариРядок.Кількість > 0))
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                СписокНоменклатури.Add(ТовариРядок.НомерРядка, ТовариРядок.Номенклатура.GetDirectoryObject()!);
            }

            #endregion

            #region ТовариВКомірках

            ТовариВКомірках_RecordsSet товариВКомірках_RecordsSet = new ТовариВКомірках_RecordsSet();

            foreach (РозміщенняТоварівНаСкладі_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (СписокНоменклатури[ТовариРядок.НомерРядка].ТипНоменклатури == Перелічення.ТипиНоменклатури.Товар)
                {
                    ТовариВКомірках_RecordsSet.Record record = new ТовариВКомірках_RecordsSet.Record();
                    товариВКомірках_RecordsSet.Records.Add(record);

                    record.Income = true;
                    record.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record.Номенклатура = ТовариРядок.Номенклатура;
                    record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record.Пакування = ТовариРядок.Пакування;
                    record.Комірка = ТовариРядок.Комірка;
                    record.Серія = ТовариРядок.Серія;
                    record.ВНаявності = ТовариРядок.Кількість;
                }
            }

            товариВКомірках_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            return true;
        }

        public static void ClearSpend(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ТовариВКомірках_RecordsSet товариВКомірках_RecordsSet = new ТовариВКомірках_RecordsSet();
            товариВКомірках_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class ПереміщенняТоварівНаСкладі_SpendTheDocument
    {
        public static bool Spend(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            #region Підготовка

            List<string> СписокПомилок = new List<string>();

            Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();
            Dictionary<int, decimal> ЗалишокНоменклатури = new Dictionary<int, decimal>();

            foreach (ПереміщенняТоварівНаСкладі_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (ТовариРядок.Номенклатура.IsEmpty())
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                if (!(ТовариРядок.Кількість > 0))
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                СписокНоменклатури.Add(ТовариРядок.НомерРядка, ТовариРядок.Номенклатура.GetDirectoryObject()!);

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
                            СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Залишок товару {ЗалишокНоменклатури[ТовариРядок.НомерРядка]}, потрібно {ТовариРядок.Кількість}");

                            СписокПомилок.Add("\n");
                            СписокПомилок.Add($"Номенклатура {ТовариРядок.Номенклатура.GetPresentation()}");
                            СписокПомилок.Add($"Характеристика {ТовариРядок.ХарактеристикаНоменклатури.GetPresentation()}");
                            СписокПомилок.Add($"Пакування {ТовариРядок.Пакування.GetPresentation()}");
                            СписокПомилок.Add($"Комірка {ТовариРядок.КоміркаВідправник.GetPresentation()}");
                            СписокПомилок.Add($"Серія {ТовариРядок.Серія.GetPresentation()}");

                            СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                            return false;
                        }
                    }
                    else
                    {
                        СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                            $"Відсутній товар {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                        return false;
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
                        ТовариВКомірках_RecordsSet.Record record = new ТовариВКомірках_RecordsSet.Record();
                        товариВКомірках_RecordsSet.Records.Add(record);

                        record.Income = false;
                        record.Owner = ДокументОбєкт.UnigueID.UGuid;

                        record.Номенклатура = ТовариРядок.Номенклатура;
                        record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                        record.Пакування = ТовариРядок.Пакування;
                        record.Комірка = ТовариРядок.КоміркаВідправник;
                        record.Серія = ТовариРядок.Серія;
                        record.ВНаявності = ТовариРядок.Кількість;
                    }

                    //КоміркаОтримувач
                    {
                        ТовариВКомірках_RecordsSet.Record record = new ТовариВКомірках_RecordsSet.Record();
                        товариВКомірках_RecordsSet.Records.Add(record);

                        record.Income = true;
                        record.Owner = ДокументОбєкт.UnigueID.UGuid;

                        record.Номенклатура = ТовариРядок.Номенклатура;
                        record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                        record.Пакування = ТовариРядок.Пакування;
                        record.Комірка = ТовариРядок.КоміркаОтримувач;
                        record.Серія = ТовариРядок.Серія;
                        record.ВНаявності = ТовариРядок.Кількість;
                    }
                }
            }

            товариВКомірках_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            return true;
        }

        public static void ClearSpend(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ТовариВКомірках_RecordsSet товариВКомірках_RecordsSet = new ТовариВКомірках_RecordsSet();
            товариВКомірках_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class ЗбіркаТоварівНаСкладі_SpendTheDocument
    {
        public static bool Spend(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт)
        {
            #region Підготовка ОтриматиЗалишкиТоваруВКомірці

            List<string> СписокПомилок = new List<string>();
            Dictionary<int, decimal> ЗалишокНоменклатури = new Dictionary<int, decimal>();

            Dictionary<int, Номенклатура_Objest> СписокНоменклатури = new Dictionary<int, Номенклатура_Objest>();

            foreach (ЗбіркаТоварівНаСкладі_Товари_TablePart.Record ТовариРядок in ДокументОбєкт.Товари_TablePart.Records)
            {
                if (ТовариРядок.Номенклатура.IsEmpty())
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Номенклатура");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                if (ТовариРядок.Комірка.IsEmpty())
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Не заповнене поле Комірка");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                if (!(ТовариРядок.Кількість > 0))
                {
                    СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. Кількість має бути більшою 0");
                    СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                    return false;
                }

                СписокНоменклатури.Add(ТовариРядок.НомерРядка, ТовариРядок.Номенклатура.GetDirectoryObject()!);

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
                            СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                                $"Залишок товару {ЗалишокНоменклатури[ТовариРядок.НомерРядка]}, потрібно {ТовариРядок.Кількість}");

                            СписокПомилок.Add("\n");
                            СписокПомилок.Add($"Номенклатура {ТовариРядок.Номенклатура.GetPresentation()}");
                            СписокПомилок.Add($"Характеристика {ТовариРядок.ХарактеристикаНоменклатури.GetPresentation()}");
                            СписокПомилок.Add($"Пакування {ТовариРядок.Пакування.GetPresentation()}");
                            СписокПомилок.Add($"Комірка {ТовариРядок.Комірка.GetPresentation()}");
                            СписокПомилок.Add($"Серія {ТовариРядок.Серія.GetPresentation()}");

                            СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                            return false;
                        }
                    }
                    else
                    {
                        СписокПомилок.Add($"Рядок {ТовариРядок.НомерРядка}. " +
                            $"Відсутній товар {СписокНоменклатури[ТовариРядок.НомерРядка].Назва}");
                        СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, СписокПомилок);

                        return false;
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
                    ТовариВКомірках_RecordsSet.Record record = new ТовариВКомірках_RecordsSet.Record();
                    товариВКомірках_RecordsSet.Records.Add(record);

                    record.Income = false;
                    record.Owner = ДокументОбєкт.UnigueID.UGuid;

                    record.Номенклатура = ТовариРядок.Номенклатура;
                    record.ХарактеристикаНоменклатури = ТовариРядок.ХарактеристикаНоменклатури;
                    record.Пакування = ТовариРядок.Пакування;
                    record.Комірка = ТовариРядок.Комірка;
                    record.Серія = ТовариРядок.Серія;
                    record.ВНаявності = ТовариРядок.Кількість;
                }
            }

            товариВКомірках_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            return true;
        }

        public static void ClearSpend(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ТовариВКомірках_RecordsSet товариВКомірках_RecordsSet = new ТовариВКомірках_RecordsSet();
            товариВКомірках_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    class РозміщенняНоменклатуриПоКоміркам_SpendTheDocument
    {
        public static bool Spend(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт)
        {
            #region Рух по регістрах

            РозміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet розміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet = new РозміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet();

            foreach (РозміщенняНоменклатуриПоКоміркам_Товари_TablePart.Record Товари_Record in ДокументОбєкт.Товари_TablePart.Records)
            {
                РозміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet.Record record = new РозміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet.Record();
                розміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet.Records.Add(record);

                record.Номенклатура = Товари_Record.Номенклатура;
                //record.Склад = Товари_Record.Склад;
                //record.Приміщення = Товари_Record.Приміщення;
                record.Комірка = Товари_Record.Комірка;
            }

            розміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet.Save(ДокументОбєкт.ДатаДок, ДокументОбєкт.UnigueID.UGuid);

            #endregion

            return true;
        }

        public static void ClearSpend(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт)
        {
            РозміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet розміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet = new РозміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet();
            розміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet.Delete(ДокументОбєкт.UnigueID.UGuid);
        }
    }

    #endregion

}
