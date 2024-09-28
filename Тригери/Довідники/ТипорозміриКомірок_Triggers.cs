
/*
    ТипорозміриКомірок_Triggers.cs
    Тригери для довідника ТипорозміриКомірок
*/

using StorageAndTrade_1_0.Константи;

namespace StorageAndTrade_1_0.Довідники
{
    class ТипорозміриКомірок_Triggers
    {
        public static async ValueTask New(ТипорозміриКомірок_Objest ДовідникОбєкт)
        {
            
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ТипорозміриКомірок_Objest ДовідникОбєкт, ТипорозміриКомірок_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ТипорозміриКомірок_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ТипорозміриКомірок_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ТипорозміриКомірок_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ТипорозміриКомірок_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
