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

namespace StorageAndTrade
{
    class PageDocuments : VBox
    {
        public PageDocuments() : base()
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
            AddLink(vRight, "Розміщення товарів на складі", РозміщенняТоварівНаСкладі);
            AddLink(vRight, "Переміщення товарів на складі", ПереміщенняТоварівНаСкладі);
            AddLink(vRight, "Збірка товарів на складі", ЗбіркаТоварівНаСкладі);

            hBoxList.PackStart(vRight, false, false, 5);

            PackStart(hBoxList, false, false, 10);

            ShowAll();
        }

        void ЗамовленняПостачальнику(object? sender, EventArgs args)
        {
            ЗамовленняПостачальнику page = new ЗамовленняПостачальнику();
            Program.GeneralForm?.CreateNotebookPage("Замовлення постачальнику", () => { return page; });
            page.SetValue();
        }

        void ПоступленняТоварівТаПослуг(object? sender, EventArgs args)
        {
            ПоступленняТоварівТаПослуг page = new ПоступленняТоварівТаПослуг();
            Program.GeneralForm?.CreateNotebookPage("Поступлення товарів та послуг", () => { return page; });
            page.SetValue();
        }

        void ЗамовленняКлієнта(object? sender, EventArgs args)
        {
            ЗамовленняКлієнта page = new ЗамовленняКлієнта();
            Program.GeneralForm?.CreateNotebookPage("Замовлення клієнта", () => { return page; });
            page.SetValue();
        }

        void РеалізаціяТоварівТаПослуг(object? sender, EventArgs args)
        {
            РеалізаціяТоварівТаПослуг page = new РеалізаціяТоварівТаПослуг();
            Program.GeneralForm?.CreateNotebookPage("Реалізація товарів та послуг", () => { return page; });
            page.SetValue();
        }

        void ВстановленняЦінНоменклатури(object? sender, EventArgs args)
        {
            ВстановленняЦінНоменклатури page = new ВстановленняЦінНоменклатури();
            Program.GeneralForm?.CreateNotebookPage("Встановлення цін номенклатури", () => { return page; });
            page.SetValue();
        }

        void ПрихіднийКасовийОрдер(object? sender, EventArgs args)
        {
            ПрихіднийКасовийОрдер page = new ПрихіднийКасовийОрдер();
            Program.GeneralForm?.CreateNotebookPage("Прихідний касовий ордер", () => { return page; });
            page.SetValue();
        }

        void РозхіднийКасовийОрдер(object? sender, EventArgs args)
        {
            РозхіднийКасовийОрдер page = new РозхіднийКасовийОрдер();
            Program.GeneralForm?.CreateNotebookPage("Розхідний касовий ордер", () => { return page; });
            page.SetValue();
        }

        void ПереміщенняТоварів(object? sender, EventArgs args)
        {
            ПереміщенняТоварів page = new ПереміщенняТоварів();
            Program.GeneralForm?.CreateNotebookPage("Переміщення товарів", () => { return page; });
            page.SetValue();
        }

        void ПоверненняТоварівПостачальнику(object? sender, EventArgs args)
        {
            ПоверненняТоварівПостачальнику page = new ПоверненняТоварівПостачальнику();
            Program.GeneralForm?.CreateNotebookPage("Повернення товарів постачальнику", () => { return page; });
            page.SetValue();
        }

        void ПоверненняТоварівВідКлієнта(object? sender, EventArgs args)
        {
            ПоверненняТоварівВідКлієнта page = new ПоверненняТоварівВідКлієнта();
            Program.GeneralForm?.CreateNotebookPage("Повернення товарів від клієнта", () => { return page; });
            page.SetValue();
        }

        void АктВиконанихРобіт(object? sender, EventArgs args)
        {
            АктВиконанихРобіт page = new АктВиконанихРобіт();
            Program.GeneralForm?.CreateNotebookPage("Акт виконаних робіт", () => { return page; });
            page.SetValue();
        }

        void ВведенняЗалишків(object? sender, EventArgs args)
        {
            ВведенняЗалишків page = new ВведенняЗалишків();
            Program.GeneralForm?.CreateNotebookPage("Введення залишків", () => { return page; });
            page.SetValue();
        }

        void ВнутрішнєСпоживанняТоварів(object? sender, EventArgs args)
        {
            ВнутрішнєСпоживанняТоварів page = new ВнутрішнєСпоживанняТоварів();
            Program.GeneralForm?.CreateNotebookPage("Внутрішнє споживання товарів", () => { return page; });
            page.SetValue();
        }

        void РахунокФактура(object? sender, EventArgs args)
        {
            РахунокФактура page = new РахунокФактура();
            Program.GeneralForm?.CreateNotebookPage("Рахунок фактура", () => { return page; });
            page.SetValue();
        }

        void РозміщенняТоварівНаСкладі(object? sender, EventArgs args)
        {
            РозміщенняТоварівНаСкладі page = new РозміщенняТоварівНаСкладі();
            Program.GeneralForm?.CreateNotebookPage("Розміщення товарів на складі", () => { return page; });
            page.SetValue();
        }

        void ПереміщенняТоварівНаСкладі(object? sender, EventArgs args)
        {
            ПереміщенняТоварівНаСкладі page = new ПереміщенняТоварівНаСкладі();
            Program.GeneralForm?.CreateNotebookPage("Переміщення товарів на складі", () => { return page; });
            page.SetValue();
        }

        void ЗбіркаТоварівНаСкладі(object? sender, EventArgs args)
        {
            ЗбіркаТоварівНаСкладі page = new ЗбіркаТоварівНаСкладі();
            Program.GeneralForm?.CreateNotebookPage("Збірка товарів на складі", () => { return page; });
            page.SetValue();
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