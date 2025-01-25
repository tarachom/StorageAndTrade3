

/*
        Номенклатура_Triggers.cs
        Тригери
*/


using GeneratedCode.Константи;
using AccountingSoftware;

namespace GeneratedCode.Довідники
{
    static class Номенклатура_Triggers
    {
        public static async ValueTask New(Номенклатура_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Номенклатура_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Номенклатура_Objest ДовідникОбєкт, Номенклатура_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Номенклатура_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Номенклатура_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Номенклатура_Objest ДовідникОбєкт, bool label)
        {
            // Помітка на видалення всіх характеристик елементу номенклатури у випадку label = true
            // Якщо мітка знімається, то з характеристик мітка не має зніматися
            if (label)
            {
                ХарактеристикиНоменклатури_Select select = new ХарактеристикиНоменклатури_Select();
                select.QuerySelect.Where.AddRange([
                    new Where(ХарактеристикиНоменклатури_Const.Номенклатура, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid),
                    new Where(ХарактеристикиНоменклатури_Const.DELETION_LABEL, Comparison.NOT, true)
                ]);

                await select.Select();

                while (select.MoveNext())
                    if (select.Current != null)
                        await select.Current.SetDeletionLabel();
            }

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(Номенклатура_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
            
            /*
            РегістриВідомостей.ШтрихкодиНоменклатури_RecordsSet r = new();
            r.QuerySelect.Where.Add(new Where(РегістриВідомостей.ШтрихкодиНоменклатури_Const.Номенклатура, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));

            await r.Read();
            await r.RemoveAll(r.Records);
            */

            //
            //Очистити регістр штрих-кодів
            //
            /*
                        string query = $@"
            DELETE FROM 
                {РегістриВідомостей.ШтрихкодиНоменклатури_Const.TABLE} AS ШтрихкодиНоменклатури
            WHERE
                ШтрихкодиНоменклатури.{РегістриВідомостей.ШтрихкодиНоменклатури_Const.Номенклатура} = @Номенклатура
            ";

                        Dictionary<string, object> paramQuery = new()
                        {
                            { "Номенклатура", ДовідникОбєкт.UnigueID.UGuid }
                        };

                        await Config.Kernel.DataBase.ExecuteSQL(query, paramQuery);
            */
            //
            //Очистка характеристик
            //

            //...
        }
    }
}
