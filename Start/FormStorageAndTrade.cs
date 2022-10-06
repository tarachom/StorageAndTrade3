using Gtk;
using System;
using System.IO;

class FormStorageAndTrade : Window
{
    public ConfigurationParam? OpenConfigurationParam { get; set; }

    public FormStorageAndTrade() : base("Зберігання та Торгівля для України")
    {
        SetDefaultSize(800, 600);
        SetPosition(WindowPosition.Center);
        SetDefaultIconFromFile("configuration.png");

        DeleteEvent += delegate { Application.Quit(); };

        ShowAll();
    }
}