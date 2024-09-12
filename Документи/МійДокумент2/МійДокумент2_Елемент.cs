
/*
        МійДокумент2_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class МійДокумент2_Елемент : ДокументЕлемент
    {
        public МійДокумент2_Objest Елемент { get; set; } = new МійДокумент2_Objest();

        #region Fields
        Entry НомерДок = new Entry() { WidthRequest = 500 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Entry Коментар = new Entry() { WidthRequest = 500 };
        Entry Поле1 = new Entry() { WidthRequest = 500 };
        Entry Поле2 = new Entry() { WidthRequest = 500 };

        #endregion

        #region TabularParts

        // Таблична частина "Контакти1" 
        МійДокумент2_ТабличнаЧастина_Контакти1 Контакти1 = new МійДокумент2_ТабличнаЧастина_Контакти1();

        #endregion

        public МійДокумент2_Елемент()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;

            CreateDocName(МійДокумент2_Const.FULLNAME, НомерДок, ДатаДок);

            CreateField(HBoxComment, "Коментар:", Коментар);

            // Таблична частина "Контакти1" 
            NotebookTablePart.InsertPage(Контакти1, new Label("Контакти1"), 0);

            NotebookTablePart.CurrentPage = 0;

        }

        protected override void CreateContainer1(Box vBox)
        {

        }

        protected override void CreateContainer2(Box vBox)
        {

        }

        protected override void CreateContainer3(Box vBox)
        {

            // Поле1
            CreateField(vBox, "Поле1:", Поле1);

            // Поле2
            CreateField(vBox, "Поле2:", Поле2);

        }

        protected override void CreateContainer4(Box vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            НомерДок.Text = Елемент.НомерДок;
            ДатаДок.Value = Елемент.ДатаДок;
            Коментар.Text = Елемент.Коментар;
            Поле1.Text = Елемент.Поле1;
            Поле2.Text = Елемент.Поле2;

            // Таблична частина "Контакти1" 
            Контакти1.ЕлементВласник = Елемент;
            await Контакти1.LoadRecords();

        }

        protected override void GetValue()
        {
            Елемент.НомерДок = НомерДок.Text;
            Елемент.ДатаДок = ДатаДок.Value;
            Елемент.Коментар = Коментар.Text;
            Елемент.Поле1 = Поле1.Text;
            Елемент.Поле2 = Поле2.Text;

        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            bool isSaved = false;

            try
            {
                if (await Елемент.Save())
                {
                    await Контакти1.SaveRecords(); // Таблична частина "Контакти1"

                    isSaved = true;
                }
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Елемент.GetBasis(), Caption, ex);
            }

            return isSaved;
        }

        protected override async ValueTask<bool> SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = await Елемент.SpendTheDocument(Елемент.ДатаДок);
                if (!isSpend) ФункціїДляПовідомлень.ПоказатиПовідомлення(Елемент.UnigueID);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new МійДокумент2_Pointer(unigueID));
        }
    }
}
