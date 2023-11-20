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

/*
 

*/

using Конфа = StorageAndTrade_1_0;
using Константи = StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.РегістриВідомостей;
using AccountingSoftware;

namespace StorageAndTrade
{
    class ФункціїДляФоновихЗавдань
    {
        public static async ValueTask ДодатиЗаписВІсторіюЗавантаженняКурсуВалют(string Стан, string Посилання, string Повідомлення = "")
        {
            Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Історія_TablePart завантаженняКурсівВалют_Історія_TablePart =
                new Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Історія_TablePart();

            Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Історія_TablePart.Record record =
                new Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Історія_TablePart.Record()
                {
                    Дата = DateTime.Now,
                    Стан = Стан,
                    Посилання = Посилання,
                    Повідомлення = Повідомлення
                };

            завантаженняКурсівВалют_Історія_TablePart.Records.Add(record);
            await завантаженняКурсівВалют_Історія_TablePart.Save(false);
        }

        public static async ValueTask ОчиститиІсторіюЗавантаженняКурсуВалют(bool clear_all = false)
        {
            string query = @$"
DELETE FROM {Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Історія_TablePart.TABLE}
";

            if (!clear_all)
            {
                query += @$"
WHERE {Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Історія_TablePart.Дата} < @Дата
";
            }

            Dictionary<string, object> paramQuery = new Dictionary<string, object>()
            {
                { "Дата", DateTime.Now.AddDays(-7) }
            };

            await Конфа.Config.Kernel!.DataBase.ExecuteSQL(query, paramQuery);
        }

        public static async ValueTask<SelectRequestAsync_Record> ОтриматиЗаписиЗІсторіїЗавантаженняКурсуВалют(int КількістьЗаписів = 50)
        {
            //Очищення
            await ОчиститиІсторіюЗавантаженняКурсуВалют();

            //Вибірка
            string query = @$"
SELECT
    {Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Історія_TablePart.Дата} AS Дата,
    {Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Історія_TablePart.Стан} AS Стан,
    {Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Історія_TablePart.Посилання} AS Посилання,
    {Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Історія_TablePart.Повідомлення} AS Повідомлення
FROM
    {Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Історія_TablePart.TABLE}
ORDER BY Дата DESC
LIMIT {КількістьЗаписів}
";
            return await Конфа.Config.Kernel!.DataBase.SelectRequestAsync(query);
        }

        public static async ValueTask<DateTime?> ОтриматиДатуОстанньогоЗавантаженняКурсуВалют()
        {
            string query = @$"
SELECT
    {Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Історія_TablePart.Дата} AS Дата
FROM
    {Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Історія_TablePart.TABLE}
ORDER BY Дата DESC
LIMIT 1
";

            var recordResult = await Конфа.Config.Kernel!.DataBase.SelectRequestAsync(query);

            if (recordResult.Result)
            {
                Dictionary<string, object> Рядок = recordResult.ListRow[0];
                return DateTime.Parse(Рядок["Дата"]?.ToString() ?? DateTime.MinValue.ToString());
            }
            else
                return null;
        }

        public static async ValueTask<SelectRequestAsync_Record> ОтриматиКурсиВалютДляСтартовоїСторінки()
        {
            string query = @$"
WITH Валюти AS
(
    SELECT 
        Валюти.uid AS Валюта,
        Валюти.{Валюти_Const.Назва} AS ВалютаНазва
    FROM
        {Валюти_Const.TABLE} AS Валюти
    WHERE
        Валюти.{Валюти_Const.ВиводитиКурсНаСтартову} = true
    ORDER BY Валюти.{Валюти_Const.Код}
)
SELECT
    Валюти.ВалютаНазва,
    (
        SELECT 
            КурсиВалют.{КурсиВалют_Const.Курс}
        FROM 
            {КурсиВалют_Const.TABLE} AS КурсиВалют
        WHERE
            КурсиВалют.{КурсиВалют_Const.Валюта} = Валюти.Валюта
        ORDER BY КурсиВалют.period DESC
        LIMIT 1
    ) AS Курс
FROM
    Валюти";

            return await Конфа.Config.Kernel!.DataBase.SelectRequestAsync(query);
        }
    }
}