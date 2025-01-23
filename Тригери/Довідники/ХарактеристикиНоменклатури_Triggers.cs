
/*
    ХарактеристикиНоменклатури_Triggers.cs
    Тригери для довідника ХарактеристикиНоменклатури
*/

using GeneratedCode.Константи;

namespace GeneratedCode.Довідники
{
    class ХарактеристикиНоменклатури_Triggers
    {
        public static async ValueTask New(ХарактеристикиНоменклатури_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ХарактеристикиНоменклатури_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ХарактеристикиНоменклатури_Objest ДовідникОбєкт, ХарактеристикиНоменклатури_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ХарактеристикиНоменклатури_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ХарактеристикиНоменклатури_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ХарактеристикиНоменклатури_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ХарактеристикиНоменклатури_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
