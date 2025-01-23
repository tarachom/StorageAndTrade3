
/*
    Каси_Triggers.cs
    Тригери для довідника Каси
*/

using GeneratedCode.Константи;

namespace GeneratedCode.Довідники
{
    class Каси_Triggers
    {
        public static async ValueTask New(Каси_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Каси_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Каси_Objest ДовідникОбєкт, Каси_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Каси_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Каси_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Каси_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(Каси_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
