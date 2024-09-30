
/*
    ПартіяТоварівКомпозит_Triggers.cs
    Тригери для довідника ПартіяТоварівКомпозит
*/

namespace StorageAndTrade_1_0.Довідники
{
    class ПартіяТоварівКомпозит_Triggers
    {
        public static async ValueTask New(ПартіяТоварівКомпозит_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПартіяТоварівКомпозит_Objest ДовідникОбєкт, ПартіяТоварівКомпозит_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПартіяТоварівКомпозит_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПартіяТоварівКомпозит_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПартіяТоварівКомпозит_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПартіяТоварівКомпозит_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
