
/*
    ПереміщенняТоварівНаСкладі_Triggers.cs
    Тригери для документу ПереміщенняТоварівНаСкладі
*/

using StorageAndTrade_1_0.Константи;
using StorageAndTrade;

namespace StorageAndTrade_1_0.Документи
{
    class ПереміщенняТоварівНаСкладі_Triggers
    {
        public static async ValueTask New(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПереміщенняТоварівНаСкладі_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт, ПереміщенняТоварівНаСкладі_Objest Основа)
        {
            ДокументОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ПереміщенняТоварівНаСкладі_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
