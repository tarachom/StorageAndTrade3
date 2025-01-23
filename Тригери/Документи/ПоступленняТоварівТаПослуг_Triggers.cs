
/*
    ПоступленняТоварівТаПослуг_Triggers.cs
    Тригери для документу ПоступленняТоварівТаПослуг
*/

using GeneratedCode.Константи;
using GeneratedCode.Довідники;
using AccountingSoftware;
using StorageAndTrade;

namespace GeneratedCode.Документи
{
    class ПоступленняТоварівТаПослуг_Triggers
    {
        public static async ValueTask New(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПоступленняТоварівТаПослуг_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт, ПоступленняТоварівТаПослуг_Objest Основа)
        {
            ДокументОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ПоступленняТоварівТаПослуг_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт, bool label)
        {
            // Помітка на видалення всіх партій
            if (label)
            {
                ПартіяТоварівКомпозит_Select select = new ПартіяТоварівКомпозит_Select();
                select.QuerySelect.Where.Add(new Where(ПартіяТоварівКомпозит_Const.ПоступленняТоварівТаПослуг, Comparison.EQ, ДокументОбєкт.UnigueID.UGuid));
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

        public static async ValueTask BeforeDelete(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
