
/*
    ПакуванняОдиниціВиміру_Triggers.cs
    Тригери для довідника ПакуванняОдиниціВиміру
*/

using StorageAndTrade_1_0.Константи;

namespace StorageAndTrade_1_0.Довідники
{
    class ПакуванняОдиниціВиміру_Triggers
    {
        public static async ValueTask New(ПакуванняОдиниціВиміру_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ПакуванняОдиниціВиміру_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПакуванняОдиниціВиміру_Objest ДовідникОбєкт, ПакуванняОдиниціВиміру_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПакуванняОдиниціВиміру_Objest ДовідникОбєкт)
        {
            if (ДовідникОбєкт.КількістьУпаковок <= 0) ДовідникОбєкт.КількістьУпаковок = 1;
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПакуванняОдиниціВиміру_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПакуванняОдиниціВиміру_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПакуванняОдиниціВиміру_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
