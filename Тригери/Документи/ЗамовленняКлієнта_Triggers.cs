
/*
    ЗамовленняКлієнта_Triggers.cs
    Тригери для документу ЗамовленняКлієнта
*/

using StorageAndTrade_1_0.Константи;
using StorageAndTrade;

namespace StorageAndTrade_1_0.Документи
{
    class ЗамовленняКлієнта_Triggers
    {
        public static async ValueTask New(ЗамовленняКлієнта_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ЗамовленняКлієнта_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ЗамовленняКлієнта_Objest ДокументОбєкт, ЗамовленняКлієнта_Objest Основа)
        {
            ДокументОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ЗамовленняКлієнта_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ЗамовленняКлієнта_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ЗамовленняКлієнта_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ЗамовленняКлієнта_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ЗамовленняКлієнта_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
