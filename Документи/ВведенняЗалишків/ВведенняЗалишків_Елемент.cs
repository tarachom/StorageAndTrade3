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
    class ВведенняЗалишків_Елемент : ДокументЕлемент
    {
        public ВведенняЗалишків_Objest ВведенняЗалишків_Objest { get; set; } = new ВведенняЗалишків_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();
        Склади_PointerControl Склад = new Склади_PointerControl();
        Контрагенти_PointerControl Контрагент = new Контрагенти_PointerControl();
        ДоговориКонтрагентів_PointerControl Договір = new ДоговориКонтрагентів_PointerControl();
        ComboBoxText ГосподарськаОперація = new ComboBoxText();
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ" };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        Entry Коментар = new Entry() { WidthRequest = 920 };

        ВведенняЗалишків_ТабличнаЧастина_Товари Товари = new ВведенняЗалишків_ТабличнаЧастина_Товари();
        ВведенняЗалишків_ТабличнаЧастина_Каси Каси = new ВведенняЗалишків_ТабличнаЧастина_Каси();
        ВведенняЗалишків_ТабличнаЧастина_БанківськіРахунки БанківськіРахунки = new ВведенняЗалишків_ТабличнаЧастина_БанківськіРахунки();
        ВведенняЗалишків_ТабличнаЧастина_РозрахункиЗКонтрагентами РозрахункиЗКонтрагентами = new ВведенняЗалишків_ТабличнаЧастина_РозрахункиЗКонтрагентами();

        #endregion

        public ВведенняЗалишків_Елемент() : base()
        {
            CreateDocName(ВведенняЗалишків_Const.FULLNAME, НомерДок, ДатаДок);

            CreateField(HBoxComment, "Коментар:", Коментар);

            NotebookTablePart.InsertPage(Товари, new Label("Товари"), 0);
            NotebookTablePart.InsertPage(Каси, new Label("Каси"), 1);
            NotebookTablePart.InsertPage(БанківськіРахунки, new Label("Банківські рахунки"), 2);
            NotebookTablePart.InsertPage(РозрахункиЗКонтрагентами, new Label("Розрахунки з контрагентами"), 3);

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
                    Перелічення.ГосподарськіОперації.ВведенняЗалишків.ToString(),
                    Конфігурація_ГосподарськіОперації.Fields["ВведенняЗалишків"].Desc);

                ГосподарськаОперація.Active = 0;
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

        protected override void CreateContainer2(VBox vBox)
        {
            //Склад
            CreateField(vBox, null, Склад);

            //Валюта
            CreateField(vBox, null, Валюта);
        }

        protected override void CreateContainer3(VBox vBox)
        {
            //ГосподарськаОперація
            CreateField(vBox, "Господарська операція:", ГосподарськаОперація);

            //Підрозділ
            CreateField(vBox, null, Підрозділ);

            //Автор
            CreateField(vBox, null, Автор);
        }

        protected override void CreateContainer4(VBox vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                ВведенняЗалишків_Objest.New();
                ВведенняЗалишків_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                ВведенняЗалишків_Objest.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
                ВведенняЗалишків_Objest.Склад = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                ВведенняЗалишків_Objest.Контрагент = ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
                ВведенняЗалишків_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
            }

            НомерДок.Text = ВведенняЗалишків_Objest.НомерДок;
            ДатаДок.Value = ВведенняЗалишків_Objest.ДатаДок;
            Організація.Pointer = ВведенняЗалишків_Objest.Організація;
            Валюта.Pointer = ВведенняЗалишків_Objest.Валюта;
            Склад.Pointer = ВведенняЗалишків_Objest.Склад;
            Контрагент.Pointer = ВведенняЗалишків_Objest.Контрагент;
            Договір.Pointer = ВведенняЗалишків_Objest.Договір;
            ГосподарськаОперація.ActiveId = ВведенняЗалишків_Objest.ГосподарськаОперація.ToString();
            Коментар.Text = ВведенняЗалишків_Objest.Коментар;
            Підрозділ.Pointer = ВведенняЗалишків_Objest.Підрозділ;
            Автор.Pointer = ВведенняЗалишків_Objest.Автор;

            if (IsNew)
            {
                //Основний договір
                if (Контрагент.AfterSelectFunc != null)
                    Контрагент.AfterSelectFunc.Invoke();
            }

            Товари.ВведенняЗалишків_Objest = ВведенняЗалишків_Objest;
            await Товари.LoadRecords();

            Каси.ВведенняЗалишків_Objest = ВведенняЗалишків_Objest;
            await Каси.LoadRecords();

            БанківськіРахунки.ВведенняЗалишків_Objest = ВведенняЗалишків_Objest;
            await БанківськіРахунки.LoadRecords();

            РозрахункиЗКонтрагентами.ВведенняЗалишків_Objest = ВведенняЗалишків_Objest;
            await РозрахункиЗКонтрагентами.LoadRecords();
        }

        protected override void GetValue()
        {
            ВведенняЗалишків_Objest.НомерДок = НомерДок.Text;
            ВведенняЗалишків_Objest.ДатаДок = ДатаДок.Value;
            ВведенняЗалишків_Objest.Організація = Організація.Pointer;
            ВведенняЗалишків_Objest.Валюта = Валюта.Pointer;
            ВведенняЗалишків_Objest.Склад = Склад.Pointer;
            ВведенняЗалишків_Objest.Контрагент = Контрагент.Pointer;
            ВведенняЗалишків_Objest.Договір = Договір.Pointer;
            ВведенняЗалишків_Objest.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            ВведенняЗалишків_Objest.Коментар = Коментар.Text;
            ВведенняЗалишків_Objest.Підрозділ = Підрозділ.Pointer;
            ВведенняЗалишків_Objest.Автор = Автор.Pointer;

            ВведенняЗалишків_Objest.КлючовіСловаДляПошуку =
                КлючовіСловаДляПошуку() +
                Товари.КлючовіСловаДляПошуку() +
                БанківськіРахунки.КлючовіСловаДляПошуку() +
                Каси.КлючовіСловаДляПошуку() +
                РозрахункиЗКонтрагентами.КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Валюта.Pointer.Назва} {Склад.Pointer.Назва} {Контрагент.Pointer.Назва} ";
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            bool isSave;

            try
            {
                isSave = await ВведенняЗалишків_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
                return false;
            }

            if (isSave)
            {
                await Товари.SaveRecords();
                await Каси.SaveRecords();
                await БанківськіРахунки.SaveRecords();
                await РозрахункиЗКонтрагентами.SaveRecords();
            }

            UnigueID = ВведенняЗалишків_Objest.UnigueID;
            Caption = ВведенняЗалишків_Objest.Назва;

            return isSave;
        }

        protected override async ValueTask<bool> SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = await ВведенняЗалишків_Objest.SpendTheDocument(ВведенняЗалишків_Objest.ДатаДок);

                if (!isSpend)
                    ФункціїДляПовідомлень.ВідкритиТермінал();

                return isSpend;
            }
            else
            {
                await ВведенняЗалишків_Objest.ClearSpendTheDocument();

                return true;
            }
        }
    }
}