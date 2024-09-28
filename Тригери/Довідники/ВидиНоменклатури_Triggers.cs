
/*
    ВидиНоменклатури_Triggers.cs
    Тригери для довідника ВидиНоменклатури
*/

using StorageAndTrade_1_0.Константи;

namespace StorageAndTrade_1_0.Довідники
{
    class ВидиНоменклатури_Triggers
    {
        public static async ValueTask New(ВидиНоменклатури_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ВидиНоменклатури_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ВидиНоменклатури_Objest ДовідникОбєкт, ВидиНоменклатури_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ВидиНоменклатури_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ВидиНоменклатури_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ВидиНоменклатури_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ВидиНоменклатури_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
