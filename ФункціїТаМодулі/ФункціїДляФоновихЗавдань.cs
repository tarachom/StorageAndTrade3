/*

Для фонових завдань 

*/

using Конфа = GeneratedCode;
using Константи = GeneratedCode.Константи;
using GeneratedCode.Довідники;
using GeneratedCode.РегістриВідомостей;
using AccountingSoftware;

namespace StorageAndTrade
{
    class ФункціїДляФоновихЗавдань
    {
        public static async ValueTask ДодатиЗаписВІсторіюЗавантаженняКурсуВалют(string Стан, string Посилання, string Повідомлення = "")
        {
            Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Історія_TablePart завантаженняКурсівВалют_Історія_TablePart = new();

            завантаженняКурсівВалют_Історія_TablePart.Records.Add(new()
            {
                Дата = DateTime.Now,
                Стан = Стан,
                Посилання = Посилання,
                Повідомлення = Повідомлення
            });

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

            await Конфа.Config.Kernel.DataBase.ExecuteSQL(query, paramQuery);
        }

        public static async ValueTask<SelectRequest_Record> ОтриматиЗаписиЗІсторіїЗавантаженняКурсуВалют(int КількістьЗаписів = 50)
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
            return await Конфа.Config.Kernel.DataBase.SelectRequest(query);
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

            var recordResult = await Конфа.Config.Kernel.DataBase.SelectRequest(query);
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

            return await Конфа.Config.Kernel.DataBase.SelectRequest(query);
        }
    }
}