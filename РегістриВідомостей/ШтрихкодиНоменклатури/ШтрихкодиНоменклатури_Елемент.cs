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
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.РегістриВідомостей;

namespace StorageAndTrade
{
    class ШтрихкодиНоменклатури_Елемент : РегістриВідомостейЕлемент
    {
        public ШтрихкодиНоменклатури_Objest ШтрихкодиНоменклатури_Objest { get; set; } = new ШтрихкодиНоменклатури_Objest();

        public Номенклатура_Pointer НоменклатураДляНового { get; set; } = new Номенклатура_Pointer();
        public ХарактеристикиНоменклатури_Pointer ХарактеристикаДляНового { get; set; } = new ХарактеристикиНоменклатури_Pointer();

        DateTimeControl ДатаШтрихкоду = new DateTimeControl();
        Entry Штрихкод = new Entry() { WidthRequest = 500 };
        Номенклатура_PointerControl Номенклатура = new Номенклатура_PointerControl();
        ХарактеристикиНоменклатури_PointerControl ХарактеристикаНоменклатури = new ХарактеристикиНоменклатури_PointerControl();
        ПакуванняОдиниціВиміру_PointerControl ПакуванняОдиниціВиміру = new ПакуванняОдиниціВиміру_PointerControl();

        public ШтрихкодиНоменклатури_Елемент() : base() { }

        protected override void CreatePack1(Box vBox)
        {
            //ДатаШтрихкоду
            CreateField(vBox, "Дата:", ДатаШтрихкоду);

            //Штрихкод
            CreateField(vBox, "Штрихкод:", Штрихкод);

            //Номенклатура
            CreateField(vBox, null, Номенклатура);

            //ХарактеристикаНоменклатури
            CreateField(vBox, null, ХарактеристикаНоменклатури);

            //ПакуванняОдиниціВиміру
            CreateField(vBox, null, ПакуванняОдиниціВиміру);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                ШтрихкодиНоменклатури_Objest.New();

                ШтрихкодиНоменклатури_Objest.Номенклатура = НоменклатураДляНового;

                if (!НоменклатураДляНового.IsEmpty())
                {
                    Номенклатура_Objest? Номенклатура_Objest = await НоменклатураДляНового.GetDirectoryObject();
                    if (Номенклатура_Objest != null)
                        ШтрихкодиНоменклатури_Objest.Пакування = Номенклатура_Objest.ОдиницяВиміру;
                }

                ШтрихкодиНоменклатури_Objest.ХарактеристикаНоменклатури = ХарактеристикаДляНового;
            }

            ДатаШтрихкоду.Value = ШтрихкодиНоменклатури_Objest.Period;
            Штрихкод.Text = ШтрихкодиНоменклатури_Objest.Штрихкод;
            Номенклатура.Pointer = ШтрихкодиНоменклатури_Objest.Номенклатура;
            ХарактеристикаНоменклатури.Pointer = ШтрихкодиНоменклатури_Objest.ХарактеристикаНоменклатури;
            ПакуванняОдиниціВиміру.Pointer = ШтрихкодиНоменклатури_Objest.Пакування;
        }

        protected override void GetValue()
        {
            ШтрихкодиНоменклатури_Objest.Period = ДатаШтрихкоду.Value;
            ШтрихкодиНоменклатури_Objest.Штрихкод = Штрихкод.Text;
            ШтрихкодиНоменклатури_Objest.Номенклатура = Номенклатура.Pointer;
            ШтрихкодиНоменклатури_Objest.ХарактеристикаНоменклатури = ХарактеристикаНоменклатури.Pointer;
            ШтрихкодиНоменклатури_Objest.Пакування = ПакуванняОдиниціВиміру.Pointer;
        }

        #endregion

        protected override async ValueTask Save()
        {
            UnigueID = ШтрихкодиНоменклатури_Objest.UnigueID;
            Caption = Штрихкод.Text;

            try
            {
                await ШтрихкодиНоменклатури_Objest.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(new UuidAndText(UnigueID.UGuid), Caption, ex);
            }
        }
    }
}