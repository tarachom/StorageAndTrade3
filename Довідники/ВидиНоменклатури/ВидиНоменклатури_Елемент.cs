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
    class ВидиНоменклатури_Елемент : ДовідникЕлемент
    {
        public ВидиНоменклатури_Objest ВидиНоменклатури_Objest { get; set; } = new ВидиНоменклатури_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        ПакуванняОдиниціВиміру_PointerControl ОдиницяВиміру = new ПакуванняОдиниціВиміру_PointerControl();

        public ВидиНоменклатури_Елемент() : base() { }

        protected override void CreatePack1()
        {
            VBox vBox = new VBox();
            HPanedTop.Pack1(vBox, false, false);

            //Код
            HBox hBoxCode = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxCode, false, false, 5);

            hBoxCode.PackStart(new Label("Код:"), false, false, 5);
            hBoxCode.PackStart(Код, false, false, 5);

            //Назва
            HBox hBoxName = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxName, false, false, 5);

            hBoxName.PackStart(new Label("Назва:"), false, false, 5);
            hBoxName.PackStart(Назва, false, false, 5);

            //ОдиницяВиміру
            HBox hBoxPackuvannja = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxPackuvannja, false, false, 5);

            hBoxPackuvannja.PackStart(ОдиницяВиміру, false, false, 5);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
                ВидиНоменклатури_Objest.New();

            Код.Text = ВидиНоменклатури_Objest.Код;
            Назва.Text = ВидиНоменклатури_Objest.Назва;
            ОдиницяВиміру.Pointer = ВидиНоменклатури_Objest.ОдиницяВиміру;
        }

        protected override void GetValue()
        {
            UnigueID = ВидиНоменклатури_Objest.UnigueID;
            Caption = Назва.Text;

            ВидиНоменклатури_Objest.Код = Код.Text;
            ВидиНоменклатури_Objest.Назва = Назва.Text;
            ВидиНоменклатури_Objest.ОдиницяВиміру = ОдиницяВиміру.Pointer;
        }

        #endregion

        protected override void Save()
        {
            try
            {
                ВидиНоменклатури_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }
        }
    }
}