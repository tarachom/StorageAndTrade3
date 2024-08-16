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
    class ПоверненняТоварівВідКлієнта_Елемент : ДокументЕлемент
    {
        public ПоверненняТоварівВідКлієнта_Objest ПоверненняТоварівВідКлієнта_Objest { get; set; } = new ПоверненняТоварівВідКлієнта_Objest();

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
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        Користувачі_PointerControl Менеджер = new Користувачі_PointerControl() { Caption = "Менеджер" };
        Entry Коментар = new Entry() { WidthRequest = 920 };
        CompositePointerControl Основа = new CompositePointerControl();

        ПоверненняТоварівВідКлієнта_ТабличнаЧастина_Товари Товари = new ПоверненняТоварівВідКлієнта_ТабличнаЧастина_Товари();

        #endregion        

        public ПоверненняТоварівВідКлієнта_Елемент() : base()
        {
            CreateDocName(ПоверненняТоварівВідКлієнта_Const.FULLNAME, НомерДок, ДатаДок);

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
                Перелічення.ГосподарськіОперації.ПоверненняТоварівВідКлієнта.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["ПоверненняТоварівВідКлієнта"].Desc);

            ГосподарськаОперація.Active = 0;
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
            //Каса
            CreateField(vBox, null, Каса);

            //Валюта
            CreateField(vBox, null, Валюта);

            //Склад
            CreateField(vBox, null, Склад);
        }

        protected override void CreateContainer3(Box vBox)
        {
            //Підрозділ
            CreateField(vBox, null, Підрозділ);

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
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                await ПоверненняТоварівВідКлієнта_Objest.New();
                ПоверненняТоварівВідКлієнта_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                ПоверненняТоварівВідКлієнта_Objest.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
                ПоверненняТоварівВідКлієнта_Objest.Каса = ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const;
                ПоверненняТоварівВідКлієнта_Objest.Склад = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                ПоверненняТоварівВідКлієнта_Objest.Контрагент = ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
                ПоверненняТоварівВідКлієнта_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
            }

            НомерДок.Text = ПоверненняТоварівВідКлієнта_Objest.НомерДок;
            ДатаДок.Value = ПоверненняТоварівВідКлієнта_Objest.ДатаДок;
            Організація.Pointer = ПоверненняТоварівВідКлієнта_Objest.Організація;
            Валюта.Pointer = ПоверненняТоварівВідКлієнта_Objest.Валюта;
            Каса.Pointer = ПоверненняТоварівВідКлієнта_Objest.Каса;
            Склад.Pointer = ПоверненняТоварівВідКлієнта_Objest.Склад;
            Контрагент.Pointer = ПоверненняТоварівВідКлієнта_Objest.Контрагент;
            Договір.Pointer = ПоверненняТоварівВідКлієнта_Objest.Договір;
            ГосподарськаОперація.ActiveId = ПоверненняТоварівВідКлієнта_Objest.ГосподарськаОперація.ToString();
            Коментар.Text = ПоверненняТоварівВідКлієнта_Objest.Коментар;
            Підрозділ.Pointer = ПоверненняТоварівВідКлієнта_Objest.Підрозділ;
            Автор.Pointer = ПоверненняТоварівВідКлієнта_Objest.Автор;
            Менеджер.Pointer = ПоверненняТоварівВідКлієнта_Objest.Менеджер;
            Основа.Pointer = ПоверненняТоварівВідКлієнта_Objest.Основа;

            //Таблична частина
            Товари.ПоверненняТоварівВідКлієнта_Objest = ПоверненняТоварівВідКлієнта_Objest;
            await Товари.LoadRecords();

            if (IsNew)
            {
                //Основний договір
                Контрагент.AfterSelectFunc?.Invoke();
            }
        }

        protected override void GetValue()
        {
            ПоверненняТоварівВідКлієнта_Objest.НомерДок = НомерДок.Text;
            ПоверненняТоварівВідКлієнта_Objest.ДатаДок = ДатаДок.Value;
            ПоверненняТоварівВідКлієнта_Objest.Організація = Організація.Pointer;
            ПоверненняТоварівВідКлієнта_Objest.Валюта = Валюта.Pointer;
            ПоверненняТоварівВідКлієнта_Objest.Каса = Каса.Pointer;
            ПоверненняТоварівВідКлієнта_Objest.Склад = Склад.Pointer;
            ПоверненняТоварівВідКлієнта_Objest.Контрагент = Контрагент.Pointer;
            ПоверненняТоварівВідКлієнта_Objest.Договір = Договір.Pointer;
            ПоверненняТоварівВідКлієнта_Objest.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            ПоверненняТоварівВідКлієнта_Objest.Коментар = Коментар.Text;
            ПоверненняТоварівВідКлієнта_Objest.Підрозділ = Підрозділ.Pointer;
            ПоверненняТоварівВідКлієнта_Objest.Автор = Автор.Pointer;
            ПоверненняТоварівВідКлієнта_Objest.Менеджер = Менеджер.Pointer;
            ПоверненняТоварівВідКлієнта_Objest.Основа = Основа.Pointer;

            ПоверненняТоварівВідКлієнта_Objest.СумаДокументу = Товари.СумаДокументу();
            ПоверненняТоварівВідКлієнта_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
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
                if (await ПоверненняТоварівВідКлієнта_Objest.Save())
                {
                    await Товари.SaveRecords();
                    isSave = true;
                }
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }

            UnigueID = ПоверненняТоварівВідКлієнта_Objest.UnigueID;
            Caption = ПоверненняТоварівВідКлієнта_Objest.Назва;

            return isSave;
        }

        protected override async ValueTask<bool> SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = await ПоверненняТоварівВідКлієнта_Objest.SpendTheDocument(ПоверненняТоварівВідКлієнта_Objest.ДатаДок);

                if (!isSpend)
                    new ФункціїДляПовідомлень().ПоказатиПовідомлення(ПоверненняТоварівВідКлієнта_Objest.UnigueID);

                return isSpend;
            }
            else
            {
                await ПоверненняТоварівВідКлієнта_Objest.ClearSpendTheDocument();
                return true;
            }
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new ПоверненняТоварівВідКлієнта_Pointer(unigueID);
        }
    }
}