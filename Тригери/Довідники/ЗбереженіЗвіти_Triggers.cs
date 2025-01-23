
/*
    ЗбереженіЗвіти_Triggers.cs
    Тригери для довідника ЗбереженіЗвіти
*/

using GeneratedCode.Константи;
using StorageAndTrade;

namespace GeneratedCode.Довідники
{
    class ЗбереженіЗвіти_Triggers
    {
        public static async ValueTask New(ЗбереженіЗвіти_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ЗбереженіЗвіти_Const).ToString("D6");
            ДовідникОбєкт.Додано = DateTime.Now;
            ДовідникОбєкт.Користувач = Program.Користувач;
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ЗбереженіЗвіти_Objest ДовідникОбєкт, ЗбереженіЗвіти_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ЗбереженіЗвіти_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ЗбереженіЗвіти_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ЗбереженіЗвіти_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ЗбереженіЗвіти_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
