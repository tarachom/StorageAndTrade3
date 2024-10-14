
/*
        ЗамовленняПостачальникам_Залишки_Звіт.cs
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
    public static class ЗамовленняПостачальникам_Залишки_Звіт
    {
        public static async ValueTask Сформувати(DateTime ДатаПочатокПеріоду, DateTime ДатаКінецьПеріоду)
        {

            string query = $@"
SELECT
    to_char(Залишки.{ЗамовленняПостачальникам_Залишки_TablePart.Період}, 'DD.MM.YYYY') AS Період,
    Залишки.{ЗамовленняПостачальникам_Залишки_TablePart.ЗамовленняПостачальнику} AS ЗамовленняПостачальнику,
            Документи_ЗамовленняПостачальнику.{ЗамовленняПостачальнику_Const.Назва} AS ЗамовленняПостачальнику_Назва,
    Залишки.{ЗамовленняПостачальникам_Залишки_TablePart.Номенклатура} AS Номенклатура,
            Довідники_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    Залишки.{ЗамовленняПостачальникам_Залишки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
            Довідники_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    Залишки.{ЗамовленняПостачальникам_Залишки_TablePart.Склад} AS Склад,
            Довідники_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Залишки.{ЗамовленняПостачальникам_Залишки_TablePart.Замовлено} AS Замовлено
FROM
    {ЗамовленняПостачальникам_Залишки_TablePart.TABLE} AS Залишки
    LEFT JOIN {ЗамовленняПостачальнику_Const.TABLE} AS Документи_ЗамовленняПостачальнику ON Документи_ЗамовленняПостачальнику.uid = 
        Залишки.{ЗамовленняПостачальникам_Залишки_TablePart.ЗамовленняПостачальнику}
    
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідники_Номенклатура ON Довідники_Номенклатура.uid = 
        Залишки.{ЗамовленняПостачальникам_Залишки_TablePart.Номенклатура}
    
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідники_ХарактеристикиНоменклатури ON Довідники_ХарактеристикиНоменклатури.uid = 
        Залишки.{ЗамовленняПостачальникам_Залишки_TablePart.ХарактеристикаНоменклатури}
    
    LEFT JOIN {Склади_Const.TABLE} AS Довідники_Склади ON Довідники_Склади.uid = 
        Залишки.{ЗамовленняПостачальникам_Залишки_TablePart.Склад}

WHERE
    Залишки.{ЗамовленняПостачальникам_Залишки_TablePart.Період} >= @ПочатокПеріоду AND
    Залишки.{ЗамовленняПостачальникам_Залишки_TablePart.Період} <= @КінецьПеріоду

ORDER BY Залишки.{ЗамовленняПостачальникам_Залишки_TablePart.Період}
";
            Dictionary<string, object> paramQuery = new()
            {
                { "ПочатокПеріоду", ДатаПочатокПеріоду },
                { "КінецьПеріоду", ДатаКінецьПеріоду }
            };

            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "ЗамовленняПостачальникам_Залишки_Звіт",
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
            Звіт.ColumnSettings.Add("ЗамовленняПостачальнику_Назва", new("Замовлення постачальнику", "ЗамовленняПостачальнику", ЗамовленняПостачальнику_Const.POINTER));
            Звіт.ColumnSettings.Add("Номенклатура_Назва", new("Номенклатура", "Номенклатура", Номенклатура_Const.POINTER));
            Звіт.ColumnSettings.Add("ХарактеристикаНоменклатури_Назва", new("Характеристика", "ХарактеристикаНоменклатури", ХарактеристикиНоменклатури_Const.POINTER));
            Звіт.ColumnSettings.Add("Склад_Назва", new("Склад", "Склад", Склади_Const.POINTER));
            Звіт.ColumnSettings.Add("Замовлено", new("Замовлено", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));

            await Звіт.Select();

            Звіт.FillTreeView();
            await Звіт.View(Program.GeneralNotebook);
        }
    }
}
