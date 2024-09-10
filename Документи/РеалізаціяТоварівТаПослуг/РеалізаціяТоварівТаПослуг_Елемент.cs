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
    class РеалізаціяТоварівТаПослуг_Елемент : ДокументЕлемент
    {
        public РеалізаціяТоварівТаПослуг_Objest Елемент { get; set; } = new РеалізаціяТоварівТаПослуг_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();
        Склади_PointerControl Склад = new Склади_PointerControl();
        Каси_PointerControl Каса = new Каси_PointerControl();
        Контрагенти_PointerControl Контрагент = new Контрагенти_PointerControl();
        ДоговориКонтрагентів_PointerControl Договір = new ДоговориКонтрагентів_PointerControl();
        ComboBoxText ГосподарськаОперація = new ComboBoxText();
        ComboBoxText ФормаОплати = new ComboBoxText();
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ" };
        DateTimeControl ДатаОплати = new DateTimeControl();
        CheckButton Узгоджений = new CheckButton("Узгоджений");
        БанківськіРахункиОрганізацій_PointerControl БанківськийрахунокОрганізації = new БанківськіРахункиОрганізацій_PointerControl() { WidthPresentation = 200 };
        БанківськіРахункиКонтрагентів_PointerControl БанківськийрахунокКонтрагента = new БанківськіРахункиКонтрагентів_PointerControl() { Caption = "Рахунок контрагента", WidthPresentation = 200 };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        CheckButton ПовернутиТару = new CheckButton("Повернути тару");
        DateTimeControl ДатаПоверненняТари = new DateTimeControl();
        ComboBoxText СпосібДоставки = new ComboBoxText();
        NumericControl Курс = new NumericControl();
        IntegerControl Кратність = new IntegerControl();
        TimeControl ЧасДоставкиЗ = new TimeControl();
        TimeControl ЧасДоставкиДо = new TimeControl();
        Користувачі_PointerControl Менеджер = new Користувачі_PointerControl() { Caption = "Менеджер" };
        Entry Коментар = new Entry() { WidthRequest = 920 };
        CompositePointerControl Основа = new CompositePointerControl();

        РеалізаціяТоварівТаПослуг_ТабличнаЧастина_Товари Товари = new РеалізаціяТоварівТаПослуг_ТабличнаЧастина_Товари();

        #endregion

        public РеалізаціяТоварівТаПослуг_Елемент()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;

            CreateDocName(РеалізаціяТоварівТаПослуг_Const.FULLNAME, НомерДок, ДатаДок);
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
                Перелічення.ГосподарськіОперації.РеалізаціяКлієнту.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["РеалізаціяКлієнту"].Desc);

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
            Контрагент.AfterSelectFunc = async () => await Контрагент.ПривязкаДоДоговору(Договір);

            //Договір
            CreateField(vBox, null, Договір);
            Договір.BeforeClickOpenFunc = () => Договір.КонтрагентВласник = Контрагент.Pointer;
        }

        protected override void CreateContainer2(Box vBox)
        {
            //Каса
            CreateField(vBox, null, Каса);

            //Валюта
            CreateField(vBox, null, Валюта);

            //Склад
            CreateField(vBox, null, Склад);
        }

        protected override void CreateContainer3(Box vBox)
        {
            //ГосподарськаОперація
            CreateField(vBox, "Господарська операція:", ГосподарськаОперація);

            //Підрозділ
            CreateField(vBox, null, Підрозділ);

            //БанківськийрахунокКонтрагента
            CreateField(vBox, null, БанківськийрахунокКонтрагента);

            //БанківськийрахунокОрганізації
            CreateField(vBox, null, БанківськийрахунокОрганізації);

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

            //Курс та Кратність
            CreateField(CreateField(vBox, "Курс:", Курс), "Кратність:", Кратність);

            //Узгоджений та ПовернутиТару
            CreateField(CreateField(vBox, null, Узгоджений), null, ПовернутиТару);

            //ДатаПоверненняТари
            CreateField(vBox, "Дата повернення тари:", ДатаПоверненняТари);

            //СпосібДоставки
            CreateField(vBox, "Спосіб доставки:", СпосібДоставки);

            //ЧасДоставки
            CreateField(CreateField(vBox, "Час доставки з", ЧасДоставкиЗ), "до", ЧасДоставкиДо);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                Елемент.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                Елемент.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
                Елемент.Каса = ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const;
                Елемент.Склад = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                Елемент.Контрагент = ЗначенняЗаЗамовчуванням.ОсновнийПокупець_Const;
                Елемент.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
                Елемент.БанківськийРахунокОрганізації = ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const;

                Елемент.Курс = 1;
                Елемент.Кратність = 1;
            }

            НомерДок.Text = Елемент.НомерДок;
            ДатаДок.Value = Елемент.ДатаДок;
            Організація.Pointer = Елемент.Організація;
            Валюта.Pointer = Елемент.Валюта;
            Каса.Pointer = Елемент.Каса;
            Склад.Pointer = Елемент.Склад;
            Контрагент.Pointer = Елемент.Контрагент;
            Договір.Pointer = Елемент.Договір;
            ГосподарськаОперація.ActiveId = Елемент.ГосподарськаОперація.ToString();
            ФормаОплати.ActiveId = Елемент.ФормаОплати.ToString();
            Коментар.Text = Елемент.Коментар;
            Підрозділ.Pointer = Елемент.Підрозділ;
            ДатаОплати.Value = Елемент.ДатаОплати;
            БанківськийрахунокОрганізації.Pointer = Елемент.БанківськийРахунокОрганізації;
            БанківськийрахунокКонтрагента.Pointer = Елемент.БанківськийРахунокКонтрагента;
            Автор.Pointer = Елемент.Автор;
            ПовернутиТару.Active = Елемент.ПовернутиТару;
            ДатаПоверненняТари.Value = Елемент.ДатаПоверненняТари;
            СпосібДоставки.ActiveId = Елемент.СпосібДоставки.ToString();
            Курс.Value = Елемент.Курс;
            Кратність.Value = Елемент.Кратність;
            ЧасДоставкиЗ.Value = Елемент.ЧасДоставкиЗ;
            ЧасДоставкиДо.Value = Елемент.ЧасДоставкиДо;
            Менеджер.Pointer = Елемент.Менеджер;
            Основа.Pointer = Елемент.Основа;

            //Таблична частина
            Товари.ЕлементВласник = Елемент;
            await Товари.LoadRecords();

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
            Елемент.Склад = Склад.Pointer;
            Елемент.Контрагент = Контрагент.Pointer;
            Елемент.Договір = Договір.Pointer;
            Елемент.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            Елемент.ФормаОплати = Enum.Parse<Перелічення.ФормаОплати>(ФормаОплати.ActiveId);
            Елемент.Коментар = Коментар.Text;
            Елемент.Підрозділ = Підрозділ.Pointer;
            Елемент.ДатаОплати = ДатаОплати.Value;
            Елемент.БанківськийРахунокОрганізації = БанківськийрахунокОрганізації.Pointer;
            Елемент.БанківськийРахунокКонтрагента = БанківськийрахунокКонтрагента.Pointer;
            Елемент.Автор = Автор.Pointer;
            Елемент.ПовернутиТару = ПовернутиТару.Active;
            Елемент.ДатаПоверненняТари = ДатаПоверненняТари.Value;
            Елемент.СпосібДоставки = Enum.Parse<Перелічення.СпособиДоставки>(СпосібДоставки.ActiveId);
            Елемент.Курс = Курс.Value;
            Елемент.Кратність = Кратність.Value;
            Елемент.ЧасДоставкиЗ = ЧасДоставкиЗ.Value;
            Елемент.ЧасДоставкиДо = ЧасДоставкиДо.Value;
            Елемент.Менеджер = Менеджер.Pointer;
            Елемент.Основа = Основа.Pointer;

            Елемент.СумаДокументу = Товари.СумаДокументу();
            Елемент.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Валюта.Pointer.Назва} {Каса.Pointer.Назва} {Склад.Pointer.Назва} {Контрагент.Pointer.Назва} ";
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            bool isSave = false;

            try
            {
                if (await Елемент.Save())
                {
                    await Товари.SaveRecords();
                    isSave = true;
                }
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new РеалізаціяТоварівТаПослуг_Pointer(unigueID));
        }
    }
}