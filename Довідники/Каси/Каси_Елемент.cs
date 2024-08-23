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
    class Каси_Елемент : ДовідникЕлемент
    {
        public Каси_Objest Каси_Objest { get; set; } = new Каси_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Валюти_PointerControl Валюта = new Валюти_PointerControl() { Caption = "Валюта:" };

        public Каси_Елемент() : base() { }

        protected override void CreatePack1(Box vBox)
        {
            //Код
            CreateField(vBox, "Код:", Код);

            //Назва
            CreateField(vBox, "Назва:", Назва);

            //Валюта
            CreateField(vBox, null, Валюта);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
                await Каси_Objest.New();

            Код.Text = Каси_Objest.Код;
            Назва.Text = Каси_Objest.Назва;
            Валюта.Pointer = Каси_Objest.Валюта;
        }

        protected override void GetValue()
        {
            Каси_Objest.Код = Код.Text;
            Каси_Objest.Назва = Назва.Text;
            Каси_Objest.Валюта = Валюта.Pointer;
        }

        #endregion

        protected override async ValueTask Save()
        {
            UnigueID = Каси_Objest.UnigueID;
            Caption = Назва.Text;

            try
            {
                await Каси_Objest.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Каси_Objest.GetBasis(), Caption, ex);
            }            
        }
    }
}