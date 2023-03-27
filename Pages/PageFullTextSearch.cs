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
using AccountingSoftware;
using StorageAndTrade_1_0;

namespace StorageAndTrade
{
    class PageFullTextSearch : VBox
    {
        VBox vBoxMessage = new VBox();

        public PageFullTextSearch() : base()
        {
            //Кнопки
            HBox hBoxTop = new HBox();
            PackStart(hBoxTop, false, false, 10);

            Button bClear = new Button("Очистити");
            //bClear.Clicked += OnClear;

            hBoxTop.PackStart(bClear, false, false, 10);

            ScrolledWindow scroll = new ScrolledWindow() { ShadowType = ShadowType.In };
            scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scroll.Add(vBoxMessage);

            PackStart(scroll, true, true, 0);

            ShowAll();
        }

        public void Find(string findtext)
        {
            foreach (Widget Child in vBoxMessage.Children)
                vBoxMessage.Remove(Child);

            List<Dictionary<string, object>>? listRow = Config.Kernel!.DataBase.SpetialTableFullTextSearchSelect(findtext);

            if (listRow != null)
                foreach (Dictionary<string, object> row in listRow)
                    CreateMessage(row);

            vBoxMessage.ShowAll();
        }

        void CreateMessage(Dictionary<string, object> row)
        {
            HBox hBoxRow = new HBox();

            hBoxRow.PackStart(new Label(row["value"].ToString()) { UseMarkup = true }, false, false, 5);

            vBoxMessage.PackStart(hBoxRow, false, false, 5);
        }
    }
}