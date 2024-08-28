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
    class КраїниСвіту_Елемент : ДовідникЕлемент
    {
        public КраїниСвіту_Objest КраїниСвіту_Objest { get; set; } = new КраїниСвіту_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };

        public КраїниСвіту_Елемент() : base() 
        {
            КраїниСвіту_Objest.UnigueIDChanged += UnigueIDChanged;
            КраїниСвіту_Objest.CaptionChanged += CaptionChanged;
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
            Код.Text = КраїниСвіту_Objest.Код;
            Назва.Text = КраїниСвіту_Objest.Назва;
        }

        protected override void GetValue()
        {
            КраїниСвіту_Objest.Код = Код.Text;
            КраїниСвіту_Objest.Назва = Назва.Text;
        }

        #endregion

        protected override async ValueTask Save()
        {
            try
            {
                await КраїниСвіту_Objest.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(КраїниСвіту_Objest.GetBasis(), Caption, ex);
            }            
        }
    }
}