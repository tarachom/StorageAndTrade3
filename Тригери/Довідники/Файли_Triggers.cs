
/*
    Файли_Triggers.cs
    Тригери для довідника Файли
*/

using StorageAndTrade_1_0.Константи;

namespace StorageAndTrade_1_0.Довідники
{
    class Файли_Triggers
    {
        public static async ValueTask New(Файли_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Файли_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Файли_Objest ДовідникОбєкт, Файли_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Файли_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Файли_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Файли_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(Файли_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
