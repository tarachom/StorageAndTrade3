#region Info

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

#endregion

using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class ПрихіднийКасовийОрдер_Елемент : VBox
    {
        public ПрихіднийКасовийОрдер? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public ПрихіднийКасовийОрдер_Objest ПрихіднийКасовийОрдер_Objest { get; set; } = new ПрихіднийКасовийОрдер_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();
        Каси_PointerControl Каса = new Каси_PointerControl();
        Каси_PointerControl КасаВідправник = new Каси_PointerControl(){Caption = "Каса відправник:"};
        NumericControl Курс = new NumericControl();
        NumericControl СумаДокументу = new NumericControl();
        Контрагенти_PointerControl Контрагент = new Контрагенти_PointerControl();
        ДоговориКонтрагентів_PointerControl Договір = new ДоговориКонтрагентів_PointerControl();
        ComboBoxText ГосподарськаОперація = new ComboBoxText();
        БанківськіРахункиОрганізацій_PointerControl БанківськийРахунок = new БанківськіРахункиОрганізацій_PointerControl() { WidthPresentation = 200 };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        СтаттяРухуКоштів_PointerControl СтаттяРухуКоштів = new СтаттяРухуКоштів_PointerControl();
        Entry Коментар = new Entry() { WidthRequest = 920 };

        #endregion

        public ПрихіднийКасовийОрдер_Елемент() : base()
        {
            new VBox();
            HBox hBox = new HBox();

            Button bSaveAndSpend = new Button("Зберегти і провести");
            bSaveAndSpend.Clicked += OnSaveAndSpendClick;

            hBox.PackStart(bSaveAndSpend, false, false, 10);

            Button bSave = new Button("Зберегти");
            bSave.Clicked += OnSaveClick;

            hBox.PackStart(bSave, false, false, 10);

            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

            hBox.PackStart(bClose, false, false, 10);

            //Проводки
            LinkButton linkButtonProvodky = new LinkButton("Проводки") { Halign = Align.Start };
            linkButtonProvodky.Clicked += (object? sender, EventArgs args) =>
            {
                Program.GeneralForm?.CreateNotebookPage($"Проводки", () =>
                {
                    Звіт_РухДокументівПоРегістрах page = new Звіт_РухДокументівПоРегістрах();
                    page.CreateReport(ПрихіднийКасовийОрдер_Objest.GetDocumentPointer());
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
                    Перелічення.ГосподарськіОперації.ПоступленняОплатиВідКлієнта.ToString(),
                    Конфігурація_ГосподарськіОперації.Fields["ПоступленняОплатиВідКлієнта"].Desc);

                ГосподарськаОперація.Append(
                    Перелічення.ГосподарськіОперації.ПоступленняКоштівЗІншоїКаси.ToString(),
                    Конфігурація_ГосподарськіОперації.Fields["ПоступленняКоштівЗІншоїКаси"].Desc);

                ГосподарськаОперація.Append(
                    Перелічення.ГосподарськіОперації.ПоступленняКоштівЗБанку.ToString(),
                    Конфігурація_ГосподарськіОперації.Fields["ПоступленняКоштівЗБанку"].Desc);

                ГосподарськаОперація.Append(
                    Перелічення.ГосподарськіОперації.ПоверненняКоштівПостачальнику.ToString(),
                    Конфігурація_ГосподарськіОперації.Fields["ПоверненняКоштівПостачальнику"].Desc);

                ГосподарськаОперація.Append(
                    Перелічення.ГосподарськіОперації.ІншіДоходи.ToString(),
                    Конфігурація_ГосподарськіОперації.Fields["ІншіДоходи"].Desc);

                ГосподарськаОперація.Active = 0;
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

            hBoxOperation.PackStart(new Label("Господарська операція: "), false, false, 0);
            hBoxOperation.PackStart(ГосподарськаОперація, false, false, 5);

            //Каса
            HBox hBoxKasa = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxKasa, false, false, 5);

            hBoxKasa.PackStart(Каса, false, false, 5);

            //КасаВідправник
            HBox hBoxKasaVidpravnyk = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxKasaVidpravnyk, false, false, 5);

            hBoxKasaVidpravnyk.PackStart(КасаВідправник, false, false, 5);

            //СумаДокументу & Курс
            HBox hBoxSuma = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSuma, false, false, 5);

            hBoxSuma.PackStart(new Label("Сума:"), false, false, 5);
            hBoxSuma.PackStart(СумаДокументу, false, false, 5);

            hBoxSuma.PackStart(new Label("Курс:"), false, false, 5);
            hBoxSuma.PackStart(Курс, false, false, 5);
        }

        void CreateContainer3(VBox vBox)
        {
            //Автор
            HBox hBoxAutor = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxAutor, false, false, 5);

            hBoxAutor.PackStart(Автор, false, false, 5);
        }

        void CreateContainer4(VBox vBox)
        {
            //БанківськийРахунок
            HBox hBoxBankRahunokOrganization = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxBankRahunokOrganization, false, false, 5);

            hBoxBankRahunokOrganization.PackStart(БанківськийРахунок, false, false, 5);            

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
                ПрихіднийКасовийОрдер_Objest.НомерДок = (++НумераціяДокументів.ПрихіднийКасовийОрдер_Const).ToString("D8");
                ПрихіднийКасовийОрдер_Objest.ДатаДок = DateTime.Now;
                ПрихіднийКасовийОрдер_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                ПрихіднийКасовийОрдер_Objest.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
                ПрихіднийКасовийОрдер_Objest.Каса = ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const; ;
                ПрихіднийКасовийОрдер_Objest.Контрагент = ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
                ПрихіднийКасовийОрдер_Objest.БанківськийРахунок = ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const;
            }

            НомерДок.Text = ПрихіднийКасовийОрдер_Objest.НомерДок;
            ДатаДок.Value = ПрихіднийКасовийОрдер_Objest.ДатаДок;
            Організація.Pointer = ПрихіднийКасовийОрдер_Objest.Організація;
            Валюта.Pointer = ПрихіднийКасовийОрдер_Objest.Валюта;
            Каса.Pointer = ПрихіднийКасовийОрдер_Objest.Каса;
            КасаВідправник.Pointer = ПрихіднийКасовийОрдер_Objest.КасаВідправник;
            Контрагент.Pointer = ПрихіднийКасовийОрдер_Objest.Контрагент;
            Договір.Pointer = ПрихіднийКасовийОрдер_Objest.Договір;
            ГосподарськаОперація.ActiveId = ((Перелічення.ГосподарськіОперації)ПрихіднийКасовийОрдер_Objest.ГосподарськаОперація).ToString();
            Коментар.Text = ПрихіднийКасовийОрдер_Objest.Коментар;
            БанківськийРахунок.Pointer = ПрихіднийКасовийОрдер_Objest.БанківськийРахунок;
            Автор.Pointer = ПрихіднийКасовийОрдер_Objest.Автор;
            СумаДокументу.Value = ПрихіднийКасовийОрдер_Objest.СумаДокументу;
            Курс.Value = ПрихіднийКасовийОрдер_Objest.Курс;
            СтаттяРухуКоштів.Pointer = ПрихіднийКасовийОрдер_Objest.СтаттяРухуКоштів;

            if (IsNew)
            {
                //Основний договір
                if (Контрагент.AfterSelectFunc != null)
                    Контрагент.AfterSelectFunc.Invoke();
            }
        }

        void GetValue()
        {
            ПрихіднийКасовийОрдер_Objest.НомерДок = НомерДок.Text;
            ПрихіднийКасовийОрдер_Objest.ДатаДок = ДатаДок.Value;
            ПрихіднийКасовийОрдер_Objest.Назва = $"Прихідний касовий ордер №{ПрихіднийКасовийОрдер_Objest.НомерДок} від {ПрихіднийКасовийОрдер_Objest.ДатаДок.ToShortDateString()}";
            ПрихіднийКасовийОрдер_Objest.Організація = Організація.Pointer;
            ПрихіднийКасовийОрдер_Objest.Валюта = Валюта.Pointer;
            ПрихіднийКасовийОрдер_Objest.Каса = Каса.Pointer;
            ПрихіднийКасовийОрдер_Objest.КасаВідправник = КасаВідправник.Pointer;
            ПрихіднийКасовийОрдер_Objest.Контрагент = Контрагент.Pointer;
            ПрихіднийКасовийОрдер_Objest.Договір = Договір.Pointer;
            ПрихіднийКасовийОрдер_Objest.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            ПрихіднийКасовийОрдер_Objest.Коментар = Коментар.Text;
            ПрихіднийКасовийОрдер_Objest.БанківськийРахунок = БанківськийРахунок.Pointer;
            ПрихіднийКасовийОрдер_Objest.Автор = Автор.Pointer;
            ПрихіднийКасовийОрдер_Objest.СумаДокументу = СумаДокументу.Value;
            ПрихіднийКасовийОрдер_Objest.Курс = Курс.Value;
            ПрихіднийКасовийОрдер_Objest.СтаттяРухуКоштів = СтаттяРухуКоштів.Pointer;
        }

        #endregion

        bool IsValidValue()
        {
            if (!ДатаДок.IsValidValue() || !Курс.IsValidValue() || !СумаДокументу.IsValidValue())
            {
                Message.Error(Program.GeneralForm, "Перевірте правельність заповнення полів типу Дата та Число");
                return false;
            }
            else return true;
        }

        void Save()
        {
            if (IsNew)
            {
                ПрихіднийКасовийОрдер_Objest.New();
                IsNew = false;
            }

            GetValue();

            ПрихіднийКасовийОрдер_Objest.Save();

            Program.GeneralForm?.RenameCurrentPageNotebook($"{ПрихіднийКасовийОрдер_Objest.Назва}");
        }

        void SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                try
                {
                    if (!ПрихіднийКасовийОрдер_Objest.SpendTheDocument(ПрихіднийКасовийОрдер_Objest.ДатаДок))
                    {
                        ПрихіднийКасовийОрдер_Objest.ClearSpendTheDocument();
                        ФункціїДляПовідомлень.ВідкритиТермінал();
                    }
                }
                catch (Exception exp)
                {
                    ПрихіднийКасовийОрдер_Objest.ClearSpendTheDocument();
                    Message.Error(Program.GeneralForm, exp.Message);
                    return;
                }
            }
            else
                ПрихіднийКасовийОрдер_Objest.ClearSpendTheDocument();
        }

        void ReloadList()
        {
            if (PageList != null)
            {
                PageList.SelectPointerItem = ПрихіднийКасовийОрдер_Objest.GetDocumentPointer();
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