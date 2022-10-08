using Gtk;

namespace StorageAndTrade
{
    class Program
    {
        public static void Main()
        {
            Application.Init();
            new FormConfigurationSelection();
            Application.Run();
        }
    }
}