using Gtk;
using System;
using System.IO;

using AccountingSoftware;

namespace StorageAndTrade
{
    class DocumentPageStart : VBox
    {

        public System.Action<int>? OpenPageCallBack { get; set; }

        void OnClick(object? sender, EventArgs args)
        {
            if (sender != null)
            {
                LinkButton lb = (LinkButton)sender;

                if (OpenPageCallBack != null)
                    OpenPageCallBack.Invoke(int.Parse(lb.Name));
            }
        }

        void AddCaption(VBox vbox, string name)
        {
            Label caption = new Label(name);
            vbox.PackStart(caption, false, false, 10);
        }

        void AddSeparator(HBox hbox)
        {
            Separator separator = new Separator(Orientation.Horizontal);
            hbox.PackStart(separator, false, false, 5);
        }

        void AddLinkButton(VBox vbox, string uri, string name)
        {
            LinkButton lb = new LinkButton(uri) { Name = name, Halign = Align.Start };
            vbox.PackStart(lb, false, false, 0);
            lb.Clicked += OnClick;
        }

        public DocumentPageStart() : base()
        {
            /* Перший блок */

            HBox HFirst = new HBox(false, 0);

            VBox VFirst = new VBox(false, 0);
            AddCaption(VFirst, "Продажі");
            AddLinkButton(VFirst, "Замовлення клієнтів", "1");
            AddLinkButton(VFirst, "Рахунок фактура", "1");
            AddLinkButton(VFirst, "Акт виконаних робіт", "1");
            AddLinkButton(VFirst, "Реалізація товарі та послуг", "1");
            AddLinkButton(VFirst, "Повернення від клієнта", "1");
            HFirst.PackStart(VFirst, false, false, 5);

            AddSeparator(HFirst);

            VBox VToo = new VBox(false, 0);
            AddCaption(VToo, "Покупки");
            AddLinkButton(VToo, "Замовлення постачальнику", "2");
            AddLinkButton(VToo, "Поступлення товарів та послуг", "2");
            AddLinkButton(VToo, "Повернення постачальнику", "2");
            HFirst.PackStart(VToo, false, false, 5);

            PackStart(HFirst, false, false, 20);

            /* Другий блок */

            HBox HToo = new HBox(false, 0);

            VBox VThree = new VBox(false, 0);
            AddCaption(VThree, "Склад");
            AddLinkButton(VThree, "Переміщення товарів", "3");
            AddLinkButton(VThree, "Внутрішнє споживання товарів", "3");
            AddLinkButton(VThree, "Псування товарі", "3");
            AddLinkButton(VThree, "Введення залишків", "3");
            HToo.PackStart(VThree, false, false, 5);

            AddSeparator(HToo);

            VBox VFour = new VBox(false, 0);
            AddCaption(VFour, "Каса");
            AddLinkButton(VFour, "Прихідний касовий ордер", "4");
            AddLinkButton(VFour, "Розхідний касовий ордер", "4");
            HToo.PackStart(VFour, false, false, 5);

            PackStart(HToo, false, false, 20);

            ShowAll();
        }
    }
}