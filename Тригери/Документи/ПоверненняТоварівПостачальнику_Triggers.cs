
/*
    ПоверненняТоварівПостачальнику_Triggers.cs
    Тригери для документу ПоверненняТоварівПостачальнику
*/

using StorageAndTrade_1_0.Константи;
using StorageAndTrade;

namespace StorageAndTrade_1_0.Документи
{
    class ПоверненняТоварівПостачальнику_Triggers
    {
        public static async ValueTask New(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПоверненняТоварівПостачальнику_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;
            
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт, ПоверненняТоварівПостачальнику_Objest Основа)
        {
            ДокументОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ПоверненняТоварівПостачальнику_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
