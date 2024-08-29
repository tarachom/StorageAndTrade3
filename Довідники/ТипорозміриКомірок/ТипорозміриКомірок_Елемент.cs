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
    class ТипорозміриКомірок_Елемент : ДовідникЕлемент
    {
        public ТипорозміриКомірок_Objest Елемент { get; set; } = new ТипорозміриКомірок_Objest();

        Entry Назва = new Entry() { WidthRequest = 250 };
        Entry Висота = new Entry() { WidthRequest = 100 };
        Entry Ширина = new Entry() { WidthRequest = 100 };
        Entry Глибина = new Entry() { WidthRequest = 100 };
        Entry Обєм = new Entry() { WidthRequest = 100 };
        Entry Вантажопідйомність = new Entry() { WidthRequest = 100 };

        public ТипорозміриКомірок_Елемент() : base()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;

            HPanedTop.Position = 150;
        }

        protected override void CreatePack1(Box vBox)
        {
            //Назва
            CreateField(vBox, "Назва:", Назва);

            //Висота
            CreateField(vBox, "Висота:", Висота);

            //Ширина
            CreateField(vBox, "Ширина:", Ширина);

            //Глибина
            CreateField(vBox, "Глибина:", Глибина);

            //Обєм
            CreateField(vBox, "Об'єм:", Обєм);

            //Вантажопідйомність
            CreateField(vBox, "Вантажопідйомність:", Вантажопідйомність);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            Назва.Text = Елемент.Назва;
            Висота.Text = Елемент.Висота;
            Ширина.Text = Елемент.Ширина;
            Глибина.Text = Елемент.Глибина;
            Обєм.Text = Елемент.Обєм;
            Вантажопідйомність.Text = Елемент.Вантажопідйомність;
        }

        protected override void GetValue()
        {
            Елемент.Назва = Назва.Text;
            Елемент.Висота = Висота.Text;
            Елемент.Ширина = Ширина.Text;
            Елемент.Глибина = Глибина.Text;
            Елемент.Обєм = Обєм.Text;
            Елемент.Вантажопідйомність = Вантажопідйомність.Text;
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