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
    class ФізичніОсоби_Елемент : ДовідникЕлемент
    {
        public ФізичніОсоби_Objest ФізичніОсоби_Objest { get; set; } = new ФізичніОсоби_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };

        public ФізичніОсоби_Елемент() : base() 
        {
            ФізичніОсоби_Objest.UnigueIDChanged += UnigueIDChanged;
            ФізичніОсоби_Objest.CaptionChanged += CaptionChanged;
        }

        protected override void CreatePack1(Box vBox)
        {
            //Код
            CreateField(vBox, "Код:", Код);

            //Назва
            CreateField(vBox, "Назва:", Назва);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
                await ФізичніОсоби_Objest.New();

            Код.Text = ФізичніОсоби_Objest.Код;
            Назва.Text = ФізичніОсоби_Objest.Назва;
        }

        protected override void GetValue()
        {
            ФізичніОсоби_Objest.Код = Код.Text;
            ФізичніОсоби_Objest.Назва = Назва.Text;
        }

        #endregion

        protected override async ValueTask Save()
        {
            try
            {
                await ФізичніОсоби_Objest.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(ФізичніОсоби_Objest.GetBasis(), Caption, ex);
            }
        }
    }
}