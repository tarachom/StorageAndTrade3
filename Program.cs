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

using Gtk;

using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Program
    {
        #region Const

        //Шлях до іконки
        public static readonly string IcoFileName = AppContext.BaseDirectory + "images/form.ico"; //????

        #endregion

        #region Global Var

        /// <summary>
        /// Основна форма
        /// </summary>
        public static FormStorageAndTrade? GeneralForm { get; set; }

        /// <summary>
        /// Авторизований користувач
        /// </summary>
        public static Користувачі_Pointer Користувач { get; set; } = new Користувачі_Pointer();

        #endregion

        public static void Main()
        {
            Application.Init();
            _ = new FormConfigurationSelection();
            Application.Run();
        }

        /// <summary>
        /// Завершення програми
        /// </summary>
        public static void Quit()
        {
            Application.Quit();
        }
    }
}