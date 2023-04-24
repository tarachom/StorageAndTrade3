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

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class РозміщенняТоварівНаСкладі_Елемент : VBox
    {
        public РозміщенняТоварівНаСкладі? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public РозміщенняТоварівНаСкладі_Objest РозміщенняТоварівНаСкладі_Objest { get; set; } = new РозміщенняТоварівНаСкладі_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Склади_PointerControl Склад = new Склади_PointerControl();
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ" };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        Entry Коментар = new Entry() { WidthRequest = 920 };
        ПоступленняТоварівТаПослуг_PointerControl ДокументПоступлення = new ПоступленняТоварівТаПослуг_PointerControl() { Caption = "Документ поступлення: " };
        Basis_PointerControl Основа = new Basis_PointerControl();

        РозміщенняТоварівНаСкладі_ТабличнаЧастина_Товари Товари = new РозміщенняТоварівНаСкладі_ТабличнаЧастина_Товари();

        #endregion

        public РозміщенняТоварівНаСкладі_Елемент() : base()
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
                    page.CreateReport(РозміщенняТоварівНаСкладі_Objest.GetDocumentPointer());
                    return page;
                });
            };

            hBox.PackStart(linkButtonProvodky, false, false, 10);

            PackStart(hBox, false, false, 10);

            HPaned hPaned = new HPaned() { Orientation = Orientation.Vertical, BorderWidth = 5 };

            CreatePack1(hPaned);
            CreatePack2(hPaned);

            PackStart(hPaned, true, true, 5);

            ShowAll();
        }

        void CreatePack1(HPaned hPaned)
        {
            VBox vBox = new VBox();
            hPaned.Pack1(vBox, false, false);

            //НомерДок ДатаДок
            HBox hBoxNumberDataDoc = new HBox() { Halign = Align.Start };
            vBox.PackStart(hBoxNumberDataDoc, false, false, 5);

            hBoxNumberDataDoc.PackStart(new Label("Розміщення товарів на складі №:"), false, false, 5);
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

            //ДокументПоступлення
            HBox hBoxDocPostuplenja = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxDocPostuplenja, false, false, 5);

            hBoxDocPostuplenja.PackStart(ДокументПоступлення, false, false, 5);
        }

        void CreateContainer2(VBox vBox)
        {
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

            //Основа
            HBox hBoxBasis = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxBasis, false, false, 5);

            hBoxBasis.PackStart(Основа, false, false, 5);
        }

        void CreateContainer4(VBox vBox)
        {

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
                РозміщенняТоварівНаСкладі_Objest.New();
                РозміщенняТоварівНаСкладі_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                РозміщенняТоварівНаСкладі_Objest.Склад = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                РозміщенняТоварівНаСкладі_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
            }

            НомерДок.Text = РозміщенняТоварівНаСкладі_Objest.НомерДок;
            ДатаДок.Value = РозміщенняТоварівНаСкладі_Objest.ДатаДок;
            Організація.Pointer = РозміщенняТоварівНаСкладі_Objest.Організація;
            Склад.Pointer = РозміщенняТоварівНаСкладі_Objest.Склад;
            Коментар.Text = РозміщенняТоварівНаСкладі_Objest.Коментар;
            Підрозділ.Pointer = РозміщенняТоварівНаСкладі_Objest.Підрозділ;
            Автор.Pointer = РозміщенняТоварівНаСкладі_Objest.Автор;
            ДокументПоступлення.Pointer = РозміщенняТоварівНаСкладі_Objest.ДокументПоступлення;
            Основа.Pointer = РозміщенняТоварівНаСкладі_Objest.Основа;

            //Таблична частина
            Товари.РозміщенняТоварівНаСкладі_Objest = РозміщенняТоварівНаСкладі_Objest;
            Товари.ОбновитиЗначенняДокумента = () =>
            {
                РозміщенняТоварівНаСкладі_Objest.Склад = Склад.Pointer;
            };

            Товари.LoadRecords();
        }

        void GetValue()
        {
            РозміщенняТоварівНаСкладі_Objest.НомерДок = НомерДок.Text;
            РозміщенняТоварівНаСкладі_Objest.ДатаДок = ДатаДок.Value;
            РозміщенняТоварівНаСкладі_Objest.Організація = Організація.Pointer;
            РозміщенняТоварівНаСкладі_Objest.Склад = Склад.Pointer;
            РозміщенняТоварівНаСкладі_Objest.Коментар = Коментар.Text;
            РозміщенняТоварівНаСкладі_Objest.Підрозділ = Підрозділ.Pointer;
            РозміщенняТоварівНаСкладі_Objest.Автор = Автор.Pointer;
            РозміщенняТоварівНаСкладі_Objest.ДокументПоступлення = ДокументПоступлення.Pointer;
            РозміщенняТоварівНаСкладі_Objest.Основа = Основа.Pointer;

            РозміщенняТоварівНаСкладі_Objest.КлючовіСловаДляПошуку = КлючовіСловаДляПошуку() + Товари.КлючовіСловаДляПошуку();
        }

        #endregion

        string КлючовіСловаДляПошуку()
        {
            return $"\n{Організація.Pointer.Назва} {Склад.Pointer.Назва}";
        }

        #region Save & Spend

        bool Save()
        {
            GetValue();

            bool isSave = false;

            try
            {
                isSave = РозміщенняТоварівНаСкладі_Objest.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомленняПроПомилку(DateTime.Now, "Запис",
                    РозміщенняТоварівНаСкладі_Objest.UnigueID.UGuid, "Документ", РозміщенняТоварівНаСкладі_Objest.Назва, ex.Message);

                ФункціїДляПовідомлень.ВідкритиТермінал();
            }

            if (!isSave)
            {
                Message.Info(Program.GeneralForm, "Не вдалось записати документ");
                return false;
            }

            Товари.SaveRecords();

            Program.GeneralForm?.RenameCurrentPageNotebook($"{РозміщенняТоварівНаСкладі_Objest.Назва}");

            return true;
        }

        void SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                if (!РозміщенняТоварівНаСкладі_Objest.SpendTheDocument(РозміщенняТоварівНаСкладі_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ВідкритиТермінал();
            }
            else
                РозміщенняТоварівНаСкладі_Objest.ClearSpendTheDocument();
        }

        void ReloadList()
        {
            if (PageList != null)
            {
                PageList.SelectPointerItem = РозміщенняТоварівНаСкладі_Objest.GetDocumentPointer();
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
            if (isSave && РозміщенняТоварівНаСкладі_Objest.Spend)
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