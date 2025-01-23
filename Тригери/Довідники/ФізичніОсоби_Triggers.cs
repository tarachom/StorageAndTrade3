
/*
    ФізичніОсоби_Triggers.cs
    Тригери для довідника ФізичніОсоби
*/

using GeneratedCode.Константи;

namespace GeneratedCode.Довідники
{
    class ФізичніОсоби_Triggers
    {
        public static async ValueTask New(ФізичніОсоби_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ФізичніОсоби_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ФізичніОсоби_Objest ДовідникОбєкт, ФізичніОсоби_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ФізичніОсоби_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ФізичніОсоби_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ФізичніОсоби_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ФізичніОсоби_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
