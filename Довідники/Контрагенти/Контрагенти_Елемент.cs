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
    class Контрагенти_Елемент : ДовідникЕлемент
    {
        public Контрагенти_Objest Елемент { get; set; } = new Контрагенти_Objest();
        public Контрагенти_Папки_Pointer РодичДляНового { get; set; } = new Контрагенти_Папки_Pointer();

        #region Field

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        TextView НазваПовна = new TextView();
        Entry РеєстраційнийНомер = new Entry() { WidthRequest = 300 };
        TextView Опис = new TextView();
        Контрагенти_Папки_PointerControl Родич = new Контрагенти_Папки_PointerControl() { Caption = "Папка:" };
        CheckButton Постачальник = new CheckButton("Постачальник");
        CheckButton Покупець = new CheckButton("Покупець");

        Контрагенти_ТабличнаЧастина_Контакти Контакти = new Контрагенти_ТабличнаЧастина_Контакти();
        Контрагенти_ТабличнаЧастина_Файли Файли = new Контрагенти_ТабличнаЧастина_Файли();

        #endregion

        public Контрагенти_Елемент()
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

            //НазваПовна
            CreateFieldView(vBox, "Повна назва:", НазваПовна, 500, 60);

            //РеєстраційнийНомер
            CreateField(vBox, "Реєстраційний номер:", РеєстраційнийНомер);

            //Опис
            CreateFieldView(vBox, "Опис:", Опис, 500, 200);

            //Постачальник та Покупець
            CreateField(CreateField(vBox, null, Постачальник), null, Покупець);
        }

        protected override void CreatePack2(Box vBox)
        {
            //Контакти
            CreateTablePart(vBox, "Контакти:", Контакти);

            //Файли
            CreateTablePart(vBox, "Файли:", Файли);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
                Елемент.Папка = РодичДляНового;

            Код.Text = Елемент.Код;
            Назва.Text = Елемент.Назва;
            Родич.Pointer = Елемент.Папка;
            НазваПовна.Buffer.Text = Елемент.НазваПовна;
            РеєстраційнийНомер.Text = Елемент.РеєстраційнийНомер;
            Опис.Buffer.Text = Елемент.Опис;
            Постачальник.Active = Елемент.Постачальник;
            Покупець.Active = Елемент.Покупець;

            Контакти.ЕлементВласник = Елемент;
            await Контакти.LoadRecords();

            Файли.ЕлементВласник = Елемент;
            await Файли.LoadRecords();
        }

        protected override void GetValue()
        {
            Елемент.Код = Код.Text;
            Елемент.Назва = Назва.Text;
            Елемент.Папка = Родич.Pointer;
            Елемент.НазваПовна = НазваПовна.Buffer.Text;
            Елемент.РеєстраційнийНомер = РеєстраційнийНомер.Text;
            Елемент.Опис = Опис.Buffer.Text;
            Елемент.Постачальник = Постачальник.Active;
            Елемент.Покупець = Покупець.Active;
            Елемент.КлючовіСловаДляПошуку = Контакти.КлючовіСловаДляПошуку();
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            try
            {
                if (await Елемент.Save())
                {
                    await Контакти.SaveRecords();
                    await Файли.SaveRecords();
                }
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