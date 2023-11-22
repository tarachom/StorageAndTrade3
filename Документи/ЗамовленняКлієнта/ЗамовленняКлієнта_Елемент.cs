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
    class ЗамовленняКлієнта_Елемент : ДокументЕлемент
    {
        public ЗамовленняКлієнта_Objest ЗамовленняКлієнта_Objest { get; set; } = new ЗамовленняКлієнта_Objest();

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
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ:" };
        DateTimeControl ДатаВідгрузки = new DateTimeControl() { OnlyDate = true };
        CheckButton Узгоджений = new CheckButton("Узгоджений");
        БанківськіРахункиКонтрагентів_PointerControl БанківськийрахунокКонтрагента = new БанківськіРахункиКонтрагентів_PointerControl() { Caption = "Рахунок контрагента:", WidthPresentation = 200 };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        CheckButton ПовернутиТару = new CheckButton("Вернути тару");
        DateTimeControl ДатаПоверненняТари = new DateTimeControl() { OnlyDate = true };
        ComboBoxText СпосібДоставки = new ComboBoxText();
        TimeControl ЧасДоставкиЗ = new TimeControl();
        TimeControl ЧасДоставкиДо = new TimeControl();
        Користувачі_PointerControl Менеджер = new Користувачі_PointerControl() { Caption = "Менеджер:" };
        Entry Коментар = new Entry() { WidthRequest = 920 };

        ЗамовленняКлієнта_ТабличнаЧастина_Товари Товари = new ЗамовленняКлієнта_ТабличнаЧастина_Товари();

        #endregion

        public ЗамовленняКлієнта_Елемент() : base()
        {
            CreateDocName(ЗамовленняКлієнта_Const.FULLNAME, НомерДок, ДатаДок);

            CreateField(HBoxComment, "Коментар:", Коментар);

            NotebookTablePart.InsertPage(Товари, new Label("Товари"), 0);
            NotebookTablePart.CurrentPage = 0;

            FillComboBoxes();
        }

        void FillComboBoxes()
        {
            if (Config.Kernel != null)
            {
                //1
                ConfigurationEnums Конфігурація_ГосподарськіОперації = Config.Kernel.Conf.Enums["ГосподарськіОперації"];

                ГосподарськаОперація.Append(
                    Перелічення.ГосподарськіОперації.ПлануванняПоЗамовленнямКлієнта.ToString(),
                    Конфігурація_ГосподарськіОперації.Fields["ПлануванняПоЗамовленнямКлієнта"].Desc);

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
        }

        protected override void CreateContainer1(VBox vBox)
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

            //Автор
            CreateField(vBox, null, Автор);

            //Менеджер
            CreateField(vBox, null, Менеджер);
        }

        protected override void CreateContainer4(VBox vBox)
        {
            //ФормаОплати
            CreateField(vBox, "Форма оплати:", ФормаОплати);

            //СпосібДоставки
            CreateField(vBox, "Спосіб доставки:", СпосібДоставки);

            //ДатаВідгрузки
            CreateField(vBox, "Дата відвантаження:", ДатаВідгрузки);

            //ЧасДоставки
            HBox hBox1 = CreateField(vBox, "Час доставки з", ЧасДоставкиЗ);
            CreateField(hBox1, "до", ЧасДоставкиДо);

            //Узгоджений & ВернутиТару
            HBox hBox2 = CreateField(vBox, null, Узгоджений);
            CreateField(hBox2, null, ПовернутиТару);

            //ДатаПоверненняТари
            CreateField(vBox, "Дата повернення тари:", ДатаПоверненняТари);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                await ЗамовленняКлієнта_Objest.New();
                ЗамовленняКлієнта_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                ЗамовленняКлієнта_Objest.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
                ЗамовленняКлієнта_Objest.Каса = ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const;
                ЗамовленняКлієнта_Objest.Склад = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                ЗамовленняКлієнта_Objest.Контрагент = ЗначенняЗаЗамовчуванням.ОсновнийПокупець_Const;
                ЗамовленняКлієнта_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
            }

            НомерДок.Text = ЗамовленняКлієнта_Objest.НомерДок;
            ДатаДок.Value = ЗамовленняКлієнта_Objest.ДатаДок;
            Організація.Pointer = ЗамовленняКлієнта_Objest.Організація;
            Валюта.Pointer = ЗамовленняКлієнта_Objest.Валюта;
            Каса.Pointer = ЗамовленняКлієнта_Objest.Каса;
            Склад.Pointer = ЗамовленняКлієнта_Objest.Склад;
            Контрагент.Pointer = ЗамовленняКлієнта_Objest.Контрагент;
            Договір.Pointer = ЗамовленняКлієнта_Objest.Договір;
            ГосподарськаОперація.ActiveId = ЗамовленняКлієнта_Objest.ГосподарськаОперація.ToString();
            ФормаОплати.ActiveId = ЗамовленняКлієнта_Objest.ФормаОплати.ToString();
            Коментар.Text = ЗамовленняКлієнта_Objest.Коментар;
            Підрозділ.Pointer = ЗамовленняКлієнта_Objest.Підрозділ;
            ДатаВідгрузки.Value = ЗамовленняКлієнта_Objest.ДатаВідгрузки;
            Узгоджений.Active = ЗамовленняКлієнта_Objest.Узгоджений;
            БанківськийрахунокКонтрагента.Pointer = ЗамовленняКлієнта_Objest.БанківськийРахунокКонтрагента;
            Автор.Pointer = ЗамовленняКлієнта_Objest.Автор;
            ПовернутиТару.Active = ЗамовленняКлієнта_Objest.ПовернутиТару;
            ДатаПоверненняТари.Value = ЗамовленняКлієнта_Objest.ДатаПоверненняТари;
            СпосібДоставки.ActiveId = ((Перелічення.СпособиДоставки)ЗамовленняКлієнта_Objest.СпосібДоставки).ToString();
            ЧасДоставкиЗ.Value = ЗамовленняКлієнта_Objest.ЧасДоставкиЗ;
            ЧасДоставкиДо.Value = ЗамовленняКлієнта_Objest.ЧасДоставкиДо;
            Менеджер.Pointer = ЗамовленняКлієнта_Objest.Менеджер;

            //Таблична частина
            Товари.ЗамовленняКлієнта_Objest = ЗамовленняКлієнта_Objest;
            await Товари.LoadRecords();

            if (IsNew)
            {
                //Основний договір
                if (Контрагент.AfterSelectFunc != null)
                    Контрагент.AfterSelectFunc.Invoke();
            }
        }

        protected override void GetValue()
        {
            ЗамовленняКлієнта_Objest.НомерДок = НомерДок.Text;
            ЗамовленняКлієнта_Objest.ДатаДок = ДатаДок.Value;
            ЗамовленняКлієнта_Objest.Організація = Організація.Pointer;
            ЗамовленняКлієнта_Objest.Валюта = Валюта.Pointer;
            ЗамовленняКлієнта_Objest.Каса = Каса.Pointer;
            ЗамовленняКлієнта_Objest.Склад = Склад.Pointer;
            ЗамовленняКлієнта_Objest.Контрагент = Контрагент.Pointer;
            ЗамовленняКлієнта_Objest.Договір = Договір.Pointer;
            ЗамовленняКлієнта_Objest.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            ЗамовленняКлієнта_Objest.ФормаОплати = Enum.Parse<Перелічення.ФормаОплати>(ФормаОплати.ActiveId);
            ЗамовленняКлієнта_Objest.Коментар = Коментар.Text;
            ЗамовленняКлієнта_Objest.Підрозділ = Підрозділ.Pointer;
            ЗамовленняКлієнта_Objest.ДатаВідгрузки = ДатаВідгрузки.Value;
            ЗамовленняКлієнта_Objest.Узгоджений = Узгоджений.Active;
            ЗамовленняКлієнта_Objest.БанківськийРахунокКонтрагента = БанківськийрахунокКонтрагента.Pointer;
            ЗамовленняКлієнта_Objest.Автор = Автор.Pointer;
            ЗамовленняКлієнта_Objest.ПовернутиТару = ПовернутиТару.Active;
            ЗамовленняКлієнта_Objest.ДатаПоверненняТари = ДатаПоверненняТари.Value;
            ЗамовленняКлієнта_Objest.СпосібДоставки = Enum.Parse<Перелічення.СпособиДоставки>(СпосібДоставки.ActiveId);
            ЗамовленняКлієнта_Objest.ЧасДоставкиЗ = ЧасДоставкиЗ.Value;
            ЗамовленняКлієнта_Objest.ЧасДоставкиДо = ЧасДоставкиДо.Value;
            ЗамовленняКлієнта_Objest.Менеджер = Менеджер.Pointer;

            ЗамовленняКлієнта_Objest.СумаДокументу = Товари.СумаДокументу();
            ЗамовленняКлієнта_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
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
                isSave = await ЗамовленняКлієнта_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
                return false;
            }

            if (isSave)
                await Товари.SaveRecords();

            UnigueID = ЗамовленняКлієнта_Objest.UnigueID;
            Caption = ЗамовленняКлієнта_Objest.Назва;

            return isSave;
        }

        protected override async ValueTask<bool> SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = await ЗамовленняКлієнта_Objest.SpendTheDocument(ЗамовленняКлієнта_Objest.ДатаДок);

                if (!isSpend)
                    ФункціїДляПовідомлень.ВідкритиТермінал();

                return isSpend;
            }
            else
            {
                await ЗамовленняКлієнта_Objest.ClearSpendTheDocument();
                return true;
            }
        }
    }
}