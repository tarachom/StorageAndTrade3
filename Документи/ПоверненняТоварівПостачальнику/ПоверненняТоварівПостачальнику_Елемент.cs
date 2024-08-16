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
    class ПоверненняТоварівПостачальнику_Елемент : ДокументЕлемент
    {
        public ПоверненняТоварівПостачальнику_Objest ПоверненняТоварівПостачальнику_Objest { get; set; } = new ПоверненняТоварівПостачальнику_Objest();

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
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ" };
        БанківськіРахункиОрганізацій_PointerControl БанківськийРахунокОрганізації = new БанківськіРахункиОрганізацій_PointerControl() { WidthPresentation = 200 };
        БанківськіРахункиКонтрагентів_PointerControl БанківськийРахунокКонтрагента = new БанківськіРахункиКонтрагентів_PointerControl() { Caption = "Рахунок контрагента", WidthPresentation = 200 };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        ComboBoxText СпосібДоставки = new ComboBoxText();
        TimeControl ЧасДоставкиЗ = new TimeControl();
        TimeControl ЧасДоставкиДо = new TimeControl();
        Користувачі_PointerControl Менеджер = new Користувачі_PointerControl() { Caption = "Менеджер" };
        Entry Коментар = new Entry() { WidthRequest = 920 };
        CompositePointerControl Основа = new CompositePointerControl();

        ПоверненняТоварівПостачальнику_ТабличнаЧастина_Товари Товари = new ПоверненняТоварівПостачальнику_ТабличнаЧастина_Товари();

        #endregion

        public ПоверненняТоварівПостачальнику_Елемент() : base()
        {
            CreateDocName(ПоверненняТоварівПостачальнику_Const.FULLNAME, НомерДок, ДатаДок);

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
                Перелічення.ГосподарськіОперації.ПоверненняТоварівПостачальнику.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["ПоверненняТоварівПостачальнику"].Desc);

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
            //Підрозділ
            CreateField(vBox, null, Підрозділ);

            //БанківськийрахунокКонтрагента
            CreateField(vBox, null, БанківськийРахунокКонтрагента);

            //БанківськийрахунокОрганізації
            CreateField(vBox, null, БанківськийРахунокОрганізації);

            //Автор
            CreateField(vBox, null, Автор);

            //Менеджер
            CreateField(vBox, null, Менеджер);

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
                await ПоверненняТоварівПостачальнику_Objest.New();
                ПоверненняТоварівПостачальнику_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                ПоверненняТоварівПостачальнику_Objest.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
                ПоверненняТоварівПостачальнику_Objest.Каса = ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const;
                ПоверненняТоварівПостачальнику_Objest.Склад = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                ПоверненняТоварівПостачальнику_Objest.Контрагент = ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
                ПоверненняТоварівПостачальнику_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
                ПоверненняТоварівПостачальнику_Objest.БанківськийРахунокОрганізації = ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const;
            }

            НомерДок.Text = ПоверненняТоварівПостачальнику_Objest.НомерДок;
            ДатаДок.Value = ПоверненняТоварівПостачальнику_Objest.ДатаДок;
            Організація.Pointer = ПоверненняТоварівПостачальнику_Objest.Організація;
            Валюта.Pointer = ПоверненняТоварівПостачальнику_Objest.Валюта;
            Каса.Pointer = ПоверненняТоварівПостачальнику_Objest.Каса;
            Склад.Pointer = ПоверненняТоварівПостачальнику_Objest.Склад;
            Контрагент.Pointer = ПоверненняТоварівПостачальнику_Objest.Контрагент;
            Договір.Pointer = ПоверненняТоварівПостачальнику_Objest.Договір;
            ГосподарськаОперація.ActiveId = ПоверненняТоварівПостачальнику_Objest.ГосподарськаОперація.ToString();
            Коментар.Text = ПоверненняТоварівПостачальнику_Objest.Коментар;
            Підрозділ.Pointer = ПоверненняТоварівПостачальнику_Objest.Підрозділ;
            БанківськийРахунокОрганізації.Pointer = ПоверненняТоварівПостачальнику_Objest.БанківськийРахунокОрганізації;
            БанківськийРахунокКонтрагента.Pointer = ПоверненняТоварівПостачальнику_Objest.БанківськийРахунокКонтрагента;
            Автор.Pointer = ПоверненняТоварівПостачальнику_Objest.Автор;
            СпосібДоставки.ActiveId = ПоверненняТоварівПостачальнику_Objest.СпосібДоставки.ToString();
            ЧасДоставкиЗ.Value = ПоверненняТоварівПостачальнику_Objest.ЧасДоставкиЗ;
            ЧасДоставкиДо.Value = ПоверненняТоварівПостачальнику_Objest.ЧасДоставкиДо;
            Менеджер.Pointer = ПоверненняТоварівПостачальнику_Objest.Менеджер;
            Основа.Pointer = ПоверненняТоварівПостачальнику_Objest.Основа;

            //Таблична частина
            Товари.ПоверненняТоварівПостачальнику_Objest = ПоверненняТоварівПостачальнику_Objest;
            await Товари.LoadRecords();

            if (IsNew)
            {
                //Основний договір
                Контрагент.AfterSelectFunc?.Invoke();
            }
        }

        protected override void GetValue()
        {
            ПоверненняТоварівПостачальнику_Objest.НомерДок = НомерДок.Text;
            ПоверненняТоварівПостачальнику_Objest.ДатаДок = ДатаДок.Value;
            ПоверненняТоварівПостачальнику_Objest.Організація = Організація.Pointer;
            ПоверненняТоварівПостачальнику_Objest.Валюта = Валюта.Pointer;
            ПоверненняТоварівПостачальнику_Objest.Каса = Каса.Pointer;
            ПоверненняТоварівПостачальнику_Objest.Склад = Склад.Pointer;
            ПоверненняТоварівПостачальнику_Objest.Контрагент = Контрагент.Pointer;
            ПоверненняТоварівПостачальнику_Objest.Договір = Договір.Pointer;
            ПоверненняТоварівПостачальнику_Objest.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            ПоверненняТоварівПостачальнику_Objest.Коментар = Коментар.Text;
            ПоверненняТоварівПостачальнику_Objest.Підрозділ = Підрозділ.Pointer;
            ПоверненняТоварівПостачальнику_Objest.БанківськийРахунокОрганізації = БанківськийРахунокОрганізації.Pointer;
            ПоверненняТоварівПостачальнику_Objest.БанківськийРахунокКонтрагента = БанківськийРахунокКонтрагента.Pointer;
            ПоверненняТоварівПостачальнику_Objest.Автор = Автор.Pointer;
            ПоверненняТоварівПостачальнику_Objest.СпосібДоставки = Enum.Parse<Перелічення.СпособиДоставки>(СпосібДоставки.ActiveId);
            ПоверненняТоварівПостачальнику_Objest.ЧасДоставкиЗ = ЧасДоставкиЗ.Value;
            ПоверненняТоварівПостачальнику_Objest.ЧасДоставкиДо = ЧасДоставкиДо.Value;
            ПоверненняТоварівПостачальнику_Objest.Менеджер = Менеджер.Pointer;
            ПоверненняТоварівПостачальнику_Objest.Основа = Основа.Pointer;

            ПоверненняТоварівПостачальнику_Objest.СумаДокументу = Товари.СумаДокументу();
            ПоверненняТоварівПостачальнику_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
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
                if (await ПоверненняТоварівПостачальнику_Objest.Save())
                {
                    await Товари.SaveRecords();
                    isSave = true;
                }
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }

            UnigueID = ПоверненняТоварівПостачальнику_Objest.UnigueID;
            Caption = ПоверненняТоварівПостачальнику_Objest.Назва;

            return isSave;
        }

        protected override async ValueTask<bool> SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = await ПоверненняТоварівПостачальнику_Objest.SpendTheDocument(ПоверненняТоварівПостачальнику_Objest.ДатаДок);

                if (!isSpend)
                    new ФункціїДляПовідомлень().ПоказатиПовідомлення(ПоверненняТоварівПостачальнику_Objest.UnigueID);

                return isSpend;
            }
            else
            {
                await ПоверненняТоварівПостачальнику_Objest.ClearSpendTheDocument();
                return true;
            }
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new ПоверненняТоварівПостачальнику_Pointer(unigueID);
        }
    }
}