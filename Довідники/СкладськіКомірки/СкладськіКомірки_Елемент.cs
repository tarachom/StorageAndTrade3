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
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class СкладськіКомірки_Елемент : ДовідникЕлемент
    {
        public СкладськіКомірки_Objest СкладськіКомірки_Objest { get; set; } = new СкладськіКомірки_Objest();
        public СкладськіПриміщення_Pointer СкладськеПриміщенняДляНового { get; set; } = new СкладськіПриміщення_Pointer();
        public СкладськіКомірки_Папки_Pointer РодичДляНового { get; set; } = new СкладськіКомірки_Папки_Pointer();

        Entry Назва = new Entry() { WidthRequest = 500 };
        СкладськіПриміщення_PointerControl СкладськеПриміщення = new СкладськіПриміщення_PointerControl() { Caption = "Приміщення:" };
        СкладськіКомірки_Папки_PointerControl Родич = new СкладськіКомірки_Папки_PointerControl() { Caption = "Папка:" };
        Entry Лінія = new Entry() { WidthRequest = 200 };
        Entry Стелаж = new Entry() { WidthRequest = 200 };
        Entry Позиція = new Entry() { WidthRequest = 200 };
        Entry Ярус = new Entry() { WidthRequest = 200 };
        ComboBoxText ТипСкладськоїКомірки = new ComboBoxText();
        ТипорозміриКомірок_PointerControl Типорозмір = new ТипорозміриКомірок_PointerControl() { Caption = "Типорозмір:" };

        public СкладськіКомірки_Елемент() : base() { }

        protected override void CreatePack1(VBox vBox)
        {
            //Назва
            CreateField(vBox, "Назва:", Назва);

            //СкладськеПриміщення
            CreateField(vBox, null, СкладськеПриміщення);

            //Родич
            Родич.BeforeClickOpenFunc = () =>
            {
                Родич.СкладПриміщенняВласник = СкладськеПриміщення.Pointer;
            };

            CreateField(vBox, null, Родич);

            //ТипСкладськоїКомірки
            foreach (var field in ПсевдонімиПерелічення.ТипиСкладськихКомірок_List())
                ТипСкладськоїКомірки.Append(field.Value.ToString(), field.Name);

            CreateField(vBox, "Тип комірки:", ТипСкладськоїКомірки);

            //Типорозмір
            CreateField(vBox, null, Типорозмір);
        }

        protected override void CreatePack2(VBox vBox)
        {
            VBox vBoxContainer = new VBox() { WidthRequest = 300, Halign = Align.Start };
            vBox.PackStart(vBoxContainer, false, false, 0);

            // Лінія
            HBox hBoxLine = new HBox() { Halign = Align.End };
            vBoxContainer.PackStart(hBoxLine, false, false, 5);

            hBoxLine.PackStart(new Label("Лінія:"), false, false, 5);
            hBoxLine.PackStart(Лінія, false, false, 5);

            // Стелаж
            HBox hBoxStelaj = new HBox() { Halign = Align.End };
            vBoxContainer.PackStart(hBoxStelaj, false, false, 5);

            hBoxStelaj.PackStart(new Label("Стелаж:"), false, false, 5);
            hBoxStelaj.PackStart(Стелаж, false, false, 5);

            // Позиція
            HBox hBoxPosition = new HBox() { Halign = Align.End };
            vBoxContainer.PackStart(hBoxPosition, false, false, 5);

            hBoxPosition.PackStart(new Label("Позиція:"), false, false, 5);
            hBoxPosition.PackStart(Позиція, false, false, 5);

            // Ярус
            HBox hBoxYarus = new HBox() { Halign = Align.End };
            vBoxContainer.PackStart(hBoxYarus, false, false, 5);

            hBoxYarus.PackStart(new Label("Ярус:"), false, false, 5);
            hBoxYarus.PackStart(Ярус, false, false, 5);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                await СкладськіКомірки_Objest.New();
                СкладськіКомірки_Objest.Приміщення = СкладськеПриміщенняДляНового;
                СкладськіКомірки_Objest.Папка = РодичДляНового;
            }

            Назва.Text = СкладськіКомірки_Objest.Назва;
            СкладськеПриміщення.Pointer = СкладськіКомірки_Objest.Приміщення;
            Родич.Pointer = СкладськіКомірки_Objest.Папка;

            ТипСкладськоїКомірки.ActiveId = СкладськіКомірки_Objest.ТипСкладськоїКомірки.ToString();

            if (ТипСкладськоїКомірки.Active == -1)
                ТипСкладськоїКомірки.ActiveId = ТипиСкладськихКомірок.Зберігання.ToString();

            Лінія.Text = СкладськіКомірки_Objest.Лінія;
            Стелаж.Text = СкладськіКомірки_Objest.Стелаж;
            Позиція.Text = СкладськіКомірки_Objest.Позиція;
            Ярус.Text = СкладськіКомірки_Objest.Ярус;

            Типорозмір.Pointer = СкладськіКомірки_Objest.Типорозмір;
        }

        protected override void GetValue()
        {
            СкладськіКомірки_Objest.Назва = Назва.Text;
            СкладськіКомірки_Objest.Приміщення = СкладськеПриміщення.Pointer;
            СкладськіКомірки_Objest.Папка = Родич.Pointer;

            СкладськіКомірки_Objest.ТипСкладськоїКомірки = Enum.Parse<ТипиСкладськихКомірок>(ТипСкладськоїКомірки.ActiveId);

            СкладськіКомірки_Objest.Лінія = Лінія.Text;
            СкладськіКомірки_Objest.Стелаж = Стелаж.Text;
            СкладськіКомірки_Objest.Позиція = Позиція.Text;
            СкладськіКомірки_Objest.Ярус = Ярус.Text;

            СкладськіКомірки_Objest.Типорозмір = Типорозмір.Pointer;
        }

        #endregion

        protected override async ValueTask Save()
        {
            try
            {
                await СкладськіКомірки_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }

            UnigueID = СкладськіКомірки_Objest.UnigueID;
            Caption = Назва.Text;
        }
    }
}