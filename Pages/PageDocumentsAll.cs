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

            AddCaption(vLeft, "Продажі");
            AddLink(vLeft, "Замовлення клієнта", ЗамовленняКлієнта);
            AddLink(vLeft, "Рахунок фактура", РахунокФактура);
            AddLink(vLeft, "Реалізація товарів та послуг", РеалізаціяТоварівТаПослуг);
            AddLink(vLeft, "Акт виконаних робіт", АктВиконанихРобіт);
            AddLink(vLeft, "Повернення товарів від клієнта", ПоверненняТоварівВідКлієнта);

            AddCaption(vLeft, "Закупки");
            AddLink(vLeft, "Замовлення постачальнику", ЗамовленняПостачальнику);
            AddLink(vLeft, "Поступлення товарів та послуг", ПоступленняТоварівТаПослуг);
            AddLink(vLeft, "Повернення товарів постачальнику", ПоверненняТоварівПостачальнику);

            hBoxList.PackStart(vLeft, false, false, 5);

            AddSeparator(hBoxList);

            VBox vRight = new VBox(false, 0);

            AddCaption(vRight, "Каса");
            AddLink(vRight, "Прихідний касовий ордер", ПрихіднийКасовийОрдер);
            AddLink(vRight, "Розхідний касовий ордер", РозхіднийКасовийОрдер);

            AddCaption(vRight, "Склад");
            AddLink(vRight, "Переміщення товарів", ПереміщенняТоварів);
            AddLink(vRight, "Введення залишків", ВведенняЗалишків);
            AddLink(vRight, "Встановлення цін номенклатури", ВстановленняЦінНоменклатури);
            AddLink(vRight, "Внутрішнє споживання товарів", ВнутрішнєСпоживанняТоварів);

            hBoxList.PackStart(vRight, false, false, 5);

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

                page.SetValue();

                return page;
            });
        }

        void ПоступленняТоварівТаПослуг(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Документи: Поступлення товарів та послуг", () =>
            {
                ПоступленняТоварівТаПослуг page = new ПоступленняТоварівТаПослуг();

                page.SetValue();

                return page;
            });
        }

        void ЗамовленняКлієнта(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Документи: Замовлення клієнта", () =>
            {
                ЗамовленняКлієнта page = new ЗамовленняКлієнта
                {
                    GeneralForm = GeneralForm
                };

                page.SetValue();

                return page;
            });
        }

        void РеалізаціяТоварівТаПослуг(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Документи: Реалізація товарів та послуг", () =>
            {
                РеалізаціяТоварівТаПослуг page = new РеалізаціяТоварівТаПослуг
                {
                    GeneralForm = GeneralForm
                };

                page.SetValue();

                return page;
            });
        }

        void ВстановленняЦінНоменклатури(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Документи: Встановлення цін номенклатури", () =>
            {
                ВстановленняЦінНоменклатури page = new ВстановленняЦінНоменклатури
                {
                    GeneralForm = GeneralForm
                };

                page.SetValue();

                return page;
            });
        }

        void ПрихіднийКасовийОрдер(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Документи: Прихідний касовий ордер", () =>
            {
                ПрихіднийКасовийОрдер page = new ПрихіднийКасовийОрдер
                {
                    GeneralForm = GeneralForm
                };

                page.SetValue();

                return page;
            });
        }

        void РозхіднийКасовийОрдер(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Документи: Розхідний касовий ордер", () =>
            {
                РозхіднийКасовийОрдер page = new РозхіднийКасовийОрдер
                {
                    GeneralForm = GeneralForm
                };

                page.SetValue();

                return page;
            });
        }

        void ПереміщенняТоварів(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Документи: Переміщення товарів", () =>
            {
                ПереміщенняТоварів page = new ПереміщенняТоварів
                {
                    GeneralForm = GeneralForm
                };

                page.SetValue();

                return page;
            });
        }

        void ПоверненняТоварівПостачальнику(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Документи: Повернення товарів постачальнику", () =>
            {
                ПоверненняТоварівПостачальнику page = new ПоверненняТоварівПостачальнику
                {
                    GeneralForm = GeneralForm
                };

                page.SetValue();

                return page;
            });
        }

        void ПоверненняТоварівВідКлієнта(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Документи: Повернення товарів від клієнта", () =>
            {
                ПоверненняТоварівВідКлієнта page = new ПоверненняТоварівВідКлієнта
                {
                    GeneralForm = GeneralForm
                };

                page.SetValue();

                return page;
            });
        }

        void АктВиконанихРобіт(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Документи: Акт виконаних робіт", () =>
            {
                АктВиконанихРобіт page = new АктВиконанихРобіт
                {
                    GeneralForm = GeneralForm
                };

                page.SetValue();

                return page;
            });
        }

        void ВведенняЗалишків(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Документи: Введення залишків", () =>
            {
                ВведенняЗалишків page = new ВведенняЗалишків
                {
                    GeneralForm = GeneralForm
                };

                page.SetValue();

                return page;
            });
        }

        void ВнутрішнєСпоживанняТоварів(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Документи: Внутрішнє споживання товарів", () =>
            {
                ВнутрішнєСпоживанняТоварів page = new ВнутрішнєСпоживанняТоварів
                {
                    GeneralForm = GeneralForm
                };

                page.SetValue();

                return page;
            });
        }

        void РахунокФактура(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Документи: Рахунок фактура", () =>
            {
                РахунокФактура page = new РахунокФактура
                {
                    GeneralForm = GeneralForm
                };

                page.SetValue();

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
            LinkButton lb = new LinkButton(" " + uri) { Halign = Align.Start, Image = new Image("images/doc.png"), AlwaysShowImage = true };
            vbox.PackStart(lb, false, false, 0);

            if (clickAction != null)
                lb.Clicked += clickAction;
        }

    }
}