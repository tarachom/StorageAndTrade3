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
    class СкладськіПриміщення_Елемент : VBox
    {
        public СкладськіПриміщення? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public СкладськіПриміщення_Objest СкладськіПриміщення_Objest { get; set; } = new СкладськіПриміщення_Objest();
        public Склади_Pointer СкладДляНового { get; set; } = new Склади_Pointer();

        Entry Назва = new Entry() { WidthRequest = 500 };
        Склади_PointerControl Склад = new Склади_PointerControl();
        ComboBoxText НалаштуванняАдресногоЗберігання = new ComboBoxText();

        public СкладськіПриміщення_Елемент() : base()
        {
            new VBox();
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

            //Склад
            HBox hBoxSkald = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSkald, false, false, 5);

            hBoxSkald.PackStart(Склад, false, false, 5);

            //Назва
            HBox hBoxName = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxName, false, false, 5);

            hBoxName.PackStart(new Label("Назва:"), false, false, 5);
            hBoxName.PackStart(Назва, false, false, 5);

            hPaned.Pack1(vBox, false, false);

            //НалаштуванняАдресногоЗберігання
            HBox hBoxAdressSave = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxAdressSave, false, false, 5);

            foreach (var field in Перелічення.ПсевдонімиПерелічення.НалаштуванняАдресногоЗберігання_Array())
                НалаштуванняАдресногоЗберігання.Append(field.Value.ToString(), field.Name);

            hBoxAdressSave.PackStart(new Label("Адресне зберігання:"), false, false, 5);
            hBoxAdressSave.PackStart(НалаштуванняАдресногоЗберігання, false, false, 5);
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
                СкладськіПриміщення_Objest.Склад = СкладДляНового;
            }

            Назва.Text = СкладськіПриміщення_Objest.Назва;
            Склад.Pointer = СкладськіПриміщення_Objest.Склад;
            НалаштуванняАдресногоЗберігання.ActiveId = СкладськіПриміщення_Objest.НалаштуванняАдресногоЗберігання.ToString();

            if (НалаштуванняАдресногоЗберігання.Active == -1)
                НалаштуванняАдресногоЗберігання.ActiveId = Перелічення.НалаштуванняАдресногоЗберігання.НеВикористовувати.ToString();
        }

        void GetValue()
        {
            СкладськіПриміщення_Objest.Назва = Назва.Text;
            СкладськіПриміщення_Objest.Склад = Склад.Pointer;
            СкладськіПриміщення_Objest.НалаштуванняАдресногоЗберігання = Enum.Parse<Перелічення.НалаштуванняАдресногоЗберігання>(НалаштуванняАдресногоЗберігання.ActiveId);
        }

        #endregion

        void Save(bool closePage = false)
        {
            if (IsNew)
            {
                СкладськіПриміщення_Objest.New();
                IsNew = false;
            }

            GetValue();

            СкладськіПриміщення_Objest.Save();

            if (closePage)
                Program.GeneralForm?.CloseCurrentPageNotebook();
            else
                Program.GeneralForm?.RenameCurrentPageNotebook($"Складські приміщення: {СкладськіПриміщення_Objest.Назва}");

            if (PageList != null)
            {
                PageList.SelectPointerItem = СкладськіПриміщення_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}