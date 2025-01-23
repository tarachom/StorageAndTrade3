
/*
    ПереміщенняТоварів_Triggers.cs
    Тригери для документу ПереміщенняТоварів
*/

using GeneratedCode.Константи;
using StorageAndTrade;

namespace GeneratedCode.Документи
{
    class ПереміщенняТоварів_Triggers
    {
        public static async ValueTask New(ПереміщенняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПереміщенняТоварів_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПереміщенняТоварів_Objest ДокументОбєкт, ПереміщенняТоварів_Objest Основа)
        {
            ДокументОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПереміщенняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ПереміщенняТоварів_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПереміщенняТоварів_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПереміщенняТоварів_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПереміщенняТоварів_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
