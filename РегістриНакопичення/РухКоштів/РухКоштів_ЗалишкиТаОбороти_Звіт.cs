
/*
        РухКоштів_ЗалишкиТаОбороти_Звіт.cs
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
    public static class РухКоштів_ЗалишкиТаОбороти_Звіт
    {
        public static async ValueTask Сформувати(DateTime ДатаПочатокПеріоду, DateTime ДатаКінецьПеріоду)
        {

            string query = $@"
SELECT
    to_char(ЗалишкиТаОбороти.{РухКоштів_ЗалишкиТаОбороти_TablePart.Період}, 'DD.MM.YYYY') AS Період,
    ЗалишкиТаОбороти.{РухКоштів_ЗалишкиТаОбороти_TablePart.Організація} AS Організація,
            Довідники_Організації.{Організації_Const.Назва} AS Організація_Назва,
    ЗалишкиТаОбороти.{РухКоштів_ЗалишкиТаОбороти_TablePart.Каса} AS Каса,
            Довідники_Каси.{Каси_Const.Назва} AS Каса_Назва,
    ЗалишкиТаОбороти.{РухКоштів_ЗалишкиТаОбороти_TablePart.Валюта} AS Валюта,
            Довідники_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    ЗалишкиТаОбороти.{РухКоштів_ЗалишкиТаОбороти_TablePart.СумаПрихід} AS СумаПрихід,
    ЗалишкиТаОбороти.{РухКоштів_ЗалишкиТаОбороти_TablePart.СумаРозхід} AS СумаРозхід,
    ЗалишкиТаОбороти.{РухКоштів_ЗалишкиТаОбороти_TablePart.СумаЗалишок} AS СумаЗалишок
FROM
    {РухКоштів_ЗалишкиТаОбороти_TablePart.TABLE} AS ЗалишкиТаОбороти
    LEFT JOIN {Організації_Const.TABLE} AS Довідники_Організації ON Довідники_Організації.uid = 
        ЗалишкиТаОбороти.{РухКоштів_ЗалишкиТаОбороти_TablePart.Організація}
    
    LEFT JOIN {Каси_Const.TABLE} AS Довідники_Каси ON Довідники_Каси.uid = 
        ЗалишкиТаОбороти.{РухКоштів_ЗалишкиТаОбороти_TablePart.Каса}
    
    LEFT JOIN {Валюти_Const.TABLE} AS Довідники_Валюти ON Довідники_Валюти.uid = 
        ЗалишкиТаОбороти.{РухКоштів_ЗалишкиТаОбороти_TablePart.Валюта}

WHERE
    ЗалишкиТаОбороти.{РухКоштів_ЗалишкиТаОбороти_TablePart.Період} >= @ПочатокПеріоду AND
    ЗалишкиТаОбороти.{РухКоштів_ЗалишкиТаОбороти_TablePart.Період} <= @КінецьПеріоду

ORDER BY ЗалишкиТаОбороти.{РухКоштів_ЗалишкиТаОбороти_TablePart.Період}
";
            Dictionary<string, object> paramQuery = new Dictionary<string, object>
            {
                { "ПочатокПеріоду", ДатаПочатокПеріоду },
                { "КінецьПеріоду", ДатаКінецьПеріоду }
            };

            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "РухКоштів_ЗалишкиТаОбороти_Звіт",
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
            Звіт.ColumnSettings.Add("Каса_Назва", new("Каса", "Каса", Каси_Const.POINTER));
            Звіт.ColumnSettings.Add("Валюта_Назва", new("Валюта", "Валюта", Валюти_Const.POINTER));
            Звіт.ColumnSettings.Add("СумаПрихід", new("Прихід", "", "", 1));
            Звіт.ColumnSettings.Add("СумаРозхід", new("Розхід", "", "", 1));
            Звіт.ColumnSettings.Add("СумаЗалишок", new("Сума", "", "", 1));

            await Звіт.Select();

            Звіт.FillTreeView();
            await Звіт.View(Program.GeneralNotebook);
        }
    }
}
