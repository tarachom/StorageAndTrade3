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
    class БанківськіРахункиКонтрагентів_Елемент : ДовідникЕлемент
    {
        public БанківськіРахункиКонтрагентів_Objest Елемент { get; set; } = new БанківськіРахункиКонтрагентів_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Валюти_PointerControl Валюта = new Валюти_PointerControl() { Caption = "Валюта:" };
        Контрагенти_PointerControl Контрагент = new Контрагенти_PointerControl() { Caption = "Контрагент:" };

        public БанківськіРахункиКонтрагентів_Елемент() : base() 
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;
        }

        protected override void CreatePack1(Box vBox)
        {
            //Код
            CreateField(vBox, "Код:", Код);

            //Назва
            CreateField(vBox, "Назва:", Назва);

            //Валюта
            CreateField(vBox, null, Валюта);

            //Контрагент
            CreateField(vBox, null, Контрагент);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            Код.Text = Елемент.Код;
            Назва.Text = Елемент.Назва;
            Валюта.Pointer = Елемент.Валюта;
            Контрагент.Pointer = Елемент.Контрагент;
        }

        protected override void GetValue()
        {
            Елемент.Код = Код.Text;
            Елемент.Назва = Назва.Text;
            Елемент.Валюта = Валюта.Pointer;
            Елемент.Контрагент = Контрагент.Pointer;
        }

        #endregion

        protected override async ValueTask Save()
        {
            try
            {
                await Елемент.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Елемент.GetBasis(), Caption, ex);
            }
        }
    }
}