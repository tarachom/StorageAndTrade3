
/*
        РухКоштів_Підсумки_Звіт.cs
        Звіт
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using GeneratedCode.Константи;
using GeneratedCode.Довідники;
using GeneratedCode.Документи;
using GeneratedCode.РегістриНакопичення;

namespace StorageAndTrade
{
    public static class РухКоштів_Підсумки_Звіт
    {
        public static async ValueTask Сформувати()
        {

            string query = $@"
SELECT
    Підсумки.{РухКоштів_Підсумки_TablePart.Організація} AS Організація,
            Довідники_Організації.{Організації_Const.Назва} AS Організація_Назва,
    Підсумки.{РухКоштів_Підсумки_TablePart.Каса} AS Каса,
            Довідники_Каси.{Каси_Const.Назва} AS Каса_Назва,
    Підсумки.{РухКоштів_Підсумки_TablePart.Валюта} AS Валюта,
            Довідники_Валюти.{Валюти_Const.Назва} AS Валюта_Назва,
    Підсумки.{РухКоштів_Підсумки_TablePart.Сума} AS Сума
FROM
    {РухКоштів_Підсумки_TablePart.TABLE} AS Підсумки
    LEFT JOIN {Організації_Const.TABLE} AS Довідники_Організації ON Довідники_Організації.uid = 
        Підсумки.{РухКоштів_Підсумки_TablePart.Організація}
    
    LEFT JOIN {Каси_Const.TABLE} AS Довідники_Каси ON Довідники_Каси.uid = 
        Підсумки.{РухКоштів_Підсумки_TablePart.Каса}
    
    LEFT JOIN {Валюти_Const.TABLE} AS Довідники_Валюти ON Довідники_Валюти.uid = 
        Підсумки.{РухКоштів_Підсумки_TablePart.Валюта}
";
            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "РухКоштів_Підсумки_Звіт",
                Caption = "Підсумки",
                Query = query
            };

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
