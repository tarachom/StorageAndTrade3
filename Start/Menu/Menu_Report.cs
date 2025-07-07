/*

Звіти

*/

using Gtk;
using InterfaceGtk3;

namespace StorageAndTrade
{
    class Menu_Report : Форма
    {
        public Menu_Report() : base()
        {
            //Список
            Box hBoxList = new Box(Orientation.Horizontal, 0);

            Box vLeft = new Box(Orientation.Vertical, 0);
            hBoxList.PackStart(vLeft, false, false, 5);

            CreateLink(vLeft, "Збережені звіти", async () =>
            {
                ЗбереженіЗвіти page = new ЗбереженіЗвіти();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Збережені звіти", () => page);
                await page.SetValue();
            });

            CreateCaptionLink(vLeft, "Звіти", null);

            CreateLink(vLeft, "Товари на складах", async () =>
            {
                Звіт_ТовариНаСкладах page = new Звіт_ТовариНаСкладах();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Звіт - Товари на складах", () => page);
                await page.SetValue();
            });

            CreateLink(vLeft, "Вільні залишки", async () =>
            {
                Звіт_ВільніЗалишки page = new Звіт_ВільніЗалишки();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Звіт - Вільні залишки", () => page);
                await page.SetValue();
            });

            CreateLink(vLeft, "Партії товарів", async () =>
            {
                Звіт_ПартіїТоварів page = new Звіт_ПартіїТоварів();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Звіт - Партії товарів", () => page);
                await page.SetValue();
            });

            CreateLink(vLeft, "Рух коштів", async () =>
            {
                Звіт_РухКоштів page = new Звіт_РухКоштів();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Звіт - Рух коштів", () => page);
                await page.SetValue();
            });

            CreateLink(vLeft, "Розрахунки з контрагентами", async () =>
            {
                Звіт_РозрахункиЗКонтрагентами page = new Звіт_РозрахункиЗКонтрагентами();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Звіт - Розрахунки з контрагентами", () => page);
                await page.SetValue();
            });

            CreateLink(vLeft, "Розрахунки з клієнтами", async () =>
            {
                Звіт_РозрахункиЗКлієнтами page = new Звіт_РозрахункиЗКлієнтами();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Звіт - Розрахунки з клієнтами", () => page);
                await page.SetValue();
            });

            CreateLink(vLeft, "Розрахунки з постачальниками", async () =>
            {
                Звіт_РозрахункиЗПостачальниками page = new Звіт_РозрахункиЗПостачальниками();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Звіт - Розрахунки з постачальниками", () => page);
                await page.SetValue();
            });

            CreateLink(vLeft, "Замовлення клієнтів", async () =>
            {
                Звіт_ЗамовленняКлієнтів page = new Звіт_ЗамовленняКлієнтів();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Звіт - Замовлення клієнтів", () => page);
                await page.SetValue();
            });

            CreateLink(vLeft, "Замовлення постачальникам", async () =>
            {
                Звіт_ЗамовленняПостачальникам page = new Звіт_ЗамовленняПостачальникам();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Звіт - Замовлення постачальникам", () => page);
                await page.SetValue();
            });

            CreateLink(vLeft, "Закупівлі", async () =>
            {
                Звіт_Закупівлі page = new Звіт_Закупівлі();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Звіт - Закупівлі", () => page);
                await page.SetValue();
            });

            CreateLink(vLeft, "Продажі", async () =>
            {
                Звіт_Продажі page = new Звіт_Продажі();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Звіт - Продажі", () => page);
                await page.SetValue();
            });

            CreateLink(vLeft, "Товари в комірках на складах", async () =>
            {
                Звіт_ТовариВКоміркахНаСкладах page = new Звіт_ТовариВКоміркахНаСкладах();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Звіт - Товари в комірках на складах", () => page);
                await page.SetValue();
            });

            PackStart(hBoxList, false, false, 10);
            ShowAll();
        }
    }
}