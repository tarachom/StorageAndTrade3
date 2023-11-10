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
    class Контрагенти_Папки_Елемент : ДовідникЕлемент
    {
        public Контрагенти_Папки_Objest Контрагенти_Папки_Objest { get; set; } = new Контрагенти_Папки_Objest();
        public Контрагенти_Папки_Pointer РодичДляНового { get; set; } = new Контрагенти_Папки_Pointer();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Контрагенти_Папки_PointerControl Родич = new Контрагенти_Папки_PointerControl() { Caption = "Родич:" };

        public Контрагенти_Папки_Елемент() : base() { }

        protected override void CreatePack1(VBox vBox)
        {
            //Код
            CreateField(vBox, "Код:", Код);

            //Назва
            CreateField(vBox, "Назва:", Назва);

            //Родич
            CreateField(vBox, null, Родич);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
            {
                Контрагенти_Папки_Objest.New();
                Контрагенти_Папки_Objest.Родич = РодичДляНового;
            }
            else
                Родич.OpenFolder = Контрагенти_Папки_Objest.UnigueID;

            Код.Text = Контрагенти_Папки_Objest.Код;
            Назва.Text = Контрагенти_Папки_Objest.Назва;
            Родич.Pointer = Контрагенти_Папки_Objest.Родич;
        }

        protected override void GetValue()
        {
            Контрагенти_Папки_Objest.Код = Код.Text;
            Контрагенти_Папки_Objest.Назва = Назва.Text;
            Контрагенти_Папки_Objest.Родич = Родич.Pointer;
        }

        #endregion

        protected override async ValueTask Save()
        {
            UnigueID = Контрагенти_Папки_Objest.UnigueID;
            Caption = Назва.Text;

            try
            {
                await Контрагенти_Папки_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }            
        }
    }
}