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

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Банки_Елемент : VBox
    {
        public Банки? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public Банки_Objest Банки_Objest { get; set; } = new Банки_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry КодМФО = new Entry() { WidthRequest = 100 };
        Entry УнікальнийКодБанку = new Entry() { WidthRequest = 100 };
        Entry КодЄДРПОУ = new Entry() { WidthRequest = 100 };

        Entry Назва = new Entry();
        Entry ПовнаНазва = new Entry();
        Entry НазваГоловноїУстановиАнг = new Entry();

        public Банки_Елемент() : base()
        {
            HBox hBox = new HBox();

            Button bSaveAndClose = new Button("Зберегти та закрити");
            bSaveAndClose.Clicked += (object? sender, EventArgs args) => { Save(true); };
            hBox.PackStart(bSaveAndClose, false, false, 10);

            Button bSave = new Button("Зберегти");
            bSave.Clicked += (object? sender, EventArgs args) => { Save(); };
            hBox.PackStart(bSave, false, false, 10);

            PackStart(hBox, false, false, 10);

            CreateTop();

            CreatePack();

            ShowAll();
        }

        void CreateTop()
        {
            VBox vBox = new VBox();

            //Назва
            HBox hBoxName = new HBox();
            vBox.PackStart(hBoxName, false, true, 5);

            hBoxName.PackStart(new Label("Коротка назва:"), false, false, 5);
            hBoxName.PackStart(Назва, true, true, 5);

            //Повна Назва
            HBox hBoxFullName = new HBox();
            vBox.PackStart(hBoxFullName, false, true, 5);

            hBoxFullName.PackStart(new Label("Повна назва:"), false, false, 5);
            hBoxFullName.PackStart(ПовнаНазва, true, true, 5);

            //Назва En
            HBox hBoxNameEn = new HBox();
            vBox.PackStart(hBoxNameEn, false, true, 5);

            hBoxNameEn.PackStart(new Label("Назва англ:"), false, false, 5);
            hBoxNameEn.PackStart(НазваГоловноїУстановиАнг, true, true, 5);

            PackStart(vBox, false, false, 0);
        }

        void CreatePack()
        {
            VBox vBox = new VBox();

            //Два блоки для полів -->
            HBox hBoxContainer = new HBox();
            vBox.PackStart(hBoxContainer, false, false, 0);

            //Container1
            VBox vBoxContainer1 = new VBox() { WidthRequest = 500 };
            hBoxContainer.PackStart(vBoxContainer1, false, false, 5);

            CreateContainer1(vBoxContainer1);

            //Container2
            VBox vBoxContainer2 = new VBox() { WidthRequest = 500 };
            hBoxContainer.PackStart(vBoxContainer2, false, false, 5);

            CreateContainer2(vBoxContainer2);
            // <--

            PackStart(vBox, false, false, 0);
        }

        void CreateContainer1(VBox vBox)
        {
            
        }

        void CreateContainer2(VBox vBox)
        {
            //Код
            HBox hBoxCode = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxCode, false, false, 5);

            hBoxCode.PackStart(new Label("Код:"), false, false, 5);
            hBoxCode.PackStart(Код, false, false, 5);

            //КодМФО
            HBox hBoxCodeMFO = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxCodeMFO, false, false, 5);

            hBoxCodeMFO.PackStart(new Label("Код МФО:"), false, false, 5);
            hBoxCodeMFO.PackStart(КодМФО, false, false, 5);

            //УнікальнийКодБанку
            HBox hBoxUniqueCode = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxUniqueCode, false, false, 5);

            hBoxUniqueCode.PackStart(new Label("Унікальний код:"), false, false, 5);
            hBoxUniqueCode.PackStart(УнікальнийКодБанку, false, false, 5);

            //КодЄДРПОУ
            HBox hBoxCodeEDRPUO = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxCodeEDRPUO, false, false, 5);

            hBoxCodeEDRPUO.PackStart(new Label("Код ЄДРПОУ:"), false, false, 5);
            hBoxCodeEDRPUO.PackStart(КодЄДРПОУ, false, false, 5);
        }

        #region Присвоєння / зчитування значень

        public void SetValue()
        {
            if (IsNew)
                Банки_Objest.Код = (++НумераціяДовідників.Банки_Const).ToString("D6");

            Код.Text = Банки_Objest.Код;
            Назва.Text = Банки_Objest.Назва;
            ПовнаНазва.Text = Банки_Objest.ПовнаНазва;
            НазваГоловноїУстановиАнг.Text = Банки_Objest.НазваГоловноїУстановиАнг;
            КодМФО.Text = Банки_Objest.КодМФО;
            УнікальнийКодБанку.Text = Банки_Objest.УнікальнийКодБанку;
            КодЄДРПОУ.Text = Банки_Objest.КодЄДРПОУ;
        }

        void GetValue()
        {
            Банки_Objest.Код = Код.Text;
            Банки_Objest.Назва = Назва.Text;
        }

        #endregion

        void Save(bool closePage = false)
        {
            if (IsNew)
            {
                Банки_Objest.New();
                IsNew = false;
            }

            GetValue();

            Банки_Objest.Save();

            if (closePage)
                Program.GeneralForm?.CloseCurrentPageNotebook();
            else
                Program.GeneralForm?.RenameCurrentPageNotebook($"Валюта: {Банки_Objest.Назва}");

            if (PageList != null)
            {
                PageList.SelectPointerItem = Банки_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}