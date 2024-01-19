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
    class ВидиЦін_Елемент : ДовідникЕлемент
    {
        public ВидиЦін_Objest ВидиЦін_Objest { get; set; } = new ВидиЦін_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Валюти_PointerControl Валюта = new Валюти_PointerControl() { Caption = "Валюта:" };

        public ВидиЦін_Елемент() : base() { }

        protected override void CreatePack1(VBox vBox)
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
                await ВидиЦін_Objest.New();

            Код.Text = ВидиЦін_Objest.Код;
            Назва.Text = ВидиЦін_Objest.Назва;
            Валюта.Pointer = ВидиЦін_Objest.Валюта;
        }

        protected override void GetValue()
        {
            ВидиЦін_Objest.Код = Код.Text;
            ВидиЦін_Objest.Назва = Назва.Text;
            ВидиЦін_Objest.Валюта = Валюта.Pointer;
        }

        #endregion

        protected override async ValueTask Save()
        {
            try
            {
                await ВидиЦін_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
                return;
            }

            UnigueID = ВидиЦін_Objest.UnigueID;
            Caption = Назва.Text;
        }
    }
}