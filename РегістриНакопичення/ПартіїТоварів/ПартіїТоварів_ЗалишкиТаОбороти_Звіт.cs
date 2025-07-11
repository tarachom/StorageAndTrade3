
/*
        ПартіїТоварів_ЗалишкиТаОбороти_Звіт.cs
        Звіт
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Константи;
using GeneratedCode.Довідники;
using GeneratedCode.Документи;
using GeneratedCode.РегістриНакопичення;

namespace StorageAndTrade
{
    public static class ПартіїТоварів_ЗалишкиТаОбороти_Звіт
    {
        public static async ValueTask Сформувати(DateTime ДатаПочатокПеріоду, DateTime ДатаКінецьПеріоду)
        {

            string query = $@"
SELECT
    to_char(ЗалишкиТаОбороти.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.Період}, 'DD.MM.YYYY') AS Період,
    ЗалишкиТаОбороти.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.Організація} AS Організація,
            Довідники_Організації.{Організації_Const.Назва} AS Організація_Назва,
    ЗалишкиТаОбороти.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.ПартіяТоварівКомпозит} AS ПартіяТоварівКомпозит,
            Довідники_ПартіяТоварівКомпозит.{ПартіяТоварівКомпозит_Const.Назва} AS ПартіяТоварівКомпозит_Назва,
    ЗалишкиТаОбороти.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.Номенклатура} AS Номенклатура,
            Довідники_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    ЗалишкиТаОбороти.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
            Довідники_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    ЗалишкиТаОбороти.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.Серія} AS Серія,
            Довідники_СеріїНоменклатури.{СеріїНоменклатури_Const.Номер} AS Серія_Назва,
    ЗалишкиТаОбороти.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.Склад} AS Склад,
            Довідники_Склади.{Склади_Const.Назва} AS Склад_Назва,
    ЗалишкиТаОбороти.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.Рядок} AS Рядок,
    ЗалишкиТаОбороти.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.КількістьПрихід} AS КількістьПрихід,
    ЗалишкиТаОбороти.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.КількістьРозхід} AS КількістьРозхід,
    ЗалишкиТаОбороти.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.КількістьЗалишок} AS КількістьЗалишок,
    ЗалишкиТаОбороти.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.СобівартістьПрихід} AS СобівартістьПрихід,
    ЗалишкиТаОбороти.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.СобівартістьРозхід} AS СобівартістьРозхід,
    ЗалишкиТаОбороти.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.СобівартістьЗалишок} AS СобівартістьЗалишок
FROM
    {ПартіїТоварів_ЗалишкиТаОбороти_TablePart.TABLE} AS ЗалишкиТаОбороти
    LEFT JOIN {Організації_Const.TABLE} AS Довідники_Організації ON Довідники_Організації.uid = 
        ЗалишкиТаОбороти.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.Організація}
    
    LEFT JOIN {ПартіяТоварівКомпозит_Const.TABLE} AS Довідники_ПартіяТоварівКомпозит ON Довідники_ПартіяТоварівКомпозит.uid = 
        ЗалишкиТаОбороти.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.ПартіяТоварівКомпозит}
    
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідники_Номенклатура ON Довідники_Номенклатура.uid = 
        ЗалишкиТаОбороти.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.Номенклатура}
    
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідники_ХарактеристикиНоменклатури ON Довідники_ХарактеристикиНоменклатури.uid = 
        ЗалишкиТаОбороти.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.ХарактеристикаНоменклатури}
    
    LEFT JOIN {СеріїНоменклатури_Const.TABLE} AS Довідники_СеріїНоменклатури ON Довідники_СеріїНоменклатури.uid = 
        ЗалишкиТаОбороти.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.Серія}
    
    LEFT JOIN {Склади_Const.TABLE} AS Довідники_Склади ON Довідники_Склади.uid = 
        ЗалишкиТаОбороти.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.Склад}

WHERE
    ЗалишкиТаОбороти.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.Період} >= @ПочатокПеріоду AND
    ЗалишкиТаОбороти.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.Період} <= @КінецьПеріоду

ORDER BY ЗалишкиТаОбороти.{ПартіїТоварів_ЗалишкиТаОбороти_TablePart.Період}
";
            Dictionary<string, object> paramQuery = new()
            {
                { "ПочатокПеріоду", ДатаПочатокПеріоду },
                { "КінецьПеріоду", ДатаКінецьПеріоду }
            };

            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "ПартіїТоварів_ЗалишкиТаОбороти_Звіт",
                Caption = "ЗалишкиТаОбороти",
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

            if (Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                Звіт.ColumnSettings.Add("ХарактеристикаНоменклатури_Назва", new("Характеристика", "ХарактеристикаНоменклатури", ХарактеристикиНоменклатури_Const.POINTER));

            if (Системні.ВестиОблікПоСеріяхНоменклатури_Const)
                Звіт.ColumnSettings.Add("Серія_Назва", new("Серія", "Серія", СеріїНоменклатури_Const.POINTER));

            Звіт.ColumnSettings.Add("Склад_Назва", new("Склад", "Склад", Склади_Const.POINTER));
            Звіт.ColumnSettings.Add("Рядок", new("Рядок", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
            Звіт.ColumnSettings.Add("КількістьПрихід", new("Кількість прихід", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
            Звіт.ColumnSettings.Add("КількістьРозхід", new("Кількість розхід", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
            Звіт.ColumnSettings.Add("КількістьЗалишок", new("Кількість", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
            Звіт.ColumnSettings.Add("СобівартістьПрихід", new("Собівартість прихід", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
            Звіт.ColumnSettings.Add("СобівартістьРозхід", new("Собівартість розхід", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
            Звіт.ColumnSettings.Add("СобівартістьЗалишок", new("Собівартість", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));

            await Звіт.Select();

            Звіт.FillTreeView();
            await Звіт.View(Program.GeneralNotebook);
        }
    }
}
