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
using Константи = StorageAndTrade_1_0.Константи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class PageSettings : VBox
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
        ВидиЦін_PointerControl ОсновнийВидЦіни = new ВидиЦін_PointerControl();
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

        ComboBoxText ОсновнийТипПеріоду_ДляЖурналівДокументів = new ComboBoxText();

        #endregion

        public PageSettings() : base()
        {
            //Кнопки
            HBox hBox = new HBox();

            Button bSave = new Button("Зберегти");
            bSave.Clicked += OnSaveClick;

            hBox.PackStart(bSave, false, false, 10);

            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

            hBox.PackStart(bClose, false, false, 10);

            PackStart(hBox, false, false, 10);

            FillComboBoxes();

            HPaned hPaned = new HPaned() { BorderWidth = 5, Position = 500 };

            CreatePack1(hPaned);
            CreatePack2(hPaned);

            PackStart(hPaned, false, false, 5);

            ShowAll();
        }

        void FillComboBoxes()
        {
            foreach (ConfigurationEnumField field in Config.Kernel!.Conf.Enums["ТипПеріодуДляЖурналівДокументів"].Fields.Values)
                ОсновнийТипПеріоду_ДляЖурналівДокументів.Append(field.Name, field.Desc);

            foreach (ConfigurationEnumField field in Config.Kernel!.Conf.Enums["МетодиСписанняПартій"].Fields.Values)
                МетодиСписанняПартій.Append(field.Name, field.Desc);
        }

        void CreatePack1(HPaned hPaned)
        {
            VBox vBox = new VBox();

            CreateDefaultBlock(vBox);

            vBox.PackStart(new Separator(Orientation.Horizontal), false, false, 10);

            CreateJournalBlock(vBox);

            hPaned.Pack1(vBox, false, false);
        }

        void CreatePack2(HPaned hPaned)
        {
            VBox vBox = new VBox();
            hPaned.Pack2(vBox, false, false);

            //
            //Системні
            //

            //1
            {
                VBox vBoxSystem = new VBox();

                Expander expanderSystem = new Expander("Налаштування обліку") { Expanded = true };
                expanderSystem.Add(vBoxSystem);

                //Info
                HBox hBoxInfo = new HBox() { Halign = Align.Start };
                vBoxSystem.PackStart(hBoxInfo, false, false, 15);
                hBoxInfo.PackStart(new Label("Видимість колонок у документах і звітах"), false, false, 5);

                //Controls
                AddControl(vBoxSystem, ВестиОблікПоХарактеристикахНоменклатури);
                AddControl(vBoxSystem, ВестиОблікПоСеріяхНоменклатури);

                vBox.PackStart(expanderSystem, false, false, 10);
            }

            //2
            {
                VBox vBoxBatch = new VBox();

                Expander expanderBatch = new Expander("Партії товарів") { Expanded = true };
                expanderBatch.Add(vBoxBatch);

                //Info
                HBox hBoxInfo = new HBox() { Halign = Align.Start };
                vBoxBatch.PackStart(hBoxInfo, false, false, 15);
                hBoxInfo.PackStart(new Label("Метод списання партій товарів"), false, false, 5);

                //Controls
                AddControl(vBoxBatch, МетодиСписанняПартій);

                vBox.PackStart(expanderBatch, false, false, 10);
            }

            //3
            {
                VBox vBoxBackgroundTask = new VBox();

                Expander expanderBackgroundTask = new Expander("Фонові обчислення") { Expanded = false };
                expanderBackgroundTask.Add(vBoxBackgroundTask);

                //Info
                HBox hBoxInfo = new HBox() { Halign = Align.Start };
                vBoxBackgroundTask.PackStart(hBoxInfo, false, false, 15);
                hBoxInfo.PackStart(new Label("Обчислення віртуальних залишків по регістрах накопичення") { Wrap = true }, false, false, 5);

                //Controls
                AddControl(vBoxBackgroundTask, ЗупинитиФоновіЗадачі);

                vBox.PackStart(expanderBackgroundTask, false, false, 10);
            }
        }

        //Значення за замовчування
        void CreateDefaultBlock(VBox vBoxTop)
        {
            VBox vBox = new VBox();

            Expander expanderConstDefault = new Expander("Значення за замовчуванням") { Expanded = true };
            expanderConstDefault.Add(vBox);

            //Info
            HBox hBoxInfo = new HBox() { Halign = Align.Start };
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
            AddPointerControl(vBox, ОсновнийВидНоменклатури);

            vBoxTop.PackStart(expanderConstDefault, false, false, 10);
        }

        void CreateJournalBlock(VBox vBoxTop)
        {
            VBox vBox = new VBox();

            Expander expander = new Expander("Журнали документів") { Expanded = true };
            expander.Add(vBox);

            //Controls
            AddCaptionAndControl(vBox, new Label("Період для журналів документів:"), ОсновнийТипПеріоду_ДляЖурналівДокументів);

            vBoxTop.PackStart(expander, false, false, 10);
        }

        void AddPointerControl(VBox vBox, Widget wgPointerControl)
        {
            HBox hBox = new HBox() { Halign = Align.End };
            vBox.PackStart(hBox, false, false, 5);

            hBox.PackStart(wgPointerControl, false, false, 5);
        }

        void AddCaptionAndControl(VBox vBox, Widget wgCaption, Widget wgControl)
        {
            HBox hBox = new HBox() { Halign = Align.Start };
            vBox.PackStart(hBox, false, false, 5);

            hBox.PackStart(wgCaption, false, false, 5);
            hBox.PackStart(wgControl, false, false, 5);
        }

        void AddControl(VBox vBox, Widget wgControl)
        {
            HBox hBox = new HBox() { Halign = Align.Start };
            vBox.PackStart(hBox, false, false, 5);

            hBox.PackStart(wgControl, false, false, 5);
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
                    ОсновнийТипПеріоду_ДляЖурналівДокументів.ActiveId = Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуМісяця.ToString();
            }
        }

        void GetValue()
        {
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

            Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const = Enum.Parse<Перелічення.ТипПеріодуДляЖурналівДокументів>(ОсновнийТипПеріоду_ДляЖурналівДокументів.ActiveId);
        }

        void OnSaveClick(object? sender, EventArgs args)
        {
            GetValue();
        }
    }
}