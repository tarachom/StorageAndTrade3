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
    class ТипорозміриКомірок_Елемент : ДовідникЕлемент
    {
        public ТипорозміриКомірок_Objest ТипорозміриКомірок_Objest { get; set; } = new ТипорозміриКомірок_Objest();

        Entry Назва = new Entry() { WidthRequest = 250 };
        Entry Висота = new Entry() { WidthRequest = 100 };
        Entry Ширина = new Entry() { WidthRequest = 100 };
        Entry Глибина = new Entry() { WidthRequest = 100 };
        Entry Обєм = new Entry() { WidthRequest = 100 };
        Entry Вантажопідйомність = new Entry() { WidthRequest = 100 };

        public ТипорозміриКомірок_Елемент() : base()
        {
            HPanedTop.Position = 150;
        }

        protected override void CreatePack1(VBox vBox)
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

        public override async void SetValue()
        {
            if (IsNew)
                await ТипорозміриКомірок_Objest.New();

            Назва.Text = ТипорозміриКомірок_Objest.Назва;
            Висота.Text = ТипорозміриКомірок_Objest.Висота;
            Ширина.Text = ТипорозміриКомірок_Objest.Ширина;
            Глибина.Text = ТипорозміриКомірок_Objest.Глибина;
            Обєм.Text = ТипорозміриКомірок_Objest.Обєм;
            Вантажопідйомність.Text = ТипорозміриКомірок_Objest.Вантажопідйомність;
        }

        protected override void GetValue()
        {
            ТипорозміриКомірок_Objest.Назва = Назва.Text;
            ТипорозміриКомірок_Objest.Висота = Висота.Text;
            ТипорозміриКомірок_Objest.Ширина = Ширина.Text;
            ТипорозміриКомірок_Objest.Глибина = Глибина.Text;
            ТипорозміриКомірок_Objest.Обєм = Обєм.Text;
            ТипорозміриКомірок_Objest.Вантажопідйомність = Вантажопідйомність.Text;
        }

        #endregion

        protected override async ValueTask Save()
        {
            try
            {
                await ТипорозміриКомірок_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }

            UnigueID = ТипорозміриКомірок_Objest.UnigueID;
            Caption = Назва.Text;
        }
    }
}