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
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class РозхіднийКасовийОрдер_Елемент : ДокументЕлемент
    {
        public РозхіднийКасовийОрдер_Objest РозхіднийКасовийОрдер_Objest { get; set; } = new РозхіднийКасовийОрдер_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Організації_PointerControl ОрганізаціяОтримувач = new Організації_PointerControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();
        Каси_PointerControl Каса = new Каси_PointerControl();
        Каси_PointerControl КасаОтримувач = new Каси_PointerControl() { Caption = "Каса отримувач:" };
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

        public РозхіднийКасовийОрдер_Елемент() : base()
        {
            CreateDocName(РозхіднийКасовийОрдер_Const.FULLNAME, НомерДок, ДатаДок);

            CreateField(HBoxComment, "Коментар:", Коментар);

            FillComboBoxes();
        }

        void FillComboBoxes()
        {
            //1
            ConfigurationEnums Конфігурація_ГосподарськіОперації = Config.Kernel!.Conf.Enums["ГосподарськіОперації"];

            ГосподарськаОперація.Append(
                Перелічення.ГосподарськіОперації.ОплатаПостачальнику.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["ОплатаПостачальнику"].Desc);

            ГосподарськаОперація.Append(
                Перелічення.ГосподарськіОперації.ВидачаКоштівВІншуКасу.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["ВидачаКоштівВІншуКасу"].Desc);

            ГосподарськаОперація.Append(
                Перелічення.ГосподарськіОперації.ЗдачаКоштівВБанк.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["ЗдачаКоштівВБанк"].Desc);

            ГосподарськаОперація.Append(
                Перелічення.ГосподарськіОперації.ІншіВитрати.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["ІншіВитрати"].Desc);

            ГосподарськаОперація.Append(
                Перелічення.ГосподарськіОперації.ПоверненняОплатиКлієнту.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["ПоверненняОплатиКлієнту"].Desc);
        }

        protected override void CreateContainer1(VBox vBox)
        {
            //Організація
            CreateField(vBox, null, Організація);

            //Контрагент
            CreateField(vBox, null, Контрагент);

            Контрагент.AfterSelectFunc = () =>
            {
                if (Договір.Pointer.IsEmpty())
                {
                    ДоговориКонтрагентів_Pointer? договірКонтрагента =
                    ФункціїДляДокументів.ОсновнийДоговірДляКонтрагента(Контрагент.Pointer, Перелічення.ТипДоговорів.ЗПостачальниками);

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

                        ДоговориКонтрагентів_Objest? договориКонтрагентів_Objest = Договір.Pointer.GetDirectoryObject();

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

        protected override void CreateContainer2(VBox vBox)
        {
            //ГосподарськаОперація
            CreateField(vBox, "Господарська операція:", ГосподарськаОперація);
            ГосподарськаОперація.Changed += OnComboBoxChanged_ГосподарськаОперація;

            //Каса
            CreateField(vBox, null, Каса);

            //КасаОтримувач
            CreateField(vBox, null, КасаОтримувач);

            //БанківськийРахунок
            CreateField(vBox, null, БанківськийРахунок);

            //СумаДокументу та Курс
            HBox hBox1 = CreateField(vBox, null, СумаДокументу);
            CreateField(hBox1, null, Курс);
        }

        protected override void CreateContainer3(VBox vBox)
        {
            //Автор
            CreateField(vBox, null, Автор);

            //Основа
            CreateField(vBox, null, Основа);
        }

        protected override void CreateContainer4(VBox vBox)
        {
            //СтаттяРухуКоштів
            CreateField(vBox, null, СтаттяРухуКоштів);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
            {
                РозхіднийКасовийОрдер_Objest.New();
                РозхіднийКасовийОрдер_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                РозхіднийКасовийОрдер_Objest.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
                РозхіднийКасовийОрдер_Objest.Каса = ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const; ;
                РозхіднийКасовийОрдер_Objest.Контрагент = ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
                РозхіднийКасовийОрдер_Objest.БанківськийРахунок = ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const;
            }

            if (IsNew || РозхіднийКасовийОрдер_Objest.ГосподарськаОперація == 0)
                РозхіднийКасовийОрдер_Objest.ГосподарськаОперація = Перелічення.ГосподарськіОперації.ОплатаПостачальнику;

            НомерДок.Text = РозхіднийКасовийОрдер_Objest.НомерДок;
            ДатаДок.Value = РозхіднийКасовийОрдер_Objest.ДатаДок;
            Організація.Pointer = РозхіднийКасовийОрдер_Objest.Організація;
            Валюта.Pointer = РозхіднийКасовийОрдер_Objest.Валюта;
            Каса.Pointer = РозхіднийКасовийОрдер_Objest.Каса;
            КасаОтримувач.Pointer = РозхіднийКасовийОрдер_Objest.КасаОтримувач;
            Контрагент.Pointer = РозхіднийКасовийОрдер_Objest.Контрагент;
            Договір.Pointer = РозхіднийКасовийОрдер_Objest.Договір;
            ГосподарськаОперація.ActiveId = РозхіднийКасовийОрдер_Objest.ГосподарськаОперація.ToString();
            Коментар.Text = РозхіднийКасовийОрдер_Objest.Коментар;
            БанківськийРахунок.Pointer = РозхіднийКасовийОрдер_Objest.БанківськийРахунок;
            Автор.Pointer = РозхіднийКасовийОрдер_Objest.Автор;
            СумаДокументу.Value = РозхіднийКасовийОрдер_Objest.СумаДокументу;
            Курс.Value = РозхіднийКасовийОрдер_Objest.Курс;
            СтаттяРухуКоштів.Pointer = РозхіднийКасовийОрдер_Objest.СтаттяРухуКоштів;
            Основа.Pointer = РозхіднийКасовийОрдер_Objest.Основа;

            if (IsNew)
            {
                //Основний договір
                if (Контрагент.AfterSelectFunc != null)
                    Контрагент.AfterSelectFunc.Invoke();
            }
        }

        protected override void GetValue()
        {
            РозхіднийКасовийОрдер_Objest.НомерДок = НомерДок.Text;
            РозхіднийКасовийОрдер_Objest.ДатаДок = ДатаДок.Value;
            РозхіднийКасовийОрдер_Objest.Організація = Організація.Pointer;
            РозхіднийКасовийОрдер_Objest.Валюта = Валюта.Pointer;
            РозхіднийКасовийОрдер_Objest.Каса = Каса.Pointer;
            РозхіднийКасовийОрдер_Objest.КасаОтримувач = КасаОтримувач.Pointer;
            РозхіднийКасовийОрдер_Objest.Контрагент = Контрагент.Pointer;
            РозхіднийКасовийОрдер_Objest.Договір = Договір.Pointer;
            РозхіднийКасовийОрдер_Objest.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            РозхіднийКасовийОрдер_Objest.Коментар = Коментар.Text;
            РозхіднийКасовийОрдер_Objest.БанківськийРахунок = БанківськийРахунок.Pointer;
            РозхіднийКасовийОрдер_Objest.Автор = Автор.Pointer;
            РозхіднийКасовийОрдер_Objest.СумаДокументу = СумаДокументу.Value;
            РозхіднийКасовийОрдер_Objest.Курс = Курс.Value;
            РозхіднийКасовийОрдер_Objest.СтаттяРухуКоштів = СтаттяРухуКоштів.Pointer;
            РозхіднийКасовийОрдер_Objest.Основа = Основа.Pointer;

            РозхіднийКасовийОрдер_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Валюта.Pointer.Назва} {Каса.Pointer.Назва} {КасаОтримувач.Pointer.Назва} {Контрагент.Pointer.Назва}";
        }

        #endregion

        void OnComboBoxChanged_ГосподарськаОперація(object? sender, EventArgs args)
        {
            switch (Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId))
            {
                case Перелічення.ГосподарськіОперації.ВидачаКоштівВІншуКасу:
                    {
                        КасаОтримувач.Sensitive = true;
                        Курс.Sensitive = true;
                        Контрагент.Sensitive = false;
                        Договір.Sensitive = false;
                        БанківськийРахунок.Sensitive = false;

                        break;
                    }
                case Перелічення.ГосподарськіОперації.ЗдачаКоштівВБанк:
                    {
                        КасаОтримувач.Sensitive = false;
                        Курс.Sensitive = false;
                        Контрагент.Sensitive = false;
                        Договір.Sensitive = false;
                        БанківськийРахунок.Sensitive = true;

                        break;
                    }
                default:
                    {
                        КасаОтримувач.Sensitive = false;
                        Курс.Sensitive = false;
                        Контрагент.Sensitive = true;
                        Договір.Sensitive = true;
                        БанківськийРахунок.Sensitive = false;

                        break;
                    }
            }
        }

        protected override bool Save()
        {
            bool isSave;
            
            try
            {
                isSave = РозхіднийКасовийОрдер_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
                return false;
            }

            UnigueID = РозхіднийКасовийОрдер_Objest.UnigueID;
            Caption = РозхіднийКасовийОрдер_Objest.Назва;

            return isSave;
        }

        protected override bool SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = РозхіднийКасовийОрдер_Objest.SpendTheDocument(РозхіднийКасовийОрдер_Objest.ДатаДок);

                if (!isSpend)
                    ФункціїДляПовідомлень.ВідкритиТермінал();

                return isSpend;
            }
            else
            {
                РозхіднийКасовийОрдер_Objest.ClearSpendTheDocument();

                return true;
            }
        }
    }
}