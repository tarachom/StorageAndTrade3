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
    class Контрагенти_Елемент : VBox
    {
        public Контрагенти? PageList { get; set; }
        public System.Action<Контрагенти_Pointer>? CallBack_OnSelectPointer { get; set; }

        public bool IsNew { get; set; } = true;

        public Контрагенти_Папки_Pointer РодичДляНового { get; set; } = new Контрагенти_Папки_Pointer();

        public Контрагенти_Objest Контрагенти_Objest { get; set; } = new Контрагенти_Objest();

        #region Field

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        TextView НазваПовна = new TextView();
        Entry РеєстраційнийНомер = new Entry() { WidthRequest = 300 };
        TextView Опис = new TextView();
        Контрагенти_Папки_PointerControl Родич = new Контрагенти_Папки_PointerControl() { Caption = "Папка:" };
        Контрагенти_ТабличнаЧастина_Контакти Контакти = new Контрагенти_ТабличнаЧастина_Контакти();
        Контрагенти_ТабличнаЧастина_Файли Файли = new Контрагенти_ТабличнаЧастина_Файли();

        #endregion

        public Контрагенти_Елемент() : base()
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

            //Родич
            HBox hBoxParent = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxParent, false, false, 5);

            hBoxParent.PackStart(Родич, false, false, 5);

            //НазваПовна
            HBox hBoxDesc = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxDesc, false, false, 5);

            hBoxDesc.PackStart(new Label("Повна назва:") { Valign = Align.Start }, false, false, 5);

            ScrolledWindow scrollTextView = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = 500, HeightRequest = 60 };
            scrollTextView.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrollTextView.Add(НазваПовна);

            hBoxDesc.PackStart(scrollTextView, false, false, 5);

            //РеєстраційнийНомер
            HBox hBoxRegisterNumber = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxRegisterNumber, false, false, 5);

            hBoxRegisterNumber.PackStart(new Label("Реєстраційний номер:"), false, false, 5);
            hBoxRegisterNumber.PackStart(РеєстраційнийНомер, false, false, 5);

            //Опис
            HBox hBoxOpys = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxOpys, false, false, 5);

            hBoxOpys.PackStart(new Label("Опис:") { Valign = Align.Start }, false, false, 5);

            ScrolledWindow scrollTextViewOpys = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = 500, HeightRequest = 200 };
            scrollTextViewOpys.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrollTextViewOpys.Add(Опис);

            hBoxOpys.PackStart(scrollTextViewOpys, false, false, 5);

            hPaned.Pack1(vBox, false, false);
        }

        void CreatePack2(HPaned hPaned)
        {
            VBox vBox = new VBox();

            //Контакти
            HBox hBoxContaktyInfo = new HBox();
            hBoxContaktyInfo.PackStart(new Label("Контакти:"), false, false, 5);
            vBox.PackStart(hBoxContaktyInfo, false, false, 5);

            HBox hBoxContakty = new HBox();
            hBoxContakty.PackStart(Контакти, true, true, 5);

            vBox.PackStart(hBoxContakty, false, false, 0);

            //Файли
            HBox hBoxFilesInfo = new HBox();
            hBoxFilesInfo.PackStart(new Label("Файли:"), false, false, 5);
            vBox.PackStart(hBoxFilesInfo, false, false, 5);

            HBox hBoxFiles = new HBox();
            hBoxFiles.PackStart(Файли, true, true, 5);

            vBox.PackStart(hBoxFiles, false, false, 0);

            hPaned.Pack2(vBox, false, false);
        }

        #region Присвоєння / зчитування значень

        public void SetValue()
        {
            if (IsNew)
            {
                Контрагенти_Objest.New();
                Контрагенти_Objest.Папка = РодичДляНового;
            }

            Код.Text = Контрагенти_Objest.Код;
            Назва.Text = Контрагенти_Objest.Назва;
            Родич.Pointer = Контрагенти_Objest.Папка;
            НазваПовна.Buffer.Text = Контрагенти_Objest.НазваПовна;
            РеєстраційнийНомер.Text = Контрагенти_Objest.РеєстраційнийНомер;
            Опис.Buffer.Text = Контрагенти_Objest.Опис;

            Контакти.Контрагенти_Objest = Контрагенти_Objest;
            Контакти.LoadRecords();

            Файли.Контрагенти_Objest = Контрагенти_Objest;
            Файли.LoadRecords();
        }

        void GetValue()
        {
            Контрагенти_Objest.Код = Код.Text;
            Контрагенти_Objest.Назва = Назва.Text;
            Контрагенти_Objest.Папка = Родич.Pointer;
            Контрагенти_Objest.НазваПовна = НазваПовна.Buffer.Text;
            Контрагенти_Objest.РеєстраційнийНомер = РеєстраційнийНомер.Text;
            Контрагенти_Objest.Опис = Опис.Buffer.Text;
        }

        #endregion

        void Save(bool closePage = false)
        {
            GetValue();

            Контрагенти_Objest.Save();
            Контакти.SaveRecords();
            Файли.SaveRecords();

            if (closePage)
                Program.GeneralForm?.CloseCurrentPageNotebook();
            else
                Program.GeneralForm?.RenameCurrentPageNotebook($"Контрагент: {Контрагенти_Objest.Назва}");

            if (CallBack_OnSelectPointer != null)
                CallBack_OnSelectPointer.Invoke(Контрагенти_Objest.GetDirectoryPointer());

            if (PageList != null)
            {
                PageList.SelectPointerItem = Контрагенти_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}