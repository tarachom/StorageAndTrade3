
/*
    ВстановленняЦінНоменклатури_Triggers.cs
    Тригери для документу ВстановленняЦінНоменклатури
*/

using GeneratedCode.Константи;
using StorageAndTrade;

namespace GeneratedCode.Документи
{
    class ВстановленняЦінНоменклатури_Triggers
    {
        public static async ValueTask New(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ВстановленняЦінНоменклатури_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ВстановленняЦінНоменклатури_Objest ДокументОбєкт, ВстановленняЦінНоменклатури_Objest Основа)
        {
            ДокументОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ВстановленняЦінНоменклатури_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ВстановленняЦінНоменклатури_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
