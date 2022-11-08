using Gtk;

using AccountingSoftware;

namespace StorageAndTrade
{
    class FormStorageAndTrade : Window
    {
        readonly object loked = new Object();
        public ConfigurationParam? OpenConfigurationParam { get; set; }

        Notebook topNotebook;
        Statusbar statusBar;

        public FormStorageAndTrade() : base("Зберігання та Торгівля для України")
        {
            SetDefaultSize(1500, 900);
            SetPosition(WindowPosition.Center);
            SetDefaultIconFromFile("images/form.ico");

            DeleteEvent += delegate { Application.Quit(); };

            VBox vbox = new VBox();
            Add(vbox);

            HBox hbox = new HBox();
            vbox.PackStart(hbox, true, true, 0);

            CreateLeftMenu(hbox);

            topNotebook = new Notebook() { Scrollable = true, EnablePopup = true, BorderWidth = 0, ShowBorder = false };
            topNotebook.TabPos = PositionType.Top;

            CreateNotebookPage("Стартова", null);

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

            ScrolledWindow scrolLeftMenu = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = 210 };
            scrolLeftMenu.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrolLeftMenu.Add(vbox);

            CreateItemLeftMenu(vbox, "Документи", OnClick_Документи, "images/documents.png");
            CreateItemLeftMenu(vbox, "Довідники", OnClick_Довідники, "images/directory.png");
            CreateItemLeftMenu(vbox, "Журнали", OnClick_Журнали, "images/journal.png");
            CreateItemLeftMenu(vbox, "Налаштування", OnClick_Налаштування, "images/preferences.png");

            hbox.PackStart(scrolLeftMenu, false, false, 0);
        }

        void CreateItemLeftMenu(VBox vBox, string name, EventHandler ClikAction, string ico)
        {
            LinkButton lb = new LinkButton(" " + name)
            {
                Halign = Align.Start,
                Image = new Image(ico),
                AlwaysShowImage = true
            };

            lb.Image.Valign = Align.End;

            lb.Clicked += ClikAction;

            vBox.PackStart(lb, false, false, 0);
        }

        void OnClick_Журнали(object? sender, EventArgs args)
        {

        }

        void OnClick_Документи(object? sender, EventArgs args)
        {
            CreateNotebookPage("Документи", () =>
            {
                PageDocumentsAll page = new PageDocumentsAll();

                return page;
            });
        }

        void OnClick_Довідники(object? sender, EventArgs args)
        {
            CreateNotebookPage("Довідники", () =>
            {
                PageDirectoryAll page = new PageDirectoryAll();
                return page;
            });
        }

        void OnClick_Налаштування(object? sender, EventArgs args)
        {

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