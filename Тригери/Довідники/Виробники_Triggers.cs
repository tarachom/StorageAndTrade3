
/*
    Виробники_Triggers.cs
    Тригери для довідника Виробники
*/

using GeneratedCode.Константи;

namespace GeneratedCode.Довідники
{
    class Виробники_Triggers
    {
        public static async ValueTask New(Виробники_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Виробники_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Виробники_Objest ДовідникОбєкт, Виробники_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Виробники_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Виробники_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Виробники_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(Виробники_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
