using Gtk;

namespace StorageAndTrade
{
    class PageJournals : VBox
    {
        public PageJournals() : base()
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

            AddLink(vLeft, "Повний", Повний);
            AddLink(vLeft, "Продажі", Продажі);
            AddLink(vLeft, "Закупки", Закупки);
            AddLink(vLeft, "Склади", Склади);
            AddLink(vLeft, "Фінанси", Фінанси);

            PackStart(hBoxList, false, false, 10);

            ShowAll();
        }

        void Повний(object? sender, EventArgs args)
        {

        }

        void Продажі(object? sender, EventArgs args)
        {

        }

        void Закупки(object? sender, EventArgs args)
        {

        }

        void Склади(object? sender, EventArgs args)
        {

        }

        void Фінанси(object? sender, EventArgs args)
        {

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