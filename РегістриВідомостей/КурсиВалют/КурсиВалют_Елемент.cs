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
    class КурсиВалют_Елемент : VBox
    {
        public КурсиВалют? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public КурсиВалют_Objest КурсиВалют_Objest { get; set; } = new КурсиВалют_Objest();

        public Валюти_Pointer ВалютаДляНового { get; set; } = new Валюти_Pointer();

        DateTimeControl ДатаКурсу = new DateTimeControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();
        NumericControl Курс = new NumericControl();
        IntegerControl Кратність = new IntegerControl();

        public КурсиВалют_Елемент() : base()
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

            //ДатаКурсу
            HBox hBoxDateKurs = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxDateKurs, false, false, 5);

            hBoxDateKurs.PackStart(new Label("Дата:"), false, false, 5);
            hBoxDateKurs.PackStart(ДатаКурсу, false, false, 5);

            //Валюта
            HBox hBoxValuta = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxValuta, false, false, 5);

            hBoxValuta.PackStart(Валюта, false, false, 5);

            //Курс
            HBox hBoxKurs = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxKurs, false, false, 5);

            hBoxKurs.PackStart(new Label("Курс:"), false, false, 5);
            hBoxKurs.PackStart(Курс, false, false, 5);

            //Кратність
            HBox hBoxKrat = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxKrat, false, false, 5);

            hBoxKrat.PackStart(new Label("Кратність:"), false, false, 5);
            hBoxKrat.PackStart(Кратність, false, false, 5);

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
                ДатаКурсу.Value = DateTime.Now;
                КурсиВалют_Objest.Валюта = ВалютаДляНового;
                КурсиВалют_Objest.Кратність = 1;
            }

            ДатаКурсу.Value = КурсиВалют_Objest.Period;
            Валюта.Pointer = КурсиВалют_Objest.Валюта;
            Курс.Value = КурсиВалют_Objest.Курс;
            Кратність.Value = КурсиВалют_Objest.Кратність;
        }

        void GetValue()
        {
            КурсиВалют_Objest.Period = ДатаКурсу.Value;
            КурсиВалют_Objest.Валюта = Валюта.Pointer;
            КурсиВалют_Objest.Курс = Курс.Value;
            КурсиВалют_Objest.Кратність = Кратність.Value;
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
            {
                КурсиВалют_Objest.New();
                IsNew = false;
            }

            GetValue();

            КурсиВалют_Objest.Save();

            Program.GeneralForm?.RenameCurrentPageNotebook($"Курс: {КурсиВалют_Objest.Курс}");

            if (PageList != null)
            {
                PageList.LoadRecords();
            }
        }
    }
}