using Gtk;
using System;
using System.IO;

using AccountingSoftware;

namespace StorageAndTrade
{
    class FormStorageAndTrade : Window
    {
        public ConfigurationParam? OpenConfigurationParam { get; set; }

        #region Notebook

        private Notebook TopNotebook;
        private Dictionary<int, NotebookPage> TopNotebookPages;
        private Dictionary<int, NameValue<System.Action<NotebookPage>>> PageAndActionDictionary;

        #endregion

        public FormStorageAndTrade() : base("Зберігання та Торгівля для України")
        {
            SetDefaultSize(800, 600);
            SetPosition(WindowPosition.Center);
            SetDefaultIconFromFile("form.ico");

            DeleteEvent += delegate { Application.Quit(); };

            TopNotebook = new Notebook() { BorderWidth = 0, ShowBorder = false };
            TopNotebook.TabPos = PositionType.Left;

            TopNotebookPages = new Dictionary<int, NotebookPage>();
            PageAndActionDictionary = new Dictionary<int, NameValue<Action<NotebookPage>>>();

            int counter = 0;

            PageAndActionDictionary.Add(counter, new NameValue<Action<NotebookPage>>("Стартова", (NotebookPage page) =>
                { page.AddVBox(new FirstPage()); }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Документи", (NotebookPage page) =>
                { page.AddVBox(new DocumentPage()); }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Журнали", (NotebookPage page) =>
                { page.AddVBox(new JournalPage()); }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Звіти", (NotebookPage page) =>
                { }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Довідники", (NotebookPage page) =>
                { page.AddVBox(new DirectoryPage()); }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Сервіс", (NotebookPage page) =>
                { }
            ));

            CreateTopNotebookPages();

            Add(TopNotebook);

            //Maximize();
            ShowAll();

            TopNotebook.SwitchPage += OnTopNotebookSelectPage;
            OnTopNotebookSelectPage(null, new SwitchPageArgs());
        }

        void OnTopNotebookSelectPage(object? sender, SwitchPageArgs args)
        {
            NotebookPage notebookPage = TopNotebookPages[TopNotebook.CurrentPage];
            if (!notebookPage.IsConstruct)
                PageAndActionDictionary[TopNotebook.CurrentPage]?.Value?.Invoke(notebookPage);
        }

        void CreateTopNotebookPages()
        {
            foreach (NameValue<Action<NotebookPage>> page in PageAndActionDictionary.Values)
            {
                ScrolledWindow scroll = new ScrolledWindow() { ShadowType = ShadowType.In };
                scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

                int numPage = TopNotebook.AppendPage(scroll, new Label { Text = page.Name, Expand = false, Halign = Align.End });

                TopNotebookPages.Add(numPage,
                    new NotebookPage
                    {
                        NumPage = numPage,
                        NamePage = page.Name,
                        ScrolledWindow = scroll
                    });
            }
        }
    }
}
