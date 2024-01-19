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
    class СкладськіКомірки_Папки_Елемент : ДовідникЕлемент
    {
        public СкладськіКомірки_Папки_Objest СкладськіКомірки_Папки_Objest { get; set; } = new СкладськіКомірки_Папки_Objest();
        public СкладськіКомірки_Папки_Pointer РодичДляНового { get; set; } = new СкладськіКомірки_Папки_Pointer();
        public СкладськіПриміщення_Pointer СкладськеПриміщенняДляНового { get; set; } = new СкладськіПриміщення_Pointer();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        СкладськіКомірки_Папки_PointerControl Родич = new СкладськіКомірки_Папки_PointerControl() { Caption = "Родич:" };
        СкладськіПриміщення_PointerControl СкладськеПриміщення = new СкладськіПриміщення_PointerControl() { Caption = "Приміщення:" };

        public СкладськіКомірки_Папки_Елемент() : base() { }

        protected override void CreatePack1(VBox vBox)
        {
            //Код
            CreateField(vBox, "Код:", Код);

            //Назва
            CreateField(vBox, "Назва:", Назва);

            //СкладськеПриміщення
            CreateField(vBox, null, СкладськеПриміщення);

            //Родич
            CreateField(vBox, null, Родич);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                await СкладськіКомірки_Папки_Objest.New();
                СкладськіКомірки_Папки_Objest.Родич = РодичДляНового;
                СкладськіКомірки_Папки_Objest.Власник = СкладськеПриміщенняДляНового;
            }
            else
                Родич.OpenFolder = СкладськіКомірки_Папки_Objest.UnigueID;

            Код.Text = СкладськіКомірки_Папки_Objest.Код;
            Назва.Text = СкладськіКомірки_Папки_Objest.Назва;
            Родич.Pointer = СкладськіКомірки_Папки_Objest.Родич;
            СкладськеПриміщення.Pointer = СкладськіКомірки_Папки_Objest.Власник;
        }

        protected override void GetValue()
        {
            СкладськіКомірки_Папки_Objest.Код = Код.Text;
            СкладськіКомірки_Папки_Objest.Назва = Назва.Text;
            СкладськіКомірки_Папки_Objest.Родич = Родич.Pointer;
            СкладськіКомірки_Папки_Objest.Власник = СкладськеПриміщення.Pointer;
        }

        #endregion

        protected override async ValueTask Save()
        {
            try
            {
                await СкладськіКомірки_Папки_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }

            UnigueID = СкладськіКомірки_Папки_Objest.UnigueID;
            Caption = Назва.Text;
        }
    }
}