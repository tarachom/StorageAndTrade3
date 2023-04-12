/*
Copyright (C) 2019-2023 TARAKHOMYN YURIY IVANOVYCH
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

using Gtk;

using AccountingSoftware;
using Configurator;

using Конфа = StorageAndTrade_1_0;
using Константи = StorageAndTrade_1_0.Константи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    /// <summary>
    /// Переоприділення форми вибору бази з Конфігуратора
    /// </summary>
    class FormConfigurationSelection : Configurator.FormConfigurationSelection
    {
        /// <summary>
        /// Тип форми - робоча програма.
        /// Добавляється кнопка "Відкрити" і стає активною процедура Open()
        /// </summary>
        public override TypeForm TypeOpenForm { get; } = TypeForm.WorkingProgram;

        /// <summary>
        /// Переоприділення процедури Open() для кнопки "Відкрити"
        /// </summary>
        public override void Open()
        {
            ListBoxRow[] selectedRows = listBox.SelectedRows;

            if (selectedRows.Length != 0)
            {
                ConfigurationParam? OpenConfigurationParam = ConfigurationParamCollection.GetConfigurationParam(selectedRows[0].Name);

                if (OpenConfigurationParam == null)
                    return;

                ConfigurationParamCollection.SelectConfigurationParam(selectedRows[0].Name);
                ConfigurationParamCollection.SaveConfigurationParamFromXML(ConfigurationParamCollection.PathToXML);

                string PathToConfXML = System.IO.Path.Combine(AppContext.BaseDirectory, "Confa.xml");

                Exception exception;

                Конфа.Config.Kernel = new Kernel();

                //Підключення до бази даних та завантаження конфігурації
                bool flagOpen = Конфа.Config.Kernel.Open(
                    PathToConfXML,
                    OpenConfigurationParam.DataBaseServer,
                    OpenConfigurationParam.DataBaseLogin,
                    OpenConfigurationParam.DataBasePassword,
                    OpenConfigurationParam.DataBasePort,
                    OpenConfigurationParam.DataBaseBaseName, out exception);

                if (!flagOpen)
                {
                    Message.Error(this, "Error: " + exception.Message);
                    return;
                }

                //
                // Авторизація
                //

                ResponseType ModalResult = ResponseType.None;

                using (FormLogIn windowFormLogIn = new FormLogIn())
                {
                    windowFormLogIn.TransientFor = this;
                    windowFormLogIn.Modal = true;
                    windowFormLogIn.Resizable = false;
                    windowFormLogIn.SetValue();
                    windowFormLogIn.Show();

                    while (ModalResult == ResponseType.None)
                    {
                        ModalResult = windowFormLogIn.ModalResult;
                        Application.RunIteration(true);
                    }
                }

                if (ModalResult == ResponseType.Cancel)
                {
                    Конфа.Config.Kernel.Close();
                    return;
                }

                if (Конфа.Config.Kernel.DataBase.IfExistsTable("tab_constants"))
                {
                    Конфа.Config.ReadAllConstants();

                    //Значення констант за замовчуванням
                    if ((int)Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const == 0)
                        Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const = Перелічення.ТипПеріодуДляЖурналівДокументів.ВесьПеріод;

                    Program.GeneralForm = new FormStorageAndTrade();
                    Program.GeneralForm.OpenConfigurationParam = ConfigurationParamCollection.GetConfigurationParam(selectedRows[0].Name);
                    Program.GeneralForm.Show();

                    //Присвоєння користувача
                    Program.GeneralForm.SetCurrentUser();

                    //Запуск фонових задач
                    Program.GeneralForm.StartBackgroundTask();

                    //Відкрити перші сторінки
                    Program.GeneralForm.OpenFirstPages();

                    //Сховати форму вибору
                    this.Hide();
                }
                else
                {
                    Message.Error(this, @"Error: Відсутня таблиця tab_constants. Потрібно відкрити Конфігуратор і зберегти конфігурацію -  
                    (Меню: Конфігурація/Зберегти конфігурацію - дальше Збереження змін. Крок 1, Збереження змін. Крок 2)");
                    
                    return;
                }
            }
        }
    }
}