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
    class ТипорозміриКомірок_Елемент : VBox
    {
        public ТипорозміриКомірок? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public ТипорозміриКомірок_Objest ТипорозміриКомірок_Objest { get; set; } = new ТипорозміриКомірок_Objest();

        Entry Назва = new Entry() { WidthRequest = 250 };
        Entry Висота = new Entry() { WidthRequest = 100 };
        Entry Ширина = new Entry() { WidthRequest = 100 };
        Entry Глибина = new Entry() { WidthRequest = 100 };
        Entry Обєм = new Entry() { WidthRequest = 100 };
        Entry Вантажопідйомність = new Entry() { WidthRequest = 100 };

        public ТипорозміриКомірок_Елемент() : base()
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

            HPaned hPaned = new HPaned() { BorderWidth = 5, Position = 150 };

            CreatePack1(hPaned);
            CreatePack2(hPaned);

            PackStart(hPaned, false, false, 5);

            ShowAll();
        }

        void CreatePack1(HPaned hPaned)
        {
            VBox vBox = new VBox();

            //Назва
            HBox hBoxName = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxName, false, false, 5);

            hBoxName.PackStart(new Label("Назва:"), false, false, 5);
            hBoxName.PackStart(Назва, false, false, 5);

            //Висота
            HBox hBoxVisota = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxVisota, false, false, 5);

            hBoxVisota.PackStart(new Label("Висота:"), false, false, 5);
            hBoxVisota.PackStart(Висота, false, false, 5);

            //Ширина
            HBox hBoxShyryna = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxShyryna, false, false, 5);

            hBoxShyryna.PackStart(new Label("Ширина:"), false, false, 5);
            hBoxShyryna.PackStart(Ширина, false, false, 5);

            //Глибина
            HBox hBoxGlybyna = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxGlybyna, false, false, 5);

            hBoxGlybyna.PackStart(new Label("Глибина:"), false, false, 5);
            hBoxGlybyna.PackStart(Глибина, false, false, 5);

            //Обєм
            HBox hBoxObjem = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxObjem, false, false, 5);

            hBoxObjem.PackStart(new Label("Обєм:"), false, false, 5);
            hBoxObjem.PackStart(Обєм, false, false, 5);

            //Вантажопідйомність
            HBox hBoxVantaj = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxVantaj, false, false, 5);

            hBoxVantaj.PackStart(new Label("Вантажопідйомність:"), false, false, 5);
            hBoxVantaj.PackStart(Вантажопідйомність, false, false, 5);

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
            Назва.Text = ТипорозміриКомірок_Objest.Назва;
            Висота.Text = ТипорозміриКомірок_Objest.Висота;
            Ширина.Text = ТипорозміриКомірок_Objest.Ширина;
            Глибина.Text = ТипорозміриКомірок_Objest.Глибина;
            Обєм.Text = ТипорозміриКомірок_Objest.Обєм;
            Вантажопідйомність.Text = ТипорозміриКомірок_Objest.Вантажопідйомність;
        }

        void GetValue()
        {
            ТипорозміриКомірок_Objest.Назва = Назва.Text;
            ТипорозміриКомірок_Objest.Висота = Висота.Text;
            ТипорозміриКомірок_Objest.Ширина = Ширина.Text;
            ТипорозміриКомірок_Objest.Глибина = Глибина.Text;
            ТипорозміриКомірок_Objest.Обєм = Обєм.Text;
            ТипорозміриКомірок_Objest.Вантажопідйомність = Вантажопідйомність.Text;
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
            {
                ТипорозміриКомірок_Objest.New();
                IsNew = false;
            }

            GetValue();

            ТипорозміриКомірок_Objest.Save();

            Program.GeneralForm?.RenameCurrentPageNotebook($"Типорозмір: {ТипорозміриКомірок_Objest.Назва}");

            if (PageList != null)
            {
                PageList.SelectPointerItem = ТипорозміриКомірок_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}