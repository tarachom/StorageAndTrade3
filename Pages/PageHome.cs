/*

Стартова сторінка.

*/

using Gtk;
using InterfaceGtk;
using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class PageHome : Box
    {
        public БлокДляСторінки_КурсиВалют БлокКурсиВалют = new БлокДляСторінки_КурсиВалют() { WidthRequest = 500 };
        public БлокДляСторінки_АктивніКористувачі АктивніКористувачі = new БлокДляСторінки_АктивніКористувачі(Config.Kernel) { WidthRequest = 500 };

        public PageHome() : base(Orientation.Vertical, 0)
        {
            {
                Box hBox = new Box(Orientation.Horizontal, 0);
                hBox.PackStart(БлокКурсиВалют, false, false, 5);

                PackStart(hBox, false, false, 5);
            }

            {
                Box hBox = new Box(Orientation.Horizontal, 0);
                hBox.PackStart(АктивніКористувачі, false, false, 5);
                PackStart(hBox, false, false, 5);
            }

            ShowAll();
        }

        /*async void TestReport()
        {
            string query = $@"
SELECT
    Номенклатура.uid AS Номенклатура,
    Номенклатура.{Номенклатура_Const.Код} AS Номенклатура_Код,
    Номенклатура.{Номенклатура_Const.Назва} AS Номенклатура_Назва
FROM
    {Номенклатура_Const.TABLE} AS Номенклатура
";
            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "Номенклатура",
                Caption = "Номенклатура",
                Query = query,
                GetInfo = () => ValueTask.FromResult("Test")
            };

            Звіт.ColumnSettings.Add("Номенклатура_Код", new("Код"));
            Звіт.ColumnSettings.Add("Номенклатура_Назва", new("Назва", "Номенклатура", Номенклатура_Const.POINTER));
            Звіт.ColumnSettings.Add("ТипНоменклатури", new("Тип"));

            await Звіт.Select();

            Звіт.FillTreeView();
            Звіт.View(Program.GeneralNotebook);
        }*/
    }
}