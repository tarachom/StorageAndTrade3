/*
Copyright (C) 2019-2023 TARAKHOMYN YURIY IVANOVYCH
All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

/*
Автор:    Тарахомин Юрій Іванович
Адреса:   Україна, м. Львів
Сайт:     accounting.org.ua
*/

using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class РеалізаціяТоварівТаПослуг_Елемент : VBox
    {
        public РеалізаціяТоварівТаПослуг? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public РеалізаціяТоварівТаПослуг_Objest РеалізаціяТоварівТаПослуг_Objest { get; set; } = new РеалізаціяТоварівТаПослуг_Objest();

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
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        CheckButton ПовернутиТару = new CheckButton("Повернути тару");
        DateTimeControl ДатаПоверненняТари = new DateTimeControl();
        ComboBoxText СпосібДоставки = new ComboBoxText();
        NumericControl Курс = new NumericControl();
        IntegerControl Кратність = new IntegerControl();
        TimeControl ЧасДоставкиЗ = new TimeControl();
        TimeControl ЧасДоставкиДо = new TimeControl();
        Користувачі_PointerControl Менеджер = new Користувачі_PointerControl() { Caption = "Менеджер" };
        Entry Коментар = new Entry() { WidthRequest = 920 };
        Basis_PointerControl Основа = new Basis_PointerControl();

        РеалізаціяТоварівТаПослуг_ТабличнаЧастина_Товари Товари = new РеалізаціяТоварівТаПослуг_ТабличнаЧастина_Товари();

        #endregion

        public РеалізаціяТоварівТаПослуг_Елемент() : base()
        {
            HBox hBox = new HBox();

            Button bSaveAndSpend = new Button("Провести та закрити");
            bSaveAndSpend.Clicked += OnSaveAndSpendClick;

            hBox.PackStart(bSaveAndSpend, false, false, 10);

            Button bSave = new Button("Зберегти без проведення");
            bSave.Clicked += OnSaveClick;

            hBox.PackStart(bSave, false, false, 10);

            //Проводки
            LinkButton linkButtonProvodky = new LinkButton("Проводки") { Halign = Align.Start };
            linkButtonProvodky.Clicked += (object? sender, EventArgs args) =>
            {
                Program.GeneralForm?.CreateNotebookPage($"Проводки", () =>
                {
                    Звіт_РухДокументівПоРегістрах page = new Звіт_РухДокументівПоРегістрах();
                    page.CreateReport(РеалізаціяТоварівТаПослуг_Objest.GetDocumentPointer());
                    return page;
                });
            };

            hBox.PackStart(linkButtonProvodky, false, false, 10);

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
                    Перелічення.ГосподарськіОперації.РеалізаціяКлієнту.ToString(),
                    Конфігурація_ГосподарськіОперації.Fields["РеалізаціяКлієнту"].Desc);

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

            hBoxNumberDataDoc.PackStart(new Label("Реалізація товарів та послуг №:"), false, false, 5);
            hBoxNumberDataDoc.PackStart(НомерДок, false, false, 5);
            hBoxNumberDataDoc.PackStart(new Label("від:"), false, false, 5);
            hBoxNumberDataDoc.PackStart(ДатаДок, false, false, 5);

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

            //Коментар
            HBox hBoxComment = new HBox() { Halign = Align.Start };
            vBox.PackStart(hBoxComment, false, false, 5);

            hBoxComment.PackStart(new Label("Коментар: "), false, false, 5);
            hBoxComment.PackStart(Коментар, false, false, 5);
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
                    ФункціїДляДокументів.ОсновнийДоговірДляКонтрагента(Контрагент.Pointer, Перелічення.ТипДоговорів.ЗПокупцями);

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
        }

        void CreateContainer2(VBox vBox)
        {
            //Каса
            HBox hBoxKasa = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxKasa, false, false, 5);

            hBoxKasa.PackStart(Каса, false, false, 5);

            //Валюта
            HBox hBoxValuta = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxValuta, false, false, 5);

            hBoxValuta.PackStart(Валюта, false, false, 5);

            //Склад
            HBox hBoxSklad = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSklad, false, false, 5);

            hBoxSklad.PackStart(Склад, false, false, 5);
        }

        void CreateContainer3(VBox vBox)
        {
            //ГосподарськаОперація
            HBox hBoxOperation = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxOperation, false, false, 5);

            hBoxOperation.PackStart(new Label("Господарська операція: "), false, false, 0);
            hBoxOperation.PackStart(ГосподарськаОперація, false, false, 5);

            //Підрозділ
            HBox hBoxPidrozdil = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxPidrozdil, false, false, 5);

            hBoxPidrozdil.PackStart(Підрозділ, false, false, 5);

            //БанківськийрахунокКонтрагента
            HBox hBoxBankRahunokKontragenta = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxBankRahunokKontragenta, false, false, 5);

            hBoxBankRahunokKontragenta.PackStart(БанківськийрахунокКонтрагента, false, false, 5);

            //БанківськийрахунокОрганізації
            HBox hBoxBankRahunokOrganization = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxBankRahunokOrganization, false, false, 5);

            hBoxBankRahunokOrganization.PackStart(БанківськийрахунокОрганізації, false, false, 5);

            //Автор
            HBox hBoxAutor = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxAutor, false, false, 5);

            hBoxAutor.PackStart(Автор, false, false, 5);

            //Менеджер
            HBox hBoxMenedjer = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxMenedjer, false, false, 5);

            hBoxMenedjer.PackStart(Менеджер, false, false, 5);

            //Основа
            HBox hBoxBasis = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxBasis, false, false, 5);

            hBoxBasis.PackStart(Основа, false, false, 5);
        }

        void CreateContainer4(VBox vBox)
        {
            //ФормаОплати
            HBox hBoxFormaOplaty = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxFormaOplaty, false, false, 5);

            hBoxFormaOplaty.PackStart(new Label("Форма оплати: "), false, false, 0);
            hBoxFormaOplaty.PackStart(ФормаОплати, false, false, 5);

            //ДатаОплати
            HBox hBoxDataOplaty = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxDataOplaty, false, false, 5);

            hBoxDataOplaty.PackStart(new Label("Дата оплати:"), false, false, 5);
            hBoxDataOplaty.PackStart(ДатаОплати, false, false, 5);

            //Курс та Кратність
            HBox hBoxKurs = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxKurs, false, false, 5);

            hBoxKurs.PackStart(new Label("Курс:"), false, false, 5);
            hBoxKurs.PackStart(Курс, false, false, 5);

            hBoxKurs.PackStart(new Label("Кратність:"), false, false, 5);
            hBoxKurs.PackStart(Кратність, false, false, 5);

            //Узгоджений та ПовернутиТару
            HBox hBoxUzgodjenyi = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxUzgodjenyi, false, false, 5);

            hBoxUzgodjenyi.PackStart(Узгоджений, false, false, 5);
            hBoxUzgodjenyi.PackStart(ПовернутиТару, false, false, 5);

            //ДатаПоверненняТари
            HBox hBoxDataPovernenjaTary = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxDataPovernenjaTary, false, false, 5);

            hBoxDataPovernenjaTary.PackStart(new Label("Дата повернення тари:"), false, false, 5);
            hBoxDataPovernenjaTary.PackStart(ДатаПоверненняТари, false, false, 5);

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
                РеалізаціяТоварівТаПослуг_Objest.New();
                РеалізаціяТоварівТаПослуг_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                РеалізаціяТоварівТаПослуг_Objest.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
                РеалізаціяТоварівТаПослуг_Objest.Каса = ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const;
                РеалізаціяТоварівТаПослуг_Objest.Склад = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                РеалізаціяТоварівТаПослуг_Objest.Контрагент = ЗначенняЗаЗамовчуванням.ОсновнийПокупець_Const;
                РеалізаціяТоварівТаПослуг_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
                РеалізаціяТоварівТаПослуг_Objest.БанківськийРахунокОрганізації = ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const;

                РеалізаціяТоварівТаПослуг_Objest.Курс = 1;
                РеалізаціяТоварівТаПослуг_Objest.Кратність = 1;
            }

            НомерДок.Text = РеалізаціяТоварівТаПослуг_Objest.НомерДок;
            ДатаДок.Value = РеалізаціяТоварівТаПослуг_Objest.ДатаДок;
            Організація.Pointer = РеалізаціяТоварівТаПослуг_Objest.Організація;
            Валюта.Pointer = РеалізаціяТоварівТаПослуг_Objest.Валюта;
            Каса.Pointer = РеалізаціяТоварівТаПослуг_Objest.Каса;
            Склад.Pointer = РеалізаціяТоварівТаПослуг_Objest.Склад;
            Контрагент.Pointer = РеалізаціяТоварівТаПослуг_Objest.Контрагент;
            Договір.Pointer = РеалізаціяТоварівТаПослуг_Objest.Договір;
            ГосподарськаОперація.ActiveId = ((Перелічення.ГосподарськіОперації)РеалізаціяТоварівТаПослуг_Objest.ГосподарськаОперація).ToString();
            ФормаОплати.ActiveId = ((Перелічення.ФормаОплати)РеалізаціяТоварівТаПослуг_Objest.ФормаОплати).ToString();
            Коментар.Text = РеалізаціяТоварівТаПослуг_Objest.Коментар;
            Підрозділ.Pointer = РеалізаціяТоварівТаПослуг_Objest.Підрозділ;
            ДатаОплати.Value = РеалізаціяТоварівТаПослуг_Objest.ДатаОплати;
            БанківськийрахунокОрганізації.Pointer = РеалізаціяТоварівТаПослуг_Objest.БанківськийРахунокОрганізації;
            БанківськийрахунокКонтрагента.Pointer = РеалізаціяТоварівТаПослуг_Objest.БанківськийРахунокКонтрагента;
            Автор.Pointer = РеалізаціяТоварівТаПослуг_Objest.Автор;
            ПовернутиТару.Active = РеалізаціяТоварівТаПослуг_Objest.ПовернутиТару;
            ДатаПоверненняТари.Value = РеалізаціяТоварівТаПослуг_Objest.ДатаПоверненняТари;
            СпосібДоставки.ActiveId = ((Перелічення.СпособиДоставки)РеалізаціяТоварівТаПослуг_Objest.СпосібДоставки).ToString();
            Курс.Value = РеалізаціяТоварівТаПослуг_Objest.Курс;
            Кратність.Value = РеалізаціяТоварівТаПослуг_Objest.Кратність;
            ЧасДоставкиЗ.Value = РеалізаціяТоварівТаПослуг_Objest.ЧасДоставкиЗ;
            ЧасДоставкиДо.Value = РеалізаціяТоварівТаПослуг_Objest.ЧасДоставкиДо;
            Менеджер.Pointer = РеалізаціяТоварівТаПослуг_Objest.Менеджер;
            Основа.Pointer = РеалізаціяТоварівТаПослуг_Objest.Основа;

            //Таблична частина
            Товари.РеалізаціяТоварівТаПослуг_Objest = РеалізаціяТоварівТаПослуг_Objest;
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
            РеалізаціяТоварівТаПослуг_Objest.НомерДок = НомерДок.Text;
            РеалізаціяТоварівТаПослуг_Objest.ДатаДок = ДатаДок.Value;
            РеалізаціяТоварівТаПослуг_Objest.Організація = Організація.Pointer;
            РеалізаціяТоварівТаПослуг_Objest.Валюта = Валюта.Pointer;
            РеалізаціяТоварівТаПослуг_Objest.Каса = Каса.Pointer;
            РеалізаціяТоварівТаПослуг_Objest.Склад = Склад.Pointer;
            РеалізаціяТоварівТаПослуг_Objest.Контрагент = Контрагент.Pointer;
            РеалізаціяТоварівТаПослуг_Objest.Договір = Договір.Pointer;
            РеалізаціяТоварівТаПослуг_Objest.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            РеалізаціяТоварівТаПослуг_Objest.ФормаОплати = Enum.Parse<Перелічення.ФормаОплати>(ФормаОплати.ActiveId);
            РеалізаціяТоварівТаПослуг_Objest.Коментар = Коментар.Text;
            РеалізаціяТоварівТаПослуг_Objest.Підрозділ = Підрозділ.Pointer;
            РеалізаціяТоварівТаПослуг_Objest.ДатаОплати = ДатаОплати.Value;
            РеалізаціяТоварівТаПослуг_Objest.БанківськийРахунокОрганізації = БанківськийрахунокОрганізації.Pointer;
            РеалізаціяТоварівТаПослуг_Objest.БанківськийРахунокКонтрагента = БанківськийрахунокКонтрагента.Pointer;
            РеалізаціяТоварівТаПослуг_Objest.Автор = Автор.Pointer;
            РеалізаціяТоварівТаПослуг_Objest.ПовернутиТару = ПовернутиТару.Active;
            РеалізаціяТоварівТаПослуг_Objest.ДатаПоверненняТари = ДатаПоверненняТари.Value;
            РеалізаціяТоварівТаПослуг_Objest.СпосібДоставки = Enum.Parse<Перелічення.СпособиДоставки>(СпосібДоставки.ActiveId);
            РеалізаціяТоварівТаПослуг_Objest.Курс = Курс.Value;
            РеалізаціяТоварівТаПослуг_Objest.Кратність = Кратність.Value;
            РеалізаціяТоварівТаПослуг_Objest.ЧасДоставкиЗ = ЧасДоставкиЗ.Value;
            РеалізаціяТоварівТаПослуг_Objest.ЧасДоставкиДо = ЧасДоставкиДо.Value;
            РеалізаціяТоварівТаПослуг_Objest.Менеджер = Менеджер.Pointer;
            РеалізаціяТоварівТаПослуг_Objest.Основа = Основа.Pointer;

            РеалізаціяТоварівТаПослуг_Objest.СумаДокументу = Товари.СумаДокументу();
            РеалізаціяТоварівТаПослуг_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
        }

        #endregion

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Валюта.Pointer.Назва} {Каса.Pointer.Назва} {Склад.Pointer.Назва} {Контрагент.Pointer.Назва} ";
        }

        bool IsValidValue()
        {
            if (!ДатаДок.IsValidValue())
            {
                Message.Error(Program.GeneralForm, "Перевірте правельність заповнення полів типу Дата та Число");
                return false;
            }
            else return true;
        }

        void Save()
        {
            GetValue();

            РеалізаціяТоварівТаПослуг_Objest.Save();
            Товари.SaveRecords();

            Program.GeneralForm?.RenameCurrentPageNotebook($"{РеалізаціяТоварівТаПослуг_Objest.Назва}");
        }

        void SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                try
                {
                    if (!РеалізаціяТоварівТаПослуг_Objest.SpendTheDocument(РеалізаціяТоварівТаПослуг_Objest.ДатаДок))
                    {
                        РеалізаціяТоварівТаПослуг_Objest.ClearSpendTheDocument();
                        ФункціїДляПовідомлень.ВідкритиТермінал();
                    }
                }
                catch (Exception exp)
                {
                    РеалізаціяТоварівТаПослуг_Objest.ClearSpendTheDocument();
                    Message.Error(Program.GeneralForm, exp.Message);
                    return;
                }
            }
            else
                РеалізаціяТоварівТаПослуг_Objest.ClearSpendTheDocument();
        }

        void ReloadList()
        {
            Товари.LoadRecords();

            if (PageList != null)
            {
                PageList.SelectPointerItem = РеалізаціяТоварівТаПослуг_Objest.GetDocumentPointer();
                PageList.LoadRecords();
            }
        }

        void OnSaveAndSpendClick(object? sender, EventArgs args)
        {
            if (!IsValidValue())
                return;

            Save();
            SpendTheDocument(true);

            ReloadList();

            if (РеалізаціяТоварівТаПослуг_Objest.Spend)
                Program.GeneralForm?.CloseCurrentPageNotebook();
        }

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (!IsValidValue())
                return;

            Save();
            SpendTheDocument(false);

            ReloadList();
        }
    }
}