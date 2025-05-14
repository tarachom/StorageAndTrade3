
/*

        ПереміщенняТоварів_Елемент.cs
        Елемент

*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using GeneratedCode;
using GeneratedCode.Константи;
using GeneratedCode.Документи;
using Перелічення = GeneratedCode.Перелічення;

namespace StorageAndTrade
{
    class ПереміщенняТоварів_Елемент : ДокументЕлемент
    {
        public ПереміщенняТоварів_Objest Елемент { get; init; } = new ПереміщенняТоварів_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Організації_PointerControl ОрганізаціяОтримувач = new Організації_PointerControl();
        Склади_PointerControl СкладВідправник = new Склади_PointerControl() { Caption = "Склад відправник:" };
        Склади_PointerControl СкладОтримувач = new Склади_PointerControl() { Caption = "Склад отримувач:" };
        ComboBoxText ГосподарськаОперація = new ComboBoxText();
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ" };
        БанківськіРахункиОрганізацій_PointerControl БанківськийРахунокОрганізації = new БанківськіРахункиОрганізацій_PointerControl() { WidthPresentation = 200 };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        ComboBoxText СпосібДоставки = new ComboBoxText();
        TimeControl ЧасДоставкиЗ = new TimeControl();
        TimeControl ЧасДоставкиДо = new TimeControl();
        Entry Коментар = new Entry() { WidthRequest = 920 };
        CompositePointerControl Основа = new CompositePointerControl() { BoundConfType = "Документи.ПереміщенняТоварів.Основа" };

        ПереміщенняТоварів_ТабличнаЧастина_Товари Товари = new ПереміщенняТоварів_ТабличнаЧастина_Товари();

        #endregion

        public ПереміщенняТоварів_Елемент()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;

            CreateDocName(ПереміщенняТоварів_Const.FULLNAME, НомерДок, ДатаДок);
            CreateField(HBoxComment, "Коментар:", Коментар);

            NotebookTablePart.InsertPage(Товари, new Label("Товари"), 0);
            NotebookTablePart.CurrentPage = 0;

            FillComboBoxes();
        }

        void FillComboBoxes()
        {
            //1
            ConfigurationEnums Конфігурація_ГосподарськіОперації = Config.Kernel.Conf.Enums["ГосподарськіОперації"];

            ГосподарськаОперація.Append(
                Перелічення.ГосподарськіОперації.ПереміщенняТоварів.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["ПереміщенняТоварів"].Desc);

            ГосподарськаОперація.Active = 0;

            //3
            foreach (var field in Перелічення.ПсевдонімиПерелічення.СпособиДоставки_List())
                СпосібДоставки.Append(field.Value.ToString(), field.Name);

            СпосібДоставки.ActiveId = Перелічення.СпособиДоставки.Самовивіз.ToString();
        }

        protected override void CreateContainer1(Box vBox)
        {
            //Організація
            CreateField(vBox, null, Організація);

            //СкладВідправник
            CreateField(vBox, null, СкладВідправник);
        }

        protected override void CreateContainer2(Box vBox)
        {
            //СкладОтримувач
            CreateField(vBox, null, СкладОтримувач);
        }

        protected override void CreateContainer3(Box vBox)
        {
            //Підрозділ
            CreateField(vBox, null, Підрозділ);

            //БанківськийрахунокОрганізації
            CreateField(vBox, null, БанківськийРахунокОрганізації);

            //Автор
            CreateField(vBox, null, Автор);

            //Основа
            CreateField(vBox, null, Основа);
        }

        protected override void CreateContainer4(Box vBox)
        {
            //ГосподарськаОперація
            CreateField(vBox, "Господарська операція:", ГосподарськаОперація);

            //СпосібДоставки
            CreateField(vBox, "Спосіб доставки:", СпосібДоставки);

            //ЧасДоставки
            CreateField(CreateField(vBox, "Час доставки з", ЧасДоставкиЗ), "до", ЧасДоставкиДо);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                Елемент.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                Елемент.СкладВідправник = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                Елемент.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
                Елемент.БанківськийРахунокОрганізації = ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const;
            }

            НомерДок.Text = Елемент.НомерДок;
            ДатаДок.Value = Елемент.ДатаДок;
            Організація.Pointer = Елемент.Організація;
            СкладВідправник.Pointer = Елемент.СкладВідправник;
            СкладОтримувач.Pointer = Елемент.СкладОтримувач;
            ГосподарськаОперація.ActiveId = Елемент.ГосподарськаОперація.ToString();
            Коментар.Text = Елемент.Коментар;
            Підрозділ.Pointer = Елемент.Підрозділ;
            БанківськийРахунокОрганізації.Pointer = Елемент.БанківськийРахунокОрганізації;
            Автор.Pointer = Елемент.Автор;
            СпосібДоставки.ActiveId = Елемент.СпосібДоставки.ToString();
            ЧасДоставкиЗ.Value = Елемент.ЧасДоставкиЗ;
            ЧасДоставкиДо.Value = Елемент.ЧасДоставкиДо;
            Основа.Pointer = Елемент.Основа;

            //Таблична частина
            Товари.ЕлементВласник = Елемент;
            await Товари.LoadRecords();
        }

        protected override void GetValue()
        {
            Елемент.НомерДок = НомерДок.Text;
            Елемент.ДатаДок = ДатаДок.Value;
            Елемент.Організація = Організація.Pointer;
            Елемент.СкладВідправник = СкладВідправник.Pointer;
            Елемент.СкладОтримувач = СкладОтримувач.Pointer;
            Елемент.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            Елемент.Коментар = Коментар.Text;
            Елемент.Підрозділ = Підрозділ.Pointer;
            Елемент.БанківськийРахунокОрганізації = БанківськийРахунокОрганізації.Pointer;
            Елемент.Автор = Автор.Pointer;
            Елемент.СпосібДоставки = Enum.Parse<Перелічення.СпособиДоставки>(СпосібДоставки.ActiveId);
            Елемент.ЧасДоставкиЗ = ЧасДоставкиЗ.Value;
            Елемент.ЧасДоставкиДо = ЧасДоставкиДо.Value;
            Елемент.Основа = Основа.Pointer;

            Елемент.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {СкладВідправник.Pointer.Назва} {СкладОтримувач.Pointer.Назва}";
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ПереміщенняТоварів_Pointer(unigueID));
        }

        protected override async ValueTask InJournal(UnigueID unigueID)
        {
            ПереміщенняТоварів page = new ПереміщенняТоварів() { SelectPointerItem = unigueID };
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ПереміщенняТоварів_Const.FULLNAME, () => page);
            await page.SetValue();
        }
    }
}