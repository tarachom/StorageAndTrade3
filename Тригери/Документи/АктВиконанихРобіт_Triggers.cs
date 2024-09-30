
/*
    АктВиконанихРобіт_Triggers.cs
    Тригери для документу АктВиконанихРобіт
*/

using StorageAndTrade_1_0.Константи;
using StorageAndTrade;

namespace StorageAndTrade_1_0.Документи
{
    class АктВиконанихРобіт_Triggers
    {
        public static async ValueTask New(АктВиконанихРобіт_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.АктВиконанихРобіт_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;
            
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(АктВиконанихРобіт_Objest ДокументОбєкт, АктВиконанихРобіт_Objest Основа)
        {
            ДокументОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(АктВиконанихРобіт_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{АктВиконанихРобіт_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(АктВиконанихРобіт_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(АктВиконанихРобіт_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(АктВиконанихРобіт_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
