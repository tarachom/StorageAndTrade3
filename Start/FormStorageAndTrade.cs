using Gtk;

using AccountingSoftware;

namespace StorageAndTrade
{
    class FormStorageAndTrade : Window
    {
        public ConfigurationParam? OpenConfigurationParam { get; set; }

        Notebook topNotebook;
        Statusbar statusBar;

        public FormStorageAndTrade() : base("Зберігання та Торгівля для України")
        {
            SetDefaultSize(1500, 900);
            SetPosition(WindowPosition.Center);
            SetDefaultIconFromFile("images/form.ico");

            DeleteEvent += delegate { Program.Quit(); };

            VBox vbox = new VBox();
            Add(vbox);

            HBox hbox = new HBox();
            vbox.PackStart(hbox, true, true, 0);

            CreateLeftMenu(hbox);

            topNotebook = new Notebook() { Scrollable = true, EnablePopup = true, BorderWidth = 0, ShowBorder = false };
            topNotebook.TabPos = PositionType.Top;

            CreateNotebookPage("Стартова", () =>
            {
                PageHome page = new PageHome();
                page.StartBackgroundTask();
                return page;
            });

            hbox.PackStart(topNotebook, true, true, 0);

            statusBar = new Statusbar();
            vbox.PackStart(statusBar, false, false, 0);

            ShowAll();
        }

        #region LeftMenu

        void CreateLeftMenu(HBox hbox)
        {
            VBox vbox = new VBox();
            vbox.BorderWidth = 15;

            ScrolledWindow scrolLeftMenu = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = 200 };
            scrolLeftMenu.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrolLeftMenu.Add(vbox);

            CreateItemLeftMenu(vbox, "Документи", Документи, "images/documents.png");
            CreateItemLeftMenu(vbox, "Журнали", Журнали, "images/journal.png");
            CreateItemLeftMenu(vbox, "Звіти", Звіти, "images/report.png");
            CreateItemLeftMenu(vbox, "Довідники", Довідники, "images/directory.png");
            CreateItemLeftMenu(vbox, "Налаштування", Налаштування, "images/preferences.png");
            CreateItemLeftMenu(vbox, "Сервіс", Сервіс, "images/service.png");

            hbox.PackStart(scrolLeftMenu, false, false, 0);
        }

        void CreateItemLeftMenu(VBox vBox, string name, EventHandler ClikAction, string image)
        {
            LinkButton lb = new LinkButton(name, name)
            {
                Halign = Align.Start,
                Image = new Image(image),
                AlwaysShowImage = true
            };

            lb.Image.Valign = Align.End;

            lb.Clicked += ClikAction;

            vBox.PackStart(lb, false, false, 10);
        }

        void Документи(object? sender, EventArgs args)
        {
            CreateNotebookPage("Документи", () =>
            {
                PageDocuments page = new PageDocuments();
                return page;
            });
        }

        void Журнали(object? sender, EventArgs args)
        {
            CreateNotebookPage("Журнали", () =>
            {
                PageJournals page = new PageJournals();
                return page;
            });
        }

        void Звіти(object? sender, EventArgs args)
        {
            CreateNotebookPage("Звіти", () =>
            {
                PageReports page = new PageReports();
                return page;
            });
        }

        void Довідники(object? sender, EventArgs args)
        {
            CreateNotebookPage("Довідники", () =>
            {
                PageDirectory page = new PageDirectory();
                return page;
            });
        }

        void Налаштування(object? sender, EventArgs args)
        {
            CreateNotebookPage("Налаштування", () =>
            {
                PageSettings page = new PageSettings();
                page.SetValue();
                return page;
            });
        }

        void Сервіс(object? sender, EventArgs args)
        {
            CreateNotebookPage("Сервіс", () =>
            {
                PageService page = new PageService();
                return page;
            });
        }

        #endregion

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