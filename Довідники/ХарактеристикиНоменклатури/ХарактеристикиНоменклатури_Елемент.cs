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
    class ХарактеристикиНоменклатури_Елемент : ДовідникЕлемент
    {
        public ХарактеристикиНоменклатури_Objest ХарактеристикиНоменклатури_Objest { get; set; } = new ХарактеристикиНоменклатури_Objest();
        public Номенклатура_Pointer НоменклатураДляНового { get; set; } = new Номенклатура_Pointer();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        TextView НазваПовна = new TextView();
        Номенклатура_PointerControl Номенклатура = new Номенклатура_PointerControl();

        public ХарактеристикиНоменклатури_Елемент() : base() { }

        protected override void CreatePack1()
        {
            VBox vBox = new VBox();
            HPanedTop.Pack1(vBox, false, false);

            //Код
            HBox hBoxCode = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxCode, false, false, 5);

            hBoxCode.PackStart(new Label("Код:"), false, false, 5);
            hBoxCode.PackStart(Код, false, false, 5);

            //Номенклатура
            HBox hBoxNomenklatura = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxNomenklatura, false, false, 5);

            hBoxNomenklatura.PackStart(Номенклатура, false, false, 5);

            //Назва
            HBox hBoxName = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxName, false, false, 5);

            hBoxName.PackStart(new Label("Назва:"), false, false, 5);
            hBoxName.PackStart(Назва, false, false, 5);

            //НазваПовна
            HBox hBoxDesc = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxDesc, false, false, 5);

            hBoxDesc.PackStart(new Label("Повна назва:") { Valign = Align.Start }, false, false, 5);

            ScrolledWindow scrollTextView = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = 500, HeightRequest = 100 };
            scrollTextView.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrollTextView.Add(НазваПовна);

            hBoxDesc.PackStart(scrollTextView, false, false, 5);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
            {
                ХарактеристикиНоменклатури_Objest.New();
                ХарактеристикиНоменклатури_Objest.Номенклатура = НоменклатураДляНового;
            }

            Код.Text = ХарактеристикиНоменклатури_Objest.Код;
            Назва.Text = ХарактеристикиНоменклатури_Objest.Назва;
            НазваПовна.Buffer.Text = ХарактеристикиНоменклатури_Objest.НазваПовна;
            Номенклатура.Pointer = ХарактеристикиНоменклатури_Objest.Номенклатура;
        }

        protected override void GetValue()
        {
            UnigueID = ХарактеристикиНоменклатури_Objest.UnigueID;
            Caption = Назва.Text;

            ХарактеристикиНоменклатури_Objest.Код = Код.Text;
            ХарактеристикиНоменклатури_Objest.Назва = Назва.Text;
            ХарактеристикиНоменклатури_Objest.НазваПовна = НазваПовна.Buffer.Text;
            ХарактеристикиНоменклатури_Objest.Номенклатура = Номенклатура.Pointer;
        }

        #endregion

        protected override void Save()
        {
            try
            {
                ХарактеристикиНоменклатури_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }
        }
    }
}