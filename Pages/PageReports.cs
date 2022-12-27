using Gtk;

namespace StorageAndTrade
{
    class PageReports : VBox
    {
        public PageReports() : base()
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
            hBoxList.PackStart(vLeft, false, false, 5);

            AddLink(vLeft, "ТовариНаСкладах", ТовариНаСкладах);
            AddLink(vLeft, "ТовариНаСкладах2", ТовариНаСкладах2);
            AddLink(vLeft, "ВільніЗалишки", ВільніЗалишки);
            AddLink(vLeft, "ПартіїТоварів", ПартіїТоварів);
            AddLink(vLeft, "РухКоштів", РухКоштів);
            AddLink(vLeft, "РозрахункиЗКонтрагентами", РозрахункиЗКонтрагентами);
            AddLink(vLeft, "РозрахункиЗКлієнтами", РозрахункиЗКлієнтами);
            AddLink(vLeft, "РозрахункиЗПостачальниками", РозрахункиЗПостачальниками);
            AddLink(vLeft, "ЗамовленняКлієнтів", ЗамовленняКлієнтів);
            AddLink(vLeft, "ЗамовленняПостачальникам", ЗамовленняПостачальникам);

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

        void ТовариНаСкладах2(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Звіт - Товари на складах2", () =>
            {
                Звіт_ТовариНаСкладах2 page = new Звіт_ТовариНаСкладах2();
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