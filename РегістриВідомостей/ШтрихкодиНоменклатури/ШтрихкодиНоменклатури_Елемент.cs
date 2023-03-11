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
using StorageAndTrade_1_0.РегістриВідомостей;

namespace StorageAndTrade
{
    class ШтрихкодиНоменклатури_Елемент : VBox
    {
        public ШтрихкодиНоменклатури? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public ШтрихкодиНоменклатури_Objest ШтрихкодиНоменклатури_Objest { get; set; } = new ШтрихкодиНоменклатури_Objest();

        public Номенклатура_Pointer НоменклатураДляНового { get; set; } = new Номенклатура_Pointer();
        public ХарактеристикиНоменклатури_Pointer ХарактеристикаДляНового { get; set; } = new ХарактеристикиНоменклатури_Pointer();

        Entry Штрихкод = new Entry() { WidthRequest = 500 };
        Номенклатура_PointerControl Номенклатура = new Номенклатура_PointerControl();
        ХарактеристикиНоменклатури_PointerControl ХарактеристикаНоменклатури = new ХарактеристикиНоменклатури_PointerControl();
        ПакуванняОдиниціВиміру_PointerControl ПакуванняОдиниціВиміру = new ПакуванняОдиниціВиміру_PointerControl();

        public ШтрихкодиНоменклатури_Елемент() : base()
        {
            HBox hBox = new HBox();
            PackStart(hBox, false, false, 10);

            Button bSave = new Button("Зберегти");
            bSave.Clicked += OnSaveClick;

            hBox.PackStart(bSave, false, false, 10);

            HPaned hPaned = new HPaned() { BorderWidth = 5, Position = 500 };

            CreatePack1(hPaned);
            CreatePack2(hPaned);

            PackStart(hPaned, false, false, 5);

            ShowAll();
        }

        void CreatePack1(HPaned hPaned)
        {
            VBox vBox = new VBox();

            //Штрихкод
            HBox hBoxShKod = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxShKod, false, false, 5);

            hBoxShKod.PackStart(new Label("Штрихкод:"), false, false, 5);
            hBoxShKod.PackStart(Штрихкод, false, false, 5);

            //Номенклатура
            HBox hBoxNomenklatura = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxNomenklatura, false, false, 5);

            hBoxNomenklatura.PackStart(Номенклатура, false, false, 5);

            //ХарактеристикаНоменклатури
            HBox hBoHarakteristyka = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoHarakteristyka, false, false, 5);

            hBoHarakteristyka.PackStart(ХарактеристикаНоменклатури, false, false, 5);

            //ПакуванняОдиниціВиміру
            HBox hBoxPak = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxPak, false, false, 5);

            hBoxPak.PackStart(ПакуванняОдиниціВиміру, false, false, 5);

            hPaned.Pack1(vBox, false, false);
        }

        void CreatePack2(HPaned hPaned)
        {
            VBox vBox = new VBox();



            hPaned.Pack2(vBox, false, false);
        }

        #region Присвоєння / зчитування значень

        public void SetValue()
        {
            if (IsNew)
            {
                ШтрихкодиНоменклатури_Objest.Номенклатура = НоменклатураДляНового;

                if (!НоменклатураДляНового.IsEmpty())
                {
                    Номенклатура_Objest? Номенклатура_Objest = НоменклатураДляНового.GetDirectoryObject();
                    if (Номенклатура_Objest != null)
                        ШтрихкодиНоменклатури_Objest.Пакування = Номенклатура_Objest.ОдиницяВиміру;
                }

                ШтрихкодиНоменклатури_Objest.ХарактеристикаНоменклатури = ХарактеристикаДляНового;
            }

            Штрихкод.Text = ШтрихкодиНоменклатури_Objest.Штрихкод;
            Номенклатура.Pointer = ШтрихкодиНоменклатури_Objest.Номенклатура;
            ХарактеристикаНоменклатури.Pointer = ШтрихкодиНоменклатури_Objest.ХарактеристикаНоменклатури;
            ПакуванняОдиниціВиміру.Pointer = ШтрихкодиНоменклатури_Objest.Пакування;
        }

        void GetValue()
        {
            ШтрихкодиНоменклатури_Objest.Штрихкод = Штрихкод.Text;
            ШтрихкодиНоменклатури_Objest.Номенклатура = Номенклатура.Pointer;
            ШтрихкодиНоменклатури_Objest.ХарактеристикаНоменклатури = ХарактеристикаНоменклатури.Pointer;
            ШтрихкодиНоменклатури_Objest.Пакування = ПакуванняОдиниціВиміру.Pointer;
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
            {
                ШтрихкодиНоменклатури_Objest.New();
                IsNew = false;
            }

            GetValue();

            ШтрихкодиНоменклатури_Objest.Save();

            Program.GeneralForm?.RenameCurrentPageNotebook($"Штрихкод: {ШтрихкодиНоменклатури_Objest.Штрихкод}");

            if (PageList != null)
            {
                PageList.LoadRecords();
            }
        }
    }
}