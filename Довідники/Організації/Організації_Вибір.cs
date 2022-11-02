using Gtk;

using AccountingSoftware;

namespace StorageAndTrade
{
    class Організації_Вибір : Window
    {
        public Організації_Вибір() : base("Організації")
        {
            SetDefaultSize(1000, 600);
            SetPosition(WindowPosition.Center);
            SetDefaultIconFromFile("form.ico");

            DeleteEvent += delegate { this.Close(); };

            Організації page = new Організації();
            page.LoadRecords();
            Add(page);

            ShowAll();
        }
    }
}