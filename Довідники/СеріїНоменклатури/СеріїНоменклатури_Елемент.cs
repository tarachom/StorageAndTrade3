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
    class СеріїНоменклатури_Елемент : VBox
    {
        public СеріїНоменклатури? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public СеріїНоменклатури_Objest СеріїНоменклатури_Objest { get; set; } = new СеріїНоменклатури_Objest();

        Entry Номер = new Entry() { WidthRequest = 500 };

        public СеріїНоменклатури_Елемент() : base()
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

            //Номер
            HBox hBoxNomer = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxNomer, false, false, 5);

            hBoxNomer.PackStart(new Label("Назва:"), false, false, 5);
            hBoxNomer.PackStart(Номер, false, false, 5);

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
            Номер.Text = СеріїНоменклатури_Objest.Номер;
        }

        void GetValue()
        {
            СеріїНоменклатури_Objest.Номер = Номер.Text;
        }

        #endregion

        void Save(bool closePage = false)
        {
            if (IsNew)
            {
                СеріїНоменклатури_Objest.New();
                IsNew = false;
            }

            GetValue();

            СеріїНоменклатури_Objest.Save();

            if (closePage)
                Program.GeneralForm?.CloseCurrentPageNotebook();
            else
                Program.GeneralForm?.RenameCurrentPageNotebook($"Серійний номер: {СеріїНоменклатури_Objest.Номер}");

            if (PageList != null)
            {
                PageList.SelectPointerItem = СеріїНоменклатури_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}