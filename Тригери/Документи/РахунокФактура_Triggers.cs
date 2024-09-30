
/*
    РахунокФактура_Triggers.cs
    Тригери для документу РахунокФактура
*/

using StorageAndTrade_1_0.Константи;
using StorageAndTrade;

namespace StorageAndTrade_1_0.Документи
{
    class РахунокФактура_Triggers
    {
        public static async ValueTask New(РахунокФактура_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.РахунокФактура_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(РахунокФактура_Objest ДокументОбєкт, РахунокФактура_Objest Основа)
        {
            ДокументОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(РахунокФактура_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{РахунокФактура_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(РахунокФактура_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(РахунокФактура_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(РахунокФактура_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
