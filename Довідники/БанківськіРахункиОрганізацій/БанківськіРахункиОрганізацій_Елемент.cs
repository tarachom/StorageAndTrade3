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
    class БанківськіРахункиОрганізацій_Елемент : ДовідникЕлемент
    {
        public БанківськіРахункиОрганізацій_Objest БанківськіРахункиОрганізацій_Objest { get; set; } = new БанківськіРахункиОрганізацій_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Валюти_PointerControl Валюта = new Валюти_PointerControl() { Caption = "Валюта:" };
        Організації_PointerControl Організація = new Організації_PointerControl() { Caption = "Організація:" };

        public БанківськіРахункиОрганізацій_Елемент() : base() { }

        protected override void CreatePack1(VBox vBox)
        {
            //Код
            CreateField(vBox, "Код:", Код);

            //Назва
            CreateField(vBox, "Назва:", Назва);

            //Валюта
            CreateField(vBox, null, Валюта);

            //Організація
            CreateField(vBox, null, Організація);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
                await БанківськіРахункиОрганізацій_Objest.New();

            Код.Text = БанківськіРахункиОрганізацій_Objest.Код;
            Назва.Text = БанківськіРахункиОрганізацій_Objest.Назва;
            Валюта.Pointer = БанківськіРахункиОрганізацій_Objest.Валюта;
            Організація.Pointer = БанківськіРахункиОрганізацій_Objest.Організація;
        }

        protected override void GetValue()
        {
            БанківськіРахункиОрганізацій_Objest.Код = Код.Text;
            БанківськіРахункиОрганізацій_Objest.Назва = Назва.Text;
            БанківськіРахункиОрганізацій_Objest.Валюта = Валюта.Pointer;
            БанківськіРахункиОрганізацій_Objest.Організація = Організація.Pointer;
        }

        #endregion

        protected override async ValueTask Save()
        {
            try
            {
                await БанківськіРахункиОрганізацій_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }

            UnigueID = БанківськіРахункиОрганізацій_Objest.UnigueID;
            Caption = Назва.Text;
        }
    }
}