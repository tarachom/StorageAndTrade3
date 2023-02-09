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

Звіти

*/

using Gtk;

namespace StorageAndTrade
{
    class PageReports : VBox
    {
        public PageReports() : base()
        {           
            //Список
            HBox hBoxList = new HBox(false, 0);

            VBox vLeft = new VBox(false, 0);
            hBoxList.PackStart(vLeft, false, false, 5);

            AddLink(vLeft, "Товари на складах", ТовариНаСкладах);
            AddLink(vLeft, "Вільні залишки", ВільніЗалишки);
            AddLink(vLeft, "Партії товарів", ПартіїТоварів);
            AddLink(vLeft, "Рух коштів", РухКоштів);
            AddLink(vLeft, "Розрахунки з контрагентами", РозрахункиЗКонтрагентами);
            AddLink(vLeft, "Розрахунки з клієнтами", РозрахункиЗКлієнтами);
            AddLink(vLeft, "Розрахунки з постачальниками", РозрахункиЗПостачальниками);
            AddLink(vLeft, "Замовлення клієнтів", ЗамовленняКлієнтів);
            AddLink(vLeft, "Замовлення постачальникам", ЗамовленняПостачальникам);
            AddLink(vLeft, "Закупівлі", Закупівлі);
            AddLink(vLeft, "Продажі", Продажі);
            AddLink(vLeft, "Товари в комірках на складах", ТовариВКоміркахНаСкладах);

            PackStart(hBoxList, false, false, 10);

            ShowAll();
        }

        void ВільніЗалишки(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Звіт - Вільні залишки", () =>
            {
                Звіт_ВільніЗалишки page = new Звіт_ВільніЗалишки();
                return page;
            });
        }

        void ЗамовленняКлієнтів(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Звіт - Замовлення клієнтів", () =>
            {
                Звіт_ЗамовленняКлієнтів page = new Звіт_ЗамовленняКлієнтів();
                return page;
            });
        }

        void ЗамовленняПостачальникам(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Звіт - Замовлення постачальникам", () =>
            {
                Звіт_ЗамовленняПостачальникам page = new Звіт_ЗамовленняПостачальникам();
                return page;
            });
        }

        void ПартіїТоварів(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Звіт - Партії товарів", () =>
            {
                Звіт_ПартіїТоварів page = new Звіт_ПартіїТоварів();
                return page;
            });
        }

        void РозрахункиЗКлієнтами(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Звіт - Розрахунки з клієнтами", () =>
            {
                Звіт_РозрахункиЗКлієнтами page = new Звіт_РозрахункиЗКлієнтами();
                return page;
            });
        }

        void РозрахункиЗКонтрагентами(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Звіт - Розрахунки з контрагентами", () =>
            {
                Звіт_РозрахункиЗКонтрагентами page = new Звіт_РозрахункиЗКонтрагентами();
                return page;
            });
        }

        void РозрахункиЗПостачальниками(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Звіт - Розрахунки з постачальниками", () =>
            {
                Звіт_РозрахункиЗПостачальниками page = new Звіт_РозрахункиЗПостачальниками();
                return page;
            });
        }

        void РухКоштів(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Звіт - Рух коштів", () =>
            {
                Звіт_РухКоштів page = new Звіт_РухКоштів();
                return page;
            });
        }

        void ТовариНаСкладах(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Звіт - Товари на складах", () =>
            {
                Звіт_ТовариНаСкладах page = new Звіт_ТовариНаСкладах();
                return page;
            });
        }

        void Закупівлі(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Звіт - Закупівлі", () =>
            {
                Звіт_Закупівлі page = new Звіт_Закупівлі();
                return page;
            });
        }

        void Продажі(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Звіт - Продажі", () =>
            {
                Звіт_Продажі page = new Звіт_Продажі();
                return page;
            });
        }

        void ТовариВКоміркахНаСкладах(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Звіт - Товари в комірках на складах", () =>
            {
                Звіт_ТовариВКоміркахНаСкладах page = new Звіт_ТовариВКоміркахНаСкладах();
                return page;
            });
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