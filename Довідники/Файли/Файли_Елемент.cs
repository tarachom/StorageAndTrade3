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
    class Файли_Елемент : ДовідникЕлемент
    {
        public Файли_Objest Файли_Objest { get; set; } = new Файли_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };

        public Файли_Елемент() : base() { }

        protected override void CreatePack1(VBox vBox)
        {
            //Код
            CreateField(vBox, "Код:", Код);

            //Назва
            CreateField(vBox, "Назва:", Назва);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
                Файли_Objest.New();

            Код.Text = Файли_Objest.Код;
            Назва.Text = Файли_Objest.Назва;
        }

        protected override void GetValue()
        {
            UnigueID = Файли_Objest.UnigueID;
            Caption = Назва.Text;

            Файли_Objest.Код = Код.Text;
            Файли_Objest.Назва = Назва.Text;
        }

        #endregion

        protected override void Save()
        {
            try
            {
                Файли_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }
        }
    }
}