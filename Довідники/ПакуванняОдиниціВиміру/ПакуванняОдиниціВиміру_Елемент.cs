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
    class ПакуванняОдиниціВиміру_Елемент : ДовідникЕлемент
    {
        public ПакуванняОдиниціВиміру_Objest ПакуванняОдиниціВиміру_Objest { get; set; } = new ПакуванняОдиниціВиміру_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Entry НазваПовна = new Entry() { WidthRequest = 500 };
        IntegerControl КількістьУпаковок = new IntegerControl();

        public ПакуванняОдиниціВиміру_Елемент() : base() { }

        protected override void CreatePack1(Box vBox)
        {
            //Код
            CreateField(vBox, "Код:", Код);

            //Назва
            CreateField(vBox, "Назва:", Назва);

            //НазваПовна
            CreateField(vBox, "Назва повна:", НазваПовна);

            //КількістьУпаковок
            CreateField(vBox, "Коєфіціент:", КількістьУпаковок);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                await ПакуванняОдиниціВиміру_Objest.New();
                ПакуванняОдиниціВиміру_Objest.КількістьУпаковок = 1;
            }

            Код.Text = ПакуванняОдиниціВиміру_Objest.Код;
            Назва.Text = ПакуванняОдиниціВиміру_Objest.Назва;
            НазваПовна.Text = ПакуванняОдиниціВиміру_Objest.НазваПовна;
            КількістьУпаковок.Value = ПакуванняОдиниціВиміру_Objest.КількістьУпаковок;
        }

        protected override void GetValue()
        {
            ПакуванняОдиниціВиміру_Objest.Код = Код.Text;
            ПакуванняОдиниціВиміру_Objest.Назва = Назва.Text;
            ПакуванняОдиниціВиміру_Objest.НазваПовна = НазваПовна.Text;
            ПакуванняОдиниціВиміру_Objest.КількістьУпаковок = КількістьУпаковок.Value;
        }

        #endregion

        protected override async ValueTask Save()
        {
            UnigueID = ПакуванняОдиниціВиміру_Objest.UnigueID;
            Caption = Назва.Text;

            try
            {
                await ПакуванняОдиниціВиміру_Objest.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(ПакуванняОдиниціВиміру_Objest.GetBasis(), Caption, ex);
            }            
        }
    }
}