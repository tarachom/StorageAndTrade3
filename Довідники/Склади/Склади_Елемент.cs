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
    class Склади_Елемент : ДовідникЕлемент
    {
        public Склади_Objest Елемент { get; set; } = new Склади_Objest();
        public Склади_Папки_Pointer РодичДляНового { get; set; } = new Склади_Папки_Pointer();

        #region Field

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Склади_Папки_PointerControl Родич = new Склади_Папки_PointerControl() { Caption = "Папка:" };
        ComboBoxText ТипСкладу = new ComboBoxText();
        ВидиЦін_PointerControl ВидЦін = new ВидиЦін_PointerControl() { Caption = "Вид цін:" };
        ComboBoxText Налаштування = new ComboBoxText();
        Склади_ТабличнаЧастина_Контакти Контакти = new Склади_ТабличнаЧастина_Контакти() { HeightRequest = 300 };

        #endregion

        public Склади_Елемент() 
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;
        }

        protected override void CreatePack1(Box vBox)
        {
            //Код
            CreateField(vBox, "Код:", Код);

            //Назва
            CreateField(vBox, "Назва:", Назва);

            //Родич
            CreateField(vBox, null, Родич);

            //ВидЦін
            CreateField(vBox, null, ВидЦін);

            //Тип складу
            foreach (var field in ПсевдонімиПерелічення.ТипиСкладів_List())
                ТипСкладу.Append(field.Value.ToString(), field.Name);

            CreateField(vBox, "Тип складу:", ТипСкладу);

            //НалаштуванняАдресногоЗберігання
            foreach (var field in ПсевдонімиПерелічення.НалаштуванняАдресногоЗберігання_List())
                Налаштування.Append(field.Value.ToString(), field.Name);

            CreateField(vBox, "Адресне зберігання:", Налаштування);
        }

        protected override void CreatePack2(Box vBox)
        {
            //Контакти
            CreateTablePart(vBox, "Контакти:", Контакти);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
                Елемент.Папка = РодичДляНового;

            Код.Text = Елемент.Код;
            Назва.Text = Елемент.Назва;
            Родич.Pointer = Елемент.Папка;
            ВидЦін.Pointer = Елемент.ВидЦін;
            ТипСкладу.ActiveId = Елемент.ТипСкладу.ToString();
            Налаштування.ActiveId = Елемент.НалаштуванняАдресногоЗберігання.ToString();

            if (ТипСкладу.Active == -1)
                ТипСкладу.ActiveId = ТипиСкладів.Гуртовий.ToString();

            if (Налаштування.Active == -1)
                Налаштування.ActiveId = НалаштуванняАдресногоЗберігання.НеВикористовувати.ToString();

            Контакти.ЕлементВласник = Елемент;
           await Контакти.LoadRecords();
        }

        protected override void GetValue()
        {
            Елемент.Код = Код.Text;
            Елемент.Назва = Назва.Text;
            Елемент.Папка = Родич.Pointer;
            Елемент.ВидЦін = ВидЦін.Pointer;
            Елемент.ТипСкладу = Enum.Parse<ТипиСкладів>(ТипСкладу.ActiveId);
            Елемент.НалаштуванняАдресногоЗберігання = Enum.Parse<НалаштуванняАдресногоЗберігання>(Налаштування.ActiveId);
            Елемент.КлючовіСловаДляПошуку = Контакти.КлючовіСловаДляПошуку();
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            try
            {
                if (await Елемент.Save())
                    await Контакти.SaveRecords();
                return true;
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Елемент.GetBasis(), Caption, ex);
                return false;
            }
        }
    }
}