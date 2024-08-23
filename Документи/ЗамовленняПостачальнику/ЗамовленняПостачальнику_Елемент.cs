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
    class ЗамовленняПостачальнику_Елемент : ДокументЕлемент
    {
        public ЗамовленняПостачальнику_Objest ЗамовленняПостачальнику_Objest { get; set; } = new ЗамовленняПостачальнику_Objest();

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
        DateTimeControl ДатаПоступлення = new DateTimeControl() { OnlyDate = true };
        БанківськіРахункиОрганізацій_PointerControl БанківськийРахунок = new БанківськіРахункиОрганізацій_PointerControl() { WidthPresentation = 200 };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        CheckButton ПовернутиТару = new CheckButton("Повернути тару:");
        ComboBoxText СпосібДоставки = new ComboBoxText();
        TimeControl ЧасДоставкиЗ = new TimeControl();
        TimeControl ЧасДоставкиДо = new TimeControl();
        Користувачі_PointerControl Менеджер = new Користувачі_PointerControl() { Caption = "Менеджер:" };
        Entry Коментар = new Entry() { WidthRequest = 920 };
        CompositePointerControl Основа = new CompositePointerControl();
        ЗамовленняПостачальнику_ТабличнаЧастина_Товари Товари = new ЗамовленняПостачальнику_ТабличнаЧастина_Товари();

        #endregion

        public ЗамовленняПостачальнику_Елемент() : base()
        {
            CreateDocName(ЗамовленняПостачальнику_Const.FULLNAME, НомерДок, ДатаДок);

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
                Перелічення.ГосподарськіОперації.ПлануванняПоЗамовленнямПостачальнику.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["ПлануванняПоЗамовленнямПостачальнику"].Desc);

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
            //ГосподарськаОперація
            CreateField(vBox, "Господарська операція:", ГосподарськаОперація);

            //Підрозділ
            CreateField(vBox, null, Підрозділ);

            //БанківськийРахунок
            CreateField(vBox, null, БанківськийРахунок);

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

            //ВернутиТару
            CreateField(vBox, null, ПовернутиТару);

            //ДатаПоступлення
            CreateField(vBox, "Дата поступлення:", ДатаПоступлення);

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
                await ЗамовленняПостачальнику_Objest.New();
                ЗамовленняПостачальнику_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                ЗамовленняПостачальнику_Objest.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
                ЗамовленняПостачальнику_Objest.Каса = ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const;
                ЗамовленняПостачальнику_Objest.Склад = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                ЗамовленняПостачальнику_Objest.Контрагент = ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
                ЗамовленняПостачальнику_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
            }

            НомерДок.Text = ЗамовленняПостачальнику_Objest.НомерДок;
            ДатаДок.Value = ЗамовленняПостачальнику_Objest.ДатаДок;
            Організація.Pointer = ЗамовленняПостачальнику_Objest.Організація;
            Валюта.Pointer = ЗамовленняПостачальнику_Objest.Валюта;
            Каса.Pointer = ЗамовленняПостачальнику_Objest.Каса;
            Склад.Pointer = ЗамовленняПостачальнику_Objest.Склад;
            Контрагент.Pointer = ЗамовленняПостачальнику_Objest.Контрагент;
            Договір.Pointer = ЗамовленняПостачальнику_Objest.Договір;
            ГосподарськаОперація.ActiveId = ЗамовленняПостачальнику_Objest.ГосподарськаОперація.ToString();
            ФормаОплати.ActiveId = ЗамовленняПостачальнику_Objest.ФормаОплати.ToString();
            Коментар.Text = ЗамовленняПостачальнику_Objest.Коментар;
            Підрозділ.Pointer = ЗамовленняПостачальнику_Objest.Підрозділ;
            ДатаПоступлення.Value = ЗамовленняПостачальнику_Objest.ДатаПоступлення;
            БанківськийРахунок.Pointer = ЗамовленняПостачальнику_Objest.БанківськийРахунок;
            Автор.Pointer = ЗамовленняПостачальнику_Objest.Автор;
            ПовернутиТару.Active = ЗамовленняПостачальнику_Objest.ПовернутиТару;
            СпосібДоставки.ActiveId = ЗамовленняПостачальнику_Objest.СпосібДоставки.ToString();
            ЧасДоставкиЗ.Value = ЗамовленняПостачальнику_Objest.ЧасДоставкиЗ;
            ЧасДоставкиДо.Value = ЗамовленняПостачальнику_Objest.ЧасДоставкиДо;
            Менеджер.Pointer = ЗамовленняПостачальнику_Objest.Менеджер;
            Основа.Pointer = ЗамовленняПостачальнику_Objest.Основа;

            //Таблична частина
            Товари.ЗамовленняПостачальнику_Objest = ЗамовленняПостачальнику_Objest;
            await Товари.LoadRecords();

            if (IsNew)
            {
                //Основний договір
                Контрагент.AfterSelectFunc?.Invoke();
            }
        }

        protected override void GetValue()
        {
            ЗамовленняПостачальнику_Objest.НомерДок = НомерДок.Text;
            ЗамовленняПостачальнику_Objest.ДатаДок = ДатаДок.Value;
            ЗамовленняПостачальнику_Objest.Організація = Організація.Pointer;
            ЗамовленняПостачальнику_Objest.Валюта = Валюта.Pointer;
            ЗамовленняПостачальнику_Objest.Каса = Каса.Pointer;
            ЗамовленняПостачальнику_Objest.Склад = Склад.Pointer;
            ЗамовленняПостачальнику_Objest.Контрагент = Контрагент.Pointer;
            ЗамовленняПостачальнику_Objest.Договір = Договір.Pointer;
            ЗамовленняПостачальнику_Objest.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            ЗамовленняПостачальнику_Objest.ФормаОплати = Enum.Parse<Перелічення.ФормаОплати>(ФормаОплати.ActiveId);
            ЗамовленняПостачальнику_Objest.Коментар = Коментар.Text;
            ЗамовленняПостачальнику_Objest.Підрозділ = Підрозділ.Pointer;
            ЗамовленняПостачальнику_Objest.ДатаПоступлення = ДатаПоступлення.Value;
            ЗамовленняПостачальнику_Objest.БанківськийРахунок = БанківськийРахунок.Pointer;
            ЗамовленняПостачальнику_Objest.Автор = Автор.Pointer;
            ЗамовленняПостачальнику_Objest.ПовернутиТару = ПовернутиТару.Active;
            ЗамовленняПостачальнику_Objest.СпосібДоставки = Enum.Parse<Перелічення.СпособиДоставки>(СпосібДоставки.ActiveId);
            ЗамовленняПостачальнику_Objest.ЧасДоставкиЗ = ЧасДоставкиЗ.Value;
            ЗамовленняПостачальнику_Objest.ЧасДоставкиДо = ЧасДоставкиДо.Value;
            ЗамовленняПостачальнику_Objest.Менеджер = Менеджер.Pointer;
            ЗамовленняПостачальнику_Objest.Основа = Основа.Pointer;

            ЗамовленняПостачальнику_Objest.СумаДокументу = Товари.СумаДокументу();
            ЗамовленняПостачальнику_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Валюта.Pointer.Назва} {Каса.Pointer.Назва} {Контрагент.Pointer.Назва} ";
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            bool isSave = false;
            UnigueID = ЗамовленняПостачальнику_Objest.UnigueID;
            Caption = ЗамовленняПостачальнику_Objest.Назва;

            try
            {
                if (await ЗамовленняПостачальнику_Objest.Save())
                {
                    await Товари.SaveRecords();
                    isSave = true;
                }
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(ЗамовленняПостачальнику_Objest.GetBasis(), Caption, ex);
            }            

            return isSave;
        }

        protected override async ValueTask<bool> SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = await ЗамовленняПостачальнику_Objest.SpendTheDocument(ЗамовленняПостачальнику_Objest.ДатаДок);

                if (!isSpend)
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(ЗамовленняПостачальнику_Objest.UnigueID);

                return isSpend;
            }
            else
            {
                await ЗамовленняПостачальнику_Objest.ClearSpendTheDocument();
                return true;
            }
        }

        protected override void ReportSpendTheDocument(UnigueID unigueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ЗамовленняПостачальнику_Pointer(unigueID));
        }
    }
}