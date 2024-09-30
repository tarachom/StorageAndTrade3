
/*
    ПсуванняТоварів_Triggers.cs
    Тригери для документу ПсуванняТоварів
*/

using StorageAndTrade_1_0.Константи;
using StorageAndTrade;

namespace StorageAndTrade_1_0.Документи
{
    class ПсуванняТоварів_Triggers
    {
        public static async ValueTask New(ПсуванняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПсуванняТоварів_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПсуванняТоварів_Objest ДокументОбєкт, ПсуванняТоварів_Objest Основа)
        {
            ДокументОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПсуванняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ПсуванняТоварів_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПсуванняТоварів_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПсуванняТоварів_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПсуванняТоварів_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
