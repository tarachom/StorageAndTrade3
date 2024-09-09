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
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class СкладськіПриміщення_Елемент : ДовідникЕлемент
    {
        public СкладськіПриміщення_Objest Елемент { get; set; } = new СкладськіПриміщення_Objest();
        public Склади_Pointer СкладДляНового { get; set; } = new Склади_Pointer();

        Entry Назва = new Entry() { WidthRequest = 500 };
        Склади_PointerControl Склад = new Склади_PointerControl() { Caption = "Склад:" };
        ComboBoxText Налаштування = new ComboBoxText();

        public СкладськіПриміщення_Елемент() 
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;
        }

        protected override void CreatePack1(Box vBox)
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

        public override void SetValue()
        {
            if (IsNew)
                Елемент.Склад = СкладДляНового;

            Назва.Text = Елемент.Назва;
            Склад.Pointer = Елемент.Склад;
            Налаштування.ActiveId = Елемент.НалаштуванняАдресногоЗберігання.ToString();

            if (Налаштування.Active == -1)
                Налаштування.ActiveId = НалаштуванняАдресногоЗберігання.НеВикористовувати.ToString();
        }

        protected override void GetValue()
        {
            Елемент.Назва = Назва.Text;
            Елемент.Склад = Склад.Pointer;
            Елемент.НалаштуванняАдресногоЗберігання = Enum.Parse<НалаштуванняАдресногоЗберігання>(Налаштування.ActiveId);
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            try
            {
                return await Елемент.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Елемент.GetBasis(), Caption, ex);
                return false;
            }
        }
    }
}