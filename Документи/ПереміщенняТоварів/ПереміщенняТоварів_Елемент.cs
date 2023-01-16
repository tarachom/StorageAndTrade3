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
using StorageAndTrade_1_0.Документи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class ПереміщенняТоварів_Елемент : VBox
    {
        public ПереміщенняТоварів? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public ПереміщенняТоварів_Objest ПереміщенняТоварів_Objest { get; set; } = new ПереміщенняТоварів_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Організації_PointerControl ОрганізаціяОтримувач = new Організації_PointerControl();
        Склади_PointerControl СкладВідправник = new Склади_PointerControl() { Caption = "Склад відправник:" };
        Склади_PointerControl СкладОтримувач = new Склади_PointerControl() { Caption = "Склад отримувач:" };
        ComboBoxText ГосподарськаОперація = new ComboBoxText();
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ" };
        БанківськіРахункиОрганізацій_PointerControl БанківськийРахунокОрганізації = new БанківськіРахункиОрганізацій_PointerControl() { WidthPresentation = 200 };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        ComboBoxText СпосібДоставки = new ComboBoxText();
        TimeControl ЧасДоставкиЗ = new TimeControl();
        TimeControl ЧасДоставкиДо = new TimeControl();
        Entry Коментар = new Entry() { WidthRequest = 920 };

        ПереміщенняТоварів_ТабличнаЧастина_Товари Товари = new ПереміщенняТоварів_ТабличнаЧастина_Товари();

        #endregion

        public ПереміщенняТоварів_Елемент() : base()
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
                    page.CreateReport(ПереміщенняТоварів_Objest.GetDocumentPointer());
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
                    Перелічення.ГосподарськіОперації.ПереміщенняТоварів.ToString(),
                    Конфігурація_ГосподарськіОперації.Fields["ПереміщенняТоварів"].Desc);

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

            hBoxNumberDataDoc.PackStart(new Label("Переміщення товарів №:"), false, false, 5);
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

            //СкладВідправник
            HBox hBoxSkladVidpravnyk = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSkladVidpravnyk, false, false, 5);

            hBoxSkladVidpravnyk.PackStart(СкладВідправник, false, false, 5);
        }

        void CreateContainer2(VBox vBox)
        {
            //СкладОтримувач
            HBox hBoxSkladOtrymuvach = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSkladOtrymuvach, false, false, 5);

            hBoxSkladOtrymuvach.PackStart(СкладОтримувач, false, false, 5);
        }

        void CreateContainer3(VBox vBox)
        {
            //Підрозділ
            HBox hBoxPidrozdil = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxPidrozdil, false, false, 5);

            hBoxPidrozdil.PackStart(Підрозділ, false, false, 5);

            //БанківськийрахунокОрганізації
            HBox hBoxBankRahunokOrganization = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxBankRahunokOrganization, false, false, 5);

            hBoxBankRahunokOrganization.PackStart(БанківськийРахунокОрганізації, false, false, 5);

            //Автор
            HBox hBoxAutor = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxAutor, false, false, 5);

            hBoxAutor.PackStart(Автор, false, false, 5);
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
                ПереміщенняТоварів_Objest.НомерДок = (++НумераціяДокументів.ПереміщенняТоварів_Const).ToString("D8");
                ПереміщенняТоварів_Objest.ДатаДок = DateTime.Now;
                ПереміщенняТоварів_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                ПереміщенняТоварів_Objest.СкладВідправник = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                ПереміщенняТоварів_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
                ПереміщенняТоварів_Objest.БанківськийРахунокОрганізації = ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const;
            }

            НомерДок.Text = ПереміщенняТоварів_Objest.НомерДок;
            ДатаДок.Value = ПереміщенняТоварів_Objest.ДатаДок;
            Організація.Pointer = ПереміщенняТоварів_Objest.Організація;
            СкладВідправник.Pointer = ПереміщенняТоварів_Objest.СкладВідправник;
            СкладОтримувач.Pointer = ПереміщенняТоварів_Objest.СкладОтримувач;
            ГосподарськаОперація.ActiveId = ((Перелічення.ГосподарськіОперації)ПереміщенняТоварів_Objest.ГосподарськаОперація).ToString();
            Коментар.Text = ПереміщенняТоварів_Objest.Коментар;
            Підрозділ.Pointer = ПереміщенняТоварів_Objest.Підрозділ;
            БанківськийРахунокОрганізації.Pointer = ПереміщенняТоварів_Objest.БанківськийРахунокОрганізації;
            Автор.Pointer = ПереміщенняТоварів_Objest.Автор;
            СпосібДоставки.ActiveId = ((Перелічення.СпособиДоставки)ПереміщенняТоварів_Objest.СпосібДоставки).ToString();
            ЧасДоставкиЗ.Value = ПереміщенняТоварів_Objest.ЧасДоставкиЗ;
            ЧасДоставкиДо.Value = ПереміщенняТоварів_Objest.ЧасДоставкиДо;

            //Таблична частина
            Товари.ПереміщенняТоварів_Objest = ПереміщенняТоварів_Objest;
            Товари.LoadRecords();
        }

        void GetValue()
        {
            ПереміщенняТоварів_Objest.НомерДок = НомерДок.Text;
            ПереміщенняТоварів_Objest.ДатаДок = ДатаДок.Value;
            ПереміщенняТоварів_Objest.Назва = $"Переміщення товарів №{ПереміщенняТоварів_Objest.НомерДок} від {ПереміщенняТоварів_Objest.ДатаДок.ToShortDateString()}";
            ПереміщенняТоварів_Objest.Організація = Організація.Pointer;
            ПереміщенняТоварів_Objest.СкладВідправник = СкладВідправник.Pointer;
            ПереміщенняТоварів_Objest.СкладОтримувач = СкладОтримувач.Pointer;
            ПереміщенняТоварів_Objest.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            ПереміщенняТоварів_Objest.Коментар = Коментар.Text;
            ПереміщенняТоварів_Objest.Підрозділ = Підрозділ.Pointer;
            ПереміщенняТоварів_Objest.БанківськийРахунокОрганізації = БанківськийРахунокОрганізації.Pointer;
            ПереміщенняТоварів_Objest.Автор = Автор.Pointer;
            ПереміщенняТоварів_Objest.СпосібДоставки = Enum.Parse<Перелічення.СпособиДоставки>(СпосібДоставки.ActiveId);
            ПереміщенняТоварів_Objest.ЧасДоставкиЗ = ЧасДоставкиЗ.Value;
            ПереміщенняТоварів_Objest.ЧасДоставкиДо = ЧасДоставкиДо.Value;
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
                ПереміщенняТоварів_Objest.New();
                IsNew = false;
            }

            GetValue();

            ПереміщенняТоварів_Objest.Save();
            Товари.SaveRecords();

            Program.GeneralForm?.RenameCurrentPageNotebook($"{ПереміщенняТоварів_Objest.Назва}");
        }

        void SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                try
                {
                    if (!ПереміщенняТоварів_Objest.SpendTheDocument(ПереміщенняТоварів_Objest.ДатаДок))
                    {
                        ПереміщенняТоварів_Objest.ClearSpendTheDocument();
                        ФункціїДляПовідомлень.ВідкритиТермінал();
                    }
                }
                catch (Exception exp)
                {
                    ПереміщенняТоварів_Objest.ClearSpendTheDocument();
                    Message.Error(Program.GeneralForm, exp.Message);
                    return;
                }
            }
            else
                ПереміщенняТоварів_Objest.ClearSpendTheDocument();
        }

        void ReloadList()
        {
            Товари.LoadRecords();
            
            if (PageList != null)
            {
                PageList.SelectPointerItem = ПереміщенняТоварів_Objest.GetDocumentPointer();
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