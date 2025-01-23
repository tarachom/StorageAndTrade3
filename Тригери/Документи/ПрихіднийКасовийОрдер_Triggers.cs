
/*
    ПрихіднийКасовийОрдер_Triggers.cs
    Тригери для документу ПрихіднийКасовийОрдер
*/

using GeneratedCode.Константи;
using StorageAndTrade;

namespace GeneratedCode.Документи
{
    class ПрихіднийКасовийОрдер_Triggers
    {
        public static async ValueTask New(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПрихіднийКасовийОрдер_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПрихіднийКасовийОрдер_Objest ДокументОбєкт, ПрихіднийКасовийОрдер_Objest Основа)
        {
            ДокументОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ПрихіднийКасовийОрдер_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПрихіднийКасовийОрдер_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
