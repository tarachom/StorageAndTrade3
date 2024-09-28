
/*
    Банки_Triggers.cs
    Тригери для довідника Банки
*/

using StorageAndTrade_1_0.Константи;

namespace StorageAndTrade_1_0.Довідники
{
    class Банки_Triggers
    {
        public static async ValueTask New(Банки_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Банки_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Банки_Objest ДовідникОбєкт, Банки_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Банки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Банки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Банки_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(Банки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
