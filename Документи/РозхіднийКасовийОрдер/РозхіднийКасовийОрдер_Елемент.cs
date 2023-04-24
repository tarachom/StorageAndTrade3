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
    class РозхіднийКасовийОрдер_Елемент : VBox
    {
        public РозхіднийКасовийОрдер? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public РозхіднийКасовийОрдер_Objest РозхіднийКасовийОрдер_Objest { get; set; } = new РозхіднийКасовийОрдер_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Організації_PointerControl ОрганізаціяОтримувач = new Організації_PointerControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();
        Каси_PointerControl Каса = new Каси_PointerControl();
        Каси_PointerControl КасаОтримувач = new Каси_PointerControl() { Caption = "Каса отримувач:" };
        NumericControl Курс = new NumericControl() { Caption = "Курс:" };
        NumericControl СумаДокументу = new NumericControl() { Caption = "Сума:" };
        Контрагенти_PointerControl Контрагент = new Контрагенти_PointerControl();
        ДоговориКонтрагентів_PointerControl Договір = new ДоговориКонтрагентів_PointerControl();
        ComboBoxText ГосподарськаОперація = new ComboBoxText();
        БанківськіРахункиОрганізацій_PointerControl БанківськийРахунок = new БанківськіРахункиОрганізацій_PointerControl() { Caption = "Банківський рахунок:" };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        СтаттяРухуКоштів_PointerControl СтаттяРухуКоштів = new СтаттяРухуКоштів_PointerControl();
        Entry Коментар = new Entry() { WidthRequest = 920 };
        Basis_PointerControl Основа = new Basis_PointerControl();

        #endregion

        public РозхіднийКасовийОрдер_Елемент() : base()
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
                    page.CreateReport(РозхіднийКасовийОрдер_Objest.GetDocumentPointer());
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

            //Валюта
            HBox hBoxValuta = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxValuta, false, false, 5);

            hBoxValuta.PackStart(Валюта, false, false, 5);
        }

        void CreateContainer2(VBox vBox)
        {
            //ГосподарськаОперація
            HBox hBoxOperation = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxOperation, false, false, 5);

            ГосподарськаОперація.Changed += OnComboBoxChanged_ГосподарськаОперація;

            hBoxOperation.PackStart(new Label("Господарська операція: "), false, false, 0);
            hBoxOperation.PackStart(ГосподарськаОперація, false, false, 5);

            //Каса
            HBox hBoxKasa = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxKasa, false, false, 5);

            hBoxKasa.PackStart(Каса, false, false, 5);

            //КасаОтримувач
            HBox hBoxKasaOtrymuvach = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxKasaOtrymuvach, false, false, 5);

            hBoxKasaOtrymuvach.PackStart(КасаОтримувач, false, false, 5);

            //БанківськийРахунок
            HBox hBoxBankRahunokOrganization = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxBankRahunokOrganization, false, false, 5);

            hBoxBankRahunokOrganization.PackStart(БанківськийРахунок, false, false, 5);

            //СумаДокументу та Курс
            HBox hBoxSuma = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSuma, false, false, 5);

            hBoxSuma.PackStart(СумаДокументу, false, false, 5);
            hBoxSuma.PackStart(Курс, false, false, 5);
        }

        void CreateContainer3(VBox vBox)
        {
            //Автор
            HBox hBoxAutor = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxAutor, false, false, 5);

            hBoxAutor.PackStart(Автор, false, false, 5);

            //Основа
            HBox hBoxBasis = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxBasis, false, false, 5);

            hBoxBasis.PackStart(Основа, false, false, 5);
        }

        void CreateContainer4(VBox vBox)
        {
            //СтаттяРухуКоштів
            HBox hBoxStatjaRuhuKoshtiv = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxStatjaRuhuKoshtiv, false, false, 5);

            hBoxStatjaRuhuKoshtiv.PackStart(СтаттяРухуКоштів, false, false, 5);
        }

        void CreatePack2(HPaned hPaned)
        {
            Notebook notebook = new Notebook() { Scrollable = true, EnablePopup = true, BorderWidth = 0, ShowBorder = false };
            notebook.TabPos = PositionType.Top;

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
                РозхіднийКасовийОрдер_Objest.New();
                РозхіднийКасовийОрдер_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                РозхіднийКасовийОрдер_Objest.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
                РозхіднийКасовийОрдер_Objest.Каса = ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const; ;
                РозхіднийКасовийОрдер_Objest.Контрагент = ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
                РозхіднийКасовийОрдер_Objest.БанківськийРахунок = ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const;
            }

            if (IsNew || (int)РозхіднийКасовийОрдер_Objest.ГосподарськаОперація == 0)
                РозхіднийКасовийОрдер_Objest.ГосподарськаОперація = Перелічення.ГосподарськіОперації.ОплатаПостачальнику;

            НомерДок.Text = РозхіднийКасовийОрдер_Objest.НомерДок;
            ДатаДок.Value = РозхіднийКасовийОрдер_Objest.ДатаДок;
            Організація.Pointer = РозхіднийКасовийОрдер_Objest.Організація;
            Валюта.Pointer = РозхіднийКасовийОрдер_Objest.Валюта;
            Каса.Pointer = РозхіднийКасовийОрдер_Objest.Каса;
            КасаОтримувач.Pointer = РозхіднийКасовийОрдер_Objest.КасаОтримувач;
            Контрагент.Pointer = РозхіднийКасовийОрдер_Objest.Контрагент;
            Договір.Pointer = РозхіднийКасовийОрдер_Objest.Договір;
            ГосподарськаОперація.ActiveId = РозхіднийКасовийОрдер_Objest.ГосподарськаОперація.ToString();
            Коментар.Text = РозхіднийКасовийОрдер_Objest.Коментар;
            БанківськийРахунок.Pointer = РозхіднийКасовийОрдер_Objest.БанківськийРахунок;
            Автор.Pointer = РозхіднийКасовийОрдер_Objest.Автор;
            СумаДокументу.Value = РозхіднийКасовийОрдер_Objest.СумаДокументу;
            Курс.Value = РозхіднийКасовийОрдер_Objest.Курс;
            СтаттяРухуКоштів.Pointer = РозхіднийКасовийОрдер_Objest.СтаттяРухуКоштів;
            Основа.Pointer = РозхіднийКасовийОрдер_Objest.Основа;

            if (IsNew)
            {
                //Основний договір
                if (Контрагент.AfterSelectFunc != null)
                    Контрагент.AfterSelectFunc.Invoke();
            }
        }

        void GetValue()
        {
            РозхіднийКасовийОрдер_Objest.НомерДок = НомерДок.Text;
            РозхіднийКасовийОрдер_Objest.ДатаДок = ДатаДок.Value;
            РозхіднийКасовийОрдер_Objest.Організація = Організація.Pointer;
            РозхіднийКасовийОрдер_Objest.Валюта = Валюта.Pointer;
            РозхіднийКасовийОрдер_Objest.Каса = Каса.Pointer;
            РозхіднийКасовийОрдер_Objest.КасаОтримувач = КасаОтримувач.Pointer;
            РозхіднийКасовийОрдер_Objest.Контрагент = Контрагент.Pointer;
            РозхіднийКасовийОрдер_Objest.Договір = Договір.Pointer;
            РозхіднийКасовийОрдер_Objest.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            РозхіднийКасовийОрдер_Objest.Коментар = Коментар.Text;
            РозхіднийКасовийОрдер_Objest.БанківськийРахунок = БанківськийРахунок.Pointer;
            РозхіднийКасовийОрдер_Objest.Автор = Автор.Pointer;
            РозхіднийКасовийОрдер_Objest.СумаДокументу = СумаДокументу.Value;
            РозхіднийКасовийОрдер_Objest.Курс = Курс.Value;
            РозхіднийКасовийОрдер_Objest.СтаттяРухуКоштів = СтаттяРухуКоштів.Pointer;
            РозхіднийКасовийОрдер_Objest.Основа = Основа.Pointer;

            РозхіднийКасовийОрдер_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку();
        }

        #endregion

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Валюта.Pointer.Назва} {Каса.Pointer.Назва} {КасаОтримувач.Pointer.Назва} {Контрагент.Pointer.Назва}";
        }

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

        #region Save & Spend

        bool Save()
        {
            GetValue();

            bool isSave = false;

            try
            {
                isSave = РозхіднийКасовийОрдер_Objest.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомленняПроПомилку(DateTime.Now, "Запис",
                    РозхіднийКасовийОрдер_Objest.UnigueID.UGuid, "Документ", РозхіднийКасовийОрдер_Objest.Назва, ex.Message);

                ФункціїДляПовідомлень.ВідкритиТермінал();
            }

            if (!isSave)
            {
                Message.Info(Program.GeneralForm, "Не вдалось записати документ");
                return false;
            }

            Program.GeneralForm?.RenameCurrentPageNotebook($"{РозхіднийКасовийОрдер_Objest.Назва}");

            return true;
        }

        void SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                if (!РозхіднийКасовийОрдер_Objest.SpendTheDocument(РозхіднийКасовийОрдер_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ВідкритиТермінал();
            }
            else
                РозхіднийКасовийОрдер_Objest.ClearSpendTheDocument();
        }

        void ReloadList()
        {
            if (PageList != null)
            {
                PageList.SelectPointerItem = РозхіднийКасовийОрдер_Objest.GetDocumentPointer();
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
            if (isSave && РозхіднийКасовийОрдер_Objest.Spend)
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