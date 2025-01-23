
/*
    КраїниСвіту_Triggers.cs
    Тригери для довідника КраїниСвіту
*/

using GeneratedCode.Константи;

namespace GeneratedCode.Довідники
{
    class КраїниСвіту_Triggers
    {
        public static async ValueTask New(КраїниСвіту_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.КраїниСвіту_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(КраїниСвіту_Objest ДовідникОбєкт, КраїниСвіту_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(КраїниСвіту_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(КраїниСвіту_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(КраїниСвіту_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(КраїниСвіту_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
