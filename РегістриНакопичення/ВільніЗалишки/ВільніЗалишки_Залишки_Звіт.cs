
/*
        ВільніЗалишки_Залишки_Звіт.cs
        Звіт
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade
{
    public static class ВільніЗалишки_Залишки_Звіт
    {
        public static async ValueTask Сформувати(DateTime ДатаПочатокПеріоду, DateTime ДатаКінецьПеріоду)
        {

            string query = $@"
SELECT
    to_char(Залишки.{ВільніЗалишки_Залишки_TablePart.Період}, 'DD.MM.YYYY') AS Період,
    Залишки.{ВільніЗалишки_Залишки_TablePart.Номенклатура} AS Номенклатура,
            Довідники_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    Залишки.{ВільніЗалишки_Залишки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
            Довідники_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    Залишки.{ВільніЗалишки_Залишки_TablePart.Склад} AS Склад,
            Довідники_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Залишки.{ВільніЗалишки_Залишки_TablePart.ВНаявності} AS ВНаявності,
    Залишки.{ВільніЗалишки_Залишки_TablePart.ВРезервіЗіСкладу} AS ВРезервіЗіСкладу,
    Залишки.{ВільніЗалишки_Залишки_TablePart.ВРезервіПідЗамовлення} AS ВРезервіПідЗамовлення
FROM
    {ВільніЗалишки_Залишки_TablePart.TABLE} AS Залишки
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідники_Номенклатура ON Довідники_Номенклатура.uid = 
        Залишки.{ВільніЗалишки_Залишки_TablePart.Номенклатура}
    
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідники_ХарактеристикиНоменклатури ON Довідники_ХарактеристикиНоменклатури.uid = 
        Залишки.{ВільніЗалишки_Залишки_TablePart.ХарактеристикаНоменклатури}
    
    LEFT JOIN {Склади_Const.TABLE} AS Довідники_Склади ON Довідники_Склади.uid = 
        Залишки.{ВільніЗалишки_Залишки_TablePart.Склад}

WHERE
    Залишки.{ВільніЗалишки_Залишки_TablePart.Період} >= @ПочатокПеріоду AND
    Залишки.{ВільніЗалишки_Залишки_TablePart.Період} <= @КінецьПеріоду

ORDER BY Залишки.{ВільніЗалишки_Залишки_TablePart.Період}
";
            Dictionary<string, object> paramQuery = new Dictionary<string, object>
            {
                { "ПочатокПеріоду", ДатаПочатокПеріоду },
                { "КінецьПеріоду", ДатаКінецьПеріоду }
            };

            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "ВільніЗалишки_Залишки_Звіт",
                Caption = "Залишки",
                Query = query,
                ParamQuery = paramQuery,
                GetInfo = () =>
                {
                    string text = $"З {ДатаПочатокПеріоду.ToString("dd.MM.yyyy")} по {ДатаКінецьПеріоду.ToString("dd.MM.yyyy")}";
                    return ValueTask.FromResult(text);
                }
            };

            Звіт.ColumnSettings.Add("Період", new("Період"));
            Звіт.ColumnSettings.Add("Номенклатура_Назва", new("Номенклатура", "Номенклатура", Номенклатура_Const.POINTER));
            Звіт.ColumnSettings.Add("ХарактеристикаНоменклатури_Назва", new("Характеристика", "ХарактеристикаНоменклатури", ХарактеристикиНоменклатури_Const.POINTER));
            Звіт.ColumnSettings.Add("Склад_Назва", new("Склад", "Склад", Склади_Const.POINTER));
            Звіт.ColumnSettings.Add("ВНаявності", new("В наявності", "", "", 1));
            Звіт.ColumnSettings.Add("ВРезервіЗіСкладу", new("В резерві зі складу", "", "", 1));
            Звіт.ColumnSettings.Add("ВРезервіПідЗамовлення", new("В резерві під замовлення", "", "", 1));

            await Звіт.Select();

            Звіт.FillTreeView();
            await Звіт.View(Program.GeneralNotebook);
        }
    }
}
