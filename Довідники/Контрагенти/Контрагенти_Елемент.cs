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
    class Контрагенти_Елемент : ДовідникЕлемент
    {
        public Контрагенти_Objest Контрагенти_Objest { get; set; } = new Контрагенти_Objest();
        public Контрагенти_Папки_Pointer РодичДляНового { get; set; } = new Контрагенти_Папки_Pointer();

        #region Field

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        TextView НазваПовна = new TextView();
        Entry РеєстраційнийНомер = new Entry() { WidthRequest = 300 };
        TextView Опис = new TextView();
        Контрагенти_Папки_PointerControl Родич = new Контрагенти_Папки_PointerControl() { Caption = "Папка:" };
        Контрагенти_ТабличнаЧастина_Контакти Контакти = new Контрагенти_ТабличнаЧастина_Контакти();
        Контрагенти_ТабличнаЧастина_Файли Файли = new Контрагенти_ТабличнаЧастина_Файли();

        #endregion

        public Контрагенти_Елемент() : base() { }

        protected override void CreatePack1(VBox vBox)
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
        }

        protected override void CreatePack2(VBox vBox)
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
            {
                await Контрагенти_Objest.New();
                Контрагенти_Objest.Папка = РодичДляНового;
            }

            Код.Text = Контрагенти_Objest.Код;
            Назва.Text = Контрагенти_Objest.Назва;
            Родич.Pointer = Контрагенти_Objest.Папка;
            НазваПовна.Buffer.Text = Контрагенти_Objest.НазваПовна;
            РеєстраційнийНомер.Text = Контрагенти_Objest.РеєстраційнийНомер;
            Опис.Buffer.Text = Контрагенти_Objest.Опис;

            Контакти.Контрагенти_Objest = Контрагенти_Objest;
            Контакти.LoadRecords();

            Файли.Контрагенти_Objest = Контрагенти_Objest;
            await Файли.LoadRecords();
        }

        protected override void GetValue()
        {
            Контрагенти_Objest.Код = Код.Text;
            Контрагенти_Objest.Назва = Назва.Text;
            Контрагенти_Objest.Папка = Родич.Pointer;
            Контрагенти_Objest.НазваПовна = НазваПовна.Buffer.Text;
            Контрагенти_Objest.РеєстраційнийНомер = РеєстраційнийНомер.Text;
            Контрагенти_Objest.Опис = Опис.Buffer.Text;
            Контрагенти_Objest.КлючовіСловаДляПошуку = Контакти.КлючовіСловаДляПошуку();
        }

        #endregion

        protected override async ValueTask Save()
        {
            try
            {
                if (await Контрагенти_Objest.Save())
                {
                    await Контакти.SaveRecords();
                    await Файли.SaveRecords();
                }
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }          

            UnigueID = Контрагенти_Objest.UnigueID;
            Caption = Назва.Text;
        }
    }
}