
/*
        FormConfigurationSelection.cs
*/

using InterfaceGtk;
using AccountingSoftware;

using GeneratedCode;
using GeneratedCode.Константи;

namespace StorageAndTrade
{
    /// <summary>
    /// Переоприділення форми вибору бази
    /// </summary>
    class FormConfigurationSelection : InterfaceGtk.FormConfigurationSelection
    {
        public FormConfigurationSelection() : base(Config.Kernel, Configurator.Program.Kernel, TypeForm.WorkingProgram) { }

        public override async ValueTask<bool> OpenProgram(ConfigurationParam? openConfigurationParam)
        {
            if (await Config.Kernel.DataBase.IfExistsTable(SpecialTables.Constants))
            {
                //Запуск фонових задач
                Config.StartBackgroundTask();

                //Значення констант за замовчуванням
                if (string.IsNullOrEmpty(ЖурналиДокументів.ОсновнийТипПеріоду_Const))
                    ЖурналиДокументів.ОсновнийТипПеріоду_Const = ПеріодДляЖурналу.ТипПеріоду.ВесьПеріод.ToString();

                Program.GeneralForm = new FormStorageAndTrade() { OpenConfigurationParam = openConfigurationParam };
                Program.GeneralNotebook = Program.GeneralForm.Notebook;
                Program.GeneralForm.Show();

                //Вивід інформації в нижній StatusBar
                Program.GeneralForm.SetStatusBar();

                //Присвоєння користувача
                Program.GeneralForm.SetCurrentUser();

                //Відкрити перші сторінки
                Program.GeneralForm.OpenFirstPages();

                return true;
            }
            else
            {
                Message.Error(this, @"Error: Відсутня таблиця tab_constants. Потрібно відкрити Конфігуратор і зберегти конфігурацію -  
                    (Меню: Конфігурація/Зберегти конфігурацію - дальше Збереження змін. Крок 1, Збереження змін. Крок 2)");

                return false;
            }
        }

        public override async ValueTask<bool> OpenConfigurator(ConfigurationParam? openConfigurationParam)
        {
            Configurator.FormConfigurator сonfigurator = new() { OpenConfigurationParam = openConfigurationParam };
            сonfigurator.Show();

            сonfigurator.SetValue();
            сonfigurator.LoadTreeAsync();

            return await ValueTask.FromResult(true);
        }
    }
}