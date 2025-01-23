
/*
    Користувачі_Triggers.cs
    Тригери для довідника Користувачі
*/

using GeneratedCode.Константи;

namespace GeneratedCode.Довідники
{
    class Користувачі_Triggers
    {
        public static async ValueTask New(Користувачі_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Користувачі_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Користувачі_Objest ДовідникОбєкт, Користувачі_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Користувачі_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Користувачі_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Користувачі_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(Користувачі_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
