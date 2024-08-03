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
using InterfaceGtk;

using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Організації_Елемент : ДовідникЕлемент
    {
        public Організації_Objest Організації_Objest { get; set; } = new Організації_Objest();

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

        public Організації_Елемент() : base() { }

        protected override void CreatePack1(VBox vBox)
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

        protected override void CreatePack2(VBox vBox)
        {
            //Контакти
            CreateTablePart(vBox, "Контакти:", Контакти);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
                await Організації_Objest.New();

            Код.Text = Організації_Objest.Код;
            Назва.Text = Організації_Objest.Назва;
            НазваСкорочена.Text = Організації_Objest.НазваСкорочена;
            ДатаРеєстрації.Value = Організації_Objest.ДатаРеєстрації;
            КраїнаРеєстрації.Text = Організації_Objest.КраїнаРеєстрації;
            СвідоцтвоСеріяНомер.Text = Організації_Objest.СвідоцтвоСеріяНомер;
            СвідоцтвоДатаВидачі.Text = Організації_Objest.СвідоцтвоДатаВидачі;
            НазваПовна.Buffer.Text = Організації_Objest.НазваПовна;
            Холдинг.Pointer = Організації_Objest.Холдинг;

            Контакти.Організації_Objest = Організації_Objest;
            Контакти.LoadRecords();
        }

        protected override void GetValue()
        {
            Організації_Objest.Код = Код.Text;
            Організації_Objest.Назва = Назва.Text;
            Організації_Objest.НазваСкорочена = НазваСкорочена.Text;
            Організації_Objest.ДатаРеєстрації = ДатаРеєстрації.Value;
            Організації_Objest.КраїнаРеєстрації = КраїнаРеєстрації.Text;
            Організації_Objest.СвідоцтвоСеріяНомер = СвідоцтвоСеріяНомер.Text;
            Організації_Objest.СвідоцтвоДатаВидачі = СвідоцтвоДатаВидачі.Text;
            Організації_Objest.НазваПовна = НазваПовна.Buffer.Text;
            Організації_Objest.Холдинг = Холдинг.Pointer;
        }

        #endregion

        protected override async ValueTask Save()
        {
            try
            {
                if (await Організації_Objest.Save())
                    await Контакти.SaveRecords();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }

            UnigueID = Організації_Objest.UnigueID;
            Caption = Назва.Text;
        }
    }
}