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
    class ПсуванняТоварів_Елемент : ДокументЕлемент
    {
        public ПсуванняТоварів_Objest ПсуванняТоварів_Objest { get; set; } = new ПсуванняТоварів_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Склади_PointerControl Склад = new Склади_PointerControl();
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ" };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        Entry Причина = new Entry() { WidthRequest = 920 };
        Entry Коментар = new Entry() { WidthRequest = 920 };
        CompositePointerControl Основа = new CompositePointerControl();

        ПсуванняТоварів_ТабличнаЧастина_Товари Товари = new ПсуванняТоварів_ТабличнаЧастина_Товари();

        #endregion

        public ПсуванняТоварів_Елемент() : base()
        {
            CreateDocName(ПсуванняТоварів_Const.FULLNAME, НомерДок, ДатаДок);

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
                await ПсуванняТоварів_Objest.New();
                ПсуванняТоварів_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                ПсуванняТоварів_Objest.Склад = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                ПсуванняТоварів_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
            }

            НомерДок.Text = ПсуванняТоварів_Objest.НомерДок;
            ДатаДок.Value = ПсуванняТоварів_Objest.ДатаДок;
            Організація.Pointer = ПсуванняТоварів_Objest.Організація;
            Склад.Pointer = ПсуванняТоварів_Objest.Склад;
            Причина.Text = ПсуванняТоварів_Objest.Причина;
            Коментар.Text = ПсуванняТоварів_Objest.Коментар;
            Підрозділ.Pointer = ПсуванняТоварів_Objest.Підрозділ;
            Автор.Pointer = ПсуванняТоварів_Objest.Автор;
            Основа.Pointer = ПсуванняТоварів_Objest.Основа;

            //Таблична частина
            Товари.ПсуванняТоварів_Objest = ПсуванняТоварів_Objest;
            await Товари.LoadRecords();
        }

        protected override void GetValue()
        {
            ПсуванняТоварів_Objest.НомерДок = НомерДок.Text;
            ПсуванняТоварів_Objest.ДатаДок = ДатаДок.Value;
            ПсуванняТоварів_Objest.Організація = Організація.Pointer;
            ПсуванняТоварів_Objest.Склад = Склад.Pointer;
            ПсуванняТоварів_Objest.Причина = Причина.Text;
            ПсуванняТоварів_Objest.Коментар = Коментар.Text;
            ПсуванняТоварів_Objest.Підрозділ = Підрозділ.Pointer;
            ПсуванняТоварів_Objest.Автор = Автор.Pointer;
            ПсуванняТоварів_Objest.Основа = Основа.Pointer;

            ПсуванняТоварів_Objest.СумаДокументу = Товари.СумаДокументу();
            ПсуванняТоварів_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Склад.Pointer.Назва} ";
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            bool isSave = false;
            UnigueID = ПсуванняТоварів_Objest.UnigueID;
            
            try
            {
                if (await ПсуванняТоварів_Objest.Save())
                {
                    await Товари.SaveRecords();
                    isSave = true;
                }

                Caption = ПсуванняТоварів_Objest.Назва;
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(ПсуванняТоварів_Objest.GetBasis(), Caption, ex);
            }

            return isSave;
        }

        protected override async ValueTask<bool> SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = await ПсуванняТоварів_Objest.SpendTheDocument(ПсуванняТоварів_Objest.ДатаДок);

                if (!isSpend)
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(ПсуванняТоварів_Objest.UnigueID);

                return isSpend;
            }
            else
            {
                await ПсуванняТоварів_Objest.ClearSpendTheDocument();
                return true;
            }
        }

        protected override void ReportSpendTheDocument(UnigueID unigueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ПсуванняТоварів_Pointer(unigueID));
        }
    }
}