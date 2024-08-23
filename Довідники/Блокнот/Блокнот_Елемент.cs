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
using InterfaceGtk;

using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Блокнот_Елемент : ДовідникЕлемент
    {
        public Блокнот_Objest Блокнот_Objest { get; set; } = new Блокнот_Objest();

        #region Fields

        Entry Код = new Entry() { WidthRequest = 500 };

        Entry Назва = new Entry() { WidthRequest = 500 };

        DateTimeControl ДатаЗапису = new DateTimeControl();

        TextView Опис = new TextView() { WidthRequest = 500 };

        Entry Лінк = new Entry() { WidthRequest = 500 };

        #endregion

        #region TabularParts

        #endregion

        public Блокнот_Елемент() : base() { }

        protected override void CreatePack1(Box vBox)
        {

            CreateField(vBox, "Код:", Код);

            CreateField(vBox, "Назва:", Назва);

            CreateField(vBox, "ДатаЗапису:", ДатаЗапису);

            CreateFieldView(vBox, "Опис:", Опис, 800, 500);

            CreateField(vBox, "Лінк:", Лінк);

        }

        protected override void CreatePack2(Box vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
                await Блокнот_Objest.New();

            Код.Text = Блокнот_Objest.Код;
            Назва.Text = Блокнот_Objest.Назва;
            ДатаЗапису.Value = Блокнот_Objest.ДатаЗапису;
            Опис.Buffer.Text = Блокнот_Objest.Опис;
            Лінк.Text = Блокнот_Objest.Лінк;
        }

        protected override void GetValue()
        {
            Блокнот_Objest.Код = Код.Text;
            Блокнот_Objest.Назва = Назва.Text;
            Блокнот_Objest.ДатаЗапису = ДатаЗапису.Value;
            Блокнот_Objest.Опис = Опис.Buffer.Text;
            Блокнот_Objest.Лінк = Лінк.Text;
        }

        #endregion

        protected override async ValueTask Save()
        {
            UnigueID = Блокнот_Objest.UnigueID;
            Caption = Назва.Text;

            try
            {
                await Блокнот_Objest.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Блокнот_Objest.GetBasis(), Caption, ex);
            }
        }
    }
}
