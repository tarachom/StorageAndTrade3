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
    class Банки_Елемент : ДовідникЕлемент
    {
        public Банки_Objest Банки_Objest { get; set; } = new Банки_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry КодМФО = new Entry() { WidthRequest = 100 };
        Entry УнікальнийКодБанку = new Entry() { WidthRequest = 100 };
        Entry КодЄДРПОУ = new Entry() { WidthRequest = 100 };

        Entry Назва = new Entry() { WidthRequest = 700 };
        Entry ПовнаНазва = new Entry() { WidthRequest = 700 };
        Entry НазваГоловноїУстановиАнг = new Entry() { WidthRequest = 700 };

        public Банки_Елемент() : base()
        {
            HPanedTop.Position = 800;
        }

        protected override void CreatePack1(VBox vBox)
        {
            //Назва
            CreateField(vBox, "Коротка назва:", Назва);

            //Повна Назва
            CreateField(vBox, "Повна назва:", ПовнаНазва);

            //Назва En
            CreateField(vBox, "Назва англ:", НазваГоловноїУстановиАнг);
        }

        protected override void CreatePack2(VBox vBox)
        {
            //Container1
            VBox vBoxContainer1 = new VBox() { WidthRequest = 300, Halign = Align.Start };
            vBox.PackStart(vBoxContainer1, false, false, 0);

            {
                //Код
                CreateField(vBoxContainer1, "Код:", Код);

                //КодМФО
                CreateField(vBoxContainer1, "Код МФО:", КодМФО);

                //УнікальнийКодБанку
                CreateField(vBoxContainer1, "Унікальний код:", УнікальнийКодБанку);

                //КодЄДРПОУ
                CreateField(vBoxContainer1, "Код ЄДРПОУ:", КодЄДРПОУ);
            }
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
                Банки_Objest.New();

            Код.Text = Банки_Objest.Код;
            Назва.Text = Банки_Objest.Назва;
            ПовнаНазва.Text = Банки_Objest.ПовнаНазва;
            НазваГоловноїУстановиАнг.Text = Банки_Objest.НазваГоловноїУстановиАнг;
            КодМФО.Text = Банки_Objest.КодМФО;
            УнікальнийКодБанку.Text = Банки_Objest.УнікальнийКодБанку;
            КодЄДРПОУ.Text = Банки_Objest.КодЄДРПОУ;
        }

        protected override void GetValue()
        {
            UnigueID = Банки_Objest.UnigueID;
            Caption = Назва.Text;

            Банки_Objest.Код = Код.Text;
            Банки_Objest.Назва = Назва.Text;
        }

        #endregion

        protected override void Save()
        {
            try
            {
                Банки_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }
        }
    }
}