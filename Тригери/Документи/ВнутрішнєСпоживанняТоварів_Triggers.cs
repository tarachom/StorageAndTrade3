
/*
    ВнутрішнєСпоживанняТоварів_Triggers.cs
    Тригери для документу ВнутрішнєСпоживанняТоварів
*/

using GeneratedCode.Константи;
using StorageAndTrade;

namespace GeneratedCode.Документи
{
    class ВнутрішнєСпоживанняТоварів_Triggers
    {
        public static async ValueTask New(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ВнутрішнєСпоживанняТоварів_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт, ВнутрішнєСпоживанняТоварів_Objest Основа)
        {
            ДокументОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ВнутрішнєСпоживанняТоварів_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
