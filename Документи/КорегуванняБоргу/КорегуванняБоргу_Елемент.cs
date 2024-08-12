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
    class КорегуванняБоргу_Елемент : ДокументЕлемент
    {
        public КорегуванняБоргу_Objest КорегуванняБоргу_Objest { get; set; } = new КорегуванняБоргу_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl() { Caption = "Організація:" };
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ:" };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl() { Caption = "Автор:" };
        Entry Коментар = new Entry() { WidthRequest = 920 };

        КорегуванняБоргу_ТабличнаЧастина_РозрахункиЗКонтрагентами РозрахункиЗКонтрагентами = new КорегуванняБоргу_ТабличнаЧастина_РозрахункиЗКонтрагентами();

        #endregion

        public КорегуванняБоргу_Елемент() : base()
        {
            CreateDocName(КорегуванняБоргу_Const.FULLNAME, НомерДок, ДатаДок);

            CreateField(HBoxComment, "Коментар:", Коментар);

            NotebookTablePart.InsertPage(РозрахункиЗКонтрагентами, new Label("Розрахунки з контрагентами"), 0);
            NotebookTablePart.CurrentPage = 0;
        }

        protected override void CreateContainer1(Box vBox)
        {
            //Організація
            CreateField(vBox, null, Організація);
        }

        protected override void CreateContainer2(Box vBox)
        {

        }

        protected override void CreateContainer3(Box vBox)
        {
            //Підрозділ
            CreateField(vBox, null, Підрозділ);

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
                await КорегуванняБоргу_Objest.New();
                КорегуванняБоргу_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                КорегуванняБоргу_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
            }

            НомерДок.Text = КорегуванняБоргу_Objest.НомерДок;
            ДатаДок.Value = КорегуванняБоргу_Objest.ДатаДок;
            Організація.Pointer = КорегуванняБоргу_Objest.Організація;
            Коментар.Text = КорегуванняБоргу_Objest.Коментар;
            Підрозділ.Pointer = КорегуванняБоргу_Objest.Підрозділ;
            Автор.Pointer = КорегуванняБоргу_Objest.Автор;

            РозрахункиЗКонтрагентами.КорегуванняБоргу_Objest = КорегуванняБоргу_Objest;
            await РозрахункиЗКонтрагентами.LoadRecords();
        }

        protected override void GetValue()
        {
            КорегуванняБоргу_Objest.НомерДок = НомерДок.Text;
            КорегуванняБоргу_Objest.ДатаДок = ДатаДок.Value;
            КорегуванняБоргу_Objest.Організація = Організація.Pointer;
            КорегуванняБоргу_Objest.Коментар = Коментар.Text;
            КорегуванняБоргу_Objest.Підрозділ = Підрозділ.Pointer;
            КорегуванняБоргу_Objest.Автор = Автор.Pointer;

            КорегуванняБоргу_Objest.КлючовіСловаДляПошуку =
                КлючовіСловаДляПошуку() +
                РозрахункиЗКонтрагентами.КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва}";
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            bool isSave = false;

            try
            {
                if (await КорегуванняБоргу_Objest.Save())
                {
                    await РозрахункиЗКонтрагентами.SaveRecords();
                    isSave = true;
                }
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }

            UnigueID = КорегуванняБоргу_Objest.UnigueID;
            Caption = КорегуванняБоргу_Objest.Назва;

            return isSave;
        }

        protected override async ValueTask<bool> SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = await КорегуванняБоргу_Objest.SpendTheDocument(КорегуванняБоргу_Objest.ДатаДок);

                if (!isSpend)
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(КорегуванняБоргу_Objest.UnigueID);

                return isSpend;
            }
            else
            {
                await КорегуванняБоргу_Objest.ClearSpendTheDocument();

                return true;
            }
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new КорегуванняБоргу_Pointer(unigueID);
        }
    }
}