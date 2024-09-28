
/*
    СкладськіПриміщення_Triggers.cs
    Тригери для довідника СкладськіПриміщення
*/

using StorageAndTrade_1_0.Константи;

namespace StorageAndTrade_1_0.Довідники
{
    class СкладськіПриміщення_Triggers
    {
        public static async ValueTask New(СкладськіПриміщення_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(СкладськіПриміщення_Objest ДовідникОбєкт, СкладськіПриміщення_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(СкладськіПриміщення_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(СкладськіПриміщення_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(СкладськіПриміщення_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(СкладськіПриміщення_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
