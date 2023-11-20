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
    class ВстановленняЦінНоменклатури_Елемент : ДокументЕлемент
    {
        public ВстановленняЦінНоменклатури_Objest ВстановленняЦінНоменклатури_Objest { get; set; } = new ВстановленняЦінНоменклатури_Objest();

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

        public ВстановленняЦінНоменклатури_Елемент() : base()
        {
            CreateDocName(ВстановленняЦінНоменклатури_Const.FULLNAME, НомерДок, ДатаДок);

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
            //Валюта
            CreateField(vBox, null, Валюта);

            //ВидЦіни
            CreateField(vBox, null, ВидЦіни);
        }

        protected override void CreateContainer3(VBox vBox)
        {
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
                ВстановленняЦінНоменклатури_Objest.New();
                ВстановленняЦінНоменклатури_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                ВстановленняЦінНоменклатури_Objest.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
            }

            НомерДок.Text = ВстановленняЦінНоменклатури_Objest.НомерДок;
            ДатаДок.Value = ВстановленняЦінНоменклатури_Objest.ДатаДок;
            Організація.Pointer = ВстановленняЦінНоменклатури_Objest.Організація;
            Валюта.Pointer = ВстановленняЦінНоменклатури_Objest.Валюта;
            ВидЦіни.Pointer = ВстановленняЦінНоменклатури_Objest.ВидЦіни;
            Коментар.Text = ВстановленняЦінНоменклатури_Objest.Коментар;
            Автор.Pointer = ВстановленняЦінНоменклатури_Objest.Автор;

            Товари.ВстановленняЦінНоменклатури_Objest = ВстановленняЦінНоменклатури_Objest;
            Товари.ОбновитиЗначенняДокумента = () =>
            {
                ВстановленняЦінНоменклатури_Objest.ВидЦіни = ВидЦіни.Pointer;
            };

            await Товари.LoadRecords();
        }

        protected override void GetValue()
        {
            ВстановленняЦінНоменклатури_Objest.НомерДок = НомерДок.Text;
            ВстановленняЦінНоменклатури_Objest.ДатаДок = ДатаДок.Value;
            ВстановленняЦінНоменклатури_Objest.Організація = Організація.Pointer;
            ВстановленняЦінНоменклатури_Objest.Валюта = Валюта.Pointer;
            ВстановленняЦінНоменклатури_Objest.ВидЦіни = ВидЦіни.Pointer;
            ВстановленняЦінНоменклатури_Objest.Коментар = Коментар.Text;
            ВстановленняЦінНоменклатури_Objest.Автор = Автор.Pointer;

            ВстановленняЦінНоменклатури_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Валюта.Pointer.Назва} ";
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            bool isSave;

            try
            {
                isSave = await ВстановленняЦінНоменклатури_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
                return false;
            }

            if (isSave)
                await Товари.SaveRecords();

            UnigueID = ВстановленняЦінНоменклатури_Objest.UnigueID;
            Caption = ВстановленняЦінНоменклатури_Objest.Назва;

            return isSave;
        }

        protected override async ValueTask<bool> SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = await ВстановленняЦінНоменклатури_Objest.SpendTheDocument(ВстановленняЦінНоменклатури_Objest.ДатаДок);

                if (!isSpend)
                    ФункціїДляПовідомлень.ВідкритиТермінал();

                return isSpend;
            }
            else
            {
                await ВстановленняЦінНоменклатури_Objest.ClearSpendTheDocument();
                return true;
            }
        }
    }
}