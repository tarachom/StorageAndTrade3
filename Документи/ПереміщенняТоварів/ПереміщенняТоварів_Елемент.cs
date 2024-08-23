/*
Copyright (C) 2019-2024 TARAKHOMYN YURIY IVANOVYCH
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
using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Документи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class ПереміщенняТоварів_Елемент : ДокументЕлемент
    {
        public ПереміщенняТоварів_Objest ПереміщенняТоварів_Objest { get; set; } = new ПереміщенняТоварів_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Організації_PointerControl ОрганізаціяОтримувач = new Організації_PointerControl();
        Склади_PointerControl СкладВідправник = new Склади_PointerControl() { Caption = "Склад відправник:" };
        Склади_PointerControl СкладОтримувач = new Склади_PointerControl() { Caption = "Склад отримувач:" };
        ComboBoxText ГосподарськаОперація = new ComboBoxText();
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ" };
        БанківськіРахункиОрганізацій_PointerControl БанківськийРахунокОрганізації = new БанківськіРахункиОрганізацій_PointerControl() { WidthPresentation = 200 };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        ComboBoxText СпосібДоставки = new ComboBoxText();
        TimeControl ЧасДоставкиЗ = new TimeControl();
        TimeControl ЧасДоставкиДо = new TimeControl();
        Entry Коментар = new Entry() { WidthRequest = 920 };
        CompositePointerControl Основа = new CompositePointerControl();

        ПереміщенняТоварів_ТабличнаЧастина_Товари Товари = new ПереміщенняТоварів_ТабличнаЧастина_Товари();

        #endregion

        public ПереміщенняТоварів_Елемент() : base()
        {
            CreateDocName(ПереміщенняТоварів_Const.FULLNAME, НомерДок, ДатаДок);

            CreateField(HBoxComment, "Коментар:", Коментар);

            NotebookTablePart.InsertPage(Товари, new Label("Товари"), 0);
            NotebookTablePart.CurrentPage = 0;

            FillComboBoxes();
        }

        void FillComboBoxes()
        {
            //1
            ConfigurationEnums Конфігурація_ГосподарськіОперації = Config.Kernel.Conf.Enums["ГосподарськіОперації"];

            ГосподарськаОперація.Append(
                Перелічення.ГосподарськіОперації.ПереміщенняТоварів.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["ПереміщенняТоварів"].Desc);

            ГосподарськаОперація.Active = 0;

            //3
            foreach (var field in Перелічення.ПсевдонімиПерелічення.СпособиДоставки_List())
                СпосібДоставки.Append(field.Value.ToString(), field.Name);

            СпосібДоставки.ActiveId = Перелічення.СпособиДоставки.Самовивіз.ToString();
        }

        protected override void CreateContainer1(Box vBox)
        {
            //Організація
            CreateField(vBox, null, Організація);

            //СкладВідправник
            CreateField(vBox, null, СкладВідправник);
        }

        protected override void CreateContainer2(Box vBox)
        {
            //СкладОтримувач
            CreateField(vBox, null, СкладОтримувач);
        }

        protected override void CreateContainer3(Box vBox)
        {
            //Підрозділ
            CreateField(vBox, null, Підрозділ);

            //БанківськийрахунокОрганізації
            CreateField(vBox, null, БанківськийРахунокОрганізації);

            //Автор
            CreateField(vBox, null, Автор);

            //Основа
            CreateField(vBox, null, Основа);
        }

        protected override void CreateContainer4(Box vBox)
        {
            //ГосподарськаОперація
            CreateField(vBox, "Господарська операція:", ГосподарськаОперація);

            //СпосібДоставки
            CreateField(vBox, "Спосіб доставки:", СпосібДоставки);

            //ЧасДоставки
            Box hBox1 = CreateField(vBox, "Час доставки з", ЧасДоставкиЗ);
            CreateField(hBox1, "до", ЧасДоставкиДо);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                await ПереміщенняТоварів_Objest.New();
                ПереміщенняТоварів_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                ПереміщенняТоварів_Objest.СкладВідправник = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                ПереміщенняТоварів_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
                ПереміщенняТоварів_Objest.БанківськийРахунокОрганізації = ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const;
            }

            НомерДок.Text = ПереміщенняТоварів_Objest.НомерДок;
            ДатаДок.Value = ПереміщенняТоварів_Objest.ДатаДок;
            Організація.Pointer = ПереміщенняТоварів_Objest.Організація;
            СкладВідправник.Pointer = ПереміщенняТоварів_Objest.СкладВідправник;
            СкладОтримувач.Pointer = ПереміщенняТоварів_Objest.СкладОтримувач;
            ГосподарськаОперація.ActiveId = ПереміщенняТоварів_Objest.ГосподарськаОперація.ToString();
            Коментар.Text = ПереміщенняТоварів_Objest.Коментар;
            Підрозділ.Pointer = ПереміщенняТоварів_Objest.Підрозділ;
            БанківськийРахунокОрганізації.Pointer = ПереміщенняТоварів_Objest.БанківськийРахунокОрганізації;
            Автор.Pointer = ПереміщенняТоварів_Objest.Автор;
            СпосібДоставки.ActiveId = ПереміщенняТоварів_Objest.СпосібДоставки.ToString();
            ЧасДоставкиЗ.Value = ПереміщенняТоварів_Objest.ЧасДоставкиЗ;
            ЧасДоставкиДо.Value = ПереміщенняТоварів_Objest.ЧасДоставкиДо;
            Основа.Pointer = ПереміщенняТоварів_Objest.Основа;

            //Таблична частина
            Товари.ПереміщенняТоварів_Objest = ПереміщенняТоварів_Objest;
            await Товари.LoadRecords();
        }

        protected override void GetValue()
        {
            ПереміщенняТоварів_Objest.НомерДок = НомерДок.Text;
            ПереміщенняТоварів_Objest.ДатаДок = ДатаДок.Value;
            ПереміщенняТоварів_Objest.Організація = Організація.Pointer;
            ПереміщенняТоварів_Objest.СкладВідправник = СкладВідправник.Pointer;
            ПереміщенняТоварів_Objest.СкладОтримувач = СкладОтримувач.Pointer;
            ПереміщенняТоварів_Objest.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            ПереміщенняТоварів_Objest.Коментар = Коментар.Text;
            ПереміщенняТоварів_Objest.Підрозділ = Підрозділ.Pointer;
            ПереміщенняТоварів_Objest.БанківськийРахунокОрганізації = БанківськийРахунокОрганізації.Pointer;
            ПереміщенняТоварів_Objest.Автор = Автор.Pointer;
            ПереміщенняТоварів_Objest.СпосібДоставки = Enum.Parse<Перелічення.СпособиДоставки>(СпосібДоставки.ActiveId);
            ПереміщенняТоварів_Objest.ЧасДоставкиЗ = ЧасДоставкиЗ.Value;
            ПереміщенняТоварів_Objest.ЧасДоставкиДо = ЧасДоставкиДо.Value;
            ПереміщенняТоварів_Objest.Основа = Основа.Pointer;

            ПереміщенняТоварів_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {СкладВідправник.Pointer.Назва} {СкладОтримувач.Pointer.Назва}";
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            bool isSave = false;
            UnigueID = ПереміщенняТоварів_Objest.UnigueID;
            Caption = ПереміщенняТоварів_Objest.Назва;

            try
            {
                if (await ПереміщенняТоварів_Objest.Save())
                {
                    await Товари.SaveRecords();
                    isSave = true;
                }
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(ПереміщенняТоварів_Objest.GetBasis(), Caption, ex);
            }

            return isSave;
        }

        protected override async ValueTask<bool> SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = await ПереміщенняТоварів_Objest.SpendTheDocument(ПереміщенняТоварів_Objest.ДатаДок);

                if (!isSpend)
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(ПереміщенняТоварів_Objest.UnigueID);

                return isSpend;
            }
            else
            {
                await ПереміщенняТоварів_Objest.ClearSpendTheDocument();

                return true;
            }
        }

        protected override void ReportSpendTheDocument(UnigueID unigueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ПереміщенняТоварів_Pointer(unigueID));
        }
    }
}