using Gtk;

using AccountingSoftware;

namespace StorageAndTrade
{
    class FormStorageAndTrade : Window
    {
        readonly object loked = new Object();
        public ConfigurationParam? OpenConfigurationParam { get; set; }

        HPaned hPaned;
        Notebook? topNotebook;
        Statusbar statusBar;

        public FormStorageAndTrade() : base("Зберігання та Торгівля для України")
        {
            SetDefaultSize(1000, 600);
            SetPosition(WindowPosition.Center);
            SetDefaultIconFromFile("form.ico");

            DeleteEvent += delegate { Application.Quit(); };

            VBox vbox = new VBox();
            Add(vbox);

            HBox hbox = new HBox();
            vbox.PackStart(hbox, true, true, 0);

            CreatePack1(hbox);
            CreatePack2(hbox);

            statusBar = new Statusbar();
            vbox.PackStart(statusBar, false, false, 0);

            ShowAll();
        }

        void CreatePack1(HBox hbox)
        {
            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = 250 };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            VBox vbox = new VBox();
            scrollTree.Add(vbox);

            CreateItemPack1(vbox, "Продажі");
            CreateItemPack1(vbox, "Закупки");
            CreateItemPack1(vbox, "Налаштування");

            hbox.PackStart(scrollTree, false, false, 0);
        }

        void CreateItemPack1(VBox vBox, string name)
        {
            HBox hBox = new HBox() { BorderWidth = 1 };
            vBox.PackStart(hBox, false, false, 5);

            Image imageItem = new Image("form.ico");
            hBox.PackStart(imageItem, false, false, 5);

            LinkButton lb = new LinkButton(name) { Halign = Align.Start };
            hBox.PackStart(lb, false, false, 0);
        }

        void CreatePack2(HBox hbox)
        {
            topNotebook = new Notebook() { Scrollable = true, EnablePopup = true, BorderWidth = 0, ShowBorder = false };
            topNotebook.TabPos = PositionType.Top;



            CreateNotebookPage("Оптимізація таблиць", () =>
                       {
                           Валюти валюти = new Валюти();
                           валюти.LoadRecords();
                           return валюти;
                       });

            hbox.PackStart(topNotebook, true, true, 0);


        }

        #region Notebook Page

        public void CloseCurrentPageNotebook()
        {
            topNotebook.RemovePage(topNotebook.CurrentPage);
        }

        public void RenameCurrentPageNotebook(string name)
        {
            topNotebook.SetTabLabelText(topNotebook.CurrentPageWidget, name);
        }

        public void CreateNotebookPage(string tabName, System.Func<Widget>? pageWidget)
        {
            ScrolledWindow scroll = new ScrolledWindow() { ShadowType = ShadowType.In };
            scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            int numPage = topNotebook.AppendPage(scroll, new Label { Text = tabName, Expand = false, Halign = Align.Start });

            if (pageWidget != null)
                scroll.Add((Widget)pageWidget.Invoke());

            topNotebook.ShowAll();

            topNotebook.CurrentPage = numPage;
        }

        #endregion
    }
}