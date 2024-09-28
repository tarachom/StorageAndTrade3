
/*
    Валюти_Triggers.cs
    Тригери для довідника Валюти
*/

using StorageAndTrade_1_0.Константи;

namespace StorageAndTrade_1_0.Довідники
{
    class Валюти_Triggers
    {
        public static async ValueTask New(Валюти_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Валюти_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Валюти_Objest ДовідникОбєкт, Валюти_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Валюти_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Валюти_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Валюти_Objest ДовідникОбєкт, bool label)
        {
            if (label)
                await BeforeDelete(ДовідникОбєкт);
        }

        public static async ValueTask BeforeDelete(Валюти_Objest ДовідникОбєкт)
        {
            //Очистити регістр КурсиВалют при видаленні валюти
            string query = $@"
DELETE FROM 
	{РегістриВідомостей.КурсиВалют_Const.TABLE} AS КурсиВалют
WHERE
    КурсиВалют.{РегістриВідомостей.КурсиВалют_Const.Валюта} = @Валюта
";

            Dictionary<string, object> paramQuery = new()
            {
                { "Валюта", ДовідникОбєкт.UnigueID.UGuid }
            };

            await Config.Kernel.DataBase.ExecuteSQL(query, paramQuery);
        }
    }
}
