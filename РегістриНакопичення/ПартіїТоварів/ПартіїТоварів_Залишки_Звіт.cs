
/*
        ПартіїТоварів_Залишки_Звіт.cs
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
    public static class ПартіїТоварів_Залишки_Звіт
    {
        public static async ValueTask Сформувати(DateTime ДатаПочатокПеріоду, DateTime ДатаКінецьПеріоду)
        {

            string query = $@"
SELECT
    to_char(Залишки.{ПартіїТоварів_Залишки_TablePart.Період}, 'DD.MM.YYYY') AS Період,
    Залишки.{ПартіїТоварів_Залишки_TablePart.Організація} AS Організація,
            Довідники_Організації.{Організації_Const.Назва} AS Організація_Назва,
    Залишки.{ПартіїТоварів_Залишки_TablePart.ПартіяТоварівКомпозит} AS ПартіяТоварівКомпозит,
            Довідники_ПартіяТоварівКомпозит.{ПартіяТоварівКомпозит_Const.Назва} AS ПартіяТоварівКомпозит_Назва,
    Залишки.{ПартіїТоварів_Залишки_TablePart.Номенклатура} AS Номенклатура,
            Довідники_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    Залишки.{ПартіїТоварів_Залишки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
            Довідники_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    Залишки.{ПартіїТоварів_Залишки_TablePart.Серія} AS Серія,
            Довідники_СеріїНоменклатури.{СеріїНоменклатури_Const.Номер} AS Серія_Назва,
    Залишки.{ПартіїТоварів_Залишки_TablePart.Склад} AS Склад,
            Довідники_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Залишки.{ПартіїТоварів_Залишки_TablePart.Рядок} AS Рядок,
    Залишки.{ПартіїТоварів_Залишки_TablePart.Кількість} AS Кількість,
    Залишки.{ПартіїТоварів_Залишки_TablePart.Собівартість} AS Собівартість
FROM
    {ПартіїТоварів_Залишки_TablePart.TABLE} AS Залишки
    LEFT JOIN {Організації_Const.TABLE} AS Довідники_Організації ON Довідники_Організації.uid = 
        Залишки.{ПартіїТоварів_Залишки_TablePart.Організація}
    
    LEFT JOIN {ПартіяТоварівКомпозит_Const.TABLE} AS Довідники_ПартіяТоварівКомпозит ON Довідники_ПартіяТоварівКомпозит.uid = 
        Залишки.{ПартіїТоварів_Залишки_TablePart.ПартіяТоварівКомпозит}
    
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідники_Номенклатура ON Довідники_Номенклатура.uid = 
        Залишки.{ПартіїТоварів_Залишки_TablePart.Номенклатура}
    
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідники_ХарактеристикиНоменклатури ON Довідники_ХарактеристикиНоменклатури.uid = 
        Залишки.{ПартіїТоварів_Залишки_TablePart.ХарактеристикаНоменклатури}
    
    LEFT JOIN {СеріїНоменклатури_Const.TABLE} AS Довідники_СеріїНоменклатури ON Довідники_СеріїНоменклатури.uid = 
        Залишки.{ПартіїТоварів_Залишки_TablePart.Серія}
    
    LEFT JOIN {Склади_Const.TABLE} AS Довідники_Склади ON Довідники_Склади.uid = 
        Залишки.{ПартіїТоварів_Залишки_TablePart.Склад}

WHERE
    Залишки.{ПартіїТоварів_Залишки_TablePart.Період} >= @ПочатокПеріоду AND
    Залишки.{ПартіїТоварів_Залишки_TablePart.Період} <= @КінецьПеріоду

ORDER BY Залишки.{ПартіїТоварів_Залишки_TablePart.Період}
";
            Dictionary<string, object> paramQuery = new()
            {
                { "ПочатокПеріоду", ДатаПочатокПеріоду },
                { "КінецьПеріоду", ДатаКінецьПеріоду }
            };

            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "ПартіїТоварів_Залишки_Звіт",
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
            Звіт.ColumnSettings.Add("Організація_Назва", new("Організація", "Організація", Організації_Const.POINTER));
            Звіт.ColumnSettings.Add("ПартіяТоварівКомпозит_Назва", new("Партія", "ПартіяТоварівКомпозит", ПартіяТоварівКомпозит_Const.POINTER));
            Звіт.ColumnSettings.Add("Номенклатура_Назва", new("Номенклатура", "Номенклатура", Номенклатура_Const.POINTER));
            Звіт.ColumnSettings.Add("ХарактеристикаНоменклатури_Назва", new("Характеристика", "ХарактеристикаНоменклатури", ХарактеристикиНоменклатури_Const.POINTER));
            Звіт.ColumnSettings.Add("Серія_Назва", new("Серія", "Серія", СеріїНоменклатури_Const.POINTER));
            Звіт.ColumnSettings.Add("Склад_Назва", new("Склад", "Склад", Склади_Const.POINTER));
            Звіт.ColumnSettings.Add("Рядок", new("Рядок", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
            Звіт.ColumnSettings.Add("Кількість", new("Кількість", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
            Звіт.ColumnSettings.Add("Собівартість", new("Собівартість", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));

            await Звіт.Select();

            Звіт.FillTreeView();
            await Звіт.View(Program.GeneralNotebook);
        }
    }
}
