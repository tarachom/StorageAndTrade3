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
    class ВстановленняЦінНоменклатури_Елемент : VBox
    {
        public ВстановленняЦінНоменклатури? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public ВстановленняЦінНоменклатури_Objest ВстановленняЦінНоменклатури_Objest { get; set; } = new ВстановленняЦінНоменклатури_Objest();

        #region Fields

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        DateTimeControl ДатаДок = new DateTimeControl();
        Організації_PointerControl Організація = new Організації_PointerControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();
        Користувачі_PointerControl Автор = new Користувачі_PointerControl();
        ВидиЦін_PointerControl ВидЦіни = new ВидиЦін_PointerControl();
        Entry Коментар = new Entry() { WidthRequest = 920 };

        ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари Товари = new ВстановленняЦінНоменклатури_ТабличнаЧастина_Товари();

        #endregion

        public ВстановленняЦінНоменклатури_Елемент() : base()
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
                    page.CreateReport(ВстановленняЦінНоменклатури_Objest.GetDocumentPointer());
                    return page;
                });
            };

            hBox.PackStart(linkButtonProvodky, false, false, 10);

            PackStart(hBox, false, false, 10);

            HPaned hPaned = new HPaned() { Orientation = Orientation.Vertical, BorderWidth = 5 };

            CreatePack1(hPaned);
            CreatePack2(hPaned);

            PackStart(hPaned, true, true, 0);

            ShowAll();
        }

        void CreatePack1(HPaned hPaned)
        {
            VBox vBox = new VBox();
            hPaned.Pack1(vBox, false, false);

            //НомерДок ДатаДок
            HBox hBoxNumberDataDoc = new HBox() { Halign = Align.Start };
            vBox.PackStart(hBoxNumberDataDoc, false, false, 5);

            hBoxNumberDataDoc.PackStart(new Label("Встановлення цін номенклатури №:"), false, false, 5);
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
            //Валюта
            HBox hBoxValuta = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxValuta, false, false, 5);

            hBoxValuta.PackStart(Валюта, false, false, 5);

            //ВидЦіни
            HBox hBoxVidCeny = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxVidCeny, false, false, 5);

            hBoxVidCeny.PackStart(ВидЦіни, false, false, 5);
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
                ВстановленняЦінНоменклатури_Objest.НомерДок = (++НумераціяДокументів.ВстановленняЦінНоменклатури_Const).ToString("D8");
                ВстановленняЦінНоменклатури_Objest.ДатаДок = DateTime.Now;
                ВстановленняЦінНоменклатури_Objest.Організація = ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
                ВстановленняЦінНоменклатури_Objest.Валюта = ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
            }

            НомерДок.Text = ВстановленняЦінНоменклатури_Objest.НомерДок;
            ДатаДок.Value = ВстановленняЦінНоменклатури_Objest.ДатаДок;
            Організація.Pointer = ВстановленняЦінНоменклатури_Objest.Організація;
            Валюта.Pointer = ВстановленняЦінНоменклатури_Objest.Валюта;
            Коментар.Text = ВстановленняЦінНоменклатури_Objest.Коментар;
            Автор.Pointer = ВстановленняЦінНоменклатури_Objest.Автор;

            Товари.ВстановленняЦінНоменклатури_Objest = ВстановленняЦінНоменклатури_Objest;
            Товари.ОбновитиЗначенняДокумента = () =>
            {
                ВстановленняЦінНоменклатури_Objest.ВидЦіни = ВидЦіни.Pointer;
            };
            Товари.LoadRecords();
        }

        void GetValue()
        {
            ВстановленняЦінНоменклатури_Objest.НомерДок = НомерДок.Text;
            ВстановленняЦінНоменклатури_Objest.ДатаДок = ДатаДок.Value;
            ВстановленняЦінНоменклатури_Objest.Назва = $"Встановлення цін номенклатури №{ВстановленняЦінНоменклатури_Objest.НомерДок} від {ВстановленняЦінНоменклатури_Objest.ДатаДок.ToShortDateString()}";
            ВстановленняЦінНоменклатури_Objest.Організація = Організація.Pointer;
            ВстановленняЦінНоменклатури_Objest.Валюта = Валюта.Pointer;
            ВстановленняЦінНоменклатури_Objest.Коментар = Коментар.Text;
            ВстановленняЦінНоменклатури_Objest.Автор = Автор.Pointer;
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
                ВстановленняЦінНоменклатури_Objest.New();
                IsNew = false;
            }

            GetValue();

            ВстановленняЦінНоменклатури_Objest.Save();
            Товари.SaveRecords();

            Program.GeneralForm?.RenameCurrentPageNotebook($"{ВстановленняЦінНоменклатури_Objest.Назва}");
        }

        void SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                try
                {
                    if (!ВстановленняЦінНоменклатури_Objest.SpendTheDocument(ВстановленняЦінНоменклатури_Objest.ДатаДок))
                    {
                        ВстановленняЦінНоменклатури_Objest.ClearSpendTheDocument();
                        ФункціїДляПовідомлень.ВідкритиТермінал();
                    }
                }
                catch (Exception exp)
                {
                    ВстановленняЦінНоменклатури_Objest.ClearSpendTheDocument();
                    Message.Error(Program.GeneralForm, exp.Message);
                    return;
                }
            }
            else
                ВстановленняЦінНоменклатури_Objest.ClearSpendTheDocument();
        }

        void ReloadList()
        {
            Товари.LoadRecords();

            if (PageList != null)
            {
                PageList.SelectPointerItem = ВстановленняЦінНоменклатури_Objest.GetDocumentPointer();
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

            if (ВстановленняЦінНоменклатури_Objest.Spend)
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