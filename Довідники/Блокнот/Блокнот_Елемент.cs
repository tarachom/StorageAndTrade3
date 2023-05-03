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

using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Блокнот_Елемент : ДовідникЕлемент
    {
        public Блокнот_Objest Блокнот_Objest { get; set; } = new Блокнот_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 800 };
        DateTimeControl ДатаЗапису = new DateTimeControl();
        TextView Опис = new TextView();
        Entry Лінк = new Entry() { WidthRequest = 800 };

        public Блокнот_Елемент() : base() { }

        protected override void CreatePack1()
        {
            VBox vBox = new VBox();
            HPanedTop.Pack1(vBox, false, false);

            //Код + ДатаЗапису
            HBox hBoxCode = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxCode, false, false, 5);

            hBoxCode.PackStart(new Label("Код:"), false, false, 5);
            hBoxCode.PackStart(Код, false, false, 5);
            hBoxCode.PackStart(ДатаЗапису, false, false, 5);

            //Назва
            HBox hBoxName = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxName, false, false, 5);

            hBoxName.PackStart(new Label("Назва:"), false, false, 5);
            hBoxName.PackStart(Назва, false, false, 5);

            //Опис
            HBox hBoxOpys = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxOpys, false, false, 5);

            hBoxOpys.PackStart(new Label("Опис:") { Valign = Align.Start }, false, false, 5);

            ScrolledWindow scrollTextViewOpys = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = 800, HeightRequest = 500 };
            scrollTextViewOpys.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrollTextViewOpys.Add(Опис);

            hBoxOpys.PackStart(scrollTextViewOpys, false, false, 5);

            //Лінк
            HBox hBoxLink = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxLink, false, false, 5);

            hBoxLink.PackStart(new Label("Лінк:"), false, false, 5);
            hBoxLink.PackStart(Лінк, false, false, 5);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
                Блокнот_Objest.New();

            Код.Text = Блокнот_Objest.Код;
            Назва.Text = Блокнот_Objest.Назва;
            ДатаЗапису.Value = Блокнот_Objest.ДатаЗапису;
            Опис.Buffer.Text = Блокнот_Objest.Опис;
            Лінк.Text = Блокнот_Objest.Лінк;
        }

        protected override void GetValue()
        {
            UnigueID = Блокнот_Objest.UnigueID;
            Caption = Назва.Text;

            Блокнот_Objest.Код = Код.Text;
            Блокнот_Objest.Назва = Назва.Text;
            Блокнот_Objest.ДатаЗапису = ДатаЗапису.Value;
            Блокнот_Objest.Опис = Опис.Buffer.Text;
            Блокнот_Objest.Лінк = Лінк.Text;
        }

        #endregion

        protected override void Save()
        {
            try
            {
                Блокнот_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }
        }
    }
}