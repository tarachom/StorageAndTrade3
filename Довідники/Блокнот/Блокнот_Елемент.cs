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

        #region Fields

        Entry Код = new Entry() { WidthRequest = 500 };

        Entry Назва = new Entry() { WidthRequest = 500 };

        DateTimeControl ДатаЗапису = new DateTimeControl();

        TextView Опис = new TextView() { WidthRequest = 500 };

        Entry Лінк = new Entry() { WidthRequest = 500 };

        #endregion

        #region TabularParts

        #endregion

        public Блокнот_Елемент() : base()
        {

        }

        protected override void CreatePack1(VBox vBox)
        {

            CreateField(vBox, "Код:", Код);

            CreateField(vBox, "Назва:", Назва);

            CreateField(vBox, "ДатаЗапису:", ДатаЗапису);

            CreateFieldView(vBox, "Опис:", Опис, 800, 500);

            CreateField(vBox, "Лінк:", Лінк);

        }

        protected override void CreatePack2(VBox vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
                Блокнот_Objest.New();

            Код.Text = Блокнот_Objest.Код;
            Назва.Text = Блокнот_Objest.Назва;
            ДатаЗапису.Value = Блокнот_Objest.ДатаЗапису;
            Опис.Buffer. Text = Блокнот_Objest.Опис;
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
