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
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class ПартіяТоварівКомпозит_Елемент : ДовідникЕлемент
    {
        public ПартіяТоварівКомпозит_Objest ПартіяТоварівКомпозит_Objest { get; set; } = new ПартіяТоварівКомпозит_Objest();

        Entry Назва = new Entry() { WidthRequest = 500 };
        ComboBoxText ТипДокументу = new ComboBoxText();
        ПоступленняТоварівТаПослуг_PointerControl ПоступленняТоварівТаПослуг = new ПоступленняТоварівТаПослуг_PointerControl() { UseWherePeriod = true };
        ВведенняЗалишків_PointerControl ВведенняЗалишків = new ВведенняЗалишків_PointerControl() { UseWherePeriod = true };

        public ПартіяТоварівКомпозит_Елемент() : base()
        {
            FillComboBoxes();
        }

        void FillComboBoxes()
        {
            foreach (var field in ПсевдонімиПерелічення.ТипДокументуПартіяТоварівКомпозит_List())
                ТипДокументу.Append(field.Value.ToString(), field.Name);

            ТипДокументу.ActiveId = ТипДокументуПартіяТоварівКомпозит.ПоступленняТоварівТаПослуг.ToString();
        }

        protected override void CreatePack1()
        {
            VBox vBox = new VBox();
            HPanedTop.Pack1(vBox, false, false);

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
        }

        protected override void CreatePack2()
        {
            VBox vBox = new VBox();
            HPanedTop.Pack2(vBox, false, false);

            HBox hBoxInfo = new HBox() { Halign = Align.Start };
            vBox.PackStart(hBoxInfo, false, false, 5);

            hBoxInfo.PackStart(new Label("Редагувати дозволено тільки назву"), false, false, 5);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
                ПартіяТоварівКомпозит_Objest.New();

            Назва.Text = ПартіяТоварівКомпозит_Objest.Назва;
            ТипДокументу.ActiveId = ПартіяТоварівКомпозит_Objest.ТипДокументу.ToString();
            ПоступленняТоварівТаПослуг.Pointer = ПартіяТоварівКомпозит_Objest.ПоступленняТоварівТаПослуг;
            ВведенняЗалишків.Pointer = ПартіяТоварівКомпозит_Objest.ВведенняЗалишків;
        }

        protected override void GetValue()
        {
            UnigueID = ПартіяТоварівКомпозит_Objest.UnigueID;
            Caption = Назва.Text;

            ПартіяТоварівКомпозит_Objest.Назва = Назва.Text;

            /*
            Редагування заборонено, тільки назва

            ПартіяТоварівКомпозит_Objest.ТипДокументу = Enum.Parse<Перелічення.ТипДокументуПартіяТоварівКомпозит>(ТипДокументу.ActiveId);
            ПартіяТоварівКомпозит_Objest.ПоступленняТоварівТаПослуг = ПоступленняТоварівТаПослуг.Pointer;
            ПартіяТоварівКомпозит_Objest.ВведенняЗалишків = ВведенняЗалишків.Pointer;
            
            */
        }

        #endregion

        protected override void Save()
        {
            try
            {
                ПартіяТоварівКомпозит_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }
        }
    }
}