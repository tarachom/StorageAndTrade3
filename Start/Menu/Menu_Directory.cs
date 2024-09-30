/*

Довідники

*/

using Gtk;
using InterfaceGtk;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Menu_Directory : Форма
    {
        public Menu_Directory() : base()
        {
            //Список
            Box hBoxList = new Box(Orientation.Horizontal, 0);
            PackStart(hBoxList, false, false, 10);

            Box vLeft = new Box(Orientation.Vertical, 0);
            hBoxList.PackStart(vLeft, false, false, 5);

            CreateLink(vLeft, Контрагенти_Const.FULLNAME, async () =>
            {
                Контрагенти page = new Контрагенти();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, Контрагенти_Const.FULLNAME, () => page);
                await page.SetValue();
            });

            CreateLink(vLeft, Номенклатура_Const.FULLNAME, async () =>
            {
                Номенклатура page = new Номенклатура();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, Номенклатура_Const.FULLNAME, () => page);
                await page.SetValue();
            });

            CreateLink(vLeft, Склади_Const.FULLNAME, async () =>
            {
                Склади page = new Склади();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, Склади_Const.FULLNAME, () => page);
                await page.SetValue();
            });

            CreateLink(vLeft, Валюти_Const.FULLNAME, async () =>
            {
                Валюти page = new Валюти();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, Валюти_Const.FULLNAME, () => page);
                await page.SetValue();
            });

            CreateLink(vLeft, Каси_Const.FULLNAME, async () =>
            {
                Каси page = new Каси();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, Каси_Const.FULLNAME, () => page);
                await page.SetValue();
            });

            CreateLink(vLeft, Організації_Const.FULLNAME, async () =>
            {
                Організації page = new Організації();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, Організації_Const.FULLNAME, () => page);
                await page.SetValue();
            });

            CreateLink(vLeft, Блокнот_Const.FULLNAME, async () =>
            {
                Блокнот page = new Блокнот();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, Блокнот_Const.FULLNAME, () => page);
                await page.SetValue();
            });

            ShowAll();
        }
    }
}