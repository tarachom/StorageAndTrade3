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

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class РозміщенняНоменклатуриПоКоміркам_Елемент : ДокументЕлемент
    {
        public РозміщенняНоменклатуриПоКоміркам_Objest РозміщенняНоменклатуриПоКоміркам_Objest { get; set; } = new РозміщенняНоменклатуриПоКоміркам_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Склади_PointerControl Склад = new Склади_PointerControl();
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ:" };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        Entry Коментар = new Entry() { WidthRequest = 920 };
        РозміщенняНоменклатуриПоКоміркам_ТабличнаЧастина_Товари Товари = new РозміщенняНоменклатуриПоКоміркам_ТабличнаЧастина_Товари();
        CompositePointerControl Основа = new CompositePointerControl();

        #endregion

        public РозміщенняНоменклатуриПоКоміркам_Елемент() : base()
        {
            CreateDocName(РозміщенняНоменклатуриПоКоміркам_Const.FULLNAME, НомерДок, ДатаДок);

            CreateField(HBoxComment, "Коментар:", Коментар);

            NotebookTablePart.InsertPage(Товари, new Label("Товари"), 0);
            NotebookTablePart.CurrentPage = 0;
        }

        protected override void CreateContainer1(VBox vBox)
        {
            //Організація
            CreateField(vBox, null, Організація);
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

        public override void SetValue()
        {
            if (IsNew)
            {
                РозміщенняНоменклатуриПоКоміркам_Objest.New();
                РозміщенняНоменклатуриПоКоміркам_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                РозміщенняНоменклатуриПоКоміркам_Objest.Склад = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                РозміщенняНоменклатуриПоКоміркам_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
            }

            НомерДок.Text = РозміщенняНоменклатуриПоКоміркам_Objest.НомерДок;
            ДатаДок.Value = РозміщенняНоменклатуриПоКоміркам_Objest.ДатаДок;
            Організація.Pointer = РозміщенняНоменклатуриПоКоміркам_Objest.Організація;
            Склад.Pointer = РозміщенняНоменклатуриПоКоміркам_Objest.Склад;
            Коментар.Text = РозміщенняНоменклатуриПоКоміркам_Objest.Коментар;
            Підрозділ.Pointer = РозміщенняНоменклатуриПоКоміркам_Objest.Підрозділ;
            Автор.Pointer = РозміщенняНоменклатуриПоКоміркам_Objest.Автор;
            Основа.Pointer = РозміщенняНоменклатуриПоКоміркам_Objest.Основа;

            //Таблична частина
            Товари.РозміщенняНоменклатуриПоКоміркам_Objest = РозміщенняНоменклатуриПоКоміркам_Objest;
            Товари.LoadRecords();
        }

        protected override void GetValue()
        {
            РозміщенняНоменклатуриПоКоміркам_Objest.НомерДок = НомерДок.Text;
            РозміщенняНоменклатуриПоКоміркам_Objest.ДатаДок = ДатаДок.Value;
            РозміщенняНоменклатуриПоКоміркам_Objest.Організація = Організація.Pointer;
            РозміщенняНоменклатуриПоКоміркам_Objest.Склад = Склад.Pointer;
            РозміщенняНоменклатуриПоКоміркам_Objest.Коментар = Коментар.Text;
            РозміщенняНоменклатуриПоКоміркам_Objest.Підрозділ = Підрозділ.Pointer;
            РозміщенняНоменклатуриПоКоміркам_Objest.Автор = Автор.Pointer;
            РозміщенняНоменклатуриПоКоміркам_Objest.Основа = Основа.Pointer;

            РозміщенняНоменклатуриПоКоміркам_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Склад.Pointer.Назва}";
        }

        #endregion

        protected override bool Save()
        {
            bool isSave = false;

            try
            {
                isSave = РозміщенняНоменклатуриПоКоміркам_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
                return false;
            }

            if (isSave)
                Товари.SaveRecords();

            UnigueID = РозміщенняНоменклатуриПоКоміркам_Objest.UnigueID;
            Caption = РозміщенняНоменклатуриПоКоміркам_Objest.Назва;

            return isSave;
        }

        protected override bool SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = РозміщенняНоменклатуриПоКоміркам_Objest.SpendTheDocument(РозміщенняНоменклатуриПоКоміркам_Objest.ДатаДок);

                if (!isSpend)
                    ФункціїДляПовідомлень.ВідкритиТермінал();

                return isSpend;
            }
            else
            {
                РозміщенняНоменклатуриПоКоміркам_Objest.ClearSpendTheDocument();

                return true;
            }
        }
    }
}