using Gtk;

using Константи = StorageAndTrade_1_0.Константи;

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
        Контрагенти_PointerControl ОсновнийПостачальник = new Контрагенти_PointerControl() { Caption = "Постачальник" };
        Контрагенти_PointerControl ОсновнийПокупець = new Контрагенти_PointerControl() { Caption = "Покупець" };
        Каси_PointerControl ОсновнаКаса = new Каси_PointerControl();
        ПакуванняОдиниціВиміру_PointerControl ОсновнаОдиницяПакування = new ПакуванняОдиниціВиміру_PointerControl() { Caption = "Пакування" };
        СтруктураПідприємства_PointerControl ОсновнийПідрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ" };
        БанківськіРахункиОрганізацій_PointerControl ОсновнийБанківськийРахунок = new БанківськіРахункиОрганізацій_PointerControl();
        ВидиЦін_PointerControl ОсновнийВидЦіни = new ВидиЦін_PointerControl();

        //
        //Системні
        //

        //Узгоджений
        CheckButton ВестиОблікПоХарактеристикахНоменклатури = new CheckButton("Вести облік по характеристиках номенклатури");
        CheckButton ВестиОблікПоСеріяхНоменклатури = new CheckButton("Вести облік по серіях номенклатури");

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

            HPaned hPaned = new HPaned() { BorderWidth = 5, Position = 500 };

            CreatePack1(hPaned);
            CreatePack2(hPaned);

            PackStart(hPaned, false, false, 5);

            ShowAll();
        }

        void CreatePack1(HPaned hPaned)
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

            hPaned.Pack1(expanderConstDefault, false, false);
        }

        void AddPointerControl(VBox vBox, Widget wgPointerControl)
        {
            HBox hBox = new HBox() { Halign = Align.End };
            vBox.PackStart(hBox, false, false, 5);

            hBox.PackStart(wgPointerControl, false, false, 5);
        }

        void AddControl(VBox vBox, Widget wgControl)
        {
            HBox hBox = new HBox() { Halign = Align.Start };
            vBox.PackStart(hBox, false, false, 5);

            hBox.PackStart(wgControl, false, false, 5);
        }

        void CreatePack2(HPaned hPaned)
        {
            VBox vBox = new VBox();

            Expander expanderSystem = new Expander("Налаштування обліку") { Expanded = true };
            expanderSystem.Add(vBox);

            //Info
            HBox hBoxInfo = new HBox() { Halign = Align.Start };
            vBox.PackStart(hBoxInfo, false, false, 15);

            hBoxInfo.PackStart(new Label("Видимість колонок у документах і звітах"), false, false, 5);

            //Controls
            AddControl(vBox, ВестиОблікПоХарактеристикахНоменклатури);
            AddControl(vBox, ВестиОблікПоСеріяхНоменклатури);

            hPaned.Pack2(expanderSystem, false, false);
        }

        public void SetValue()
        {
            //
            //Значення за замовчуванням
            //

            ОсновнаОрганізація.Pointer = Константи.ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const;
            ОсновнийСклад.Pointer = Константи.ЗначенняЗаЗамовчуванням.ОснонийСклад_Const;
            ОсновнаВалюта.Pointer = Константи.ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const;
            ОсновнийПостачальник.Pointer = Константи.ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const;
            ОсновнийПокупець.Pointer = Константи.ЗначенняЗаЗамовчуванням.ОсновнийПокупець_Const;
            ОсновнаКаса.Pointer = Константи.ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const;
            ОсновнаОдиницяПакування.Pointer = Константи.ЗначенняЗаЗамовчуванням.ОсновнаОдиницяПакування_Const;
            ОсновнийПідрозділ.Pointer = Константи.ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const;
            ОсновнийБанківськийРахунок.Pointer = Константи.ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const;
            ОсновнийВидЦіни.Pointer = Константи.ЗначенняЗаЗамовчуванням.ОсновнийВидЦіни_Const;

            //
            //Значення за замовчуванням
            //

            ВестиОблікПоСеріяхНоменклатури.Active = Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const;
            ВестиОблікПоХарактеристикахНоменклатури.Active = Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const;
        }

        void GetValue()
        {
            //
            //Значення за замовчуванням
            //

            Константи.ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const = ОсновнаОрганізація.Pointer;
            Константи.ЗначенняЗаЗамовчуванням.ОснонийСклад_Const = ОсновнийСклад.Pointer;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const = ОсновнаВалюта.Pointer;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const = ОсновнийПостачальник.Pointer;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнийПокупець_Const = ОсновнийПокупець.Pointer;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const = ОсновнаКаса.Pointer;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнаОдиницяПакування_Const = ОсновнаОдиницяПакування.Pointer;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнийПідрозділ_Const = ОсновнийПідрозділ.Pointer;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнийБанківськийРахунок_Const = ОсновнийБанківськийРахунок.Pointer;
            Константи.ЗначенняЗаЗамовчуванням.ОсновнийВидЦіни_Const = ОсновнийВидЦіни.Pointer;

            //
            //Значення за замовчуванням
            //

            Константи.Системні.ВестиОблікПоСеріяхНоменклатури_Const = ВестиОблікПоСеріяхНоменклатури.Active;
            Константи.Системні.ВестиОблікПоХарактеристикахНоменклатури_Const = ВестиОблікПоХарактеристикахНоменклатури.Active;
        }

        void OnSaveClick(object? sender, EventArgs args)
        {
            GetValue();
        }
    }
}