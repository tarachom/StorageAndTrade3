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

Стартова сторінка.

*/

using Gtk;

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;

namespace StorageAndTrade
{
    class PageHome : VBox
    {
        public БлокДляСторінки_КурсиВалют БлокКурсиВалют = new БлокДляСторінки_КурсиВалют() { WidthRequest = 500 };
        public БлокДляСторінки_АктивніКористувачі АктивніКористувачі = new БлокДляСторінки_АктивніКористувачі() { WidthRequest = 500 };

        public PageHome() : base()
        {
            VBox vBox = new VBox();

            {
                HBox hBox = new HBox();
                hBox.PackStart(БлокКурсиВалют, false, false, 5);

                vBox.PackStart(hBox, false, false, 5);
            }

            {
                HBox hBox = new HBox();
                hBox.PackStart(АктивніКористувачі, false, false, 5);
                vBox.PackStart(hBox, false, false, 5);
            }

            PackStart(vBox, false, false, 5);

            ShowAll();
        }
    }
}