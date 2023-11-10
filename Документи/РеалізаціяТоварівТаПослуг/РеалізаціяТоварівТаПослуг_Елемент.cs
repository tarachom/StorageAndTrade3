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
    class РеалізаціяТоварівТаПослуг_Елемент : ДокументЕлемент
    {
        public РеалізаціяТоварівТаПослуг_Objest РеалізаціяТоварівТаПослуг_Objest { get; set; } = new РеалізаціяТоварівТаПослуг_Objest();

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

        public РеалізаціяТоварівТаПослуг_Елемент() : base()
        {
            CreateDocName(РеалізаціяТоварівТаПослуг_Const.FULLNAME, НомерДок, ДатаДок);

            CreateField(HBoxComment, "Коментар:", Коментар);

            NotebookTablePart.InsertPage(Товари, new Label("Товари"), 0);
            NotebookTablePart.CurrentPage = 0;

            FillComboBoxes();
        }

        void FillComboBoxes()
        {
            //1
            ConfigurationEnums Конфігурація_ГосподарськіОперації = Config.Kernel!.Conf.Enums["ГосподарськіОперації"];

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
                    ФункціїДляДокументів.ОсновнийДоговірДляКонтрагента(Контрагент.Pointer, Перелічення.ТипДоговорів.ЗПокупцями);

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
        }

        protected override void CreateContainer2(VBox vBox)
        {
            //Каса
            CreateField(vBox, null, Каса);

            //Валюта
            CreateField(vBox, null, Валюта);

            //Склад
            CreateField(vBox, null, Склад);
        }

        protected override void CreateContainer3(VBox vBox)
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

        protected override void CreateContainer4(VBox vBox)
        {
            //ФормаОплати
            CreateField(vBox, "Форма оплати:", ФормаОплати);

            //ДатаОплати
            CreateField(vBox, "Дата оплати:", ДатаОплати);

            //Курс та Кратність
            HBox hBox1 = CreateField(vBox, "Курс:", Курс);
            CreateField(hBox1, "Кратність:", Кратність);

            //Узгоджений та ПовернутиТару
            HBox hBox2 = CreateField(vBox, null, Узгоджений);
            CreateField(hBox2, null, ПовернутиТару);

            //ДатаПоверненняТари
            CreateField(vBox, "Дата повернення тари:", ДатаПоверненняТари);

            //СпосібДоставки
            CreateField(vBox, "Спосіб доставки:", СпосібДоставки);

            //ЧасДоставки
            HBox hBox3 = CreateField(vBox, "Час доставки з", ЧасДоставкиЗ);
            CreateField(hBox3, "до", ЧасДоставкиДо);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
            {
                РеалізаціяТоварівТаПослуг_Objest.New();
                РеалізаціяТоварівТаПослуг_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                РеалізаціяТоварівТаПослуг_Objest.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
                РеалізаціяТоварівТаПослуг_Objest.Каса = ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const;
                РеалізаціяТоварівТаПослуг_Objest.Склад = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                РеалізаціяТоварівТаПослуг_Objest.Контрагент = ЗначенняЗаЗамовчуванням.ОсновнийПокупець_Const;
                РеалізаціяТоварівТаПослуг_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
                РеалізаціяТоварівТаПослуг_Objest.БанківськийРахунокОрганізації = ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const;

                РеалізаціяТоварівТаПослуг_Objest.Курс = 1;
                РеалізаціяТоварівТаПослуг_Objest.Кратність = 1;
            }

            НомерДок.Text = РеалізаціяТоварівТаПослуг_Objest.НомерДок;
            ДатаДок.Value = РеалізаціяТоварівТаПослуг_Objest.ДатаДок;
            Організація.Pointer = РеалізаціяТоварівТаПослуг_Objest.Організація;
            Валюта.Pointer = РеалізаціяТоварівТаПослуг_Objest.Валюта;
            Каса.Pointer = РеалізаціяТоварівТаПослуг_Objest.Каса;
            Склад.Pointer = РеалізаціяТоварівТаПослуг_Objest.Склад;
            Контрагент.Pointer = РеалізаціяТоварівТаПослуг_Objest.Контрагент;
            Договір.Pointer = РеалізаціяТоварівТаПослуг_Objest.Договір;
            ГосподарськаОперація.ActiveId = РеалізаціяТоварівТаПослуг_Objest.ГосподарськаОперація.ToString();
            ФормаОплати.ActiveId = РеалізаціяТоварівТаПослуг_Objest.ФормаОплати.ToString();
            Коментар.Text = РеалізаціяТоварівТаПослуг_Objest.Коментар;
            Підрозділ.Pointer = РеалізаціяТоварівТаПослуг_Objest.Підрозділ;
            ДатаОплати.Value = РеалізаціяТоварівТаПослуг_Objest.ДатаОплати;
            БанківськийрахунокОрганізації.Pointer = РеалізаціяТоварівТаПослуг_Objest.БанківськийРахунокОрганізації;
            БанківськийрахунокКонтрагента.Pointer = РеалізаціяТоварівТаПослуг_Objest.БанківськийРахунокКонтрагента;
            Автор.Pointer = РеалізаціяТоварівТаПослуг_Objest.Автор;
            ПовернутиТару.Active = РеалізаціяТоварівТаПослуг_Objest.ПовернутиТару;
            ДатаПоверненняТари.Value = РеалізаціяТоварівТаПослуг_Objest.ДатаПоверненняТари;
            СпосібДоставки.ActiveId = РеалізаціяТоварівТаПослуг_Objest.СпосібДоставки.ToString();
            Курс.Value = РеалізаціяТоварівТаПослуг_Objest.Курс;
            Кратність.Value = РеалізаціяТоварівТаПослуг_Objest.Кратність;
            ЧасДоставкиЗ.Value = РеалізаціяТоварівТаПослуг_Objest.ЧасДоставкиЗ;
            ЧасДоставкиДо.Value = РеалізаціяТоварівТаПослуг_Objest.ЧасДоставкиДо;
            Менеджер.Pointer = РеалізаціяТоварівТаПослуг_Objest.Менеджер;
            Основа.Pointer = РеалізаціяТоварівТаПослуг_Objest.Основа;

            //Таблична частина
            Товари.РеалізаціяТоварівТаПослуг_Objest = РеалізаціяТоварівТаПослуг_Objest;
            Товари.LoadRecords();

            if (IsNew)
            {
                //Основний договір
                if (Контрагент.AfterSelectFunc != null)
                    Контрагент.AfterSelectFunc.Invoke();
            }
        }

        protected override void GetValue()
        {
            РеалізаціяТоварівТаПослуг_Objest.НомерДок = НомерДок.Text;
            РеалізаціяТоварівТаПослуг_Objest.ДатаДок = ДатаДок.Value;
            РеалізаціяТоварівТаПослуг_Objest.Організація = Організація.Pointer;
            РеалізаціяТоварівТаПослуг_Objest.Валюта = Валюта.Pointer;
            РеалізаціяТоварівТаПослуг_Objest.Каса = Каса.Pointer;
            РеалізаціяТоварівТаПослуг_Objest.Склад = Склад.Pointer;
            РеалізаціяТоварівТаПослуг_Objest.Контрагент = Контрагент.Pointer;
            РеалізаціяТоварівТаПослуг_Objest.Договір = Договір.Pointer;
            РеалізаціяТоварівТаПослуг_Objest.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            РеалізаціяТоварівТаПослуг_Objest.ФормаОплати = Enum.Parse<Перелічення.ФормаОплати>(ФормаОплати.ActiveId);
            РеалізаціяТоварівТаПослуг_Objest.Коментар = Коментар.Text;
            РеалізаціяТоварівТаПослуг_Objest.Підрозділ = Підрозділ.Pointer;
            РеалізаціяТоварівТаПослуг_Objest.ДатаОплати = ДатаОплати.Value;
            РеалізаціяТоварівТаПослуг_Objest.БанківськийРахунокОрганізації = БанківськийрахунокОрганізації.Pointer;
            РеалізаціяТоварівТаПослуг_Objest.БанківськийРахунокКонтрагента = БанківськийрахунокКонтрагента.Pointer;
            РеалізаціяТоварівТаПослуг_Objest.Автор = Автор.Pointer;
            РеалізаціяТоварівТаПослуг_Objest.ПовернутиТару = ПовернутиТару.Active;
            РеалізаціяТоварівТаПослуг_Objest.ДатаПоверненняТари = ДатаПоверненняТари.Value;
            РеалізаціяТоварівТаПослуг_Objest.СпосібДоставки = Enum.Parse<Перелічення.СпособиДоставки>(СпосібДоставки.ActiveId);
            РеалізаціяТоварівТаПослуг_Objest.Курс = Курс.Value;
            РеалізаціяТоварівТаПослуг_Objest.Кратність = Кратність.Value;
            РеалізаціяТоварівТаПослуг_Objest.ЧасДоставкиЗ = ЧасДоставкиЗ.Value;
            РеалізаціяТоварівТаПослуг_Objest.ЧасДоставкиДо = ЧасДоставкиДо.Value;
            РеалізаціяТоварівТаПослуг_Objest.Менеджер = Менеджер.Pointer;
            РеалізаціяТоварівТаПослуг_Objest.Основа = Основа.Pointer;

            РеалізаціяТоварівТаПослуг_Objest.СумаДокументу = Товари.СумаДокументу();
            РеалізаціяТоварівТаПослуг_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Валюта.Pointer.Назва} {Каса.Pointer.Назва} {Склад.Pointer.Назва} {Контрагент.Pointer.Назва} ";
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            bool isSave;

            try
            {
                isSave = await РеалізаціяТоварівТаПослуг_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
                return false;
            }

            if (isSave)
                await Товари.SaveRecords();

            UnigueID = РеалізаціяТоварівТаПослуг_Objest.UnigueID;
            Caption = РеалізаціяТоварівТаПослуг_Objest.Назва;

            return isSave;
        }

        protected override async ValueTask<bool> SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = await РеалізаціяТоварівТаПослуг_Objest.SpendTheDocument(РеалізаціяТоварівТаПослуг_Objest.ДатаДок);

                if (!isSpend)
                    ФункціїДляПовідомлень.ВідкритиТермінал();

                return isSpend;
            }
            else
            {
                await РеалізаціяТоварівТаПослуг_Objest.ClearSpendTheDocument();

                return true;
            }
        }
    }
}