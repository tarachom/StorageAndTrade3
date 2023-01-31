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
    class Валюти_Елемент : VBox
    {
        public Валюти? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public Валюти_Objest Валюти_Objest { get; set; } = new Валюти_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Entry КороткаНазва = new Entry() { WidthRequest = 500 };
        Entry Код_R030 = new Entry() { WidthRequest = 500 };
        CheckButton ВиводитиКурсНаСтартову = new CheckButton("Виводити курс на стартову");

        public Валюти_Елемент() : base()
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

            //КороткаНазва
            HBox hBoxSmallName = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSmallName, false, false, 5);

            hBoxSmallName.PackStart(new Label("Коротка назва:"), false, false, 5);
            hBoxSmallName.PackStart(КороткаНазва, false, false, 5);

            //Код_R030
            HBox hBoxCodeR030 = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxCodeR030, false, false, 5);

            hBoxCodeR030.PackStart(new Label("Код R030:"), false, false, 5);
            hBoxCodeR030.PackStart(Код_R030, false, false, 5);

            //ВиводитиКурсНаСтартову
            HBox hBoxVisibleCursOnStartPage = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxVisibleCursOnStartPage, false, false, 5);

            hBoxVisibleCursOnStartPage.PackStart(ВиводитиКурсНаСтартову, false, false, 5);

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
                Валюти_Objest.Код = (++НумераціяДовідників.Валюти_Const).ToString("D6");

            Код.Text = Валюти_Objest.Код;
            Назва.Text = Валюти_Objest.Назва;
            КороткаНазва.Text = Валюти_Objest.КороткаНазва;
            Код_R030.Text = Валюти_Objest.Код_R030;
            ВиводитиКурсНаСтартову.Active = Валюти_Objest.ВиводитиКурсНаСтартову;
        }

        void GetValue()
        {
            Валюти_Objest.Код = Код.Text;
            Валюти_Objest.Назва = Назва.Text;
            Валюти_Objest.КороткаНазва = КороткаНазва.Text;
            Валюти_Objest.Код_R030 = Код_R030.Text;
            Валюти_Objest.ВиводитиКурсНаСтартову = ВиводитиКурсНаСтартову.Active;
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
            {
                Валюти_Objest.New();
                IsNew = false;
            }

            GetValue();

            Валюти_Objest.Save();

            Program.GeneralForm?.RenameCurrentPageNotebook($"Валюта: {Валюти_Objest.Назва}");

            if (PageList != null)
            {
                PageList.SelectPointerItem = Валюти_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}