
/*
    СкладськіКомірки_Triggers.cs
    Тригери для довідника СкладськіКомірки
*/

namespace StorageAndTrade_1_0.Довідники
{
    class СкладськіКомірки_Triggers
    {
        public static async ValueTask New(СкладськіКомірки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(СкладськіКомірки_Objest ДовідникОбєкт, СкладськіКомірки_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(СкладськіКомірки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(СкладськіКомірки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(СкладськіКомірки_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(СкладськіКомірки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
