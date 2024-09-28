
/*
    БанківськіРахункиКонтрагентів_Triggers.cs
    Тригери для довідника БанківськіРахункиКонтрагентів
*/

using StorageAndTrade_1_0.Константи;

namespace StorageAndTrade_1_0.Довідники
{
    class БанківськіРахункиКонтрагентів_Triggers
    {
        public static async ValueTask New(БанківськіРахункиКонтрагентів_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.БанківськіРахункиКонтрагентів_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(БанківськіРахункиКонтрагентів_Objest ДовідникОбєкт, БанківськіРахункиКонтрагентів_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(БанківськіРахункиКонтрагентів_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(БанківськіРахункиКонтрагентів_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(БанківськіРахункиКонтрагентів_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(БанківськіРахункиКонтрагентів_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
