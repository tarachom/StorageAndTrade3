
/*
    ДоговориКонтрагентів_Triggers.cs
    Тригери для довідника ДоговориКонтрагентів
*/

using GeneratedCode.Константи;
using GeneratedCode.Перелічення;

namespace GeneratedCode.Довідники
{
    class ДоговориКонтрагентів_Triggers
    {
        public static async ValueTask New(ДоговориКонтрагентів_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ДоговориКонтрагентів_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ДоговориКонтрагентів_Objest ДовідникОбєкт, ДоговориКонтрагентів_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ДоговориКонтрагентів_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.ТипДоговоруПредставлення = ПсевдонімиПерелічення.ТипДоговорів_Alias(ДовідникОбєкт.ТипДоговору);
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ДоговориКонтрагентів_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ДоговориКонтрагентів_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ДоговориКонтрагентів_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
