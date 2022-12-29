using Gtk;

using AccountingSoftware;
using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class ДоговориКонтрагентів_Елемент : VBox
    {
        public ДоговориКонтрагентів? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public ДоговориКонтрагентів_Objest ДоговориКонтрагентів_Objest { get; set; } = new ДоговориКонтрагентів_Objest();

        #region Fields

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        DateTimeControl Дата = new DateTimeControl() { OnlyDate = true };
        Entry Номер = new Entry() { WidthRequest = 100 };
        БанківськіРахункиОрганізацій_PointerControl БанківськийРахунок = new БанківськіРахункиОрганізацій_PointerControl();
        БанківськіРахункиКонтрагентів_PointerControl БанківськийРахунокКонтрагента = new БанківськіРахункиКонтрагентів_PointerControl();
        Валюти_PointerControl ВалютаВзаєморозрахунків = new Валюти_PointerControl();
        DateTimeControl ДатаПочаткуДії = new DateTimeControl() { OnlyDate = true };
        DateTimeControl ДатаЗакінченняДії = new DateTimeControl() { OnlyDate = true };
        Організації_PointerControl Організація = new Організації_PointerControl();
        Контрагенти_PointerControl Контрагент = new Контрагенти_PointerControl();
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl();
        CheckButton Узгоджений = new CheckButton("Узгоджений");
        ComboBoxText Статус = new ComboBoxText();
        ComboBoxText ГосподарськаОперація = new ComboBoxText();
        ComboBoxText ТипДоговору = new ComboBoxText();
        NumericControl ДопустимаСумаЗаборгованості = new NumericControl();
        NumericControl Сума = new NumericControl();
        Entry Коментар = new Entry() { WidthRequest = 500 };

        #endregion

        public ДоговориКонтрагентів_Елемент() : base()
        {
            new VBox();
            HBox hBox = new HBox();

            Button bSave = new Button("Зберегти");
            bSave.Clicked += OnSaveClick;

            hBox.PackStart(bSave, false, false, 10);

            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

            hBox.PackStart(bClose, false, false, 10);

            PackStart(hBox, false, false, 10);

            FillComboBoxes();

            HPaned hPaned = new HPaned() { BorderWidth = 5, Position = 500 };

            CreatePack1(hPaned);
            CreatePack2(hPaned);

            PackStart(hPaned, false, false, 5);

            ShowAll();
        }

        void FillComboBoxes()
        {
            if (Config.Kernel != null)
            {
                //1
                foreach (ConfigurationEnumField field in Config.Kernel.Conf.Enums["ГосподарськіОперації"].Fields.Values)
                    ГосподарськаОперація.Append(field.Name, field.Desc);

                ГосподарськаОперація.ActiveId = Перелічення.ГосподарськіОперації.РеалізаціяКлієнту.ToString();

                //2
                foreach (ConfigurationEnumField field in Config.Kernel.Conf.Enums["СтатусиДоговорівКонтрагентів"].Fields.Values)
                    Статус.Append(field.Name, field.Desc);

                Статус.ActiveId = Перелічення.СтатусиДоговорівКонтрагентів.Діє.ToString();

                //3
                foreach (ConfigurationEnumField field in Config.Kernel.Conf.Enums["ТипДоговорів"].Fields.Values)
                    ТипДоговору.Append(field.Name, field.Desc);

                ТипДоговору.ActiveId = Перелічення.ТипДоговорів.ЗПокупцями.ToString();
            }
        }

        void CreatePack1(HPaned hPaned)
        {
            VBox vBox = new VBox();
            hPaned.Pack1(vBox, false, false);

            //Код
            HBox hBoxCode = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxCode, false, false, 5);

            hBoxCode.PackStart(new Label("Код:"), false, false, 5);
            hBoxCode.PackStart(Код, false, false, 5);

            //Назва
            HBox hBoxName = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxName, false, false, 5);

            hBoxName.PackStart(new Label("Назва:"), false, false, 5);
            hBoxName.PackStart(Назва, false, false, 5);

            //Дата
            HBox hBoxData = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxData, false, false, 5);

            hBoxData.PackStart(new Label("Дата:"), false, false, 5);
            hBoxData.PackStart(Дата, false, false, 5);

            //Номер
            HBox hBoxNomer = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxNomer, false, false, 5);

            hBoxNomer.PackStart(new Label("Номер:"), false, false, 5);
            hBoxNomer.PackStart(Номер, false, false, 5);

            //БанківськийРахунок
            HBox hBoxBankRachunok = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxBankRachunok, false, false, 5);

            hBoxBankRachunok.PackStart(БанківськийРахунок, false, false, 5);

            //БанківськийРахунокКонтрагента
            HBox hBoxBankRachunokKontragenta = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxBankRachunokKontragenta, false, false, 5);

            hBoxBankRachunokKontragenta.PackStart(БанківськийРахунокКонтрагента, false, false, 5);

            //ВалютаВзаєморозрахунків
            HBox hBoxValuta = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxValuta, false, false, 5);

            hBoxValuta.PackStart(ВалютаВзаєморозрахунків, false, false, 5);

            //ДатаПочаткуДії
            HBox hBoxDataStart = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxDataStart, false, false, 5);

            hBoxDataStart.PackStart(new Label("Дата початку дії:"), false, false, 5);
            hBoxDataStart.PackStart(ДатаПочаткуДії, false, false, 5);

            //ДатаЗакінченняДії
            HBox hBoxDataStop = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxDataStop, false, false, 5);

            hBoxDataStop.PackStart(new Label("Дата закінчення дії:"), false, false, 5);
            hBoxDataStop.PackStart(ДатаЗакінченняДії, false, false, 5);

            //Організація
            HBox hBoxOrganization = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxOrganization, false, false, 5);

            hBoxOrganization.PackStart(Організація, false, false, 5);

            //Контрагент
            HBox hBoxKontragent = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxKontragent, false, false, 5);

            hBoxKontragent.PackStart(Контрагент, false, false, 5);

            //Підрозділ
            HBox hBoxPidrozdil = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxPidrozdil, false, false, 5);

            hBoxPidrozdil.PackStart(Підрозділ, false, false, 5);

            //Узгоджений
            HBox hBoxUzgodjenyi = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxUzgodjenyi, false, false, 5);

            hBoxUzgodjenyi.PackStart(Узгоджений, false, false, 5);

            //Статус
            HBox hBoxStatus = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxStatus, false, false, 5);

            hBoxStatus.PackStart(new Label("Статус: "), false, false, 0);
            hBoxStatus.PackStart(Статус, false, false, 5);

            //ГосподарськаОперація
            HBox hBoxOperation = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxOperation, false, false, 5);

            hBoxOperation.PackStart(new Label("Господарська операція: "), false, false, 0);
            hBoxOperation.PackStart(ГосподарськаОперація, false, false, 5);

            //ТипДоговору
            HBox hBoxTypeDogovor = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxTypeDogovor, false, false, 5);

            hBoxTypeDogovor.PackStart(new Label("Тип договору: "), false, false, 0);
            hBoxTypeDogovor.PackStart(ТипДоговору, false, false, 5);

            //ДопустимаСумаЗаборгованості
            HBox hBoxDopustymaSumaBorgu = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxDopustymaSumaBorgu, false, false, 5);

            hBoxDopustymaSumaBorgu.PackStart(new Label("Допустима сума заборгованості: "), false, false, 5);
            hBoxDopustymaSumaBorgu.PackStart(ДопустимаСумаЗаборгованості, false, false, 5);

            //Сума
            HBox hBoxSuma = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSuma, false, false, 5);

            hBoxSuma.PackStart(new Label("Сума: "), false, false, 5);
            hBoxSuma.PackStart(Сума, false, false, 5);

            //Коментар
            HBox hBoxComment = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxComment, false, false, 5);

            hBoxComment.PackStart(new Label("Коментар: "), false, false, 5);
            hBoxComment.PackStart(Коментар, false, false, 5);
        }

        void CreatePack2(HPaned hPaned)
        {
            VBox vBox = new VBox();



            hPaned.Pack2(vBox, false, false);
        }

        #region Присвоєння / зчитування значень

        public void SetValue()
        {
            if (IsNew)
                ДоговориКонтрагентів_Objest.Код = (++НумераціяДовідників.ДоговориКонтрагентів_Const).ToString("D6");

            Код.Text = ДоговориКонтрагентів_Objest.Код;
            Назва.Text = ДоговориКонтрагентів_Objest.Назва;
            Дата.Value = ДоговориКонтрагентів_Objest.Дата;
            Номер.Text = ДоговориКонтрагентів_Objest.Номер;
            БанківськийРахунок.Pointer = ДоговориКонтрагентів_Objest.БанківськийРахунок;
            БанківськийРахунокКонтрагента.Pointer = ДоговориКонтрагентів_Objest.БанківськийРахунокКонтрагента;
            ВалютаВзаєморозрахунків.Pointer = ДоговориКонтрагентів_Objest.ВалютаВзаєморозрахунків;
            ДатаПочаткуДії.Value = ДоговориКонтрагентів_Objest.ДатаПочаткуДії;
            ДатаЗакінченняДії.Value = ДоговориКонтрагентів_Objest.ДатаЗакінченняДії;
            Організація.Pointer = ДоговориКонтрагентів_Objest.Організація;
            Контрагент.Pointer = ДоговориКонтрагентів_Objest.Контрагент;
            Підрозділ.Pointer = ДоговориКонтрагентів_Objest.Підрозділ;
            Узгоджений.Active = ДоговориКонтрагентів_Objest.Узгоджений;
            Статус.ActiveId = ((Перелічення.СтатусиДоговорівКонтрагентів)ДоговориКонтрагентів_Objest.Статус).ToString();
            ГосподарськаОперація.ActiveId = ((Перелічення.ГосподарськіОперації)ДоговориКонтрагентів_Objest.ГосподарськаОперація).ToString();
            ТипДоговору.ActiveId = ((Перелічення.ТипДоговорів)ДоговориКонтрагентів_Objest.ТипДоговору).ToString();
            ДопустимаСумаЗаборгованості.Value = ДоговориКонтрагентів_Objest.ДопустимаСумаЗаборгованості;
            Сума.Value = ДоговориКонтрагентів_Objest.Сума;
            Коментар.Text = ДоговориКонтрагентів_Objest.Коментар;
        }

        void GetValue()
        {
            ДоговориКонтрагентів_Objest.Код = Код.Text;
            ДоговориКонтрагентів_Objest.Назва = Назва.Text;
            ДоговориКонтрагентів_Objest.Дата = Дата.Value;
            ДоговориКонтрагентів_Objest.Номер = Номер.Text;
            ДоговориКонтрагентів_Objest.БанківськийРахунок = БанківськийРахунок.Pointer;
            ДоговориКонтрагентів_Objest.БанківськийРахунокКонтрагента = БанківськийРахунокКонтрагента.Pointer;
            ДоговориКонтрагентів_Objest.ВалютаВзаєморозрахунків = ВалютаВзаєморозрахунків.Pointer;
            ДоговориКонтрагентів_Objest.ДатаПочаткуДії = ДатаПочаткуДії.Value;
            ДоговориКонтрагентів_Objest.ДатаЗакінченняДії = ДатаЗакінченняДії.Value;
            ДоговориКонтрагентів_Objest.Організація = Організація.Pointer;
            ДоговориКонтрагентів_Objest.Контрагент = Контрагент.Pointer;
            ДоговориКонтрагентів_Objest.Підрозділ = Підрозділ.Pointer;
            ДоговориКонтрагентів_Objest.Узгоджений = Узгоджений.Active;
            ДоговориКонтрагентів_Objest.Статус = Enum.Parse<Перелічення.СтатусиДоговорівКонтрагентів>(Статус.ActiveId);
            ДоговориКонтрагентів_Objest.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            ДоговориКонтрагентів_Objest.ТипДоговору = Enum.Parse<Перелічення.ТипДоговорів>(ТипДоговору.ActiveId);
            ДоговориКонтрагентів_Objest.ДопустимаСумаЗаборгованості = ДопустимаСумаЗаборгованості.Value;
            ДоговориКонтрагентів_Objest.Сума = Сума.Value;
            ДоговориКонтрагентів_Objest.Коментар = Коментар.Text;
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
            {
                ДоговориКонтрагентів_Objest.New();
                IsNew = false;
            }

            GetValue();

            ДоговориКонтрагентів_Objest.Save();

            Program.GeneralForm?.RenameCurrentPageNotebook($"Договір: {ДоговориКонтрагентів_Objest.Назва}");

            if (PageList != null)
            {
                PageList.SelectPointerItem = ДоговориКонтрагентів_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}