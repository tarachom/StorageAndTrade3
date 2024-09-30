
/*
    Блокнот_Triggers.cs
    Тригери для довідника Блокнот
*/

using StorageAndTrade_1_0.Константи;

namespace StorageAndTrade_1_0.Довідники
{
    class Блокнот_Triggers
    {
        public static async ValueTask New(Блокнот_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Блокнот_Const).ToString("D6");
            ДовідникОбєкт.ДатаЗапису = DateTime.Now;
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Блокнот_Objest ДовідникОбєкт, Блокнот_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Блокнот_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Блокнот_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Блокнот_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(Блокнот_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
