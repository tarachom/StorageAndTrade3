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
    class БанківськіРахункиОрганізацій_Елемент : VBox
    {
        public БанківськіРахункиОрганізацій? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public БанківськіРахункиОрганізацій_Objest БанківськіРахункиОрганізацій_Objest { get; set; } = new БанківськіРахункиОрганізацій_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Валюти_PointerControl Валюта = new Валюти_PointerControl();
        Організації_PointerControl Організація = new Організації_PointerControl();

        public БанківськіРахункиОрганізацій_Елемент() : base()
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

            //Валюта
            HBox hBoxValuta = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxValuta, false, false, 5);

            hBoxValuta.PackStart(Валюта, false, false, 5);

            //Організація
            HBox hBoxOrganisation = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxOrganisation, false, false, 5);

            hBoxOrganisation.PackStart(Організація, false, false, 5);

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
                БанківськіРахункиОрганізацій_Objest.Код = (++НумераціяДовідників.БанківськіРахункиОрганізацій_Const).ToString("D6");

            Код.Text = БанківськіРахункиОрганізацій_Objest.Код;
            Назва.Text = БанківськіРахункиОрганізацій_Objest.Назва;
            Валюта.Pointer = БанківськіРахункиОрганізацій_Objest.Валюта;
            Організація.Pointer = БанківськіРахункиОрганізацій_Objest.Організація;
        }

        void GetValue()
        {
            БанківськіРахункиОрганізацій_Objest.Код = Код.Text;
            БанківськіРахункиОрганізацій_Objest.Назва = Назва.Text;
            БанківськіРахункиОрганізацій_Objest.Валюта = Валюта.Pointer;
            БанківськіРахункиОрганізацій_Objest.Організація = Організація.Pointer;
        }

        #endregion

        void Save(bool closePage = false)
        {
            if (IsNew)
            {
                БанківськіРахункиОрганізацій_Objest.New();
                IsNew = false;
            }

            GetValue();

            БанківськіРахункиОрганізацій_Objest.Save();

            if (closePage)
                Program.GeneralForm?.CloseCurrentPageNotebook();
            else
                Program.GeneralForm?.RenameCurrentPageNotebook($"Банківський рахунок організації: {БанківськіРахункиОрганізацій_Objest.Назва}");

            if (PageList != null)
            {
                PageList.SelectPointerItem = БанківськіРахункиОрганізацій_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}