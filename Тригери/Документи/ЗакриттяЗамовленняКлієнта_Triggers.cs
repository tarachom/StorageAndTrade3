
/*
    ЗакриттяЗамовленняКлієнта_Triggers.cs
    Тригери для документу ЗакриттяЗамовленняКлієнта
*/

using GeneratedCode.Константи;
using StorageAndTrade;

namespace GeneratedCode.Документи
{
    class ЗакриттяЗамовленняКлієнта_Triggers
    {
        public static async ValueTask New(ЗакриттяЗамовленняКлієнта_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ЗакриттяЗамовленняКлієнта_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ЗакриттяЗамовленняКлієнта_Objest ДокументОбєкт, ЗакриттяЗамовленняКлієнта_Objest Основа)
        {
            ДокументОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ЗакриттяЗамовленняКлієнта_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ЗакриттяЗамовленняКлієнта_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ЗакриттяЗамовленняКлієнта_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ЗакриттяЗамовленняКлієнта_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ЗакриттяЗамовленняКлієнта_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
