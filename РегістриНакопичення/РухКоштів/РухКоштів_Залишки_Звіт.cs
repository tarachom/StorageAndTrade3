
/*
        РухКоштів_Залишки_Звіт.cs
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
    public static class РухКоштів_Залишки_Звіт
    {
        public static async ValueTask Сформувати(DateTime ДатаПочатокПеріоду, DateTime ДатаКінецьПеріоду)
        {

            string query = $@"
SELECT
    to_char(Залишки.{РухКоштів_Залишки_TablePart.Період}, 'DD.MM.YYYY') AS Період,
    Залишки.{РухКоштів_Залишки_TablePart.Організація} AS Організація,
            Довідники_Організації.{Організації_Const.Назва} AS Організація_Назва,
    Залишки.{РухКоштів_Залишки_TablePart.Каса} AS Каса,
            Довідники_Каси.{Каси_Const.Назва} AS Каса_Назва,
    Залишки.{РухКоштів_Залишки_TablePart.Валюта} AS Валюта,
            Довідники_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    Залишки.{РухКоштів_Залишки_TablePart.Сума} AS Сума
FROM
    {РухКоштів_Залишки_TablePart.TABLE} AS Залишки
    LEFT JOIN {Організації_Const.TABLE} AS Довідники_Організації ON Довідники_Організації.uid = 
        Залишки.{РухКоштів_Залишки_TablePart.Організація}
    
    LEFT JOIN {Каси_Const.TABLE} AS Довідники_Каси ON Довідники_Каси.uid = 
        Залишки.{РухКоштів_Залишки_TablePart.Каса}
    
    LEFT JOIN {Валюти_Const.TABLE} AS Довідники_Валюти ON Довідники_Валюти.uid = 
        Залишки.{РухКоштів_Залишки_TablePart.Валюта}

WHERE
    Залишки.{РухКоштів_Залишки_TablePart.Період} >= @ПочатокПеріоду AND
    Залишки.{РухКоштів_Залишки_TablePart.Період} <= @КінецьПеріоду

ORDER BY Залишки.{РухКоштів_Залишки_TablePart.Період}
";
            Dictionary<string, object> paramQuery = new()
            {
                { "ПочатокПеріоду", ДатаПочатокПеріоду },
                { "КінецьПеріоду", ДатаКінецьПеріоду }
            };

            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "РухКоштів_Залишки_Звіт",
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
            Звіт.ColumnSettings.Add("Каса_Назва", new("Каса", "Каса", Каси_Const.POINTER));
            Звіт.ColumnSettings.Add("Валюта_Назва", new("Валюта", "Валюта", Валюти_Const.POINTER));
            Звіт.ColumnSettings.Add("Сума", new("Сума", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));

            await Звіт.Select();

            Звіт.FillTreeView();
            await Звіт.View(Program.GeneralNotebook);
        }
    }
}
