
/*
    ЗакриттяЗамовленняПостачальнику_Triggers.cs
    Тригери для документу ЗакриттяЗамовленняПостачальнику
*/

using StorageAndTrade_1_0.Константи;

namespace StorageAndTrade_1_0.Документи
{
    class ЗакриттяЗамовленняПостачальнику_Triggers
    {
        public static async ValueTask New(ЗакриттяЗамовленняПостачальнику_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ЗакриттяЗамовленняПостачальнику_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ЗакриттяЗамовленняПостачальнику_Objest ДокументОбєкт, ЗакриттяЗамовленняПостачальнику_Objest Основа)
        {
            ДокументОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ЗакриттяЗамовленняПостачальнику_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ЗакриттяЗамовленняПостачальнику_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ЗакриттяЗамовленняПостачальнику_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ЗакриттяЗамовленняПостачальнику_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ЗакриттяЗамовленняПостачальнику_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
