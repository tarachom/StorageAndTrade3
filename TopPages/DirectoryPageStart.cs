using Gtk;
using System;
using System.IO;

using AccountingSoftware;

namespace StorageAndTrade
{
    class DirectoryPageStart : VBox
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

        public DirectoryPageStart() : base()
        {
            HBox HFirst = new HBox(false, 0);

            VBox VFirst = new VBox(false, 0);
            AddLinkButton(VFirst, "Організації", "1");
            AddLinkButton(VFirst, "Номенклатура", "1");
            AddLinkButton(VFirst, "Характеристики номенклатури", "1");
            AddLinkButton(VFirst, "Контрагенти", "1");
            AddLinkButton(VFirst, "Склади", "1");
            AddLinkButton(VFirst, "Валюти", "1");
            AddLinkButton(VFirst, "Каси", "1");
            AddLinkButton(VFirst, "Користувачі", "1");
            AddLinkButton(VFirst, "Файли", "1");

            HFirst.PackStart(VFirst, false, false, 5);

            AddSeparator(HFirst);

            VBox VToo = new VBox(false, 0);
            AddLinkButton(VToo, "Види номенклатури", "2");
            AddLinkButton(VToo, "Банківські рахунки контрагентів", "2");
            AddLinkButton(VToo, "Банківські рахунки організацій", "2");
            AddLinkButton(VToo, "Види цін", "2");
            AddLinkButton(VToo, "Виробники", "2");
            AddLinkButton(VToo, "Договори контрагентів", "2");
            AddLinkButton(VToo, "Пакування одиниці виміру", "2");
            AddLinkButton(VToo, "Партії товарів", "2");
            AddLinkButton(VToo, "Серії номенклатури", "2");
            AddLinkButton(VToo, "Структура підприємства", "2");
            AddLinkButton(VToo, "Фізичні особи", "2");
            HFirst.PackStart(VToo, false, false, 5);

            PackStart(HFirst, false, false, 20);

            ShowAll();
        }
    }
}