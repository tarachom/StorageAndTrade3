

/*
        КасиККМ_Triggers.cs
        Тригери
*/

using GeneratedCode.Константи;
using AccountingSoftware;

namespace GeneratedCode.Довідники
{
    static class КасиККМ_Triggers
    {
        public static async ValueTask New(КасиККМ_Objest ДовідникОбєкт)
        {
            
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(КасиККМ_Objest ДовідникОбєкт, КасиККМ_Objest Основа)
        {
            
            ДовідникОбєкт.Назва += " - Копія";
            
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(КасиККМ_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(КасиККМ_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(КасиККМ_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(КасиККМ_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
    