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

namespace StorageAndTrade
{
    class СпільніФорми_ВивідПовідомленняПроПомилки : VBox
    {
        VBox vBoxMessage = new VBox();

        public СпільніФорми_ВивідПовідомленняПроПомилки() : base()
        {
            //Кнопки
            HBox hBoxTop = new HBox();
            PackStart(hBoxTop, false, false, 10);

            Button bClear = new Button("Очистити");
            bClear.Clicked += OnClear;

            hBoxTop.PackStart(bClear, false, false, 10);

            ScrolledWindow scroll = new ScrolledWindow() { ShadowType = ShadowType.In };
            scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scroll.Add(vBoxMessage);

            PackStart(scroll, true, true, 0);

            ShowAll();
        }

        public void LoadRecords()
        {
            foreach (Widget Child in vBoxMessage.Children)
                vBoxMessage.Remove(Child);

            List<Dictionary<string, object>> listRow = ФункціїДляПовідомлень.ПрочитатиПовідомленняПроПомилку();

            foreach (Dictionary<string, object> row in listRow)
                CreateMessage(row);
        }

        void CreateMessage(Dictionary<string, object> row)
        {
            //Дата
            HBox hBoxDate = new HBox();

            hBoxDate.PackStart(new Label(
                row["Дата"].ToString() + "\t" + "[ " + row["НазваПроцесу"].ToString() + " ]\t" + row["НазваОбєкту"].ToString()), false, false, 5);

            vBoxMessage.PackStart(hBoxDate, false, false, 5);

            HBox hBoxInfo = new HBox();
            vBoxMessage.PackStart(hBoxInfo, false, false, 10);

            hBoxInfo.PackStart(new Image(AppContext.BaseDirectory + "images/error.png"), false, false, 25);

            VBox vBoxInfo = new VBox();
            hBoxInfo.PackStart(vBoxInfo, false, false, 10);

            //Повідомлення
            HBox hBoxMessage = new HBox();
            hBoxMessage.PackStart(new Label("-> " + row["Повідомлення"].ToString()) { Wrap = true }, false, false, 5);
            vBoxInfo.PackStart(hBoxMessage, false, false, 5);

            vBoxMessage.PackStart(new Separator(Orientation.Horizontal), false, false, 5);
        }

        void OnClear(object? sender, EventArgs args)
        {
            ФункціїДляПовідомлень.ОчиститиПовідомлення();
            LoadRecords();
        }

    }
}