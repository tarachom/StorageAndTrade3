
/*

        ВстановленняЦінНоменклатури_Елемент.cs
        Елемент

*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;

using GeneratedCode.Константи;
using GeneratedCode.Документи;

namespace StorageAndTrade
{
    class ВстановленняЦінНоменклатури_Елемент : ДокументЕлемент
    {
        public ВстановленняЦінНоменклатури_Objest Елемент { get; init; } = new ВстановленняЦінНоменклатури_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        ВидиЦін_PointerControl ВидЦіни = new ВидиЦін_PointerControl();
        Entry Коментар = new Entry() { WidthRequest = 920 };
        CompositePointerControl Основа = new CompositePointerControl() { BoundConfType = "Документи.ВстановленняЦінНоменклатури.Основа" };

        ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари Товари = new ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари();

        #endregion

        public ВстановленняЦінНоменклатури_Елемент()
        {
            Element = Елемент;

            CreateDocName(ВстановленняЦінНоменклатури_Const.FULLNAME, НомерДок, ДатаДок);
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
            //Основа
            CreateField(vBox, null, Основа);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                Елемент.Організація = ЗначенняТипові.ОсновнаОрганізація_Const;
                Елемент.Валюта = ЗначенняТипові.ОсновнаВалюта_Const;
            }

            НомерДок.Text = Елемент.НомерДок;
            ДатаДок.Value = Елемент.ДатаДок;
            Організація.Pointer = Елемент.Організація;
            Валюта.Pointer = Елемент.Валюта;
            ВидЦіни.Pointer = Елемент.ВидЦіни;
            Коментар.Text = Елемент.Коментар;
            Автор.Pointer = Елемент.Автор;
            Основа.Pointer = Елемент.Основа;

            Товари.ЕлементВласник = Елемент;
            await Товари.LoadRecords();

            Товари.ОбновитиЗначенняДокумента = () => Елемент.ВидЦіни = ВидЦіни.Pointer;
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
            Елемент.Основа = Основа.Pointer;

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

        protected override async ValueTask InJournal(UnigueID unigueID)
        {
            ВстановленняЦінНоменклатури page = new ВстановленняЦінНоменклатури() { SelectPointerItem = unigueID };
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ВстановленняЦінНоменклатури_Const.FULLNAME, () => page);
            await page.SetValue();
        }
    }
}