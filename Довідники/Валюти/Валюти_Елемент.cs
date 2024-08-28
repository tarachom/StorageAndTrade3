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
    class Валюти_Елемент : ДовідникЕлемент
    {
        public Валюти_Objest Валюти_Objest { get; set; } = new Валюти_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Entry КороткаНазва = new Entry() { WidthRequest = 500 };
        Entry Код_R030 = new Entry() { WidthRequest = 500 };
        CheckButton ВиводитиКурсНаСтартову = new CheckButton("Виводити курс на стартову");

        public Валюти_Елемент() : base() { }

        protected override void CreatePack1(Box vBox)
        {
            //Код
            CreateField(vBox, "Код:", Код);

            //Назва
            CreateField(vBox, "Назва:", Назва);

            //КороткаНазва
            CreateField(vBox, "Коротка назва:", КороткаНазва);

            //Код_R030
            CreateField(vBox, "Код R030:", Код_R030);

            //ВиводитиКурсНаСтартову
            CreateField(vBox, null, ВиводитиКурсНаСтартову);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
                await Валюти_Objest.New();

            Код.Text = Валюти_Objest.Код;
            Назва.Text = Валюти_Objest.Назва;
            КороткаНазва.Text = Валюти_Objest.КороткаНазва;
            Код_R030.Text = Валюти_Objest.Код_R030;
            ВиводитиКурсНаСтартову.Active = Валюти_Objest.ВиводитиКурсНаСтартову;
        }

        protected override void GetValue()
        {
            Валюти_Objest.Код = Код.Text;
            Валюти_Objest.Назва = Назва.Text;
            Валюти_Objest.КороткаНазва = КороткаНазва.Text;
            Валюти_Objest.Код_R030 = Код_R030.Text;
            Валюти_Objest.ВиводитиКурсНаСтартову = ВиводитиКурсНаСтартову.Active;
        }

        #endregion

        protected override async ValueTask Save()
        {
            UnigueID = Валюти_Objest.UnigueID;
            Caption = Валюти_Objest.Назва;

            try
            {
                await Валюти_Objest.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Валюти_Objest.GetBasis(), Caption, ex);
            }
        }
    }
}