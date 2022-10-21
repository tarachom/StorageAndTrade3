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
 

*/

using System;
using System.Collections.Generic;

using AccountingSoftware;

using Конфа = StorageAndTrade_1_0;
using Константи = StorageAndTrade_1_0.Константи;
using System.Security.Cryptography.X509Certificates;

namespace StorageAndTrade
{
    class ФункціїДляФоновихЗавдань
    {
        public static void ДодатиЗаписВІсторіюЗавантаженняКурсуВалют(string Стан, string Посилання, string Повідомлення = "")
        {
            Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Історія_TablePart завантаженняКурсівВалют_Історія_TablePart =
                new Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Історія_TablePart();

            Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Історія_TablePart.Record record =
                new Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Історія_TablePart.Record();

            record.Дата = DateTime.Now;
            record.Стан = Стан;
            record.Посилання = Посилання;
            record.Повідомлення = Повідомлення;

            завантаженняКурсівВалют_Історія_TablePart.Records.Add(record);
            завантаженняКурсівВалют_Історія_TablePart.Save(false);
        }

        public static void ОчиститиІсторіюЗавантаженняКурсуВалют()
        {
            string query = @$"
DELETE FROM {Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Історія_TablePart.TABLE}
WHERE {Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Історія_TablePart.Дата} < @Дата
";
            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("Дата", DateTime.Now.AddDays(-7));

            Конфа.Config.Kernel!.DataBase.ExecuteSQL(query, paramQuery);
        }

        public static List<Dictionary<string, object>> ОтриматиЗаписиЗІсторіїЗавантаженняКурсуВалют(int КількістьЗаписів = 50)
        {
            //Очищення
            ОчиститиІсторіюЗавантаженняКурсуВалют();

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

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();

            string[] columnsName;
            List<Dictionary<string, object>> listRow;

            Конфа.Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

            return listRow;
        }

        public static DateTime? ОтриматиДатуОстанньогоЗавантаженняКурсуВалют()
        {
            string query = @$"
SELECT
    {Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Історія_TablePart.Дата} AS Дата
FROM
    {Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Історія_TablePart.TABLE}
ORDER BY Дата DESC
LIMIT 1
";
            Dictionary<string, object> paramQuery = new Dictionary<string, object>();

            string[] columnsName;
            List<Dictionary<string, object>> listRow;

            Конфа.Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

            if (listRow.Count == 1)
            {
                Dictionary<string, object> Рядок = listRow[0];
                return DateTime.Parse(Рядок["Дата"]?.ToString() ?? DateTime.MinValue.ToString());
            }
            else
                return null;
        }
    }
}