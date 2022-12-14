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
    class ПоверненняТоварівВідКлієнта_Елемент : VBox
    {
        public ПоверненняТоварівВідКлієнта? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public ПоверненняТоварівВідКлієнта_Objest ПоверненняТоварівВідКлієнта_Objest { get; set; } = new ПоверненняТоварівВідКлієнта_Objest();

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
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        Користувачі_PointerControl Менеджер = new Користувачі_PointerControl() { Caption = "Менеджер" };
        Entry Коментар = new Entry() { WidthRequest = 920 };

        ПоверненняТоварівВідКлієнта_ТабличнаЧастина_Товари Товари = new ПоверненняТоварівВідКлієнта_ТабличнаЧастина_Товари();

        #endregion        

        public ПоверненняТоварівВідКлієнта_Елемент() : base()
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
                    page.CreateReport(ПоверненняТоварівВідКлієнта_Objest.GetDocumentPointer());
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
                    Перелічення.ГосподарськіОперації.ПоверненняТоварівВідКлієнта.ToString(),
                    Конфігурація_ГосподарськіОперації.Fields["ПоверненняТоварівВідКлієнта"].Desc);

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

            hBoxNumberDataDoc.PackStart(new Label("Повернення товарів від клієнта №:"), false, false, 5);
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
            //Підрозділ
            HBox hBoxPidrozdil = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxPidrozdil, false, false, 5);

            hBoxPidrozdil.PackStart(Підрозділ, false, false, 5);

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
            //ГосподарськаОперація
            HBox hBoxOperation = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxOperation, false, false, 5);

            hBoxOperation.PackStart(new Label("Господарська операція: "), false, false, 0);
            hBoxOperation.PackStart(ГосподарськаОперація, false, false, 5);
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
                ПоверненняТоварівВідКлієнта_Objest.НомерДок = (++НумераціяДокументів.ПоверненняТоварівВідКлієнта_Const).ToString("D8");
                ПоверненняТоварівВідКлієнта_Objest.ДатаДок = DateTime.Now;
                ПоверненняТоварівВідКлієнта_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                ПоверненняТоварівВідКлієнта_Objest.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
                ПоверненняТоварівВідКлієнта_Objest.Каса = ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const;
                ПоверненняТоварівВідКлієнта_Objest.Склад = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                ПоверненняТоварівВідКлієнта_Objest.Контрагент = ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
                ПоверненняТоварівВідКлієнта_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
            }

            НомерДок.Text = ПоверненняТоварівВідКлієнта_Objest.НомерДок;
            ДатаДок.Value = ПоверненняТоварівВідКлієнта_Objest.ДатаДок;
            Організація.Pointer = ПоверненняТоварівВідКлієнта_Objest.Організація;
            Валюта.Pointer = ПоверненняТоварівВідКлієнта_Objest.Валюта;
            Каса.Pointer = ПоверненняТоварівВідКлієнта_Objest.Каса;
            Склад.Pointer = ПоверненняТоварівВідКлієнта_Objest.Склад;
            Контрагент.Pointer = ПоверненняТоварівВідКлієнта_Objest.Контрагент;
            Договір.Pointer = ПоверненняТоварівВідКлієнта_Objest.Договір;
            ГосподарськаОперація.ActiveId = ((Перелічення.ГосподарськіОперації)ПоверненняТоварівВідКлієнта_Objest.ГосподарськаОперація).ToString();
            Коментар.Text = ПоверненняТоварівВідКлієнта_Objest.Коментар;
            Підрозділ.Pointer = ПоверненняТоварівВідКлієнта_Objest.Підрозділ;
            Автор.Pointer = ПоверненняТоварівВідКлієнта_Objest.Автор;
            Менеджер.Pointer = ПоверненняТоварівВідКлієнта_Objest.Менеджер;

            //Таблична частина
            Товари.ПоверненняТоварівВідКлієнта_Objest = ПоверненняТоварівВідКлієнта_Objest;
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
            ПоверненняТоварівВідКлієнта_Objest.НомерДок = НомерДок.Text;
            ПоверненняТоварівВідКлієнта_Objest.ДатаДок = ДатаДок.Value;
            ПоверненняТоварівВідКлієнта_Objest.Назва = $"Повернення товарів від клієнта №{ПоверненняТоварівВідКлієнта_Objest.НомерДок} від {ПоверненняТоварівВідКлієнта_Objest.ДатаДок.ToShortDateString()}";
            ПоверненняТоварівВідКлієнта_Objest.Організація = Організація.Pointer;
            ПоверненняТоварівВідКлієнта_Objest.Валюта = Валюта.Pointer;
            ПоверненняТоварівВідКлієнта_Objest.Каса = Каса.Pointer;
            ПоверненняТоварівВідКлієнта_Objest.Склад = Склад.Pointer;
            ПоверненняТоварівВідКлієнта_Objest.Контрагент = Контрагент.Pointer;
            ПоверненняТоварівВідКлієнта_Objest.Договір = Договір.Pointer;
            ПоверненняТоварівВідКлієнта_Objest.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            ПоверненняТоварівВідКлієнта_Objest.Коментар = Коментар.Text;
            ПоверненняТоварівВідКлієнта_Objest.Підрозділ = Підрозділ.Pointer;
            ПоверненняТоварівВідКлієнта_Objest.Автор = Автор.Pointer;
            ПоверненняТоварівВідКлієнта_Objest.Менеджер = Менеджер.Pointer;

            ПоверненняТоварівВідКлієнта_Objest.СумаДокументу = Товари.СумаДокументу();
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
                ПоверненняТоварівВідКлієнта_Objest.New();
                IsNew = false;
            }

            GetValue();

            ПоверненняТоварівВідКлієнта_Objest.Save();
            Товари.SaveRecords();

            Program.GeneralForm?.RenameCurrentPageNotebook($"{ПоверненняТоварівВідКлієнта_Objest.Назва}");
        }

        void SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                try
                {
                    if (!ПоверненняТоварівВідКлієнта_Objest.SpendTheDocument(ПоверненняТоварівВідКлієнта_Objest.ДатаДок))
                    {
                        ПоверненняТоварівВідКлієнта_Objest.ClearSpendTheDocument();
                        ФункціїДляПовідомлень.ВідкритиТермінал();
                    }
                }
                catch (Exception exp)
                {
                    ПоверненняТоварівВідКлієнта_Objest.ClearSpendTheDocument();
                    Message.Error(Program.GeneralForm, exp.Message);
                    return;
                }
            }
            else
                ПоверненняТоварівВідКлієнта_Objest.ClearSpendTheDocument();
        }

        void ReloadList()
        {
            if (PageList != null)
            {
                Товари.LoadRecords();

                PageList.SelectPointerItem = ПоверненняТоварівВідКлієнта_Objest.GetDocumentPointer();
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