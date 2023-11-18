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
    class АктВиконанихРобіт_Елемент : ДокументЕлемент
    {
        public АктВиконанихРобіт_Objest АктВиконанихРобіт_Objest { get; set; } = new АктВиконанихРобіт_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();
        Каси_PointerControl Каса = new Каси_PointerControl();
        Контрагенти_PointerControl Контрагент = new Контрагенти_PointerControl();
        ДоговориКонтрагентів_PointerControl Договір = new ДоговориКонтрагентів_PointerControl();
        ComboBoxText ГосподарськаОперація = new ComboBoxText();
        ComboBoxText ФормаОплати = new ComboBoxText();
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ" };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        Користувачі_PointerControl Менеджер = new Користувачі_PointerControl() { Caption = "Менеджер" };
        Entry Коментар = new Entry() { WidthRequest = 920 };

        АктВиконанихРобіт_ТабличнаЧастина_Послуги Послуги = new АктВиконанихРобіт_ТабличнаЧастина_Послуги();

        #endregion

        public АктВиконанихРобіт_Елемент() : base()
        {
            CreateDocName(АктВиконанихРобіт_Const.FULLNAME, НомерДок, ДатаДок);

            CreateField(HBoxComment, "Коментар:", Коментар);

            NotebookTablePart.InsertPage(Послуги, new Label("Послуги"), 0);
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
                    Перелічення.ГосподарськіОперації.РеалізаціяКлієнту.ToString(),
                    Конфігурація_ГосподарськіОперації.Fields["РеалізаціяКлієнту"].Desc);

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
            //Каса
            CreateField(vBox, null, Каса);

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

            //Менеджер
            CreateField(vBox, null, Менеджер);
        }

        protected override void CreateContainer4(VBox vBox)
        {
            //ФормаОплати
            CreateField(vBox, "Форма оплати:", ФормаОплати);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
            {
                АктВиконанихРобіт_Objest.New();
                АктВиконанихРобіт_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                АктВиконанихРобіт_Objest.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
                АктВиконанихРобіт_Objest.Каса = ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const;
                АктВиконанихРобіт_Objest.Контрагент = ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
                АктВиконанихРобіт_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
            }

            НомерДок.Text = АктВиконанихРобіт_Objest.НомерДок;
            ДатаДок.Value = АктВиконанихРобіт_Objest.ДатаДок;
            Організація.Pointer = АктВиконанихРобіт_Objest.Організація;
            Валюта.Pointer = АктВиконанихРобіт_Objest.Валюта;
            Каса.Pointer = АктВиконанихРобіт_Objest.Каса;
            Контрагент.Pointer = АктВиконанихРобіт_Objest.Контрагент;
            Договір.Pointer = АктВиконанихРобіт_Objest.Договір;
            ГосподарськаОперація.ActiveId = АктВиконанихРобіт_Objest.ГосподарськаОперація.ToString();
            ФормаОплати.ActiveId = АктВиконанихРобіт_Objest.ФормаОплати.ToString();
            Коментар.Text = АктВиконанихРобіт_Objest.Коментар;
            Підрозділ.Pointer = АктВиконанихРобіт_Objest.Підрозділ;
            Автор.Pointer = АктВиконанихРобіт_Objest.Автор;
            Менеджер.Pointer = АктВиконанихРобіт_Objest.Менеджер;

            //Таблична частина
            Послуги.АктВиконанихРобіт_Objest = АктВиконанихРобіт_Objest;
            Послуги.LoadRecords();

            if (IsNew)
            {
                //Основний договір
                if (Контрагент.AfterSelectFunc != null)
                    Контрагент.AfterSelectFunc.Invoke();
            }
        }

        protected override void GetValue()
        {
            АктВиконанихРобіт_Objest.НомерДок = НомерДок.Text;
            АктВиконанихРобіт_Objest.ДатаДок = ДатаДок.Value;
            АктВиконанихРобіт_Objest.Організація = Організація.Pointer;
            АктВиконанихРобіт_Objest.Валюта = Валюта.Pointer;
            АктВиконанихРобіт_Objest.Каса = Каса.Pointer;
            АктВиконанихРобіт_Objest.Контрагент = Контрагент.Pointer;
            АктВиконанихРобіт_Objest.Договір = Договір.Pointer;
            АктВиконанихРобіт_Objest.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            АктВиконанихРобіт_Objest.ФормаОплати = Enum.Parse<Перелічення.ФормаОплати>(ФормаОплати.ActiveId);
            АктВиконанихРобіт_Objest.Коментар = Коментар.Text;
            АктВиконанихРобіт_Objest.Підрозділ = Підрозділ.Pointer;
            АктВиконанихРобіт_Objest.Автор = Автор.Pointer;
            АктВиконанихРобіт_Objest.Менеджер = Менеджер.Pointer;

            АктВиконанихРобіт_Objest.СумаДокументу = Послуги.СумаДокументу();
            АктВиконанихРобіт_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Послуги.КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Валюта.Pointer.Назва} {Каса.Pointer.Назва} " +
                $"{Контрагент.Pointer.Назва} ";
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            bool isSave;

            try
            {
                isSave = await АктВиконанихРобіт_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
                return false;
            }

            if (isSave)
                await Послуги.SaveRecords();

            UnigueID = АктВиконанихРобіт_Objest.UnigueID;
            Caption = АктВиконанихРобіт_Objest.Назва;

            return isSave;
        }

        protected override async ValueTask<bool> SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = await АктВиконанихРобіт_Objest.SpendTheDocument(АктВиконанихРобіт_Objest.ДатаДок);

                if (!isSpend)
                    ФункціїДляПовідомлень.ВідкритиТермінал();

                return isSpend;
            }
            else
            {
                await АктВиконанихРобіт_Objest.ClearSpendTheDocument();
                return true;
            }
        }
    }
}