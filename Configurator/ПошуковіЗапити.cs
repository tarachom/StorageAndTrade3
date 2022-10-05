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

ПошуковіЗапити - це статичні пошукові запити для DirectoryControl
Використовується для довідників, документів, звітів, обробок там де використовується DirectoryControl

*/

using Довідники = StorageAndTrade_1_0.Довідники;
using Документи = StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ПошуковіЗапити
    {
        //
        // ДОВІДНИКИ
        //

        public static readonly string Файли = $@"
SELECT 
    uid,
    {Довідники.Файли_Const.Назва} AS Назва
FROM
    {Довідники.Файли_Const.TABLE}
WHERE
    LOWER({Довідники.Файли_Const.Назва}) LIKE @like_param
ORDER BY Назва
LIMIT 10
";

        public static readonly string ФізичніОсоби = $@"
SELECT 
    uid,
    {Довідники.ФізичніОсоби_Const.Назва} AS Назва
FROM
    {Довідники.ФізичніОсоби_Const.TABLE}
WHERE
    LOWER({Довідники.ФізичніОсоби_Const.Назва}) LIKE @like_param
ORDER BY Назва
LIMIT 10
";

        public static readonly string ВидиНоменклатури = $@"
SELECT 
    uid,
    {Довідники.ВидиНоменклатури_Const.Назва} AS Назва
FROM
    {Довідники.ВидиНоменклатури_Const.TABLE}
WHERE
    LOWER({Довідники.ВидиНоменклатури_Const.Назва}) LIKE @like_param
ORDER BY Назва
LIMIT 10
";

        public static readonly string Виробники = $@"
SELECT 
    uid,
    {Довідники.Виробники_Const.Назва} AS Назва
FROM
    {Довідники.Виробники_Const.TABLE}
WHERE
    LOWER({Довідники.Виробники_Const.Назва}) LIKE @like_param
ORDER BY Назва
LIMIT 10
";

        public static readonly string Контрагенти = $@"
SELECT 
    uid,
    {Довідники.Контрагенти_Const.Назва} AS Назва
FROM
    {Довідники.Контрагенти_Const.TABLE}
WHERE
    LOWER({Довідники.Контрагенти_Const.Назва}) LIKE @like_param
ORDER BY Назва
LIMIT 10
";

        public static readonly string Контрагенти_Папки = $@"
SELECT 
    uid,
    {Довідники.Контрагенти_Папки_Const.Назва} AS Назва
FROM
    {Довідники.Контрагенти_Папки_Const.TABLE}
WHERE
    LOWER({Довідники.Контрагенти_Папки_Const.Назва}) LIKE @like_param
ORDER BY Назва
LIMIT 10
";

        public static readonly string Організації = $@"
SELECT 
    uid,
    {Довідники.Організації_Const.Назва} AS Назва
FROM
    {Довідники.Організації_Const.TABLE}
WHERE
    LOWER({Довідники.Організації_Const.Назва}) LIKE @like_param
ORDER BY Назва
LIMIT 10
";

        public static readonly string Валюти = $@"
SELECT 
    uid,
    {Довідники.Валюти_Const.Назва} AS Назва
FROM
    {Довідники.Валюти_Const.TABLE}
WHERE
    LOWER({Довідники.Валюти_Const.Назва}) LIKE @like_param
ORDER BY Назва
LIMIT 10
";

        public static readonly string Каси = $@"
SELECT 
    uid,
    {Довідники.Каси_Const.Назва} AS Назва
FROM
    {Довідники.Каси_Const.TABLE}
WHERE
    LOWER({Довідники.Каси_Const.Назва}) LIKE @like_param
ORDER BY Назва
LIMIT 10
";

        public static readonly string БанківськіРахункиОрганізацій = $@"
SELECT 
    uid,
    {Довідники.БанківськіРахункиОрганізацій_Const.Назва} AS Назва
FROM
    {Довідники.БанківськіРахункиОрганізацій_Const.TABLE}
WHERE
    LOWER({Довідники.БанківськіРахункиОрганізацій_Const.Назва}) LIKE @like_param
ORDER BY Назва
LIMIT 10
";

        public static readonly string Номенклатура_Папки = $@"
SELECT 
    uid,
    {Довідники.Номенклатура_Папки_Const.Назва} AS Назва
FROM
    {Довідники.Номенклатура_Папки_Const.TABLE}
WHERE
    LOWER({Довідники.Номенклатура_Папки_Const.Назва}) LIKE @like_param
ORDER BY Назва
LIMIT 10
";

        public static readonly string Номенклатура = $@"
SELECT 
    uid,
    {Довідники.Номенклатура_Const.Назва} AS Назва
FROM
    {Довідники.Номенклатура_Const.TABLE}
WHERE
    LOWER({Довідники.Номенклатура_Const.Назва}) LIKE @like_param
ORDER BY Назва
LIMIT 10
";

        public static readonly string Склади_Папки = $@"
SELECT 
    uid,
    {Довідники.Склади_Папки_Const.Назва} AS Назва
FROM
    {Довідники.Склади_Папки_Const.TABLE}
WHERE
    LOWER({Довідники.Склади_Папки_Const.Назва}) LIKE @like_param
ORDER BY Назва
LIMIT 10
";

        public static readonly string Склади = $@"
SELECT 
    uid,
    {Довідники.Склади_Const.Назва} AS Назва
FROM
    {Довідники.Склади_Const.TABLE}
WHERE
    LOWER({Довідники.Склади_Const.Назва}) LIKE @like_param
ORDER BY Назва
LIMIT 10
";

        public static readonly string СеріїНоменклатури = $@"
SELECT 
    uid,
    {Довідники.СеріїНоменклатури_Const.Номер} AS Назва
FROM
    {Довідники.СеріїНоменклатури_Const.TABLE}
WHERE
    LOWER({Довідники.СеріїНоменклатури_Const.Номер}) LIKE @like_param
ORDER BY Назва
LIMIT 10
";

        public static readonly string ВидиЦін = $@"
SELECT 
    uid,
    {Довідники.ВидиЦін_Const.Назва} AS Назва
FROM
    {Довідники.ВидиЦін_Const.TABLE}
WHERE
    LOWER({Довідники.ВидиЦін_Const.Назва}) LIKE @like_param
ORDER BY Назва
LIMIT 10
";

        public static readonly string ПакуванняОдиниціВиміру = $@"
SELECT 
    uid,
    {Довідники.ПакуванняОдиниціВиміру_Const.Назва} AS Назва
FROM
    {Довідники.ПакуванняОдиниціВиміру_Const.TABLE}
WHERE
    LOWER({Довідники.ПакуванняОдиниціВиміру_Const.Назва}) LIKE @like_param
ORDER BY Назва
LIMIT 10
";

        public static string ХарактеристикаНоменклатуриЗВідбором(Довідники.Номенклатура_Pointer Номенклатура = null)
        {
            string query = $@"
SELECT 
    ХарактеристикиНоменклатури.uid,";

            if (Номенклатура != null && !Номенклатура.IsEmpty())
            {
                query += $@"
ХарактеристикиНоменклатури.{Довідники.ХарактеристикиНоменклатури_Const.Назва} AS Назва
";
            }
            else
            {
                query += $@"
CONCAT(Довідник_Номенклатура.{Довідники.Номенклатура_Const.Назва}, '; ', ХарактеристикиНоменклатури.{Довідники.ХарактеристикиНоменклатури_Const.Назва}) AS Назва
";
            }

            query += $@"
FROM
    {Довідники.ХарактеристикиНоменклатури_Const.TABLE} AS ХарактеристикиНоменклатури
         LEFT JOIN {Довідники.Номенклатура_Const.TABLE} AS Довідник_Номенклатура ON Довідник_Номенклатура.uid = 
            ХарактеристикиНоменклатури.{Довідники.ХарактеристикиНоменклатури_Const.Номенклатура}
WHERE
    LOWER(ХарактеристикиНоменклатури.{Довідники.ХарактеристикиНоменклатури_Const.Назва}) LIKE @like_param
";

            if (Номенклатура != null && !Номенклатура.IsEmpty())
            {
                query += $@"
AND ХарактеристикиНоменклатури.{Довідники.ХарактеристикиНоменклатури_Const.Номенклатура} = '{Номенклатура.UnigueID}'
";
            }

            query += $@"
ORDER BY ХарактеристикиНоменклатури.{Довідники.ХарактеристикиНоменклатури_Const.Назва}
LIMIT 10
";

            return query;
        }

        //
        // ДОКУМЕНТИ
        //

        public static readonly string РеалізаціяТоварівТаПослуг = $@"
SELECT 
    РеалізаціяТоварівТаПослуг.uid,
    РеалізаціяТоварівТаПослуг.{Документи.РеалізаціяТоварівТаПослуг_Const.Назва} AS Назва
FROM
    {Документи.РеалізаціяТоварівТаПослуг_Const.TABLE} AS РеалізаціяТоварівТаПослуг
WHERE
    LOWER(РеалізаціяТоварівТаПослуг.{Документи.РеалізаціяТоварівТаПослуг_Const.Назва}) LIKE @like_param
ORDER BY Назва
LIMIT 10
";

        public static readonly string ПоступленняТоварівТаПослуг = $@"
SELECT 
    ПоступленняТоварівТаПослуг.uid,
    ПоступленняТоварівТаПослуг.{Документи.ПоступленняТоварівТаПослуг_Const.Назва} AS Назва
FROM
    {Документи.ПоступленняТоварівТаПослуг_Const.TABLE} AS ПоступленняТоварівТаПослуг
WHERE
    LOWER(ПоступленняТоварівТаПослуг.{Документи.ПоступленняТоварівТаПослуг_Const.Назва}) LIKE @like_param
ORDER BY Назва
LIMIT 10
";

        public static readonly string ЗамовленняПостачальнику = $@"
SELECT 
    ЗамовленняПостачальнику.uid,
    ЗамовленняПостачальнику.{Документи.ЗамовленняПостачальнику_Const.Назва} AS Назва
FROM
    {Документи.ЗамовленняПостачальнику_Const.TABLE} AS ЗамовленняПостачальнику
WHERE
    LOWER(ЗамовленняПостачальнику.{Документи.ЗамовленняПостачальнику_Const.Назва}) LIKE @like_param
ORDER BY Назва
LIMIT 10
";

        public static readonly string ЗамовленняКлієнта = $@"
SELECT 
    ЗамовленняКлієнта.uid,
    ЗамовленняКлієнта.{Документи.ЗамовленняКлієнта_Const.Назва} AS Назва
FROM
    {Документи.ЗамовленняКлієнта_Const.TABLE} AS ЗамовленняКлієнта
WHERE
    LOWER(ЗамовленняКлієнта.{Документи.ЗамовленняКлієнта_Const.Назва}) LIKE @like_param
ORDER BY Назва
LIMIT 10
";

        public static readonly string РахунокФактура = $@"
SELECT 
    РахунокФактура.uid,
    РахунокФактура.{Документи.РахунокФактура_Const.Назва} AS Назва
FROM
    {Документи.РахунокФактура_Const.TABLE} AS РахунокФактура
WHERE
    LOWER(РахунокФактура.{Документи.РахунокФактура_Const.Назва}) LIKE @like_param
ORDER BY Назва
LIMIT 10
";
    }
}
