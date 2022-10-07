using Gtk;
using System;
using System.IO;

class FormStorageAndTrade : Window
{
    public ConfigurationParam? OpenConfigurationParam { get; set; }

    #region Notebook

    private Notebook TopNotebook;
    private readonly string[] MasPageNames = new string[] { "Головна", "Довідники", "Журнали", "Документи", "Звіти", "Сервіс" };
    private Dictionary<int, NotebookPage> TopNotebookPages;
    class NotebookPage
    {
        public NotebookPage()
        {
            NamePage = "";
        }

        public int NumPage { get; set; }
        public string NamePage { get; set; }
        public ScrolledWindow? ScrolledWindow { get; set; }
        public VBox? VBox { get; set; }
        public bool IsConstruct { get; set; }

        public void AddVBox(VBox vBox)
        {
            VBox = vBox;
            ScrolledWindow?.Add(vBox);
            IsConstruct = true;
        }
    }

    #endregion

    public FormStorageAndTrade() : base("Зберігання та Торгівля для України")
    {
        SetDefaultSize(800, 600);
        SetPosition(WindowPosition.Center);
        SetDefaultIconFromFile("form.ico");

        DeleteEvent += delegate { Application.Quit(); };

        TopNotebook = new Notebook();
        TopNotebook.TabPos = PositionType.Top;
        TopNotebookPages = new Dictionary<int, NotebookPage>();

        CreateTopNotebookPages(MasPageNames);

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
        {
            switch (TopNotebook.CurrentPage)
            {
                case 0:
                    {
                        notebookPage.AddVBox(new FirstPage());
                        break;
                    }
                case 1:
                    {
                        notebookPage.AddVBox(new DirectoryPage());
                        break;
                    }
                case 2:
                    {
                        notebookPage.AddVBox(new JournalPage());
                        break;
                    }
                case 3:
                    {
                        Валюти валюти = new Валюти();
                        //валюти.LoadRecords();
                        notebookPage.AddVBox(валюти);
                        break;
                    }
            }
        }
    }

    void CreateTopNotebookPages(string[] names)
    {
        foreach (string name in names)
        {
            ScrolledWindow scroll = new ScrolledWindow() { ShadowType = ShadowType.In };
            scroll.SetPolicy(PolicyType.Never, PolicyType.Automatic);

            int numPage = TopNotebook.AppendPage(scroll, new Label { Text = name, Expand = false });

            TopNotebookPages.Add(numPage,
                new NotebookPage
                {
                    NumPage = numPage,
                    NamePage = name,
                    ScrolledWindow = scroll
                });
        }
    }
}


// Console.WriteLine(_notebook.Children.Length);

// foreach (Widget page in _notebook.Children)
// {
//     Console.WriteLine(page.Name);
//     Console.WriteLine(_notebook.GetTabLabelText(page));

//     //delete
//     //_notebook.DetachTab(page);
// } 