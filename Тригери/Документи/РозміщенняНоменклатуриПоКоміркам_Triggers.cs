
/*
    РозміщенняНоменклатуриПоКоміркам_Triggers.cs
    Тригери для документу РозміщенняНоменклатуриПоКоміркам
*/

using StorageAndTrade_1_0.Константи;
using StorageAndTrade;

namespace StorageAndTrade_1_0.Документи
{
    class РозміщенняНоменклатуриПоКоміркам_Triggers
    {
        public static async ValueTask New(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.РозміщенняНоменклатуриПоКоміркам_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт, РозміщенняНоменклатуриПоКоміркам_Objest Основа)
        {
            ДокументОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{РозміщенняНоменклатуриПоКоміркам_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
