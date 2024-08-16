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
    class ЗбіркаТоварівНаСкладі_Елемент : ДокументЕлемент
    {
        public ЗбіркаТоварівНаСкладі_Objest ЗбіркаТоварівНаСкладі_Objest { get; set; } = new ЗбіркаТоварівНаСкладі_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Склади_PointerControl Склад = new Склади_PointerControl();
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ" };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        Entry Коментар = new Entry() { WidthRequest = 920 };
        РеалізаціяТоварівТаПослуг_PointerControl ДокументРеалізації = new РеалізаціяТоварівТаПослуг_PointerControl() { Caption = "Документ реалізації:" };
        CompositePointerControl Основа = new CompositePointerControl();

        ЗбіркаТоварівНаСкладі_ТабличнаЧастина_Товари Товари = new ЗбіркаТоварівНаСкладі_ТабличнаЧастина_Товари();

        #endregion

        public ЗбіркаТоварівНаСкладі_Елемент() : base()
        {
            CreateDocName(ЗбіркаТоварівНаСкладі_Const.FULLNAME, НомерДок, ДатаДок);

            CreateField(HBoxComment, "Коментар:", Коментар);

            NotebookTablePart.InsertPage(Товари, new Label("Товари"), 0);
            NotebookTablePart.CurrentPage = 0;
        }

        protected override void CreateContainer1(Box vBox)
        {
            //Організація
            CreateField(vBox, null, Організація);

            //ДокументРеалізації
            CreateField(vBox, null, ДокументРеалізації);
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
                await ЗбіркаТоварівНаСкладі_Objest.New();
                ЗбіркаТоварівНаСкладі_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                ЗбіркаТоварівНаСкладі_Objest.Склад = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                ЗбіркаТоварівНаСкладі_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
            }

            НомерДок.Text = ЗбіркаТоварівНаСкладі_Objest.НомерДок;
            ДатаДок.Value = ЗбіркаТоварівНаСкладі_Objest.ДатаДок;
            Організація.Pointer = ЗбіркаТоварівНаСкладі_Objest.Організація;
            Склад.Pointer = ЗбіркаТоварівНаСкладі_Objest.Склад;
            Коментар.Text = ЗбіркаТоварівНаСкладі_Objest.Коментар;
            Підрозділ.Pointer = ЗбіркаТоварівНаСкладі_Objest.Підрозділ;
            Автор.Pointer = ЗбіркаТоварівНаСкладі_Objest.Автор;
            ДокументРеалізації.Pointer = ЗбіркаТоварівНаСкладі_Objest.ДокументРеалізації;
            Основа.Pointer = ЗбіркаТоварівНаСкладі_Objest.Основа;

            //Таблична частина
            Товари.ЗбіркаТоварівНаСкладі_Objest = ЗбіркаТоварівНаСкладі_Objest;
            Товари.ОбновитиЗначенняДокумента = () =>
            {
                ЗбіркаТоварівНаСкладі_Objest.Склад = Склад.Pointer;
            };

            await Товари.LoadRecords();
        }

        protected override void GetValue()
        {
            ЗбіркаТоварівНаСкладі_Objest.НомерДок = НомерДок.Text;
            ЗбіркаТоварівНаСкладі_Objest.ДатаДок = ДатаДок.Value;
            ЗбіркаТоварівНаСкладі_Objest.Організація = Організація.Pointer;
            ЗбіркаТоварівНаСкладі_Objest.Склад = Склад.Pointer;
            ЗбіркаТоварівНаСкладі_Objest.Коментар = Коментар.Text;
            ЗбіркаТоварівНаСкладі_Objest.Підрозділ = Підрозділ.Pointer;
            ЗбіркаТоварівНаСкладі_Objest.Автор = Автор.Pointer;
            ЗбіркаТоварівНаСкладі_Objest.ДокументРеалізації = ДокументРеалізації.Pointer;
            ЗбіркаТоварівНаСкладі_Objest.Основа = Основа.Pointer;

            ЗбіркаТоварівНаСкладі_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Склад.Pointer.Назва} ";
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            bool isSave = false;

            try
            {
                if (await ЗбіркаТоварівНаСкладі_Objest.Save())
                {
                    await Товари.SaveRecords();
                    isSave = true;
                }
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }

            UnigueID = ЗбіркаТоварівНаСкладі_Objest.UnigueID;
            Caption = ЗбіркаТоварівНаСкладі_Objest.Назва;

            return isSave;
        }

        protected override async ValueTask<bool> SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = await ЗбіркаТоварівНаСкладі_Objest.SpendTheDocument(ЗбіркаТоварівНаСкладі_Objest.ДатаДок);

                if (!isSpend)
                    new ФункціїДляПовідомлень().ПоказатиПовідомлення(ЗбіркаТоварівНаСкладі_Objest.UnigueID);

                return isSpend;
            }
            else
            {
                await ЗбіркаТоварівНаСкладі_Objest.ClearSpendTheDocument();
                return true;
            }
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new ЗбіркаТоварівНаСкладі_Pointer(unigueID);
        }
    }
}