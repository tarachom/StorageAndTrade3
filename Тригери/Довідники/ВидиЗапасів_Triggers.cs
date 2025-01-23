
/*
    ВидиЗапасів_Triggers.cs
    Тригери для довідника ВидиЗапасів
*/

using GeneratedCode.Константи;

namespace GeneratedCode.Довідники
{
    class ВидиЗапасів_Triggers
    {
        public static async ValueTask New(ВидиЗапасів_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ВидиЗапасів_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ВидиЗапасів_Objest ДовідникОбєкт, ВидиЗапасів_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ВидиЗапасів_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ВидиЗапасів_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ВидиЗапасів_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ВидиЗапасів_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
