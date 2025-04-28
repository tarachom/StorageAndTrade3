/*

Для фонових завдань 

*/

using GeneratedCode;
using GeneratedCode.Довідники;
using GeneratedCode.РегістриВідомостей;
using AccountingSoftware;

using Історія = GeneratedCode.Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Історія_TablePart;

namespace StorageAndTrade
{
    class ФункціїДляФоновихЗавдань
    {
        public static async ValueTask ДодатиЗаписВІсторіюЗавантаженняКурсуВалют(string Стан, string Посилання, string Повідомлення = "")
        {
            Історія Таблиця = new();

            Таблиця.Records.Add(new()
            {
                Дата = DateTime.Now,
                Стан = Стан,
                Посилання = Посилання,
                Повідомлення = Повідомлення
            });

            await Таблиця.Save(false);
        }

        public static async ValueTask ОчиститиІсторіюЗавантаженняКурсуВалют(bool clear_all = false)
        {
            string query = @$"
DELETE FROM {Історія.TABLE}
";

            if (!clear_all)
            {
                query += @$"
WHERE {Історія.Дата} < @Дата
";
            }

            Dictionary<string, object> paramQuery = new()
            {
                { "Дата", DateTime.Now.AddDays(-7) }
            };

            await Config.Kernel.DataBase.ExecuteSQL(query, paramQuery);
        }

        public static async ValueTask<SelectRequest_Record> ОтриматиЗаписиЗІсторіїЗавантаженняКурсуВалют(int КількістьЗаписів = 50)
        {
            //Очищення
            await ОчиститиІсторіюЗавантаженняКурсуВалют();

            //Вибірка
            string query = @$"
SELECT
    {Історія.Дата} AS Дата,
    {Історія.Стан} AS Стан,
    {Історія.Посилання} AS Посилання,
    {Історія.Повідомлення} AS Повідомлення
FROM
    {Історія.TABLE}
ORDER BY Дата DESC
LIMIT {КількістьЗаписів}
";
            return await Config.Kernel.DataBase.SelectRequest(query);
        }

        public static async ValueTask<DateTime?> ОтриматиДатуОстанньогоЗавантаженняКурсуВалют()
        {
            string query = @$"
SELECT
    {Історія.Дата} AS Дата
FROM
    {Історія.TABLE}
ORDER BY Дата DESC
LIMIT 1
";

            var recordResult = await Config.Kernel.DataBase.SelectRequest(query);
            if (recordResult.Result)
            {
                Dictionary<string, object> Рядок = recordResult.ListRow[0];
                return DateTime.Parse(Рядок["Дата"]?.ToString() ?? DateTime.MinValue.ToString());
            }
            else
                return null;
        }

        public static async ValueTask<SelectRequest_Record> ОтриматиКурсиВалютДляСтартовоїСторінки()
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

            return await Config.Kernel.DataBase.SelectRequest(query);
        }
    }
}