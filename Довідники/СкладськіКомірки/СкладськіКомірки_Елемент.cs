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
    class СкладськіКомірки_Елемент : ДовідникЕлемент
    {
        public СкладськіКомірки_Objest Елемент { get; set; } = new СкладськіКомірки_Objest();
        public СкладськіПриміщення_Pointer СкладськеПриміщенняДляНового { get; set; } = new СкладськіПриміщення_Pointer();
        public СкладськіКомірки_Папки_Pointer РодичДляНового { get; set; } = new СкладськіКомірки_Папки_Pointer();

        Entry Назва = new Entry() { WidthRequest = 500 };
        СкладськіПриміщення_PointerControl СкладськеПриміщення = new СкладськіПриміщення_PointerControl() { Caption = "Приміщення:" };
        СкладськіКомірки_Папки_PointerControl Родич = new СкладськіКомірки_Папки_PointerControl() { Caption = "Папка:" };
        Entry Лінія = new Entry() { WidthRequest = 200 };
        Entry Стелаж = new Entry() { WidthRequest = 200 };
        Entry Позиція = new Entry() { WidthRequest = 200 };
        Entry Ярус = new Entry() { WidthRequest = 200 };
        ComboBoxText ТипСкладськоїКомірки = new ComboBoxText();
        ТипорозміриКомірок_PointerControl Типорозмір = new ТипорозміриКомірок_PointerControl() { Caption = "Типорозмір:" };

        public СкладськіКомірки_Елемент() : base()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;
        }

        protected override void CreatePack1(Box vBox)
        {
            //Назва
            CreateField(vBox, "Назва:", Назва);

            //СкладськеПриміщення
            CreateField(vBox, null, СкладськеПриміщення);

            //Родич
            Родич.BeforeClickOpenFunc = () => Родич.СкладПриміщенняВласник = СкладськеПриміщення.Pointer;
            CreateField(vBox, null, Родич);

            //ТипСкладськоїКомірки
            foreach (var field in ПсевдонімиПерелічення.ТипиСкладськихКомірок_List())
                ТипСкладськоїКомірки.Append(field.Value.ToString(), field.Name);

            CreateField(vBox, "Тип комірки:", ТипСкладськоїКомірки);

            //Типорозмір
            CreateField(vBox, null, Типорозмір);

            // Лінія
            CreateField(vBox, "Лінія:", Лінія);

            // Стелаж
            CreateField(vBox, "Стелаж:", Стелаж);

            // Позиція
            CreateField(vBox, "Позиція:", Позиція);

            // Ярус
            CreateField(vBox, "Ярус:", Ярус);
        }

        protected override void CreatePack2(Box vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
            {
                Елемент.Приміщення = СкладськеПриміщенняДляНового;
                Елемент.Папка = РодичДляНового;
            }

            Назва.Text = Елемент.Назва;
            СкладськеПриміщення.Pointer = Елемент.Приміщення;
            Родич.Pointer = Елемент.Папка;

            ТипСкладськоїКомірки.ActiveId = Елемент.ТипСкладськоїКомірки.ToString();

            if (ТипСкладськоїКомірки.Active == -1)
                ТипСкладськоїКомірки.ActiveId = ТипиСкладськихКомірок.Зберігання.ToString();

            Лінія.Text = Елемент.Лінія;
            Стелаж.Text = Елемент.Стелаж;
            Позиція.Text = Елемент.Позиція;
            Ярус.Text = Елемент.Ярус;

            Типорозмір.Pointer = Елемент.Типорозмір;
        }

        protected override void GetValue()
        {
            Елемент.Назва = Назва.Text;
            Елемент.Приміщення = СкладськеПриміщення.Pointer;
            Елемент.Папка = Родич.Pointer;

            Елемент.ТипСкладськоїКомірки = Enum.Parse<ТипиСкладськихКомірок>(ТипСкладськоїКомірки.ActiveId);

            Елемент.Лінія = Лінія.Text;
            Елемент.Стелаж = Стелаж.Text;
            Елемент.Позиція = Позиція.Text;
            Елемент.Ярус = Ярус.Text;

            Елемент.Типорозмір = Типорозмір.Pointer;
        }

        #endregion

        protected override async ValueTask Save()
        {
            try
            {
                await Елемент.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Елемент.GetBasis(), Caption, ex);
            }
        }
    }
}