
/*
        РозрахункиЗПостачальниками_Залишки_Звіт.cs
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
    public static class РозрахункиЗПостачальниками_Залишки_Звіт
    {
        public static async ValueTask Сформувати(DateTime ДатаПочатокПеріоду, DateTime ДатаКінецьПеріоду)
        {

            string query = $@"
SELECT
    to_char(Залишки.{РозрахункиЗПостачальниками_Залишки_TablePart.Період}, 'DD.MM.YYYY') AS Період,
    Залишки.{РозрахункиЗПостачальниками_Залишки_TablePart.Контрагент} AS Контрагент,
            Довідники_Контрагенти.{Контрагенти_Const.Назва} AS Контрагент_Назва,
    Залишки.{РозрахункиЗПостачальниками_Залишки_TablePart.Валюта} AS Валюта,
            Довідники_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    Залишки.{РозрахункиЗПостачальниками_Залишки_TablePart.Сума} AS Сума
FROM
    {РозрахункиЗПостачальниками_Залишки_TablePart.TABLE} AS Залишки
    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідники_Контрагенти ON Довідники_Контрагенти.uid = 
        Залишки.{РозрахункиЗПостачальниками_Залишки_TablePart.Контрагент}
    
    LEFT JOIN {Валюти_Const.TABLE} AS Довідники_Валюти ON Довідники_Валюти.uid = 
        Залишки.{РозрахункиЗПостачальниками_Залишки_TablePart.Валюта}

WHERE
    Залишки.{РозрахункиЗПостачальниками_Залишки_TablePart.Період} >= @ПочатокПеріоду AND
    Залишки.{РозрахункиЗПостачальниками_Залишки_TablePart.Період} <= @КінецьПеріоду

ORDER BY Залишки.{РозрахункиЗПостачальниками_Залишки_TablePart.Період}
";
            Dictionary<string, object> paramQuery = new Dictionary<string, object>
            {
                { "ПочатокПеріоду", ДатаПочатокПеріоду },
                { "КінецьПеріоду", ДатаКінецьПеріоду }
            };

            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "РозрахункиЗПостачальниками_Залишки_Звіт",
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
            Звіт.ColumnSettings.Add("Контрагент_Назва", new("Контрагент", "Контрагент", Контрагенти_Const.POINTER));
            Звіт.ColumnSettings.Add("Валюта_Назва", new("Валюта", "Валюта", Валюти_Const.POINTER));
            Звіт.ColumnSettings.Add("Сума", new("Сума", "", "", 1));

            await Звіт.Select();

            Звіт.FillTreeView();
            Звіт.View(Program.GeneralNotebook);
        }
    }
}
