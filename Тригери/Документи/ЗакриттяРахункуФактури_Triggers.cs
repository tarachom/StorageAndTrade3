
/*
    ЗакриттяРахункуФактури_Triggers.cs
    Тригери для документу ЗакриттяРахункуФактури
*/

using StorageAndTrade_1_0.Константи;
using StorageAndTrade;

namespace StorageAndTrade_1_0.Документи
{
    class ЗакриттяРахункуФактури_Triggers
    {
        public static async ValueTask New(ЗакриттяРахункуФактури_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ЗакриттяРахункуФактури_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ЗакриттяРахункуФактури_Objest ДокументОбєкт, ЗакриттяРахункуФактури_Objest Основа)
        {
            ДокументОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ЗакриттяРахункуФактури_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ЗакриттяРахункуФактури_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ЗакриттяРахункуФактури_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ЗакриттяРахункуФактури_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ЗакриттяРахункуФактури_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
