

/*
        ПерерахунокТоварів_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;

using GeneratedCode.Константи;
using GeneratedCode.Документи;

namespace StorageAndTrade
{
    class ПерерахунокТоварів_Елемент : ДокументЕлемент
    {
        public ПерерахунокТоварів_Objest Елемент { get; init; } = new ПерерахунокТоварів_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Склади_PointerControl Склад = new Склади_PointerControl() { Caption = "Склад", WidthPresentation = 300 };
        ФізичніОсоби_PointerControl Відповідальний = new ФізичніОсоби_PointerControl() { Caption = "Відповідальний", WidthPresentation = 300 };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl() { Caption = "Автор", WidthPresentation = 300 };
        Організації_PointerControl Організація = new Організації_PointerControl() { Caption = "Організація", WidthPresentation = 300 };
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ", WidthPresentation = 300 };
        Entry Коментар = new Entry() { WidthRequest = 920 };
        CompositePointerControl Основа = new CompositePointerControl() { BoundConfType = "Документи.ПерерахунокТоварів.Основа" };

        ПерерахунокТоварів_ТабличнаЧастина_Товари Товари = new ПерерахунокТоварів_ТабличнаЧастина_Товари();

        #endregion

        public ПерерахунокТоварів_Елемент()
        {
            Element = Елемент;

            CreateDocName(ПерерахунокТоварів_Const.FULLNAME, НомерДок, ДатаДок);
            CreateField(HBoxComment, "Коментар:", Коментар);

            NotebookTablePart.InsertPage(Товари, new Label("Товари"), 0);
            NotebookTablePart.CurrentPage = 0;

            FillComboBoxes();
        }

        void FillComboBoxes() { }

        protected override void CreateContainer1(Box vBox)
        {
            //Організація
            CreateField(vBox, null, Організація);

            //Склад
            CreateField(vBox, null, Склад);
        }

        protected override void CreateContainer2(Box vBox)
        {
            //Відповідальний
            CreateField(vBox, null, Відповідальний);
        }

        protected override void CreateContainer3(Box vBox)
        {
            //Автор
            CreateField(vBox, null, Автор);

            //Основа
            CreateField(vBox, null, Основа);
        }

        protected override void CreateContainer4(Box vBox)
        {
            //Підрозділ
            CreateField(vBox, null, Підрозділ);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                Елемент.Організація = ЗначенняТипові.ОсновнаОрганізація_Const;
                Елемент.Підрозділ = ЗначенняТипові.ОсновнийПідрозділ_Const;
                Елемент.Склад = ЗначенняТипові.ОсновнийСклад_Const;
            }

            НомерДок.Text = Елемент.НомерДок;
            ДатаДок.Value = Елемент.ДатаДок;
            Склад.Pointer = Елемент.Склад;
            Відповідальний.Pointer = Елемент.Відповідальний;
            Коментар.Text = Елемент.Коментар;
            Автор.Pointer = Елемент.Автор;
            Організація.Pointer = Елемент.Організація;
            Підрозділ.Pointer = Елемент.Підрозділ;
            Основа.Pointer = Елемент.Основа;

            //Таблична частина Товари
            Товари.ЕлементВласник = Елемент;
            await Товари.LoadRecords();
        }

        protected override void GetValue()
        {
            Елемент.НомерДок = НомерДок.Text;
            Елемент.ДатаДок = ДатаДок.Value;
            Елемент.Склад = Склад.Pointer;
            Елемент.Відповідальний = Відповідальний.Pointer;
            Елемент.Коментар = Коментар.Text;
            Елемент.Автор = Автор.Pointer;
            Елемент.Організація = Організація.Pointer;
            Елемент.Підрозділ = Підрозділ.Pointer;
            Елемент.Основа = Основа.Pointer;

            Елемент.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ПерерахунокТоварів_Pointer(unigueID));
        }

        protected override async ValueTask InJournal(UnigueID unigueID)
        {
            ПерерахунокТоварів page = new ПерерахунокТоварів() { SelectPointerItem = unigueID };
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ПерерахунокТоварів_Const.FULLNAME, () => page);
            await page.SetValue();
        }
    }
}
