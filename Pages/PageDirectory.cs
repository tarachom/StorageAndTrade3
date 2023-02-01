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

/*

Довідники

*/

using Gtk;

namespace StorageAndTrade
{
    class PageDirectory : VBox
    {
        public PageDirectory() : base()
        {
            //Кнопки
            HBox hBoxBotton = new HBox();

            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

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
            AddLink(vLeft, "Статті руху коштів", СтаттіРухуКоштів);

            AddCaption(vLeft, "Адресне зберігання");
            AddLink(vLeft, "Складські приміщення", СкладськіПриміщення);
            AddLink(vLeft, "Складські комірки", СкладськіКомірки);
            AddLink(vLeft, "Типорозміри комірок", ТипорозміриКомірок);

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
            Program.GeneralForm?.CreateNotebookPage("Організації", () =>
            {
                Організації page = new Організації();
                page.LoadRecords();
                return page;
            });
        }

        void Номенклатура(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Номенклатура", () =>
            {
                Номенклатура page = new Номенклатура();
                page.LoadTree();
                return page;
            });
        }

        void ХарактеристикаНоменклатури(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Характеристики номенклатури", () =>
            {
                ХарактеристикиНоменклатури page = new ХарактеристикиНоменклатури();
                page.LoadRecords();
                return page;
            });
        }

        void Контрагенти(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Контрагенти", () =>
            {
                Контрагенти page = new Контрагенти();
                page.LoadTree();
                return page;
            });
        }

        void Склади(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Склади", () =>
            {
                Склади page = new Склади();
                page.LoadTree();
                return page;
            });
        }

        public static void Валюти(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Валюти", () =>
            {
                Валюти page = new Валюти();
                page.LoadRecords();
                return page;
            });
        }

        void Каси(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Каси", () =>
            {
                Каси page = new Каси();
                page.LoadRecords();
                return page;
            });
        }

        void Користувачі(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Користувачі", () =>
            {
                Користувачі page = new Користувачі();
                page.LoadRecords();
                return page;
            });
        }

        void Файли(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Користувачі", () =>
            {
                Файли page = new Файли();
                page.LoadRecords();
                return page;
            });
        }

        void ВидиЦін(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Види цін", () =>
            {
                ВидиЦін page = new ВидиЦін();
                page.LoadRecords();
                return page;
            });
        }

        void БанківськіРахункиОрганізацій(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Банківські рахунки організацій", () =>
            {
                БанківськіРахункиОрганізацій page = new БанківськіРахункиОрганізацій();
                page.LoadRecords();
                return page;
            });
        }

        void ФізичніОсоби(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Фізичні особи", () =>
            {
                ФізичніОсоби page = new ФізичніОсоби();
                page.LoadRecords();
                return page;
            });
        }

        void СтруктураПідприємства(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Структура підприємства", () =>
            {
                СтруктураПідприємства page = new СтруктураПідприємства();
                page.LoadRecords();
                return page;
            });
        }

        void ДоговориКонтрагентів(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("договори контрагентів", () =>
            {
                ДоговориКонтрагентів page = new ДоговориКонтрагентів();
                page.LoadRecords();
                return page;
            });
        }

        void БанківськіРахункиКонтрагентів(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Банківські рахунки контрагентів", () =>
            {
                БанківськіРахункиКонтрагентів page = new БанківськіРахункиКонтрагентів();
                page.LoadRecords();
                return page;
            });
        }

        void ПакуванняОдиниціВиміру(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Пакування номенклатури", () =>
            {
                ПакуванняОдиниціВиміру page = new ПакуванняОдиниціВиміру();
                page.LoadRecords();
                return page;
            });
        }

        void ВидиНоменклатури(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Види номенклатури", () =>
            {
                ВидиНоменклатури page = new ВидиНоменклатури();
                page.LoadRecords();
                return page;
            });
        }

        void ПартіяТоварівКомпозит(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Партія товарів", () =>
            {
                ПартіяТоварівКомпозит page = new ПартіяТоварівКомпозит();
                page.LoadRecords();
                return page;
            });
        }

        void СеріїНоменклатури(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Серії номенклатури", () =>
            {
                СеріїНоменклатури page = new СеріїНоменклатури();
                page.LoadRecords();
                return page;
            });
        }

        void Виробники(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Виробники", () =>
            {
                Виробники page = new Виробники();
                page.LoadRecords();
                return page;
            });
        }

        void СтаттіРухуКоштів(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Статті руху коштів", () =>
            {
                СтаттяРухуКоштів page = new СтаттяРухуКоштів();
                page.LoadRecords();
                return page;
            });
        }

        void СкладськіПриміщення(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Складські приміщення", () =>
            {
                СкладськіПриміщення page = new СкладськіПриміщення();
                page.LoadRecords();
                return page;
            });
        }

        void СкладськіКомірки(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Складські комірки", () =>
            {
                СкладськіКомірки page = new СкладськіКомірки();
                page.LoadTree();
                return page;
            });
        }

        void ТипорозміриКомірок(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Типорозміри комірок", () =>
            {
                ТипорозміриКомірок page = new ТипорозміриКомірок();
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
            LinkButton lb = new LinkButton(uri, " " + uri) { Halign = Align.Start, Image = new Image("images/doc.png"), AlwaysShowImage = true };
            vbox.PackStart(lb, false, false, 0);

            if (clickAction != null)
                lb.Clicked += clickAction;
        }
    }
}