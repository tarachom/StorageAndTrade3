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
    class Організації_Елемент : ДовідникЕлемент
    {
        public Організації_Objest Елемент { get; set; } = new Організації_Objest();

        #region Field

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Entry НазваСкорочена = new Entry() { WidthRequest = 500 };
        TextView НазваПовна = new TextView();
        DateTimeControl ДатаРеєстрації = new DateTimeControl() { OnlyDate = true };
        Entry КраїнаРеєстрації = new Entry() { WidthRequest = 300 };
        Entry СвідоцтвоСеріяНомер = new Entry() { WidthRequest = 300 };
        Entry СвідоцтвоДатаВидачі = new Entry() { WidthRequest = 300 };
        Організації_PointerControl Холдинг = new Організації_PointerControl() { Caption = "Холдинг:" };

        Організації_ТабличнаЧастина_Контакти Контакти = new Організації_ТабличнаЧастина_Контакти();

        #endregion

        public Організації_Елемент() 
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;

            Контакти.ЕлементВласник = Елемент;
        }

        protected override void CreatePack1(Box vBox)
        {
            //Код
            CreateField(vBox, "Код:", Код);

            //Назва
            CreateField(vBox, "Назва:", Назва);

            //НазваСкорочена
            CreateField(vBox, "Назва скорочена:", НазваСкорочена);

            //НазваПовна
            CreateFieldView(vBox, "Повна назва:", НазваПовна, 500, 100);

            //ДатаРеєстрації
            CreateField(vBox, "Дата реєстрації:", ДатаРеєстрації);

            //КраїнаРеєстрації
            CreateField(vBox, "Країна реєстрації:", КраїнаРеєстрації);

            //СвідоцтвоСеріяНомер
            CreateField(vBox, "Свідоцтво серія номер:", СвідоцтвоСеріяНомер);

            //СвідоцтвоДатаВидачі
            CreateField(vBox, "Свідоцтво дата видачі:", СвідоцтвоДатаВидачі);

            //Холдинг
            CreateField(vBox, null, Холдинг);
        }

        protected override void CreatePack2(Box vBox)
        {
            //Контакти
            CreateTablePart(vBox, "Контакти:", Контакти);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            Код.Text = Елемент.Код;
            Назва.Text = Елемент.Назва;
            НазваСкорочена.Text = Елемент.НазваСкорочена;
            ДатаРеєстрації.Value = Елемент.ДатаРеєстрації;
            КраїнаРеєстрації.Text = Елемент.КраїнаРеєстрації;
            СвідоцтвоСеріяНомер.Text = Елемент.СвідоцтвоСеріяНомер;
            СвідоцтвоДатаВидачі.Text = Елемент.СвідоцтвоДатаВидачі;
            НазваПовна.Buffer.Text = Елемент.НазваПовна;
            
            Холдинг.Pointer = Елемент.Холдинг;
            Холдинг.OpenFolder = Елемент.UnigueID;//!+

            Контакти.LoadRecords();
        }

        protected override void GetValue()
        {
            Елемент.Код = Код.Text;
            Елемент.Назва = Назва.Text;
            Елемент.НазваСкорочена = НазваСкорочена.Text;
            Елемент.ДатаРеєстрації = ДатаРеєстрації.Value;
            Елемент.КраїнаРеєстрації = КраїнаРеєстрації.Text;
            Елемент.СвідоцтвоСеріяНомер = СвідоцтвоСеріяНомер.Text;
            Елемент.СвідоцтвоДатаВидачі = СвідоцтвоДатаВидачі.Text;
            Елемент.НазваПовна = НазваПовна.Buffer.Text;
            Елемент.Холдинг = Холдинг.Pointer;
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