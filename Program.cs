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

using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Program
    {
        /// <summary>
        /// Список токенів для управління потоками
        /// При завершенні програми, всі потоки повинні також завершити свою роботу
        /// </summary>
        public static List<CancellationTokenSource> ListCancellationToken = new List<CancellationTokenSource>();

        /// <summary>
        /// Авторизований користувач
        /// </summary>
        public static Користувачі_Pointer Користувач { get; set; } = new Користувачі_Pointer();

        public static void Main()
        {
            Application.Init();
            new FormConfigurationSelection();
            Application.Run();
        }

        /// <summary>
        /// Завершення програми
        /// </summary>
        public static void Quit()
        {
            foreach (CancellationTokenSource cancellationTokenItem in ListCancellationToken)
            {
                try
                {
                    cancellationTokenItem.Cancel();
                }
                catch { }
            }

            Application.Quit();
        }

        /// <summary>
        /// Основна форма
        /// </summary>
        public static FormStorageAndTrade? GeneralForm { get; set; }

        #region Const

        //Шлях до іконки
        public static readonly string IcoFileName = AppContext.BaseDirectory +"images/form.ico";

        #endregion
    }
}