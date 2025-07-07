
/*
        РозрахункиЗКлієнтами_Залишки_Звіт.cs
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
    public static class РозрахункиЗКлієнтами_Залишки_Звіт
    {
        public static async ValueTask Сформувати(DateTime ДатаПочатокПеріоду, DateTime ДатаКінецьПеріоду)
        {
            string query = $@"
SELECT
    to_char(Залишки.{РозрахункиЗКлієнтами_Залишки_TablePart.Період}, 'DD.MM.YYYY') AS Період,
    Залишки.{РозрахункиЗКлієнтами_Залишки_TablePart.Валюта} AS Валюта,
            Довідники_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    Залишки.{РозрахункиЗКлієнтами_Залишки_TablePart.Контрагент} AS Контрагент,
            Довідники_Контрагенти.{Контрагенти_Const.Назва} AS Контрагент_Назва,
    Залишки.{РозрахункиЗКлієнтами_Залишки_TablePart.Сума} AS Сума
FROM
    {РозрахункиЗКлієнтами_Залишки_TablePart.TABLE} AS Залишки
    LEFT JOIN {Валюти_Const.TABLE} AS Довідники_Валюти ON Довідники_Валюти.uid = 
        Залишки.{РозрахункиЗКлієнтами_Залишки_TablePart.Валюта}
    
    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідники_Контрагенти ON Довідники_Контрагенти.uid = 
        Залишки.{РозрахункиЗКлієнтами_Залишки_TablePart.Контрагент}

WHERE
    Залишки.{РозрахункиЗКлієнтами_Залишки_TablePart.Період} >= @ПочатокПеріоду AND
    Залишки.{РозрахункиЗКлієнтами_Залишки_TablePart.Період} <= @КінецьПеріоду

ORDER BY Залишки.{РозрахункиЗКлієнтами_Залишки_TablePart.Період}
";
            Dictionary<string, object> paramQuery = new()
            {
                { "ПочатокПеріоду", ДатаПочатокПеріоду },
                { "КінецьПеріоду", ДатаКінецьПеріоду }
            };

            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "РозрахункиЗКлієнтами_Залишки_Звіт",
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
            Звіт.ColumnSettings.Add("Валюта_Назва", new("Валюта", "Валюта", Валюти_Const.POINTER));
            Звіт.ColumnSettings.Add("Контрагент_Назва", new("Контрагент", "Контрагент", Контрагенти_Const.POINTER));
            Звіт.ColumnSettings.Add("Сума", new("Сума", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));

            await Звіт.Select();

            Звіт.FillTreeView();
            await Звіт.View(Program.GeneralNotebook);
        }
    }
}
