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

using AccountingSoftware;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Довідники;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class СкладськіКомірки_Елемент : VBox
    {
        public СкладськіКомірки? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public СкладськіКомірки_Objest СкладськіКомірки_Objest { get; set; } = new СкладськіКомірки_Objest();
        public СкладськіПриміщення_Pointer СкладськеПриміщенняДляНового { get; set; } = new СкладськіПриміщення_Pointer();
        public СкладськіКомірки_Папки_Pointer РодичДляНового { get; set; } = new СкладськіКомірки_Папки_Pointer();

        Entry Назва = new Entry() { WidthRequest = 500 };
        СкладськіПриміщення_PointerControl СкладськеПриміщення = new СкладськіПриміщення_PointerControl();
        СкладськіКомірки_Папки_PointerControl Родич = new СкладськіКомірки_Папки_PointerControl() { Caption = "Папка:" };

        public СкладськіКомірки_Елемент() : base()
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

            //СкладськеПриміщення
            HBox hBoxSkaldPrem = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSkaldPrem, false, false, 5);

            hBoxSkaldPrem.PackStart(СкладськеПриміщення, false, false, 5);

            //Назва
            HBox hBoxName = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxName, false, false, 5);

            hBoxName.PackStart(new Label("Назва:"), false, false, 5);
            hBoxName.PackStart(Назва, false, false, 5);

            hPaned.Pack1(vBox, false, false);

            //Родич
            HBox hBoxParent = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxParent, false, false, 5);

            Родич.BeforeClickOpenFunc = () =>
            {
                Родич.СкладПриміщенняВласник = СкладськеПриміщення.Pointer;
            };

            hBoxParent.PackStart(Родич, false, false, 5);

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
                СкладськіКомірки_Objest.Приміщення = СкладськеПриміщенняДляНового;
                СкладськіКомірки_Objest.Папка = РодичДляНового;
            }

            Назва.Text = СкладськіКомірки_Objest.Назва;
            СкладськеПриміщення.Pointer = СкладськіКомірки_Objest.Приміщення;
            Родич.Pointer = СкладськіКомірки_Objest.Папка;
        }

        void GetValue()
        {
            СкладськіКомірки_Objest.Назва = Назва.Text;
            СкладськіКомірки_Objest.Приміщення = СкладськеПриміщення.Pointer;
            СкладськіКомірки_Objest.Папка = Родич.Pointer;
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
            {
                СкладськіКомірки_Objest.New();
                IsNew = false;
            }

            GetValue();

            СкладськіКомірки_Objest.Save();

            Program.GeneralForm?.RenameCurrentPageNotebook($"Складська комірка: {СкладськіКомірки_Objest.Назва}");

            if (PageList != null)
            {
                PageList.SelectPointerItem = СкладськіКомірки_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}