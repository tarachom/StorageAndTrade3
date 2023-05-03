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
    class Користувачі_Елемент : ДовідникЕлемент
    {
        public Користувачі_Objest Користувачі_Objest { get; set; } = new Користувачі_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        ФізичніОсоби_PointerControl ФізичнаОсоба = new ФізичніОсоби_PointerControl() { Caption = "Фізична особа:" };
        TextView Коментар = new TextView();

        public Користувачі_Елемент() : base() { }

        protected override void CreatePack1(VBox vBox)
        {
            //Код
            CreateField(vBox, "Код:", Код);

            //Назва
            CreateField(vBox, "Назва:", Назва);

            //ФізичнаОсоба
            CreateField(vBox, null, ФізичнаОсоба);

            //Коментар
            CreateFieldView(vBox, "Коментар:", Коментар, 500, 200);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
                Користувачі_Objest.New();

            Код.Text = Користувачі_Objest.Код;
            Назва.Text = Користувачі_Objest.Назва;
            ФізичнаОсоба.Pointer = Користувачі_Objest.ФізичнаОсоба;
            Коментар.Buffer.Text = Користувачі_Objest.Коментар;
        }

        protected override void GetValue()
        {
            UnigueID = Користувачі_Objest.UnigueID;
            Caption = Назва.Text;

            Користувачі_Objest.Код = Код.Text;
            Користувачі_Objest.Назва = Назва.Text;
            Користувачі_Objest.ФізичнаОсоба = ФізичнаОсоба.Pointer;
            Користувачі_Objest.Коментар = Коментар.Buffer.Text;
        }

        #endregion

        protected override void Save()
        {
            try
            {
                Користувачі_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }
        }
    }
}