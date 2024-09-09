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
    class Блокнот_Елемент : ДовідникЕлемент
    {
        public Блокнот_Objest Елемент { get; set; } = new Блокнот_Objest();

        #region Fields

        Entry Код = new Entry() { WidthRequest = 500 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        DateTimeControl ДатаЗапису = new DateTimeControl();
        TextView Опис = new TextView() { WidthRequest = 500 };
        Entry Лінк = new Entry() { WidthRequest = 500 };

        #endregion

        #region TabularParts

        #endregion

        public Блокнот_Елемент()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;
        }

        protected override void CreatePack1(Box vBox)
        {

            CreateField(vBox, "Код:", Код);

            CreateField(vBox, "Назва:", Назва);

            CreateField(vBox, "ДатаЗапису:", ДатаЗапису);

            CreateFieldView(vBox, "Опис:", Опис, 800, 500);

            CreateField(vBox, "Лінк:", Лінк);

        }

        protected override void CreatePack2(Box vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            Код.Text = Елемент.Код;
            Назва.Text = Елемент.Назва;
            ДатаЗапису.Value = Елемент.ДатаЗапису;
            Опис.Buffer.Text = Елемент.Опис;
            Лінк.Text = Елемент.Лінк;
        }

        protected override void GetValue()
        {
            Елемент.Код = Код.Text;
            Елемент.Назва = Назва.Text;
            Елемент.ДатаЗапису = ДатаЗапису.Value;
            Елемент.Опис = Опис.Buffer.Text;
            Елемент.Лінк = Лінк.Text;
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            try
            {
                return await Елемент.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Елемент.GetBasis(), Caption, ex);
                return false;
            }
        }
    }
}
