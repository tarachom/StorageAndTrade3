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

Журнали

*/

using Gtk;

namespace StorageAndTrade
{
    class PageJournals : VBox
    {
        public PageJournals() : base()
        {
            //Кнопки
            HBox hBoxBotton = new HBox();

            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

            hBoxBotton.PackStart(bClose, false, false, 10);

            PackStart(hBoxBotton, false, false, 10);

            //Список
            HBox hBoxList = new HBox(false, 0);

            VBox vLeft = new VBox(false, 0);
            hBoxList.PackStart(vLeft, false, false, 5);

            AddLink(vLeft, "Повний", Повний);
            AddLink(vLeft, "Продажі", Продажі);
            AddLink(vLeft, "Закупки", Закупки);
            AddLink(vLeft, "Склади", Склади);
            AddLink(vLeft, "Фінанси", Фінанси);

            PackStart(hBoxList, false, false, 10);

            ShowAll();
        }

        void Повний(object? sender, EventArgs args)
        {

        }

        void Продажі(object? sender, EventArgs args)
        {

        }

        void Закупки(object? sender, EventArgs args)
        {

        }

        void Склади(object? sender, EventArgs args)
        {

        }

        void Фінанси(object? sender, EventArgs args)
        {

        }

        void AddLink(VBox vbox, string uri, EventHandler? clickAction = null)
        {
            LinkButton lb = new LinkButton(uri, " " + uri) { Halign = Align.Start, Image = new Image("images/doc.png"), AlwaysShowImage = true };
            vbox.PackStart(lb, false, false, 0);

            if (clickAction != null)
                lb.Clicked += clickAction;
        }
    }
}