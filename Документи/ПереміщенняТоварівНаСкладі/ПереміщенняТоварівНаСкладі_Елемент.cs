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
    class ПереміщенняТоварівНаСкладі_Елемент : VBox
    {
        public ПереміщенняТоварівНаСкладі? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public ПереміщенняТоварівНаСкладі_Objest ПереміщенняТоварівНаСкладі_Objest { get; set; } = new ПереміщенняТоварівНаСкладі_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Склади_PointerControl Склад = new Склади_PointerControl();
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ" };
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        Entry Коментар = new Entry() { WidthRequest = 920 };
        ПереміщенняТоварівНаСкладі_ТабличнаЧастина_Товари Товари = new ПереміщенняТоварівНаСкладі_ТабличнаЧастина_Товари();
        Basis_PointerControl Основа = new Basis_PointerControl();

        #endregion

        public ПереміщенняТоварівНаСкладі_Елемент() : base()
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
                    page.CreateReport(ПереміщенняТоварівНаСкладі_Objest.GetDocumentPointer());
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
                ПереміщенняТоварівНаСкладі_Objest.НомерДок = (++НумераціяДокументів.ПереміщенняТоварівНаСкладі_Const).ToString("D8");
                ПереміщенняТоварівНаСкладі_Objest.ДатаДок = DateTime.Now;
                ПереміщенняТоварівНаСкладі_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                ПереміщенняТоварівНаСкладі_Objest.Склад = ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
                ПереміщенняТоварівНаСкладі_Objest.Підрозділ = ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
                ПереміщенняТоварівНаСкладі_Objest.Автор = Program.Користувач;
            }

            НомерДок.Text = ПереміщенняТоварівНаСкладі_Objest.НомерДок;
            ДатаДок.Value = ПереміщенняТоварівНаСкладі_Objest.ДатаДок;
            Організація.Pointer = ПереміщенняТоварівНаСкладі_Objest.Організація;
            Склад.Pointer = ПереміщенняТоварівНаСкладі_Objest.Склад;
            Коментар.Text = ПереміщенняТоварівНаСкладі_Objest.Коментар;
            Підрозділ.Pointer = ПереміщенняТоварівНаСкладі_Objest.Підрозділ;
            Автор.Pointer = ПереміщенняТоварівНаСкладі_Objest.Автор;
            Основа.Pointer = ПереміщенняТоварівНаСкладі_Objest.Основа;

            //Таблична частина
            Товари.ПереміщенняТоварівНаСкладі_Objest = ПереміщенняТоварівНаСкладі_Objest;
            Товари.ОбновитиЗначенняДокумента = () =>
            {
                ПереміщенняТоварівНаСкладі_Objest.Склад = Склад.Pointer;
            };
            Товари.LoadRecords();
        }

        void GetValue()
        {
            ПереміщенняТоварівНаСкладі_Objest.НомерДок = НомерДок.Text;
            ПереміщенняТоварівНаСкладі_Objest.ДатаДок = ДатаДок.Value;
            ПереміщенняТоварівНаСкладі_Objest.Назва = $"Переміщення товарів на складі №{ПереміщенняТоварівНаСкладі_Objest.НомерДок} від {ПереміщенняТоварівНаСкладі_Objest.ДатаДок.ToShortDateString()}";
            ПереміщенняТоварівНаСкладі_Objest.Організація = Організація.Pointer;
            ПереміщенняТоварівНаСкладі_Objest.Склад = Склад.Pointer;
            ПереміщенняТоварівНаСкладі_Objest.Коментар = Коментар.Text;
            ПереміщенняТоварівНаСкладі_Objest.Підрозділ = Підрозділ.Pointer;
            ПереміщенняТоварівНаСкладі_Objest.Автор = Автор.Pointer;
            ПереміщенняТоварівНаСкладі_Objest.Основа = Основа.Pointer;
        }

        #endregion

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
            if (IsNew)
            {
                ПереміщенняТоварівНаСкладі_Objest.New();
                IsNew = false;
            }

            GetValue();

            ПереміщенняТоварівНаСкладі_Objest.Save();
            Товари.SaveRecords();

            Program.GeneralForm?.RenameCurrentPageNotebook($"{ПереміщенняТоварівНаСкладі_Objest.Назва}");
        }

        void SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                try
                {
                    if (!ПереміщенняТоварівНаСкладі_Objest.SpendTheDocument(ПереміщенняТоварівНаСкладі_Objest.ДатаДок))
                    {
                        ПереміщенняТоварівНаСкладі_Objest.ClearSpendTheDocument();
                        ФункціїДляПовідомлень.ВідкритиТермінал();
                    }
                }
                catch (Exception exp)
                {
                    ПереміщенняТоварівНаСкладі_Objest.ClearSpendTheDocument();
                    Message.Error(Program.GeneralForm, exp.Message);
                    return;
                }
            }
            else
                ПереміщенняТоварівНаСкладі_Objest.ClearSpendTheDocument();
        }

        void ReloadList()
        {
            Товари.LoadRecords();

            if (PageList != null)
            {
                PageList.SelectPointerItem = ПереміщенняТоварівНаСкладі_Objest.GetDocumentPointer();
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

            if (ПереміщенняТоварівНаСкладі_Objest.Spend)
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