
/*
    Організації_Triggers.cs
    Тригери для довідника Організації
*/

using StorageAndTrade_1_0.Константи;

namespace StorageAndTrade_1_0.Довідники
{
    class Організації_Triggers
    {
        public static async ValueTask New(Організації_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Організації_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Організації_Objest ДовідникОбєкт, Організації_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Організації_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Організації_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Організації_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(Організації_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}