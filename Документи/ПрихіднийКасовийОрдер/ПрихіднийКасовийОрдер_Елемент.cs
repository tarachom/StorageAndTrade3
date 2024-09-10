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
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class ПрихіднийКасовийОрдер_Елемент : ДокументЕлемент
    {
        public ПрихіднийКасовийОрдер_Objest Елемент { get; set; } = new ПрихіднийКасовийОрдер_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();
        Каси_PointerControl Каса = new Каси_PointerControl();
        Каси_PointerControl КасаВідправник = new Каси_PointerControl() { Caption = "Каса відправник:" };
        NumericControl Курс = new NumericControl() { Caption = "Курс:" };
        NumericControl СумаДокументу = new NumericControl() { Caption = "Сума:" };
        Контрагенти_PointerControl Контрагент = new Контрагенти_PointerControl();
        ДоговориКонтрагентів_PointerControl Договір = new ДоговориКонтрагентів_PointerControl();
        ComboBoxText ГосподарськаОперація = new ComboBoxText();
        БанківськіРахункиОрганізацій_PointerControl БанківськийРахунок = new БанківськіРахункиОрганізацій_PointerControl() { Caption = "Банківський рахунок:" };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        СтаттяРухуКоштів_PointerControl СтаттяРухуКоштів = new СтаттяРухуКоштів_PointerControl();
        Entry Коментар = new Entry() { WidthRequest = 920 };
        CompositePointerControl Основа = new CompositePointerControl();

        #endregion

        public ПрихіднийКасовийОрдер_Елемент()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;

            CreateDocName(ПрихіднийКасовийОрдер_Const.FULLNAME, НомерДок, ДатаДок);
            CreateField(HBoxComment, "Коментар:", Коментар);

            FillComboBoxes();
        }

        void FillComboBoxes()
        {
            //1
            ConfigurationEnums Конфігурація_ГосподарськіОперації = Config.Kernel.Conf.Enums["ГосподарськіОперації"];

            ГосподарськаОперація.Append(
                Перелічення.ГосподарськіОперації.ПоступленняОплатиВідКлієнта.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["ПоступленняОплатиВідКлієнта"].Desc);

            ГосподарськаОперація.Append(
                Перелічення.ГосподарськіОперації.ПоступленняКоштівЗІншоїКаси.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["ПоступленняКоштівЗІншоїКаси"].Desc);

            ГосподарськаОперація.Append(
                Перелічення.ГосподарськіОперації.ПоступленняКоштівЗБанку.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["ПоступленняКоштівЗБанку"].Desc);

            ГосподарськаОперація.Append(
                Перелічення.ГосподарськіОперації.ПоверненняКоштівПостачальнику.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["ПоверненняКоштівПостачальнику"].Desc);

            ГосподарськаОперація.Append(
                Перелічення.ГосподарськіОперації.ІншіДоходи.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["ІншіДоходи"].Desc);
        }

        protected override void CreateContainer1(Box vBox)
        {
            //Організація
            CreateField(vBox, null, Організація);

            //Контрагент
            CreateField(vBox, null, Контрагент);
            Контрагент.AfterSelectFunc = async () => await Контрагент.ПривязкаДоДоговору(Договір);

            //Договір
            CreateField(vBox, null, Договір);
            Договір.BeforeClickOpenFunc = () => Договір.КонтрагентВласник = Контрагент.Pointer;

            //Валюта
            CreateField(vBox, null, Валюта);
        }

        protected override void CreateContainer2(Box vBox)
        {
            //ГосподарськаОперація
            CreateField(vBox, "Господарська операція:", ГосподарськаОперація);
            ГосподарськаОперація.Changed += OnComboBoxChanged_ГосподарськаОперація;

            //Каса
            CreateField(vBox, null, Каса);

            //КасаВідправник
            CreateField(vBox, null, КасаВідправник);

            //БанківськийРахунок
            CreateField(vBox, null, БанківськийРахунок);

            //СумаДокументу & Курс
            CreateField(CreateField(vBox, null, СумаДокументу), null, Курс);
        }

        protected override void CreateContainer3(Box vBox)
        {
            //Автор
            CreateField(vBox, null, Автор);

            //Основа
            CreateField(vBox, null, Основа);
        }

        protected override void CreateContainer4(Box vBox)
        {
            //СтаттяРухуКоштів
            CreateField(vBox, null, СтаттяРухуКоштів);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
            {
                Елемент.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                Елемент.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
                Елемент.Каса = ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const; ;
                Елемент.Контрагент = ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
                Елемент.БанківськийРахунок = ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const;
            }

            if (IsNew || Елемент.ГосподарськаОперація == 0)
                Елемент.ГосподарськаОперація = Перелічення.ГосподарськіОперації.ПоступленняОплатиВідКлієнта;

            НомерДок.Text = Елемент.НомерДок;
            ДатаДок.Value = Елемент.ДатаДок;
            Організація.Pointer = Елемент.Організація;
            Валюта.Pointer = Елемент.Валюта;
            Каса.Pointer = Елемент.Каса;
            КасаВідправник.Pointer = Елемент.КасаВідправник;
            Контрагент.Pointer = Елемент.Контрагент;
            Договір.Pointer = Елемент.Договір;
            ГосподарськаОперація.ActiveId = Елемент.ГосподарськаОперація.ToString();
            Коментар.Text = Елемент.Коментар;
            БанківськийРахунок.Pointer = Елемент.БанківськийРахунок;
            Автор.Pointer = Елемент.Автор;
            СумаДокументу.Value = Елемент.СумаДокументу;
            Курс.Value = Елемент.Курс;
            СтаттяРухуКоштів.Pointer = Елемент.СтаттяРухуКоштів;
            Основа.Pointer = Елемент.Основа;

            if (IsNew)
            {
                //Основний договір
                Контрагент.AfterSelectFunc?.Invoke();
            }
        }

        protected override void GetValue()
        {
            Елемент.НомерДок = НомерДок.Text;
            Елемент.ДатаДок = ДатаДок.Value;
            Елемент.Організація = Організація.Pointer;
            Елемент.Валюта = Валюта.Pointer;
            Елемент.Каса = Каса.Pointer;
            Елемент.КасаВідправник = КасаВідправник.Pointer;
            Елемент.Контрагент = Контрагент.Pointer;
            Елемент.Договір = Договір.Pointer;
            Елемент.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            Елемент.Коментар = Коментар.Text;
            Елемент.БанківськийРахунок = БанківськийРахунок.Pointer;
            Елемент.Автор = Автор.Pointer;
            Елемент.СумаДокументу = СумаДокументу.Value;
            Елемент.Курс = Курс.Value;
            Елемент.СтаттяРухуКоштів = СтаттяРухуКоштів.Pointer;
            Елемент.Основа = Основа.Pointer;

            Елемент.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Валюта.Pointer.Назва} {Каса.Pointer.Назва} {КасаВідправник.Pointer.Назва} {Контрагент.Pointer.Назва}";
        }

        #endregion

        void OnComboBoxChanged_ГосподарськаОперація(object? sender, EventArgs args)
        {
            switch (Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId))
            {
                case Перелічення.ГосподарськіОперації.ПоступленняКоштівЗІншоїКаси:
                    {
                        КасаВідправник.Sensitive = true;
                        Курс.Sensitive = true;
                        Контрагент.Sensitive = false;
                        Договір.Sensitive = false;
                        БанківськийРахунок.Sensitive = false;

                        break;
                    }
                case Перелічення.ГосподарськіОперації.ПоступленняКоштівЗБанку:
                    {
                        КасаВідправник.Sensitive = false;
                        Курс.Sensitive = false;
                        Контрагент.Sensitive = false;
                        Договір.Sensitive = false;
                        БанківськийРахунок.Sensitive = true;

                        break;
                    }
                default:
                    {
                        КасаВідправник.Sensitive = false;
                        Курс.Sensitive = false;
                        Контрагент.Sensitive = true;
                        Договір.Sensitive = true;
                        БанківськийРахунок.Sensitive = false;

                        break;
                    }
            }
        }

        protected override async ValueTask<bool> Save()
        {
            bool isSave = false;

            try
            {
                isSave = await Елемент.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Елемент.GetBasis(), Caption, ex);
            }

            return isSave;
        }

        protected override async ValueTask<bool> SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = await Елемент.SpendTheDocument(Елемент.ДатаДок);

                if (!isSpend)
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(Елемент.UnigueID);

                return isSpend;
            }
            else
            {
                await Елемент.ClearSpendTheDocument();
                return true;
            }
        }

        protected override void ReportSpendTheDocument(UnigueID unigueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ПрихіднийКасовийОрдер_Pointer(unigueID));
        }
    }
}