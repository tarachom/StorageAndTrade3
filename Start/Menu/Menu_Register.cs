/*

Регістри відомостей

*/

using Gtk;
using InterfaceGtk;

namespace StorageAndTrade
{
    class Menu_Register : Форма
    {
        public Menu_Register() : base()
        {
            //Список
            Box hBoxList = new Box(Orientation.Horizontal, 0);
            PackStart(hBoxList, false, false, 10);

            Box vLeft = new Box(Orientation.Vertical, 0);
            hBoxList.PackStart(vLeft, false, false, 5);

            

            ShowAll();
        }
    }
}