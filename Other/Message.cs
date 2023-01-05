#region Info

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

#endregion

using Gtk;

namespace StorageAndTrade
{
    class Message
    {
        public static void Info(Window? pwin, string message)
        {
            MessageDialog md = new MessageDialog(pwin, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Ok, message);
            md.WindowPosition = WindowPosition.Center;
            md.Run();
            md.Destroy();
        }

        public static void Error(Window? pwin, string message)
        {
            MessageDialog md = new MessageDialog(pwin, DialogFlags.DestroyWithParent, MessageType.Warning, ButtonsType.Close, message);
            md.WindowPosition = WindowPosition.Center;
            md.Run();
            md.Destroy();
        }

        public static ResponseType Request(Window? pwin, string message)
        {
            MessageDialog md = new MessageDialog(pwin, DialogFlags.DestroyWithParent, MessageType.Question, ButtonsType.YesNo, message);
            ResponseType response = (ResponseType)md.Run();
            md.Destroy();

            return response;
        }
    }
}