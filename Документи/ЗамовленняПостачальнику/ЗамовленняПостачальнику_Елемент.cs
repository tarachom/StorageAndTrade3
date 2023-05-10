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
    class ЗамовленняПостачальнику_Елемент : VBox
    {
        public ЗамовленняПостачальнику? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public ЗамовленняПостачальнику_Objest ЗамовленняПостачальнику_Objest { get; set; } = new ЗамовленняПостачальнику_Objest();

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
        DateTimeControl ДатаПоступлення = new DateTimeControl() { OnlyDate = true };
        БанківськіРахункиОрганізацій_PointerControl БанківськийРахунок = new БанківськіРахункиОрганізацій_PointerControl() { WidthPresentation = 200 };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        CheckButton ПовернутиТару = new CheckButton("Повернути тару:");
        ComboBoxText СпосібДоставки = new ComboBoxText();
        TimeControl ЧасДоставкиЗ = new TimeControl();
        TimeControl ЧасДоставкиДо = new TimeControl();
        Користувачі_PointerControl Менеджер = new Користувачі_PointerControl() { Caption = "Менеджер:" };
        Entry Коментар = new Entry() { WidthRequest = 920 };
        CompositePointerControl Основа = new CompositePointerControl();
        ЗамовленняПостачальнику_ТабличнаЧастина_Товари Товари = new ЗамовленняПостачальнику_ТабличнаЧастина_Товари();

        #endregion

        public ЗамовленняПостачальнику_Елемент() : base()
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
                    page.CreateReport(ЗамовленняПостачальнику_Objest.GetDocumentPointer());
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
                    Перелічення.ГосподарськіОперації.ПлануванняПоЗамовленнямПостачальнику.ToString(),
                    Конфігурація_ГосподарськіОперації.Fields["ПлануванняПоЗамовленнямПостачальнику"].Desc);

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

            hBoxNumberDataDoc.PackStart(new Label($"{ЗамовленняПостачальнику_Const.FULLNAME} №:"), false, false, 5);
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
            //ГосподарськаОперація
            HBox hBoxOperation = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxOperation, false, false, 5);

            hBoxOperation.PackStart(new Label("Господарська операція: "), false, false, 0);
            hBoxOperation.PackStart(ГосподарськаОперація, false, false, 5);

            //Підрозділ
            HBox hBoxPidrozdil = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxPidrozdil, false, false, 5);

            hBoxPidrozdil.PackStart(Підрозділ, false, false, 5);

            //БанківськийРахунок
            HBox hBoxBankRahunokOrganization = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxBankRahunokOrganization, false, false, 5);

            hBoxBankRahunokOrganization.PackStart(БанківськийРахунок, false, false, 5);

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

            //ВернутиТару
            HBox hBoxVernutyTaru = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxVernutyTaru, false, false, 5);

            hBoxVernutyTaru.PackStart(ПовернутиТару, false, false, 5);

            //ДатаПоступлення
            HBox hBoxDataOplaty = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxDataOplaty, false, false, 5);

            hBoxDataOplaty.PackStart(new Label("Дата поступлення:"), false, false, 5);
            hBoxDataOplaty.PackStart(ДатаПоступлення, false, false, 5);

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
                ЗамовленняПостачальнику_Objest.New();
                ЗамовленняПостачальнику_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                ЗамовленняПостачальнику_Objest.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
                ЗамовленняПостачальнику_Objest.Каса = ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const;
                ЗамовленняПостачальнику_Objest.Склад = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                ЗамовленняПостачальнику_Objest.Контрагент = ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
                ЗамовленняПостачальнику_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
            }

            НомерДок.Text = ЗамовленняПостачальнику_Objest.НомерДок;
            ДатаДок.Value = ЗамовленняПостачальнику_Objest.ДатаДок;
            Організація.Pointer = ЗамовленняПостачальнику_Objest.Організація;
            Валюта.Pointer = ЗамовленняПостачальнику_Objest.Валюта;
            Каса.Pointer = ЗамовленняПостачальнику_Objest.Каса;
            Склад.Pointer = ЗамовленняПостачальнику_Objest.Склад;
            Контрагент.Pointer = ЗамовленняПостачальнику_Objest.Контрагент;
            Договір.Pointer = ЗамовленняПостачальнику_Objest.Договір;
            ГосподарськаОперація.ActiveId = ((Перелічення.ГосподарськіОперації)ЗамовленняПостачальнику_Objest.ГосподарськаОперація).ToString();
            ФормаОплати.ActiveId = ((Перелічення.ФормаОплати)ЗамовленняПостачальнику_Objest.ФормаОплати).ToString();
            Коментар.Text = ЗамовленняПостачальнику_Objest.Коментар;
            Підрозділ.Pointer = ЗамовленняПостачальнику_Objest.Підрозділ;
            ДатаПоступлення.Value = ЗамовленняПостачальнику_Objest.ДатаПоступлення;
            БанківськийРахунок.Pointer = ЗамовленняПостачальнику_Objest.БанківськийРахунок;
            Автор.Pointer = ЗамовленняПостачальнику_Objest.Автор;
            ПовернутиТару.Active = ЗамовленняПостачальнику_Objest.ПовернутиТару;
            СпосібДоставки.ActiveId = ((Перелічення.СпособиДоставки)ЗамовленняПостачальнику_Objest.СпосібДоставки).ToString();
            ЧасДоставкиЗ.Value = ЗамовленняПостачальнику_Objest.ЧасДоставкиЗ;
            ЧасДоставкиДо.Value = ЗамовленняПостачальнику_Objest.ЧасДоставкиДо;
            Менеджер.Pointer = ЗамовленняПостачальнику_Objest.Менеджер;
            Основа.Pointer = ЗамовленняПостачальнику_Objest.Основа;

            //Таблична частина
            Товари.ЗамовленняПостачальнику_Objest = ЗамовленняПостачальнику_Objest;
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
            ЗамовленняПостачальнику_Objest.НомерДок = НомерДок.Text;
            ЗамовленняПостачальнику_Objest.ДатаДок = ДатаДок.Value;
            ЗамовленняПостачальнику_Objest.Організація = Організація.Pointer;
            ЗамовленняПостачальнику_Objest.Валюта = Валюта.Pointer;
            ЗамовленняПостачальнику_Objest.Каса = Каса.Pointer;
            ЗамовленняПостачальнику_Objest.Склад = Склад.Pointer;
            ЗамовленняПостачальнику_Objest.Контрагент = Контрагент.Pointer;
            ЗамовленняПостачальнику_Objest.Договір = Договір.Pointer;
            ЗамовленняПостачальнику_Objest.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            ЗамовленняПостачальнику_Objest.ФормаОплати = Enum.Parse<Перелічення.ФормаОплати>(ФормаОплати.ActiveId);
            ЗамовленняПостачальнику_Objest.Коментар = Коментар.Text;
            ЗамовленняПостачальнику_Objest.Підрозділ = Підрозділ.Pointer;
            ЗамовленняПостачальнику_Objest.ДатаПоступлення = ДатаПоступлення.Value;
            ЗамовленняПостачальнику_Objest.БанківськийРахунок = БанківськийРахунок.Pointer;
            ЗамовленняПостачальнику_Objest.Автор = Автор.Pointer;
            ЗамовленняПостачальнику_Objest.ПовернутиТару = ПовернутиТару.Active;
            ЗамовленняПостачальнику_Objest.СпосібДоставки = Enum.Parse<Перелічення.СпособиДоставки>(СпосібДоставки.ActiveId);
            ЗамовленняПостачальнику_Objest.ЧасДоставкиЗ = ЧасДоставкиЗ.Value;
            ЗамовленняПостачальнику_Objest.ЧасДоставкиДо = ЧасДоставкиДо.Value;
            ЗамовленняПостачальнику_Objest.Менеджер = Менеджер.Pointer;
            ЗамовленняПостачальнику_Objest.Основа = Основа.Pointer;

            ЗамовленняПостачальнику_Objest.СумаДокументу = Товари.СумаДокументу();
            ЗамовленняПостачальнику_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
        }

        #endregion

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Валюта.Pointer.Назва} {Каса.Pointer.Назва} {Контрагент.Pointer.Назва} ";
        }

        #region Save & Spend

        bool Save()
        {
            GetValue();

            bool isSave = false;

            try
            {
                isSave = ЗамовленняПостачальнику_Objest.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомленняПроПомилку(DateTime.Now, "Запис",
                    ЗамовленняПостачальнику_Objest.UnigueID.UGuid, "Документ", ЗамовленняПостачальнику_Objest.Назва, ex.Message);

                ФункціїДляПовідомлень.ВідкритиТермінал();
            }

            if (!isSave)
            {
                Message.Info(Program.GeneralForm, "Не вдалось записати документ");
                return false;
            }

            Товари.SaveRecords();

            Program.GeneralForm?.RenameCurrentPageNotebook($"{ЗамовленняПостачальнику_Objest.Назва}");

            return true;
        }

        void SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                if (!ЗамовленняПостачальнику_Objest.SpendTheDocument(ЗамовленняПостачальнику_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ВідкритиТермінал();
            }
            else
                ЗамовленняПостачальнику_Objest.ClearSpendTheDocument();
        }

        void ReloadList()
        {
            if (PageList != null)
            {
                PageList.SelectPointerItem = ЗамовленняПостачальнику_Objest.UnigueID;
                PageList.LoadRecords();
            }
        }

        void OnSaveAndSpendClick(object? sender, EventArgs args)
        {
            //Зберегти
            bool isSave = Save();

            //Провести
            if (isSave)
                SpendTheDocument(true);

            //Закрити сторінку
            if (isSave && ЗамовленняПостачальнику_Objest.Spend)
                Program.GeneralForm?.CloseCurrentPageNotebook();

            ReloadList();
        }

        void OnSaveClick(object? sender, EventArgs args)
        {
            //Зберегти
            bool isSave = Save();

            //Очистити проводки
            if (isSave)
                SpendTheDocument(false);

            ReloadList();
        }

        #endregion
    }
}