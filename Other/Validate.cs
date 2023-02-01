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

/*

Перевірка правельності числових значеннь

*/

namespace StorageAndTrade
{
    class Validate
    {
        /// <summary>
        /// Чи це ціле число
        /// </summary>
        /// <param name="text">Значення</param>
        /// <returns></returns>
        public static (bool, int) IsInt(string text)
        {
            int value;
            return (int.TryParse(text, out value), value);
        }

        /// <summary>
        /// Чи це число з комою
        /// </summary>
        /// <param name="text">Значення</param>
        /// <returns></returns>
        public static (bool, decimal) IsDecimal(string text)
        {
            // Треба протестувати чи варто заміняти крапку на кому на інших мовах операційної системи
            if (text.IndexOf(".") != -1)
                text = text.Replace(".", ",");

            decimal value;
            return (decimal.TryParse(text, out value), value);
        }
    }
}