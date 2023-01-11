using Gtk;

namespace StorageAndTrade
{
    class Program
    {
        public static CancellationTokenSource? CancellationTokenBackgroundTask { get; set; }

        public static void Main()
        {
            Application.Init();
            new FormConfigurationSelection();
            Application.Run();
        }

        public static void Quit()
        {
            if (CancellationTokenBackgroundTask != null)
                CancellationTokenBackgroundTask.Cancel();

            Application.Quit();
        }

        //Основна форма
        public static FormStorageAndTrade? GeneralForm { get; set; }

        #region Const

        //Шлях до іконки
        public const string IcoFileName = "images/form.ico";

        #endregion
    }
}