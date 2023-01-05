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
    class Номенклатура_Папки_Елемент : VBox
    {
        public Номенклатура_Папки_Дерево? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public Номенклатура_Папки_Pointer РодичДляНового { get; set; } = new Номенклатура_Папки_Pointer();

        public Номенклатура_Папки_Objest Номенклатура_Папки_Objest { get; set; } = new Номенклатура_Папки_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Номенклатура_Папки_PointerControl Родич = new Номенклатура_Папки_PointerControl();

        public Номенклатура_Папки_Елемент() : base()
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
                Номенклатура_Папки_Objest.Код = (++НумераціяДовідників.Номенклатура_Папки_Const).ToString("D6");
                Номенклатура_Папки_Objest.Родич = РодичДляНового;
            }
            else
                Родич.UidOpenFolder = Номенклатура_Папки_Objest.UnigueID.ToString();

            Код.Text = Номенклатура_Папки_Objest.Код;
            Назва.Text = Номенклатура_Папки_Objest.Назва;
            Родич.Pointer = Номенклатура_Папки_Objest.Родич;
        }

        void GetValue()
        {
            Номенклатура_Папки_Objest.Код = Код.Text;
            Номенклатура_Папки_Objest.Назва = Назва.Text;
            Номенклатура_Папки_Objest.Родич = Родич.Pointer;
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
            {
                Номенклатура_Папки_Objest.New();
                IsNew = false;
            }

            GetValue();

            Номенклатура_Папки_Objest.Save();

            Program.GeneralForm?.RenameCurrentPageNotebook($"Контрагент Папки: {Номенклатура_Папки_Objest.Назва}");

            if (PageList != null)
            {
                PageList.Parent_Pointer = Номенклатура_Папки_Objest.GetDirectoryPointer();
                PageList.LoadTree();
            }
        }
    }
}