/*

Обробки

*/

using Gtk;
using InterfaceGtk;

namespace StorageAndTrade
{
    class PageWorking : Форма
    {
        public PageWorking() : base()
        {
            Box vBox = new Box(Orientation.Vertical, 0);
            PackStart(vBox, false, false, 5);

            CreateLink(vBox, "Заповнення початковими даними", () =>
            {
                Обробка_ПочатковеЗаповнення page = new Обробка_ПочатковеЗаповнення();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Заповнення початковими даними", () => page);
            });

            CreateLink(vBox, "Завантаження курсів валют НБУ", () =>
            {
                Обробка_ЗавантаженняКурсівВалют page = new Обробка_ЗавантаженняКурсівВалют();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Завантаження курсів валют НБУ", () => page);
            });

            CreateLink(vBox, "Завантаження списку банків", () =>
            {
                Обробка_ЗавантаженняБанків page = new Обробка_ЗавантаженняБанків();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Завантаження списку банків", () => page);
            });
        }

        public void SetValue()
        {

        }
    }
}