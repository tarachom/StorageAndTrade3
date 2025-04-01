
/*
        РозхіднийКасовийОрдер_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using GeneratedCode;
using GeneratedCode.Константи;
using GeneratedCode.Довідники;
using GeneratedCode.Документи;
using Перелічення = GeneratedCode.Перелічення;

namespace StorageAndTrade
{
    class РозхіднийКасовийОрдер_Елемент : ДокументЕлемент
    {
        public РозхіднийКасовийОрдер_Objest Елемент { get; init; } = new РозхіднийКасовийОрдер_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Організації_PointerControl ОрганізаціяОтримувач = new Організації_PointerControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();
        Каси_PointerControl Каса = new Каси_PointerControl();
        Каси_PointerControl КасаОтримувач = new Каси_PointerControl() { Caption = "Каса отримувач:" };
        NumericControl Курс = new NumericControl();
        NumericControl СумаДокументу = new NumericControl();
        Контрагенти_PointerControl Контрагент = new Контрагенти_PointerControl();
        ДоговориКонтрагентів_PointerControl Договір = new ДоговориКонтрагентів_PointerControl();
        ComboBoxText ГосподарськаОперація = new ComboBoxText();
        БанківськіРахункиОрганізацій_PointerControl БанківськийРахунок = new БанківськіРахункиОрганізацій_PointerControl() { Caption = "Банківський рахунок:" };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        СтаттяРухуКоштів_PointerControl СтаттяРухуКоштів = new СтаттяРухуКоштів_PointerControl();
        Entry Коментар = new Entry() { WidthRequest = 920 };
        CompositePointerControl Основа = new CompositePointerControl() { BoundConfType = "Документи.РозхіднийКасовийОрдер.Основа" };

        #endregion

        public РозхіднийКасовийОрдер_Елемент()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;

            CreateDocName(РозхіднийКасовийОрдер_Const.FULLNAME, НомерДок, ДатаДок);
            CreateField(HBoxComment, "Коментар:", Коментар);

            FillComboBoxes();
        }

        void FillComboBoxes()
        {
            //1
            ConfigurationEnums Конфігурація_ГосподарськіОперації = Config.Kernel.Conf.Enums["ГосподарськіОперації"];

            ГосподарськаОперація.Append(
                Перелічення.ГосподарськіОперації.ОплатаПостачальнику.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["ОплатаПостачальнику"].Desc);

            ГосподарськаОперація.Append(
                Перелічення.ГосподарськіОперації.ВидачаКоштівВІншуКасу.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["ВидачаКоштівВІншуКасу"].Desc);

            ГосподарськаОперація.Append(
                Перелічення.ГосподарськіОперації.ЗдачаКоштівВБанк.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["ЗдачаКоштівВБанк"].Desc);

            ГосподарськаОперація.Append(
                Перелічення.ГосподарськіОперації.ІншіВитрати.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["ІншіВитрати"].Desc);

            ГосподарськаОперація.Append(
                Перелічення.ГосподарськіОперації.ПоверненняОплатиКлієнту.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["ПоверненняОплатиКлієнту"].Desc);
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

            //Валюта
            CreateField(vBox, null, Валюта);
        }

        protected override void CreateContainer2(Box vBox)
        {
            //ГосподарськаОперація
            CreateField(vBox, "Господарська операція:", ГосподарськаОперація);
            ГосподарськаОперація.Changed += OnComboBoxChanged_ГосподарськаОперація;

            //Каса
            CreateField(vBox, null, Каса);

            //КасаОтримувач
            CreateField(vBox, null, КасаОтримувач);

            //БанківськийРахунок
            CreateField(vBox, null, БанківськийРахунок);

            //СумаДокументу та Курс
            CreateField(CreateField(vBox, "Сума:", СумаДокументу), "Курс:", Курс);
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
            //СтаттяРухуКоштів
            CreateField(vBox, null, СтаттяРухуКоштів);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
            {
                Елемент.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                Елемент.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
                Елемент.Каса = ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const; ;
                Елемент.Контрагент = ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
                Елемент.БанківськийРахунок = ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const;
            }

            if (IsNew || Елемент.ГосподарськаОперація == 0)
                Елемент.ГосподарськаОперація = Перелічення.ГосподарськіОперації.ОплатаПостачальнику;

            НомерДок.Text = Елемент.НомерДок;
            ДатаДок.Value = Елемент.ДатаДок;
            Організація.Pointer = Елемент.Організація;
            Валюта.Pointer = Елемент.Валюта;
            Каса.Pointer = Елемент.Каса;
            КасаОтримувач.Pointer = Елемент.КасаОтримувач;
            Контрагент.Pointer = Елемент.Контрагент;
            Договір.Pointer = Елемент.Договір;
            ГосподарськаОперація.ActiveId = Елемент.ГосподарськаОперація.ToString();
            Коментар.Text = Елемент.Коментар;
            БанківськийРахунок.Pointer = Елемент.БанківськийРахунок;
            Автор.Pointer = Елемент.Автор;
            СумаДокументу.Value = Елемент.СумаДокументу;
            Курс.Value = Елемент.Курс;
            СтаттяРухуКоштів.Pointer = Елемент.СтаттяРухуКоштів;
            Основа.Pointer = Елемент.Основа;

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
            Елемент.КасаОтримувач = КасаОтримувач.Pointer;
            Елемент.Контрагент = Контрагент.Pointer;
            Елемент.Договір = Договір.Pointer;
            Елемент.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            Елемент.Коментар = Коментар.Text;
            Елемент.БанківськийРахунок = БанківськийРахунок.Pointer;
            Елемент.Автор = Автор.Pointer;
            Елемент.СумаДокументу = СумаДокументу.Value;
            Елемент.Курс = Курс.Value;
            Елемент.СтаттяРухуКоштів = СтаттяРухуКоштів.Pointer;
            Елемент.Основа = Основа.Pointer;

            Елемент.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Валюта.Pointer.Назва} {Каса.Pointer.Назва} {КасаОтримувач.Pointer.Назва} {Контрагент.Pointer.Назва}";
        }

        #endregion

        void OnComboBoxChanged_ГосподарськаОперація(object? sender, EventArgs args)
        {
            switch (Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId))
            {
                case Перелічення.ГосподарськіОперації.ВидачаКоштівВІншуКасу:
                    {
                        КасаОтримувач.Sensitive = true;
                        Курс.Sensitive = true;
                        Контрагент.Sensitive = false;
                        Договір.Sensitive = false;
                        БанківськийРахунок.Sensitive = false;

                        break;
                    }
                case Перелічення.ГосподарськіОперації.ЗдачаКоштівВБанк:
                    {
                        КасаОтримувач.Sensitive = false;
                        Курс.Sensitive = false;
                        Контрагент.Sensitive = false;
                        Договір.Sensitive = false;
                        БанківськийРахунок.Sensitive = true;

                        break;
                    }
                default:
                    {
                        КасаОтримувач.Sensitive = false;
                        Курс.Sensitive = false;
                        Контрагент.Sensitive = true;
                        Договір.Sensitive = true;
                        БанківськийРахунок.Sensitive = false;

                        break;
                    }
            }
        }

        protected override async ValueTask<bool> Save()
        {
            bool isSave = false;

            try
            {
                isSave = await Елемент.Save();
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new РозхіднийКасовийОрдер_Pointer(unigueID));
        }
    }
}