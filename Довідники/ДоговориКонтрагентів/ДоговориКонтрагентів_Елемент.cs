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
    class ДоговориКонтрагентів_Елемент : ДовідникЕлемент
    {
        public ДоговориКонтрагентів_Objest ДоговориКонтрагентів_Objest { get; set; } = new ДоговориКонтрагентів_Objest();

        #region Fields

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        DateTimeControl Дата = new DateTimeControl() { OnlyDate = true };
        Entry Номер = new Entry() { WidthRequest = 100 };
        БанківськіРахункиОрганізацій_PointerControl БанківськийРахунок = new БанківськіРахункиОрганізацій_PointerControl() { Caption = "Банківський рахунок:" };
        БанківськіРахункиКонтрагентів_PointerControl БанківськийРахунокКонтрагента = new БанківськіРахункиКонтрагентів_PointerControl() { Caption = "Банківський рахунок:" };
        Валюти_PointerControl ВалютаВзаєморозрахунків = new Валюти_PointerControl() { Caption = "Валюта:" };
        DateTimeControl ДатаПочаткуДії = new DateTimeControl() { OnlyDate = true };
        DateTimeControl ДатаЗакінченняДії = new DateTimeControl() { OnlyDate = true };
        Організації_PointerControl Організація = new Організації_PointerControl() { Caption = "Організація:" };
        Контрагенти_PointerControl Контрагент = new Контрагенти_PointerControl() { Caption = "Контрагент:" };
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ:" };
        CheckButton Узгоджений = new CheckButton("Узгоджений");
        ComboBoxText Статус = new ComboBoxText();
        ComboBoxText ГосподарськаОперація = new ComboBoxText();
        ComboBoxText ТипДоговору = new ComboBoxText();
        NumericControl ДопустимаСумаЗаборгованості = new NumericControl();
        NumericControl Сума = new NumericControl();
        Entry Коментар = new Entry() { WidthRequest = 500 };

        #endregion

        public ДоговориКонтрагентів_Елемент() : base()
        {
            FillComboBoxes();
        }

        void FillComboBoxes()
        {
            //1
            foreach (var field in ПсевдонімиПерелічення.ГосподарськіОперації_List())
                ГосподарськаОперація.Append(field.Value.ToString(), field.Name);

            ГосподарськаОперація.ActiveId = ГосподарськіОперації.РеалізаціяКлієнту.ToString();

            //2
            foreach (var field in ПсевдонімиПерелічення.СтатусиДоговорівКонтрагентів_List())
                Статус.Append(field.Value.ToString(), field.Name);

            Статус.ActiveId = СтатусиДоговорівКонтрагентів.Діє.ToString();

            //3
            foreach (var field in ПсевдонімиПерелічення.ТипДоговорів_List())
                ТипДоговору.Append(field.Value.ToString(), field.Name);

            ТипДоговору.ActiveId = ТипДоговорів.ЗПокупцями.ToString();
        }

        protected override void CreatePack1()
        {
            VBox vBox = new VBox();
            HPanedTop.Pack1(vBox, false, false);

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

            //Дата
            HBox hBoxData = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxData, false, false, 5);

            hBoxData.PackStart(new Label("Дата:"), false, false, 5);
            hBoxData.PackStart(Дата, false, false, 5);

            //Номер
            HBox hBoxNomer = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxNomer, false, false, 5);

            hBoxNomer.PackStart(new Label("Номер:"), false, false, 5);
            hBoxNomer.PackStart(Номер, false, false, 5);

            //БанківськийРахунок
            HBox hBoxBankRachunok = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxBankRachunok, false, false, 5);

            hBoxBankRachunok.PackStart(БанківськийРахунок, false, false, 5);

            //БанківськийРахунокКонтрагента
            HBox hBoxBankRachunokKontragenta = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxBankRachunokKontragenta, false, false, 5);

            hBoxBankRachunokKontragenta.PackStart(БанківськийРахунокКонтрагента, false, false, 5);

            //ВалютаВзаєморозрахунків
            HBox hBoxValuta = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxValuta, false, false, 5);

            hBoxValuta.PackStart(ВалютаВзаєморозрахунків, false, false, 5);

            //ДатаПочаткуДії
            HBox hBoxDataStart = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxDataStart, false, false, 5);

            hBoxDataStart.PackStart(new Label("Дата початку дії:"), false, false, 5);
            hBoxDataStart.PackStart(ДатаПочаткуДії, false, false, 5);

            //ДатаЗакінченняДії
            HBox hBoxDataStop = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxDataStop, false, false, 5);

            hBoxDataStop.PackStart(new Label("Дата закінчення дії:"), false, false, 5);
            hBoxDataStop.PackStart(ДатаЗакінченняДії, false, false, 5);

            //Організація
            HBox hBoxOrganization = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxOrganization, false, false, 5);

            hBoxOrganization.PackStart(Організація, false, false, 5);

            //Контрагент
            HBox hBoxKontragent = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxKontragent, false, false, 5);

            hBoxKontragent.PackStart(Контрагент, false, false, 5);

            //Підрозділ
            HBox hBoxPidrozdil = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxPidrozdil, false, false, 5);

            hBoxPidrozdil.PackStart(Підрозділ, false, false, 5);

            //Узгоджений
            HBox hBoxUzgodjenyi = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxUzgodjenyi, false, false, 5);

            hBoxUzgodjenyi.PackStart(Узгоджений, false, false, 5);

            //Статус
            HBox hBoxStatus = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxStatus, false, false, 5);

            hBoxStatus.PackStart(new Label("Статус: "), false, false, 0);
            hBoxStatus.PackStart(Статус, false, false, 5);

            //ГосподарськаОперація
            HBox hBoxOperation = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxOperation, false, false, 5);

            hBoxOperation.PackStart(new Label("Господарська операція: "), false, false, 0);
            hBoxOperation.PackStart(ГосподарськаОперація, false, false, 5);

            //ТипДоговору
            HBox hBoxTypeDogovor = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxTypeDogovor, false, false, 5);

            hBoxTypeDogovor.PackStart(new Label("Тип договору: "), false, false, 0);
            hBoxTypeDogovor.PackStart(ТипДоговору, false, false, 5);

            //ДопустимаСумаЗаборгованості
            HBox hBoxDopustymaSumaBorgu = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxDopustymaSumaBorgu, false, false, 5);

            hBoxDopustymaSumaBorgu.PackStart(new Label("Допустима сума заборгованості: "), false, false, 5);
            hBoxDopustymaSumaBorgu.PackStart(ДопустимаСумаЗаборгованості, false, false, 5);

            //Сума
            HBox hBoxSuma = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSuma, false, false, 5);

            hBoxSuma.PackStart(new Label("Сума: "), false, false, 5);
            hBoxSuma.PackStart(Сума, false, false, 5);

            //Коментар
            HBox hBoxComment = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxComment, false, false, 5);

            hBoxComment.PackStart(new Label("Коментар: "), false, false, 5);
            hBoxComment.PackStart(Коментар, false, false, 5);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
                ДоговориКонтрагентів_Objest.New();

            Код.Text = ДоговориКонтрагентів_Objest.Код;
            Назва.Text = ДоговориКонтрагентів_Objest.Назва;
            Дата.Value = ДоговориКонтрагентів_Objest.Дата;
            Номер.Text = ДоговориКонтрагентів_Objest.Номер;
            БанківськийРахунок.Pointer = ДоговориКонтрагентів_Objest.БанківськийРахунок;
            БанківськийРахунокКонтрагента.Pointer = ДоговориКонтрагентів_Objest.БанківськийРахунокКонтрагента;
            ВалютаВзаєморозрахунків.Pointer = ДоговориКонтрагентів_Objest.ВалютаВзаєморозрахунків;
            ДатаПочаткуДії.Value = ДоговориКонтрагентів_Objest.ДатаПочаткуДії;
            ДатаЗакінченняДії.Value = ДоговориКонтрагентів_Objest.ДатаЗакінченняДії;
            Організація.Pointer = ДоговориКонтрагентів_Objest.Організація;
            Контрагент.Pointer = ДоговориКонтрагентів_Objest.Контрагент;
            Підрозділ.Pointer = ДоговориКонтрагентів_Objest.Підрозділ;
            Узгоджений.Active = ДоговориКонтрагентів_Objest.Узгоджений;
            Статус.ActiveId = ДоговориКонтрагентів_Objest.Статус.ToString();
            ГосподарськаОперація.ActiveId = ДоговориКонтрагентів_Objest.ГосподарськаОперація.ToString();
            ТипДоговору.ActiveId = ДоговориКонтрагентів_Objest.ТипДоговору.ToString();
            ДопустимаСумаЗаборгованості.Value = ДоговориКонтрагентів_Objest.ДопустимаСумаЗаборгованості;
            Сума.Value = ДоговориКонтрагентів_Objest.Сума;
            Коментар.Text = ДоговориКонтрагентів_Objest.Коментар;
        }

        protected override void GetValue()
        {
            UnigueID = ДоговориКонтрагентів_Objest.UnigueID;
            Caption = Назва.Text;

            ДоговориКонтрагентів_Objest.Код = Код.Text;
            ДоговориКонтрагентів_Objest.Назва = Назва.Text;
            ДоговориКонтрагентів_Objest.Дата = Дата.Value;
            ДоговориКонтрагентів_Objest.Номер = Номер.Text;
            ДоговориКонтрагентів_Objest.БанківськийРахунок = БанківськийРахунок.Pointer;
            ДоговориКонтрагентів_Objest.БанківськийРахунокКонтрагента = БанківськийРахунокКонтрагента.Pointer;
            ДоговориКонтрагентів_Objest.ВалютаВзаєморозрахунків = ВалютаВзаєморозрахунків.Pointer;
            ДоговориКонтрагентів_Objest.ДатаПочаткуДії = ДатаПочаткуДії.Value;
            ДоговориКонтрагентів_Objest.ДатаЗакінченняДії = ДатаЗакінченняДії.Value;
            ДоговориКонтрагентів_Objest.Організація = Організація.Pointer;
            ДоговориКонтрагентів_Objest.Контрагент = Контрагент.Pointer;
            ДоговориКонтрагентів_Objest.Підрозділ = Підрозділ.Pointer;
            ДоговориКонтрагентів_Objest.Узгоджений = Узгоджений.Active;
            ДоговориКонтрагентів_Objest.Статус = Enum.Parse<СтатусиДоговорівКонтрагентів>(Статус.ActiveId);
            ДоговориКонтрагентів_Objest.ГосподарськаОперація = Enum.Parse<ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            ДоговориКонтрагентів_Objest.ТипДоговору = Enum.Parse<ТипДоговорів>(ТипДоговору.ActiveId);
            ДоговориКонтрагентів_Objest.ДопустимаСумаЗаборгованості = ДопустимаСумаЗаборгованості.Value;
            ДоговориКонтрагентів_Objest.Сума = Сума.Value;
            ДоговориКонтрагентів_Objest.Коментар = Коментар.Text;
        }

        #endregion

        protected override void Save()
        {
            try
            {
                ДоговориКонтрагентів_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }
        }
    }
}