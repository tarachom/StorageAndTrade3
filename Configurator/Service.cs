/*
Copyright (C) 2019-2022 TARAKHOMYN YURIY IVANOVYCH
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

Модуль розрахунку віртуальних залишків.
Віртуальні залишки - це згруповані залишки за певний період (місяць, день)

*/

using System;
using System.Collections.Generic;

using AccountingSoftware;
using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade.Service
{
    class CalculationBalances
    {
        /// <summary>
        /// Функція добавляє в таблицю фонових задач нову задачу для обчислення залишків по регістрах
        /// </summary>
        /// <param name="documentUid">UID Документу</param>
        /// <param name="documentType">Тип документу</param>
        /// <param name="typeMovement">Тип руху по регістру (добавлення, видалення)</param>
        /// <param name="periodCalculation">Період розрахунку</param>
        /// <param name="userName">Користувач</param>
        public static void AddTask(string documentUid, string documentType, string typeMovement, DateTime periodCalculation, string userName)
        {
            if (Системні.ЗупинитиФоновіЗадачі_Const)
                return;

            Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart обчисленняВіртуальнихЗалишків_TablePart =
                new Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart();

            Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.Record record =
                new Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.Record();

            record.Дата = DateTime.Now;
            record.Документ = documentUid;
            record.ТипДокументу = documentType;
            record.ПеріодОбчислення = periodCalculation;
            record.ТипРухуПоРегістру = typeMovement;
            record.Користувач = userName;

            обчисленняВіртуальнихЗалишків_TablePart.Records.Add(record);
            обчисленняВіртуальнихЗалишків_TablePart.Save(false);
        }

        /// <summary>
        /// Видалити всі задачі
        /// </summary>
        public static void ClearAllTask()
        {
            //Очистка
            string queryClear = $@"
DELETE FROM {Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.TABLE}
";
            Config.KernelBackgroundTask.DataBase.ExecuteSQL(queryClear);
        }

        public static void ПідключитиДодаток_UUID_OSSP()
        {
            string query = "CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"";
            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

            /*
            --CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
            --SELECT uuid_generate_v4();

            --CREATE EXTENSION IF NOT EXISTS "pgcrypto";
            --SELECT gen_random_uuid(), uuid_generate_v4(); 
            */
        }

        /// <summary>
        /// Список віртуальних регістрів для яких відбувається обчислення
        /// </summary>
        public static readonly string[] СписокДоступнихВіртуальнихРегістрів = new string[] 
        {
            "ЗамовленняКлієнтів",
            "ТовариНаСкладах",
            "ПартіїТоварів",
            "РозрахункиЗКлієнтами",
            "РозрахункиЗПостачальниками",
            "ЗамовленняПостачальникам",
            "ВільніЗалишки",
            "РухКоштів",
        };


        /// <summary>
        /// Функція обчислює залишки за весь період групуючи їх по днях
        /// для кожного регістру окремо
        /// </summary>
        /// <param name="registerAccumulation">Регістер для якого розраховуються залишки по днях</param>
        public static void ОбчисленняВіртуальнихЗалишківПоВсіхДнях(string registerAccumulation)
        {
            switch (registerAccumulation)
            {
                case "ЗамовленняКлієнтів":
                    {
                        string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.TABLE};

INSERT INTO {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.ХарактеристикаНоменклатури},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Замовлено},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Сума}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('day', Рег_ЗамовленняКлієнтів.period::timestamp) as period_day,
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Номенклатура} AS Номенклатура, 
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Склад} AS Склад,
    SUM(CASE WHEN Рег_ЗамовленняКлієнтів.income = true THEN 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Замовлено} ELSE 
       -Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Замовлено} END) AS Замовлено,
    SUM(CASE WHEN Рег_ЗамовленняКлієнтів.income = true THEN 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Сума} ELSE 
       -Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Сума} END) AS Сума
FROM 
    {ЗамовленняКлієнтів_Const.TABLE} AS Рег_ЗамовленняКлієнтів
GROUP BY 
    period_day, Номенклатура, ХарактеристикаНоменклатури, Склад
HAVING 
   SUM(CASE WHEN Рег_ЗамовленняКлієнтів.income = true THEN 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Замовлено} ELSE 
       -Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Замовлено} END) != 0
OR
    SUM(CASE WHEN Рег_ЗамовленняКлієнтів.income = true THEN 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Сума} ELSE 
       -Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Сума} END) != 0
";

                        Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                        break;
                    }
                case "ТовариНаСкладах":
                    {
                        string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.TABLE};

INSERT INTO {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ХарактеристикаНоменклатури},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Серія},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ВНаявності},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ДоВідвантаження}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('day', Рег_ТовариНаСкладах.period::timestamp) as period_day,
    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Номенклатура} AS Номенклатура, 
    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Склад} AS Склад,
    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Серія} AS Серія,
    SUM(CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} ELSE 
       -Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} END) AS ВНаявності,
    SUM(CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ДоВідвантаження} ELSE 
       -Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ДоВідвантаження} END) AS ДоВідвантаження
FROM 
    {ТовариНаСкладах_Const.TABLE} AS Рег_ТовариНаСкладах
GROUP BY 
    period_day, Номенклатура, ХарактеристикаНоменклатури, Склад, Серія
HAVING 
   SUM(CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} ELSE 
       -Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} END) != 0
OR
    SUM(CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ДоВідвантаження} ELSE 
       -Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ДоВідвантаження} END) != 0
";

                        Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                        break;
                    }
                case "ПартіїТоварів":
                    {
                        string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.TABLE};

INSERT INTO {ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Організація},
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.ПартіяТоварівКомпозит},
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.ХарактеристикаНоменклатури},
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Серія},
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Кількість},
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Собівартість}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('day', Рег_ПартіїТоварів.period::timestamp) as period_day,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Організація} AS Організація, 
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.ПартіяТоварівКомпозит} AS ПартіяТоварівКомпозит, 
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Номенклатура} AS Номенклатура, 
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Серія} AS Серія,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Склад} AS Склад,
    SUM(CASE WHEN Рег_ПартіїТоварів.income = true THEN 
        Рег_ПартіїТоварів.{ПартіїТоварів_Const.Кількість} ELSE 
       -Рег_ПартіїТоварів.{ПартіїТоварів_Const.Кількість} END) AS Кількість,
    SUM(CASE WHEN Рег_ПартіїТоварів.income = true THEN 
        Рег_ПартіїТоварів.{ПартіїТоварів_Const.Собівартість} ELSE 
       -Рег_ПартіїТоварів.{ПартіїТоварів_Const.Собівартість} END) AS Собівартість
FROM 
    {ПартіїТоварів_Const.TABLE} AS Рег_ПартіїТоварів
GROUP BY 
    period_day, Організація, ПартіяТоварівКомпозит, Номенклатура, ХарактеристикаНоменклатури, Серія, Склад
HAVING 
   SUM(CASE WHEN Рег_ПартіїТоварів.income = true THEN 
        Рег_ПартіїТоварів.{ПартіїТоварів_Const.Кількість} ELSE 
       -Рег_ПартіїТоварів.{ПартіїТоварів_Const.Кількість} END) != 0
OR
    SUM(CASE WHEN Рег_ПартіїТоварів.income = true THEN 
        Рег_ПартіїТоварів.{ПартіїТоварів_Const.Собівартість} ELSE 
       -Рег_ПартіїТоварів.{ПартіїТоварів_Const.Собівартість} END) != 0
";

                        Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                        break;
                    }
                case "РозрахункиЗКлієнтами":
                    {
                        string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.TABLE};

INSERT INTO {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Валюта},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Контрагент},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Сума}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('day', Рег_РозрахункиЗКлієнтами.period::timestamp) as period_day,
    Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Валюта} AS Валюта, 
    Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Контрагент} AS Контрагент,
    SUM(CASE WHEN Рег_РозрахункиЗКлієнтами.income = true THEN 
        Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} ELSE 
       -Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} END) AS Сума
FROM 
    {РозрахункиЗКлієнтами_Const.TABLE} AS Рег_РозрахункиЗКлієнтами
GROUP BY 
    period_day, Валюта, Контрагент
HAVING
   SUM(CASE WHEN Рег_РозрахункиЗКлієнтами.income = true THEN 
        Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} ELSE 
       -Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} END) != 0
";

                        Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                        break;
                    }
                case "РозрахункиЗПостачальниками":
                    {
                        string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.TABLE};

INSERT INTO {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Валюта},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Контрагент},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Сума}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('day', Рег_РозрахункиЗПостачальниками.period::timestamp) as period_day,
    Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Валюта} AS Валюта, 
    Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Контрагент} AS Контрагент,
    SUM(CASE WHEN Рег_РозрахункиЗПостачальниками.income = true THEN 
        Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} ELSE 
       -Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} END) AS Сума
FROM 
    {РозрахункиЗПостачальниками_Const.TABLE} AS Рег_РозрахункиЗПостачальниками
GROUP BY 
    period_day, Валюта, Контрагент
HAVING
   SUM(CASE WHEN Рег_РозрахункиЗПостачальниками.income = true THEN 
        Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} ELSE 
       -Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} END) != 0
";

                        Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                        break;
                    }
                case "ЗамовленняПостачальникам":
                    {
                        string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.TABLE};

INSERT INTO {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.ХарактеристикаНоменклатури},
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Замовлено}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('day', Рег_ЗамовленняПостачальникам.period::timestamp) as period_day,
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Номенклатура} AS Номенклатура, 
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Склад} AS Склад,
    SUM(CASE WHEN Рег_ЗамовленняПостачальникам.income = true THEN 
        Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} ELSE 
       -Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} END) AS Замовлено
FROM 
    {ЗамовленняПостачальникам_Const.TABLE} AS Рег_ЗамовленняПостачальникам
GROUP BY 
    period_day, Номенклатура, ХарактеристикаНоменклатури, Склад
HAVING 
   SUM(CASE WHEN Рег_ЗамовленняПостачальникам.income = true THEN 
        Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} ELSE 
       -Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} END) != 0
";

                        Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                        break;
                    }
                case "ВільніЗалишки":
                    {
                        string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.TABLE};

INSERT INTO {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ХарактеристикаНоменклатури},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ВНаявності},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ВРезервіЗіСкладу},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ВРезервіПідЗамовлення}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('day', Рег_ВільніЗалишки.period::timestamp) as period_day,
    Рег_ВільніЗалишки.{ВільніЗалишки_Const.Номенклатура} AS Номенклатура, 
    Рег_ВільніЗалишки.{ВільніЗалишки_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Рег_ВільніЗалишки.{ВільніЗалишки_Const.Склад} AS Склад,
    SUM(CASE WHEN Рег_ВільніЗалишки.income = true THEN 
        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВНаявності} ELSE 
       -Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВНаявності} END) AS ВНаявності,
    SUM(CASE WHEN Рег_ВільніЗалишки.income = true THEN 
        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіЗіСкладу} ELSE 
       -Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіЗіСкладу} END) AS ВРезервіЗіСкладу,
    SUM(CASE WHEN Рег_ВільніЗалишки.income = true THEN 
        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіПідЗамовлення} ELSE 
       -Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіПідЗамовлення} END) AS ВРезервіПідЗамовлення
FROM 
    {ВільніЗалишки_Const.TABLE} AS Рег_ВільніЗалишки
GROUP BY 
    period_day, Номенклатура, ХарактеристикаНоменклатури, Склад
HAVING 
   SUM(CASE WHEN Рег_ВільніЗалишки.income = true THEN 
        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВНаявності} ELSE 
       -Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВНаявності} END) != 0
OR
    SUM(CASE WHEN Рег_ВільніЗалишки.income = true THEN 
        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіЗіСкладу} ELSE 
       -Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіЗіСкладу} END) != 0
OR
    SUM(CASE WHEN Рег_ВільніЗалишки.income = true THEN 
        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіПідЗамовлення} ELSE 
       -Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіПідЗамовлення} END) != 0
";

                        Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                        break;
                    }
                case "РухКоштів":
                    {
                        string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.TABLE};

INSERT INTO {ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Організація},
    {ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Каса},
    {ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Валюта},
    {ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Сума}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('day', Рег_РухКоштів.period::timestamp) as period_day,
    Рег_РухКоштів.{РухКоштів_Const.Організація} AS Організація, 
    Рег_РухКоштів.{РухКоштів_Const.Каса} AS Каса,
    Рег_РухКоштів.{РухКоштів_Const.Валюта} AS Валюта,
    SUM(CASE WHEN Рег_РухКоштів.income = true THEN 
        Рег_РухКоштів.{РухКоштів_Const.Сума} ELSE 
       -Рег_РухКоштів.{РухКоштів_Const.Сума} END) AS Сума
FROM 
    {РухКоштів_Const.TABLE} AS Рег_РухКоштів
GROUP BY 
    period_day, Організація, Каса, Валюта
HAVING 
   SUM(CASE WHEN Рег_РухКоштів.income = true THEN 
        Рег_РухКоштів.{РухКоштів_Const.Сума} ELSE 
       -Рег_РухКоштів.{РухКоштів_Const.Сума} END) != 0
";

                        Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                        break;
                    }
            }
        }

        /// <summary>
        /// Функція скидає актуальність залишків по місяцях та очищує відповідно залишки по місяцях. 
        /// Список місців обчислюється із залишків по днях.
        /// </summary>
        /// <param name="registerAccumulation">Регістер</param>
        public static void СкинутиЗначенняАктуальностіВіртуальнихЗалишківПоВсіхМісяцях(string registerAccumulation)
        {
            string query = $@"
DELETE FROM {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.TABLE}
WHERE {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.Регістр} = '{registerAccumulation}';
";
            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

            query = $@"
INSERT INTO {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.TABLE}
(
    uid,
    {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.Регістр},
    {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.Місяць},
    {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.Актуально}
)";
            //Таблична частина День з конфігурації
            ConfigurationObjectTablePart tablePartDay = Config.Kernel.Conf.ConstantsBlock["ВіртуальніТаблиціРегістрів"].Constants[registerAccumulation].TabularParts["День"];

            string tableDay = tablePartDay.Table;
            string fieldDay = tablePartDay.Fields["Період"].NameInTable;

            query += $@"
WITH register AS (
    SELECT DISTINCT date_trunc('month', {fieldDay}::timestamp) AS period_month FROM {tableDay}
)
SELECT
    uuid_generate_v4(),
    '{registerAccumulation}',
    register.period_month,
    false
FROM
    register
";

            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

            //Таблична частина Місяць з конфігурації
            ConfigurationObjectTablePart tablePartMonth = Config.Kernel.Conf.ConstantsBlock["ВіртуальніТаблиціРегістрів"].Constants[registerAccumulation].TabularParts["Місяць"];

            string tableMonth = tablePartMonth.Table;

            //Запит очищення таблиць Місяць
            string clearQueryMonth = $@"
DELETE FROM {tableMonth};
";
            Config.KernelBackgroundTask.DataBase.ExecuteSQL(clearQueryMonth);
        }


        /// <summary>
        /// Функція перевіряє список фонових задач для обчислення віртуальних залишків
        /// та обчислює залишки на дату проведення документу.
        /// Алгоритм обчислення:
        /// 1. Отримати список задач
        /// 2. Отримати список регістрів доступних для документу який вказаний в задачі
        /// 3. Розрахувати залишки на дату проведення документу по всіх доступних регістрах
        /// 4. Зафікусувати що задача виконана
        /// </summary>
        public static void ОбчисленняВіртуальнихЗалишківПоДнях()
        {
            //
            //Вибірка задач для обчислення
            //

            string querySelectTask = $@"
SELECT
    Задачі.uid,
    Задачі.{Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.Дата} AS Дата,
    Задачі.{Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.Документ} AS Документ,
    Задачі.{Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.ТипДокументу} AS ТипДокументу,
    to_char(Задачі.{Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.ПеріодОбчислення}, 'YYYY-MM-DD') AS Період
FROM
    {Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.TABLE} AS Задачі
WHERE
    Задачі.{Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.Виконано} = false
ORDER BY Дата ASC
";
            Dictionary<string, List<string>> documentProcessedList = new Dictionary<string, List<string>>();

            string[] columnsName;
            List<object[]> listRow;

            Config.KernelBackgroundTask.DataBase.SelectRequest(querySelectTask, null, out columnsName, out listRow);

            Config.KernelBackgroundTask.DataBase.BeginTransaction();

            //Обробка задач
            foreach (object[] row in listRow)
            {
                string uid = row[0].ToString();
                string Дата = row[1].ToString();
                string Документ = row[2].ToString();
                string ТипДокументу = row[3].ToString();
                string Період = row[4].ToString();

                bool documentProcessed = false;

                if (documentProcessedList.ContainsKey(Період))
                {
                    if (documentProcessedList[Період].Contains(ТипДокументу))
                    {
                        documentProcessed = true;
                    }
                    else
                        documentProcessedList[Період].Add(ТипДокументу);
                }
                else
                    documentProcessedList.Add(Період, new List<string>() { ТипДокументу });

                if (!documentProcessed)
                {
                    //Список регістрів доступних для документу
                    List<string> allowRegisterAccumulation = Config.Kernel.Conf.Documents[ТипДокументу].AllowRegisterAccumulation;

                    foreach (string registerAccumulation in allowRegisterAccumulation)
                    {
                        switch (registerAccumulation)
                        {
                            case "ЗамовленняКлієнтів":
                                {
                                    string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.TABLE}
WHERE date_trunc('day', {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Період}::timestamp) = '{Період}';

INSERT INTO {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.ХарактеристикаНоменклатури},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Замовлено},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Сума}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('day', Рег_ЗамовленняКлієнтів.period::timestamp) as period_day,
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Номенклатура} AS Номенклатура, 
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Склад} AS Склад,
    SUM(CASE WHEN Рег_ЗамовленняКлієнтів.income = true THEN 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Замовлено} ELSE 
       -Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Замовлено} END) AS Замовлено,
    SUM(CASE WHEN Рег_ЗамовленняКлієнтів.income = true THEN 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Сума} ELSE 
       -Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Сума} END) AS Сума
FROM 
    {ЗамовленняКлієнтів_Const.TABLE} AS Рег_ЗамовленняКлієнтів
WHERE
    date_trunc('day', Рег_ЗамовленняКлієнтів.period::timestamp) = '{Період}'
GROUP BY 
    period_day, Номенклатура, ХарактеристикаНоменклатури, Склад
HAVING 
   SUM(CASE WHEN Рег_ЗамовленняКлієнтів.income = true THEN 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Замовлено} ELSE 
       -Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Замовлено} END) != 0
OR
    SUM(CASE WHEN Рег_ЗамовленняКлієнтів.income = true THEN 
        Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Сума} ELSE 
       -Рег_ЗамовленняКлієнтів.{ЗамовленняКлієнтів_Const.Сума} END) != 0
";

                                    Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                                    break;
                                }
                            case "ТовариНаСкладах":
                                {
                                    string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.TABLE}
WHERE date_trunc('day', {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Період}::timestamp) = '{Період}';

INSERT INTO {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ХарактеристикаНоменклатури},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Серія},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ВНаявності},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ДоВідвантаження}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('day', Рег_ТовариНаСкладах.period::timestamp) as period_day,
    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Номенклатура} AS Номенклатура, 
    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Склад} AS Склад,
    Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.Серія} AS Серія,
    SUM(CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} ELSE 
       -Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} END) AS ВНаявності,
    SUM(CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ДоВідвантаження} ELSE 
       -Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ДоВідвантаження} END) AS ДоВідвантаження
FROM 
    {ТовариНаСкладах_Const.TABLE} AS Рег_ТовариНаСкладах
WHERE
    date_trunc('day', Рег_ТовариНаСкладах.period::timestamp) = '{Період}'
GROUP BY 
    period_day, Номенклатура, ХарактеристикаНоменклатури, Склад, Серія
HAVING 
   SUM(CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} ELSE 
       -Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ВНаявності} END) != 0
OR
    SUM(CASE WHEN Рег_ТовариНаСкладах.income = true THEN 
        Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ДоВідвантаження} ELSE 
       -Рег_ТовариНаСкладах.{ТовариНаСкладах_Const.ДоВідвантаження} END) != 0
";

                                    Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                                    break;
                                }
                            case "ПартіїТоварів":
                                {
                                    string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.TABLE}
WHERE date_trunc('day', {ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Період}::timestamp) = '{Період}';

INSERT INTO {ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Організація},
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.ПартіяТоварівКомпозит},
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.ХарактеристикаНоменклатури},
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Серія},
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Кількість},
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Собівартість}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('day', Рег_ПартіїТоварів.period::timestamp) as period_day,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Організація} AS Організація, 
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.ПартіяТоварівКомпозит} AS ПартіяТоварівКомпозит, 
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Номенклатура} AS Номенклатура, 
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Серія} AS Серія,
    Рег_ПартіїТоварів.{ПартіїТоварів_Const.Склад} AS Склад,
    SUM(CASE WHEN Рег_ПартіїТоварів.income = true THEN 
        Рег_ПартіїТоварів.{ПартіїТоварів_Const.Кількість} ELSE 
       -Рег_ПартіїТоварів.{ПартіїТоварів_Const.Кількість} END) AS Кількість,
    SUM(CASE WHEN Рег_ПартіїТоварів.income = true THEN 
        Рег_ПартіїТоварів.{ПартіїТоварів_Const.Собівартість} ELSE 
       -Рег_ПартіїТоварів.{ПартіїТоварів_Const.Собівартість} END) AS Собівартість
FROM 
    {ПартіїТоварів_Const.TABLE} AS Рег_ПартіїТоварів
WHERE
    date_trunc('day', Рег_ПартіїТоварів.period::timestamp) = '{Період}'
GROUP BY 
    period_day, Організація, ПартіяТоварівКомпозит, Номенклатура, ХарактеристикаНоменклатури, Серія, Склад
HAVING 
   SUM(CASE WHEN Рег_ПартіїТоварів.income = true THEN 
        Рег_ПартіїТоварів.{ПартіїТоварів_Const.Кількість} ELSE 
       -Рег_ПартіїТоварів.{ПартіїТоварів_Const.Кількість} END) != 0
OR
    SUM(CASE WHEN Рег_ПартіїТоварів.income = true THEN 
        Рег_ПартіїТоварів.{ПартіїТоварів_Const.Собівартість} ELSE 
       -Рег_ПартіїТоварів.{ПартіїТоварів_Const.Собівартість} END) != 0
";

                                    Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                                    break;
                                }
                            case "РозрахункиЗКлієнтами":
                                {
                                    string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.TABLE}
WHERE date_trunc('day', {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Період}::timestamp) = '{Період}';

INSERT INTO {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Валюта},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Контрагент},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Сума}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('day', Рег_РозрахункиЗКлієнтами.period::timestamp) as period_day,
    Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Валюта} AS Валюта, 
    Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Контрагент} AS Контрагент,
    SUM(CASE WHEN Рег_РозрахункиЗКлієнтами.income = true THEN 
        Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} ELSE 
       -Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} END) AS Сума
FROM 
    {РозрахункиЗКлієнтами_Const.TABLE} AS Рег_РозрахункиЗКлієнтами
WHERE
    date_trunc('day', Рег_РозрахункиЗКлієнтами.period::timestamp) = '{Період}'
GROUP BY 
    period_day, Валюта, Контрагент
HAVING
   SUM(CASE WHEN Рег_РозрахункиЗКлієнтами.income = true THEN 
        Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} ELSE 
       -Рег_РозрахункиЗКлієнтами.{РозрахункиЗКлієнтами_Const.Сума} END) != 0
";

                                    Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                                    break;
                                }
                            case "РозрахункиЗПостачальниками":
                                {
                                    string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.TABLE}
WHERE date_trunc('day', {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Період}::timestamp) = '{Період}';

INSERT INTO {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Валюта},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Контрагент},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Сума}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('day', Рег_РозрахункиЗПостачальниками.period::timestamp) as period_day,
    Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Валюта} AS Валюта, 
    Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Контрагент} AS Контрагент,
    SUM(CASE WHEN Рег_РозрахункиЗПостачальниками.income = true THEN 
        Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} ELSE 
       -Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} END) AS Сума
FROM 
    {РозрахункиЗПостачальниками_Const.TABLE} AS Рег_РозрахункиЗПостачальниками
WHERE
    date_trunc('day', Рег_РозрахункиЗПостачальниками.period::timestamp) = '{Період}'
GROUP BY 
    period_day, Валюта, Контрагент
HAVING
   SUM(CASE WHEN Рег_РозрахункиЗПостачальниками.income = true THEN 
        Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} ELSE 
       -Рег_РозрахункиЗПостачальниками.{РозрахункиЗПостачальниками_Const.Сума} END) != 0
";

                                    Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                                    break;
                                }
                            case "ЗамовленняПостачальникам":
                                {
                                    string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.TABLE}
WHERE date_trunc('day', {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Період}::timestamp) = '{Період}';

INSERT INTO {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.ХарактеристикаНоменклатури},
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Замовлено}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('day', Рег_ЗамовленняПостачальникам.period::timestamp) as period_day,
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Номенклатура} AS Номенклатура, 
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Склад} AS Склад,
    SUM(CASE WHEN Рег_ЗамовленняПостачальникам.income = true THEN 
        Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} ELSE 
       -Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} END) AS Замовлено
FROM 
    {ЗамовленняПостачальникам_Const.TABLE} AS Рег_ЗамовленняПостачальникам
WHERE
    date_trunc('day', Рег_ЗамовленняПостачальникам.period::timestamp) = '{Період}'
GROUP BY 
    period_day, Номенклатура, ХарактеристикаНоменклатури, Склад
HAVING 
   SUM(CASE WHEN Рег_ЗамовленняПостачальникам.income = true THEN 
        Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} ELSE 
       -Рег_ЗамовленняПостачальникам.{ЗамовленняПостачальникам_Const.Замовлено} END) != 0
";

                                    Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                                    break;
                                }
                            case "ВільніЗалишки":
                                {
                                    string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.TABLE}
WHERE date_trunc('day', {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.Період}::timestamp) = '{Період}';

INSERT INTO {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ХарактеристикаНоменклатури},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ВНаявності},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ВРезервіЗіСкладу},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ВРезервіПідЗамовлення}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('day', Рег_ВільніЗалишки.period::timestamp) as period_day,
    Рег_ВільніЗалишки.{ВільніЗалишки_Const.Номенклатура} AS Номенклатура, 
    Рег_ВільніЗалишки.{ВільніЗалишки_Const.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    Рег_ВільніЗалишки.{ВільніЗалишки_Const.Склад} AS Склад,
    SUM(CASE WHEN Рег_ВільніЗалишки.income = true THEN 
        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВНаявності} ELSE 
       -Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВНаявності} END) AS ВНаявності,
    SUM(CASE WHEN Рег_ВільніЗалишки.income = true THEN 
        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіЗіСкладу} ELSE 
       -Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіЗіСкладу} END) AS ВРезервіЗіСкладу,
    SUM(CASE WHEN Рег_ВільніЗалишки.income = true THEN 
        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіПідЗамовлення} ELSE 
       -Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіПідЗамовлення} END) AS ВРезервіПідЗамовлення
FROM 
    {ВільніЗалишки_Const.TABLE} AS Рег_ВільніЗалишки
WHERE
    date_trunc('day', Рег_ВільніЗалишки.period::timestamp) = '{Період}'
GROUP BY 
    period_day, Номенклатура, ХарактеристикаНоменклатури, Склад
HAVING 
   SUM(CASE WHEN Рег_ВільніЗалишки.income = true THEN 
        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВНаявності} ELSE 
       -Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВНаявності} END) != 0
OR
    SUM(CASE WHEN Рег_ВільніЗалишки.income = true THEN 
        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіЗіСкладу} ELSE 
       -Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіЗіСкладу} END) != 0
OR
    SUM(CASE WHEN Рег_ВільніЗалишки.income = true THEN 
        Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіПідЗамовлення} ELSE 
       -Рег_ВільніЗалишки.{ВільніЗалишки_Const.ВРезервіПідЗамовлення} END) != 0
";

                                    Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                                    break;
                                }
                            case "РухКоштів":
                                {
                                    string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.TABLE}
WHERE date_trunc('day', {ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Період}::timestamp) = '{Період}';

INSERT INTO {ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Організація},
    {ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Каса},
    {ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Валюта},
    {ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Сума}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('day', Рег_РухКоштів.period::timestamp) as period_day,
    Рег_РухКоштів.{РухКоштів_Const.Організація} AS Організація, 
    Рег_РухКоштів.{РухКоштів_Const.Каса} AS Каса,
    Рег_РухКоштів.{РухКоштів_Const.Валюта} AS Валюта,
    SUM(CASE WHEN Рег_РухКоштів.income = true THEN 
        Рег_РухКоштів.{РухКоштів_Const.Сума} ELSE 
       -Рег_РухКоштів.{РухКоштів_Const.Сума} END) AS Сума
FROM 
    {РухКоштів_Const.TABLE} AS Рег_РухКоштів
WHERE
    date_trunc('day', Рег_РухКоштів.period::timestamp) = '{Період}'
GROUP BY 
    period_day, Організація, Каса, Валюта
HAVING 
   SUM(CASE WHEN Рег_РухКоштів.income = true THEN 
        Рег_РухКоштів.{РухКоштів_Const.Сума} ELSE 
       -Рег_РухКоштів.{РухКоштів_Const.Сума} END) != 0
";

                                    Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                                    break;
                                }
                        }
                    }
                }

                string queryUpdate = $@"
UPDATE {Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.TABLE}
    SET {Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.Виконано} = true
WHERE uid = '{uid}'
";
                Config.KernelBackgroundTask.DataBase.ExecuteSQL(queryUpdate);
            }

            //Обновлення актуальності віртуальних залишків по місяцях
            foreach (KeyValuePair<string, List<string>> documentProcessed in documentProcessedList)
            {
                List<string> allAllowRegisterAccumulation = new List<string>();

                foreach (string documentType in documentProcessed.Value)
                {
                    //Список регістрів доступних для документу
                    List<string> allowRegisterAccumulation = Config.Kernel.Conf.Documents[documentType].AllowRegisterAccumulation;

                    foreach (string registerAccumulation in allowRegisterAccumulation)
                    {
                        if (!allAllowRegisterAccumulation.Contains(registerAccumulation))
                            allAllowRegisterAccumulation.Add(registerAccumulation);
                    }
                }

                ОбновитиЗначенняАктуальностіВіртуальнихЗалишківПоМісяцях(documentProcessed.Key, allAllowRegisterAccumulation);
            }

            Config.KernelBackgroundTask.DataBase.CommitTransaction();

            //Очистка
            string queryClear = $@"
DELETE FROM {Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.TABLE}
WHERE 
    {Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.Виконано} = true AND
    {Системні.ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart.Дата} < current_date
";
            Config.KernelBackgroundTask.DataBase.ExecuteSQL(queryClear);
        }

        /// <summary>
        /// Функція обновляє значення актуальності для ВіртальнихТаблицьРегістрів по місяцях.
        /// </summary>
        /// <param name="period">Дата</param>
        /// <param name="allowRegisterAccumulation">Список регістрів</param>
        public static void ОбновитиЗначенняАктуальностіВіртуальнихЗалишківПоМісяцях(string period, List<string> allowRegisterAccumulation)
        {
            string queryPartRegisterAccumulation = "'" + string.Join("','", allowRegisterAccumulation) + "'";

            string queryDelete = $@"
DELETE FROM {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.TABLE}
WHERE date_trunc('month', '{period}'::timestamp) = {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.Місяць} AND
    {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.Регістр} IN({queryPartRegisterAccumulation})";

            Config.KernelBackgroundTask.DataBase.ExecuteSQL(queryDelete);

            foreach (string registerAccumulation in allowRegisterAccumulation)
            {
                string queryInsert = $@"
INSERT INTO {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.TABLE}
(
    uid,
    {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.Регістр},
    {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.Місяць},
    {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.Актуально}
)
VALUES
(
    uuid_generate_v4(),
    '{registerAccumulation}',
    date_trunc('month', '{period}'::timestamp),
    false
)";

                Config.KernelBackgroundTask.DataBase.ExecuteSQL(queryInsert);
            }
        }

        /// <summary>
        /// Функція обчислює залишки по місяцях
        /// </summary>
        public static void ОбчисленняВіртуальнихЗалишківПоМісяцях()
        {
            string querySelect = $@"
SELECT
    Актуальність.uid,
    Актуальність.{Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.Регістр} AS Регістр,
    to_char(Актуальність.{Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.Місяць}, 'YYYY-MM-DD') AS Місяць
FROM
    {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.TABLE} AS Актуальність
WHERE
    Актуальність.{Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.Актуально} = false
ORDER BY Місяць ASC
";
            string[] columnsName;
            List<Dictionary<string, object>> listRow;

            Config.KernelBackgroundTask.DataBase.SelectRequest(querySelect, null, out columnsName, out listRow);

            //Обробка
            foreach (Dictionary<string, object> row in listRow)
            {
                string uid = row["uid"].ToString();
                string Регістр = row["Регістр"].ToString();
                string Місяць = row["Місяць"].ToString();

                switch (Регістр)
                {
                    case "ЗамовленняКлієнтів":
                        {
                            string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.TABLE}
WHERE date_trunc('month', {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Період}::timestamp) = '{Місяць}';

INSERT INTO {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.ХарактеристикаНоменклатури},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Замовлено},
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_Місяць_TablePart.Сума}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('month', ЗамовленняКлієнтів_День.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Період}::timestamp) as Період,
    ЗамовленняКлієнтів_День.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Номенклатура} AS Номенклатура, 
    ЗамовленняКлієнтів_День.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    ЗамовленняКлієнтів_День.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Склад} AS Склад,
    SUM(ЗамовленняКлієнтів_День.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Замовлено}) AS Замовлено,
    SUM(ЗамовленняКлієнтів_День.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Сума}) AS Сума
FROM 
    {ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.TABLE} AS ЗамовленняКлієнтів_День
WHERE
    date_trunc('month', ЗамовленняКлієнтів_День.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Період}::timestamp) = '{Місяць}'
GROUP BY 
    Період, Номенклатура, ХарактеристикаНоменклатури, Склад
HAVING 
    SUM(ЗамовленняКлієнтів_День.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Замовлено}) != 0 OR
    SUM(ЗамовленняКлієнтів_День.{ВіртуальніТаблиціРегістрів.ЗамовленняКлієнтів_День_TablePart.Сума}) != 0
";

                            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                            break;
                        }
                    case "ТовариНаСкладах":
                        {
                            string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.TABLE}
WHERE date_trunc('month', {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Період}::timestamp) = '{Місяць}';

INSERT INTO {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ХарактеристикаНоменклатури},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Серія},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ВНаявності},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ДоВідвантаження}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('month', ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Період}::timestamp) as Період,
    ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Номенклатура} AS Номенклатура, 
    ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Склад} AS Склад,
    ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Серія} AS Серія,
    SUM(ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ВНаявності}) AS ВНаявності,
    SUM(ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ДоВідвантаження}) AS ДоВідвантаження
FROM 
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.TABLE} AS ТовариНаСкладах_День
WHERE
    date_trunc('month', ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.Період}::timestamp) = '{Місяць}'
GROUP BY 
    Період, Номенклатура, ХарактеристикаНоменклатури, Склад, Серія
HAVING 
   SUM(ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ВНаявності}) != 0 OR
   SUM(ТовариНаСкладах_День.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_День_TablePart.ДоВідвантаження}) != 0
";

                            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                            //Додаткове обчислення підсумкових залишків
                            query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Підсумок_TablePart.TABLE};

INSERT INTO {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Підсумок_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Підсумок_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Підсумок_TablePart.ХарактеристикаНоменклатури},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Підсумок_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Підсумок_TablePart.Серія},
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Підсумок_TablePart.ВНаявності}
)
SELECT 
    uuid_generate_v4(),
    ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Номенклатура} AS Номенклатура, 
    ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Склад} AS Склад,
    ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.Серія} AS Серія,
    SUM(ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ВНаявності}) AS ВНаявності
FROM 
    {ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.TABLE} AS ТовариНаСкладах_Місяць
GROUP BY 
    Номенклатура, ХарактеристикаНоменклатури, Склад, Серія
HAVING 
   SUM(ТовариНаСкладах_Місяць.{ВіртуальніТаблиціРегістрів.ТовариНаСкладах_Місяць_TablePart.ВНаявності}) != 0
";

                            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                            break;
                        }
                    case "ПартіїТоварів":
                        {
                            string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.TABLE}
WHERE date_trunc('month', {ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.Період}::timestamp) = '{Місяць}';

INSERT INTO {ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.Організація},
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.ПартіяТоварівКомпозит},
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.ХарактеристикаНоменклатури},
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.Серія},
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.Кількість},
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_Місяць_TablePart.Собівартість}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('month', ПартіїТоварів_День.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Період}::timestamp) as Період,
    ПартіїТоварів_День.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Організація} AS Організація,
    ПартіїТоварів_День.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.ПартіяТоварівКомпозит} AS ПартіяТоварівКомпозит,
    ПартіїТоварів_День.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Номенклатура} AS Номенклатура, 
    ПартіїТоварів_День.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    ПартіїТоварів_День.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Серія} AS Серія,
    ПартіїТоварів_День.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Склад} AS Склад,
    SUM(ПартіїТоварів_День.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Кількість}) AS Кількість,
    SUM(ПартіїТоварів_День.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Собівартість}) AS Собівартість
FROM 
    {ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.TABLE} AS ПартіїТоварів_День
WHERE
    date_trunc('month', ПартіїТоварів_День.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Період}::timestamp) = '{Місяць}'
GROUP BY 
    Період, Організація, ПартіяТоварівКомпозит, Номенклатура, ХарактеристикаНоменклатури, Серія, Склад
HAVING 
   SUM(ПартіїТоварів_День.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Кількість}) != 0 OR
   SUM(ПартіїТоварів_День.{ВіртуальніТаблиціРегістрів.ПартіїТоварів_День_TablePart.Собівартість}) != 0
";

                            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                            break;
                        }
                    case "РозрахункиЗКлієнтами":
                        {
                            string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.TABLE}
WHERE date_trunc('month', {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.Період}::timestamp) = '{Місяць}';

INSERT INTO {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.Валюта},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.Контрагент},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_Місяць_TablePart.Сума}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('month', РозрахункиЗКлієнтами_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Період}::timestamp) as Період,
    РозрахункиЗКлієнтами_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Валюта} AS Валюта, 
    РозрахункиЗКлієнтами_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Контрагент} AS Контрагент,
    SUM(РозрахункиЗКлієнтами_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Сума}) AS Сума
FROM 
    {ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.TABLE} AS РозрахункиЗКлієнтами_День
WHERE
    date_trunc('month', РозрахункиЗКлієнтами_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Період}::timestamp) = '{Місяць}'
GROUP BY 
    Період, Валюта, Контрагент
HAVING
   SUM(РозрахункиЗКлієнтами_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗКлієнтами_День_TablePart.Сума}) != 0
";

                            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                            break;
                        }
                    case "РозрахункиЗПостачальниками":
                        {
                            string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.TABLE}
WHERE date_trunc('month', {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Період}::timestamp) = '{Місяць}';

INSERT INTO {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Валюта},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Контрагент},
    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_Місяць_TablePart.Сума}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('month', РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Період}::timestamp) as Період,
    РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Валюта} AS Валюта, 
    РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Контрагент} AS Контрагент,
    SUM(РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Сума}) AS Сума
FROM 
    {ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.TABLE} AS РозрахункиЗПостачальниками_День
WHERE
    date_trunc('month', РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Період}::timestamp) = '{Місяць}'
GROUP BY
    Період, Валюта, Контрагент
HAVING
    SUM(РозрахункиЗПостачальниками_День.{ВіртуальніТаблиціРегістрів.РозрахункиЗПостачальниками_День_TablePart.Сума}) != 0
";

                            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                            break;
                        }
                    case "ЗамовленняПостачальникам":
                        {
                            string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.TABLE}
WHERE date_trunc('month', {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.Період}::timestamp) = '{Місяць}';

INSERT INTO {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.ХарактеристикаНоменклатури},
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_Місяць_TablePart.Замовлено}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('month', ЗамовленняПостачальникам_День.{ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Період}::timestamp) as Період,
    ЗамовленняПостачальникам_День.{ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Номенклатура} AS Номенклатура, 
    ЗамовленняПостачальникам_День.{ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    ЗамовленняПостачальникам_День.{ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Склад} AS Склад,
    SUM(ЗамовленняПостачальникам_День.{ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Замовлено}) AS Замовлено
FROM 
    {ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.TABLE} AS ЗамовленняПостачальникам_День
WHERE
    date_trunc('month', ЗамовленняПостачальникам_День.{ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Період}::timestamp) = '{Місяць}'
GROUP BY 
    Період, Номенклатура, ХарактеристикаНоменклатури, Склад
HAVING 
    SUM(ЗамовленняПостачальникам_День.{ВіртуальніТаблиціРегістрів.ЗамовленняПостачальникам_День_TablePart.Замовлено}) != 0
";

                            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                            break;
                        }
                    case "ВільніЗалишки":
                        {
                            string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.TABLE}
WHERE date_trunc('month', {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.Період}::timestamp) = '{Місяць}';

INSERT INTO {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.Номенклатура},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.ХарактеристикаНоменклатури},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.Склад},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.ВНаявності},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.ВРезервіЗіСкладу},
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_Місяць_TablePart.ВРезервіПідЗамовлення}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('month', ВільніЗалишки_День.{ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.Період}::timestamp) as Період,
    ВільніЗалишки_День.{ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.Номенклатура} AS Номенклатура, 
    ВільніЗалишки_День.{ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
    ВільніЗалишки_День.{ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.Склад} AS Склад,
    SUM(ВільніЗалишки_День.{ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ВНаявності}) AS ВНаявності,
    SUM(ВільніЗалишки_День.{ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ВРезервіЗіСкладу}) AS ВРезервіЗіСкладу,
    SUM(ВільніЗалишки_День.{ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ВРезервіПідЗамовлення}) AS ВРезервіПідЗамовлення
FROM 
    {ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.TABLE} AS ВільніЗалишки_День
WHERE
    date_trunc('month', ВільніЗалишки_День.{ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.Період}::timestamp) = '{Місяць}'
GROUP BY 
    Період, Номенклатура, ХарактеристикаНоменклатури, Склад
HAVING 
   SUM(ВільніЗалишки_День.{ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ВНаявності}) != 0 OR
   SUM(ВільніЗалишки_День.{ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ВРезервіЗіСкладу}) != 0 OR
   SUM(ВільніЗалишки_День.{ВіртуальніТаблиціРегістрів.ВільніЗалишки_День_TablePart.ВРезервіПідЗамовлення}) != 0
";

                            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                            break;
                        }
                    case "РухКоштів":
                        {
                            string query = $@"
DELETE FROM {ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.TABLE}
WHERE date_trunc('month', {ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Період}::timestamp) = '{Місяць}';

INSERT INTO {ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.TABLE}
(
    uid,
    {ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Період},
    {ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Організація},
    {ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Каса},
    {ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Валюта},
    {ВіртуальніТаблиціРегістрів.РухКоштів_Місяць_TablePart.Сума}
)
SELECT 
    uuid_generate_v4(),
    date_trunc('month', РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Період}::timestamp) as Період,
    РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Організація} AS Організація, 
    РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Каса} AS Каса,
    РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Валюта} AS Валюта,
    SUM(РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Сума}) AS Сума
FROM 
    {ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.TABLE} AS РухКоштів_День
WHERE
    date_trunc('month', РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Період}::timestamp) = '{Місяць}'
GROUP BY 
    Період, Організація, Каса, Валюта
HAVING 
   SUM(РухКоштів_День.{ВіртуальніТаблиціРегістрів.РухКоштів_День_TablePart.Сума}) != 0
";

                            Config.KernelBackgroundTask.DataBase.ExecuteSQL(query);

                            break;
                        }
                }

                string queryUpdate = $@"
UPDATE {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.TABLE}
    SET {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.Актуально} = true,
        {Системні.ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart.ДатаОстанньогоОбчислення} = current_timestamp
WHERE uid = '{uid}'
";
                Config.KernelBackgroundTask.DataBase.ExecuteSQL(queryUpdate);
            }
        }

    }

    class BlockedConfigurationObject
    {

        public static void AddBlockedDocument(Guid documentUid, string userName)
        {
            Системні.ЗаблокованіОбєкти_Документи_TablePart заблокованіОбєкти_Документи_TablePart =
                new Системні.ЗаблокованіОбєкти_Документи_TablePart();

            Системні.ЗаблокованіОбєкти_Документи_TablePart.Record record =
                new Системні.ЗаблокованіОбєкти_Документи_TablePart.Record();

            record.Обєкт = documentUid;
            record.Користувач = userName;
            record.ДатаБлокування = DateTime.Now;

            заблокованіОбєкти_Документи_TablePart.Records.Add(record);
            заблокованіОбєкти_Документи_TablePart.Save(false);
        }

        public static void UpadateBlockedDocument(Guid documentUid, string userName)
        {
            Системні.ЗаблокованіОбєкти_Документи_TablePart заблокованіОбєкти_Документи_TablePart =
                new Системні.ЗаблокованіОбєкти_Документи_TablePart();

            
        }
    }
}

