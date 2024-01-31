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

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class РозміщенняТоварівНаСкладі_Елемент : ДокументЕлемент
    {
        public РозміщенняТоварівНаСкладі_Objest РозміщенняТоварівНаСкладі_Objest { get; set; } = new РозміщенняТоварівНаСкладі_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Склади_PointerControl Склад = new Склади_PointerControl();
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ" };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        Entry Коментар = new Entry() { WidthRequest = 920 };
        ПоступленняТоварівТаПослуг_PointerControl ДокументПоступлення = new ПоступленняТоварівТаПослуг_PointerControl() { Caption = "Документ поступлення: " };
        CompositePointerControl Основа = new CompositePointerControl();

        РозміщенняТоварівНаСкладі_ТабличнаЧастина_Товари Товари = new РозміщенняТоварівНаСкладі_ТабличнаЧастина_Товари();

        #endregion

        public РозміщенняТоварівНаСкладі_Елемент() : base()
        {
            CreateDocName(РозміщенняТоварівНаСкладі_Const.FULLNAME, НомерДок, ДатаДок);

            CreateField(HBoxComment, "Коментар:", Коментар);

            NotebookTablePart.InsertPage(Товари, new Label("Товари"), 0);
            NotebookTablePart.CurrentPage = 0;
        }

        protected override void CreateContainer1(VBox vBox)
        {
            //Організація
            CreateField(vBox, null, Організація);

            //ДокументПоступлення
            CreateField(vBox, null, ДокументПоступлення);
        }

        protected override void CreateContainer2(VBox vBox)
        {
            //Склад
            CreateField(vBox, null, Склад);
        }

        protected override void CreateContainer3(VBox vBox)
        {
            //Підрозділ
            CreateField(vBox, null, Підрозділ);

            //Автор
            CreateField(vBox, null, Автор);

            //Основа
            CreateField(vBox, null, Основа);
        }

        protected override void CreateContainer4(VBox vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                await РозміщенняТоварівНаСкладі_Objest.New();
                РозміщенняТоварівНаСкладі_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                РозміщенняТоварівНаСкладі_Objest.Склад = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                РозміщенняТоварівНаСкладі_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
            }

            НомерДок.Text = РозміщенняТоварівНаСкладі_Objest.НомерДок;
            ДатаДок.Value = РозміщенняТоварівНаСкладі_Objest.ДатаДок;
            Організація.Pointer = РозміщенняТоварівНаСкладі_Objest.Організація;
            Склад.Pointer = РозміщенняТоварівНаСкладі_Objest.Склад;
            Коментар.Text = РозміщенняТоварівНаСкладі_Objest.Коментар;
            Підрозділ.Pointer = РозміщенняТоварівНаСкладі_Objest.Підрозділ;
            Автор.Pointer = РозміщенняТоварівНаСкладі_Objest.Автор;
            ДокументПоступлення.Pointer = РозміщенняТоварівНаСкладі_Objest.ДокументПоступлення;
            Основа.Pointer = РозміщенняТоварівНаСкладі_Objest.Основа;

            //Таблична частина
            Товари.РозміщенняТоварівНаСкладі_Objest = РозміщенняТоварівНаСкладі_Objest;
            Товари.ОбновитиЗначенняДокумента = () =>
            {
                РозміщенняТоварівНаСкладі_Objest.Склад = Склад.Pointer;
            };

            await Товари.LoadRecords();
        }

        protected override void GetValue()
        {
            РозміщенняТоварівНаСкладі_Objest.НомерДок = НомерДок.Text;
            РозміщенняТоварівНаСкладі_Objest.ДатаДок = ДатаДок.Value;
            РозміщенняТоварівНаСкладі_Objest.Організація = Організація.Pointer;
            РозміщенняТоварівНаСкладі_Objest.Склад = Склад.Pointer;
            РозміщенняТоварівНаСкладі_Objest.Коментар = Коментар.Text;
            РозміщенняТоварівНаСкладі_Objest.Підрозділ = Підрозділ.Pointer;
            РозміщенняТоварівНаСкладі_Objest.Автор = Автор.Pointer;
            РозміщенняТоварівНаСкладі_Objest.ДокументПоступлення = ДокументПоступлення.Pointer;
            РозміщенняТоварівНаСкладі_Objest.Основа = Основа.Pointer;

            РозміщенняТоварівНаСкладі_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Склад.Pointer.Назва}";
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            bool isSave = false;

            try
            {
                if (await РозміщенняТоварівНаСкладі_Objest.Save())
                {
                    await Товари.SaveRecords();
                    isSave = true;
                }
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }

            UnigueID = РозміщенняТоварівНаСкладі_Objest.UnigueID;
            Caption = РозміщенняТоварівНаСкладі_Objest.Назва;

            return isSave;
        }

        protected override async ValueTask<bool> SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = await РозміщенняТоварівНаСкладі_Objest.SpendTheDocument(РозміщенняТоварівНаСкладі_Objest.ДатаДок);

                if (!isSpend)
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(РозміщенняТоварівНаСкладі_Objest.UnigueID);

                return isSpend;
            }
            else
            {
                await РозміщенняТоварівНаСкладі_Objest.ClearSpendTheDocument();
                return true;
            }
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new РозміщенняТоварівНаСкладі_Pointer(unigueID);
        }
    }
}