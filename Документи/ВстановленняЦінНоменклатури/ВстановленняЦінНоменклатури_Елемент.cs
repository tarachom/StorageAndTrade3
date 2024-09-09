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
    class ВстановленняЦінНоменклатури_Елемент : ДокументЕлемент
    {
        public ВстановленняЦінНоменклатури_Objest Елемент { get; set; } = new ВстановленняЦінНоменклатури_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        ВидиЦін_PointerControl ВидЦіни = new ВидиЦін_PointerControl();
        Entry Коментар = new Entry() { WidthRequest = 920 };

        ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари Товари = new ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари();

        #endregion

        public ВстановленняЦінНоменклатури_Елемент() 
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;

            CreateDocName(ВстановленняЦінНоменклатури_Const.FULLNAME, НомерДок, ДатаДок);

            CreateField(HBoxComment, "Коментар:", Коментар);

            Товари.ЕлементВласник = Елемент;
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
            //Валюта
            CreateField(vBox, null, Валюта);

            //ВидЦіни
            CreateField(vBox, null, ВидЦіни);
        }

        protected override void CreateContainer3(Box vBox)
        {
            //Автор
            CreateField(vBox, null, Автор);
        }

        protected override void CreateContainer4(Box vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                Елемент.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                Елемент.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
            }

            НомерДок.Text = Елемент.НомерДок;
            ДатаДок.Value = Елемент.ДатаДок;
            Організація.Pointer = Елемент.Організація;
            Валюта.Pointer = Елемент.Валюта;
            ВидЦіни.Pointer = Елемент.ВидЦіни;
            Коментар.Text = Елемент.Коментар;
            Автор.Pointer = Елемент.Автор;

            await Товари.LoadRecords();

            Товари.ОбновитиЗначенняДокумента = () =>
            {
                Елемент.ВидЦіни = ВидЦіни.Pointer;
            };
        }

        protected override void GetValue()
        {
            Елемент.НомерДок = НомерДок.Text;
            Елемент.ДатаДок = ДатаДок.Value;
            Елемент.Організація = Організація.Pointer;
            Елемент.Валюта = Валюта.Pointer;
            Елемент.ВидЦіни = ВидЦіни.Pointer;
            Елемент.Коментар = Коментар.Text;
            Елемент.Автор = Автор.Pointer;

            Елемент.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Валюта.Pointer.Назва} ";
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            bool isSave = false;
            
            try
            {
                if (await Елемент.Save())
                {
                    await Товари.SaveRecords();
                    isSave = true;
                }
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Елемент.GetBasis(), Caption, ex);
            }

            return isSave;
        }

        protected override async ValueTask<bool> SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = await Елемент.SpendTheDocument(Елемент.ДатаДок);

                if (!isSpend)
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(Елемент.UnigueID);

                return isSpend;
            }
            else
            {
                await Елемент.ClearSpendTheDocument();
                return true;
            }
        }

        protected override void ReportSpendTheDocument(UnigueID unigueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ВстановленняЦінНоменклатури_Pointer(unigueID));
        }
    }
}