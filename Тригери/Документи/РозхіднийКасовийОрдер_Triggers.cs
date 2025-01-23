
/*
    РозхіднийКасовийОрдер_Triggers.cs
    Тригери для документу РозхіднийКасовийОрдер
*/

using GeneratedCode.Константи;
using StorageAndTrade;

namespace GeneratedCode.Документи
{
    class РозхіднийКасовийОрдер_Triggers
    {
        public static async ValueTask New(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.РозхіднийКасовийОрдер_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(РозхіднийКасовийОрдер_Objest ДокументОбєкт, РозхіднийКасовийОрдер_Objest Основа)
        {
            ДокументОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{РозхіднийКасовийОрдер_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(РозхіднийКасовийОрдер_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
