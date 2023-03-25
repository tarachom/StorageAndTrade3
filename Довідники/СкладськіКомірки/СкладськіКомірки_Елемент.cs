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
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class СкладськіКомірки_Елемент : VBox
    {
        public СкладськіКомірки? PageList { get; set; }
        public System.Action<СкладськіКомірки_Pointer>? CallBack_OnSelectPointer { get; set; }

        public bool IsNew { get; set; } = true;

        public СкладськіКомірки_Objest СкладськіКомірки_Objest { get; set; } = new СкладськіКомірки_Objest();
        public СкладськіПриміщення_Pointer СкладськеПриміщенняДляНового { get; set; } = new СкладськіПриміщення_Pointer();
        public СкладськіКомірки_Папки_Pointer РодичДляНового { get; set; } = new СкладськіКомірки_Папки_Pointer();

        Entry Назва = new Entry() { WidthRequest = 500 };
        СкладськіПриміщення_PointerControl СкладськеПриміщення = new СкладськіПриміщення_PointerControl();
        СкладськіКомірки_Папки_PointerControl Родич = new СкладськіКомірки_Папки_PointerControl() { Caption = "Папка:" };
        Entry Лінія = new Entry() { WidthRequest = 200 };
        Entry Стелаж = new Entry() { WidthRequest = 200 };
        Entry Позиція = new Entry() { WidthRequest = 200 };
        Entry Ярус = new Entry() { WidthRequest = 200 };
        ComboBoxText ТипСкладськоїКомірки = new ComboBoxText();
        ТипорозміриКомірок_PointerControl Типорозмір = new ТипорозміриКомірок_PointerControl();

        public СкладськіКомірки_Елемент() : base()
        {
            HBox hBox = new HBox();

            Button bSaveAndClose = new Button("Зберегти та закрити");
            bSaveAndClose.Clicked += (object? sender, EventArgs args) => { Save(true); };
            hBox.PackStart(bSaveAndClose, false, false, 10);

            Button bSave = new Button("Зберегти");
            bSave.Clicked += (object? sender, EventArgs args) => { Save(); };
            hBox.PackStart(bSave, false, false, 10);

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

            //Назва
            HBox hBoxName = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxName, false, false, 5);

            hBoxName.PackStart(new Label("Назва:"), false, false, 5);
            hBoxName.PackStart(Назва, false, false, 5);

            hPaned.Pack1(vBox, false, false);

            //СкладськеПриміщення
            HBox hBoxSkaldPrem = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSkaldPrem, false, false, 5);

            hBoxSkaldPrem.PackStart(СкладськеПриміщення, false, false, 5);

            //Родич
            HBox hBoxParent = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxParent, false, false, 5);

            Родич.BeforeClickOpenFunc = () =>
            {
                Родич.СкладПриміщенняВласник = СкладськеПриміщення.Pointer;
            };

            hBoxParent.PackStart(Родич, false, false, 5);

            //ТипСкладськоїКомірки
            HBox hBoxTypeCell = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxTypeCell, false, false, 5);

            foreach (var field in Перелічення.ПсевдонімиПерелічення.ТипиСкладськихКомірок_Array())
                ТипСкладськоїКомірки.Append(field.Value.ToString(), field.Name);

            hBoxTypeCell.PackStart(new Label("Тип комірки:"), false, false, 5);
            hBoxTypeCell.PackStart(ТипСкладськоїКомірки, false, false, 5);

            // Типорозмір
            HBox hBoxType = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxType, false, false, 5);

            hBoxType.PackStart(Типорозмір, false, false, 5);

            hPaned.Pack1(vBox, false, false);
        }

        void CreatePack2(HPaned hPaned)
        {
            VBox vBox = new VBox();

            VBox vBoxContainer = new VBox() { WidthRequest = 300, Halign = Align.Start };
            vBox.PackStart(vBoxContainer, false, false, 0);

            // Лінія
            HBox hBoxLine = new HBox() { Halign = Align.End };
            vBoxContainer.PackStart(hBoxLine, false, false, 5);

            hBoxLine.PackStart(new Label("Лінія:"), false, false, 5);
            hBoxLine.PackStart(Лінія, false, false, 5);

            // Стелаж
            HBox hBoxStelaj = new HBox() { Halign = Align.End };
            vBoxContainer.PackStart(hBoxStelaj, false, false, 5);

            hBoxStelaj.PackStart(new Label("Стелаж:"), false, false, 5);
            hBoxStelaj.PackStart(Стелаж, false, false, 5);

            // Позиція
            HBox hBoxPosition = new HBox() { Halign = Align.End };
            vBoxContainer.PackStart(hBoxPosition, false, false, 5);

            hBoxPosition.PackStart(new Label("Позиція:"), false, false, 5);
            hBoxPosition.PackStart(Позиція, false, false, 5);

            // Ярус
            HBox hBoxYarus = new HBox() { Halign = Align.End };
            vBoxContainer.PackStart(hBoxYarus, false, false, 5);

            hBoxYarus.PackStart(new Label("Ярус:"), false, false, 5);
            hBoxYarus.PackStart(Ярус, false, false, 5);

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

            ТипСкладськоїКомірки.ActiveId = СкладськіКомірки_Objest.ТипСкладськоїКомірки.ToString();

            if (ТипСкладськоїКомірки.Active == -1)
                ТипСкладськоїКомірки.ActiveId = Перелічення.ТипиСкладськихКомірок.Зберігання.ToString();

            Лінія.Text = СкладськіКомірки_Objest.Лінія;
            Стелаж.Text = СкладськіКомірки_Objest.Стелаж;
            Позиція.Text = СкладськіКомірки_Objest.Позиція;
            Ярус.Text = СкладськіКомірки_Objest.Ярус;

            Типорозмір.Pointer = СкладськіКомірки_Objest.Типорозмір;
        }

        void GetValue()
        {
            СкладськіКомірки_Objest.Назва = Назва.Text;
            СкладськіКомірки_Objest.Приміщення = СкладськеПриміщення.Pointer;
            СкладськіКомірки_Objest.Папка = Родич.Pointer;

            СкладськіКомірки_Objest.ТипСкладськоїКомірки = Enum.Parse<Перелічення.ТипиСкладськихКомірок>(ТипСкладськоїКомірки.ActiveId);

            СкладськіКомірки_Objest.Лінія = Лінія.Text;
            СкладськіКомірки_Objest.Стелаж = Стелаж.Text;
            СкладськіКомірки_Objest.Позиція = Позиція.Text;
            СкладськіКомірки_Objest.Ярус = Ярус.Text;

            СкладськіКомірки_Objest.Типорозмір = Типорозмір.Pointer;
        }

        #endregion

        void Save(bool closePage = false)
        {
            if (IsNew)
            {
                СкладськіКомірки_Objest.New();
                IsNew = false;
            }

            GetValue();

            СкладськіКомірки_Objest.Save();

            if (closePage)
                Program.GeneralForm?.CloseCurrentPageNotebook();
            else
                Program.GeneralForm?.RenameCurrentPageNotebook($"Складські комірки: {СкладськіКомірки_Objest.Назва}");

            if (CallBack_OnSelectPointer != null)
                CallBack_OnSelectPointer.Invoke(СкладськіКомірки_Objest.GetDirectoryPointer());

            if (PageList != null)
            {
                PageList.SelectPointerItem = СкладськіКомірки_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}