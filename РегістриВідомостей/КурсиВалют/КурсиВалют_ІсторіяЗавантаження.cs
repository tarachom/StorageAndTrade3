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

using Gtk;

namespace StorageAndTrade
{
    class КурсиВалют_ІсторіяЗавантаження : Box
    {
        Box vBoxMessage = new Box(Orientation.Vertical, 0);

        public КурсиВалют_ІсторіяЗавантаження() : base(Orientation.Vertical, 0)
        {
            //Кнопки
            Box hBoxTop = new Box(Orientation.Horizontal, 0);

            Button bClear = new Button("Очистити");
            bClear.Clicked += OnClear;

            hBoxTop.PackStart(bClear, false, false, 10);

            PackStart(hBoxTop, false, false, 10);

            ScrolledWindow scroll = new ScrolledWindow();
            scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scroll.Add(vBoxMessage);

            PackStart(scroll, true, true, 0);

            ShowAll();
        }

        public async void LoadRecords()
        {
            foreach (Widget Child in vBoxMessage.Children)
                vBoxMessage.Remove(Child);

            var recordResult = await ФункціїДляФоновихЗавдань.ОтриматиЗаписиЗІсторіїЗавантаженняКурсуВалют();

            foreach (Dictionary<string, object> row in recordResult.ListRow)
                CreateMessage(row);

            vBoxMessage.ShowAll();
        }

        void CreateMessage(Dictionary<string, object> row)
        {
            //Дата
            Box hBoxDate = new Box(Orientation.Horizontal, 0);

            hBoxDate.PackStart(new Label(
                row["Дата"].ToString() + "\t" + "[ " + row["Стан"].ToString() + " ]\t" +
                row["Посилання"].ToString() + "\t" +
                row["Повідомлення"].ToString()), false, false, 5);

            vBoxMessage.PackStart(hBoxDate, false, false, 5);
            vBoxMessage.PackStart(new Separator(Orientation.Horizontal), false, false, 5);
        }

        async void OnClear(object? sender, EventArgs args)
        {
            await ФункціїДляФоновихЗавдань.ОчиститиІсторіюЗавантаженняКурсуВалют(true);
            LoadRecords();
        }

    }
}