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
    class Склади_Папки_Елемент : VBox
    {
        public Склади_Папки_Дерево? PageList { get; set; }
        public System.Action<Склади_Папки_Pointer>? CallBack_OnSelectPointer { get; set; }

        public bool IsNew { get; set; } = true;

        public Склади_Папки_Pointer РодичДляНового { get; set; } = new Склади_Папки_Pointer();

        public Склади_Папки_Objest Склади_Папки_Objest { get; set; } = new Склади_Папки_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Склади_Папки_PointerControl Родич = new Склади_Папки_PointerControl();

        public Склади_Папки_Елемент() : base()
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
                Склади_Папки_Objest.New();
                Склади_Папки_Objest.Родич = РодичДляНового;
            }
            else
                Родич.UidOpenFolder = Склади_Папки_Objest.UnigueID.ToString();

            Код.Text = Склади_Папки_Objest.Код;
            Назва.Text = Склади_Папки_Objest.Назва;
            Родич.Pointer = Склади_Папки_Objest.Родич;
        }

        void GetValue()
        {
            Склади_Папки_Objest.Код = Код.Text;
            Склади_Папки_Objest.Назва = Назва.Text;
            Склади_Папки_Objest.Родич = Родич.Pointer;
        }

        #endregion

        void Save(bool closePage = false)
        {
            GetValue();

            Склади_Папки_Objest.Save();

            if (closePage)
                Program.GeneralForm?.CloseCurrentPageNotebook();
            else
                Program.GeneralForm?.RenameCurrentPageNotebook($"Контрагент Папки: {Склади_Папки_Objest.Назва}");

            if (CallBack_OnSelectPointer != null)
                CallBack_OnSelectPointer.Invoke(Склади_Папки_Objest.GetDirectoryPointer());

            if (PageList != null)
            {
                PageList.Parent_Pointer = Склади_Папки_Objest.GetDirectoryPointer();
                PageList.LoadTree();
            }
        }
    }
}