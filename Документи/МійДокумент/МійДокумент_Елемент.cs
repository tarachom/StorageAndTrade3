

/*
        МійДокумент_Елемент.cs
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
    class МійДокумент_Елемент : ДокументЕлемент
    {
        public МійДокумент_Objest Елемент { get; set; } = new МійДокумент_Objest();

        #region Fields

        DateTimeControl ДатаДок = new DateTimeControl();

        Entry НомерДок = new Entry() { WidthRequest = 500 };

        Entry Коментар = new Entry() { WidthRequest = 500 };

        #endregion

        #region TabularParts

        #endregion

        public МійДокумент_Елемент()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;

            CreateDocName(МійДокумент_Const.FULLNAME, НомерДок, ДатаДок);


            CreateField(HBoxComment, "Коментар:", Коментар);

        }

        protected override void CreateContainer1(Box vBox)
        {

        }

        protected override void CreateContainer2(Box vBox)
        {

        }

        protected override void CreateContainer3(Box vBox)
        {

        }

        protected override void CreateContainer4(Box vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
                await Елемент.New();

            ДатаДок.Value = Елемент.ДатаДок;
            НомерДок.Text = Елемент.НомерДок;
            Коментар.Text = Елемент.Коментар;

        }

        protected override void GetValue()
        {

            Елемент.ДатаДок = ДатаДок.Value;

            Елемент.НомерДок = НомерДок.Text;

            Елемент.Коментар = Коментар.Text;

        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            bool isSaved = false;

            try
            {
                if (await Елемент.Save())
                {

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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new МійДокумент_Pointer(unigueID));
        }
    }
}
