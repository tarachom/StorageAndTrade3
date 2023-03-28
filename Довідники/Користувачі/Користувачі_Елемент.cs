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
    class Користувачі_Елемент : VBox
    {
        public Користувачі? PageList { get; set; }
        public System.Action<Користувачі_Pointer>? CallBack_OnSelectPointer { get; set; }

        public bool IsNew { get; set; } = true;

        public Користувачі_Objest Користувачі_Objest { get; set; } = new Користувачі_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        ФізичніОсоби_PointerControl ФізичнаОсоба = new ФізичніОсоби_PointerControl();
        TextView Коментар = new TextView();

        public Користувачі_Елемент() : base()
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

            //ФізичнаОсоба
            HBox hBoxFizOsoba = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxFizOsoba, false, false, 5);

            hBoxFizOsoba.PackStart(ФізичнаОсоба, false, false, 5);

            //Коментар
            HBox hBoxComment = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxComment, false, false, 5);

            hBoxComment.PackStart(new Label("Коментар:") { Valign = Align.Start }, false, false, 5);

            ScrolledWindow scrollTextViewComment = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = 500, HeightRequest = 200 };
            scrollTextViewComment.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrollTextViewComment.Add(Коментар);

            hBoxComment.PackStart(scrollTextViewComment, false, false, 5);

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
                Користувачі_Objest.New();
                
            Код.Text = Користувачі_Objest.Код;
            Назва.Text = Користувачі_Objest.Назва;
            ФізичнаОсоба.Pointer = Користувачі_Objest.ФізичнаОсоба;
            Коментар.Buffer.Text = Користувачі_Objest.Коментар;
        }

        void GetValue()
        {
            Користувачі_Objest.Код = Код.Text;
            Користувачі_Objest.Назва = Назва.Text;
            Користувачі_Objest.ФізичнаОсоба = ФізичнаОсоба.Pointer;
            Користувачі_Objest.Коментар = Коментар.Buffer.Text;
        }

        #endregion

        void Save(bool closePage = false)
        {
            GetValue();

            Користувачі_Objest.Save();

            if (closePage)
                Program.GeneralForm?.CloseCurrentPageNotebook();
            else
                Program.GeneralForm?.RenameCurrentPageNotebook($"Користувач: {Користувачі_Objest.Назва}");

            if (CallBack_OnSelectPointer != null)
                CallBack_OnSelectPointer.Invoke(Користувачі_Objest.GetDirectoryPointer());

            if (PageList != null)
            {
                PageList.SelectPointerItem = Користувачі_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}