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

/*

Діалогове повідомлення

*/

using Gtk;

namespace StorageAndTrade
{
    class Message
    {
        public static void Info(Window? pwin, string message)
        {
            MessageDialog md = new MessageDialog(pwin, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Ok, message) { WindowPosition = WindowPosition.Center };
            md.Run();
            md.Dispose();
            md.Destroy();
        }

        public static void Error(Window? pwin, string message)
        {
            MessageDialog md = new MessageDialog(pwin, DialogFlags.DestroyWithParent, MessageType.Warning, ButtonsType.Close, message) { WindowPosition = WindowPosition.Center };
            md.Run();
            md.Dispose();
            md.Destroy();
        }

        /// <summary>
        /// Повідомлення запит Так/Ні
        /// </summary>
        /// <param name="pwin">Вікно власник</param>
        /// <param name="message">Текст</param>
        /// <returns>Так або Ні</returns>
        public static ResponseType Request(Window? pwin, string message)
        {
            MessageDialog md = new MessageDialog(pwin, DialogFlags.DestroyWithParent, MessageType.Question, ButtonsType.YesNo, message);
            ResponseType response = (ResponseType)md.Run();
            md.Dispose();
            md.Destroy();

            return response;
        }
    }
}