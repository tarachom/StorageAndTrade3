
/*

        ВведенняЗалишків_Елемент.cs
        Елемент

*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;

using GeneratedCode;
using GeneratedCode.Константи;
using GeneratedCode.Довідники;
using GeneratedCode.Документи;
using Перелічення = GeneratedCode.Перелічення;

namespace StorageAndTrade
{
    class ВведенняЗалишків_Елемент : ДокументЕлемент
    {
        public ВведенняЗалишків_Objest Елемент { get; init; } = new ВведенняЗалишків_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();
        Склади_PointerControl Склад = new Склади_PointerControl();
        Контрагенти_PointerControl Контрагент = new Контрагенти_PointerControl();
        ДоговориКонтрагентів_PointerControl Договір = new ДоговориКонтрагентів_PointerControl();
        ComboBoxText ГосподарськаОперація = new ComboBoxText();
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ" };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        Entry Коментар = new Entry() { WidthRequest = 920 };
        CompositePointerControl Основа = new CompositePointerControl() { BoundConfType = "Документи.ВведенняЗалишків.Основа" };

        ВведенняЗалишків_ТабличнаЧастина_Товари Товари = new ВведенняЗалишків_ТабличнаЧастина_Товари();
        ВведенняЗалишків_ТабличнаЧастина_Каси Каси = new ВведенняЗалишків_ТабличнаЧастина_Каси();
        ВведенняЗалишків_ТабличнаЧастина_БанківськіРахунки БанківськіРахунки = new ВведенняЗалишків_ТабличнаЧастина_БанківськіРахунки();
        ВведенняЗалишків_ТабличнаЧастина_РозрахункиЗКонтрагентами РозрахункиЗКонтрагентами = new ВведенняЗалишків_ТабличнаЧастина_РозрахункиЗКонтрагентами();

        #endregion

        public ВведенняЗалишків_Елемент()
        {
            Element = Елемент;

            CreateDocName(ВведенняЗалишків_Const.FULLNAME, НомерДок, ДатаДок);
            CreateField(HBoxComment, "Коментар:", Коментар);

            NotebookTablePart.InsertPage(Товари, new Label("Товари"), 0);
            NotebookTablePart.InsertPage(Каси, new Label("Каси"), 1);
            NotebookTablePart.InsertPage(БанківськіРахунки, new Label("Банківські рахунки"), 2);
            NotebookTablePart.InsertPage(РозрахункиЗКонтрагентами, new Label("Розрахунки з контрагентами"), 3);

            NotebookTablePart.CurrentPage = 0;

            FillComboBoxes();
        }

        void FillComboBoxes()
        {
            //1
            ConfigurationEnums Конфігурація_ГосподарськіОперації = Config.Kernel.Conf.Enums["ГосподарськіОперації"];

            ГосподарськаОперація.Append(
                Перелічення.ГосподарськіОперації.ВведенняЗалишків.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["ВведенняЗалишків"].Desc);

            ГосподарськаОперація.Active = 0;
        }

        protected override void CreateContainer1(Box vBox)
        {
            //Організація
            CreateField(vBox, null, Організація);

            //Контрагент
            CreateField(vBox, null, Контрагент);
            Контрагент.AfterSelectFunc = async () => await Контрагент.ПривязкаДоДоговору(Договір);

            //Договір
            CreateField(vBox, null, Договір);
            Договір.BeforeClickOpenFunc = () => Договір.КонтрагентВласник = Контрагент.Pointer;
        }

        protected override void CreateContainer2(Box vBox)
        {
            //Склад
            CreateField(vBox, null, Склад);

            //Валюта
            CreateField(vBox, null, Валюта);
        }

        protected override void CreateContainer3(Box vBox)
        {
            //ГосподарськаОперація
            CreateField(vBox, "Господарська операція:", ГосподарськаОперація);

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
                Елемент.Валюта = ЗначенняТипові.ОсновнаВалюта_Const;
                Елемент.Склад = ЗначенняТипові.ОсновнийСклад_Const;
                Елемент.Контрагент = ЗначенняТипові.ОсновнийПостачальник_Const;
                Елемент.Підрозділ = ЗначенняТипові.ОсновнийПідрозділ_Const;
            }

            НомерДок.Text = Елемент.НомерДок;
            ДатаДок.Value = Елемент.ДатаДок;
            Організація.Pointer = Елемент.Організація;
            Валюта.Pointer = Елемент.Валюта;
            Склад.Pointer = Елемент.Склад;
            Контрагент.Pointer = Елемент.Контрагент;
            Договір.Pointer = Елемент.Договір;
            ГосподарськаОперація.ActiveId = Елемент.ГосподарськаОперація.ToString();
            Коментар.Text = Елемент.Коментар;
            Підрозділ.Pointer = Елемент.Підрозділ;
            Автор.Pointer = Елемент.Автор;
            Основа.Pointer = Елемент.Основа;

            Товари.ЕлементВласник = Елемент;
            await Товари.LoadRecords();

            Каси.ЕлементВласник = Елемент;
            await Каси.LoadRecords();

            БанківськіРахунки.ЕлементВласник = Елемент;
            await БанківськіРахунки.LoadRecords();

            РозрахункиЗКонтрагентами.ЕлементВласник = Елемент;
            await РозрахункиЗКонтрагентами.LoadRecords();

            if (IsNew)
            {
                //Основний договір
                Контрагент.AfterSelectFunc?.Invoke();
            }
        }

        protected override void GetValue()
        {
            Елемент.НомерДок = НомерДок.Text;
            Елемент.ДатаДок = ДатаДок.Value;
            Елемент.Організація = Організація.Pointer;
            Елемент.Валюта = Валюта.Pointer;
            Елемент.Склад = Склад.Pointer;
            Елемент.Контрагент = Контрагент.Pointer;
            Елемент.Договір = Договір.Pointer;
            Елемент.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            Елемент.Коментар = Коментар.Text;
            Елемент.Підрозділ = Підрозділ.Pointer;
            Елемент.Автор = Автор.Pointer;
            Елемент.Основа = Основа.Pointer;

            Елемент.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() +
                Товари.КлючовіСловаДляПошуку() +
                БанківськіРахунки.КлючовіСловаДляПошуку() +
                Каси.КлючовіСловаДляПошуку() +
                РозрахункиЗКонтрагентами.КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Валюта.Pointer.Назва} {Склад.Pointer.Назва} {Контрагент.Pointer.Назва} ";
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
                    await Каси.SaveRecords();
                    await БанківськіРахунки.SaveRecords();
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ВведенняЗалишків_Pointer(unigueID));
        }

        protected override async ValueTask InJournal(UnigueID unigueID)
        {
            ВведенняЗалишків page = new ВведенняЗалишків() { SelectPointerItem = unigueID };
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ВведенняЗалишків_Const.FULLNAME, () => page);
            await page.SetValue();
        }
    }
}