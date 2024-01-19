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
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class СкладськіПриміщення_Елемент : ДовідникЕлемент
    {
        public СкладськіПриміщення_Objest СкладськіПриміщення_Objest { get; set; } = new СкладськіПриміщення_Objest();
        public Склади_Pointer СкладДляНового { get; set; } = new Склади_Pointer();

        Entry Назва = new Entry() { WidthRequest = 500 };
        Склади_PointerControl Склад = new Склади_PointerControl() { Caption = "Склад:" };
        ComboBoxText Налаштування = new ComboBoxText();

        public СкладськіПриміщення_Елемент() : base() { }

        protected override void CreatePack1(VBox vBox)
        {
            //Склад
            CreateField(vBox, null, Склад);

            //Назва
            CreateField(vBox, "Назва:", Назва);

            //НалаштуванняАдресногоЗберігання
            foreach (var field in ПсевдонімиПерелічення.НалаштуванняАдресногоЗберігання_List())
                Налаштування.Append(field.Value.ToString(), field.Name);

            CreateField(vBox, "Адресне зберігання:", Налаштування);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                await СкладськіПриміщення_Objest.New();
                СкладськіПриміщення_Objest.Склад = СкладДляНового;
            }

            Назва.Text = СкладськіПриміщення_Objest.Назва;
            Склад.Pointer = СкладськіПриміщення_Objest.Склад;
            Налаштування.ActiveId = СкладськіПриміщення_Objest.НалаштуванняАдресногоЗберігання.ToString();

            if (Налаштування.Active == -1)
                Налаштування.ActiveId = НалаштуванняАдресногоЗберігання.НеВикористовувати.ToString();
        }

        protected override void GetValue()
        {
            СкладськіПриміщення_Objest.Назва = Назва.Text;
            СкладськіПриміщення_Objest.Склад = Склад.Pointer;
            СкладськіПриміщення_Objest.НалаштуванняАдресногоЗберігання = Enum.Parse<НалаштуванняАдресногоЗберігання>(Налаштування.ActiveId);
        }

        #endregion

        protected override async ValueTask Save()
        {
            try
            {
                await СкладськіПриміщення_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
                return;
            }

            UnigueID = СкладськіПриміщення_Objest.UnigueID;
            Caption = Назва.Text;
        }
    }
}