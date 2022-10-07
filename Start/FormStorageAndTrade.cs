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
                        notebookPage.VBox = new FirstPage();
                        notebookPage.ScrolledWindow?.Add(notebookPage.VBox);
                        notebookPage.IsConstruct = true;
                        break;
                    }
            }
        }
    }

    void CreateTopNotebookPages(string[] names)
    {
        foreach (string name in names)
        {
            ScrolledWindow scroll = new ScrolledWindow();

            int numPage = TopNotebook.AppendPage(scroll, new Label { Text = name, Expand = true });

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