
/*
    БанківськіРахункиОрганізацій_Triggers.cs
    Тригери для довідника БанківськіРахункиОрганізацій
*/

using StorageAndTrade_1_0.Константи;

namespace StorageAndTrade_1_0.Довідники
{
    class БанківськіРахункиОрганізацій_Triggers
    {
        public static async ValueTask New(БанківськіРахункиОрганізацій_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.БанківськіРахункиОрганізацій_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(БанківськіРахункиОрганізацій_Objest ДовідникОбєкт, БанківськіРахункиОрганізацій_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(БанківськіРахункиОрганізацій_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(БанківськіРахункиОрганізацій_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(БанківськіРахункиОрганізацій_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(БанківськіРахункиОрганізацій_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
