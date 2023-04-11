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
    class ПоверненняТоварівПостачальнику_Елемент : VBox
    {
        public ПоверненняТоварівПостачальнику? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public ПоверненняТоварівПостачальнику_Objest ПоверненняТоварівПостачальнику_Objest { get; set; } = new ПоверненняТоварівПостачальнику_Objest();

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
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ" };
        БанківськіРахункиОрганізацій_PointerControl БанківськийРахунокОрганізації = new БанківськіРахункиОрганізацій_PointerControl() { WidthPresentation = 200 };
        БанківськіРахункиКонтрагентів_PointerControl БанківськийРахунокКонтрагента = new БанківськіРахункиКонтрагентів_PointerControl() { Caption = "Рахунок контрагента", WidthPresentation = 200 };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        ComboBoxText СпосібДоставки = new ComboBoxText();
        TimeControl ЧасДоставкиЗ = new TimeControl();
        TimeControl ЧасДоставкиДо = new TimeControl();
        Користувачі_PointerControl Менеджер = new Користувачі_PointerControl() { Caption = "Менеджер" };
        Entry Коментар = new Entry() { WidthRequest = 920 };
        Basis_PointerControl Основа = new Basis_PointerControl();

        ПоверненняТоварівПостачальнику_ТабличнаЧастина_Товари Товари = new ПоверненняТоварівПостачальнику_ТабличнаЧастина_Товари();

        #endregion

        public ПоверненняТоварівПостачальнику_Елемент() : base()
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
                    page.CreateReport(ПоверненняТоварівПостачальнику_Objest.GetDocumentPointer());
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
                    Перелічення.ГосподарськіОперації.ПоверненняТоварівПостачальнику.ToString(),
                    Конфігурація_ГосподарськіОперації.Fields["ПоверненняТоварівПостачальнику"].Desc);

                ГосподарськаОперація.Active = 0;

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

            hBoxNumberDataDoc.PackStart(new Label("Повернення товарів постачальнику №:"), false, false, 5);
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
        }

        void CreateContainer2(VBox vBox)
        {
            //Склад
            HBox hBoxSklad = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSklad, false, false, 5);

            hBoxSklad.PackStart(Склад, false, false, 5);

            //Каса
            HBox hBoxKasa = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxKasa, false, false, 5);

            hBoxKasa.PackStart(Каса, false, false, 5);

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

            //БанківськийрахунокКонтрагента
            HBox hBoxBankRahunokKontragenta = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxBankRahunokKontragenta, false, false, 5);

            hBoxBankRahunokKontragenta.PackStart(БанківськийРахунокКонтрагента, false, false, 5);

            //БанківськийрахунокОрганізації
            HBox hBoxBankRahunokOrganization = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxBankRahunokOrganization, false, false, 5);

            hBoxBankRahunokOrganization.PackStart(БанківськийРахунокОрганізації, false, false, 5);

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
            //ГосподарськаОперація
            HBox hBoxOperation = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxOperation, false, false, 5);

            hBoxOperation.PackStart(new Label("Господарська операція: "), false, false, 0);
            hBoxOperation.PackStart(ГосподарськаОперація, false, false, 5);

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
                ПоверненняТоварівПостачальнику_Objest.New();
                ПоверненняТоварівПостачальнику_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                ПоверненняТоварівПостачальнику_Objest.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
                ПоверненняТоварівПостачальнику_Objest.Каса = ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const;
                ПоверненняТоварівПостачальнику_Objest.Склад = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                ПоверненняТоварівПостачальнику_Objest.Контрагент = ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
                ПоверненняТоварівПостачальнику_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
                ПоверненняТоварівПостачальнику_Objest.БанківськийРахунокОрганізації = ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const;
            }

            НомерДок.Text = ПоверненняТоварівПостачальнику_Objest.НомерДок;
            ДатаДок.Value = ПоверненняТоварівПостачальнику_Objest.ДатаДок;
            Організація.Pointer = ПоверненняТоварівПостачальнику_Objest.Організація;
            Валюта.Pointer = ПоверненняТоварівПостачальнику_Objest.Валюта;
            Каса.Pointer = ПоверненняТоварівПостачальнику_Objest.Каса;
            Склад.Pointer = ПоверненняТоварівПостачальнику_Objest.Склад;
            Контрагент.Pointer = ПоверненняТоварівПостачальнику_Objest.Контрагент;
            Договір.Pointer = ПоверненняТоварівПостачальнику_Objest.Договір;
            ГосподарськаОперація.ActiveId = ((Перелічення.ГосподарськіОперації)ПоверненняТоварівПостачальнику_Objest.ГосподарськаОперація).ToString();
            Коментар.Text = ПоверненняТоварівПостачальнику_Objest.Коментар;
            Підрозділ.Pointer = ПоверненняТоварівПостачальнику_Objest.Підрозділ;
            БанківськийРахунокОрганізації.Pointer = ПоверненняТоварівПостачальнику_Objest.БанківськийРахунокОрганізації;
            БанківськийРахунокКонтрагента.Pointer = ПоверненняТоварівПостачальнику_Objest.БанківськийРахунокКонтрагента;
            Автор.Pointer = ПоверненняТоварівПостачальнику_Objest.Автор;
            СпосібДоставки.ActiveId = ((Перелічення.СпособиДоставки)ПоверненняТоварівПостачальнику_Objest.СпосібДоставки).ToString();
            ЧасДоставкиЗ.Value = ПоверненняТоварівПостачальнику_Objest.ЧасДоставкиЗ;
            ЧасДоставкиДо.Value = ПоверненняТоварівПостачальнику_Objest.ЧасДоставкиДо;
            Менеджер.Pointer = ПоверненняТоварівПостачальнику_Objest.Менеджер;
            Основа.Pointer = ПоверненняТоварівПостачальнику_Objest.Основа;

            //Таблична частина
            Товари.ПоверненняТоварівПостачальнику_Objest = ПоверненняТоварівПостачальнику_Objest;
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
            ПоверненняТоварівПостачальнику_Objest.НомерДок = НомерДок.Text;
            ПоверненняТоварівПостачальнику_Objest.ДатаДок = ДатаДок.Value;
            ПоверненняТоварівПостачальнику_Objest.Організація = Організація.Pointer;
            ПоверненняТоварівПостачальнику_Objest.Валюта = Валюта.Pointer;
            ПоверненняТоварівПостачальнику_Objest.Каса = Каса.Pointer;
            ПоверненняТоварівПостачальнику_Objest.Склад = Склад.Pointer;
            ПоверненняТоварівПостачальнику_Objest.Контрагент = Контрагент.Pointer;
            ПоверненняТоварівПостачальнику_Objest.Договір = Договір.Pointer;
            ПоверненняТоварівПостачальнику_Objest.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            ПоверненняТоварівПостачальнику_Objest.Коментар = Коментар.Text;
            ПоверненняТоварівПостачальнику_Objest.Підрозділ = Підрозділ.Pointer;
            ПоверненняТоварівПостачальнику_Objest.БанківськийРахунокОрганізації = БанківськийРахунокОрганізації.Pointer;
            ПоверненняТоварівПостачальнику_Objest.БанківськийРахунокКонтрагента = БанківськийРахунокКонтрагента.Pointer;
            ПоверненняТоварівПостачальнику_Objest.Автор = Автор.Pointer;
            ПоверненняТоварівПостачальнику_Objest.СпосібДоставки = Enum.Parse<Перелічення.СпособиДоставки>(СпосібДоставки.ActiveId);
            ПоверненняТоварівПостачальнику_Objest.ЧасДоставкиЗ = ЧасДоставкиЗ.Value;
            ПоверненняТоварівПостачальнику_Objest.ЧасДоставкиДо = ЧасДоставкиДо.Value;
            ПоверненняТоварівПостачальнику_Objest.Менеджер = Менеджер.Pointer;
            ПоверненняТоварівПостачальнику_Objest.Основа = Основа.Pointer;

            ПоверненняТоварівПостачальнику_Objest.СумаДокументу = Товари.СумаДокументу();
            ПоверненняТоварівПостачальнику_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
        }

        #endregion

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Валюта.Pointer.Назва} {Каса.Pointer.Назва} {Склад.Pointer.Назва} {Контрагент.Pointer.Назва}";
        }

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
            GetValue();

            ПоверненняТоварівПостачальнику_Objest.Save();
            Товари.SaveRecords();

            Program.GeneralForm?.RenameCurrentPageNotebook($"{ПоверненняТоварівПостачальнику_Objest.Назва}");
        }

        void SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                if (!ПоверненняТоварівПостачальнику_Objest.SpendTheDocument(ПоверненняТоварівПостачальнику_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ВідкритиТермінал();
            }
            else
                ПоверненняТоварівПостачальнику_Objest.ClearSpendTheDocument();
        }

        void ReloadList()
        {
            Товари.LoadRecords();

            if (PageList != null)
            {
                PageList.SelectPointerItem = ПоверненняТоварівПостачальнику_Objest.GetDocumentPointer();
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

            if (ПоверненняТоварівПостачальнику_Objest.Spend)
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