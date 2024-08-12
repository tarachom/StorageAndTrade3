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
using AccountingSoftware;

namespace StorageAndTrade
{
    class СпільніФорми_ВивідПовідомленняПроПомилки : Box
    {
        Box vBoxMessage = new Box(Orientation.Vertical, 0);

        public СпільніФорми_ВивідПовідомленняПроПомилки() : base(Orientation.Vertical, 0)
        {
            //Кнопки
            Box hBoxTop = new Box(Orientation.Horizontal, 0);
            PackStart(hBoxTop, false, false, 10);

            Button bClear = new Button("Очистити");
            bClear.Clicked += OnClear;
            hBoxTop.PackStart(bClear, false, false, 10);

            Button bReload = new Button("Перечитати");
            bReload.Clicked += OnReload;
            hBoxTop.PackStart(bReload, false, false, 10);

            ScrolledWindow scroll = new ScrolledWindow() { ShadowType = ShadowType.In };
            scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scroll.Add(vBoxMessage);

            PackStart(scroll, true, true, 0);

            ShowAll();
        }

        public async ValueTask LoadRecords()
        {
            foreach (Widget Child in vBoxMessage.Children)
                vBoxMessage.Remove(Child);

            SelectRequest_Record record = await ФункціїДляПовідомлень.ПрочитатиПовідомленняПроПомилки();

            foreach (Dictionary<string, object> row in record.ListRow)
                CreateMessage(row);

            vBoxMessage.ShowAll();
        }

        void CreateMessage(Dictionary<string, object> row)
        {
            Box vBoxInfo = new Box(Orientation.Vertical, 0);

            //Image
            {
                Box hBox = new Box(Orientation.Horizontal, 0);
                hBox.PackStart(new Image(AppContext.BaseDirectory + "images/error.png"), false, false, 25);
                hBox.PackStart(vBoxInfo, false, false, 10);
                vBoxMessage.PackStart(hBox, false, false, 10);
            }

            //Перший рядок
            {
                Box hBox = new Box(Orientation.Horizontal, 0);
                Label line = new Label("<i>" + row["Дата"].ToString() + " " + row["НазваПроцесу"].ToString() + "</i>")
                {
                    UseMarkup = true
                };

                hBox.PackStart(line, false, false, 5);
                vBoxInfo.PackStart(hBox, false, false, 5);
            }

            //Другий рядок
            {
                Box hBox = new Box(Orientation.Horizontal, 0);
                Label line = new Label("<b>" + row["НазваОбєкту"].ToString() + "</b>")
                {
                    UseMarkup = true
                };

                hBox.PackStart(line, false, false, 5);
                vBoxInfo.PackStart(hBox, false, false, 5);
            }

            //Повідомлення
            {
                Box hBox = new Box(Orientation.Horizontal, 0);
                hBox.PackStart(new Label("-> " + row["Повідомлення"].ToString()) { Wrap = true }, false, false, 5);
                vBoxInfo.PackStart(hBox, false, false, 5);
            }

            //Для відкриття
            {
                CompositePointerControl Обєкт = new CompositePointerControl
                {
                    Pointer = new UuidAndText(new UnigueID(row["Обєкт"]).UGuid, row["ТипОбєкту"].ToString() ?? ""),
                    Caption = ""
                };

                Box hBoxObject = new Box(Orientation.Horizontal, 0);
                hBoxObject.PackStart(Обєкт, false, false, 0);
                vBoxInfo.PackStart(hBoxObject, false, false, 0);
            }

            vBoxMessage.PackStart(new Separator(Orientation.Horizontal), false, false, 5);
        }

        async void OnClear(object? sender, EventArgs args)
        {
            await ФункціїДляПовідомлень.ОчиститиВсіПовідомлення();
            await LoadRecords();
        }

        async void OnReload(object? sender, EventArgs args)
        {
            await LoadRecords();
        }
    }

    class СпільніФорми_ВивідПовідомленняПроПомилки_ШвидкийВивід : Box
    {
        Box vBoxMessage = new Box(Orientation.Vertical, 0);

        public СпільніФорми_ВивідПовідомленняПроПомилки_ШвидкийВивід(int width = 800, int height = 400) : base(Orientation.Vertical, 0)
        {
            ScrolledWindow scroll = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = width, HeightRequest = height };
            scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scroll.Add(vBoxMessage);

            PackStart(scroll, true, true, 0);
            ShowAll();
        }

        public async ValueTask LoadRecords(UnigueID? ВідбірПоОбєкту = null, int? limit = null)
        {
            SelectRequest_Record record = await ФункціїДляПовідомлень.ПрочитатиПовідомленняПроПомилки(ВідбірПоОбєкту, limit);

            foreach (Dictionary<string, object> row in record.ListRow)
                CreateMessage(row);

            vBoxMessage.ShowAll();
        }

        void CreateMessage(Dictionary<string, object> row)
        {
            Box vBoxInfo = new Box(Orientation.Vertical, 0);

            //Image
            {
                Box hBox = new Box(Orientation.Horizontal, 0);
                hBox.PackStart(new Image(AppContext.BaseDirectory + "images/error.png"), false, false, 25);
                hBox.PackStart(vBoxInfo, false, false, 10);
                vBoxMessage.PackStart(hBox, false, false, 10);
            }

            //Перший рядок
            {
                Box hBox = new Box(Orientation.Horizontal, 0);
                Label line = new Label("<i>" + row["Час"].ToString() + " " + row["НазваПроцесу"].ToString() + "</i>")
                {
                    UseMarkup = true
                };

                hBox.PackStart(line, false, false, 5);
                vBoxInfo.PackStart(hBox, false, false, 5);
            }

            //Другий рядок
            {
                Box hBox = new Box(Orientation.Horizontal, 0);
                Label line = new Label("<b>" + row["НазваОбєкту"].ToString() + "</b>")
                {
                    UseMarkup = true
                };

                hBox.PackStart(line, false, false, 5);
                vBoxInfo.PackStart(hBox, false, false, 5);
            }

            //Повідомлення
            {
                Box hBox = new Box(Orientation.Horizontal, 0);
                hBox.PackStart(new Label("-> " + row["Повідомлення"].ToString()) { Wrap = true }, false, false, 5);
                vBoxInfo.PackStart(hBox, false, false, 5);
            }

            vBoxMessage.PackStart(new Separator(Orientation.Horizontal), false, false, 5);
        }
    }
}