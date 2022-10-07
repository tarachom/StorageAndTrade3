using Gtk;
using System;
using System.IO;

class FormStorageAndTrade : Window
{
    public ConfigurationParam? OpenConfigurationParam { get; set; }

    private Notebook topNotebook;
    private Dictionary<int, VBox> topNotebookPageBox;

    public FormStorageAndTrade() : base("Зберігання та Торгівля для України")
    {
        SetDefaultSize(800, 600);
        SetPosition(WindowPosition.Center);
        SetDefaultIconFromFile("form.ico");

        DeleteEvent += delegate { Application.Quit(); };

        topNotebook = new Notebook();
        topNotebook.TabPos = PositionType.Top;

        topNotebookPageBox = new Dictionary<int, VBox>();

        CreateTopNotebookPages(new string[] { "Головна", "Довідники", "Журнали", "Документи", "Звіти", "Сервіс" });

        Add(topNotebook);

        //Maximize();
        ShowAll();



        topNotebook.SwitchPage += OnTopNotebookSelectPage;
    }

    void OnTopNotebookSelectPage(object? sender, SwitchPageArgs args)
    {
        Console.WriteLine(topNotebook.CurrentPage);
    }

    void CreateTopNotebookPages(string[] names)
    {
        foreach (string name in names)
        {
            ScrolledWindow scroll = new ScrolledWindow();
            VBox vbox = new VBox(false, 2);
            vbox.Halign = Align.Start;
            
            scroll.Add(vbox);

            int pageNum = topNotebook.AppendPage(scroll, new Label { Text = name, Expand = true });
            topNotebookPageBox.Add(pageNum, vbox);

            HBox hBoxButton = new HBox();
            Label l = new Label(name);
            hBoxButton.Add(l);

            vbox.PackStart(hBoxButton, false, false, 0);
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