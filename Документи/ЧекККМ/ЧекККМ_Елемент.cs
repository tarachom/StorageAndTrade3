
/*
        ЧекККМ_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;

using GeneratedCode;
using GeneratedCode.Константи;
using GeneratedCode.Довідники;
using GeneratedCode.Документи;
using GeneratedCode.Перелічення;

namespace StorageAndTrade
{
    class ЧекККМ_Елемент : ДокументЕлемент
    {
        public ЧекККМ_Objest Елемент { get; init; } = new ЧекККМ_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Entry Коментар = new Entry() { WidthRequest = 920 };
        Організації_PointerControl Організація = new Організації_PointerControl() { Caption = "Організація" };
        Валюти_PointerControl Валюта = new Валюти_PointerControl() { Caption = "Валюта" };
        Склади_PointerControl Склад = new Склади_PointerControl() { Caption = "Склад" };
        CompositePointerControl Основа = new CompositePointerControl() { BoundConfType = "Документи.ЧекККМ.Основа" };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        КасиККМ_PointerControl КасаККМ = new КасиККМ_PointerControl() { Caption = "Каса ККМ" };

        #endregion

        #region TabularParts

        // Таблична частина "Товари" 
        ЧекККМ_ТабличнаЧастина_Товари Товари = new ЧекККМ_ТабличнаЧастина_Товари() { WidthRequest = 500, HeightRequest = 300 };

        #endregion

        public ЧекККМ_Елемент()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;

            CreateDocName(ЧекККМ_Const.FULLNAME, НомерДок, ДатаДок);
            CreateField(HBoxComment, "Коментар:", Коментар);

            // Таблична частина "Товари" 
            NotebookTablePart.InsertPage(Товари, new Label("Товари"), 0);
            NotebookTablePart.CurrentPage = 0;
        }

        protected override void CreateContainer1(Box vBox)
        {
            // Організація
            CreateField(vBox, null, Організація);

            // Склад
            CreateField(vBox, null, Склад);
        }

        protected override void CreateContainer2(Box vBox)
        {
            // Валюта
            CreateField(vBox, null, Валюта);

            // КасаККМ
            CreateField(vBox, null, КасаККМ);
        }

        protected override void CreateContainer3(Box vBox)
        {
            // Основа
            CreateField(vBox, null, Основа);
        }

        protected override void CreateContainer4(Box vBox)
        {
            //Автор
            CreateField(vBox, null, Автор);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                Елемент.Організація = ЗначенняТипові.ОсновнаОрганізація_Const;
                Елемент.Валюта = ЗначенняТипові.ОсновнаВалюта_Const;
                Елемент.КасаККМ = ЗначенняТипові.ОсновнаКасаККМ_Const;
                Елемент.Склад = ЗначенняТипові.ОсновнийСклад_Const;
            }

            НомерДок.Text = Елемент.НомерДок;
            ДатаДок.Value = Елемент.ДатаДок;
            Коментар.Text = Елемент.Коментар;
            Організація.Pointer = Елемент.Організація;
            Валюта.Pointer = Елемент.Валюта;
            Склад.Pointer = Елемент.Склад;
            КасаККМ.Pointer = Елемент.КасаККМ;
            Основа.Pointer = Елемент.Основа;
            Автор.Pointer = Елемент.Автор;

            // Таблична частина "Товари" 
            Товари.ЕлементВласник = Елемент;
            await Товари.LoadRecords();
        }

        protected override void GetValue()
        {
            Елемент.НомерДок = НомерДок.Text;
            Елемент.ДатаДок = ДатаДок.Value;
            Елемент.Коментар = Коментар.Text;
            Елемент.Організація = Організація.Pointer;
            Елемент.Валюта = Валюта.Pointer;
            Елемент.Склад = Склад.Pointer;
            Елемент.КасаККМ = КасаККМ.Pointer;
            Елемент.Основа = Основа.Pointer;
            Елемент.Автор = Автор.Pointer;
                        
            Елемент.СумаБезЗнижки = Товари.СумаБезЗнижки();
            Елемент.Знижка = Товари.СумаЗнижки();
            Елемент.СумаДокументу = Товари.СумаДокументу();

            Елемент.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return $"\n {Організація.Pointer.Назва} {Валюта.Pointer.Назва} {Склад.Pointer.Назва} {КасаККМ.Pointer.Назва} {Автор.Pointer.Назва}" +
                Товари.КлючовіСловаДляПошуку();
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            bool isSaved = false;
            try
            {
                if (await Елемент.Save())
                {
                    await Товари.SaveRecords(); // Таблична частина "Товари"

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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ЧекККМ_Pointer(unigueID));
        }

        protected override async ValueTask InJournal(UnigueID unigueID)
        {
            ЧекККМ page = new ЧекККМ() { SelectPointerItem = unigueID };
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ЧекККМ_Const.FULLNAME, () => page);
            await page.SetValue();
        }
    }
}
