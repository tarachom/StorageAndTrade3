/*
    СеріїНоменклатури_Triggers.cs
    Тригери для довідника СеріїНоменклатури
*/

using AccountingSoftware;

namespace GeneratedCode.Довідники
{
    class СеріїНоменклатури_Triggers
    {
        public static async ValueTask New(СеріїНоменклатури_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.ДатаСтворення = DateTime.Now;
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(СеріїНоменклатури_Objest ДовідникОбєкт, СеріїНоменклатури_Objest Основа)
        {
            ДовідникОбєкт.Номер = Guid.NewGuid().ToString();
            ДовідникОбєкт.Коментар = "Копія - " + Основа.Номер;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(СеріїНоменклатури_Objest ДовідникОбєкт)
        {
            СеріїНоменклатури_Select select = new СеріїНоменклатури_Select();
            select.QuerySelect.Where.Add(new Where(СеріїНоменклатури_Const.Номер, Comparison.EQ, ДовідникОбєкт.Номер));
            select.QuerySelect.Where.Add(new Where("uid", Comparison.NOT, ДовідникОбєкт.UnigueID.UGuid));

            if (await select.SelectSingle())
            {
                ДовідникОбєкт.Коментар = $"Помилка: Серійний номер [ {ДовідникОбєкт.Номер} ] вже існує в базі даних. " + ДовідникОбєкт.Коментар;
                ДовідникОбєкт.Номер = Guid.NewGuid().ToString();
            }
        }

        public static async ValueTask AfterSave(СеріїНоменклатури_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(СеріїНоменклатури_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(СеріїНоменклатури_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
