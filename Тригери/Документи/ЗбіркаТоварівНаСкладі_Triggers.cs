
/*
    ЗбіркаТоварівНаСкладі_Triggers.cs
    Тригери для документу ЗбіркаТоварівНаСкладі
*/

using StorageAndTrade_1_0.Константи;
using StorageAndTrade;

namespace StorageAndTrade_1_0.Документи
{
    class ЗбіркаТоварівНаСкладі_Triggers
    {
        public static async ValueTask New(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ЗбіркаТоварівНаСкладі_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт, ЗбіркаТоварівНаСкладі_Objest Основа)
        {
            ДокументОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ЗбіркаТоварівНаСкладі_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
