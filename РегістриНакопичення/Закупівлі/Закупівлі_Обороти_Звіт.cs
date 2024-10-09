
/*
        Закупівлі_Обороти_Звіт.cs
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
    public static class Закупівлі_Обороти_Звіт
    {
        public static async ValueTask Сформувати(DateTime ДатаПочатокПеріоду, DateTime ДатаКінецьПеріоду)
        {

            string query = $@"
SELECT
    to_char(Обороти.{Закупівлі_Обороти_TablePart.Період}, 'DD.MM.YYYY') AS Період,
    Обороти.{Закупівлі_Обороти_TablePart.Організація} AS Організація,
            Довідники_Організації.{Організації_Const.Назва} AS Організація_Назва,
    Обороти.{Закупівлі_Обороти_TablePart.Склад} AS Склад,
            Довідники_Склади.{Склади_Const.Назва} AS Склад_Назва,
    Обороти.{Закупівлі_Обороти_TablePart.Контрагент} AS Контрагент,
            Довідники_Контрагенти.{Контрагенти_Const.Назва} AS Контрагент_Назва,
    Обороти.{Закупівлі_Обороти_TablePart.Договір} AS Договір,
            concat_ws (', ', Довідники_ДоговориКонтрагентів.{ДоговориКонтрагентів_Const.Назва}, 
                             Довідники_ДоговориКонтрагентів.{ДоговориКонтрагентів_Const.ТипДоговоруПредставлення}) AS Договір_Назва,
    Обороти.{Закупівлі_Обороти_TablePart.Номенклатура} AS Номенклатура,
            Довідники_Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва,
    Обороти.{Закупівлі_Обороти_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
            Довідники_ХарактеристикиНоменклатури.{ХарактеристикиНоменклатури_Const.Назва} AS ХарактеристикаНоменклатури_Назва,
    Обороти.{Закупівлі_Обороти_TablePart.Кількість} AS Кількість,
    Обороти.{Закупівлі_Обороти_TablePart.Сума} AS Сума,
    Обороти.{Закупівлі_Обороти_TablePart.Собівартість} AS Собівартість
FROM
    {Закупівлі_Обороти_TablePart.TABLE} AS Обороти
    LEFT JOIN {Організації_Const.TABLE} AS Довідники_Організації ON Довідники_Організації.uid = 
        Обороти.{Закупівлі_Обороти_TablePart.Організація}
    
    LEFT JOIN {Склади_Const.TABLE} AS Довідники_Склади ON Довідники_Склади.uid = 
        Обороти.{Закупівлі_Обороти_TablePart.Склад}
    
    LEFT JOIN {Контрагенти_Const.TABLE} AS Довідники_Контрагенти ON Довідники_Контрагенти.uid = 
        Обороти.{Закупівлі_Обороти_TablePart.Контрагент}
    
    LEFT JOIN {ДоговориКонтрагентів_Const.TABLE} AS Довідники_ДоговориКонтрагентів ON Довідники_ДоговориКонтрагентів.uid = 
        Обороти.{Закупівлі_Обороти_TablePart.Договір}
    
    LEFT JOIN {Номенклатура_Const.TABLE} AS Довідники_Номенклатура ON Довідники_Номенклатура.uid = 
        Обороти.{Закупівлі_Обороти_TablePart.Номенклатура}
    
    LEFT JOIN {ХарактеристикиНоменклатури_Const.TABLE} AS Довідники_ХарактеристикиНоменклатури ON Довідники_ХарактеристикиНоменклатури.uid = 
        Обороти.{Закупівлі_Обороти_TablePart.ХарактеристикаНоменклатури}

WHERE
    Обороти.{Закупівлі_Обороти_TablePart.Період} >= @ПочатокПеріоду AND
    Обороти.{Закупівлі_Обороти_TablePart.Період} <= @КінецьПеріоду

ORDER BY Обороти.{Закупівлі_Обороти_TablePart.Період}
";
            Dictionary<string, object> paramQuery = new Dictionary<string, object>
            {
                { "ПочатокПеріоду", ДатаПочатокПеріоду },
                { "КінецьПеріоду", ДатаКінецьПеріоду }
            };

            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "Закупівлі_Обороти_Звіт",
                Caption = "Обороти",
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
            Звіт.ColumnSettings.Add("Склад_Назва", new("Склад", "Склад", Склади_Const.POINTER));
            Звіт.ColumnSettings.Add("Контрагент_Назва", new("Контрагент", "Контрагент", Контрагенти_Const.POINTER));
            Звіт.ColumnSettings.Add("Договір_Назва", new("Договір", "Договір", ДоговориКонтрагентів_Const.POINTER));
            Звіт.ColumnSettings.Add("Номенклатура_Назва", new("Номенклатура", "Номенклатура", Номенклатура_Const.POINTER));
            Звіт.ColumnSettings.Add("ХарактеристикаНоменклатури_Назва", new("Характеристика", "ХарактеристикаНоменклатури", ХарактеристикиНоменклатури_Const.POINTER));
            Звіт.ColumnSettings.Add("Кількість", new("Кількість", "", "", 1));
            Звіт.ColumnSettings.Add("Сума", new("Сума", "", "", 1));
            Звіт.ColumnSettings.Add("Собівартість", new("Собівартість", "", "", 1));

            await Звіт.Select();

            Звіт.FillTreeView();
            Звіт.View(Program.GeneralNotebook);
        }
    }
}
