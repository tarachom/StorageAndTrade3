using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class ПоступленняТоварівТаПослуг_Елемент : VBox
    {
        public ПоступленняТоварівТаПослуг? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public ПоступленняТоварівТаПослуг_Objest ПоступленняТоварівТаПослуг_Objest { get; set; } = new ПоступленняТоварівТаПослуг_Objest();

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
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ" };
        DateTimeControl ДатаОплати = new DateTimeControl();
        CheckButton Узгоджений = new CheckButton("Узгоджений");
        БанківськіРахункиОрганізацій_PointerControl БанківськийрахунокОрганізації = new БанківськіРахункиОрганізацій_PointerControl() { WidthPresentation = 200 };
        БанківськіРахункиКонтрагентів_PointerControl БанківськийрахунокКонтрагента = new БанківськіРахункиКонтрагентів_PointerControl() { Caption = "Рахунок контрагента", WidthPresentation = 200 };
        Entry НомерВхідногоДокументу = new Entry() { WidthRequest = 200 };
        DateTimeControl ДатаВхідногоДокументу = new DateTimeControl();
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        CheckButton ПовернутиТару = new CheckButton("Вернути тару");
        DateTimeControl ДатаПоверненняТари = new DateTimeControl();
        ComboBoxText СпосібДоставки = new ComboBoxText();
        NumericControl Курс = new NumericControl();
        IntegerControl Кратність = new IntegerControl();
        TimeControl ЧасДоставкиЗ = new TimeControl();
        TimeControl ЧасДоставкиДо = new TimeControl();
        Користувачі_PointerControl Менеджер = new Користувачі_PointerControl() { Caption = "Менеджер" };
        СтаттяРухуКоштів_PointerControl СтаттяРухуКоштів = new СтаттяРухуКоштів_PointerControl();
        Entry Коментар = new Entry() { WidthRequest = 900 };

        ПоступленняТоварівТаПослуг_ТабличнаЧастина_Товари Товари = new ПоступленняТоварівТаПослуг_ТабличнаЧастина_Товари();

        #endregion

        public ПоступленняТоварівТаПослуг_Елемент() : base()
        {
            new VBox();
            HBox hBox = new HBox();

            Button bSave = new Button("Зберегти");
            bSave.Clicked += OnSaveClick;

            hBox.PackStart(bSave, false, false, 10);

            Button bSpendTheDocument = new Button("Провести");
            bSpendTheDocument.Clicked += OnSpendTheDocument;

            hBox.PackStart(bSpendTheDocument, false, false, 10);

            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

            hBox.PackStart(bClose, false, false, 10);

            PackStart(hBox, false, false, 10);

            HPaned hPaned = new HPaned() { Orientation = Orientation.Vertical, BorderWidth = 5 };

            FillComboBoxes();

            CreatePack1(hPaned);
            CreatePack2(hPaned);

            PackStart(hPaned, true, true, 5);

            ShowAll();
        }

        void FillComboBoxes()
        {
            if (Config.Kernel != null)
            {
                //1
                ConfigurationEnums Конфігурація_ГосподарськіОперації = Config.Kernel.Conf.Enums["ГосподарськіОперації"];

                ГосподарськаОперація.Append(
                    Перелічення.ГосподарськіОперації.ЗакупівляВПостачальника.ToString(),
                    Конфігурація_ГосподарськіОперації.Fields["ЗакупівляВПостачальника"].Desc);

                ГосподарськаОперація.Active = 0;

                //2
                foreach (ConfigurationEnumField field in Config.Kernel.Conf.Enums["ФормаОплати"].Fields.Values)
                    ФормаОплати.Append(field.Name, field.Desc);

                ФормаОплати.ActiveId = Перелічення.ФормаОплати.Готівка.ToString();

                //3
                foreach (ConfigurationEnumField field in Config.Kernel.Conf.Enums["СпособиДоставки"].Fields.Values)
                    СпосібДоставки.Append(field.Name, field.Desc);

                СпосібДоставки.ActiveId = Перелічення.СпособиДоставки.Самовивіз.ToString();
            }
        }

        void CreatePack1(HPaned hPaned)
        {
            VBox vBox = new VBox();
            hPaned.Pack1(vBox, false, false);

            //НомерДок ДатаДок
            HBox hBoxNumberDataDoc = new HBox() { Halign = Align.Start };
            vBox.PackStart(hBoxNumberDataDoc, false, false, 5);

            hBoxNumberDataDoc.PackStart(new Label("Поступлення товарів та послуг №:"), false, false, 5);
            hBoxNumberDataDoc.PackStart(НомерДок, false, false, 5);
            hBoxNumberDataDoc.PackStart(new Label("від:"), false, false, 5);
            hBoxNumberDataDoc.PackStart(ДатаДок, false, false, 5);

            //Коментар
            HBox hBoxComment = new HBox() { Halign = Align.Start };
            vBox.PackStart(hBoxComment, false, false, 5);

            hBoxComment.PackStart(new Label("Коментар: "), false, false, 5);
            hBoxComment.PackStart(Коментар, false, false, 5);

            //Два блоки для полів -->
            HBox hBoxContainer = new HBox();

            Expander expanderHead = new Expander("Реквізити шапки") { Expanded = true };
            expanderHead.Add(hBoxContainer);

            vBox.PackStart(expanderHead, false, false, 5);

            //Container1
            VBox vBoxContainer1 = new VBox() { WidthRequest = 500 };
            hBoxContainer.PackStart(vBoxContainer1, false, false, 5);

            CreateContainer1(vBoxContainer1);

            //Container2
            VBox vBoxContainer2 = new VBox() { WidthRequest = 500 };
            hBoxContainer.PackStart(vBoxContainer2, false, false, 5);

            CreateContainer2(vBoxContainer2);
            // <--
        }

        void CreateContainer1(VBox vBox)
        {
            //Організація
            HBox hBoxOrganization = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxOrganization, false, false, 5);

            hBoxOrganization.PackStart(Організація, false, false, 5);

            //Контрагент
            HBox hBoxKontragent = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxKontragent, false, false, 5);

            Контрагент.AfterSelectFunc = () =>
            {
                if (Договір.Pointer.IsEmpty())
                {
                    ДоговориКонтрагентів_Pointer? договірКонтрагента =
                    ФункціїДляДокументів.ОсновнийДоговірДляКонтрагента(Контрагент.Pointer, Перелічення.ТипДоговорів.ЗПостачальниками);

                    if (договірКонтрагента != null)
                        Договір.Pointer = договірКонтрагента;
                }
                else
                {
                    if (Контрагент.Pointer.IsEmpty())
                        Договір.Pointer = new ДоговориКонтрагентів_Pointer();
                    else
                    {
                        //
                        //Перевірити чи змінився контрагент
                        //

                        ДоговориКонтрагентів_Objest? договориКонтрагентів_Objest = Договір.Pointer.GetDirectoryObject();

                        if (договориКонтрагентів_Objest != null)
                            if (договориКонтрагентів_Objest.Контрагент != Контрагент.Pointer)
                            {
                                Договір.Pointer = new ДоговориКонтрагентів_Pointer();
                                Контрагент.AfterSelectFunc!.Invoke();
                            };
                    }
                }
            };

            hBoxKontragent.PackStart(Контрагент, false, false, 5);

            //Договір
            HBox hBoxDogovir = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxDogovir, false, false, 5);

            Договір.BeforeClickOpenFunc = () =>
            {
                Договір.КонтрагентВласник = Контрагент.Pointer;
            };

            hBoxDogovir.PackStart(Договір, false, false, 5);

            //Каса
            HBox hBoxKasa = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxKasa, false, false, 5);

            hBoxKasa.PackStart(Каса, false, false, 5);

            //Склад
            HBox hBoxSklad = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSklad, false, false, 5);

            hBoxSklad.PackStart(Склад, false, false, 5);
        }

        void CreateContainer2(VBox vBox)
        {
            //ГосподарськаОперація
            HBox hBoxOperation = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxOperation, false, false, 5);

            hBoxOperation.PackStart(new Label("Господарська операція: "), false, false, 0);
            hBoxOperation.PackStart(ГосподарськаОперація, false, false, 5);

            //ФормаОплати
            HBox hBoxFormaOplaty = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxFormaOplaty, false, false, 5);

            hBoxFormaOplaty.PackStart(new Label("Форма оплати: "), false, false, 0);
            hBoxFormaOplaty.PackStart(ФормаОплати, false, false, 5);

            //Валюта
            HBox hBoxValuta = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxValuta, false, false, 5);

            hBoxValuta.PackStart(Валюта, false, false, 5);
        }

        void CreateContainer3(VBox vBox)
        {
            //Підрозділ
            HBox hBoxPidrozdil = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxPidrozdil, false, false, 5);

            hBoxPidrozdil.PackStart(Підрозділ, false, false, 5);

            //ДатаОплати
            HBox hBoxDataOplaty = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxDataOplaty, false, false, 5);

            hBoxDataOplaty.PackStart(new Label("Дата оплати:"), false, false, 5);
            hBoxDataOplaty.PackStart(ДатаОплати, false, false, 5);

            //БанківськийрахунокКонтрагента
            HBox hBoxBankRahunokKontragenta = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxBankRahunokKontragenta, false, false, 5);

            hBoxBankRahunokKontragenta.PackStart(БанківськийрахунокКонтрагента, false, false, 5);

            //Автор
            HBox hBoxAutor = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxAutor, false, false, 5);

            hBoxAutor.PackStart(Автор, false, false, 5);

            //Менеджер
            HBox hBoxMenedjer = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxMenedjer, false, false, 5);

            hBoxMenedjer.PackStart(Менеджер, false, false, 5);

            //СпосібДоставки
            HBox hBoxSposibDostavky = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSposibDostavky, false, false, 5);

            hBoxSposibDostavky.PackStart(new Label("Спосіб доставки:"), false, false, 0);
            hBoxSposibDostavky.PackStart(СпосібДоставки, false, false, 5);

            //ЧасДоставки
            HBox hBoxTchasDostavky = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxTchasDostavky, false, false, 5);

            hBoxTchasDostavky.PackStart(new Label("Час доставки з"), false, false, 5);
            hBoxTchasDostavky.PackStart(ЧасДоставкиЗ, false, false, 5);
            hBoxTchasDostavky.PackStart(new Label("до"), false, false, 5);
            hBoxTchasDostavky.PackStart(ЧасДоставкиДо, false, false, 5);
        }

        void CreateContainer4(VBox vBox)
        {
            //Узгоджений
            HBox hBoxUzgodjenyi = new HBox() { Halign = Align.Start };
            vBox.PackStart(hBoxUzgodjenyi, false, false, 5);

            hBoxUzgodjenyi.PackStart(Узгоджений, false, false, 5);

            //ВернутиТару
            HBox hBoxVernutyTaru = new HBox() { Halign = Align.Start };
            vBox.PackStart(hBoxVernutyTaru, false, false, 5);

            hBoxVernutyTaru.PackStart(ПовернутиТару, false, false, 5);

            //БанківськийрахунокОрганізації
            HBox hBoxBankRahunokOrganization = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxBankRahunokOrganization, false, false, 5);

            hBoxBankRahunokOrganization.PackStart(БанківськийрахунокОрганізації, false, false, 5);

            //НомерВхідногоДокументу
            HBox hBoxNomerDocVhid = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxNomerDocVhid, false, false, 5);

            hBoxNomerDocVhid.PackStart(new Label("Номер вхід. док.:"), false, false, 5);
            hBoxNomerDocVhid.PackStart(НомерВхідногоДокументу, false, false, 5);

            //ДатаВхідногоДокументу
            HBox hBoxDataDocVhid = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxDataDocVhid, false, false, 5);

            hBoxDataDocVhid.PackStart(new Label("Дата вхід. док.:"), false, false, 5);
            hBoxDataDocVhid.PackStart(ДатаВхідногоДокументу, false, false, 5);

            //ДатаПоверненняТари
            HBox hBoxDataPovernenjaTary = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxDataPovernenjaTary, false, false, 5);

            hBoxDataPovernenjaTary.PackStart(new Label("Дата повернення тари.:"), false, false, 5);
            hBoxDataPovernenjaTary.PackStart(ДатаПоверненняТари, false, false, 5);

            //Курс
            HBox hBoxKurs = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxKurs, false, false, 5);

            hBoxKurs.PackStart(new Label("Курс:"), false, false, 5);
            hBoxKurs.PackStart(Курс, false, false, 5);

            //Кратність
            HBox hBoxKratnist = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxKratnist, false, false, 5);

            hBoxKratnist.PackStart(new Label("Кратність:"), false, false, 5);
            hBoxKratnist.PackStart(Кратність, false, false, 5);

            //СтаттяРухуКоштів
            HBox hBoxStatjaRuhuKoshtiv = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxStatjaRuhuKoshtiv, false, false, 5);

            hBoxStatjaRuhuKoshtiv.PackStart(СтаттяРухуКоштів, false, false, 5);
        }

        void CreatePack2(HPaned hPaned)
        {
            Notebook notebook = new Notebook() { Scrollable = true, EnablePopup = true, BorderWidth = 0, ShowBorder = false };
            notebook.TabPos = PositionType.Top;
            notebook.AppendPage(Товари, new Label("Товари"));

            VBox vBox = new VBox();
            notebook.AppendPage(vBox, new Label("Додаткові реквізити"));

            HBox hBoxContainer = new HBox();
            vBox.PackStart(hBoxContainer, false, false, 5);

            VBox vBoxContainer1 = new VBox() { WidthRequest = 500 };
            hBoxContainer.PackStart(vBoxContainer1, false, false, 5);

            CreateContainer3(vBoxContainer1);

            VBox vBoxContainer2 = new VBox() { WidthRequest = 500 };
            hBoxContainer.PackStart(vBoxContainer2, false, false, 5);

            CreateContainer4(vBoxContainer2);

            hPaned.Pack2(notebook, true, false);
        }

        #region Присвоєння / зчитування значень

        public void SetValue()
        {
            if (IsNew)
            {
                ПоступленняТоварівТаПослуг_Objest.НомерДок = (++НумераціяДокументів.ПоступленняТоварівТаПослуг_Const).ToString("D8");
                ПоступленняТоварівТаПослуг_Objest.ДатаДок = DateTime.Now;
                ПоступленняТоварівТаПослуг_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                ПоступленняТоварівТаПослуг_Objest.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
                ПоступленняТоварівТаПослуг_Objest.Каса = ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const;
                ПоступленняТоварівТаПослуг_Objest.Склад = ЗначенняЗаЗамовчуванням.ОснонийСклад_Const;
                ПоступленняТоварівТаПослуг_Objest.Контрагент = ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
                ПоступленняТоварівТаПослуг_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
                ПоступленняТоварівТаПослуг_Objest.БанківськийРахунокОрганізації = ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const;
            }

            НомерДок.Text = ПоступленняТоварівТаПослуг_Objest.НомерДок;
            ДатаДок.Value = ПоступленняТоварівТаПослуг_Objest.ДатаДок;
            Організація.Pointer = ПоступленняТоварівТаПослуг_Objest.Організація;
            Валюта.Pointer = ПоступленняТоварівТаПослуг_Objest.Валюта;
            Каса.Pointer = ПоступленняТоварівТаПослуг_Objest.Каса;
            Склад.Pointer = ПоступленняТоварівТаПослуг_Objest.Склад;
            Контрагент.Pointer = ПоступленняТоварівТаПослуг_Objest.Контрагент;
            Договір.Pointer = ПоступленняТоварівТаПослуг_Objest.Договір;
            ГосподарськаОперація.ActiveId = ((Перелічення.ГосподарськіОперації)ПоступленняТоварівТаПослуг_Objest.ГосподарськаОперація).ToString();
            ФормаОплати.ActiveId = ((Перелічення.ФормаОплати)ПоступленняТоварівТаПослуг_Objest.ФормаОплати).ToString();
            Коментар.Text = ПоступленняТоварівТаПослуг_Objest.Коментар;
            Підрозділ.Pointer = ПоступленняТоварівТаПослуг_Objest.Підрозділ;
            ДатаОплати.Value = ПоступленняТоварівТаПослуг_Objest.ДатаОплати;
            Узгоджений.Active = ПоступленняТоварівТаПослуг_Objest.Узгоджений;
            БанківськийрахунокОрганізації.Pointer = ПоступленняТоварівТаПослуг_Objest.БанківськийРахунокОрганізації;
            БанківськийрахунокКонтрагента.Pointer = ПоступленняТоварівТаПослуг_Objest.БанківськийРахунокКонтрагента;
            НомерВхідногоДокументу.Text = ПоступленняТоварівТаПослуг_Objest.НомерВхідногоДокументу;
            ДатаВхідногоДокументу.Value = ПоступленняТоварівТаПослуг_Objest.ДатаВхідногоДокументу;
            Автор.Pointer = ПоступленняТоварівТаПослуг_Objest.Автор;
            ПовернутиТару.Active = ПоступленняТоварівТаПослуг_Objest.ПовернутиТару;
            ДатаПоверненняТари.Value = ПоступленняТоварівТаПослуг_Objest.ДатаПоверненняТари;
            СпосібДоставки.ActiveId = ((Перелічення.СпособиДоставки)ПоступленняТоварівТаПослуг_Objest.СпосібДоставки).ToString();
            Курс.Value = ПоступленняТоварівТаПослуг_Objest.Курс;
            Кратність.Value = ПоступленняТоварівТаПослуг_Objest.Кратність;
            ЧасДоставкиЗ.Value = ПоступленняТоварівТаПослуг_Objest.ЧасДоставкиЗ;
            ЧасДоставкиДо.Value = ПоступленняТоварівТаПослуг_Objest.ЧасДоставкиДо;
            Менеджер.Pointer = ПоступленняТоварівТаПослуг_Objest.Менеджер;
            СтаттяРухуКоштів.Pointer = ПоступленняТоварівТаПослуг_Objest.СтаттяРухуКоштів;

            //Таблична частина
            Товари.ПоступленняТоварівТаПослуг_Objest = ПоступленняТоварівТаПослуг_Objest;
            Товари.LoadRecords();

            if (IsNew)
            {
                //Основний договір
                if (Контрагент.AfterSelectFunc != null)
                    Контрагент.AfterSelectFunc.Invoke();
            }
        }

        void GetValue()
        {
            ПоступленняТоварівТаПослуг_Objest.НомерДок = НомерДок.Text;
            ПоступленняТоварівТаПослуг_Objest.ДатаДок = ДатаДок.Value;
            ПоступленняТоварівТаПослуг_Objest.Назва = $"Поступлення товарів та послуг №{ПоступленняТоварівТаПослуг_Objest.НомерДок} від {ПоступленняТоварівТаПослуг_Objest.ДатаДок.ToShortDateString()}";
            ПоступленняТоварівТаПослуг_Objest.Організація = Організація.Pointer;
            ПоступленняТоварівТаПослуг_Objest.Валюта = Валюта.Pointer;
            ПоступленняТоварівТаПослуг_Objest.Каса = Каса.Pointer;
            ПоступленняТоварівТаПослуг_Objest.Склад = Склад.Pointer;
            ПоступленняТоварівТаПослуг_Objest.Контрагент = Контрагент.Pointer;
            ПоступленняТоварівТаПослуг_Objest.Договір = Договір.Pointer;
            ПоступленняТоварівТаПослуг_Objest.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            ПоступленняТоварівТаПослуг_Objest.ФормаОплати = Enum.Parse<Перелічення.ФормаОплати>(ФормаОплати.ActiveId);
            ПоступленняТоварівТаПослуг_Objest.Коментар = Коментар.Text;
            ПоступленняТоварівТаПослуг_Objest.Підрозділ = Підрозділ.Pointer;
            ПоступленняТоварівТаПослуг_Objest.ДатаОплати = ДатаОплати.Value;
            ПоступленняТоварівТаПослуг_Objest.Узгоджений = Узгоджений.Active;
            ПоступленняТоварівТаПослуг_Objest.БанківськийРахунокОрганізації = БанківськийрахунокОрганізації.Pointer;
            ПоступленняТоварівТаПослуг_Objest.БанківськийРахунокКонтрагента = БанківськийрахунокКонтрагента.Pointer;
            ПоступленняТоварівТаПослуг_Objest.НомерВхідногоДокументу = НомерВхідногоДокументу.Text;
            ПоступленняТоварівТаПослуг_Objest.ДатаВхідногоДокументу = ДатаВхідногоДокументу.Value;
            ПоступленняТоварівТаПослуг_Objest.Автор = Автор.Pointer;
            ПоступленняТоварівТаПослуг_Objest.ПовернутиТару = ПовернутиТару.Active;
            ПоступленняТоварівТаПослуг_Objest.ДатаПоверненняТари = ДатаПоверненняТари.Value;
            ПоступленняТоварівТаПослуг_Objest.СпосібДоставки = Enum.Parse<Перелічення.СпособиДоставки>(СпосібДоставки.ActiveId);
            ПоступленняТоварівТаПослуг_Objest.Курс = Курс.Value;
            ПоступленняТоварівТаПослуг_Objest.Кратність = Кратність.Value;
            ПоступленняТоварівТаПослуг_Objest.ЧасДоставкиЗ = ЧасДоставкиЗ.Value;
            ПоступленняТоварівТаПослуг_Objest.ЧасДоставкиДо = ЧасДоставкиДо.Value;
            ПоступленняТоварівТаПослуг_Objest.Менеджер = Менеджер.Pointer;
            ПоступленняТоварівТаПослуг_Objest.СтаттяРухуКоштів = СтаттяРухуКоштів.Pointer;

            ПоступленняТоварівТаПослуг_Objest.СумаДокументу = Товари.СумаДокументу();
        }

        #endregion

        void Save()
        {
            if (IsNew)
            {
                ПоступленняТоварівТаПослуг_Objest.New();
                IsNew = false;
            }

            GetValue();

            ПоступленняТоварівТаПослуг_Objest.Save();
            Товари.SaveRecords();

            Program.GeneralForm?.RenameCurrentPageNotebook($"{ПоступленняТоварівТаПослуг_Objest.Назва}");
        }

        void SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                try
                {
                    if (!ПоступленняТоварівТаПослуг_Objest.SpendTheDocument(ПоступленняТоварівТаПослуг_Objest.ДатаДок))
                        ФункціїДляПовідомлень.ВідкритиТермінал();
                }
                catch (Exception exp)
                {
                    ПоступленняТоварівТаПослуг_Objest.ClearSpendTheDocument();
                    Message.Error(Program.GeneralForm, exp.Message);
                    return;
                }
            }
            else
                ПоступленняТоварівТаПослуг_Objest.ClearSpendTheDocument();
        }

        void ReloadList()
        {
            if (PageList != null)
            {
                Товари.LoadRecords();

                PageList.SelectPointerItem = ПоступленняТоварівТаПослуг_Objest.GetDocumentPointer();
                PageList.LoadRecords();
            }
        }

        void OnSaveClick(object? sender, EventArgs args)
        {
            Save();
            SpendTheDocument(false);

            ReloadList();
        }

        void OnSpendTheDocument(object? sender, EventArgs args)
        {
            Save();
            SpendTheDocument(true);

            ReloadList();
        }
    }
}