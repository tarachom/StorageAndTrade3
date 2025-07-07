
/*

        ЗамовленняКлієнта_Елемент.cs
        Елемент

*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;

using GeneratedCode;
using GeneratedCode.Константи;
using GeneratedCode.Документи;
using Перелічення = GeneratedCode.Перелічення;

namespace StorageAndTrade
{
    class ЗамовленняКлієнта_Елемент : ДокументЕлемент
    {
        public ЗамовленняКлієнта_Objest Елемент { get; init; } = new ЗамовленняКлієнта_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();
        Склади_PointerControl Склад = new Склади_PointerControl();
        Каси_PointerControl Каса = new Каси_PointerControl();
        Контрагенти_PointerControl Контрагент = new Контрагенти_PointerControl();
        ДоговориКонтрагентів_PointerControl Договір = new ДоговориКонтрагентів_PointerControl();
        ComboBoxText ГосподарськаОперація = new ComboBoxText();
        ComboBoxText ФормаОплати = new ComboBoxText();
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ:" };
        DateTimeControl ДатаВідгрузки = new DateTimeControl() { OnlyDate = true };
        CheckButton Узгоджений = new CheckButton("Узгоджений");
        БанківськіРахункиКонтрагентів_PointerControl БанківськийрахунокКонтрагента = new БанківськіРахункиКонтрагентів_PointerControl() { Caption = "Рахунок контрагента:", WidthPresentation = 200 };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        CheckButton ПовернутиТару = new CheckButton("Вернути тару");
        DateTimeControl ДатаПоверненняТари = new DateTimeControl() { OnlyDate = true };
        ComboBoxText СпосібДоставки = new ComboBoxText();
        TimeControl ЧасДоставкиЗ = new TimeControl();
        TimeControl ЧасДоставкиДо = new TimeControl();
        Користувачі_PointerControl Менеджер = new Користувачі_PointerControl() { Caption = "Менеджер:" };
        Entry Коментар = new Entry() { WidthRequest = 920 };
        CompositePointerControl Основа = new CompositePointerControl() { BoundConfType = "Документи.ЗамовленняКлієнта.Основа" };

        ЗамовленняКлієнта_ТабличнаЧастина_Товари Товари = new ЗамовленняКлієнта_ТабличнаЧастина_Товари();

        #endregion

        public ЗамовленняКлієнта_Елемент()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;

            CreateDocName(ЗамовленняКлієнта_Const.FULLNAME, НомерДок, ДатаДок);
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
                Перелічення.ГосподарськіОперації.ПлануванняПоЗамовленнямКлієнта.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["ПлануванняПоЗамовленнямКлієнта"].Desc);

            ГосподарськаОперація.Active = 0;

            //2
            foreach (var field in Перелічення.ПсевдонімиПерелічення.ФормаОплати_List())
                ФормаОплати.Append(field.Value.ToString(), field.Name);

            ФормаОплати.ActiveId = Перелічення.ФормаОплати.Готівка.ToString();

            //3
            foreach (var field in Перелічення.ПсевдонімиПерелічення.СпособиДоставки_List())
                СпосібДоставки.Append(field.Value.ToString(), field.Name);

            СпосібДоставки.ActiveId = Перелічення.СпособиДоставки.Самовивіз.ToString();
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
            //Каса
            CreateField(vBox, null, Каса);

            //Валюта
            CreateField(vBox, null, Валюта);

            //Склад
            CreateField(vBox, null, Склад);
        }

        protected override void CreateContainer3(Box vBox)
        {
            //ГосподарськаОперація
            CreateField(vBox, "Господарська операція:", ГосподарськаОперація);

            //Підрозділ
            CreateField(vBox, null, Підрозділ);

            //БанківськийрахунокКонтрагента
            CreateField(vBox, null, БанківськийрахунокКонтрагента);

            //Автор
            CreateField(vBox, null, Автор);

            //Менеджер
            CreateField(vBox, null, Менеджер);

            // Основа
            CreateField(vBox, null, Основа);
        }

        protected override void CreateContainer4(Box vBox)
        {
            //ФормаОплати
            CreateField(vBox, "Форма оплати:", ФормаОплати);

            //СпосібДоставки
            CreateField(vBox, "Спосіб доставки:", СпосібДоставки);

            //ДатаВідгрузки
            CreateField(vBox, "Дата відвантаження:", ДатаВідгрузки);

            //ЧасДоставки
            CreateField(CreateField(vBox, "Час доставки з", ЧасДоставкиЗ), "до", ЧасДоставкиДо);

            //Узгоджений & ВернутиТару
            CreateField(CreateField(vBox, null, Узгоджений), null, ПовернутиТару);

            //ДатаПоверненняТари
            CreateField(vBox, "Дата повернення тари:", ДатаПоверненняТари);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                Елемент.Організація = ЗначенняТипові.ОсновнаОрганізація_Const;
                Елемент.Валюта = ЗначенняТипові.ОсновнаВалюта_Const;
                Елемент.Каса = ЗначенняТипові.ОсновнаКаса_Const;
                Елемент.Склад = ЗначенняТипові.ОсновнийСклад_Const;
                Елемент.Контрагент = ЗначенняТипові.ОсновнийПокупець_Const;
                Елемент.Підрозділ = ЗначенняТипові.ОсновнийПідрозділ_Const;
            }

            НомерДок.Text = Елемент.НомерДок;
            ДатаДок.Value = Елемент.ДатаДок;
            Організація.Pointer = Елемент.Організація;
            Валюта.Pointer = Елемент.Валюта;
            Каса.Pointer = Елемент.Каса;
            Склад.Pointer = Елемент.Склад;
            Контрагент.Pointer = Елемент.Контрагент;
            Договір.Pointer = Елемент.Договір;
            ГосподарськаОперація.ActiveId = Елемент.ГосподарськаОперація.ToString();
            ФормаОплати.ActiveId = Елемент.ФормаОплати.ToString();
            Коментар.Text = Елемент.Коментар;
            Підрозділ.Pointer = Елемент.Підрозділ;
            ДатаВідгрузки.Value = Елемент.ДатаВідгрузки;
            Узгоджений.Active = Елемент.Узгоджений;
            БанківськийрахунокКонтрагента.Pointer = Елемент.БанківськийРахунокКонтрагента;
            Автор.Pointer = Елемент.Автор;
            ПовернутиТару.Active = Елемент.ПовернутиТару;
            ДатаПоверненняТари.Value = Елемент.ДатаПоверненняТари;
            СпосібДоставки.ActiveId = Елемент.СпосібДоставки.ToString();
            ЧасДоставкиЗ.Value = Елемент.ЧасДоставкиЗ;
            ЧасДоставкиДо.Value = Елемент.ЧасДоставкиДо;
            Менеджер.Pointer = Елемент.Менеджер;
            Основа.Pointer = Елемент.Основа;

            //Таблична частина
            Товари.ЕлементВласник = Елемент;
            await Товари.LoadRecords();

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
            Елемент.Каса = Каса.Pointer;
            Елемент.Склад = Склад.Pointer;
            Елемент.Контрагент = Контрагент.Pointer;
            Елемент.Договір = Договір.Pointer;
            Елемент.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            Елемент.ФормаОплати = Enum.Parse<Перелічення.ФормаОплати>(ФормаОплати.ActiveId);
            Елемент.Коментар = Коментар.Text;
            Елемент.Підрозділ = Підрозділ.Pointer;
            Елемент.ДатаВідгрузки = ДатаВідгрузки.Value;
            Елемент.Узгоджений = Узгоджений.Active;
            Елемент.БанківськийРахунокКонтрагента = БанківськийрахунокКонтрагента.Pointer;
            Елемент.Автор = Автор.Pointer;
            Елемент.ПовернутиТару = ПовернутиТару.Active;
            Елемент.ДатаПоверненняТари = ДатаПоверненняТари.Value;
            Елемент.СпосібДоставки = Enum.Parse<Перелічення.СпособиДоставки>(СпосібДоставки.ActiveId);
            Елемент.ЧасДоставкиЗ = ЧасДоставкиЗ.Value;
            Елемент.ЧасДоставкиДо = ЧасДоставкиДо.Value;
            Елемент.Менеджер = Менеджер.Pointer;
            Елемент.Основа = Основа.Pointer;

            Елемент.СумаДокументу = Товари.СумаДокументу();
            Елемент.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Валюта.Pointer.Назва} {Каса.Pointer.Назва} {Склад.Pointer.Назва} {Контрагент.Pointer.Назва} ";
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ЗамовленняКлієнта_Pointer(unigueID));
        }

        protected override async ValueTask InJournal(UnigueID unigueID)
        {
            ЗамовленняКлієнта page = new ЗамовленняКлієнта() { SelectPointerItem = unigueID };
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ЗамовленняКлієнта_Const.FULLNAME, () => page);
            await page.SetValue();
        }
    }
}