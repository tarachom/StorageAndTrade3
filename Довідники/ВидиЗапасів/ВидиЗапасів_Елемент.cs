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
    class ВидиЗапасів_Елемент : ДовідникЕлемент
    {
        public ВидиЗапасів_Objest ВидиЗапасів_Objest { get; set; } = new ВидиЗапасів_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };

        public ВидиЗапасів_Елемент() : base() 
        {
            ВидиЗапасів_Objest.UnigueIDChanged += UnigueIDChanged;
            ВидиЗапасів_Objest.CaptionChanged += CaptionChanged;
        }

        protected override void CreatePack1(Box vBox)
        {
            //Код
            CreateField(vBox, "Код:", Код);

            //Назва
            CreateField(vBox, "Назва:", Назва);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            Код.Text = ВидиЗапасів_Objest.Код;
            Назва.Text = ВидиЗапасів_Objest.Назва;
        }

        protected override void GetValue()
        {
            ВидиЗапасів_Objest.Код = Код.Text;
            ВидиЗапасів_Objest.Назва = Назва.Text;
        }

        #endregion

        protected override async ValueTask Save()
        {
            try
            {
                await ВидиЗапасів_Objest.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(ВидиЗапасів_Objest.GetBasis(), Caption, ex);
            }
        }
    }
}