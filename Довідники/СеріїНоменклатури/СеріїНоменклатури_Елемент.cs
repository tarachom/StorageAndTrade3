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
        public СеріїНоменклатури_Objest СеріїНоменклатури_Objest { get; set; } = new СеріїНоменклатури_Objest();

        Entry Номер = new Entry() { WidthRequest = 500 };
        Entry Коментар = new Entry() { WidthRequest = 500 };
        DateTimeControl ДатаСтворення = new DateTimeControl();

        public СеріїНоменклатури_Елемент() : base() { }

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

        public override async void SetValue()
        {
            if (IsNew)
                await СеріїНоменклатури_Objest.New();

            Номер.Text = СеріїНоменклатури_Objest.Номер;
            Коментар.Text = СеріїНоменклатури_Objest.Коментар;
            ДатаСтворення.Value = СеріїНоменклатури_Objest.ДатаСтворення;
        }

        protected override void GetValue()
        {
            СеріїНоменклатури_Objest.Номер = Номер.Text;
            СеріїНоменклатури_Objest.Коментар = Коментар.Text;
            СеріїНоменклатури_Objest.ДатаСтворення = ДатаСтворення.Value;
        }

        #endregion

        protected override async ValueTask Save()
        {
            UnigueID = СеріїНоменклатури_Objest.UnigueID;
            Caption = СеріїНоменклатури_Objest.Номер;

            try
            {
                await СеріїНоменклатури_Objest.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(СеріїНоменклатури_Objest.GetBasis(), Caption, ex);
            }            
        }
    }
}