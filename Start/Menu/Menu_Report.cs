/*

Звіти

*/

using Gtk;
using InterfaceGtk;

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

            CreateLink(vLeft, "Товари на складах", async () =>
            {
                Звіт_ТовариНаСкладах page = new Звіт_ТовариНаСкладах();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Звіт - Товари на складах", () => page);
                await page.SetValue();
            });

            CreateLink(vLeft, "Вільні залишки", () =>
            {
                Звіт_ВільніЗалишки page = new Звіт_ВільніЗалишки();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Звіт - Вільні залишки", () => page);
            });

            CreateLink(vLeft, "Партії товарів", () =>
            {
                Звіт_ПартіїТоварів page = new Звіт_ПартіїТоварів();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Звіт - Партії товарів", () => page);
            });

            CreateLink(vLeft, "Рух коштів", () =>
            {
                Звіт_РухКоштів page = new Звіт_РухКоштів();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Звіт - Рух коштів", () => page);
            });

            CreateLink(vLeft, "Розрахунки з контрагентами", () =>
            {
                Звіт_РозрахункиЗКонтрагентами page = new Звіт_РозрахункиЗКонтрагентами();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Звіт - Розрахунки з контрагентами", () => page);
            });

            CreateLink(vLeft, "Розрахунки з клієнтами", () =>
            {
                Звіт_РозрахункиЗКлієнтами page = new Звіт_РозрахункиЗКлієнтами();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Звіт - Розрахунки з клієнтами", () => page);
            });

            CreateLink(vLeft, "Розрахунки з постачальниками", () =>
            {
                Звіт_РозрахункиЗПостачальниками page = new Звіт_РозрахункиЗПостачальниками();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Звіт - Розрахунки з постачальниками", () => page);
            });

            CreateLink(vLeft, "Замовлення клієнтів", () =>
            {
                Звіт_ЗамовленняКлієнтів page = new Звіт_ЗамовленняКлієнтів();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Звіт - Замовлення клієнтів", () => page);
            });

            CreateLink(vLeft, "Замовлення постачальникам", () =>
            {
                Звіт_ЗамовленняПостачальникам page = new Звіт_ЗамовленняПостачальникам();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Звіт - Замовлення постачальникам", () => page);
            });

            CreateLink(vLeft, "Закупівлі", () =>
            {
                Звіт_Закупівлі page = new Звіт_Закупівлі();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Звіт - Закупівлі", () => page);
            });

            CreateLink(vLeft, "Продажі", () =>
            {
                Звіт_Продажі page = new Звіт_Продажі();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Звіт - Продажі", () => page);
            });

            CreateLink(vLeft, "Товари в комірках на складах", () =>
            {
                Звіт_ТовариВКоміркахНаСкладах page = new Звіт_ТовариВКоміркахНаСкладах();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Звіт - Товари в комірках на складах", () => page);
            });

            PackStart(hBoxList, false, false, 10);
            ShowAll();
        }
    }
}