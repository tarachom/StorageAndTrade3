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
using StorageAndTrade_1_0.Документи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class ВнутрішнєСпоживанняТоварів_Елемент : ДокументЕлемент
    {
        public ВнутрішнєСпоживанняТоварів_Objest ВнутрішнєСпоживанняТоварів_Objest { get; set; } = new ВнутрішнєСпоживанняТоварів_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();
        Склади_PointerControl Склад = new Склади_PointerControl();
        ComboBoxText ГосподарськаОперація = new ComboBoxText();
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ" };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        Entry Коментар = new Entry() { WidthRequest = 920 };
        CompositePointerControl Основа = new CompositePointerControl();

        ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари Товари = new ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари();

        #endregion

        public ВнутрішнєСпоживанняТоварів_Елемент() : base()
        {
            CreateDocName(ВнутрішнєСпоживанняТоварів_Const.FULLNAME, НомерДок, ДатаДок);

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
                    Перелічення.ГосподарськіОперації.ВнутрішнєСпоживанняТоварів.ToString(),
                    Конфігурація_ГосподарськіОперації.Fields["ВнутрішнєСпоживанняТоварів"].Desc);

                ГосподарськаОперація.Active = 0;
            }
        }

        protected override void CreateContainer1(VBox vBox)
        {
            //Організація
            CreateField(vBox, null, Організація);

            //Склад
            CreateField(vBox, null, Склад);
        }

        protected override void CreateContainer2(VBox vBox)
        {
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
            //Основа
            CreateField(vBox, null, Основа);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                await ВнутрішнєСпоживанняТоварів_Objest.New();
                ВнутрішнєСпоживанняТоварів_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                ВнутрішнєСпоживанняТоварів_Objest.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
                ВнутрішнєСпоживанняТоварів_Objest.Склад = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                ВнутрішнєСпоживанняТоварів_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
            }

            НомерДок.Text = ВнутрішнєСпоживанняТоварів_Objest.НомерДок;
            ДатаДок.Value = ВнутрішнєСпоживанняТоварів_Objest.ДатаДок;
            Організація.Pointer = ВнутрішнєСпоживанняТоварів_Objest.Організація;
            Валюта.Pointer = ВнутрішнєСпоживанняТоварів_Objest.Валюта;
            Склад.Pointer = ВнутрішнєСпоживанняТоварів_Objest.Склад;
            ГосподарськаОперація.ActiveId = ВнутрішнєСпоживанняТоварів_Objest.ГосподарськаОперація.ToString();
            Коментар.Text = ВнутрішнєСпоживанняТоварів_Objest.Коментар;
            Підрозділ.Pointer = ВнутрішнєСпоживанняТоварів_Objest.Підрозділ;
            Автор.Pointer = ВнутрішнєСпоживанняТоварів_Objest.Автор;
            Основа.Pointer = ВнутрішнєСпоживанняТоварів_Objest.Основа;

            //Таблична частина
            Товари.ВнутрішнєСпоживанняТоварів_Objest = ВнутрішнєСпоживанняТоварів_Objest;
            await Товари.LoadRecords();
        }

        protected override void GetValue()
        {
            ВнутрішнєСпоживанняТоварів_Objest.НомерДок = НомерДок.Text;
            ВнутрішнєСпоживанняТоварів_Objest.ДатаДок = ДатаДок.Value;
            ВнутрішнєСпоживанняТоварів_Objest.Організація = Організація.Pointer;
            ВнутрішнєСпоживанняТоварів_Objest.Валюта = Валюта.Pointer;
            ВнутрішнєСпоживанняТоварів_Objest.Склад = Склад.Pointer;
            ВнутрішнєСпоживанняТоварів_Objest.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            ВнутрішнєСпоживанняТоварів_Objest.Коментар = Коментар.Text;
            ВнутрішнєСпоживанняТоварів_Objest.Підрозділ = Підрозділ.Pointer;
            ВнутрішнєСпоживанняТоварів_Objest.Автор = Автор.Pointer;
            ВнутрішнєСпоживанняТоварів_Objest.Основа = Основа.Pointer;

            ВнутрішнєСпоживанняТоварів_Objest.СумаДокументу = Товари.СумаДокументу();
            ВнутрішнєСпоживанняТоварів_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Валюта.Pointer.Назва} {Склад.Pointer.Назва} ";
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            bool isSave;

            try
            {
                isSave = await ВнутрішнєСпоживанняТоварів_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
                return false;
            }

            if (isSave)
                await Товари.SaveRecords();

            UnigueID = ВнутрішнєСпоживанняТоварів_Objest.UnigueID;
            Caption = ВнутрішнєСпоживанняТоварів_Objest.Назва;

            return isSave;
        }

        protected override async ValueTask<bool> SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = await ВнутрішнєСпоживанняТоварів_Objest.SpendTheDocument(ВнутрішнєСпоживанняТоварів_Objest.ДатаДок);

                if (!isSpend)
                    ФункціїДляПовідомлень.ВідкритиТермінал();

                return isSpend;
            }
            else
            {
                await ВнутрішнєСпоживанняТоварів_Objest.ClearSpendTheDocument();
                return true;
            }
        }
    }
}