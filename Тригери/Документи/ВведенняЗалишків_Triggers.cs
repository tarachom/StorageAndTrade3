
/*
    ВведенняЗалишків_Triggers.cs
    Тригери для документу ВведенняЗалишків
*/

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade;
using AccountingSoftware;

namespace StorageAndTrade_1_0.Документи
{
    class ВведенняЗалишків_Triggers
    {
        public static async ValueTask New(ВведенняЗалишків_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ВведенняЗалишків_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ВведенняЗалишків_Objest ДокументОбєкт, ВведенняЗалишків_Objest Основа)
        {
            ДокументОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ВведенняЗалишків_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ВведенняЗалишків_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ВведенняЗалишків_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ВведенняЗалишків_Objest ДокументОбєкт, bool label)
        {
            // Помітка на виделення всіх партій
            if (label)
            {
                ПартіяТоварівКомпозит_Select select = new ПартіяТоварівКомпозит_Select();
                select.QuerySelect.Where.Add(new Where(ПартіяТоварівКомпозит_Const.ВведенняЗалишків, Comparison.EQ, ДокументОбєкт.UnigueID.UGuid));
                select.QuerySelect.Where.Add(new Where(ПартіяТоварівКомпозит_Const.DELETION_LABEL, Comparison.NOT, true));
                await select.Select();

                while (select.MoveNext())
                    if (select.Current != null)
                    {
                        ПартіяТоварівКомпозит_Objest? Обєкт = await select.Current.GetDirectoryObject();
                        if (Обєкт != null)
                            await Обєкт.SetDeletionLabel();
                    }
            }
        }

        public static async ValueTask BeforeDelete(ВведенняЗалишків_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
