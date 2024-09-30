
/*
    ЗамовленняПостачальнику_Triggers.cs
    Тригери для документу ЗамовленняПостачальнику
*/

using StorageAndTrade_1_0.Константи;
using StorageAndTrade;

namespace StorageAndTrade_1_0.Документи
{
    class ЗамовленняПостачальнику_Triggers
    {
        public static async ValueTask New(ЗамовленняПостачальнику_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ЗамовленняПостачальнику_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;
            
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ЗамовленняПостачальнику_Objest ДокументОбєкт, ЗамовленняПостачальнику_Objest Основа)
        {
            ДокументОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ЗамовленняПостачальнику_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ЗамовленняПостачальнику_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ЗамовленняПостачальнику_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ЗамовленняПостачальнику_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ЗамовленняПостачальнику_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
