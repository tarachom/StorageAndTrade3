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
    /// <summary>
    /// Для побудови основних меню документів, довідників, журналів, звітів ...
    /// </summary>
    class Link
    {
        public static void AddCaption(Box vBox, string name, EventHandler? clickAction = null)
        {
            if (clickAction != null)
            {
                LinkButton lb = new LinkButton(name, " " + name);
                vBox.PackStart(lb, false, false, 5);

                lb.Clicked += clickAction;
            }
            else
            {
                Label caption = new Label(name);
                vBox.PackStart(caption, false, false, 5);
            }
        }

        public static void AddSeparator(Box hbox)
        {
            Separator separator = new Separator(Orientation.Horizontal);
            hbox.PackStart(separator, false, false, 5);
        }

        public static void AddLink(Box vbox, string uri, System.Action? clickAction = null)
        {
            LinkButton lb = new LinkButton(uri, " " + uri) { Halign = Align.Start, Image = new Image($"{AppContext.BaseDirectory}images/doc.png"), AlwaysShowImage = true };
            vbox.PackStart(lb, false, false, 0);

            if (clickAction != null)
                lb.Clicked += (object? sender, EventArgs args) =>
                {
                    clickAction.Invoke();
                };
        }

        // public static void AddLink(Box hbox, string uri, System.Action? clickAction = null)
        // {
        //     LinkButton lb = new LinkButton(uri, " " + uri) { Halign = Align.Start, Image = new Image($"{AppContext.BaseDirectory}images/doc.png"), AlwaysShowImage = true };
        //     hbox.PackStart(lb, false, false, 0);

        //     if (clickAction != null)
        //         lb.Clicked += (object? sender, EventArgs args) =>
        //         {
        //             clickAction.Invoke();
        //         };
        // }
    }
}