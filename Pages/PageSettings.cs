/*
Copyright (C) 2019-2024 TARAKHOMYN YURIY IVANOVYCH
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

/*

Налаштування

*/

using Gtk;
using InterfaceGtk;

using AccountingSoftware;
using StorageAndTrade_1_0;
using Константи = StorageAndTrade_1_0.Константи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class PageSettings : Box
    {

        #region Const

        //
        //Значення за замовчуванням
        //

        Організації_PointerControl ОсновнаОрганізація = new Організації_PointerControl();
        Склади_PointerControl ОсновнийСклад = new Склади_PointerControl();
        Валюти_PointerControl ОсновнаВалюта = new Валюти_PointerControl();
        Контрагенти_PointerControl ОсновнийПостачальник = new Контрагенти_PointerControl() { Caption = "Постачальник:" };
        Контрагенти_PointerControl ОсновнийПокупець = new Контрагенти_PointerControl() { Caption = "Покупець:" };
        Каси_PointerControl ОсновнаКаса = new Каси_PointerControl();
        ПакуванняОдиниціВиміру_PointerControl ОсновнаОдиницяПакування = new ПакуванняОдиниціВиміру_PointerControl() { Caption = "Пакування:" };
        СтруктураПідприємства_PointerControl ОсновнийПідрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ:" };
        БанківськіРахункиОрганізацій_PointerControl ОсновнийБанківськийРахунок = new БанківськіРахункиОрганізацій_PointerControl() { Caption = "Банківський рахунок:" };
        ВидиЦін_PointerControl ОсновнийВидЦіни = new ВидиЦін_PointerControl() { Caption = "Вид цін продажу:" };
        ВидиЦін_PointerControl ОсновнийВидЦіниЗакупівлі = new ВидиЦін_PointerControl() { Caption = "Вид цін закупівлі:" };
        ВидиНоменклатури_PointerControl ОсновнийВидНоменклатури = new ВидиНоменклатури_PointerControl();

        //
        //Системні
        //

        CheckButton ВестиОблікПоХарактеристикахНоменклатури = new CheckButton("Вести облік по характеристиках номенклатури");
        CheckButton ВестиОблікПоСеріяхНоменклатури = new CheckButton("Вести облік по серіях номенклатури");
        ComboBoxText МетодиСписанняПартій = new ComboBoxText();
        CheckButton ЗупинитиФоновіЗадачі = new CheckButton("Зупинити фонове обчислення віртуальних залишків");

        //
        //ЖурналиДокументів
        //

        ComboBoxText ОсновнийТипПеріоду_ДляЖурналівДокументів = ПеріодДляЖурналу.СписокВідбірПоПеріоду();

        //
        //ЗавантаженняДанихІзСайтів
        //

        Entry ЗавантаженняДанихІзСайтів = new Entry() { WidthRequest = 500 };
        Entry ЗавантаженняСпискуБанківІзСайтів = new Entry() { WidthRequest = 500 };

        #endregion

        public PageSettings() : base(Orientation.Vertical, 0)
        {
            //Кнопки
            Box hBox = new Box(Orientation.Horizontal, 0);

            Button bSave = new Button("Зберегти");
            bSave.Clicked += OnSaveClick;

            hBox.PackStart(bSave, false, false, 10);

            PackStart(hBox, false, false, 10);

            FillComboBoxes();

            Paned hPaned = new Paned(Orientation.Horizontal) { BorderWidth = 5, Position = 500 };

            CreatePack1(hPaned);
            CreatePack2(hPaned);

            PackStart(hPaned, false, false, 5);

            ShowAll();
        }

        void FillComboBoxes()
        {
            foreach (ConfigurationEnumField field in Config.Kernel.Conf.Enums["МетодиСписанняПартій"].Fields.Values)
                МетодиСписанняПартій.Append(field.Name, field.Desc);
        }

        void CreatePack1(Paned hPaned)
        {
            Box vBox = new Box(Orientation.Vertical, 0);

            CreateDefaultBlock(vBox);

            vBox.PackStart(new Separator(Orientation.Horizontal), false, false, 10);

            CreateJournalBlock(vBox);

            vBox.PackStart(new Separator(Orientation.Horizontal), false, false, 10);

            CreateLinkBlock(vBox);

            hPaned.Pack1(vBox, false, false);
        }

        void CreatePack2(Paned hPaned)
        {
            Box vBox = new Box(Orientation.Vertical, 0);
            hPaned.Pack2(vBox, false, false);

            //
            //Системні
            //

            //1
            {
                Box vBoxSystem = new Box(Orientation.Vertical, 0);

                Expander expanderSystem = new Expander("Налаштування обліку") { Expanded = false };
                expanderSystem.Add(vBoxSystem);

                //Info
                Box hBoxInfo = new Box(Orientation.Horizontal, 0) { Halign = Align.Start };
                vBoxSystem.PackStart(hBoxInfo, false, false, 15);
                hBoxInfo.PackStart(new Label("Видимість колонок у документах і звітах"), false, false, 5);

                //Controls
                AddControl(vBoxSystem, ВестиОблікПоХарактеристикахНоменклатури);
                AddControl(vBoxSystem, ВестиОблікПоСеріяхНоменклатури);

                vBox.PackStart(expanderSystem, false, false, 10);
            }

            //2
            {
                Box vBoxBatch = new Box(Orientation.Vertical, 0);

                Expander expanderBatch = new Expander("Партії товарів") { Expanded = false };
                expanderBatch.Add(vBoxBatch);

                //Info
                Box hBoxInfo = new Box(Orientation.Horizontal, 0) { Halign = Align.Start };
                vBoxBatch.PackStart(hBoxInfo, false, false, 15);
                hBoxInfo.PackStart(new Label("Метод списання партій товарів"), false, false, 5);

                //Controls
                AddControl(vBoxBatch, МетодиСписанняПартій);

                vBox.PackStart(expanderBatch, false, false, 10);
            }

            //3
            {
                Box vBoxBackgroundTask = new Box(Orientation.Vertical, 0);

                Expander expanderBackgroundTask = new Expander("Фонові обчислення") { Expanded = false };
                expanderBackgroundTask.Add(vBoxBackgroundTask);

                //Info
                Box hBoxInfo = new Box(Orientation.Horizontal, 0) { Halign = Align.Start };
                vBoxBackgroundTask.PackStart(hBoxInfo, false, false, 15);
                hBoxInfo.PackStart(new Label("Обчислення віртуальних залишків по регістрах накопичення") { Wrap = true }, false, false, 5);

                //Controls
                AddControl(vBoxBackgroundTask, ЗупинитиФоновіЗадачі);

                vBox.PackStart(expanderBackgroundTask, false, false, 10);
            }

            //4
            {
                Box vBoxDownloadCursNBU = new Box(Orientation.Vertical, 0);

                Expander expanderDownloadCursNBU = new Expander("Завантаження курсів валют НБУ") { Expanded = false };
                expanderDownloadCursNBU.Add(vBoxDownloadCursNBU);

                //Info
                Box hBoxInfo = new Box(Orientation.Horizontal, 0) { Halign = Align.Start };
                vBoxDownloadCursNBU.PackStart(hBoxInfo, false, false, 15);
                hBoxInfo.PackStart(new Label("За замовчуванням: https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange") { Selectable = true, Wrap = true }, false, false, 5);

                //Controls
                AddCaptionAndControl(vBoxDownloadCursNBU, new Label("Лінк:"), ЗавантаженняДанихІзСайтів);

                vBox.PackStart(expanderDownloadCursNBU, false, false, 10);
            }

            //5
            {
                Box vBoxDownloadListBank = new Box(Orientation.Vertical, 0);

                Expander expanderDownloadListBank = new Expander("Завантаження списку банків") { Expanded = false };
                expanderDownloadListBank.Add(vBoxDownloadListBank);

                //Info
                Box hBoxInfo = new Box(Orientation.Horizontal, 0) { Halign = Align.Start };
                vBoxDownloadListBank.PackStart(hBoxInfo, false, false, 15);
                hBoxInfo.PackStart(new Label("За замовчуванням: https://bank.gov.ua/NBU_BankInfo/get_data_branch_glbank") { Selectable = true, Wrap = true }, false, false, 5);

                //Controls
                AddCaptionAndControl(vBoxDownloadListBank, new Label("Лінк:"), ЗавантаженняСпискуБанківІзСайтів);

                vBox.PackStart(expanderDownloadListBank, false, false, 10);
            }
        }

        //Значення за замовчування
        void CreateDefaultBlock(Box vBoxTop)
        {
            Box vBox = new Box(Orientation.Vertical, 0);

            Expander expanderConstDefault = new Expander("Значення за замовчуванням") { Expanded = true };
            expanderConstDefault.Add(vBox);

            //Info
            Box hBoxInfo = new Box(Orientation.Horizontal, 0) { Halign = Align.Start };
            vBox.PackStart(hBoxInfo, false, false, 15);
            hBoxInfo.PackStart(new Label("Для заповненння нових документів та довідників"), false, false, 5);

            //Controls
            AddPointerControl(vBox, ОсновнаОрганізація);
            AddPointerControl(vBox, ОсновнийСклад);
            AddPointerControl(vBox, ОсновнаВалюта);
            AddPointerControl(vBox, ОсновнийПостачальник);
            AddPointerControl(vBox, ОсновнийПокупець);
            AddPointerControl(vBox, ОсновнаКаса);
            AddPointerControl(vBox, ОсновнаОдиницяПакування);
            AddPointerControl(vBox, ОсновнийПідрозділ);
            AddPointerControl(vBox, ОсновнийБанківськийРахунок);
            AddPointerControl(vBox, ОсновнийВидЦіни);
            AddPointerControl(vBox, ОсновнийВидЦіниЗакупівлі);
            AddPointerControl(vBox, ОсновнийВидНоменклатури);

            vBoxTop.PackStart(expanderConstDefault, false, false, 10);
        }

        void CreateJournalBlock(Box vBoxTop)
        {
            Box vBox = new Box(Orientation.Vertical, 0);

            Expander expander = new Expander("Журнали документів") { Expanded = true };
            expander.Add(vBox);

            //Controls
            AddCaptionAndControl(vBox, new Label("Період для журналів документів:"), ОсновнийТипПеріоду_ДляЖурналівДокументів);

            vBoxTop.PackStart(expander, false, false, 10);
        }

        void CreateLinkBlock(Box vBoxTop)
        {
            Box vBox = new Box(Orientation.Vertical, 0);

            Expander expander = new Expander("Додатково") { Expanded = true };
            expander.Add(vBox);

            AddLink(vBox, "Заповнення початковими даними", (object? sender, EventArgs args) =>
            {
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,"Початкове заповнення", () =>
                {
                    return new Обробка_ПочатковеЗаповнення();
                });
            });

            AddLink(vBox, "Завантаження курсів валют НБУ", (object? sender, EventArgs args) =>
            {
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,"Завантаження курсів валют НБУ", () =>
                {
                    return new Обробка_ЗавантаженняКурсівВалют();
                });
            });

            vBoxTop.PackStart(expander, false, false, 0);
        }

        void AddPointerControl(Box vBox, Widget wgPointerControl)
        {
            Box hBox = new Box(Orientation.Horizontal, 0) { Halign = Align.End };
            vBox.PackStart(hBox, false, false, 5);

            hBox.PackStart(wgPointerControl, false, false, 5);
        }

        void AddCaptionAndControl(Box vBox, Widget wgCaption, Widget wgControl)
        {
            Box hBox = new Box(Orientation.Horizontal, 0) { Halign = Align.Start };
            vBox.PackStart(hBox, false, false, 5);

            hBox.PackStart(wgCaption, false, false, 5);
            hBox.PackStart(wgControl, false, false, 5);
        }

        void AddControl(Box vBox, Widget wgControl)
        {
            Box hBox = new Box(Orientation.Horizontal, 0) { Halign = Align.Start };
            vBox.PackStart(hBox, false, false, 5);

            hBox.PackStart(wgControl, false, false, 5);
        }

        void AddLink(Box vbox, string uri, EventHandler? clickAction = null)
        {
            LinkButton lb = new LinkButton(uri, " " + uri) { Halign = Align.Start, Image = new Image(InterfaceGtk.Іконки.ДляКнопок.Doc), AlwaysShowImage = true };
            vbox.PackStart(lb, false, false, 0);

            if (clickAction != null)
                lb.Clicked += clickAction;
        }

        public void SetValue()
        {
            //
            //Значення за замовчуванням
            //

            ОсновнаОрганізація.Pointer = Константи.ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
            ОсновнийСклад.Pointer = Константи.ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const;
            ОсновнаВалюта.Pointer = Константи.ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
            ОсновнийПостачальник.Pointer = Константи.ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
            ОсновнийПокупець.Pointer = Константи.ЗначенняЗаЗамовчуванням.ОсновнийПокупець_Const;
            ОсновнаКаса.Pointer = Константи.ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const;
            ОсновнаОдиницяПакування.Pointer = Константи.ЗначенняЗаЗамовчуванням.ОсновнаОдиницяПакування_Const;
            ОсновнийПідрозділ.Pointer = Константи.ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
            ОсновнийБанківськийРахунок.Pointer = Константи.ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const;
            ОсновнийВидЦіни.Pointer = Константи.ЗначенняЗаЗамовчуванням.ОсновнийВидЦіни_Const;
            ОсновнийВидЦіниЗакупівлі.Pointer = Константи.ЗначенняЗаЗамовчуванням.ОсновнийВидЦіниЗакупівлі_Const;
            ОсновнийВидНоменклатури.Pointer = Константи.ЗначенняЗаЗамовчуванням.ОсновнийВидНоменклатури_Const;

            //
            //Системні
            //

            ВестиОблікПоСеріяхНоменклатури.Active = Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const;
            ВестиОблікПоХарактеристикахНоменклатури.Active = Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const;

            //ПартіїТоварів
            {
                МетодиСписанняПартій.ActiveId = Константи.ПартіїТоварів.МетодСписанняПартій_Const.ToString();

                if (МетодиСписанняПартій.Active == -1)
                    МетодиСписанняПартій.ActiveId = Перелічення.МетодиСписанняПартій.FIFO.ToString();
            }

            ЗупинитиФоновіЗадачі.Active = Константи.Системні.ЗупинитиФоновіЗадачі_Const;

            //
            //ЖурналиДокументів
            //

            {
                ОсновнийТипПеріоду_ДляЖурналівДокументів.ActiveId = Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const.ToString();

                if (ОсновнийТипПеріоду_ДляЖурналівДокументів.Active == -1)
                    ОсновнийТипПеріоду_ДляЖурналівДокументів.ActiveId = ПеріодДляЖурналу.ТипПеріоду.ВесьПеріод.ToString();
            }

            //
            //ЗавантаженняДанихІзСайтів
            //

            ЗавантаженняДанихІзСайтів.Text = Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Const;
            ЗавантаженняСпискуБанківІзСайтів.Text = Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняСпискуБанків_Const;
        }

        void GetValue()
        {
            NotebookFunction.SensitiveNotebookPageToCode(Program.GeneralNotebook, this.Name, false);

            //
            //Значення за замовчуванням
            //

            Константи.ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const = ОсновнаОрганізація.Pointer;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const = ОсновнийСклад.Pointer;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const = ОсновнаВалюта.Pointer;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const = ОсновнийПостачальник.Pointer;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнийПокупець_Const = ОсновнийПокупець.Pointer;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const = ОсновнаКаса.Pointer;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнаОдиницяПакування_Const = ОсновнаОдиницяПакування.Pointer;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const = ОсновнийПідрозділ.Pointer;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const = ОсновнийБанківськийРахунок.Pointer;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнийВидЦіни_Const = ОсновнийВидЦіни.Pointer;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнийВидЦіниЗакупівлі_Const = ОсновнийВидЦіниЗакупівлі.Pointer;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнийВидНоменклатури_Const = ОсновнийВидНоменклатури.Pointer;

            //
            //Системні
            //

            Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const = ВестиОблікПоСеріяхНоменклатури.Active;
            Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const = ВестиОблікПоХарактеристикахНоменклатури.Active;
            Константи.ПартіїТоварів.МетодСписанняПартій_Const = Enum.Parse<Перелічення.МетодиСписанняПартій>(МетодиСписанняПартій.ActiveId);
            Константи.Системні.ЗупинитиФоновіЗадачі_Const = ЗупинитиФоновіЗадачі.Active;

            //
            //ЖурналиДокументів
            //

            Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const = ОсновнийТипПеріоду_ДляЖурналівДокументів.ActiveId;

            //
            //ЗавантаженняДанихІзСайтів
            //

            Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Const = ЗавантаженняДанихІзСайтів.Text;
            Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняСпискуБанків_Const = ЗавантаженняСпискуБанківІзСайтів.Text;
 
            NotebookFunction.SensitiveNotebookPageToCode(Program.GeneralNotebook, this.Name, true);
        }

        void OnSaveClick(object? sender, EventArgs args)
        {
            GetValue();
        }
    }
}