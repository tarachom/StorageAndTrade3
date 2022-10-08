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

        public DocumentPageStart() : base()
        {
            /* Перший блок */

            HBox hFirstBlock = new HBox(false, 0);

            VBox VFirst = new VBox(false, 0);

            Label caption11 = new Label("Продажі");

            LinkButton lb11 = new LinkButton("Замовлення клієнтів") { Name = "1", Halign = Align.Start };
            lb11.Clicked += OnClick;
            LinkButton lb12 = new LinkButton("Рахунок фактура") { Name = "2", Halign = Align.Start };
            lb12.Clicked += OnClick;
            LinkButton lb13 = new LinkButton("Акт виконаних робіт") { Name = "3", Halign = Align.Start };
            lb13.Clicked += OnClick;
            LinkButton lb14 = new LinkButton("Реалізація товарі та послуг") { Name = "1", Halign = Align.Start };
            lb14.Clicked += OnClick;
            LinkButton lb15 = new LinkButton("Повернення від клієнта") { Name = "1", Halign = Align.Start };
            lb15.Clicked += OnClick;

            VFirst.PackStart(caption11, false, false, 10);
            VFirst.PackStart(lb11, false, false, 0);
            VFirst.PackStart(lb12, false, false, 0);
            VFirst.PackStart(lb13, false, false, 0);
            VFirst.PackStart(lb14, false, false, 0);
            VFirst.PackStart(lb15, false, false, 0);

            hFirstBlock.PackStart(VFirst, false, false, 5);

            Separator separator1 = new Separator(Orientation.Horizontal);
            hFirstBlock.PackStart(separator1, false, false, 5);

            VBox VToo = new VBox(false, 0);

            Label caption21 = new Label("Покупки");
            LinkButton lb21 = new LinkButton("Замовлення постачальнику") { Halign = Align.Start };
            LinkButton lb22 = new LinkButton("Поступлення товарів та послуг") { Halign = Align.Start };
            LinkButton lb23 = new LinkButton("Повернення постачальнику") { Halign = Align.Start };

            VToo.PackStart(caption21, false, false, 10);
            VToo.PackStart(lb21, false, false, 0);
            VToo.PackStart(lb22, false, false, 0);
            VToo.PackStart(lb23, false, false, 0);

            hFirstBlock.PackStart(VToo, false, false, 5);

            PackStart(hFirstBlock, false, false, 20);


            /* Другий блок */


            HBox hTooBlock = new HBox(false, 0);

            VBox VThree = new VBox(false, 0);

            Label caption31 = new Label("Склад");
            LinkButton lb31 = new LinkButton("Переміщення товарів") { Halign = Align.Start };
            LinkButton lb32 = new LinkButton("Внутрішнє споживання товарів") { Halign = Align.Start };
            LinkButton lb33 = new LinkButton("Псування товарі") { Halign = Align.Start };

            VThree.PackStart(caption31, false, false, 10);
            VThree.PackStart(lb31, false, false, 0);
            VThree.PackStart(lb32, false, false, 0);
            VThree.PackStart(lb33, false, false, 0);

            hTooBlock.PackStart(VThree, false, false, 5);

            Separator separator3 = new Separator(Orientation.Horizontal);
            hTooBlock.PackStart(separator3, false, false, 5);

            VBox VTooFirst = new VBox(false, 0);

            Label caption41 = new Label("Каса");
            LinkButton lb41 = new LinkButton("Прихідний касовий ордер") { Halign = Align.Start };
            LinkButton lb42 = new LinkButton("Розхідний касовий ордер") { Halign = Align.Start };

            VTooFirst.PackStart(caption41, false, false, 10);
            VTooFirst.PackStart(lb41, false, false, 0);
            VTooFirst.PackStart(lb42, false, false, 0);

            hTooBlock.PackStart(VTooFirst, false, false, 5);

            PackStart(hTooBlock, false, false, 20);

            ShowAll();
        }

    }
}