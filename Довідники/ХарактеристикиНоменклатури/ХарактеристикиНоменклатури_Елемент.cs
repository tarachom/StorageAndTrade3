#region Info

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

#endregion

using Gtk;

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class ХарактеристикиНоменклатури_Елемент : VBox
    {
        public ХарактеристикиНоменклатури? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public ХарактеристикиНоменклатури_Objest ХарактеристикиНоменклатури_Objest { get; set; } = new ХарактеристикиНоменклатури_Objest();
        public Номенклатура_Pointer НоменклатураДляНового { get; set; } = new Номенклатура_Pointer();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        TextView НазваПовна = new TextView();
        Номенклатура_PointerControl Номенклатура = new Номенклатура_PointerControl();

        public ХарактеристикиНоменклатури_Елемент() : base()
        {
            new VBox();
            HBox hBox = new HBox();

            Button bSave = new Button("Зберегти");
            bSave.Clicked += OnSaveClick;

            hBox.PackStart(bSave, false, false, 10);

            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

            hBox.PackStart(bClose, false, false, 10);

            PackStart(hBox, false, false, 10);

            HPaned hPaned = new HPaned() { BorderWidth = 5, Position = 500 };

            CreatePack1(hPaned);
            CreatePack2(hPaned);

            PackStart(hPaned, false, false, 5);

            ShowAll();
        }

        void CreatePack1(HPaned hPaned)
        {
            VBox vBox = new VBox();

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
                ХарактеристикиНоменклатури_Objest.Код = (++НумераціяДовідників.ХарактеристикиНоменклатури_Const).ToString("D6");
                ХарактеристикиНоменклатури_Objest.Номенклатура = НоменклатураДляНового;
            }

            Код.Text = ХарактеристикиНоменклатури_Objest.Код;
            Назва.Text = ХарактеристикиНоменклатури_Objest.Назва;
            НазваПовна.Buffer.Text = ХарактеристикиНоменклатури_Objest.НазваПовна;
            Номенклатура.Pointer = ХарактеристикиНоменклатури_Objest.Номенклатура;
        }

        void GetValue()
        {
            ХарактеристикиНоменклатури_Objest.Код = Код.Text;
            ХарактеристикиНоменклатури_Objest.Назва = Назва.Text;
            ХарактеристикиНоменклатури_Objest.НазваПовна = НазваПовна.Buffer.Text;
            ХарактеристикиНоменклатури_Objest.Номенклатура = Номенклатура.Pointer;
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
            {
                ХарактеристикиНоменклатури_Objest.New();
                IsNew = false;
            }

            GetValue();

            ХарактеристикиНоменклатури_Objest.Save();

            Program.GeneralForm?.RenameCurrentPageNotebook($"Характеристики: {ХарактеристикиНоменклатури_Objest.Назва}");

            if (PageList != null)
            {
                PageList.SelectPointerItem = ХарактеристикиНоменклатури_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}