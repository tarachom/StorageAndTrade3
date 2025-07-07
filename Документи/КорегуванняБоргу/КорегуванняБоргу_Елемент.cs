
/*

        КорегуванняБоргу_Елемент.cs
        Елемент

*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;

using GeneratedCode.Константи;
using GeneratedCode.Документи;

namespace StorageAndTrade
{
    class КорегуванняБоргу_Елемент : ДокументЕлемент
    {
        public КорегуванняБоргу_Objest Елемент { get; init; } = new КорегуванняБоргу_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl() { Caption = "Організація:" };
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ:" };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl() { Caption = "Автор:" };
        Entry Коментар = new Entry() { WidthRequest = 920 };
        CompositePointerControl Основа = new CompositePointerControl() { BoundConfType = "Документи.КорегуванняБоргу.Основа" };

        КорегуванняБоргу_ТабличнаЧастина_РозрахункиЗКонтрагентами РозрахункиЗКонтрагентами = new КорегуванняБоргу_ТабличнаЧастина_РозрахункиЗКонтрагентами();

        #endregion

        public КорегуванняБоргу_Елемент()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;

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
            //Основа
            CreateField(vBox, null, Основа);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                Елемент.Організація = ЗначенняТипові.ОсновнаОрганізація_Const;
                Елемент.Підрозділ = ЗначенняТипові.ОсновнийПідрозділ_Const;
            }

            НомерДок.Text = Елемент.НомерДок;
            ДатаДок.Value = Елемент.ДатаДок;
            Організація.Pointer = Елемент.Організація;
            Коментар.Text = Елемент.Коментар;
            Підрозділ.Pointer = Елемент.Підрозділ;
            Автор.Pointer = Елемент.Автор;
            Основа.Pointer = Елемент.Основа;

            РозрахункиЗКонтрагентами.ЕлементВласник = Елемент;
            await РозрахункиЗКонтрагентами.LoadRecords();
        }

        protected override void GetValue()
        {
            Елемент.НомерДок = НомерДок.Text;
            Елемент.ДатаДок = ДатаДок.Value;
            Елемент.Організація = Організація.Pointer;
            Елемент.Коментар = Коментар.Text;
            Елемент.Підрозділ = Підрозділ.Pointer;
            Елемент.Автор = Автор.Pointer;
            Елемент.Основа = Основа.Pointer;

            Елемент.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + РозрахункиЗКонтрагентами.КлючовіСловаДляПошуку();
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
                if (await Елемент.Save())
                {
                    await РозрахункиЗКонтрагентами.SaveRecords();
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new КорегуванняБоргу_Pointer(unigueID));
        }

        protected override async ValueTask InJournal(UnigueID unigueID)
        {
            КорегуванняБоргу page = new КорегуванняБоргу() { SelectPointerItem = unigueID };
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, КорегуванняБоргу_Const.FULLNAME, () => page);
            await page.SetValue();
        }
    }
}