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
    class ЗамовленняКлієнта_Елемент : VBox
    {
        public ЗамовленняКлієнта? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public ЗамовленняКлієнта_Objest ЗамовленняКлієнта_Objest { get; set; } = new ЗамовленняКлієнта_Objest();

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

        ЗамовленняКлієнта_ТабличнаЧастина_Товари Товари = new ЗамовленняКлієнта_ТабличнаЧастина_Товари();

        #endregion

        public ЗамовленняКлієнта_Елемент() : base()
        {
            new VBox();
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
                    page.CreateReport(ЗамовленняКлієнта_Objest.GetDocumentPointer());
                    return page;
                });
            };

            hBox.PackStart(linkButtonProvodky, false, false, 10);

            PackStart(hBox, false, false, 10);

            HPaned hPaned = new HPaned() { Orientation = Orientation.Vertical, BorderWidth = 5 };

            FillComboBoxes();

            CreatePack1(hPaned);
            CreatePack2(hPaned);

            PackStart(hPaned, true, true, 0);

            ShowAll();
        }

        void FillComboBoxes()
        {
            if (Config.Kernel != null)
            {
                //1
                ConfigurationEnums Конфігурація_ГосподарськіОперації = Config.Kernel.Conf.Enums["ГосподарськіОперації"];

                ГосподарськаОперація.Append(
                    Перелічення.ГосподарськіОперації.ПлануванняПоЗамовленнямКлієнта.ToString(),
                    Конфігурація_ГосподарськіОперації.Fields["ПлануванняПоЗамовленнямКлієнта"].Desc);

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

            hBoxNumberDataDoc.PackStart(new Label("Замовлення клієнта №:"), false, false, 5);
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

            //Автор
            HBox hBoxAutor = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxAutor, false, false, 5);

            hBoxAutor.PackStart(Автор, false, false, 5);

            //Менеджер
            HBox hBoxMenedjer = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxMenedjer, false, false, 5);

            hBoxMenedjer.PackStart(Менеджер, false, false, 5);
        }

        void CreateContainer4(VBox vBox)
        {
            //ФормаОплати
            HBox hBoxFormaOplaty = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxFormaOplaty, false, false, 5);

            hBoxFormaOplaty.PackStart(new Label("Форма оплати: "), false, false, 0);
            hBoxFormaOplaty.PackStart(ФормаОплати, false, false, 5);

            //СпосібДоставки
            HBox hBoxSposibDostavky = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSposibDostavky, false, false, 5);

            hBoxSposibDostavky.PackStart(new Label("Спосіб доставки:"), false, false, 0);
            hBoxSposibDostavky.PackStart(СпосібДоставки, false, false, 5);

            //ДатаВідгрузки
            HBox hBoxDataOplaty = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxDataOplaty, false, false, 5);

            hBoxDataOplaty.PackStart(new Label("Дата відвантаження:"), false, false, 5);
            hBoxDataOplaty.PackStart(ДатаВідгрузки, false, false, 5);

            //ЧасДоставки
            HBox hBoxTchasDostavky = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxTchasDostavky, false, false, 5);

            hBoxTchasDostavky.PackStart(new Label("Час доставки з"), false, false, 5);
            hBoxTchasDostavky.PackStart(ЧасДоставкиЗ, false, false, 5);
            hBoxTchasDostavky.PackStart(new Label("до"), false, false, 5);
            hBoxTchasDostavky.PackStart(ЧасДоставкиДо, false, false, 5);

            //Узгоджений & ВернутиТару
            HBox hBoxUzgodjenyi = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxUzgodjenyi, false, false, 5);

            hBoxUzgodjenyi.PackStart(Узгоджений, false, false, 5);
            hBoxUzgodjenyi.PackStart(ПовернутиТару, false, false, 5);

            //ДатаПоверненняТари
            HBox hBoxDataPovernenjaTary = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxDataPovernenjaTary, false, false, 5);

            hBoxDataPovernenjaTary.PackStart(new Label("Дата повернення тари:"), false, false, 5);
            hBoxDataPovernenjaTary.PackStart(ДатаПоверненняТари, false, false, 5);
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
                ЗамовленняКлієнта_Objest.НомерДок = (++НумераціяДокументів.ЗамовленняКлієнта_Const).ToString("D8");
                ЗамовленняКлієнта_Objest.ДатаДок = DateTime.Now;
                ЗамовленняКлієнта_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                ЗамовленняКлієнта_Objest.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
                ЗамовленняКлієнта_Objest.Каса = ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const;
                ЗамовленняКлієнта_Objest.Склад = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                ЗамовленняКлієнта_Objest.Контрагент = ЗначенняЗаЗамовчуванням.ОсновнийПокупець_Const;
                ЗамовленняКлієнта_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
                ЗамовленняКлієнта_Objest.Автор = Program.Користувач;
                ЗамовленняКлієнта_Objest.Менеджер = Program.Користувач;
            }

            НомерДок.Text = ЗамовленняКлієнта_Objest.НомерДок;
            ДатаДок.Value = ЗамовленняКлієнта_Objest.ДатаДок;
            Організація.Pointer = ЗамовленняКлієнта_Objest.Організація;
            Валюта.Pointer = ЗамовленняКлієнта_Objest.Валюта;
            Каса.Pointer = ЗамовленняКлієнта_Objest.Каса;
            Склад.Pointer = ЗамовленняКлієнта_Objest.Склад;
            Контрагент.Pointer = ЗамовленняКлієнта_Objest.Контрагент;
            Договір.Pointer = ЗамовленняКлієнта_Objest.Договір;
            ГосподарськаОперація.ActiveId = ((Перелічення.ГосподарськіОперації)ЗамовленняКлієнта_Objest.ГосподарськаОперація).ToString();
            ФормаОплати.ActiveId = ((Перелічення.ФормаОплати)ЗамовленняКлієнта_Objest.ФормаОплати).ToString();
            Коментар.Text = ЗамовленняКлієнта_Objest.Коментар;
            Підрозділ.Pointer = ЗамовленняКлієнта_Objest.Підрозділ;
            ДатаВідгрузки.Value = ЗамовленняКлієнта_Objest.ДатаВідгрузки;
            Узгоджений.Active = ЗамовленняКлієнта_Objest.Узгоджений;
            БанківськийрахунокКонтрагента.Pointer = ЗамовленняКлієнта_Objest.БанківськийРахунокКонтрагента;
            Автор.Pointer = ЗамовленняКлієнта_Objest.Автор;
            ПовернутиТару.Active = ЗамовленняКлієнта_Objest.ПовернутиТару;
            ДатаПоверненняТари.Value = ЗамовленняКлієнта_Objest.ДатаПоверненняТари;
            СпосібДоставки.ActiveId = ((Перелічення.СпособиДоставки)ЗамовленняКлієнта_Objest.СпосібДоставки).ToString();
            ЧасДоставкиЗ.Value = ЗамовленняКлієнта_Objest.ЧасДоставкиЗ;
            ЧасДоставкиДо.Value = ЗамовленняКлієнта_Objest.ЧасДоставкиДо;
            Менеджер.Pointer = ЗамовленняКлієнта_Objest.Менеджер;

            //Таблична частина
            Товари.ЗамовленняКлієнта_Objest = ЗамовленняКлієнта_Objest;
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
            ЗамовленняКлієнта_Objest.НомерДок = НомерДок.Text;
            ЗамовленняКлієнта_Objest.ДатаДок = ДатаДок.Value;
            ЗамовленняКлієнта_Objest.Назва = $"Замовлення клієнта №{ЗамовленняКлієнта_Objest.НомерДок} від {ЗамовленняКлієнта_Objest.ДатаДок.ToShortDateString()}";
            ЗамовленняКлієнта_Objest.Організація = Організація.Pointer;
            ЗамовленняКлієнта_Objest.Валюта = Валюта.Pointer;
            ЗамовленняКлієнта_Objest.Каса = Каса.Pointer;
            ЗамовленняКлієнта_Objest.Склад = Склад.Pointer;
            ЗамовленняКлієнта_Objest.Контрагент = Контрагент.Pointer;
            ЗамовленняКлієнта_Objest.Договір = Договір.Pointer;
            ЗамовленняКлієнта_Objest.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            ЗамовленняКлієнта_Objest.ФормаОплати = Enum.Parse<Перелічення.ФормаОплати>(ФормаОплати.ActiveId);
            ЗамовленняКлієнта_Objest.Коментар = Коментар.Text;
            ЗамовленняКлієнта_Objest.Підрозділ = Підрозділ.Pointer;
            ЗамовленняКлієнта_Objest.ДатаВідгрузки = ДатаВідгрузки.Value;
            ЗамовленняКлієнта_Objest.Узгоджений = Узгоджений.Active;
            ЗамовленняКлієнта_Objest.БанківськийРахунокКонтрагента = БанківськийрахунокКонтрагента.Pointer;
            ЗамовленняКлієнта_Objest.Автор = Автор.Pointer;
            ЗамовленняКлієнта_Objest.ПовернутиТару = ПовернутиТару.Active;
            ЗамовленняКлієнта_Objest.ДатаПоверненняТари = ДатаПоверненняТари.Value;
            ЗамовленняКлієнта_Objest.СпосібДоставки = Enum.Parse<Перелічення.СпособиДоставки>(СпосібДоставки.ActiveId);
            ЗамовленняКлієнта_Objest.ЧасДоставкиЗ = ЧасДоставкиЗ.Value;
            ЗамовленняКлієнта_Objest.ЧасДоставкиДо = ЧасДоставкиДо.Value;
            ЗамовленняКлієнта_Objest.Менеджер = Менеджер.Pointer;

            ЗамовленняКлієнта_Objest.СумаДокументу = Товари.СумаДокументу();
        }

        #endregion

        bool IsValidValue()
        {
            if (!ДатаДок.IsValidValue())
            {
                Message.Error(Program.GeneralForm, "Перевірте правельність заповнення полів");
                return false;
            }
            else return true;
        }

        void Save()
        {
            if (IsNew)
            {
                ЗамовленняКлієнта_Objest.New();
                IsNew = false;
            }

            GetValue();

            ЗамовленняКлієнта_Objest.Save();
            Товари.SaveRecords();

            Program.GeneralForm?.RenameCurrentPageNotebook($"{ЗамовленняКлієнта_Objest.Назва}");
        }

        void SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                try
                {
                    if (!ЗамовленняКлієнта_Objest.SpendTheDocument(ЗамовленняКлієнта_Objest.ДатаДок))
                    {
                        ЗамовленняКлієнта_Objest.ClearSpendTheDocument();
                        ФункціїДляПовідомлень.ВідкритиТермінал();
                    }
                }
                catch (Exception exp)
                {
                    ЗамовленняКлієнта_Objest.ClearSpendTheDocument();
                    Message.Error(Program.GeneralForm, exp.Message);
                    return;
                }
            }
            else
                ЗамовленняКлієнта_Objest.ClearSpendTheDocument();
        }

        void ReloadList()
        {
            Товари.LoadRecords();

            if (PageList != null)
            {
                PageList.SelectPointerItem = ЗамовленняКлієнта_Objest.GetDocumentPointer();
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

            if (ЗамовленняКлієнта_Objest.Spend)
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