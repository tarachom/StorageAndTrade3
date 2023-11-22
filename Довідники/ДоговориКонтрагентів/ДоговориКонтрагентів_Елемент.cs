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

        protected override void CreatePack1(VBox vBox)
        {
            //Код
            CreateField(vBox, "Код:", Код);

            //Назва
            CreateField(vBox, "Назва:", Назва);

            //Дата
            CreateField(vBox, "Дата:", Дата);

            //Номер
            CreateField(vBox, "Номер:", Номер);

            //БанківськийРахунок
            CreateField(vBox, null, БанківськийРахунок);

            //БанківськийРахунокКонтрагента
            CreateField(vBox, null, БанківськийРахунокКонтрагента);

            //ВалютаВзаєморозрахунків
            CreateField(vBox, null, ВалютаВзаєморозрахунків);

            //ДатаПочаткуДії
            CreateField(vBox, "Дата початку дії:", ДатаПочаткуДії);

            //ДатаЗакінченняДії
            CreateField(vBox, "Дата закінчення дії:", ДатаЗакінченняДії);

            //Організація
            CreateField(vBox, null, Організація);

            //Контрагент
            CreateField(vBox, null, Контрагент);

            //Підрозділ
            CreateField(vBox, null, Підрозділ);

            //Узгоджений
            CreateField(vBox, null, Узгоджений);

            //Статус
            CreateField(vBox, "Статус:", Статус);

            //ГосподарськаОперація
            CreateField(vBox, "Господарська операція:", ГосподарськаОперація);

            //ТипДоговору
            CreateField(vBox, "Тип договору:", ТипДоговору);

            //ДопустимаСумаЗаборгованості
            CreateField(vBox, "Допустима сума заборгованості:", ДопустимаСумаЗаборгованості);

            //Сума
            CreateField(vBox, "Сума:", Сума);

            //Коментар
            CreateField(vBox, "Коментар:", Коментар);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
               await ДоговориКонтрагентів_Objest.New();

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

        protected override async ValueTask Save()
        {
            try
            {
                await ДоговориКонтрагентів_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }

            UnigueID = ДоговориКонтрагентів_Objest.UnigueID;
            Caption = Назва.Text;
        }
    }
}