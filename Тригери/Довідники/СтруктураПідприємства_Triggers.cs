
/*
    СтруктураПідприємства_Triggers.cs
    Тригери для довідника СтруктураПідприємства
*/

using GeneratedCode.Константи;

namespace GeneratedCode.Довідники
{
    class СтруктураПідприємства_Triggers
    {
        public static async ValueTask New(СтруктураПідприємства_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.СтруктураПідприємства_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(СтруктураПідприємства_Objest ДовідникОбєкт, СтруктураПідприємства_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(СтруктураПідприємства_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(СтруктураПідприємства_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(СтруктураПідприємства_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(СтруктураПідприємства_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
