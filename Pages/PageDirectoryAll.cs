using Gtk;

namespace StorageAndTrade
{
    class PageDirectoryAll : VBox
    {
        public FormStorageAndTrade? GeneralForm { get; set; }

        public PageDirectoryAll() : base()
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

            AddCaption(vLeft, "Підприємство");
            AddLink(vLeft, "Організації", Організація);
            AddLink(vLeft, "Склади", Склади);
            AddLink(vLeft, "Валюти", Валюти);
            AddLink(vLeft, "Каси", Каси);
            AddLink(vLeft, "Види цін", ВидиЦін);
            AddLink(vLeft, "Банківські рахунки організацій", БанківськіРахункиОрганізацій);
            AddLink(vLeft, "Структура підприємства", СтруктураПідприємства);

            AddCaption(vLeft, "Додаткові");
            AddLink(vLeft, "Користувачі", Користувачі);
            AddLink(vLeft, "Фізичні особи", ФізичніОсоби);
            AddLink(vLeft, "Файли", Файли);

            hBoxList.PackStart(vLeft, false, false, 5);

            AddSeparator(hBoxList);

            VBox vRight = new VBox(false, 0);

            AddCaption(vRight, "Партнери");
            AddLink(vRight, "Контрагенти", Контрагенти);
            AddLink(vRight, "Договори контрагентів", ДоговориКонтрагентів);
            AddLink(vRight, "Банківські рахунки контрагентів", БанківськіРахункиКонтрагентів);

            AddCaption(vRight, "Товари та послуги");
            AddLink(vRight, "Номенклатура", Номенклатура);
            AddLink(vRight, "Характеристики номенклатури", ХарактеристикаНоменклатури);
            AddLink(vRight, "Пакування номенклатури", ПакуванняОдиниціВиміру);
            AddLink(vRight, "Види номенклатури", ВидиНоменклатури);
            AddLink(vRight, "Партії товарів", ПартіяТоварівКомпозит);
            AddLink(vRight, "Серії номенклатури", СеріїНоменклатури);
            AddLink(vRight, "Виробники", Виробники);

            hBoxList.PackStart(vRight, false, false, 5);

            PackStart(hBoxList, false, false, 10);

            ShowAll();
        }

        void Організація(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Довідник: Організації", () =>
            {
                Організації page = new Організації();

                page.LoadRecords();

                return page;
            });
        }

        void Номенклатура(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Довідник: Номенклатура", () =>
            {
                Номенклатура page = new Номенклатура
                {
                    GeneralForm = GeneralForm
                };

                page.LoadRecords();

                return page;
            });
        }

        void ХарактеристикаНоменклатури(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Довідник: Характеристики номенклатури", () =>
            {
                ХарактеристикиНоменклатури page = new ХарактеристикиНоменклатури
                {
                    GeneralForm = GeneralForm
                };

                page.LoadRecords();

                return page;
            });
        }

        void Контрагенти(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Довідник: Контрагенти", () =>
            {
                Контрагенти page = new Контрагенти();

                page.LoadRecords();

                return page;
            });
        }

        void Склади(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Довідник: Склади", () =>
            {
                Склади page = new Склади
                {
                    GeneralForm = GeneralForm
                };

                page.LoadRecords();

                return page;
            });
        }

        void Валюти(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Довідник: Валюти", () =>
            {
                Валюти page = new Валюти();

                page.LoadRecords();

                return page;
            });
        }

        void Каси(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Довідник: Каси", () =>
            {
                Каси page = new Каси
                {
                    GeneralForm = GeneralForm
                };

                page.LoadRecords();

                return page;
            });
        }

        void Користувачі(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Довідник: Користувачі", () =>
            {
                Користувачі page = new Користувачі
                {
                    GeneralForm = GeneralForm
                };

                page.LoadRecords();

                return page;
            });
        }

        void Файли(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Довідник: Користувачі", () =>
            {
                Файли page = new Файли
                {
                    GeneralForm = GeneralForm
                };

                page.LoadRecords();

                return page;
            });
        }

        void ВидиЦін(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Довідник: Види цін", () =>
            {
                ВидиЦін page = new ВидиЦін
                {
                    GeneralForm = GeneralForm
                };

                page.LoadRecords();

                return page;
            });
        }

        void БанківськіРахункиОрганізацій(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Довідник: Банківські рахунки організацій", () =>
            {
                БанківськіРахункиОрганізацій page = new БанківськіРахункиОрганізацій
                {
                    GeneralForm = GeneralForm
                };

                page.LoadRecords();

                return page;
            });
        }

        void ФізичніОсоби(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Довідник: Фізичні особи", () =>
            {
                ФізичніОсоби page = new ФізичніОсоби
                {
                    GeneralForm = GeneralForm
                };

                page.LoadRecords();

                return page;
            });
        }

        void СтруктураПідприємства(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Довідник: Структура підприємства", () =>
            {
                СтруктураПідприємства page = new СтруктураПідприємства
                {
                    GeneralForm = GeneralForm
                };

                page.LoadRecords();

                return page;
            });
        }

        void ДоговориКонтрагентів(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Довідник: договори контрагентів", () =>
            {
                ДоговориКонтрагентів page = new ДоговориКонтрагентів
                {
                    GeneralForm = GeneralForm
                };

                page.LoadRecords();

                return page;
            });
        }

        void БанківськіРахункиКонтрагентів(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Довідник: Банківські рахунки контрагентів", () =>
            {
                БанківськіРахункиКонтрагентів page = new БанківськіРахункиКонтрагентів();

                page.LoadRecords();

                return page;
            });
        }

        void ПакуванняОдиниціВиміру(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Довідник: Пакування номенклатури", () =>
            {
                ПакуванняОдиниціВиміру page = new ПакуванняОдиниціВиміру
                {
                    GeneralForm = GeneralForm
                };

                page.LoadRecords();

                return page;
            });
        }

        void ВидиНоменклатури(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Довідник: Види номенклатури", () =>
            {
                ВидиНоменклатури page = new ВидиНоменклатури
                {
                    GeneralForm = GeneralForm
                };

                page.LoadRecords();

                return page;
            });
        }

        void ПартіяТоварівКомпозит(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Довідник: Партія товарів", () =>
            {
                ПартіяТоварівКомпозит page = new ПартіяТоварівКомпозит
                {
                    GeneralForm = GeneralForm
                };

                page.LoadRecords();

                return page;
            });
        }

        void СеріїНоменклатури(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Довідник: Серії номенклатури", () =>
            {
                СеріїНоменклатури page = new СеріїНоменклатури
                {
                    GeneralForm = GeneralForm
                };

                page.LoadRecords();

                return page;
            });
        }

        void Виробники(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Довідник: Виробники", () =>
            {
                Виробники page = new Виробники
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
            LinkButton lb = new LinkButton(" " + uri) { Halign = Align.Start, Image = new Image("doc.png"), AlwaysShowImage = true };
            vbox.PackStart(lb, false, false, 0);

            if (clickAction != null)
                lb.Clicked += clickAction;
        }

    }
}