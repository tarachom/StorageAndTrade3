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

Функції для журналів

*/
using Gtk;



using AccountingSoftware;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class ФункціїДляЖурналів
    {
        public static void ВідкритиСписокДокументів(Widget relative_to, Dictionary<string, string> allowDocument, ТипПеріодуДляЖурналівДокументів periodWhere = 0)
        {
            VBox vBox = new VBox();

            foreach (KeyValuePair<string, string> typeDoc in allowDocument)
            {
                LinkButton lb = new LinkButton(typeDoc.Value, " " + typeDoc.Value) { Halign = Align.Start };
                vBox.PackStart(lb, false, false, 0);

                lb.Clicked += (object? sender, EventArgs args) =>
                {
                    ФункціїДляДокументів.ВідкритиДокументВідповідноДоВиду(typeDoc.Key, new UnigueID(), periodWhere);
                };
            }

            Popover PopoverSelect = new Popover(relative_to) { Position = PositionType.Bottom, BorderWidth = 2 };

            PopoverSelect.Add(vBox);
            PopoverSelect.ShowAll();
        }
    }
}
