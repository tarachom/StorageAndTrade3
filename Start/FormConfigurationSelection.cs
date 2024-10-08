/*
Copyright (C) 2019-2024 TARAKHOMYN YURIY IVANOVYCH
All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

/*
Автор:    Тарахомин Юрій Іванович
Адреса:   Україна, м. Львів
Сайт:     accounting.org.ua
*/

using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0;
using Константи = StorageAndTrade_1_0.Константи;

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
                if (string.IsNullOrEmpty(Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const))
                    Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const = ПеріодДляЖурналу.ТипПеріоду.ВесьПеріод.ToString();

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