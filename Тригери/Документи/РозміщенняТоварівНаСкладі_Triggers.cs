
/*
    РозміщенняТоварівНаСкладі_Triggers.cs
    Тригери для документу РозміщенняТоварівНаСкладі
*/

using StorageAndTrade_1_0.Константи;
using StorageAndTrade;

namespace StorageAndTrade_1_0.Документи
{
    class РозміщенняТоварівНаСкладі_Triggers
    {
        public static async ValueTask New(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.РозміщенняТоварівНаСкладі_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт, РозміщенняТоварівНаСкладі_Objest Основа)
        {
            ДокументОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{РозміщенняТоварівНаСкладі_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
