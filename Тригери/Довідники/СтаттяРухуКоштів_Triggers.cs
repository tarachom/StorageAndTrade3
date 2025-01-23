
/*
    СтаттяРухуКоштів_Triggers.cs
    Тригери для довідника СтаттяРухуКоштів
*/

using GeneratedCode.Константи;

namespace GeneratedCode.Довідники
{
    class СтаттяРухуКоштів_Triggers
    {
        public static async ValueTask New(СтаттяРухуКоштів_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.СтаттяРухуКоштів_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(СтаттяРухуКоштів_Objest ДовідникОбєкт, СтаттяРухуКоштів_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(СтаттяРухуКоштів_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(СтаттяРухуКоштів_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(СтаттяРухуКоштів_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(СтаттяРухуКоштів_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
