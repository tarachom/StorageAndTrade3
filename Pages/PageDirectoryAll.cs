using Gtk;
using System;
using System.IO;

using AccountingSoftware;

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
            AddLink(vLeft, "Організації", "1", Організація);
            AddLink(vLeft, "Номенклатура", "1", Номенклатура);
            AddLink(vLeft, "Характеристики номенклатури", "1", ХарактеристикаНоменклатури);
            AddLink(vLeft, "Контрагенти", "1", Контрагенти);
            AddLink(vLeft, "Склади", "1", Склади);
            AddLink(vLeft, "Валюти", "1", Валюти);
            AddLink(vLeft, "Каси", "1");
            AddLink(vLeft, "Користувачі", "1");
            AddLink(vLeft, "Файли", "1");

            hBoxList.PackStart(vLeft, false, false, 5);

            AddSeparator(hBoxList);

            VBox vRight = new VBox(false, 0);
            AddLink(vRight, "Види номенклатури", "2");
            AddLink(vRight, "Банківські рахунки контрагентів", "2");
            AddLink(vRight, "Банківські рахунки організацій", "2");
            AddLink(vRight, "Види цін", "2");
            AddLink(vRight, "Виробники", "2");
            AddLink(vRight, "Договори контрагентів", "2");
            AddLink(vRight, "Пакування одиниці виміру", "2");
            AddLink(vRight, "Партії товарів", "2");
            AddLink(vRight, "Серії номенклатури", "2");
            AddLink(vRight, "Структура підприємства", "2");
            AddLink(vRight, "Фізичні особи", "2");
            hBoxList.PackStart(vRight, false, false, 5);

            PackStart(hBoxList, false, false, 10);

            ShowAll();
        }

        void Організація(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Довідник: Організації", () =>
            {
                Організації page = new Організації
                {
                    GeneralForm = GeneralForm
                };

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
                Контрагенти page = new Контрагенти
                {
                    GeneralForm = GeneralForm
                };

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
                Валюти page = new Валюти
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

        void AddLink(VBox vbox, string uri, string name, EventHandler? clickAction = null)
        {
            LinkButton lb = new LinkButton(uri) { Name = name, Halign = Align.Start };
            vbox.PackStart(lb, false, false, 0);

            if (clickAction != null)
                lb.Clicked += clickAction;
        }

    }
}