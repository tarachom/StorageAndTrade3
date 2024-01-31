

/*
        ПерерахунокТоварів_Елемент.cs
        Елемент
*/

using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class ПерерахунокТоварів_Елемент : ДокументЕлемент
    {
        public ПерерахунокТоварів_Objest ПерерахунокТоварів_Objest { get; set; } = new ПерерахунокТоварів_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Склади_PointerControl Склад = new Склади_PointerControl() { Caption = "Склад", WidthPresentation = 300 };
        ФізичніОсоби_PointerControl Відповідальний = new ФізичніОсоби_PointerControl() { Caption = "Відповідальний", WidthPresentation = 300 };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl() { Caption = "Автор", WidthPresentation = 300 };
        Організації_PointerControl Організація = new Організації_PointerControl() { Caption = "Організація", WidthPresentation = 300 };
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ", WidthPresentation = 300 };
        Entry Коментар = new Entry() { WidthRequest = 920 };

        #endregion

        #region TabularParts
        ПерерахунокТоварів_ТабличнаЧастина_Товари Товари = new ПерерахунокТоварів_ТабличнаЧастина_Товари();

        #endregion

        public ПерерахунокТоварів_Елемент() : base()
        {
            CreateDocName(ПерерахунокТоварів_Const.FULLNAME, НомерДок, ДатаДок);

            CreateField(HBoxComment, "Коментар:", Коментар);

            NotebookTablePart.InsertPage(Товари, new Label("Товари"), 0);
            NotebookTablePart.CurrentPage = 0;

            FillComboBoxes();
        }

        void FillComboBoxes()
        {

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
            //Відповідальний
            CreateField(vBox, null, Відповідальний);
        }

        protected override void CreateContainer3(VBox vBox)
        {
            //Автор
            CreateField(vBox, null, Автор);
        }

        protected override void CreateContainer4(VBox vBox)
        {
            //Підрозділ
            CreateField(vBox, null, Підрозділ);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                await ПерерахунокТоварів_Objest.New();

                ПерерахунокТоварів_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                ПерерахунокТоварів_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
                ПерерахунокТоварів_Objest.Склад = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
            }

            НомерДок.Text = ПерерахунокТоварів_Objest.НомерДок;
            ДатаДок.Value = ПерерахунокТоварів_Objest.ДатаДок;
            Склад.Pointer = ПерерахунокТоварів_Objest.Склад;
            Відповідальний.Pointer = ПерерахунокТоварів_Objest.Відповідальний;
            Коментар.Text = ПерерахунокТоварів_Objest.Коментар;
            Автор.Pointer = ПерерахунокТоварів_Objest.Автор;
            Організація.Pointer = ПерерахунокТоварів_Objest.Організація;
            Підрозділ.Pointer = ПерерахунокТоварів_Objest.Підрозділ;

            /* Таблична частина: Товари */
            Товари.ПерерахунокТоварів_Objest = ПерерахунокТоварів_Objest;
            await Товари.LoadRecords();
        }

        protected override void GetValue()
        {
            ПерерахунокТоварів_Objest.НомерДок = НомерДок.Text;
            ПерерахунокТоварів_Objest.ДатаДок = ДатаДок.Value;
            ПерерахунокТоварів_Objest.Склад = Склад.Pointer;
            ПерерахунокТоварів_Objest.Відповідальний = Відповідальний.Pointer;
            ПерерахунокТоварів_Objest.Коментар = Коментар.Text;
            ПерерахунокТоварів_Objest.Автор = Автор.Pointer;
            ПерерахунокТоварів_Objest.Організація = Організація.Pointer;
            ПерерахунокТоварів_Objest.Підрозділ = Підрозділ.Pointer;

            ПерерахунокТоварів_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Склад.Pointer.Назва} {Відповідальний.Pointer.Назва} {Підрозділ.Pointer.Назва}";
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            bool isSave = false;

            try
            {
                if (await ПерерахунокТоварів_Objest.Save())
                {
                    await Товари.SaveRecords();
                    isSave = true;
                }
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }

            UnigueID = ПерерахунокТоварів_Objest.UnigueID;
            Caption = ПерерахунокТоварів_Objest.Назва;

            return isSave;
        }

        protected override async ValueTask<bool> SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = await ПерерахунокТоварів_Objest.SpendTheDocument(ПерерахунокТоварів_Objest.ДатаДок);

                if (!isSpend)
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(ПерерахунокТоварів_Objest.UnigueID);

                return isSpend;
            }
            else
            {
                await ПерерахунокТоварів_Objest.ClearSpendTheDocument();

                return true;
            }
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new ПерерахунокТоварів_Pointer(unigueID);
        }
    }
}
