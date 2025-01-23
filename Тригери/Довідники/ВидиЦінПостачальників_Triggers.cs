
/*
    ВидиЦінПостачальників_Triggers.cs
    Тригери для довідника ВидиЦінПостачальників
*/

using GeneratedCode.Константи;

namespace GeneratedCode.Довідники
{
    class ВидиЦінПостачальників_Triggers
    {
        public static async ValueTask New(ВидиЦінПостачальників_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ВидиЦінПостачальників_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ВидиЦінПостачальників_Objest ДовідникОбєкт, ВидиЦінПостачальників_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ВидиЦінПостачальників_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ВидиЦінПостачальників_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ВидиЦінПостачальників_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ВидиЦінПостачальників_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
