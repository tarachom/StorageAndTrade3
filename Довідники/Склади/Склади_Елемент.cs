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
    class Склади_Елемент : ДовідникЕлемент
    {
        public Склади_Objest Склади_Objest { get; set; } = new Склади_Objest();
        public Склади_Папки_Pointer РодичДляНового { get; set; } = new Склади_Папки_Pointer();

        #region Field

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Склади_Папки_PointerControl Родич = new Склади_Папки_PointerControl() { Caption = "Папка:" };
        ComboBoxText ТипСкладу = new ComboBoxText();
        ВидиЦін_PointerControl ВидЦін = new ВидиЦін_PointerControl();
        ComboBoxText Налаштування = new ComboBoxText();
        Склади_ТабличнаЧастина_Контакти Контакти = new Склади_ТабличнаЧастина_Контакти();

        #endregion

        public Склади_Елемент() : base() { }

        protected override void CreatePack1(VBox vBox)
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

        protected override void CreatePack2(VBox vBox)
        {
            //Контакти
            CreateTablePart(vBox, "Контакти:", Контакти);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
            {
                Склади_Objest.New();
                Склади_Objest.Папка = РодичДляНового;
            }

            Код.Text = Склади_Objest.Код;
            Назва.Text = Склади_Objest.Назва;
            Родич.Pointer = Склади_Objest.Папка;
            ВидЦін.Pointer = Склади_Objest.ВидЦін;
            ТипСкладу.ActiveId = Склади_Objest.ТипСкладу.ToString();
            Налаштування.ActiveId = Склади_Objest.НалаштуванняАдресногоЗберігання.ToString();

            if (ТипСкладу.Active == -1)
                ТипСкладу.ActiveId = ТипиСкладів.Гуртовий.ToString();

            if (Налаштування.Active == -1)
                Налаштування.ActiveId = НалаштуванняАдресногоЗберігання.НеВикористовувати.ToString();

            Контакти.Склади_Objest = Склади_Objest;
            Контакти.LoadRecords();
        }

        protected override void GetValue()
        {
            UnigueID = Склади_Objest.UnigueID;
            Caption = Назва.Text;

            Склади_Objest.Код = Код.Text;
            Склади_Objest.Назва = Назва.Text;
            Склади_Objest.Папка = Родич.Pointer;
            Склади_Objest.ВидЦін = ВидЦін.Pointer;
            Склади_Objest.ТипСкладу = Enum.Parse<ТипиСкладів>(ТипСкладу.ActiveId);
            Склади_Objest.НалаштуванняАдресногоЗберігання = Enum.Parse<НалаштуванняАдресногоЗберігання>(Налаштування.ActiveId);
        }

        #endregion

        protected override void Save()
        {
            try
            {
                Склади_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }

            Контакти.SaveRecords();
        }
    }
}