using Gtk;
using System;
using System.IO;

class FormStorageAndTrade : Window
{
    public ConfigurationParam? OpenConfigurationParam { get; set; }

    private Notebook _notebook;

    public FormStorageAndTrade() : base("Зберігання та Торгівля для України")
    {
        SetDefaultSize(800, 600);
        SetPosition(WindowPosition.Center);
        SetDefaultIconFromFile("form.ico");

        DeleteEvent += delegate { Application.Quit(); };

        _notebook = new Notebook();
        _notebook.TabPos = PositionType.Top;

        Crate("Журнал");
        Crate("Поступлення товарів та послуг");
        Crate("Реалізаці товарів та послуг");
        Crate("Довідник Номенклатура");
        Crate("Валюти");
        Crate("Сервіс");
        Crate("Константи");

        Add(_notebook);

        //Maximize();
        ShowAll();

        Console.WriteLine(_notebook.Children.Length);

        foreach (Widget page in _notebook.Children)
        {
            Console.WriteLine(page.Name);
            Console.WriteLine(_notebook.GetTabLabelText(page));

            //delete
            //_notebook.DetachTab(page);
        }
    }

    void Crate(string name)
    {
        var scroll1 = new ScrolledWindow();
        var vpanned = new VPaned();
        vpanned.Position = 300;
        scroll1.Child = vpanned;

        _notebook.AppendPage(scroll1, new Label { Text = name, Expand = true });


    }
}