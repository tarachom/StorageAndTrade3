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
    class СеріїНоменклатури_Елемент : ДовідникЕлемент
    {
        public СеріїНоменклатури_Objest Елемент { get; set; } = new СеріїНоменклатури_Objest();

        Entry Номер = new Entry() { WidthRequest = 500 };
        Entry Коментар = new Entry() { WidthRequest = 500 };
        DateTimeControl ДатаСтворення = new DateTimeControl();

        public СеріїНоменклатури_Елемент() : base() 
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;
        }

        protected override void CreatePack1(Box vBox)
        {
            //Номер
            CreateField(vBox, "Номер:", Номер);

            //Коментар
            CreateField(vBox, "Коментар:", Коментар);

            //ДатаСтворення
            CreateField(vBox, "Cтворений:", ДатаСтворення);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            Номер.Text = Елемент.Номер;
            Коментар.Text = Елемент.Коментар;
            ДатаСтворення.Value = Елемент.ДатаСтворення;
        }

        protected override void GetValue()
        {
            Елемент.Номер = Номер.Text;
            Елемент.Коментар = Коментар.Text;
            Елемент.ДатаСтворення = ДатаСтворення.Value;
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