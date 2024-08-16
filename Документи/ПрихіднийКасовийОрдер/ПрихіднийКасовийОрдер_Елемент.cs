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
        public ПрихіднийКасовийОрдер_Objest ПрихіднийКасовийОрдер_Objest { get; set; } = new ПрихіднийКасовийОрдер_Objest();

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

        public ПрихіднийКасовийОрдер_Елемент() : base()
        {
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

            Контрагент.AfterSelectFunc = async () =>
            {
                if (Договір.Pointer.IsEmpty())
                {
                    ДоговориКонтрагентів_Pointer? договірКонтрагента =
                    await ФункціїДляДокументів.ОсновнийДоговірДляКонтрагента(Контрагент.Pointer, Перелічення.ТипДоговорів.ЗПокупцями);

                    if (договірКонтрагента != null)
                        Договір.Pointer = договірКонтрагента;
                }
                else
                {
                    if (Контрагент.Pointer.IsEmpty())
                        Договір.Pointer = new ДоговориКонтрагентів_Pointer();
                    else
                    {
                        //
                        //Перевірити чи змінився контрагент
                        //

                        ДоговориКонтрагентів_Objest? договориКонтрагентів_Objest = await Договір.Pointer.GetDirectoryObject();

                        if (договориКонтрагентів_Objest != null)
                            if (договориКонтрагентів_Objest.Контрагент != Контрагент.Pointer)
                            {
                                Договір.Pointer = new ДоговориКонтрагентів_Pointer();
                                Контрагент.AfterSelectFunc!.Invoke();
                            };
                    }
                }
            };

            //Договір
            CreateField(vBox, null, Договір);

            Договір.BeforeClickOpenFunc = () =>
            {
                Договір.КонтрагентВласник = Контрагент.Pointer;
            };

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
            Box hBox1 = CreateField(vBox, null, СумаДокументу);
            CreateField(hBox1, null, Курс);
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

        public override async void SetValue()
        {
            if (IsNew)
            {
                await ПрихіднийКасовийОрдер_Objest.New();
                ПрихіднийКасовийОрдер_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                ПрихіднийКасовийОрдер_Objest.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
                ПрихіднийКасовийОрдер_Objest.Каса = ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const; ;
                ПрихіднийКасовийОрдер_Objest.Контрагент = ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
                ПрихіднийКасовийОрдер_Objest.БанківськийРахунок = ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const;
            }

            if (IsNew || ПрихіднийКасовийОрдер_Objest.ГосподарськаОперація == 0)
                ПрихіднийКасовийОрдер_Objest.ГосподарськаОперація = Перелічення.ГосподарськіОперації.ПоступленняОплатиВідКлієнта;

            НомерДок.Text = ПрихіднийКасовийОрдер_Objest.НомерДок;
            ДатаДок.Value = ПрихіднийКасовийОрдер_Objest.ДатаДок;
            Організація.Pointer = ПрихіднийКасовийОрдер_Objest.Організація;
            Валюта.Pointer = ПрихіднийКасовийОрдер_Objest.Валюта;
            Каса.Pointer = ПрихіднийКасовийОрдер_Objest.Каса;
            КасаВідправник.Pointer = ПрихіднийКасовийОрдер_Objest.КасаВідправник;
            Контрагент.Pointer = ПрихіднийКасовийОрдер_Objest.Контрагент;
            Договір.Pointer = ПрихіднийКасовийОрдер_Objest.Договір;
            ГосподарськаОперація.ActiveId = ПрихіднийКасовийОрдер_Objest.ГосподарськаОперація.ToString();
            Коментар.Text = ПрихіднийКасовийОрдер_Objest.Коментар;
            БанківськийРахунок.Pointer = ПрихіднийКасовийОрдер_Objest.БанківськийРахунок;
            Автор.Pointer = ПрихіднийКасовийОрдер_Objest.Автор;
            СумаДокументу.Value = ПрихіднийКасовийОрдер_Objest.СумаДокументу;
            Курс.Value = ПрихіднийКасовийОрдер_Objest.Курс;
            СтаттяРухуКоштів.Pointer = ПрихіднийКасовийОрдер_Objest.СтаттяРухуКоштів;
            Основа.Pointer = ПрихіднийКасовийОрдер_Objest.Основа;

            if (IsNew)
            {
                //Основний договір
                Контрагент.AfterSelectFunc?.Invoke();
            }
        }

        protected override void GetValue()
        {
            ПрихіднийКасовийОрдер_Objest.НомерДок = НомерДок.Text;
            ПрихіднийКасовийОрдер_Objest.ДатаДок = ДатаДок.Value;
            ПрихіднийКасовийОрдер_Objest.Організація = Організація.Pointer;
            ПрихіднийКасовийОрдер_Objest.Валюта = Валюта.Pointer;
            ПрихіднийКасовийОрдер_Objest.Каса = Каса.Pointer;
            ПрихіднийКасовийОрдер_Objest.КасаВідправник = КасаВідправник.Pointer;
            ПрихіднийКасовийОрдер_Objest.Контрагент = Контрагент.Pointer;
            ПрихіднийКасовийОрдер_Objest.Договір = Договір.Pointer;
            ПрихіднийКасовийОрдер_Objest.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            ПрихіднийКасовийОрдер_Objest.Коментар = Коментар.Text;
            ПрихіднийКасовийОрдер_Objest.БанківськийРахунок = БанківськийРахунок.Pointer;
            ПрихіднийКасовийОрдер_Objest.Автор = Автор.Pointer;
            ПрихіднийКасовийОрдер_Objest.СумаДокументу = СумаДокументу.Value;
            ПрихіднийКасовийОрдер_Objest.Курс = Курс.Value;
            ПрихіднийКасовийОрдер_Objest.СтаттяРухуКоштів = СтаттяРухуКоштів.Pointer;
            ПрихіднийКасовийОрдер_Objest.Основа = Основа.Pointer;

            ПрихіднийКасовийОрдер_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку();
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
                isSave = await ПрихіднийКасовийОрдер_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }

            UnigueID = ПрихіднийКасовийОрдер_Objest.UnigueID;
            Caption = ПрихіднийКасовийОрдер_Objest.Назва;

            return isSave;
        }

        protected override async ValueTask<bool> SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = await ПрихіднийКасовийОрдер_Objest.SpendTheDocument(ПрихіднийКасовийОрдер_Objest.ДатаДок);

                if (!isSpend)
                    new ФункціїДляПовідомлень().ПоказатиПовідомлення(ПрихіднийКасовийОрдер_Objest.UnigueID);

                return isSpend;
            }
            else
            {
                await ПрихіднийКасовийОрдер_Objest.ClearSpendTheDocument();
                return true;
            }
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new ПрихіднийКасовийОрдер_Pointer(unigueID);
        }
    }
}