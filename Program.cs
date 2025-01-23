
using Gtk;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class Program
    {
        #region Global Var

        /// <summary>
        /// Основна форма
        /// </summary>
        public static FormStorageAndTrade? GeneralForm { get; set; }

        /// <summary>
        /// Основний блокнот
        /// </summary>
        public static Notebook? GeneralNotebook { get; set; }

        /// <summary>
        /// Авторизований користувач
        /// </summary>
        public static Користувачі_Pointer Користувач { get; set; } = new Користувачі_Pointer();

        #endregion

        public static void Main()
        {
            Application.Init();
            _ = new FormConfigurationSelection();

            string styleDefaultFile = System.IO.Path.Combine(AppContext.BaseDirectory, "Default.css");
            if (File.Exists(styleDefaultFile))
            {
                CssProvider provider = new CssProvider();
                provider.LoadFromPath(styleDefaultFile);
                StyleContext.AddProviderForScreen(Gdk.Screen.Default, provider, 800);
            }

            Application.Run();
        }
    }
}