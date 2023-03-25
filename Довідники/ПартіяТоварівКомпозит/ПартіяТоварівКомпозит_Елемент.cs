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

using AccountingSoftware;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Довідники;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class ПартіяТоварівКомпозит_Елемент : VBox
    {
        public ПартіяТоварівКомпозит? PageList { get; set; }
        public System.Action<ПартіяТоварівКомпозит_Pointer>? CallBack_OnSelectPointer { get; set; }

        public bool IsNew { get; set; } = true;

        public ПартіяТоварівКомпозит_Objest ПартіяТоварівКомпозит_Objest { get; set; } = new ПартіяТоварівКомпозит_Objest();

        Entry Назва = new Entry() { WidthRequest = 500 };
        ComboBoxText ТипДокументу = new ComboBoxText();
        ПоступленняТоварівТаПослуг_PointerControl ПоступленняТоварівТаПослуг = new ПоступленняТоварівТаПослуг_PointerControl() { UseWherePeriod = true };
        ВведенняЗалишків_PointerControl ВведенняЗалишків = new ВведенняЗалишків_PointerControl() { UseWherePeriod = true };

        public ПартіяТоварівКомпозит_Елемент() : base()
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

            FillComboBoxes();

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

            //Тип
            HBox hBoxTypeDoc = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxTypeDoc, false, false, 5);

            hBoxTypeDoc.PackStart(new Label("Тип документу:"), false, false, 5);
            hBoxTypeDoc.PackStart(ТипДокументу, false, false, 5);

            //ПоступленняТоварівТаПослуг
            HBox hBoxПоступленняТоварівТаПослуг = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxПоступленняТоварівТаПослуг, false, false, 5);

            hBoxПоступленняТоварівТаПослуг.PackStart(ПоступленняТоварівТаПослуг, false, false, 5);

            //ВведенняЗалишків
            HBox hBoxВведенняЗалишків = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxВведенняЗалишків, false, false, 5);

            hBoxВведенняЗалишків.PackStart(ВведенняЗалишків, false, false, 5);

            hPaned.Pack1(vBox, false, false);
        }

        void CreatePack2(HPaned hPaned)
        {
            VBox vBox = new VBox();

            HBox hBoxInfo = new HBox() { Halign = Align.Start };
            vBox.PackStart(hBoxInfo, false, false, 5);

            hBoxInfo.PackStart(new Label("Редагувати дозволено тільки назву"), false, false, 5);


            hPaned.Pack2(vBox, false, false);
        }

        void FillComboBoxes()
        {
            if (Config.Kernel != null)
            {
                foreach (ConfigurationEnumField field in Config.Kernel.Conf.Enums["ТипДокументуПартіяТоварівКомпозит"].Fields.Values)
                    ТипДокументу.Append(field.Name, field.Desc);

                ТипДокументу.ActiveId = Перелічення.ТипДокументуПартіяТоварівКомпозит.ПоступленняТоварівТаПослуг.ToString();
            }
        }

        #region Присвоєння / зчитування значень

        public void SetValue()
        {
            Назва.Text = ПартіяТоварівКомпозит_Objest.Назва;
            ТипДокументу.ActiveId = ПартіяТоварівКомпозит_Objest.ТипДокументу.ToString();
            ПоступленняТоварівТаПослуг.Pointer = ПартіяТоварівКомпозит_Objest.ПоступленняТоварівТаПослуг;
            ВведенняЗалишків.Pointer = ПартіяТоварівКомпозит_Objest.ВведенняЗалишків;
        }

        void GetValue()
        {
            ПартіяТоварівКомпозит_Objest.Назва = Назва.Text;

            /*
            Редагування заборонено, тільки назва

            ПартіяТоварівКомпозит_Objest.ТипДокументу = Enum.Parse<Перелічення.ТипДокументуПартіяТоварівКомпозит>(ТипДокументу.ActiveId);
            ПартіяТоварівКомпозит_Objest.ПоступленняТоварівТаПослуг = ПоступленняТоварівТаПослуг.Pointer;
            ПартіяТоварівКомпозит_Objest.ВведенняЗалишків = ВведенняЗалишків.Pointer;
            
            */
        }

        #endregion

        void Save(bool closePage = false)
        {
            if (IsNew)
            {
                ПартіяТоварівКомпозит_Objest.New();
                IsNew = false;
            }

            GetValue();

            ПартіяТоварівКомпозит_Objest.Save();

            if (closePage)
                Program.GeneralForm?.CloseCurrentPageNotebook();
            else
                Program.GeneralForm?.RenameCurrentPageNotebook($"Партія: {ПартіяТоварівКомпозит_Objest.Назва}");

            if (CallBack_OnSelectPointer != null)
                CallBack_OnSelectPointer.Invoke(ПартіяТоварівКомпозит_Objest.GetDirectoryPointer());

            if (PageList != null)
            {
                PageList.SelectPointerItem = ПартіяТоварівКомпозит_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}