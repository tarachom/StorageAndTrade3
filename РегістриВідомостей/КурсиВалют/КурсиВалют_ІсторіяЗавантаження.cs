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
    class КурсиВалют_ІсторіяЗавантаження : VBox
    {
        VBox vBoxMessage = new VBox();

        public КурсиВалют_ІсторіяЗавантаження() : base()
        {
            //Кнопки
            HBox hBoxTop = new HBox();

            Button bClear = new Button("Очистити");
            bClear.Clicked += OnClear;

            hBoxTop.PackStart(bClear, false, false, 10);

            PackStart(hBoxTop, false, false, 10);

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

            List<Dictionary<string, object>> listRow = ФункціїДляФоновихЗавдань.ОтриматиЗаписиЗІсторіїЗавантаженняКурсуВалют();

            foreach (Dictionary<string, object> row in listRow)
                CreateMessage(row);
        }

        void CreateMessage(Dictionary<string, object> row)
        {
            //Дата
            HBox hBoxDate = new HBox();

            hBoxDate.PackStart(new Label(
                row["Дата"].ToString() + "\t" + "[ " + row["Стан"].ToString() + " ]\t" +
                row["Посилання"].ToString() + "\t" +
                row["Повідомлення"].ToString()), false, false, 5);

            vBoxMessage.PackStart(hBoxDate, false, false, 5);
            vBoxMessage.PackStart(new Separator(Orientation.Horizontal), false, false, 5);
        }

        void OnClear(object? sender, EventArgs args)
        {
            ФункціїДляФоновихЗавдань.ОчиститиІсторіюЗавантаженняКурсуВалют(true);
            LoadRecords();
        }

    }
}