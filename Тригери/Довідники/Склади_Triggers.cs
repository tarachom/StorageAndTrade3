
/*
    Склади_Triggers.cs
    Тригери для довідника Склади
*/

using StorageAndTrade_1_0.Константи;

namespace StorageAndTrade_1_0.Довідники
{
    class Склади_Triggers
    {
        public static async ValueTask New(Склади_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Склади_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Склади_Objest ДовідникОбєкт, Склади_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Склади_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Склади_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Склади_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(Склади_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
