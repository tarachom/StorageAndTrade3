/*

Журнали

*/

using Gtk;
using InterfaceGtk;

namespace StorageAndTrade
{
    class Menu_Journal : Форма
    {
        public Menu_Journal() : base()
        {
            //Список
            Box hBoxList = new Box(Orientation.Horizontal, 0);
            PackStart(hBoxList, false, false, 10);

            Box vLeft = new Box(Orientation.Vertical, 0);
            hBoxList.PackStart(vLeft, false, false, 5);

            CreateLink(vLeft, "Повний (всі документи)", async () =>
            {
                Журнал_Повний page = new Журнал_Повний();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Повний", () => page);
                await page.SetValue();
            });

            CreateLink(vLeft, "Продажі", async () =>
            {
                Журнал_Продажі page = new Журнал_Продажі();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Продажі", () => page);
                await page.SetValue();
            });

            CreateLink(vLeft, "Закупівлі", async () =>
            {
                Журнал_Закупівлі page = new Журнал_Закупівлі();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Закупівлі", () => page);
                await page.SetValue();
            });

            CreateLink(vLeft, "Каса", async () =>
            {
                Журнал_Каса page = new Журнал_Каса();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Каса", () => page);
                await page.SetValue();
            });

            CreateLink(vLeft, "Склад", async () =>
            {
                Журнал_Склад page = new Журнал_Склад();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Склад", () => page);
                await page.SetValue();
            });

            CreateLink(vLeft, "Адресне зберігання на складах", async () =>
            {
                Журнал_АдреснеЗберігання page = new Журнал_АдреснеЗберігання();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Адресне зберігання", () => page);
                await page.SetValue();
            });

            ShowAll();
        }
    }
}