/*

        АктВиконанихРобіт_Елемент.cs
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
    class АктВиконанихРобіт_Елемент : ДокументЕлемент
    {
        public АктВиконанихРобіт_Objest Елемент { get; init; } = new АктВиконанихРобіт_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();
        Каси_PointerControl Каса = new Каси_PointerControl();
        Контрагенти_PointerControl Контрагент = new Контрагенти_PointerControl();
        ДоговориКонтрагентів_PointerControl Договір = new ДоговориКонтрагентів_PointerControl();
        ComboBoxText ГосподарськаОперація = new ComboBoxText();
        ComboBoxText ФормаОплати = new ComboBoxText();
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ" };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        Користувачі_PointerControl Менеджер = new Користувачі_PointerControl() { Caption = "Менеджер" };
        Entry Коментар = new Entry() { WidthRequest = 920 };
        CompositePointerControl Основа = new CompositePointerControl() { BoundConfType = "Документи.АктВиконанихРобіт.Основа" };

        АктВиконанихРобіт_ТабличнаЧастина_Послуги Послуги = new АктВиконанихРобіт_ТабличнаЧастина_Послуги();

        #endregion

        public АктВиконанихРобіт_Елемент()
        {
            Element = Елемент;

            CreateDocName(АктВиконанихРобіт_Const.FULLNAME, НомерДок, ДатаДок);
            CreateField(HBoxComment, "Коментар:", Коментар);

            NotebookTablePart.InsertPage(Послуги, new Label("Послуги"), 0);
            NotebookTablePart.CurrentPage = 0;

            FillComboBoxes();
        }

        void FillComboBoxes()
        {
            //1
            ConfigurationEnums Конфігурація_ГосподарськіОперації = Config.Kernel.Conf.Enums["ГосподарськіОперації"];

            ГосподарськаОперація.Append(
                Перелічення.ГосподарськіОперації.РеалізаціяКлієнту.ToString(),
                Конфігурація_ГосподарськіОперації.Fields["РеалізаціяКлієнту"].Desc);

            ГосподарськаОперація.Active = 0;

            //2
            foreach (var field in Перелічення.ПсевдонімиПерелічення.ФормаОплати_List())
                ФормаОплати.Append(field.Value.ToString(), field.Name);

            ФормаОплати.ActiveId = Перелічення.ФормаОплати.Готівка.ToString();
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
        }

        protected override void CreateContainer3(Box vBox)
        {
            //ГосподарськаОперація
            CreateField(vBox, "Господарська операція:", ГосподарськаОперація);

            //Підрозділ
            CreateField(vBox, null, Підрозділ);

            //Автор
            CreateField(vBox, null, Автор);

            //Менеджер
            CreateField(vBox, null, Менеджер);
        }

        protected override void CreateContainer4(Box vBox)
        {
            //ФормаОплати
            CreateField(vBox, "Форма оплати:", ФормаОплати);

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
                Елемент.Каса = ЗначенняТипові.ОсновнаКаса_Const;
                Елемент.Контрагент = ЗначенняТипові.ОсновнийПостачальник_Const;
                Елемент.Підрозділ = ЗначенняТипові.ОсновнийПідрозділ_Const;
            }

            НомерДок.Text = Елемент.НомерДок;
            ДатаДок.Value = Елемент.ДатаДок;
            Організація.Pointer = Елемент.Організація;
            Валюта.Pointer = Елемент.Валюта;
            Каса.Pointer = Елемент.Каса;
            Контрагент.Pointer = Елемент.Контрагент;
            Договір.Pointer = Елемент.Договір;
            ГосподарськаОперація.ActiveId = Елемент.ГосподарськаОперація.ToString();
            ФормаОплати.ActiveId = Елемент.ФормаОплати.ToString();
            Коментар.Text = Елемент.Коментар;
            Підрозділ.Pointer = Елемент.Підрозділ;
            Автор.Pointer = Елемент.Автор;
            Менеджер.Pointer = Елемент.Менеджер;
            Основа.Pointer = Елемент.Основа;

            //Таблична частина
            Послуги.ЕлементВласник = Елемент;
            await Послуги.LoadRecords();

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
            Елемент.Контрагент = Контрагент.Pointer;
            Елемент.Договір = Договір.Pointer;
            Елемент.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            Елемент.ФормаОплати = Enum.Parse<Перелічення.ФормаОплати>(ФормаОплати.ActiveId);
            Елемент.Коментар = Коментар.Text;
            Елемент.Підрозділ = Підрозділ.Pointer;
            Елемент.Автор = Автор.Pointer;
            Елемент.Менеджер = Менеджер.Pointer;
            Елемент.Основа = Основа.Pointer;

            Елемент.СумаДокументу = Послуги.СумаДокументу();
            Елемент.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Послуги.КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Валюта.Pointer.Назва} {Каса.Pointer.Назва} " +
                $"{Контрагент.Pointer.Назва} ";
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            bool isSave = false;

            try
            {
                if (await Елемент.Save())
                {
                    await Послуги.SaveRecords();
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new АктВиконанихРобіт_Pointer(unigueID));
        }

        protected override async ValueTask InJournal(UnigueID unigueID)
        {
            АктВиконанихРобіт page = new АктВиконанихРобіт() { SelectPointerItem = unigueID };
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, АктВиконанихРобіт_Const.FULLNAME, () => page);
            await page.SetValue();
        }
    }
}