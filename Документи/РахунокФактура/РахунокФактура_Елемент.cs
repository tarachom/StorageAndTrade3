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
    class РахунокФактура_Елемент : ДокументЕлемент
    {
        public РахунокФактура_Objest РахунокФактура_Objest { get; set; } = new РахунокФактура_Objest();

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
        БанківськіРахункиОрганізацій_PointerControl БанківськийРахунок = new БанківськіРахункиОрганізацій_PointerControl() { WidthPresentation = 200 };
        БанківськіРахункиКонтрагентів_PointerControl БанківськийРахунокКонтрагента = new БанківськіРахункиКонтрагентів_PointerControl() { Caption = "Рахунок контрагента:", WidthPresentation = 200 };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        Користувачі_PointerControl Менеджер = new Користувачі_PointerControl() { Caption = "Менеджер:" };
        Entry Коментар = new Entry() { WidthRequest = 920 };

        РахунокФактура_ТабличнаЧастина_Товари Товари = new РахунокФактура_ТабличнаЧастина_Товари();

        #endregion

        public РахунокФактура_Елемент() : base()
        {
            CreateDocName(РахунокФактура_Const.FULLNAME, НомерДок, ДатаДок);

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
            }
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

            //Автор
            CreateField(vBox, null, Автор);

            //Менеджер
            CreateField(vBox, null, Менеджер);
        }

        protected override void CreateContainer4(VBox vBox)
        {
            //ФормаОплати
            CreateField(vBox, "Форма оплати:", ФормаОплати);

            //БанківськийрахунокКонтрагента
            CreateField(vBox, null, БанківськийРахунокКонтрагента);

            //БанківськийрахунокОрганізації
            CreateField(vBox, null, БанківськийРахунок);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
            {
                РахунокФактура_Objest.New();
                РахунокФактура_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                РахунокФактура_Objest.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
                РахунокФактура_Objest.Каса = ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const;
                РахунокФактура_Objest.Склад = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                РахунокФактура_Objest.Контрагент = ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
                РахунокФактура_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
                РахунокФактура_Objest.БанківськийРахунок = ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const;
            }

            НомерДок.Text = РахунокФактура_Objest.НомерДок;
            ДатаДок.Value = РахунокФактура_Objest.ДатаДок;
            Організація.Pointer = РахунокФактура_Objest.Організація;
            Валюта.Pointer = РахунокФактура_Objest.Валюта;
            Каса.Pointer = РахунокФактура_Objest.Каса;
            Склад.Pointer = РахунокФактура_Objest.Склад;
            Контрагент.Pointer = РахунокФактура_Objest.Контрагент;
            Договір.Pointer = РахунокФактура_Objest.Договір;
            ГосподарськаОперація.ActiveId = РахунокФактура_Objest.ГосподарськаОперація.ToString();
            ФормаОплати.ActiveId = РахунокФактура_Objest.ФормаОплати.ToString();
            Коментар.Text = РахунокФактура_Objest.Коментар;
            Підрозділ.Pointer = РахунокФактура_Objest.Підрозділ;
            БанківськийРахунок.Pointer = РахунокФактура_Objest.БанківськийРахунок;
            БанківськийРахунокКонтрагента.Pointer = РахунокФактура_Objest.БанківськийРахунокКонтрагента;
            Автор.Pointer = РахунокФактура_Objest.Автор;
            Менеджер.Pointer = РахунокФактура_Objest.Менеджер;

            //Таблична частина
            Товари.РахунокФактура_Objest = РахунокФактура_Objest;
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
            РахунокФактура_Objest.НомерДок = НомерДок.Text;
            РахунокФактура_Objest.ДатаДок = ДатаДок.Value;
            РахунокФактура_Objest.Організація = Організація.Pointer;
            РахунокФактура_Objest.Валюта = Валюта.Pointer;
            РахунокФактура_Objest.Каса = Каса.Pointer;
            РахунокФактура_Objest.Склад = Склад.Pointer;
            РахунокФактура_Objest.Контрагент = Контрагент.Pointer;
            РахунокФактура_Objest.Договір = Договір.Pointer;
            РахунокФактура_Objest.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            РахунокФактура_Objest.ФормаОплати = Enum.Parse<Перелічення.ФормаОплати>(ФормаОплати.ActiveId);
            РахунокФактура_Objest.Коментар = Коментар.Text;
            РахунокФактура_Objest.Підрозділ = Підрозділ.Pointer;
            РахунокФактура_Objest.БанківськийРахунок = БанківськийРахунок.Pointer;
            РахунокФактура_Objest.БанківськийРахунокКонтрагента = БанківськийРахунокКонтрагента.Pointer;
            РахунокФактура_Objest.Автор = Автор.Pointer;
            РахунокФактура_Objest.Менеджер = Менеджер.Pointer;

            РахунокФактура_Objest.СумаДокументу = Товари.СумаДокументу();
            РахунокФактура_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
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
                isSave = await РахунокФактура_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
                return false;
            }

            if (isSave)
                await Товари.SaveRecords();

            UnigueID = РахунокФактура_Objest.UnigueID;
            Caption = РахунокФактура_Objest.Назва;

            return isSave;
        }

        protected override async ValueTask<bool> SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = await РахунокФактура_Objest.SpendTheDocument(РахунокФактура_Objest.ДатаДок);

                if (!isSpend)
                    ФункціїДляПовідомлень.ВідкритиТермінал();

                return isSpend;
            }
            else
            {
                await РахунокФактура_Objest.ClearSpendTheDocument();

                return true;
            }
        }
    }
}