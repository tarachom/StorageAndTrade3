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
    class ПоступленняТоварівТаПослуг_Елемент : ДокументЕлемент
    {
        public ПоступленняТоварівТаПослуг_Objest ПоступленняТоварівТаПослуг_Objest { get; set; } = new ПоступленняТоварівТаПослуг_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();
        Склади_PointerControl Склад = new Склади_PointerControl();
        Каси_PointerControl Каса = new Каси_PointerControl();
        Контрагенти_PointerControl Контрагент = new Контрагенти_PointerControl();
        ДоговориКонтрагентів_PointerControl Договір = new ДоговориКонтрагентів_PointerControl();
        ЗамовленняПостачальнику_PointerControl ЗамовленняПостачальнику = new ЗамовленняПостачальнику_PointerControl() { Caption = "Замовлення постачальнику:" };
        ComboBoxText ГосподарськаОперація = new ComboBoxText();
        ComboBoxText ФормаОплати = new ComboBoxText();
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ:" };
        DateTimeControl ДатаОплати = new DateTimeControl() { OnlyDate = true };
        CheckButton Узгоджений = new CheckButton("Узгоджений");
        БанківськіРахункиОрганізацій_PointerControl БанківськийРахунокОрганізації = new БанківськіРахункиОрганізацій_PointerControl() { WidthPresentation = 200 };
        БанківськіРахункиКонтрагентів_PointerControl БанківськийРахунокКонтрагента = new БанківськіРахункиКонтрагентів_PointerControl() { Caption = "Рахунок контрагента:", WidthPresentation = 200 };
        Entry НомерВхідногоДокументу = new Entry() { WidthRequest = 200 };
        DateTimeControl ДатаВхідногоДокументу = new DateTimeControl() { OnlyDate = true };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        CheckButton ПовернутиТару = new CheckButton("Вернути тару:");
        DateTimeControl ДатаПоверненняТари = new DateTimeControl() { OnlyDate = true };
        ComboBoxText СпосібДоставки = new ComboBoxText();
        NumericControl Курс = new NumericControl();
        IntegerControl Кратність = new IntegerControl();
        TimeControl ЧасДоставкиЗ = new TimeControl();
        TimeControl ЧасДоставкиДо = new TimeControl();
        Користувачі_PointerControl Менеджер = new Користувачі_PointerControl() { Caption = "Менеджер:" };
        СтаттяРухуКоштів_PointerControl СтаттяРухуКоштів = new СтаттяРухуКоштів_PointerControl();
        Entry Коментар = new Entry() { WidthRequest = 920 };
        CompositePointerControl Основа = new CompositePointerControl() { BoundConfType = "Документи.ПоступленняТоварівТаПослуг.Основа" };

        ПоступленняТоварівТаПослуг_ТабличнаЧастина_Товари Товари = new ПоступленняТоварівТаПослуг_ТабличнаЧастина_Товари();

        #endregion

        public ПоступленняТоварівТаПослуг_Елемент() : base()
        {
            CreateDocName(ПоступленняТоварівТаПослуг_Const.FULLNAME, НомерДок, ДатаДок);

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
                Перелічення.ГосподарськіОперації.ЗакупівляВПостачальника.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["ЗакупівляВПостачальника"].Desc);

            ГосподарськаОперація.Active = 0;

            //2
            foreach (var field in Перелічення.ПсевдонімиПерелічення.ФормаОплати_List())
                ФормаОплати.Append(field.Value.ToString(), field.Name);

            ФормаОплати.ActiveId = Перелічення.ФормаОплати.Готівка.ToString();

            //3
            foreach (var field in Перелічення.ПсевдонімиПерелічення.СпособиДоставки_List())
                СпосібДоставки.Append(field.Value.ToString(), field.Name);

            СпосібДоставки.ActiveId = Перелічення.СпособиДоставки.Самовивіз.ToString();
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
                    await ФункціїДляДокументів.ОсновнийДоговірДляКонтрагента(Контрагент.Pointer, Перелічення.ТипДоговорів.ЗПостачальниками);

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
        }

        protected override void CreateContainer2(Box vBox)
        {
            //Склад
            CreateField(vBox, null, Склад);

            //Каса
            CreateField(vBox, null, Каса);

            //Валюта
            CreateField(vBox, null, Валюта);
        }

        protected override void CreateContainer3(Box vBox)
        {
            //ЗамовленняПостачальнику
            CreateField(vBox, null, ЗамовленняПостачальнику);

            //ГосподарськаОперація
            CreateField(vBox, "Господарська операція:", ГосподарськаОперація);

            //Підрозділ
            CreateField(vBox, null, Підрозділ);

            //БанківськийрахунокКонтрагента
            CreateField(vBox, null, БанківськийРахунокКонтрагента);

            //БанківськийрахунокОрганізації
            CreateField(vBox, null, БанківськийРахунокОрганізації);

            //СтаттяРухуКоштів
            CreateField(vBox, null, СтаттяРухуКоштів);

            //Автор
            CreateField(vBox, null, Автор);

            //Менеджер
            CreateField(vBox, null, Менеджер);

            //Основа
            CreateField(vBox, null, Основа);
        }

        protected override void CreateContainer4(Box vBox)
        {
            //ФормаОплати
            CreateField(vBox, "Форма оплати:", ФормаОплати);

            //ДатаОплати
            CreateField(vBox, "Дата оплати:", ДатаОплати);

            //Узгоджений та ВернутиТару
            Box hBox1 = CreateField(vBox, null, Узгоджений);
            CreateField(hBox1, null, ПовернутиТару);

            //ДатаПоверненняТари
            CreateField(vBox, "Дата повернення тари:", ДатаПоверненняТари);

            //НомерВхідногоДокументу
            CreateField(vBox, "Номер вхід. док:", НомерВхідногоДокументу);

            //ДатаВхідногоДокументу
            CreateField(vBox, "Дата вхід. док:", ДатаВхідногоДокументу);

            //Курс та Кратність
            Box hBox2 = CreateField(vBox, "Курс:", Курс);
            CreateField(hBox2, "Кратність:", Кратність);

            //СпосібДоставки
            CreateField(vBox, "Спосіб доставки:", СпосібДоставки);

            //ЧасДоставки
            Box hBox3 = CreateField(vBox, "Час доставки з", ЧасДоставкиЗ);
            CreateField(hBox3, "до", ЧасДоставкиДо);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                await ПоступленняТоварівТаПослуг_Objest.New();
                ПоступленняТоварівТаПослуг_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                ПоступленняТоварівТаПослуг_Objest.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
                ПоступленняТоварівТаПослуг_Objest.Каса = ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const;
                ПоступленняТоварівТаПослуг_Objest.Склад = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                ПоступленняТоварівТаПослуг_Objest.Контрагент = ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
                ПоступленняТоварівТаПослуг_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
                ПоступленняТоварівТаПослуг_Objest.БанківськийРахунокОрганізації = ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const;
            }

            НомерДок.Text = ПоступленняТоварівТаПослуг_Objest.НомерДок;
            ДатаДок.Value = ПоступленняТоварівТаПослуг_Objest.ДатаДок;
            Організація.Pointer = ПоступленняТоварівТаПослуг_Objest.Організація;
            Валюта.Pointer = ПоступленняТоварівТаПослуг_Objest.Валюта;
            Каса.Pointer = ПоступленняТоварівТаПослуг_Objest.Каса;
            Склад.Pointer = ПоступленняТоварівТаПослуг_Objest.Склад;
            Контрагент.Pointer = ПоступленняТоварівТаПослуг_Objest.Контрагент;
            Договір.Pointer = ПоступленняТоварівТаПослуг_Objest.Договір;
            ГосподарськаОперація.ActiveId = ПоступленняТоварівТаПослуг_Objest.ГосподарськаОперація.ToString();
            ФормаОплати.ActiveId = ПоступленняТоварівТаПослуг_Objest.ФормаОплати.ToString();
            Коментар.Text = ПоступленняТоварівТаПослуг_Objest.Коментар;
            Підрозділ.Pointer = ПоступленняТоварівТаПослуг_Objest.Підрозділ;
            ДатаОплати.Value = ПоступленняТоварівТаПослуг_Objest.ДатаОплати;
            Узгоджений.Active = ПоступленняТоварівТаПослуг_Objest.Узгоджений;
            БанківськийРахунокОрганізації.Pointer = ПоступленняТоварівТаПослуг_Objest.БанківськийРахунокОрганізації;
            БанківськийРахунокКонтрагента.Pointer = ПоступленняТоварівТаПослуг_Objest.БанківськийРахунокКонтрагента;
            НомерВхідногоДокументу.Text = ПоступленняТоварівТаПослуг_Objest.НомерВхідногоДокументу;
            ДатаВхідногоДокументу.Value = ПоступленняТоварівТаПослуг_Objest.ДатаВхідногоДокументу;
            Автор.Pointer = ПоступленняТоварівТаПослуг_Objest.Автор;
            ПовернутиТару.Active = ПоступленняТоварівТаПослуг_Objest.ПовернутиТару;
            ДатаПоверненняТари.Value = ПоступленняТоварівТаПослуг_Objest.ДатаПоверненняТари;
            СпосібДоставки.ActiveId = ПоступленняТоварівТаПослуг_Objest.СпосібДоставки.ToString();
            Курс.Value = ПоступленняТоварівТаПослуг_Objest.Курс;
            Кратність.Value = ПоступленняТоварівТаПослуг_Objest.Кратність;
            ЧасДоставкиЗ.Value = ПоступленняТоварівТаПослуг_Objest.ЧасДоставкиЗ;
            ЧасДоставкиДо.Value = ПоступленняТоварівТаПослуг_Objest.ЧасДоставкиДо;
            Менеджер.Pointer = ПоступленняТоварівТаПослуг_Objest.Менеджер;
            СтаттяРухуКоштів.Pointer = ПоступленняТоварівТаПослуг_Objest.СтаттяРухуКоштів;
            Основа.Pointer = ПоступленняТоварівТаПослуг_Objest.Основа;

            //Таблична частина
            Товари.ПоступленняТоварівТаПослуг_Objest = ПоступленняТоварівТаПослуг_Objest;
            await Товари.LoadRecords();

            if (IsNew)
            {
                //Основний договір
                Контрагент.AfterSelectFunc?.Invoke();
            }
        }

        protected override void GetValue()
        {
            ПоступленняТоварівТаПослуг_Objest.НомерДок = НомерДок.Text;
            ПоступленняТоварівТаПослуг_Objest.ДатаДок = ДатаДок.Value;
            ПоступленняТоварівТаПослуг_Objest.Організація = Організація.Pointer;
            ПоступленняТоварівТаПослуг_Objest.Валюта = Валюта.Pointer;
            ПоступленняТоварівТаПослуг_Objest.Каса = Каса.Pointer;
            ПоступленняТоварівТаПослуг_Objest.Склад = Склад.Pointer;
            ПоступленняТоварівТаПослуг_Objest.Контрагент = Контрагент.Pointer;
            ПоступленняТоварівТаПослуг_Objest.Договір = Договір.Pointer;
            ПоступленняТоварівТаПослуг_Objest.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            ПоступленняТоварівТаПослуг_Objest.ФормаОплати = Enum.Parse<Перелічення.ФормаОплати>(ФормаОплати.ActiveId);
            ПоступленняТоварівТаПослуг_Objest.Коментар = Коментар.Text;
            ПоступленняТоварівТаПослуг_Objest.Підрозділ = Підрозділ.Pointer;
            ПоступленняТоварівТаПослуг_Objest.ДатаОплати = ДатаОплати.Value;
            ПоступленняТоварівТаПослуг_Objest.Узгоджений = Узгоджений.Active;
            ПоступленняТоварівТаПослуг_Objest.БанківськийРахунокОрганізації = БанківськийРахунокОрганізації.Pointer;
            ПоступленняТоварівТаПослуг_Objest.БанківськийРахунокКонтрагента = БанківськийРахунокКонтрагента.Pointer;
            ПоступленняТоварівТаПослуг_Objest.НомерВхідногоДокументу = НомерВхідногоДокументу.Text;
            ПоступленняТоварівТаПослуг_Objest.ДатаВхідногоДокументу = ДатаВхідногоДокументу.Value;
            ПоступленняТоварівТаПослуг_Objest.Автор = Автор.Pointer;
            ПоступленняТоварівТаПослуг_Objest.ПовернутиТару = ПовернутиТару.Active;
            ПоступленняТоварівТаПослуг_Objest.ДатаПоверненняТари = ДатаПоверненняТари.Value;
            ПоступленняТоварівТаПослуг_Objest.СпосібДоставки = Enum.Parse<Перелічення.СпособиДоставки>(СпосібДоставки.ActiveId);
            ПоступленняТоварівТаПослуг_Objest.Курс = Курс.Value;
            ПоступленняТоварівТаПослуг_Objest.Кратність = Кратність.Value;
            ПоступленняТоварівТаПослуг_Objest.ЧасДоставкиЗ = ЧасДоставкиЗ.Value;
            ПоступленняТоварівТаПослуг_Objest.ЧасДоставкиДо = ЧасДоставкиДо.Value;
            ПоступленняТоварівТаПослуг_Objest.Менеджер = Менеджер.Pointer;
            ПоступленняТоварівТаПослуг_Objest.СтаттяРухуКоштів = СтаттяРухуКоштів.Pointer;
            ПоступленняТоварівТаПослуг_Objest.Основа = Основа.Pointer;

            ПоступленняТоварівТаПослуг_Objest.СумаДокументу = Товари.СумаДокументу();
            ПоступленняТоварівТаПослуг_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Валюта.Pointer.Назва} {Каса.Pointer.Назва} {Склад.Pointer.Назва} {Контрагент.Pointer.Назва}";
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            bool isSave = false;

            try
            {
                if (await ПоступленняТоварівТаПослуг_Objest.Save())
                {
                    await Товари.SaveRecords();
                    isSave = true;
                }
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }

            UnigueID = ПоступленняТоварівТаПослуг_Objest.UnigueID;
            Caption = ПоступленняТоварівТаПослуг_Objest.Назва;

            return isSave;
        }

        protected override async ValueTask<bool> SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = await ПоступленняТоварівТаПослуг_Objest.SpendTheDocument(ПоступленняТоварівТаПослуг_Objest.ДатаДок);

                if (!isSpend)
                    new ФункціїДляПовідомлень().ПоказатиПовідомлення(ПоступленняТоварівТаПослуг_Objest.UnigueID);

                return isSpend;
            }
            else
            {
                await ПоступленняТоварівТаПослуг_Objest.ClearSpendTheDocument();
                return true;
            }
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new ПоступленняТоварівТаПослуг_Pointer(unigueID);
        }
    }
}