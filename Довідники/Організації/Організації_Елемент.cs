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
    class Організації_Елемент : VBox
    {
        public Організації? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public Організації_Objest Організації_Objest { get; set; } = new Організації_Objest();

        #region Field

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Entry НазваСкорочена = new Entry() { WidthRequest = 500 };
        TextView НазваПовна = new TextView();
        DateTimeControl ДатаРеєстрації = new DateTimeControl() { OnlyDate = true };
        Entry КраїнаРеєстрації = new Entry() { WidthRequest = 300 };
        Entry СвідоцтвоСеріяНомер = new Entry() { WidthRequest = 300 };
        Entry СвідоцтвоДатаВидачі = new Entry() { WidthRequest = 300 };
        Організації_PointerControl Холдинг = new Організації_PointerControl() { Caption = "Холдинг:" };

        Організації_ТабличнаЧастина_Контакти Контакти = new Організації_ТабличнаЧастина_Контакти();

        #endregion

        public Організації_Елемент() : base()
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

            //Назва
            HBox hBoxName = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxName, false, false, 5);

            hBoxName.PackStart(new Label("Назва:"), false, false, 5);
            hBoxName.PackStart(Назва, false, false, 5);

            //НазваСкорочена
            HBox hBoxSmallName = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSmallName, false, false, 5);

            hBoxSmallName.PackStart(new Label("Назва скорочена:"), false, false, 5);
            hBoxSmallName.PackStart(НазваСкорочена, false, false, 5);

            //НазваПовна
            HBox hBoxDesc = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxDesc, false, false, 5);

            hBoxDesc.PackStart(new Label("Повна назва:") { Valign = Align.Start }, false, false, 5);

            ScrolledWindow scrollTextView = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = 500, HeightRequest = 100 };
            scrollTextView.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrollTextView.Add(НазваПовна);

            hBoxDesc.PackStart(scrollTextView, false, false, 5);

            //ДатаРеєстрації
            HBox hBoxDateRegister = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxDateRegister, false, false, 5);

            hBoxDateRegister.PackStart(new Label("Дата реєстрації:"), false, false, 5);
            hBoxDateRegister.PackStart(ДатаРеєстрації, false, false, 5);

            //ДатаРеєстрації
            HBox hBoxCountryRegister = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxCountryRegister, false, false, 5);

            hBoxCountryRegister.PackStart(new Label("Країна реєстрації:"), false, false, 5);
            hBoxCountryRegister.PackStart(КраїнаРеєстрації, false, false, 5);

            //СвідоцтвоСеріяНомер
            HBox hBoxSvidotstvo = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSvidotstvo, false, false, 5);

            hBoxSvidotstvo.PackStart(new Label("Свідоцтво серія номер:"), false, false, 5);
            hBoxSvidotstvo.PackStart(СвідоцтвоСеріяНомер, false, false, 5);

            //СвідоцтвоДатаВидачі
            HBox hBoxSvidotstvoData = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSvidotstvoData, false, false, 5);

            hBoxSvidotstvoData.PackStart(new Label("Свідоцтво дата видачі:"), false, false, 5);
            hBoxSvidotstvoData.PackStart(СвідоцтвоДатаВидачі, false, false, 5);

            hPaned.Pack1(vBox, false, false);

            //Холдинг
            HBox hBoxHolding = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxHolding, false, false, 5);

            hBoxHolding.PackStart(Холдинг, false, false, 3);
        }

        void CreatePack2(HPaned hPaned)
        {
            VBox vBox = new VBox();

            HBox hBox = new HBox();

            hBox.PackStart(new Label("Контакти:"), false, false, 5);
            vBox.PackStart(hBox, false, false, 5);

            HBox hBoxContakty = new HBox();
            hBoxContakty.PackStart(Контакти, true, true, 5);

            vBox.PackStart(hBoxContakty, false, false, 0);
            hPaned.Pack2(vBox, false, false);
        }

        #region Присвоєння / зчитування значень

        public void SetValue()
        {
            if (IsNew)
                Організації_Objest.Код = (++НумераціяДовідників.Організації_Const).ToString("D6");

            Код.Text = Організації_Objest.Код;
            Назва.Text = Організації_Objest.Назва;
            НазваСкорочена.Text = Організації_Objest.НазваСкорочена;
            ДатаРеєстрації.Value = Організації_Objest.ДатаРеєстрації;
            КраїнаРеєстрації.Text = Організації_Objest.КраїнаРеєстрації;
            СвідоцтвоСеріяНомер.Text = Організації_Objest.СвідоцтвоСеріяНомер;
            СвідоцтвоДатаВидачі.Text = Організації_Objest.СвідоцтвоДатаВидачі;
            НазваПовна.Buffer.Text = Організації_Objest.НазваПовна;
            Холдинг.Pointer = Організації_Objest.Холдинг;

            Контакти.Організації_Objest = Організації_Objest;
            Контакти.LoadRecords();
        }

        void GetValue()
        {
            Організації_Objest.Код = Код.Text;
            Організації_Objest.Назва = Назва.Text;
            Організації_Objest.НазваСкорочена = НазваСкорочена.Text;
            Організації_Objest.ДатаРеєстрації = ДатаРеєстрації.Value;
            Організації_Objest.КраїнаРеєстрації = КраїнаРеєстрації.Text;
            Організації_Objest.СвідоцтвоСеріяНомер = СвідоцтвоСеріяНомер.Text;
            Організації_Objest.СвідоцтвоДатаВидачі = СвідоцтвоДатаВидачі.Text;
            Організації_Objest.НазваПовна = НазваПовна.Buffer.Text;
            Організації_Objest.Холдинг = Холдинг.Pointer;
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
            {
                Організації_Objest.New();
                IsNew = false;
            }

            GetValue();

            Організації_Objest.Save();
            Контакти.SaveRecords();

            Program.GeneralForm?.RenameCurrentPageNotebook($"Організація: {Організації_Objest.Назва}");

            if (PageList != null)
            {
                PageList.SelectPointerItem = Організації_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}