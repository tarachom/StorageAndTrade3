
/*
    ПерерахунокТоварів_Triggers.cs
    Тригери для документу ПерерахунокТоварів
*/

using GeneratedCode.Константи;
using StorageAndTrade;

namespace GeneratedCode.Документи
{
    class ПерерахунокТоварів_Triggers
    {
        public static async ValueTask New(ПерерахунокТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПерерахунокТоварів_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            //Відповідального можна отримати з Користувача
            var Користувач_Обєкт = await Program.Користувач.GetDirectoryObject();
            if (Користувач_Обєкт != null)
                ДокументОбєкт.Відповідальний = Користувач_Обєкт.ФізичнаОсоба;
        }

        public static async ValueTask Copying(ПерерахунокТоварів_Objest ДокументОбєкт, ПерерахунокТоварів_Objest Основа)
        {
            ДокументОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПерерахунокТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ПерерахунокТоварів_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПерерахунокТоварів_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПерерахунокТоварів_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПерерахунокТоварів_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
