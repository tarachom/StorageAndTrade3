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
    class БанківськіРахункиКонтрагентів_Елемент : ДовідникЕлемент
    {
        public БанківськіРахункиКонтрагентів_Objest БанківськіРахункиКонтрагентів_Objest { get; set; } = new БанківськіРахункиКонтрагентів_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Валюти_PointerControl Валюта = new Валюти_PointerControl() { Caption = "Валюта:" };
        Контрагенти_PointerControl Контрагент = new Контрагенти_PointerControl() { Caption = "Контрагент:" };

        public БанківськіРахункиКонтрагентів_Елемент() : base() { }

        protected override void CreatePack1()
        {
            VBox vBox = new VBox();
            HPanedTop.Pack1(vBox, false, false);

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
            if (IsNew)
                БанківськіРахункиКонтрагентів_Objest.New();

            Код.Text = БанківськіРахункиКонтрагентів_Objest.Код;
            Назва.Text = БанківськіРахункиКонтрагентів_Objest.Назва;
            Валюта.Pointer = БанківськіРахункиКонтрагентів_Objest.Валюта;
            Контрагент.Pointer = БанківськіРахункиКонтрагентів_Objest.Контрагент;
        }

        protected override void GetValue()
        {
            UnigueID = БанківськіРахункиКонтрагентів_Objest.UnigueID;
            Caption = Назва.Text;

            БанківськіРахункиКонтрагентів_Objest.Код = Код.Text;
            БанківськіРахункиКонтрагентів_Objest.Назва = Назва.Text;
            БанківськіРахункиКонтрагентів_Objest.Валюта = Валюта.Pointer;
            БанківськіРахункиКонтрагентів_Objest.Контрагент = Контрагент.Pointer;
        }

        #endregion

        protected override void Save()
        {
            try
            {
                БанківськіРахункиКонтрагентів_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }
        }
    }
}