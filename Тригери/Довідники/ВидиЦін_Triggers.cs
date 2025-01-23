
/*
    ВидиЦін_Triggers.cs
    Тригери для довідника ВидиЦін
*/

using GeneratedCode.Константи;

namespace GeneratedCode.Довідники
{
    class ВидиЦін_Triggers
    {
        public static async ValueTask New(ВидиЦін_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ВидиЦін_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ВидиЦін_Objest ДовідникОбєкт, ВидиЦін_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ВидиЦін_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ВидиЦін_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ВидиЦін_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ВидиЦін_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
