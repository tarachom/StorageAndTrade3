
/*
    РеалізаціяТоварівТаПослуг_Triggers.cs
    Тригери для документу РеалізаціяТоварівТаПослуг
*/

using StorageAndTrade_1_0.Константи;
using StorageAndTrade;

namespace StorageAndTrade_1_0.Документи
{
    class РеалізаціяТоварівТаПослуг_Triggers
    {
        public static async ValueTask New(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.РеалізаціяТоварівТаПослуг_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт, РеалізаціяТоварівТаПослуг_Objest Основа)
        {
            ДокументОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{РеалізаціяТоварівТаПослуг_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
