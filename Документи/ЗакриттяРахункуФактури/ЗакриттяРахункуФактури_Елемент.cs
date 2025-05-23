
/*
        ЗакриттяРахункуФактури_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using GeneratedCode.Константи;
using GeneratedCode.Документи;
using GeneratedCode.Перелічення;

namespace StorageAndTrade
{
    class ЗакриттяРахункуФактури_Елемент : ДокументЕлемент
    {
        public ЗакриттяРахункуФактури_Objest Елемент { get; init; } = new ЗакриттяРахункуФактури_Objest();

        #region Fields
        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Контрагенти_PointerControl Контрагент = new Контрагенти_PointerControl() { Caption = "Контрагент" };
        Організації_PointerControl Організація = new Організації_PointerControl() { Caption = "Організація" };
        Валюти_PointerControl Валюта = new Валюти_PointerControl() { Caption = "Валюта" };
        Склади_PointerControl Склад = new Склади_PointerControl() { Caption = "Склад" };
        РахунокФактура_PointerControl РахунокФактура = new РахунокФактура_PointerControl() { Caption = "Рахунок фактура" };
        Каси_PointerControl Каса = new Каси_PointerControl() { Caption = "Каса" };
        ДоговориКонтрагентів_PointerControl Договір = new ДоговориКонтрагентів_PointerControl() { Caption = "Договір" };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl() { Caption = "Автор" };
        Entry Коментар = new Entry() { WidthRequest = 920 };
        Користувачі_PointerControl Менеджер = new Користувачі_PointerControl() { Caption = "Менеджер" };
        ComboBoxText ПричиниЗакриттяРахунку = new ComboBoxText();
        CompositePointerControl Основа = new CompositePointerControl() { BoundConfType = "Документи.ЗакриттяРахункуФактури.Основа" };

        #endregion

        #region TabularParts

        // Таблична частина "Товари" 
        ЗакриттяРахункуФактури_ТабличнаЧастина_Товари Товари = new ЗакриттяРахункуФактури_ТабличнаЧастина_Товари() { WidthRequest = 500, HeightRequest = 300 };

        #endregion

        public ЗакриттяРахункуФактури_Елемент()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;

            CreateDocName(ЗакриттяРахункуФактури_Const.FULLNAME, НомерДок, ДатаДок);
            CreateField(HBoxComment, "Коментар:", Коментар);

            // Таблична частина "Товари" 
            NotebookTablePart.InsertPage(Товари, new Label("Товари"), 0);
            NotebookTablePart.CurrentPage = 0;

            foreach (var field in ПсевдонімиПерелічення.ПричиниЗакриттяРахункуФактури_List())
                ПричиниЗакриттяРахунку.Append(field.Value.ToString(), field.Name);
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

            // РахунокФактура
            CreateField(vBox, null, РахунокФактура);
        }

        protected override void CreateContainer2(Box vBox)
        {
            //Каса
            CreateField(vBox, null, Каса);

            //Валюта
            CreateField(vBox, null, Валюта);

            //Склад
            CreateField(vBox, null, Склад);

            // ПричинаЗакриттяЗамовлення
            CreateField(vBox, "Причина закриття:", ПричиниЗакриттяРахунку);
        }

        protected override void CreateContainer3(Box vBox)
        {
            // Автор
            CreateField(vBox, null, Автор);

            // Менеджер
            CreateField(vBox, null, Менеджер);
        }

        protected override void CreateContainer4(Box vBox)
        {
            // Основа
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
                Елемент.Склад = ЗначенняТипові.ОсновнийСклад_Const;
                Елемент.Контрагент = ЗначенняТипові.ОсновнийПокупець_Const;
            }

            НомерДок.Text = Елемент.НомерДок;
            ДатаДок.Value = Елемент.ДатаДок;
            Контрагент.Pointer = Елемент.Контрагент;
            Організація.Pointer = Елемент.Організація;
            Валюта.Pointer = Елемент.Валюта;
            Склад.Pointer = Елемент.Склад;
            РахунокФактура.Pointer = Елемент.РахунокФактура;
            Каса.Pointer = Елемент.Каса;
            Договір.Pointer = Елемент.Договір;
            Автор.Pointer = Елемент.Автор;
            Коментар.Text = Елемент.Коментар;
            Менеджер.Pointer = Елемент.Менеджер;

            ПричиниЗакриттяРахунку.ActiveId = Елемент.ПричинаЗакриттяРахунку.ToString();
            if (ПричиниЗакриттяРахунку.Active == -1) ПричиниЗакриттяРахунку.Active = 0;

            Основа.Pointer = Елемент.Основа;

            // Таблична частина "Товари" 
            Товари.ЕлементВласник = Елемент;
            Товари.ОбновитиЗначенняДокумента = () => Елемент.РахунокФактура = РахунокФактура.Pointer;
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
            Елемент.Контрагент = Контрагент.Pointer;
            Елемент.Організація = Організація.Pointer;
            Елемент.Валюта = Валюта.Pointer;
            Елемент.Склад = Склад.Pointer;
            Елемент.РахунокФактура = РахунокФактура.Pointer;
            Елемент.Каса = Каса.Pointer;
            Елемент.Договір = Договір.Pointer;
            Елемент.Автор = Автор.Pointer;
            Елемент.Коментар = Коментар.Text;
            Елемент.Менеджер = Менеджер.Pointer;

            if (ПричиниЗакриттяРахунку.Active != -1)
                Елемент.ПричинаЗакриттяРахунку = Enum.Parse<ПричиниЗакриттяРахункуФактури>(ПричиниЗакриттяРахунку.ActiveId);

            Елемент.Основа = Основа.Pointer;

            Елемент.СумаДокументу = Товари.СумаДокументу();
            Елемент.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку();
        }

        string КлючовіСловаДляПошуку()
        {
            return "\n {Контрагент.Назва} {Організація.Назва} {Валюта.Назва} {Склад.Назва} {РахунокФактура.Назва} {Каса.Назва} {Договір.Назва} {Автор.Назва} {Менеджер.Назва}"
             + Товари.КлючовіСловаДляПошуку();
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ЗакриттяРахункуФактури_Pointer(unigueID));
        }

        protected override async ValueTask InJournal(UnigueID unigueID)
        {
            ЗакриттяРахункуФактури page = new ЗакриттяРахункуФактури() { SelectPointerItem = unigueID };
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ЗакриттяРахункуФактури_Const.FULLNAME, () => page);
            await page.SetValue();
        }
    }
}
