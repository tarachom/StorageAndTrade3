

/*
        ЧекККМ_Triggers.cs
        Тригери
*/

using GeneratedCode.Константи;
using AccountingSoftware;
using StorageAndTrade;

namespace GeneratedCode.Документи
{
    static class ЧекККМ_Triggers
    {
        public static async ValueTask New(ЧекККМ_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ЧекККМ_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ЧекККМ_Objest ДокументОбєкт, ЧекККМ_Objest Основа)
        {
            
            ДокументОбєкт.Назва += " - Копія";
            
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ЧекККМ_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ЧекККМ_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ЧекККМ_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ЧекККМ_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ЧекККМ_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
    