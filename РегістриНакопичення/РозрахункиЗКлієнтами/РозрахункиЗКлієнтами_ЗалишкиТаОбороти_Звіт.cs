
/*
        РозрахункиЗКлієнтами_ЗалишкиТаОбороти_Звіт.cs
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
    public static class РозрахункиЗКлієнтами_ЗалишкиТаОбороти_Звіт
    {
        public static async ValueTask Сформувати(DateTime ДатаПочатокПеріоду, DateTime ДатаКінецьПеріоду)
        {

            string query = $@"
SELECT
    to_char(ЗалишкиТаОбороти.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.Період}, 'DD.MM.YYYY') AS Період,
    ЗалишкиТаОбороти.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.Валюта} AS Валюта,
            Довідники_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    ЗалишкиТаОбороти.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.Контрагент} AS Контрагент,
            Довідники_Контрагенти.{Контрагенти_Const.Назва} AS Контрагент_Назва,
    ЗалишкиТаОбороти.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.СумаПрихід} AS СумаПрихід,
    ЗалишкиТаОбороти.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.СумаРозхід} AS СумаРозхід,
    ЗалишкиТаОбороти.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.СумаЗалишок} AS СумаЗалишок
FROM
    {РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.TABLE} AS ЗалишкиТаОбороти
    LEFT JOIN {Валюти_Const.TABLE} AS Довідники_Валюти ON Довідники_Валюти.uid = 
        ЗалишкиТаОбороти.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.Валюта}
    
    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідники_Контрагенти ON Довідники_Контрагенти.uid = 
        ЗалишкиТаОбороти.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.Контрагент}

WHERE
    ЗалишкиТаОбороти.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.Період} >= @ПочатокПеріоду AND
    ЗалишкиТаОбороти.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.Період} <= @КінецьПеріоду

ORDER BY ЗалишкиТаОбороти.{РозрахункиЗКлієнтами_ЗалишкиТаОбороти_TablePart.Період}
";
            Dictionary<string, object> paramQuery = new Dictionary<string, object>
            {
                { "ПочатокПеріоду", ДатаПочатокПеріоду },
                { "КінецьПеріоду", ДатаКінецьПеріоду }
            };

            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "РозрахункиЗКлієнтами_ЗалишкиТаОбороти_Звіт",
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
            Звіт.ColumnSettings.Add("Валюта_Назва", new("Валюта", "Валюта", Валюти_Const.POINTER));
            Звіт.ColumnSettings.Add("Контрагент_Назва", new("Контрагент", "Контрагент", Контрагенти_Const.POINTER));
            Звіт.ColumnSettings.Add("СумаПрихід", new("Прихід", "", "", 1));
            Звіт.ColumnSettings.Add("СумаРозхід", new("Розхід", "", "", 1));
            Звіт.ColumnSettings.Add("СумаЗалишок", new("Сума", "", "", 1));

            await Звіт.Select();

            Звіт.FillTreeView();
            await Звіт.View(Program.GeneralNotebook);
        }
    }
}
