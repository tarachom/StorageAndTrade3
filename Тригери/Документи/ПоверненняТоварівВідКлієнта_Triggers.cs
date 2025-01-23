
/*
    ПоверненняТоварівВідКлієнта_Triggers.cs
    Тригери для документу ПоверненняТоварівВідКлієнта
*/

using GeneratedCode.Константи;
using StorageAndTrade;

namespace GeneratedCode.Документи
{
    class ПоверненняТоварівВідКлієнта_Triggers
    {
        public static async ValueTask New(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПоверненняТоварівВідКлієнта_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;
            
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт, ПоверненняТоварівВідКлієнта_Objest Основа)
        {
            ДокументОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ПоверненняТоварівВідКлієнта_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
