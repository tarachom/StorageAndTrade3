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

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ПереміщенняТоварівНаСкладі_Елемент : ДокументЕлемент
    {
        public ПереміщенняТоварівНаСкладі_Objest ПереміщенняТоварівНаСкладі_Objest { get; set; } = new ПереміщенняТоварівНаСкладі_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Склади_PointerControl Склад = new Склади_PointerControl();
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ" };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        Entry Коментар = new Entry() { WidthRequest = 920 };
        ПереміщенняТоварівНаСкладі_ТабличнаЧастина_Товари Товари = new ПереміщенняТоварівНаСкладі_ТабличнаЧастина_Товари();
        CompositePointerControl Основа = new CompositePointerControl();

        #endregion

        public ПереміщенняТоварівНаСкладі_Елемент() : base()
        {
            CreateDocName(ПереміщенняТоварівНаСкладі_Const.FULLNAME, НомерДок, ДатаДок);

            CreateField(HBoxComment, "Коментар:", Коментар);

            NotebookTablePart.InsertPage(Товари, new Label("Товари"), 0);
            NotebookTablePart.CurrentPage = 0;
        }

        protected override void CreateContainer1(Box vBox)
        {
            //Організація
            CreateField(vBox, null, Організація);
        }

        protected override void CreateContainer2(Box vBox)
        {
            //Склад
            CreateField(vBox, null, Склад);
        }

        protected override void CreateContainer3(Box vBox)
        {
            //Підрозділ
            CreateField(vBox, null, Підрозділ);

            //Автор
            CreateField(vBox, null, Автор);

            //Основа
            CreateField(vBox, null, Основа);
        }

        protected override void CreateContainer4(Box vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                await ПереміщенняТоварівНаСкладі_Objest.New();
                ПереміщенняТоварівНаСкладі_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                ПереміщенняТоварівНаСкладі_Objest.Склад = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                ПереміщенняТоварівНаСкладі_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
            }

            НомерДок.Text = ПереміщенняТоварівНаСкладі_Objest.НомерДок;
            ДатаДок.Value = ПереміщенняТоварівНаСкладі_Objest.ДатаДок;
            Організація.Pointer = ПереміщенняТоварівНаСкладі_Objest.Організація;
            Склад.Pointer = ПереміщенняТоварівНаСкладі_Objest.Склад;
            Коментар.Text = ПереміщенняТоварівНаСкладі_Objest.Коментар;
            Підрозділ.Pointer = ПереміщенняТоварівНаСкладі_Objest.Підрозділ;
            Автор.Pointer = ПереміщенняТоварівНаСкладі_Objest.Автор;
            Основа.Pointer = ПереміщенняТоварівНаСкладі_Objest.Основа;

            //Таблична частина
            Товари.ПереміщенняТоварівНаСкладі_Objest = ПереміщенняТоварівНаСкладі_Objest;
            Товари.ОбновитиЗначенняДокумента = () =>
            {
                ПереміщенняТоварівНаСкладі_Objest.Склад = Склад.Pointer;
            };

            await Товари.LoadRecords();
        }

        protected override void GetValue()
        {
            ПереміщенняТоварівНаСкладі_Objest.НомерДок = НомерДок.Text;
            ПереміщенняТоварівНаСкладі_Objest.ДатаДок = ДатаДок.Value;
            ПереміщенняТоварівНаСкладі_Objest.Організація = Організація.Pointer;
            ПереміщенняТоварівНаСкладі_Objest.Склад = Склад.Pointer;
            ПереміщенняТоварівНаСкладі_Objest.Коментар = Коментар.Text;
            ПереміщенняТоварівНаСкладі_Objest.Підрозділ = Підрозділ.Pointer;
            ПереміщенняТоварівНаСкладі_Objest.Автор = Автор.Pointer;
            ПереміщенняТоварівНаСкладі_Objest.Основа = Основа.Pointer;

            ПереміщенняТоварівНаСкладі_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
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
                if (await ПереміщенняТоварівНаСкладі_Objest.Save())
                {
                    await Товари.SaveRecords();
                    isSave = true;
                }
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }

            UnigueID = ПереміщенняТоварівНаСкладі_Objest.UnigueID;
            Caption = ПереміщенняТоварівНаСкладі_Objest.Назва;

            return isSave;
        }

        protected override async ValueTask<bool> SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = await ПереміщенняТоварівНаСкладі_Objest.SpendTheDocument(ПереміщенняТоварівНаСкладі_Objest.ДатаДок);

                if (!isSpend)
                    new ФункціїДляПовідомлень().ПоказатиПовідомлення(ПереміщенняТоварівНаСкладі_Objest.UnigueID);

                return isSpend;
            }
            else
            {
                await ПереміщенняТоварівНаСкладі_Objest.ClearSpendTheDocument();
                return true;
            }
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new ПереміщенняТоварівНаСкладі_Pointer(unigueID);
        }
    }
}