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
using StorageAndTrade_1_0.Документи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class ВнутрішнєСпоживанняТоварів_Елемент : VBox
    {
        public ВнутрішнєСпоживанняТоварів? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public ВнутрішнєСпоживанняТоварів_Objest ВнутрішнєСпоживанняТоварів_Objest { get; set; } = new ВнутрішнєСпоживанняТоварів_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();
        Склади_PointerControl Склад = new Склади_PointerControl();
        ComboBoxText ГосподарськаОперація = new ComboBoxText();
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ" };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        Entry Коментар = new Entry() { WidthRequest = 920 };
        Basis_PointerControl Основа = new Basis_PointerControl();

        ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари Товари = new ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари();

        #endregion

        public ВнутрішнєСпоживанняТоварів_Елемент() : base()
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
                    page.CreateReport(ВнутрішнєСпоживанняТоварів_Objest.GetDocumentPointer());
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
                    Перелічення.ГосподарськіОперації.ВнутрішнєСпоживанняТоварів.ToString(),
                    Конфігурація_ГосподарськіОперації.Fields["ВнутрішнєСпоживанняТоварів"].Desc);

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

            hBoxNumberDataDoc.PackStart(new Label($"{ВнутрішнєСпоживанняТоварів_Const.FULLNAME} №:"), false, false, 5);
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

            //Склад
            HBox hBoxSklad = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSklad, false, false, 5);

            hBoxSklad.PackStart(Склад, false, false, 5);
        }

        void CreateContainer2(VBox vBox)
        {
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

            //Автор
            HBox hBoxAutor = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxAutor, false, false, 5);

            hBoxAutor.PackStart(Автор, false, false, 5);
        }

        void CreateContainer4(VBox vBox)
        {
            //Основа
            HBox hBoxBasis = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxBasis, false, false, 5);

            hBoxBasis.PackStart(Основа, false, false, 5);
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
                ВнутрішнєСпоживанняТоварів_Objest.New();
                ВнутрішнєСпоживанняТоварів_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                ВнутрішнєСпоживанняТоварів_Objest.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
                ВнутрішнєСпоживанняТоварів_Objest.Склад = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                ВнутрішнєСпоживанняТоварів_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
            }

            НомерДок.Text = ВнутрішнєСпоживанняТоварів_Objest.НомерДок;
            ДатаДок.Value = ВнутрішнєСпоживанняТоварів_Objest.ДатаДок;
            Організація.Pointer = ВнутрішнєСпоживанняТоварів_Objest.Організація;
            Валюта.Pointer = ВнутрішнєСпоживанняТоварів_Objest.Валюта;
            Склад.Pointer = ВнутрішнєСпоживанняТоварів_Objest.Склад;
            ГосподарськаОперація.ActiveId = ((Перелічення.ГосподарськіОперації)ВнутрішнєСпоживанняТоварів_Objest.ГосподарськаОперація).ToString();
            Коментар.Text = ВнутрішнєСпоживанняТоварів_Objest.Коментар;
            Підрозділ.Pointer = ВнутрішнєСпоживанняТоварів_Objest.Підрозділ;
            Автор.Pointer = ВнутрішнєСпоживанняТоварів_Objest.Автор;
            Основа.Pointer = ВнутрішнєСпоживанняТоварів_Objest.Основа;

            //Таблична частина
            Товари.ВнутрішнєСпоживанняТоварів_Objest = ВнутрішнєСпоживанняТоварів_Objest;
            Товари.LoadRecords();
        }

        void GetValue()
        {
            ВнутрішнєСпоживанняТоварів_Objest.НомерДок = НомерДок.Text;
            ВнутрішнєСпоживанняТоварів_Objest.ДатаДок = ДатаДок.Value;
            ВнутрішнєСпоживанняТоварів_Objest.Організація = Організація.Pointer;
            ВнутрішнєСпоживанняТоварів_Objest.Валюта = Валюта.Pointer;
            ВнутрішнєСпоживанняТоварів_Objest.Склад = Склад.Pointer;
            ВнутрішнєСпоживанняТоварів_Objest.ГосподарськаОперація = Enum.Parse<Перелічення.ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            ВнутрішнєСпоживанняТоварів_Objest.Коментар = Коментар.Text;
            ВнутрішнєСпоживанняТоварів_Objest.Підрозділ = Підрозділ.Pointer;
            ВнутрішнєСпоживанняТоварів_Objest.Автор = Автор.Pointer;
            ВнутрішнєСпоживанняТоварів_Objest.Основа = Основа.Pointer;

            ВнутрішнєСпоживанняТоварів_Objest.СумаДокументу = Товари.СумаДокументу();
            ВнутрішнєСпоживанняТоварів_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
        }

        #endregion

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Валюта.Pointer.Назва} {Склад.Pointer.Назва} ";
        }

        #region Save & Spend

        bool Save()
        {
            GetValue();

            bool isSave = false;

            try
            {
                isSave = ВнутрішнєСпоживанняТоварів_Objest.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомленняПроПомилку(DateTime.Now, "Запис",
                    ВнутрішнєСпоживанняТоварів_Objest.UnigueID.UGuid, "Документ", ВнутрішнєСпоживанняТоварів_Objest.Назва, ex.Message);

                ФункціїДляПовідомлень.ВідкритиТермінал();
            }

            if (!isSave)
            {
                Message.Info(Program.GeneralForm, "Не вдалось записати документ");
                return false;
            }

            Товари.SaveRecords();

            Program.GeneralForm?.RenameCurrentPageNotebook($"{ВнутрішнєСпоживанняТоварів_Objest.Назва}");

            return true;
        }

        void SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                if (!ВнутрішнєСпоживанняТоварів_Objest.SpendTheDocument(ВнутрішнєСпоживанняТоварів_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ВідкритиТермінал();
            }
            else
                ВнутрішнєСпоживанняТоварів_Objest.ClearSpendTheDocument();
        }

        void ReloadList()
        {
            if (PageList != null)
            {
                PageList.SelectPointerItem = ВнутрішнєСпоживанняТоварів_Objest.UnigueID;
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
            if (isSave && ВнутрішнєСпоживанняТоварів_Objest.Spend)
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