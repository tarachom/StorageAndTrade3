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
            SetDefaultSize(1000, 600);
            SetPosition(WindowPosition.Center);
            SetDefaultIconFromFile("form.ico");

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

            ScrolledWindow scrolLeftMenu = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = 250 };
            scrolLeftMenu.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrolLeftMenu.Add(vbox);

            CreateItemLeftMenu(vbox, "Продажі", OnClick_Продажі);
            CreateItemLeftMenu(vbox, "Закупки", OnClick_Закупки);
            CreateItemLeftMenu(vbox, "Документи", OnClick_Документи);
            CreateItemLeftMenu(vbox, "Довідники", OnClick_Довідники);
            CreateItemLeftMenu(vbox, "Налаштування", OnClick_Налаштування);

            hbox.PackStart(scrolLeftMenu, false, false, 0);
        }

        void CreateItemLeftMenu(VBox vBox, string name, EventHandler ClikAction)
        {
            HBox hBox = new HBox() { BorderWidth = 1 };
            vBox.PackStart(hBox, false, false, 5);

            Image imageItem = new Image("form.ico");
            hBox.PackStart(imageItem, false, false, 5);

            LinkButton lb = new LinkButton(name) { Halign = Align.Start };
            lb.Clicked += ClikAction;
            hBox.PackStart(lb, false, false, 0);
        }

        void OnClick_Продажі(object? sender, EventArgs args)
        {

        }

        void OnClick_Закупки(object? sender, EventArgs args)
        {

        }

        void OnClick_Документи(object? sender, EventArgs args)
        {
            CreateNotebookPage("Документи", () =>
            {
                PageDocumentsAll page = new PageDocumentsAll
                {
                    GeneralForm = this
                };

                return page;
            });
        }

        void OnClick_Довідники(object? sender, EventArgs args)
        {
            CreateNotebookPage("Довідники", () =>
            {
                PageDirectoryAll page = new PageDirectoryAll
                {
                    GeneralForm = this
                };

                return page;
            });
        }

        void OnClick_Налаштування(object? sender, EventArgs args)
        {

        }

        #endregion

        void CreatePack2(HBox hbox)
        {


            CreateNotebookPage("Оптимізація таблиць", () =>
            {
                Номенклатура номенклатура = new Номенклатура();
                номенклатура.LoadRecords();
                return номенклатура;
            });


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