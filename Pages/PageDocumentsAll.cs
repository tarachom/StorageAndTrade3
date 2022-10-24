using Gtk;

namespace StorageAndTrade
{
    class PageDocumentsAll : VBox
    {
        public FormStorageAndTrade? GeneralForm { get; set; }

        public PageDocumentsAll() : base()
        {
            //Кнопки
            HBox hBoxBotton = new HBox();

            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { GeneralForm?.CloseCurrentPageNotebook(); };

            hBoxBotton.PackStart(bClose, false, false, 10);

            PackStart(hBoxBotton, false, false, 10);

            //Список
            HBox hBoxList = new HBox(false, 0);

            VBox vLeft = new VBox(false, 0);

            AddLink(vLeft, "Замовлення постачальнику", ЗамовленняПостачальнику);
            AddLink(vLeft, "Поступлення товарів та послуг", ПоступленняТоварівТаПослуг);
            // AddLink(vLeft, "Валюти", Валюти);
            // AddLink(vLeft, "Каси", Каси);
            // AddLink(vLeft, "Види цін", ВидиЦін);
            // AddLink(vLeft, "Банківські рахунки організацій", БанківськіРахункиОрганізацій);
            // AddLink(vLeft, "Структура підприємства", СтруктураПідприємства);

            hBoxList.PackStart(vLeft, false, false, 5);

            // AddSeparator(hBoxList);

            // VBox vRight = new VBox(false, 0);

            // AddCaption(vRight, "Партнери");
            // AddLink(vRight, "Контрагенти", Контрагенти);
            // AddLink(vRight, "Договори контрагентів", ДоговориКонтрагентів);
            // AddLink(vRight, "Банківські рахунки контрагентів", БанківськіРахункиКонтрагентів);

            // AddCaption(vRight, "Товари та послуги");
            // AddLink(vRight, "Номенклатура", Номенклатура);
            // AddLink(vRight, "Характеристики номенклатури", ХарактеристикаНоменклатури);
            // AddLink(vRight, "Пакування номенклатури", ПакуванняОдиниціВиміру);
            // AddLink(vRight, "Види номенклатури", ВидиНоменклатури);
            // AddLink(vRight, "Партії товарів", ПартіяТоварівКомпозит);
            // AddLink(vRight, "Серії номенклатури", СеріїНоменклатури);
            // AddLink(vRight, "Виробники", Виробники);

            // hBoxList.PackStart(vRight, false, false, 5);

            PackStart(hBoxList, false, false, 10);

            ShowAll();
        }

        void ЗамовленняПостачальнику(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Документи: Замовлення постачальнику", () =>
            {
                ЗамовленняПостачальнику page = new ЗамовленняПостачальнику
                {
                    GeneralForm = GeneralForm
                };

                page.LoadRecords();

                return page;
            });
        }

        void ПоступленняТоварівТаПослуг(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Документи: Поступлення товарів та послуг", () =>
            {
                ПоступленняТоварівТаПослуг page = new ПоступленняТоварівТаПослуг
                {
                    GeneralForm = GeneralForm
                };

                page.LoadRecords();

                return page;
            });
        }



        void AddCaption(VBox vBox, string name)
        {
            Label caption = new Label(name);
            vBox.PackStart(caption, false, false, 10);
        }

        void AddSeparator(HBox hbox)
        {
            Separator separator = new Separator(Orientation.Horizontal);
            hbox.PackStart(separator, false, false, 5);
        }

        void AddLink(VBox vbox, string uri, EventHandler? clickAction = null)
        {
            LinkButton lb = new LinkButton(uri) { Halign = Align.Start };
            vbox.PackStart(lb, false, false, 0);

            if (clickAction != null)
                lb.Clicked += clickAction;
        }

    }
}