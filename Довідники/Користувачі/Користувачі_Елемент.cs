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
    class Користувачі_Елемент : ДовідникЕлемент
    {
        public Користувачі_Objest Користувачі_Objest { get; set; } = new Користувачі_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        ФізичніОсоби_PointerControl ФізичнаОсоба = new ФізичніОсоби_PointerControl() { Caption = "Фізична особа:" };
        TextView Коментар = new TextView();

        public Користувачі_Елемент() : base() 
        {
            Користувачі_Objest.UnigueIDChanged += UnigueIDChanged;
            Користувачі_Objest.CaptionChanged += CaptionChanged;
        }

        protected override void CreatePack1(Box vBox)
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
            Код.Text = Користувачі_Objest.Код;
            Назва.Text = Користувачі_Objest.Назва;
            ФізичнаОсоба.Pointer = Користувачі_Objest.ФізичнаОсоба;
            Коментар.Buffer.Text = Користувачі_Objest.Коментар;
        }

        protected override void GetValue()
        {
            Користувачі_Objest.Код = Код.Text;
            Користувачі_Objest.Назва = Назва.Text;
            Користувачі_Objest.ФізичнаОсоба = ФізичнаОсоба.Pointer;
            Користувачі_Objest.Коментар = Коментар.Buffer.Text;
        }

        #endregion

        protected override async ValueTask Save()
        {
            try
            {
                await Користувачі_Objest.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Користувачі_Objest.GetBasis(), Caption, ex);
            }            
        }
    }
}